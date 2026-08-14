# Bplus Link Copier for Autodesk Revit® (2019 – 2027)

![Revit Version](https://img.shields.io/badge/Revit-2019--2027-blue.svg)
![NET Version](https://img.shields.io/badge/.NET-4.8%20%7C%208.0%20%7C%2010.0-green.svg)
![License](https://img.shields.io/badge/License-MIT-yellow.svg)
![Bplus](https://img.shields.io/badge/Bplus-Plus_preparing_for_tomorrow-red.svg)

> **Fast, Intelligent Element Transfer from Linked Revit Models**

The **Bplus Link Copier** is a high-performance C# Revit API add-in designed for BIM managers, structural engineers, architects, and MEP coordinators. It streamlines model transfer by allowing users to inspect, filter, select, and copy elements from linked Revit models into active host projects with 100% geospatial coordinate precision.

---

## ⚡ Key Capabilities

* **🔗 Intelligent Link Scanner**: Automatically detects and scans all loaded `RevitLinkInstance` documents in the active project.
* **🌳 Categorized Family Tree Browser**: Hierarchical selection by `Category ➔ Family ➔ Type ➔ Element` with real-time search filtering by family name or element ID.
* **🟦 Blue-Highlight Interactive Multi-Select**: Pick multiple elements directly in active 3D or plan views. Every hovered element highlights in bright blue on hover and selection.
* **🎯 100% Geospatial Origin Alignment**: Automatically applies total link transforms (`linkInstance.GetTotalTransform()`) to ensure copied elements land in exact host coordinates.
* **🛡️ Automatic Background Handling**: Automatically imports required family type definitions and prevents duplicate element IDs without manual setup.
* **🌐 Multi-Version Compatibility**: Supports Autodesk® Revit® versions 2019, 2020, 2021, 2022, 2023, 2024, 2025, 2026, and 2027 out of the box.

---

## 📥 Direct Download & Installation

### Option 1: Standalone Windows Setup (.exe)
Download the unified setup installer to automatically deploy Bplus Link Copier across all installed Revit versions:

👉 **[Direct Installer Download (BplusLinkCopier_Full_Setup_2019-2027.exe)](https://raw.githubusercontent.com/infoatbplus-BIM/BPlus_Link_Copier/main/BplusLinkCopier_Full_Setup_2019-2027.exe)**

### Option 2: Manual Add-in Installation
Copy the `.addin` manifest and assembly files to your Revit add-in directory:
`%APPDATA%\Autodesk\Revit\Addins\<Year>\`

---

## 💻 Building from Source

### Prerequisites
* Visual Studio 2022 (with .NET Desktop Development & .NET Framework 4.8 targeting packs)
* Autodesk Revit API DLLs (referenced automatically via NuGet package `Revit_All_Main_Versions_API_x64`)
* Inno Setup 6 (optional, for compiling the installer script)

### Build Commands
```bash
# Build for Revit 2025 - 2027 (.NET 8.0 & 10.0)
dotnet build Bplus.LinkCopier.csproj -c Release

# Build for Revit 2019 - 2024 (.NET 4.8)
dotnet build Bplus.LinkCopier.net48.csproj -c Release
```

---

## 🏢 About B Plus Pvt Ltd

**B Plus Pvt Ltd — Plus preparing for tomorrow**  
We specialize in building custom Autodesk Revit add-ins, BIM computational tools, and model automation software for engineering and architectural firms worldwide.

* **Official Website**: [https://bplus.lk/](https://bplus.lk/)
* **Custom Tool Inquiries**: Visit [bplus.lk](https://bplus.lk/) to request custom C# Revit plugin development.

---

## 📄 License
This project is licensed under the [MIT License](LICENSE).
