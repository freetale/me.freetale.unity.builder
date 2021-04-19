# me.freetale.unity.builder
helper for automation build outside unity ecosystem

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
# Usage
TODO: introduce usage with openupm and unity package manager

Powershell
```ps1
$UNITY_EDITOR_PATH="C:\Program Files\Unity\2019.4.24f1\Editor\Unity.exe" #Path to unity installation
$BUILD_TARGET="Standalone" #(Recommend) unity startup build target, https://docs.unity3d.com/Manual/CommandLineArguments.html
$PROJECT_PATH=${PWD} #(Optional) default to ${PWD}
$EXTRA_ARGS="--config=Examples/BuildConfig.json" #(Require in tutorial) change where BuildConfig.json locate
$TARGET="windows" # target to build,
.\Examples\lib.ps1
```

# Configuration

BuildConfig.json look like
```json
{
  "Targets": [
    {
      "Name": "android-dev",
      "BuildPlayerOptions": {
        "locationPathName": "Builds/android/android.apk",
        "target": "Android",
        "targetGroup": "Android",
        "options": [
          "IL2CPP",
          "Development"
        ]
      },
      "StaticProperties": {
        "UnityEditor.PlayerSettings,UnityEditor": {
          "keystorePass": "my-password",
          "keyaliasPass": "my-password"
        }
      }
    }
  ]
}
```
### Targets
list of define target, when build we need choose one from this list. each target contains 3 fields.
- `Name` name of target we select from script
- `BuildPlayerOptions` options pass to `BuildPlayerPipeline` [referance](https://docs.unity3d.com/ScriptReference/BuildPlayerOptions.html)
- `StaticProperties` static property need to set before build, like `PlayerSettings.keystorePass` which does not remember by unity, name must be [AssemblyQualifiedName](https://docs.microsoft.com/en-us/dotnet/api/system.type.assemblyqualifiedname?view=net-5.0#System_Type_AssemblyQualifiedName) like `UnityEditor.PlayerSettings+Android,UnityEditor`

> :warning: Store password in plain text is insecure.

## Yaml
this library not support yaml directly but use [yq](https://github.com/mikefarah/yq) for convert yaml to json configuration
```
yq eval --tojson Examples/BuildConfig.yaml > Examples/BuildConfig.json
```

# TODO
- [x] build from outside unity
- [x] yaml support (partial)
- [ ] unity project wide configuration
- [ ] support openupm
- [ ] support bash script