; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Modboy"
#define MyAppVersion "1.0"
#define MyAppPublisher "GameBanana"
#define MyAppURL "http://www.modboy.io/"
#define MyAppExeName "Modboy.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{46D16449-B400-4AD8-8EAC-2FDD009B9D73}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputDir=bin
OutputBaseFilename=modboy_install
SetupIconFile=favicon.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\Modboy\bin\Release\*.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Modboy\bin\Release\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Modboy\bin\Release\{#MyAppName}.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Modboy\bin\Release\Resources\*"; DestDir: "{app}\Resources"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Registry]
; Register a protocol
Root: HKCR; Subkey: "modboy"; Flags: uninsdeletekey
Root: HKCR; Subkey: "modboy"; ValueType: string; ValueName: ""; ValueData: "URL:modboy"
Root: HKCR; Subkey: "modboy"; ValueType: string; ValueName: "URL Protocol"; ValueData: ""
Root: HKCR; Subkey: "modboy\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{#MyAppExeName},1"
Root: HKCR; Subkey: "modboy\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
procedure PromptDeleteUserFiles();
begin
  if MsgBox('Do you want to also delete user files (settings, logs, etc)?', mbConfirmation, MB_YESNO) = IDYES then
    DelTree(ExpandConstant('{userappdata}') + '\{#MyAppName}', True, True, True);
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usPostUninstall then
    PromptDeleteUserFiles();
end;