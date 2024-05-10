using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundle
{
    [MenuItem("Assets/Build Bundle")]
    private static void BuildBundles()
    {
        var assetBundlePath = Application.dataPath + "/../AssetBundle";
        Directory.CreateDirectory(assetBundlePath);
        BuildPipeline.BuildAssetBundles(assetBundlePath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
