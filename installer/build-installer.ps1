<#
.SYNOPSIS
    Builds the AmbiBack USB Flasher and creates the setup and the portable ZIP.

.DESCRIPTION
    Used by .github/workflows/ci.yml, .github/workflows/release.yml and for local builds. Steps:
      1. Release build with warnings as errors; the version is set in Properties\AssemblyInfo.cs
         for the build only, the file is restored afterwards
      2. Staging folder without .pdb/.xml files, plus LICENSE, THIRD_PARTY_NOTICES.md,
         README.md, README.de.md and .env.example
      3. Optional Authenticode signing (PFX, password in the environment variable SIGN_PFX_PASSWORD)
      4. Portable ZIP and Inno Setup installer in <OutRoot>\dist

.EXAMPLE
    .\installer\build-installer.ps1
    Builds with the version from AssemblyInfo.cs into %TEMP%\ambiback-flasher-build.

.EXAMPLE
    .\installer\build-installer.ps1 -Version 1.1.0 -BuildNumber 17
#>
[CmdletBinding()]
param(
    # Version from the tag without "v", e.g. 1.2.3 or 1.2.3-beta1. Default: from AssemblyInfo.cs
    [string]$Version,
    # Fourth part of the assembly version (GitHub run number)
    [int]$BuildNumber = 0,
    # Build root; bin, obj, staging and dist are created below it
    [string]$OutRoot = (Join-Path ([IO.Path]::GetTempPath()) 'ambiback-flasher-build'),
    [string]$MSBuild,
    [string]$Iscc,
    # PFX certificate for signing; signing is skipped when empty
    [string]$SignPfx,
    [string]$TimestampUrl = 'http://timestamp.digicert.com'
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$repo = Split-Path -Parent $PSScriptRoot
$assemblyInfo = Join-Path $repo 'Properties\AssemblyInfo.cs'
$exeName = 'AmbiBack USB Flasher.exe'
$baseName = 'AmbiBack-USB-Flasher'

function Find-Tool([string]$given, [string]$name, [string[]]$candidates) {
    if ($given) { if (Test-Path $given) { return $given } throw "$name not found: $given" }
    $cmd = Get-Command $name -ErrorAction SilentlyContinue
    if ($cmd) { return $cmd.Source }
    foreach ($c in $candidates) { if ($c -and (Test-Path $c)) { return $c } }
    throw "$name not found. Pass it as a parameter."
}

function Invoke-Checked([string]$exe, [string[]]$arguments) {
    & $exe @arguments
    if ($LASTEXITCODE -ne 0) { throw "$(Split-Path -Leaf $exe) failed with exit code $LASTEXITCODE." }
}

# --- Tools
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$vsMSBuild = $null
if (-not $MSBuild -and (Test-Path $vswhere)) {
    $vsMSBuild = & $vswhere -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
}
$MSBuild = Find-Tool $MSBuild 'MSBuild.exe' @($vsMSBuild)
$Iscc = Find-Tool $Iscc 'ISCC.exe' @(
    (Join-Path $env:LOCALAPPDATA 'Programs\Inno Setup 6\ISCC.exe'),
    (Join-Path ${env:ProgramFiles(x86)} 'Inno Setup 6\ISCC.exe'),
    (Join-Path $env:ProgramFiles 'Inno Setup 6\ISCC.exe'))

# --- Version
$originalInfo = [IO.File]::ReadAllBytes($assemblyInfo)
$infoText = [IO.File]::ReadAllText($assemblyInfo)
if (-not $Version) {
    if ($infoText -notmatch '\[assembly: AssemblyVersion\("(\d+\.\d+\.\d+)') { throw 'AssemblyVersion not found in AssemblyInfo.cs.' }
    $Version = $Matches[1]
}
if ($Version -notmatch '^(\d+)\.(\d+)\.(\d+)(-[0-9A-Za-z.-]+)?$') {
    throw "Version '$Version' must look like 1.2.3 or 1.2.3-beta1."
}
if ($BuildNumber -lt 0 -or $BuildNumber -gt 65534) { throw "BuildNumber $BuildNumber is out of range (0-65534)." }
$numeric = '{0}.{1}.{2}' -f $Matches[1], $Matches[2], $Matches[3]
$fileVersion = "$numeric.$BuildNumber"
Write-Host "Version $Version, assembly version $fileVersion"

$bin = Join-Path $OutRoot 'bin\'
$obj = Join-Path $OutRoot 'obj\'
$staging = Join-Path $OutRoot 'staging'
$dist = Join-Path $OutRoot 'dist'
foreach ($dir in @($bin, $obj, $staging, $dist)) {
    if (Test-Path $dir) { Remove-Item -Recurse -Force $dir }
    New-Item -ItemType Directory -Force $dir | Out-Null
}

# --- Build (only the attribute lines are replaced, not the commented example above them)
$patched = $infoText -replace '(?m)^\[assembly: AssemblyVersion\("[^"]*"\)\]', "[assembly: AssemblyVersion(""$fileVersion"")]" `
                     -replace '(?m)^\[assembly: AssemblyFileVersion\("[^"]*"\)\]', "[assembly: AssemblyFileVersion(""$fileVersion"")]"
if ($Version -ne $numeric) {
    $patched = $patched.TrimEnd() + "`r`n[assembly: AssemblyInformationalVersion(""$Version"")]`r`n"
}
try {
    [IO.File]::WriteAllText($assemblyInfo, $patched, (New-Object Text.UTF8Encoding($true)))
    Invoke-Checked $MSBuild @((Join-Path $repo 'AmbiBack USB Flasher.csproj'), '-t:Rebuild', '-p:Configuration=Release',
        "-p:OutputPath=$bin", "-p:BaseIntermediateOutputPath=$obj", '-p:TreatWarningsAsErrors=true', '-warnaserror',
        '-nologo', '-v:minimal')
}
finally {
    [IO.File]::WriteAllBytes($assemblyInfo, $originalInfo)
}

$exe = Join-Path $bin $exeName
$builtVersion = (Get-Item $exe).VersionInfo.FileVersion
if ($builtVersion -ne $fileVersion) { throw "$exeName has version $builtVersion, expected $fileVersion." }

# --- Staging
Get-ChildItem $bin -Recurse -File |
    Where-Object { $_.Extension -notin '.pdb', '.xml' -and $_.Name -notlike '*.vshost.*' } |
    ForEach-Object {
        $target = Join-Path $staging $_.FullName.Substring($bin.Length)
        New-Item -ItemType Directory -Force (Split-Path -Parent $target) | Out-Null
        Copy-Item $_.FullName $target
    }
foreach ($f in 'LICENSE', 'THIRD_PARTY_NOTICES.md', 'README.md', 'README.de.md', '.env.example') {
    Copy-Item (Join-Path $repo $f) $staging
}
foreach ($f in "$exeName", "$exeName.config", 'scripts\upload-firmware.ps1', 'devices\m5camtx.png') {
    if (-not (Test-Path (Join-Path $staging $f))) { throw "Staging is missing $f." }
}

# --- Signing (optional)
$signArgs = $null
if ($SignPfx) {
    if (-not $env:SIGN_PFX_PASSWORD) { throw 'SIGN_PFX_PASSWORD is not set.' }
    $kitsBin = Join-Path ${env:ProgramFiles(x86)} 'Windows Kits\10\bin'
    $signtool = Get-ChildItem $kitsBin -Recurse -Filter signtool.exe -ErrorAction SilentlyContinue |
        Where-Object { $_.DirectoryName -like '*\x64' } | Sort-Object FullName -Descending | Select-Object -First 1
    if (-not $signtool) { throw 'signtool.exe not found (Windows SDK).' }
    $signArgs = @('sign', '/fd', 'SHA256', '/td', 'SHA256', '/tr', $TimestampUrl, '/f', $SignPfx, '/p', $env:SIGN_PFX_PASSWORD)
    Invoke-Checked $signtool.FullName ($signArgs + @((Join-Path $staging $exeName)))
}

# --- Portable ZIP
$zip = Join-Path $dist "$baseName-$Version.zip"
Compress-Archive -Path (Join-Path $staging '*') -DestinationPath $zip -CompressionLevel Optimal

# --- Setup
$isccArgs = @("/DAppVersion=$Version", "/DFileVersion=$fileVersion", "/DSourceDir=$staging", "/DOutputDir=$dist",
    "/DOutputName=$baseName-$Version-setup", '/Qp')
if ($signArgs) {
    # $q is replaced by a quote and $f by the file name by Inno Setup
    $quotedArgs = ($signArgs | ForEach-Object { if ($_ -match '\s') { "`$q$_`$q" } else { $_ } }) -join ' '
    $isccArgs += @('/DSign', "/Ssigntool=`$q$($signtool.FullName)`$q $quotedArgs `$f")
}
Invoke-Checked $Iscc ($isccArgs + @((Join-Path $PSScriptRoot 'AmbiBackFlasher.iss')))

Write-Host ''
Write-Host 'Created:'
Get-ChildItem $dist | ForEach-Object { Write-Host ("  {0}  ({1:N1} MB)" -f $_.FullName, ($_.Length / 1MB)) }

if ($env:GITHUB_OUTPUT) {
    # Forward slashes: the glob matching of the release upload action does not accept backslashes
    $setup = Join-Path $dist "$baseName-$Version-setup.exe"
    @("version=$Version",
      "file_version=$fileVersion",
      "setup_path=$($setup -replace '\\', '/')",
      "zip_path=$($zip -replace '\\', '/')") |
        Out-File -FilePath $env:GITHUB_OUTPUT -Append -Encoding utf8
}
