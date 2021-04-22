

function Write-Text {
    param (
        $Message
    )
    $CONOLSE_COLOR="$([char]27)[30m$([char]27)[42m"
    $CONSOLE_RESTORE="$([char]27)[0m"
    echo "${CONOLSE_COLOR}${Message}${CONSOLE_RESTORE}"
}

if ($UNITY_EDITOR_PATH -eq $null) {
    Write-Text "missing UNITY_EDITOR_PATH"
    exit(1)
}

if ($PROJECT_PATH -eq $null) {
    $PROJECT_PATH=${PWD}
}

$ARGS = @()
if ($BUILD_TARGET -eq $null) {
    Write-Text "BUILD_TARGET not define, ignore"
    if ($CACHE_NAME -eq $null -and $CACHE_PATH -eq $null) {
        Write-Text "missing CACHE_NAME and BUILD_TARGET"
        exit(1)
    }
} else {
    #build target may help with unity startup time
    $ARGS = $ARGS + "-buildTarget" + $BUILD_TARGET
    if ($CACHE_NAME -eq $null) {
        $CACHE_NAME="Cache/$BUILD_TARGET"
        Write-Text "missing CACHE_NAME set as ${BUILD_TARGET}"
    }
}

if (-not $($TARGET -eq $null)){
    $ARGS = $ARGS + "--target=${TARGET}"
}

if ($CACHE_PATH -eq $null) {
    $CACHE_PATH="${PROJECT_PATH}/$CACHE_NAME"
}

if ($TEMP_CACHE_PATH -eq $null) {
    $TEMP_CACHE_PATH="${PROJECT_PATH}/Cache/Temp"
}

if (-not $(Test-Path "${PROJECT_PATH}/Assets")) {
    Write-Text "Assets path not exist, are PROJECT_PATH point to project root? current is ${PROJECT_PATH}"
    exit(1)
}

Write-Text "swaping library"
if (-not $(Test-Path "${PROJECT_PATH}/Cache")) {
    mkdir "${PROJECT_PATH}/Cache"
}

mv "${PROJECT_PATH}/Library" "$TEMP_CACHE_PATH" -ErrorAction SilentlyContinue
mv "${CACHE_PATH}" "${PROJECT_PATH}/Library" -ErrorAction SilentlyContinue

Write-Text "Executing unity editor"
&$UNITY_EDITOR_PATH -quit -batchmode -executeMethod "FreeTale.Unity.Builder.Builder.BuildMain" -logFile - -projectPath "$PROJECT_PATH" $ARGS ${EXTRA_ARGS} | Out-Host
$UNITY_EXIT_CODE="$LASTEXITCODE"

Write-Text "swaping library back"
mv "${PROJECT_PATH}/Library" "${CACHE_PATH}" -ErrorAction SilentlyContinue
mv "$TEMP_CACHE_PATH" "${PROJECT_PATH}/Library" -ErrorAction SilentlyContinue

Write-Text "finalized build with exit code ${UNITY_EXIT_CODE}"
