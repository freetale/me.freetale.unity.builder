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

# Yaml
this library not support yaml directly but use [yq](https://github.com/mikefarah/yq) for convert yaml to json configuration
```
yq eval --tojson Examples/BuildConfig.yaml > Examples/BuildConfig.json
```

# TODO
[x] build from outside unity
[x] yaml support (partial)
[ ] unity project wide configuration
[ ] support openupm
[ ] support bash script