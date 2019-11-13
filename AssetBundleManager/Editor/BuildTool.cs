using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using System.IO;

public class BuildTool : MonoBehaviour
{

    [MenuItem("ProTool/Build/CopyBaseStreamAssets")]
    public static void CopyStreamAssets()
    {
        //clear streamAsset
        ClearStreamAssets();

        //baseAsset
        string baseAssetPath = Application.dataPath + "/Editor/BuildTool/BaseAssets.txt";
        StreamReader baseAssetRead = new StreamReader(baseAssetPath);
        List<string> baseAssets = new List<string>();
        while (!baseAssetRead.EndOfStream)
        {
            baseAssets.Add(baseAssetRead.ReadLine());
        }

        //copy
        foreach (var baseAsset in baseAssets)
        {
            string ortAssetPath = Application.dataPath.Replace("Assets", "AssetBundles/") + AssetBundles.Utility.GetPlatformName() + "/" + baseAsset;
            string tarAssetPath = Application.streamingAssetsPath + "/" + baseAsset;

            string tarAssetFold = Path.GetDirectoryName(tarAssetPath);

            if (!Directory.Exists(tarAssetFold))
            {
                Directory.CreateDirectory(tarAssetFold);
            }

            File.Copy(ortAssetPath, tarAssetPath);
        }

        AssetDatabase.Refresh();

    }

    [MenuItem("ProTool/Build/CopyAllStreamAssets")]
    public static void CopyAllAssets()
    {
        ClearStreamAssets();

        string orgAssetFold = Application.dataPath.Replace("Assets", "AssetBundles/") + AssetBundles.Utility.GetPlatformName();
        var files = Directory.GetFiles(orgAssetFold, "*.*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            string tarAssetPath = file.Replace("AssetBundles/" + AssetBundles.Utility.GetPlatformName(), "Assets/StreamingAssets");
            Debug.Log("tarAssetPath:" + tarAssetPath);

            string tarAssetFold = Path.GetDirectoryName(tarAssetPath);

            if (!Directory.Exists(tarAssetFold))
            {
                Directory.CreateDirectory(tarAssetFold);
            }

            if (file.EndsWith(".common"))
            {
                var fileBytes = BundleEncryption.Encryption(file);
                File.WriteAllBytes(tarAssetPath, fileBytes);
            }
            else
            {

                File.Copy(file, tarAssetPath);
            }
        }
    }

    [MenuItem("ProTool/Build/CopyAllWWW")]
    public static void CopyAllWWW()
    {
        ClearWWWAssets();

        string orgAssetFold = Application.dataPath.Replace("Assets", "AssetBundles/") + AssetBundles.Utility.GetPlatformName();
        var files = Directory.GetFiles(orgAssetFold, "*.*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            string tarAssetPath = file.Replace(orgAssetFold, "D:/project/Assets/3DMMO/Res/" + AssetBundles.Utility.GetPlatformName());
            Debug.Log("tarAssetPath:" + tarAssetPath);

            string tarAssetFold = Path.GetDirectoryName(tarAssetPath);

            if (!Directory.Exists(tarAssetFold))
            {
                Directory.CreateDirectory(tarAssetFold);
            }

            File.Copy(file, tarAssetPath);
        }
    }

    #region private

    public static void ClearStreamAssets()
    {
        //clear streamAsset
        try
        {
            var directs = Directory.GetDirectories(Application.streamingAssetsPath, "*", SearchOption.AllDirectories);
            foreach (var direct in directs)
            {
                Directory.Delete(direct, true);
            }

            var files = Directory.GetFiles(Application.streamingAssetsPath, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("clear stream error:" + e.ToString());
        }
    }

    public static void ClearWWWAssets()
    {
        string wwwPath = Application.dataPath + "/3DMMO/Res/" + AssetBundles.Utility.GetPlatformName();
        //clear streamAsset
        var directs = Directory.GetDirectories(wwwPath, "*", SearchOption.AllDirectories);
        foreach (var direct in directs)
        {
            Directory.Delete(direct, true);
        }

        var files = Directory.GetFiles(wwwPath, "*.*", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
        {
            File.Delete(file);
        }
    }

    #endregion
}
