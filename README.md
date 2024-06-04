k5-unity-app
==============

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
KEYSTORE_PASS="sample"
KEY_ALIAS_NAME="debug"
KEY_ALIAS_PASS="sample"

/Applications/Unity/Hub/Editor/${UNITY_CLI_VERSION}/Unity.app/Contents/MacOS/Unity \
  -quit \
  -batchmode -executeMethod K5unity.Editor.CustomBuild.PrepareForGoogle \
  -logFile ${PROJECT_PATH}/1_prepare_for_google.log \
  -projectPath ${PROJECT_PATH} \
  -defineSymbols ${SYMBOLS}

/Applications/Unity/Hub/Editor/${UNITY_CLI_VERSION}/Unity.app/Contents/MacOS/Unity \
  -quit \
  -batchmode -executeMethod K5unity.Editor.CustomBuild.BuildForGoogle \
  -logFile ${PROJECT_PATH}/2_build_for_google.log \
  -projectPath ${PROJECT_PATH} \
  -keystorePass ${KEYSTORE_PASS} \
  -keyAliasName ${KEY_ALIAS_NAME} \
  -keyAliasPass ${KEY_ALIAS_PASS} \
  -defineSymbols ${SYMBOLS}
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