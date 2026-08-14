; -- Bplus Link Copier Professional Setup Script --
; Industry-standard installer for Revit 2019-2027

[Setup]
AppId={{E9B1C2A3-4567-4890-ABCD-123456789FED}
AppName=Bplus Link Copier for Revit
AppVersion=1.0.0
AppPublisher=B Plus Pvt Ltd
AppPublisherURL=https://bplus.lk/
AppSupportURL=https://bplus.lk/
AppUpdatesURL=https://bplus.lk/
DefaultDirName={userappdata}\Autodesk\Revit\Addins
DefaultGroupName=Bplus Automation
DisableProgramGroupPage=yes
LicenseFile=..\LICENSE
PrivilegesRequired=lowest
OutputBaseFilename=BplusLinkCopier_Full_Setup_2019-2027
OutputDir=..\
Compression=lzma2/max
SolidCompression=yes
WizardStyle=modern

[Files]
; Revit 2027 Deployment (.NET 10)
Source: "Revit_2027-2028\BplusLinkCopier.addin"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2027"; Flags: ignoreversion
Source: "Revit_2027-2028\BplusLinkCopier.dll"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2027"; Flags: ignoreversion
Source: "Revit_2027-2028\BplusLinkCopier.deps.json"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2027"; Flags: ignoreversion

; Revit 2026 Deployment (.NET 8)
Source: "Revit_2025-2026\BplusLinkCopier.addin"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2026"; Flags: ignoreversion
Source: "Revit_2025-2026\BplusLinkCopier.dll"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2026"; Flags: ignoreversion
Source: "Revit_2025-2026\BplusLinkCopier.deps.json"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2026"; Flags: ignoreversion

; Revit 2025 Deployment (.NET 8)
Source: "Revit_2025-2026\BplusLinkCopier.addin"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2025"; Flags: ignoreversion
Source: "Revit_2025-2026\BplusLinkCopier.dll"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2025"; Flags: ignoreversion
Source: "Revit_2025-2026\BplusLinkCopier.deps.json"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2025"; Flags: ignoreversion

; Revit 2024 Deployment (.NET 4.8)
Source: "Revit_2023-2024\BplusLinkCopier.addin"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2024"; Flags: ignoreversion
Source: "Revit_2023-2024\BplusLinkCopier.dll"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2024"; Flags: ignoreversion

; Revit 2023 Deployment (.NET 4.8)
Source: "Revit_2023-2024\BplusLinkCopier.addin"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2023"; Flags: ignoreversion
Source: "Revit_2023-2024\BplusLinkCopier.dll"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2023"; Flags: ignoreversion

; Revit 2022 Deployment (.NET 4.8)
Source: "Revit_2023-2024\BplusLinkCopier.addin"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2022"; Flags: ignoreversion
Source: "Revit_2023-2024\BplusLinkCopier.dll"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2022"; Flags: ignoreversion

; Revit 2021 Deployment (.NET 4.8)
Source: "Revit_2023-2024\BplusLinkCopier.addin"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2021"; Flags: ignoreversion
Source: "Revit_2023-2024\BplusLinkCopier.dll"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2021"; Flags: ignoreversion

; Revit 2020 Deployment (.NET 4.8)
Source: "Revit_2023-2024\BplusLinkCopier.addin"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2020"; Flags: ignoreversion
Source: "Revit_2023-2024\BplusLinkCopier.dll"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2020"; Flags: ignoreversion

; Revit 2019 Deployment (.NET 4.8)
Source: "Revit_2023-2024\BplusLinkCopier.addin"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2019"; Flags: ignoreversion
Source: "Revit_2023-2024\BplusLinkCopier.dll"; DestDir: "{userappdata}\Autodesk\Revit\Addins\2019"; Flags: ignoreversion

[UninstallDelete]
Type: files; Name: "{userappdata}\Autodesk\Revit\Addins\2027\BplusLinkCopier.*"
Type: files; Name: "{userappdata}\Autodesk\Revit\Addins\2026\BplusLinkCopier.*"
Type: files; Name: "{userappdata}\Autodesk\Revit\Addins\2025\BplusLinkCopier.*"
Type: files; Name: "{userappdata}\Autodesk\Revit\Addins\2024\BplusLinkCopier.*"
Type: files; Name: "{userappdata}\Autodesk\Revit\Addins\2023\BplusLinkCopier.*"
Type: files; Name: "{userappdata}\Autodesk\Revit\Addins\2022\BplusLinkCopier.*"
Type: files; Name: "{userappdata}\Autodesk\Revit\Addins\2021\BplusLinkCopier.*"
Type: files; Name: "{userappdata}\Autodesk\Revit\Addins\2020\BplusLinkCopier.*"
Type: files; Name: "{userappdata}\Autodesk\Revit\Addins\2019\BplusLinkCopier.*"
