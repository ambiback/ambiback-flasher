; AmbiBack USB Flasher setup (Inno Setup 6.3 or later)
;
; Built by .github/workflows/release.yml, or locally with installer/build-installer.ps1.
; Defines passed on the ISCC command line:
;   /DAppVersion=1.2.3        version shown in the setup and in Windows "Installed apps"
;   /DFileVersion=1.2.3.45    numeric version of the setup file (optional, default AppVersion.0)
;   /DSourceDir=<folder>      staged application files (exe, config, devices\, scripts\, LICENSE ...)
;   /DOutputDir=<folder>      where the setup is written
;   /DOutputName=<name>       setup file name without .exe
;   /DSign                    sign setup and uninstaller with the sign tool "signtool" (ISCC /Ssigntool=...)
;
; Data folder: the tool stores arduino-cli, the ESP32 core, libraries, source and firmware images
; (several hundred MB) in its program folder when that folder is writable, otherwise in
; %LOCALAPPDATA%\AmbiBack\USB Flasher (AppPaths.cs). A per-user install (default) is writable.

#ifndef AppVersion
  #define AppVersion "0.0.0"
#endif
#ifndef FileVersion
  #define FileVersion AppVersion + ".0"
#endif
#ifndef SourceDir
  #error SourceDir is not defined
#endif
#ifndef OutputDir
  #define OutputDir "."
#endif
#ifndef OutputName
  #define OutputName "AmbiBack-USB-Flasher-" + AppVersion + "-setup"
#endif

#define AppName "AmbiBack USB Flasher"
#define AppExe "AmbiBack USB Flasher.exe"
#define AppPublisher "Andre Lorbach"
#define AppUrl "https://github.com/ambiback/ambiback-flasher"

