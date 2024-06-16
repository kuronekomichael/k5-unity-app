using UnityEngine;

namespace K5unity.Editor
{
    public static class CustomBuildConfig
    {
        public const string BundleVersion = "0.0.2";
        public const int BundleVersionCode = 2;
        public static string BundleBuildNumber => BundleVersionCode.ToString();

        public const string ProductName = "K5";
        public const string BundleIdentifierGoogle = "io.tass.casual.vd.k5unity";
        public const string BundleIdentifierApple = "io.tass.casual.vd.k5unity";
    }
}
