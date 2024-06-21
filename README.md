k5-unity-app
==============

[![Build unity app for development](https://github.com/kuronekomichael/k5-unity-app/actions/workflows/development-build.yml/badge.svg)](https://github.com/kuronekomichael/k5-unity-app/actions/workflows/development-build.yml)
[![Build unity app for production](https://github.com/kuronekomichael/k5-unity-app/actions/workflows/production-build.yml/badge.svg)](https://github.com/kuronekomichael/k5-unity-app/actions/workflows/production-build.yml)
[![Android](https://github.com/kuronekomichael/k5-unity-app/actions/workflows/_android.yml/badge.svg)](https://github.com/kuronekomichael/k5-unity-app/actions/workflows/_android.yml)
[![iOS](https://github.com/kuronekomichael/k5-unity-app/actions/workflows/_ios.yml/badge.svg)](https://github.com/kuronekomichael/k5-unity-app/actions/workflows/_ios.yml)

Sample application for Unity/C#

## CLI Build

```sh
# Common
UNITY_CLI_VERSION="2021.3.36f1"
PROJECT_PATH="/path/to/k5-unity-app"
SYMBOLS="SYM_A;SYM_B;SYM_C"
```

### Android

```sh
KEYSTORE_FILEPATH="/path/to/dev.keystore"
KEYSTORE_PASSWORD="xxxxxxx"
KEYALIAS_NAME="yyyyyy"
KEYALIAS_PASSWORD="zzzzzzz"


/Applications/Unity/Hub/Editor/${UNITY_CLI_VERSION}/Unity.app/Contents/MacOS/Unity \
  -quit \
  -batchmode -executeMethod K5unity.Editor.CustomBuild.PrepareForGoogle \
  -logFile ${PROJECT_PATH}/1_prepare_for_google.log \
  -projectPath ${PROJECT_PATH} \
  -keystoreName $KEYSTORE_FILEPATH \
  -keystorePass $KEYSTORE_PASSWORD \
  -keyAliasName $KEYALIAS_NAME \
  -keyAliasPass $KEYALIAS_PASSWORD \
  -defineSymbols ${SYMBOLS}

/Applications/Unity/Hub/Editor/${UNITY_CLI_VERSION}/Unity.app/Contents/MacOS/Unity \
  -quit \
  -batchmode -executeMethod K5unity.Editor.CustomBuild.BuildForGoogle \
  -logFile ${PROJECT_PATH}/2_build_for_google.log \
  -projectPath ${PROJECT_PATH} \
  -keystoreName $KEYSTORE_FILEPATH \
  -keystorePass $KEYSTORE_PASSWORD \
  -keyAliasName $KEYALIAS_NAME \
  -keyAliasPass $KEYALIAS_PASSWORD \
  -defineSymbols ${SYMBOLS}

cat ${PROJECT_PATH}/2_build_for_google.log | grep "Build result"
cat ${PROJECT_PATH}/2_build_for_google.log | grep -v '> Task :' | grep -v '> Configure project :' | grep -E "^\s*> "
```

### iOS

```sh
PROVISONING_PROFILE_ID="abcdefg-1234-abcded-5678-abcdefg012345"
APPLE_DEVELOPER_TEAM_ID="1234ABCD"

/Applications/Unity/Hub/Editor/${UNITY_CLI_VERSION}/Unity.app/Contents/MacOS/Unity \
  -quit \
  -batchmode -executeMethod K5unity.Editor.CustomBuild.PrepareForApple \
  -logFile ${PROJECT_PATH}/1_prepare_for_apple.log \
  -projectPath ${PROJECT_PATH} \
  -defineSymbols ${SYMBOLS}

/Applications/Unity/Hub/Editor/${UNITY_CLI_VERSION}/Unity.app/Contents/MacOS/Unity \
  -quit \
  -batchmode -executeMethod K5unity.Editor.CustomBuild.BuildForApple \
  -logFile ${PROJECT_PATH}/2_build_for_apple.log \
  -projectPath ${PROJECT_PATH} \
  -provisioningProfileId ${PROVISONING_PROFILE_ID} \
  -appleDeveloperTeamId ${APPLE_DEVELOPER_TEAM_ID} \
  -defineSymbols ${SYMBOLS}
```
