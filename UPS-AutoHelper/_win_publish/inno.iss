; �ű��� Inno Setup �ű��� ���ɣ�
; �йش��� Inno Setup �ű��ļ�����ϸ��������İ����ĵ���

#define MyAppName "UPS�Զ��ػ�"
#define MyAppVersion "1.0"
#define MyAppPublisher "�ҵĹ�˾"
#define MyAppURL "https://www.example.com/"
#define MyAppExeName "UPS�Զ��ػ�.exe"

[Setup]
; ע: AppId��ֵΪ������ʶ��Ӧ�ó���
; ��ҪΪ������װ����ʹ����ͬ��AppIdֵ��
; (��Ҫ�����µ� GUID�����ڲ˵��е�� "����|���� GUID"��)
AppId={{00DF2B29-25B5-4070-86AB-84878A9813A0}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\UPS-AutoHepler
DefaultGroupName=UPS�Զ��ػ�
; ������ȡ��ע�ͣ����ڷǹ���װģʽ�����У���Ϊ��ǰ�û���װ����
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
; ע��: ��Ҫ���κι���ϵͳ�ļ���ʹ�á�Flags: ignoreversion��

[Icons]
; ����ͼ��
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\tray.ico"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon; IconFilename: "{app}\tray.ico"
;���ÿ�������
Name: "{commonstartup}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"


[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent


