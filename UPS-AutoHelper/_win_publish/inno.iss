; 脚本由 Inno Setup 脚本向导 生成！
; 有关创建 Inno Setup 脚本文件的详细资料请查阅帮助文档！

#define MyAppName "UPS自动关机"
#define MyAppVersion "1.0"
#define MyAppPublisher "我的公司"
#define MyAppURL "https://www.example.com/"
#define MyAppExeName "UPS自动关机.exe"

[Setup]
; 注: AppId的值为单独标识该应用程序。
; 不要为其他安装程序使用相同的AppId值。
; (若要生成新的 GUID，可在菜单中点击 "工具|生成 GUID"。)
AppId={{00DF2B29-25B5-4070-86AB-84878A9813A0}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\UPS-AutoHepler
DefaultGroupName=UPS自动关机
; 以下行取消注释，以在非管理安装模式下运行（仅为当前用户安装）。
;PrivilegesRequired=lowest
OutputDir=%USERPROFILE%\Desktop
OutputBaseFilename=upsauto-settup
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "%USERPROFILE%\Desktop\publish\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "%USERPROFILE%\Desktop\publish\av_libglesv2.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "%USERPROFILE%\Desktop\publish\libHarfBuzzSharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "%USERPROFILE%\Desktop\publish\libSkiaSharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "%USERPROFILE%\Desktop\publish\tray.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "%USERPROFILE%\Desktop\publish\tray.png"; DestDir: "{app}"; Flags: ignoreversion
; 注意: 不要在任何共享系统文件上使用“Flags: ignoreversion”

[Icons]
; 设置图标
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\tray.ico"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon; IconFilename: "{app}\tray.ico"
;设置开机自启
Name: "{commonstartup}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"


[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent


