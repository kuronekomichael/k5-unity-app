using UnityEngine;

namespace K5unity.Editor
{
    public static class CustomBuildConfig
    {
        public const string BundleVersion = "0.0.1";
        public const int BundleVersionCode = 1;
        public static string BundleBuildNumber => BundleVersionCode.ToString();

        public const string ProductName = "K5";
        public const string BundleIdentifierGoogle = "io.tass.casual.vd.k5unity";
        public const string BundleIdentifierApple = "io.tass.casual.vd.k5unity";
        public const string AndroidKeystoreName = "build.keystore";
    }
}
