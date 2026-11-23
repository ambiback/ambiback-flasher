<#
.SYNOPSIS
    Uploads a firmware image to an update server with OpenSSH sftp.

.DESCRIPTION
    Only a server is required. Authentication uses your normal OpenSSH setup:
    ssh-agent, default keys in ~/.ssh and ~/.ssh/config. Settings come from
    environment variables or a .env file. Environment variables take priority.

    AMBIBACK_UPLOAD_HOST  Server name, IP address or ~/.ssh/config alias (required)
    AMBIBACK_UPLOAD_USER  SSH user (optional)
    AMBIBACK_UPLOAD_PORT  SSH port (optional)
    AMBIBACK_UPLOAD_PATH  Remote base folder (optional, default: firmware/esp32)
    AMBIBACK_UPLOAD_KEY   Private key file (optional)

    The image is stored as <path>/<device>/<version>/update.bin.

.EXAMPLE
    .\upload-firmware.ps1 -Device m5alit -Version 0.5.231 -SourceFile .\firmware\m5alit\0.5.231\AmbiBack.Controller.ino.bin
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)][string]$Device,
    [Parameter(Mandatory = $true)][string]$Version,
    [Parameter(Mandatory = $true)][string]$SourceFile,
    [string]$EnvFile,
    [switch]$DryRun
)

Set-StrictMode -Version 2.0
$ErrorActionPreference = 'Stop'

function Read-EnvFile([string]$Path) {
    $values = @{}
    if (-not $Path -or -not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        return $values
    }
    foreach ($line in [IO.File]::ReadAllLines($Path)) {
        $text = $line.Trim()
        if ($text -match '^(?:export\s+)?(AMBIBACK_UPLOAD_[A-Z]+)\s*=\s*(.*)$') {
            $name = $Matches[1]
            $value = $Matches[2].Trim()
            if ($value.Length -ge 2 -and (($value[0] -eq '"' -and $value[-1] -eq '"') -or ($value[0] -eq "'" -and $value[-1] -eq "'"))) {
                $value = $value.Substring(1, $value.Length - 2)
            }
            $values[$name] = $value
        }
    }
    return $values
}

function Get-Setting([hashtable]$FileValues, [string]$Name, [string]$Default = '') {
    $fromEnvironment = [Environment]::GetEnvironmentVariable($Name)
    if ($fromEnvironment) { return $fromEnvironment }
    if ($FileValues.ContainsKey($Name) -and $FileValues[$Name]) { return $FileValues[$Name] }
    return $Default
}

function Find-Sftp {
    # A 32-bit host process has to use Sysnative to reach the Windows OpenSSH client.
    foreach ($folder in @('Sysnative', 'System32')) {
        $candidate = Join-Path $env:SystemRoot "$folder\OpenSSH\sftp.exe"
        if (Test-Path -LiteralPath $candidate) { return $candidate }
    }
    $command = Get-Command sftp -ErrorAction SilentlyContinue
    if ($command) { return $command.Source }
    throw 'sftp was not found. Install the Windows feature "OpenSSH Client".'
}

