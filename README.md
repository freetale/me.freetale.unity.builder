# me.freetale.unity.builder
helper for cli build outside unity ecosystem 

[![openupm](https://img.shields.io/npm/v/me.freetale.unity.builder?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/me.freetale.unity.builder/)


# Feature
- [x] build from outside unity
- [x] yaml support (partial)
- [x] unity project wide configuration
- [x] support openupm
- [ ] support bash script
- [x] config project as target in editor (for debugging target)
- [ ] build as target in editor (for debugging target)
- [x] script define symbols
- [ ] set field from command line
- [x] leave no trace in build

# Prerequisite
- Unity (with license activated)
- powershell

on windows it may need to change ExecutionPolicy, default not allow run ps1 script
```
Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Scope CurrentUser
```

add this line `.gitignore`
```
/[Cc]ache/
```
# Installation

## a. via package.json

insert dependencies to Packages/manifest.json
```json
{
  "dependencies": {
    ...
    "me.freetale.unity.builder": "https://github.com/freetale/me.freetale.unity.builder.git?path=Assets/me.freetale.unity.builder"
  }
}
```

## b. via openupm

```
openupm add me.freetale.unity.builder
```

# Usage

Create file `BuildConfig.json` in project root
```json
{
  "Targets": [
    {
      "Name": "default",
      "BuildPlayerOptions": {
        "locationPathName": "Builds/windows/windows.exe",
        "target": "StandaloneWindows",
        "targetGroup": "Standalone"
      },
      "StaticProperties": {
        "UnityEditor.PlayerSettings,UnityEditor": {
          "bundleVersion": "0.2"
        }
      },
      "ScriptingDefineSymbols": [
        "DEFINE_A",
        "DEFINE_B"
      ]
    }
  ]
}
```

Powershell
```ps1
$UNITY_EDITOR_PATH="C:/Program Files/Unity/2019.4.24f1/Editor/Unity.exe" #(Require) Path to unity installation
$BUILD_TARGET="Standalone" #(Recommend) unity startup build target, https://docs.unity3d.com/Manual/CommandLineArguments.html
$PROJECT_PATH=${PWD} #(Optional) default to ${PWD}
$TARGET="default" # target to build, default to "default"
./Library/PackageCache/me.freetale.unity.builder@1.0.0/Script/lib.ps1 # may change by version number
```

# Configuration

BuildConfig.json require only Target[] object

### Targets
list of define target, when build we need choose one from this list. each target contains 4 fields.
- `Name` name of target we select from script
- `BuildPlayerOptions` options pass to `BuildPlayerPipeline` [referance](https://docs.unity3d.com/ScriptReference/BuildPlayerOptions.html)
- `StaticProperties` static property need to set before build, like `PlayerSettings.keystorePass` which does not remember by unity, name must be [AssemblyQualifiedName](https://docs.microsoft.com/en-us/dotnet/api/system.type.assemblyqualifiedname?view=net-5.0#System_Type_AssemblyQualifiedName) like `UnityEditor.PlayerSettings+Android,UnityEditor`
- `ScriptingDefineSymbols` list of define directive as string
> :warning: Store password in plain text is insecure.

## Yaml
this library not support yaml directly but use [yq](https://github.com/mikefarah/yq) for convert yaml to json configuration
```
yq eval --tojson BuildConfig.yaml > BuildConfig.json
```

# Apply Target config

GUI Locate in unity editor
Window > Build Target

if hit on target button, it will apply to project. useful for debigging.
