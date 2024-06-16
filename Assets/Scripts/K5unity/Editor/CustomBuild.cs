using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace K5unity.Editor
{
    public static class CustomBuild
    {
        // Common
        private const string SymbolArg = "-defineSymbols";

        // Android(Google)
        private const string KeystoreNameArg = "-keystoreName";
        private const string KeystorePassArg = "-keystorePass";
        private const string KeyAliasNameArg = "-keyAliasName";
        private const string KeyAliasPassArg = "-keyAliasPass";

        // iOS(Apple)
        private const string ProvisioningProfileIdArg = "-provisioningProfileId";
        private const string AppleDeveloperTeamIdArg = "-appleDeveloperTeamId";
    
        private static string[] GetScenesAll() => (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToArray();
        
        private static string GetArgByName(string argName) => Environment.GetCommandLineArgs()
            .SkipWhile(arg => arg != argName)
            .Skip(1)
            .FirstOrDefault();

        private static void Prepare(BuildTargetGroup buildTarget)
        {
            var cliArgSymbolString = GetArgByName(SymbolArg);
            if (string.IsNullOrEmpty(cliArgSymbolString))
            {
                Debug.LogWarning($"Command line argument {SymbolArg} not set");
            }
            else
            {
                Debug.Log($">>> Set Scripting Define Symbols from command line parameter: {cliArgSymbolString}");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, cliArgSymbolString);
            }

            Debug.Log($"[{buildTarget.ToString()}] preparation completed");
        }

        private static void PrintReport(BuildReport report)
        {
            Debug.Log(report.files
                .Select(file => $"[{file.id}]\t{file.role}\t{file.size}\t{file.path}\n")
                .Aggregate(new StringBuilder($">>> Build Files Report:\n"), (stringBuilder, logString) => stringBuilder.Append(logString))
                .ToString()
            );

            var resultLog = $"[{report.summary.platform.ToString()}] Build result: {report.summary.result.ToString()} (error count = {report.summary.totalErrors})";
            switch (report.summary.result)
            {
                case BuildResult.Failed:
                    Debug.LogError(resultLog);
                    EditorApplication.Exit(1);
                    break;
                case BuildResult.Succeeded:
                    Debug.Log(resultLog);
                    EditorApplication.Exit(0);
                    break;
                case BuildResult.Unknown:
                case BuildResult.Cancelled:
                default:
                    Debug.LogWarning(resultLog);
                    EditorApplication.Exit(2);
                    break;
            }
        }
        
        [MenuItem("Build/Android/1. Prepare")]
        public static void PrepareForGoogle() => Prepare(BuildTargetGroup.Android);
        
        [MenuItem("Build/Android/2. Build")]
        public static void BuildForGoogle()
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
            EditorUserBuildSettings.buildAppBundle = true;
            PlayerSettings.applicationIdentifier = CustomBuildConfig.BundleIdentifierGoogle;

            var keystoreName = GetArgByName(KeystoreNameArg);
            var keystorePass = GetArgByName(KeystorePassArg);
            var keyAliasName = GetArgByName(KeyAliasNameArg);
            var keyAliasPass = GetArgByName(KeyAliasPassArg);
            if (string.IsNullOrEmpty(keystoreName) || string.IsNullOrEmpty(keystorePass) ||
                string.IsNullOrEmpty(keyAliasName) || string.IsNullOrEmpty(keyAliasPass))
            {
                PlayerSettings.Android.useCustomKeystore = false;
            }
            else
            {
                PlayerSettings.Android.useCustomKeystore = true;
                PlayerSettings.Android.keystoreName = keystoreName;
                PlayerSettings.Android.keystorePass = keystorePass;
                PlayerSettings.Android.keyaliasName = keyAliasName;
                PlayerSettings.Android.keyaliasPass = keyAliasPass;
            }

            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            
            PlayerSettings.Android.useAPKExpansionFiles = true;
            PlayerSettings.Android.bundleVersionCode = CustomBuildConfig.BundleVersionCode;
            PlayerSettings.Android.minifyDebug = true;
            PlayerSettings.Android.minifyRelease = true;
            PlayerSettings.Android.minifyWithR8 = true;

            PlayerSettings.bundleVersion = CustomBuildConfig.BundleVersion;
            PlayerSettings.productName = CustomBuildConfig.ProductName;
            PlayerSettings.statusBarHidden = true;
            PlayerSettings.SplashScreen.show = false;

            const BuildOptions buildOptions = BuildOptions.CompressWithLz4;
            var report = BuildPipeline.BuildPlayer(GetScenesAll(), "binary.aab", BuildTarget.Android, buildOptions);
            PrintReport(report);
        }

        [MenuItem("Build/iOS/1. Prepare")]
        public static void PrepareForApple() => Prepare(BuildTargetGroup.iOS);
        
        [MenuItem("Build/iOS/2. Build")]
        public static void BuildForApple()
        {
            Prepare(BuildTargetGroup.iOS);

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
            PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
            PlayerSettings.iOS.buildNumber = CustomBuildConfig.BundleBuildNumber;

            PlayerSettings.iOS.appleEnableAutomaticSigning = false;

            var provisioningProfileId = GetArgByName(ProvisioningProfileIdArg);
            if (!string.IsNullOrEmpty(provisioningProfileId))
            {
                PlayerSettings.iOS.iOSManualProvisioningProfileID = provisioningProfileId;
                Debug.Log($">>> iOS Manual Provisioning Profile ID = {PlayerSettings.iOS.iOSManualProvisioningProfileID}");
            }

            var appleDeveloperTeamId = GetArgByName(AppleDeveloperTeamIdArg);
            if (!string.IsNullOrEmpty(appleDeveloperTeamId))
            {
                PlayerSettings.iOS.appleDeveloperTeamID = appleDeveloperTeamId;
                Debug.Log($">>> Apple Developer Team ID = {PlayerSettings.iOS.appleDeveloperTeamID}");
            }

            PlayerSettings.applicationIdentifier = CustomBuildConfig.BundleIdentifierApple;
            PlayerSettings.bundleVersion = CustomBuildConfig.BundleVersion;
            PlayerSettings.productName = CustomBuildConfig.ProductName;
            PlayerSettings.statusBarHidden = true;
            PlayerSettings.SplashScreen.show = false;

            const BuildOptions buildOptions = BuildOptions.None;
            var report = BuildPipeline.BuildPlayer(GetScenesAll(), "XcodeProject", BuildTarget.iOS, buildOptions);
            PrintReport(report);
        }
    } // end of class
} // end of namespace
