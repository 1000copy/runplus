; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "1000tools"
#define MyAppVerName "1000tools 1.0"
#define MyAppPublisher "1000copy, Inc."
#define MyAppExeName "runplus.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{D83ED962-FAB2-4A9B-B433-431DCA7A4F2C}
AppName={#MyAppName}
AppVerName={#MyAppVerName}
AppPublisher={#MyAppPublisher}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
InfoAfterFile=..\\pack\1000toolsreadme.txt
OutputDir=..\\bin2
OutputBaseFilename=1000tools
Compression=lzma
SolidCompression=yes

[Languages]
Name: english; MessagesFile: compiler:Default.isl

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked
Name: quicklaunchicon; Description: {cm:CreateQuickLaunchIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked

[Files]
Source: ..\\pack\*; DestDir: {app}; Flags: ignoreversion recursesubdirs createallsubdirs
[Icons]
Name: {group}\{#MyAppName}; Filename: {app}\{#MyAppExeName}
Name: {group}\easycmd; Filename: {app}\easycmd.exe
Name: {group}\{cm:UninstallProgram,{#MyAppName}}; Filename: {uninstallexe}
Name: {commondesktop}\{#MyAppName}; Filename: {app}\{#MyAppExeName}; Tasks: desktopicon
Name: {userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}; Filename: {app}\{#MyAppExeName}; Tasks: quicklaunchicon

[Run]
Filename: {app}\{#MyAppExeName}; Description: {cm:LaunchProgram,{#MyAppName}}; Flags: nowait postinstall skipifsilent
[Registry]
Root: HKCR; Subkey: .txt; ValueType: string; ValueData: 1000tools
Root: HKCR; Subkey: .rb; ValueType: string; ValueData: 1000tools
Root: HKCR; Subkey: .sql; ValueType: string; ValueData: 1000tools
Root: HKCR; Subkey: .log; ValueType: string; ValueData: 1000tools
Root: HKCR; Subkey: .cs; ValueType: string; ValueData: 1000tools
Root: HKCR; Subkey: .pas; ValueType: string; ValueData: 1000tools
Root: HKCR; Subkey: 1000tools\shell\open\command; ValueType: string; ValueData: {app}\sc178.exe %1
Root: HKCU; Subkey: Software\Microsoft\Windows\CurrentVersion\Run; ValueType: string; ValueData: {app}\runplus.exe