function ConvertTo-SftpArgument([string]$Value) {
    return '"' + $Value.Replace('\', '/') + '"'
}

$workFolder = $null
try {
    if ($Device -notmatch '^[A-Za-z0-9_-]+$') { throw "Invalid device name: $Device" }
    if ($Version -notmatch '^\d+\.\d+\.\d+$') { throw "Invalid version, expected for example 0.5.231: $Version" }
    if (-not (Test-Path -LiteralPath $SourceFile -PathType Leaf)) { throw "Firmware file not found: $SourceFile" }

    $fileValues = Read-EnvFile $EnvFile
    $serverName = Get-Setting $fileValues 'AMBIBACK_UPLOAD_HOST'
    $userName = Get-Setting $fileValues 'AMBIBACK_UPLOAD_USER'
    $port = Get-Setting $fileValues 'AMBIBACK_UPLOAD_PORT'
    $remoteBase = (Get-Setting $fileValues 'AMBIBACK_UPLOAD_PATH' 'firmware/esp32').TrimEnd('/')
    $keyFile = Get-Setting $fileValues 'AMBIBACK_UPLOAD_KEY'

    if (-not $serverName) { throw 'No server set. Set AMBIBACK_UPLOAD_HOST in the environment or in a .env file.' }
    if ($serverName -notmatch '^(?:[A-Za-z0-9][A-Za-z0-9._-]*|\[[0-9A-Fa-f:]+\])$') { throw 'AMBIBACK_UPLOAD_HOST is invalid.' }
    if ($userName -and $userName -notmatch '^[A-Za-z0-9_][A-Za-z0-9._-]*$') { throw 'AMBIBACK_UPLOAD_USER is invalid.' }
    if ($port -and ($port -notmatch '^\d{1,5}$' -or [int]$port -lt 1 -or [int]$port -gt 65535)) { throw 'AMBIBACK_UPLOAD_PORT is invalid.' }
    if (-not $remoteBase -or $remoteBase -notmatch '^[A-Za-z0-9._~/-]+$' -or $remoteBase -match '(^|/)\.\.(/|$)') { throw 'AMBIBACK_UPLOAD_PATH is invalid.' }

    $sftpArguments = @('-o', 'ConnectTimeout=15')
    if ($port) { $sftpArguments += @('-P', $port) }
    if ($keyFile) {
        if ($keyFile.StartsWith('~')) { $keyFile = Join-Path $HOME $keyFile.Substring(1).TrimStart('/', '\') }
        if (-not (Test-Path -LiteralPath $keyFile -PathType Leaf)) { throw 'The file in AMBIBACK_UPLOAD_KEY was not found.' }
        $sftpArguments += @('-i', $keyFile, '-o', 'IdentitiesOnly=yes')
    }
    $target = if ($userName) { "$userName@$serverName" } else { $serverName }

    $workFolder = Join-Path ([IO.Path]::GetTempPath()) ('ambiback-upload-' + [Guid]::NewGuid().ToString('N'))
    New-Item -ItemType Directory -Path $workFolder | Out-Null
    $localImage = Join-Path $workFolder 'update.bin'
    Copy-Item -LiteralPath $SourceFile -Destination $localImage
    $batchFile = Join-Path $workFolder 'upload.sftp'

    # The leading "-" lets sftp continue when a folder or an old image does not exist.
    $batch = New-Object System.Collections.Generic.List[string]
    $current = if ($remoteBase.StartsWith('/')) { '' } else { $null }
    foreach ($part in (($remoteBase -split '/') + @($Device, $Version) | Where-Object { $_ })) {
        $current = if ($null -eq $current) { $part } else { "$current/$part" }
        if ($part -ne '~') { $batch.Add('-mkdir ' + (ConvertTo-SftpArgument $current)) }
    }
    $remoteFile = "$remoteBase/$Device/$Version/update.bin"
    $batch.Add('put ' + (ConvertTo-SftpArgument $localImage) + ' ' + (ConvertTo-SftpArgument "$remoteFile.uploading"))
    $batch.Add('-rm ' + (ConvertTo-SftpArgument $remoteFile))
    $batch.Add('rename ' + (ConvertTo-SftpArgument "$remoteFile.uploading") + ' ' + (ConvertTo-SftpArgument $remoteFile))
    [IO.File]::WriteAllLines($batchFile, $batch, (New-Object Text.UTF8Encoding($false)))

    Write-Output "Uploading $Device $Version to ${target}:$remoteFile"
    if ($DryRun) {
        Write-Output '--- sftp batch (dry run, nothing was sent) ---'
        $batch | ForEach-Object { Write-Output $_ }
        exit 0
    }

    $sftp = Find-Sftp
    $ErrorActionPreference = 'Continue'
    & $sftp @sftpArguments -b $batchFile $target
    $exitCode = $LASTEXITCODE
    $ErrorActionPreference = 'Stop'
    if ($exitCode -ne 0) {
        throw "sftp failed with exit code $exitCode. Check the server and your SSH key. Run 'ssh $target' once to confirm the host key."
    }
    Write-Output 'Upload finished.'
    exit 0
}
catch {
    [Console]::Error.WriteLine("Upload failed: " + $_.Exception.Message)
    exit 1
}
finally {
    if ($workFolder -and (Test-Path -LiteralPath $workFolder)) {
        Remove-Item -LiteralPath $workFolder -Recurse -Force
    }
}