[Setup]
AppId={{B3E7A1D4-58C2-4F0B-9A6E-7C1D2F84E953}
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppUrl}
AppSupportURL={#AppUrl}/issues
AppUpdatesURL={#AppUrl}/releases
AppCopyright=Copyright (C) 2020-2026 {#AppPublisher}
VersionInfoVersion={#FileVersion}
VersionInfoProductVersion={#FileVersion}
VersionInfoDescription={#AppName} Setup
; Per user by default (no admin), "for all users" can be chosen in the setup
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
DefaultDirName={autopf}\AmbiBack USB Flasher
DefaultGroupName=AmbiBack
DisableProgramGroupPage=yes
ArchitecturesAllowed=x86compatible x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
LicenseFile={#SourceDir}\LICENSE
SetupIconFile=..\Resources\ambiback.ico
UninstallDisplayIcon={app}\{#AppExe}
UninstallDisplayName={#AppName}
WizardStyle=modern
ShowLanguageDialog=no
LanguageDetectionMethod=uilanguage
CloseApplications=yes
RestartApplications=no
OutputDir={#OutputDir}
OutputBaseFilename={#OutputName}
Compression=lzma2/max
SolidCompression=yes
#ifdef Sign
SignTool=signtool
SignedUninstaller=yes
#endif

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "de"; MessagesFile: "compiler:Languages\German.isl"

[CustomMessages]
en.DotNetMissing=The AmbiBack USB Flasher needs the Microsoft .NET Framework 4.7.2 or later, which is not installed.%n%nDownload and install %1 from Microsoft now? This needs an internet connection and administrator rights.
de.DotNetMissing=Der AmbiBack USB Flasher braucht das Microsoft .NET Framework 4.7.2 oder neuer, das nicht installiert ist.%n%nJetzt %1 von Microsoft herunterladen und installieren? Dafür werden eine Internetverbindung und Administratorrechte gebraucht.
en.DotNetDeclined=Without the .NET Framework the flasher will not start. You can install it later from https://dotnet.microsoft.com/download/dotnet-framework.%n%nContinue the setup anyway?
de.DotNetDeclined=Ohne das .NET Framework startet der Flasher nicht. Es lässt sich später von https://dotnet.microsoft.com/download/dotnet-framework installieren.%n%nSetup trotzdem fortsetzen?
en.DotNetFailed=The .NET Framework could not be installed (%1). Windows may need a restart first. The flasher is installed anyway; if it does not start, install the .NET Framework from https://dotnet.microsoft.com/download/dotnet-framework.
de.DotNetFailed=Das .NET Framework konnte nicht installiert werden (%1). Eventuell muss Windows zuerst neu gestartet werden. Der Flasher wird trotzdem installiert; falls er nicht startet, das .NET Framework von https://dotnet.microsoft.com/download/dotnet-framework installieren.
en.DotNetDownloading=Downloading the Microsoft .NET Framework ...
de.DotNetDownloading=Microsoft .NET Framework wird heruntergeladen ...
en.DotNetInstalling=Installing the Microsoft .NET Framework ...
de.DotNetInstalling=Microsoft .NET Framework wird installiert ...
en.UsbDrivers=If Windows shows no COM port for your device, install the driver for its USB chip:
de.UsbDrivers=Zeigt Windows keinen COM-Port für das Gerät, den Treiber für seinen USB-Chip installieren:
en.UninstallData=Also delete the downloaded tools, firmware source, built firmware images and the settings of the AmbiBack USB Flasher?
de.UninstallData=Auch die heruntergeladenen Werkzeuge, den Firmware-Quellcode, die gebauten Firmware-Images und die Einstellungen des AmbiBack USB Flashers löschen?

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#SourceDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\{#AppName}"; Filename: "{app}\{#AppExe}"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExe}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#AppExe}"; Description: "{cm:LaunchProgram,{#AppName}}"; Flags: nowait postinstall skipifsilent

[Code]
const
  DotNet472Release = 461808;
  DotNet481Url = 'https://go.microsoft.com/fwlink/?LinkId=2203304';
  DotNet48Url = 'https://go.microsoft.com/fwlink/?LinkId=2085155';

var
  DownloadPage: TDownloadWizardPage;
  DotNetChecked: Boolean;
  DotNetNeedsRestart: Boolean;

function NeedRestart: Boolean;
begin
  Result := DotNetNeedsRestart;
end;

{ --- .NET Framework ------------------------------------------------------- }

function DotNetInstalled: Boolean;
var
  Release: Cardinal;
begin
  Result := RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', Release)
    and (Release >= DotNet472Release);
end;

{ 4.8.1 runs on Windows 10 21H2 (build 19044), Windows 11 and Server 2022 and later, 4.8 below that }
function UseDotNet481: Boolean;
var
  Version: TWindowsVersion;
begin
  GetWindowsVersionEx(Version);
  Result := Version.Build >= 19044;
end;

function DotNetName: String;
begin
  if UseDotNet481 then
    Result := '.NET Framework 4.8.1'
  else
    Result := '.NET Framework 4.8';
end;

function OnDownloadProgress(const Url, FileName: String; const Progress, ProgressMax: Int64): Boolean;
begin
  Result := True;
end;

{ Returns False when the setup should go back to the Ready page }
function InstallDotNet: Boolean;
var
  Url, FileName: String;
  ResultCode: Integer;
begin
  Result := True;
  if UseDotNet481 then
  begin
    Url := DotNet481Url;
    FileName := 'NDP481-Web.exe';
  end
  else
  begin
    Url := DotNet48Url;
    FileName := 'ndp48-web.exe';
  end;

  DownloadPage.Clear;
  DownloadPage.Add(Url, FileName, '');
  DownloadPage.SetText(CustomMessage('DotNetDownloading'), '');
  DownloadPage.Show;
  try
    try
      DownloadPage.Download;
      DownloadPage.SetText(CustomMessage('DotNetInstalling'), '');
      DownloadPage.ProgressBar.Style := npbstMarquee;
      { The web installer needs administrator rights, runas shows the UAC prompt }
      if not ShellExec('runas', ExpandConstant('{tmp}\') + FileName, '/passive /norestart', '',
        SW_SHOW, ewWaitUntilTerminated, ResultCode) then
        MsgBox(FmtMessage(CustomMessage('DotNetFailed'), [SysErrorMessage(ResultCode)]), mbError, MB_OK)
      else if (ResultCode <> 0) and (ResultCode <> 1641) and (ResultCode <> 3010) then
        MsgBox(FmtMessage(CustomMessage('DotNetFailed'), [IntToStr(ResultCode)]), mbError, MB_OK)
      else if (ResultCode = 1641) or (ResultCode = 3010) then
        DotNetNeedsRestart := True;
    except
      if DownloadPage.AbortedByUser then
        Result := False
      else
        MsgBox(FmtMessage(CustomMessage('DotNetFailed'), [GetExceptionMessage]), mbError, MB_OK);
    end;
  finally
    DownloadPage.ProgressBar.Style := npbstNormal;
    DownloadPage.Hide;
  end;
end;

{ --- Wizard --------------------------------------------------------------- }

procedure InitializeWizard;
begin
  DownloadPage := CreateDownloadPage(SetupMessage(msgWizardPreparing), SetupMessage(msgPreparingDesc), @OnDownloadProgress);
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := True;
  if (CurPageID = wpReady) and not DotNetChecked and not DotNetInstalled then
  begin
    if MsgBox(FmtMessage(CustomMessage('DotNetMissing'), [DotNetName]), mbConfirmation, MB_YESNO) = IDYES then
      Result := InstallDotNet
    else
      Result := MsgBox(CustomMessage('DotNetDeclined'), mbConfirmation, MB_YESNO) = IDYES;
    DotNetChecked := Result;
  end;
end;

procedure OpenLink(Sender: TObject);
var
  ErrorCode: Integer;
begin
  ShellExecAsOriginalUser('open', TNewStaticText(Sender).Hint, '', '', SW_SHOWNORMAL, ewNoWait, ErrorCode);
end;

function AddLink(const Caption, Url: String; Top: Integer): Integer;
var
  Link: TNewStaticText;
begin
  Link := TNewStaticText.Create(WizardForm);
  Link.Parent := WizardForm.FinishedPage;
  Link.Left := WizardForm.FinishedLabel.Left + ScaleX(12);
  Link.Top := Top;
  Link.Caption := Caption;
  Link.Hint := Url;
  Link.Cursor := crHand;
  Link.Font.Color := clHotLight;
  Link.Font.Style := [fsUnderline];
  Link.OnClick := @OpenLink;
  Result := Link.Top + Link.Height + ScaleY(4);
end;

procedure CurPageChanged(CurPageID: Integer);
var
  Info: TNewStaticText;
  Top: Integer;
begin
  if (CurPageID = wpFinished) and (WizardForm.FinishedPage.Tag = 0) then
  begin
    WizardForm.FinishedPage.Tag := 1;
    Info := TNewStaticText.Create(WizardForm);
    Info.Parent := WizardForm.FinishedPage;
    Info.Left := WizardForm.FinishedLabel.Left;
    Info.Width := WizardForm.FinishedLabel.Width;
    Info.AutoSize := False;
    Info.WordWrap := True;
    Info.Caption := CustomMessage('UsbDrivers');
    Info.Height := ScaleY(30);
    Info.Top := WizardForm.FinishedPage.ClientHeight - ScaleY(100);
    Top := Info.Top + Info.Height + ScaleY(2);
    Top := AddLink('FTDI VCP Drivers', 'https://ftdichip.com/drivers/vcp-drivers/', Top);
    Top := AddLink('Silicon Labs CP210x Drivers', 'https://www.silabs.com/developers/usb-to-uart-bridge-vcp-drivers', Top);
    AddLink('WCH CH340 / CH9102 Drivers', 'https://www.wch-ic.com/downloads/CH341SER_EXE.html', Top);
  end;
end;

{ --- Uninstall ------------------------------------------------------------ }

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if (CurUninstallStep = usPostUninstall) and not UninstallSilent then
    if SuppressibleMsgBox(CustomMessage('UninstallData'), mbConfirmation, MB_YESNO or MB_DEFBUTTON2, IDNO) = IDYES then
    begin
      { Data root is the program folder when it was writable, otherwise %LOCALAPPDATA%\AmbiBack\USB Flasher (AppPaths.cs).
        Settings: user.config in %LOCALAPPDATA%\AmbiBack_USB_Flasher (empty AssemblyCompany). }
      DelTree(ExpandConstant('{app}'), True, True, True);
      DelTree(ExpandConstant('{localappdata}\AmbiBack\USB Flasher'), True, True, True);
      RemoveDir(ExpandConstant('{localappdata}\AmbiBack'));
      DelTree(ExpandConstant('{localappdata}\AmbiBack_USB_Flasher'), True, True, True);
    end;
end;
