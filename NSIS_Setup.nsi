; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "Q-Drive"
!define PRODUCT_VERSION "1.0.0"
!define PRODUCT_PUBLISHER "Tobias Hattinger @ Endev"
!define PRODUCT_WEB_SITE "https://endev.at/p/q-drive"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\QDriveAutostart.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"
!define PRODUCT_STARTMENU_REGVAL "NSIS:StartMenuDir"

SetCompressor lzma

; MUI 1.67 compatible ------
!include "MUI.nsh"

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "Content\QDriveLogo.ico"
!define MUI_UNICON "Content\QDriveUninstall.ico"

; Welcome page
!insertmacro MUI_PAGE_WELCOME
; License page
!insertmacro MUI_PAGE_LICENSE "LICENSE"
; Components page
!insertmacro MUI_PAGE_COMPONENTS
; Directory page
!insertmacro MUI_PAGE_DIRECTORY
; Start menu page
var ICONS_GROUP
!define MUI_STARTMENUPAGE_NODISABLE
!define MUI_STARTMENUPAGE_DEFAULTFOLDER "Q-Drive"
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "${PRODUCT_UNINST_ROOT_KEY}"
!define MUI_STARTMENUPAGE_REGISTRY_KEY "${PRODUCT_UNINST_KEY}"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "${PRODUCT_STARTMENU_REGVAL}"
!insertmacro MUI_PAGE_STARTMENU Application $ICONS_GROUP
; Instfiles page
!insertmacro MUI_PAGE_INSTFILES
; Finish page
!define MUI_FINISHPAGE_RUN "$INSTDIR\QDriveAutostart.exe"
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES

; Language files
!insertmacro MUI_LANGUAGE "English"

; Reserve files
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS

; MUI end ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "QDrive_Setup.exe"
InstallDir "$PROGRAMFILES\Q-Drive"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
ShowInstDetails show
ShowUnInstDetails show

Section "Q-Drive Base" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite try
  File "QDriveManager\bin\Release\BouncyCastle.Crypto.dll"
  File "QDriveManager\bin\Release\EnvDTE.dll"
  File "QDriveManager\bin\Release\Google.Protobuf.dll"
  File "QDriveManager\bin\Release\K4os.Compression.LZ4.dll"
  File "QDriveManager\bin\Release\K4os.Compression.LZ4.Streams.dll"
  File "QDriveManager\bin\Release\K4os.Hash.xxHash.dll"
  File "QDriveManager\bin\Release\MySql.Data.dll"
  File "QDriveManager\bin\Release\QDriveLib.dll"
  File "QDriveManager\bin\Release\QDriveLib.dll.config"
  File "QDriveManager\bin\Release\Renci.SshNet.dll"
  File "QDriveManager\bin\Release\stdole.dll"
  File "QDriveManager\bin\Release\Syncfusion.Core.WinForms.dll"
  File "QDriveManager\bin\Release\Syncfusion.Data.WinForms.dll"
  File "QDriveManager\bin\Release\Syncfusion.Grid.Base.dll"
  File "QDriveManager\bin\Release\Syncfusion.Grid.Windows.dll"
  File "QDriveManager\bin\Release\Syncfusion.Grid.Windows.XmlSerializers.dll"
  File "QDriveManager\bin\Release\Syncfusion.Licensing.dll"
  File "QDriveManager\bin\Release\Syncfusion.Shared.Base.dll"
  File "QDriveManager\bin\Release\Syncfusion.Shared.Windows.dll"
  File "QDriveManager\bin\Release\Syncfusion.SpellChecker.Base.dll"
  File "QDriveManager\bin\Release\Syncfusion.Tools.Base.dll"
  File "QDriveManager\bin\Release\Syncfusion.Tools.Windows.dll"
  File "QDriveManager\bin\Release\System.Buffers.dll"
  File "QDriveManager\bin\Release\System.Data.SQLite.dll"
  File "QDriveManager\bin\Release\System.Data.SQLite.dll.config"
  File "QDriveManager\bin\Release\System.Memory.dll"
  File "QDriveManager\bin\Release\System.Numerics.Vectors.dll"
  File "QDriveManager\bin\Release\System.Runtime.CompilerServices.Unsafe.dll"
  File "QDriveManager\bin\Release\Ubiety.Dns.Core.dll"
  SetOutPath "$INSTDIR\x64"
  File "QDriveManager\bin\Release\x64\SQLite.Interop.dll"
  SetOutPath "$INSTDIR\x86"
  File "QDriveManager\bin\Release\x86\SQLite.Interop.dll"
  SetOutPath "$INSTDIR"
  File "QDriveManager\bin\Release\Zstandard.Net.dll"
  SetOverwrite ifnewer
  File "QDriveAutostart\bin\Release\QDriveAutostart.exe"
  File "QDriveManager\bin\Release\QDriveManager.exe"
  File "QDriveSetup\bin\Release\QDriveSetup.exe"

; Shortcuts
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
  CreateDirectory "$SMPROGRAMS\$ICONS_GROUP"
  CreateShortCut "$SMPROGRAMS\$ICONS_GROUP\Q-Drive.lnk" "$INSTDIR\QDriveAutostart.exe"
  CreateDirectory "$INSTDIR"
  CreateShortCut "$INSTDIR\Q-Drive.lnk" "$INSTDIR\QDriveAutostart.exe"
  CreateShortCut "$SMPROGRAMS\$ICONS_GROUP\Q-Drive Manager.lnk" "$INSTDIR\QDriveManager.exe"
  CreateShortCut "$DESKTOP\Q-Drive Manager.lnk" "$INSTDIR\QDriveManager.exe"
  CreateShortCut "$SMPROGRAMS\$ICONS_GROUP\Q-Drive Setup.lnk" "$INSTDIR\QDriveSetup.exe"
  CreateShortCut "$DESKTOP\Q-Drive Setup.lnk" "$INSTDIR\QDriveSetup.exe"
  CreateShortCut "$SMSTARTUP\Q-Drive.lnk" "$INSTDIR\QDriveAutostart.exe"
  !insertmacro MUI_STARTMENU_WRITE_END
SectionEnd

Section "Q-Drive Admin Console" SEC02
  File "QDriveAdminConsole\bin\Release\QDriveAdminConsole.exe"

; Shortcuts
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
  CreateShortCut "$SMPROGRAMS\$ICONS_GROUP\Q-Drive Admin Console.lnk" "$INSTDIR\QDriveAdminConsole.exe"
  CreateShortCut "$DESKTOP\Q-Drive Admin Console.lnk" "$INSTDIR\QDriveAdminConsole.exe"
  !insertmacro MUI_STARTMENU_WRITE_END
SectionEnd

Section -AdditionalIcons
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
  WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\$ICONS_GROUP\Website.lnk" "$INSTDIR\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\$ICONS_GROUP\Uninstall.lnk" "$INSTDIR\uninst.exe"
  !insertmacro MUI_STARTMENU_WRITE_END
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\QDriveAutostart.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\QDriveAutostart.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd

; Section descriptions
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${SEC01} "Required. The Main Q-Drive Program and management tools"
  !insertmacro MUI_DESCRIPTION_TEXT ${SEC02} "Admin-Console for configuring QD-Online. Only required for network or system administrator."
!insertmacro MUI_FUNCTION_DESCRIPTION_END


Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) was uninstalled successfully."
FunctionEnd

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Do you want to uninstall $(^Name) and all of its components?" IDYES +2
  Abort
FunctionEnd

Section Uninstall
  !insertmacro MUI_STARTMENU_GETFOLDER "Application" $ICONS_GROUP
  Delete "$INSTDIR\${PRODUCT_NAME}.url"
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\QDriveAdminConsole.exe"
  Delete "$INSTDIR\QDriveSetup.exe"
  Delete "$INSTDIR\QDriveManager.exe"
  Delete "$INSTDIR\QDriveAutostart.exe"
  Delete "$INSTDIR\Zstandard.Net.dll"
  Delete "$INSTDIR\x86\SQLite.Interop.dll"
  Delete "$INSTDIR\x64\SQLite.Interop.dll"
  Delete "$INSTDIR\Ubiety.Dns.Core.dll"
  Delete "$INSTDIR\System.Runtime.CompilerServices.Unsafe.dll"
  Delete "$INSTDIR\System.Numerics.Vectors.dll"
  Delete "$INSTDIR\System.Memory.dll"
  Delete "$INSTDIR\System.Data.SQLite.dll.config"
  Delete "$INSTDIR\System.Data.SQLite.dll"
  Delete "$INSTDIR\System.Buffers.dll"
  Delete "$INSTDIR\Syncfusion.Tools.Windows.dll"
  Delete "$INSTDIR\Syncfusion.Tools.Base.dll"
  Delete "$INSTDIR\Syncfusion.SpellChecker.Base.dll"
  Delete "$INSTDIR\Syncfusion.Shared.Windows.dll"
  Delete "$INSTDIR\Syncfusion.Shared.Base.dll"
  Delete "$INSTDIR\Syncfusion.Licensing.dll"
  Delete "$INSTDIR\Syncfusion.Grid.Windows.XmlSerializers.dll"
  Delete "$INSTDIR\Syncfusion.Grid.Windows.dll"
  Delete "$INSTDIR\Syncfusion.Grid.Base.dll"
  Delete "$INSTDIR\Syncfusion.Data.WinForms.dll"
  Delete "$INSTDIR\Syncfusion.Core.WinForms.dll"
  Delete "$INSTDIR\stdole.dll"
  Delete "$INSTDIR\Renci.SshNet.dll"
  Delete "$INSTDIR\QDriveLib.dll.config"
  Delete "$INSTDIR\QDriveLib.dll"
  Delete "$INSTDIR\MySql.Data.dll"
  Delete "$INSTDIR\K4os.Hash.xxHash.dll"
  Delete "$INSTDIR\K4os.Compression.LZ4.Streams.dll"
  Delete "$INSTDIR\K4os.Compression.LZ4.dll"
  Delete "$INSTDIR\Google.Protobuf.dll"
  Delete "$INSTDIR\EnvDTE.dll"
  Delete "$INSTDIR\BouncyCastle.Crypto.dll"

  Delete "$SMPROGRAMS\$ICONS_GROUP\Uninstall.lnk"
  Delete "$SMPROGRAMS\$ICONS_GROUP\Website.lnk"
  Delete "$DESKTOP\Q-Drive Admin Console.lnk"
  Delete "$SMPROGRAMS\$ICONS_GROUP\Q-Drive Admin Console.lnk"
  Delete "$DESKTOP\Q-Drive Setup.lnk"
  Delete "$SMPROGRAMS\$ICONS_GROUP\Q-Drive Setup.lnk"
  Delete "$DESKTOP\Q-Drive Manager.lnk"
  Delete "$SMPROGRAMS\$ICONS_GROUP\Q-Drive Manager.lnk"
  Delete "$INSTDIR\Q-Drive.lnk"
  Delete "$SMPROGRAMS\$ICONS_GROUP\Q-Drive.lnk"
  Delete "$SMSTARTUP\Q-Drive.lnk"

  RMDir "$SMPROGRAMS\$ICONS_GROUP"
  RMDir "$INSTDIR\x86"
  RMDir "$INSTDIR\x64"
  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd