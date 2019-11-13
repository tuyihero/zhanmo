using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class ClassifyBundles : MonoBehaviour
{


    #region 

    public static void AddResourceDict(Dictionary<string, string> resources, string path, string bundleName)
    {
        string resAssetPath = path.Replace(Application.dataPath, "Assets").Replace("\\", "/");
        if (!resources.ContainsKey(resAssetPath))
        {
            resources.Add(resAssetPath, bundleName);
        }
    }

    public static void SetResourceBundleName(Dictionary<string, string> resources)
    {
        string[] bundleNames = AssetDatabase.GetAllAssetBundleNames();
        foreach (var bundleName in bundleNames)
        {
            //if (bundleName == ("ui/countdown.common"))
            {
                string[] bundleAssets = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);
                foreach (var bundleAsset in bundleAssets)
                {
                    var prefabImporter = AssetImporter.GetAtPath(bundleAsset);
                    prefabImporter.assetBundleVariant = "";
                    prefabImporter.assetBundleName = "";
                }
            }
        }

        Dictionary<string, string> dependBundles = new Dictionary<string, string>();
        foreach (var res in resources)
        {
            string resAssetPath = res.Key;
            var prefabImporter = AssetImporter.GetAtPath(resAssetPath);
            if (prefabImporter != null)
            {
                prefabImporter.assetBundleName = res.Value;
                prefabImporter.assetBundleVariant = "common";
            }
            Debug.Log("SetResourceBundleName:" + res.Value + "," + resAssetPath);

            Object assetData = AssetDatabase.LoadAssetAtPath<Object>(resAssetPath);
            Object[] dependObjs = EditorUtility.CollectDependencies(new Object[1] { assetData });
            foreach (var dependObj in dependObjs)
            {
                if (dependObj == null)
                    continue;

                if (dependObj is UnityEngine.Mesh
                        || dependObj is Texture2D
                        || dependObj is Material
                        || dependObj is Animation
                        || dependObj is AudioClip
                        || dependObj is GameObject
                        || dependObj is Shader
                        )
                {
                    string dependObjPath = AssetDatabase.GetAssetPath(dependObj);
                    if (dependObjPath.Contains("unity default resources"))
                        continue;

                    if (dependObjPath.StartsWith("Resources/"))
                        continue;

                    string dependObjObsorbPath = dependObjPath;
                    if (resources.ContainsKey(dependObjObsorbPath))
                        continue;

                    if (dependObj == assetData)
                        continue;

                    if (!dependBundles.ContainsKey(dependObjPath))
                    {
                        dependBundles.Add(dependObjPath, "");
                    }
                    dependBundles[dependObjPath] += res.Value;

                    if (dependObj is Shader)
                    {
                        dependBundles[dependObjPath] = "Shader";
                    }
                }
            }
        }

        foreach (var dependBundle in dependBundles)
        {
            string dependBundleName = "Depend/Depend_" + EncryptWithMD5(dependBundle.Value);
            if (dependBundle.Value.Equals("Shader"))
            {
                dependBundleName = "Shader";
            }
            //Debug.Log("SetResourceBundleName:" + dependBundleName + "," + dependBundle.Key);
            var prefabImporter = AssetImporter.GetAtPath(dependBundle.Key);
            if (prefabImporter != null)
            {
                prefabImporter.assetBundleName = dependBundleName;
                prefabImporter.assetBundleVariant = "common";
            }
        }


    }

    public static void ClassifyScene(Dictionary<string, string> resBundels)
    {
        string sysPath = Application.dataPath + "/FightCraft/Res/Scenes";
        string[] filePaths = Directory.GetFiles(sysPath, "*.unity", SearchOption.AllDirectories);
        Dictionary<string, string> sceneDict = new Dictionary<string, string>();
        foreach (var sceneFile in filePaths)
        {
            string sceneName = Path.GetFileNameWithoutExtension(sceneFile);
            sceneDict.Add(sceneName, sceneFile);
        }

        foreach (var stageInfo in Tables.TableReader.StageInfo.Records.Values)
        {
            string sceneLogicPath = Application.dataPath + "FightCraft/Resources/FightSceneLogic/" + stageInfo.FightLogicPath;
            string sceneLogicBundleName = "FightSceneLogic/" + stageInfo.FightLogicPath;
            AddResourceDict(resBundels, sceneLogicPath, sceneLogicBundleName);

            foreach (var sceneName in stageInfo.ScenePath)
            {
                if (string.IsNullOrEmpty(sceneName))
                    continue;

                if (sceneDict.ContainsKey(sceneName))
                {
                    AddResourceDict(resBundels, sceneDict[sceneName], "Scene/" + sceneName);
                }
            }
        }

        foreach (var sceneName in FightSceneLogicRandomArea._ShaMoScene)
        {
            if (string.IsNullOrEmpty(sceneName))
                continue;

            if (sceneDict.ContainsKey(sceneName))
            {
                AddResourceDict(resBundels, sceneDict[sceneName], "Scene/" + sceneName);
            }
        }

        foreach (var sceneName in FightSceneLogicRandomArea._CaoYuanScene)
        {
            if (string.IsNullOrEmpty(sceneName))
                continue;

            if (sceneDict.ContainsKey(sceneName))
            {
                AddResourceDict(resBundels, sceneDict[sceneName], "Scene/" + sceneName);
            }
        }

        foreach (var sceneName in FightSceneLogicRandomArea._BingYuanScene)
        {
            if (string.IsNullOrEmpty(sceneName))
                continue;

            if (sceneDict.ContainsKey(sceneName))
            {
                AddResourceDict(resBundels, sceneDict[sceneName], "Scene/" + sceneName);
            }
        }

        foreach (var sceneName in FightSceneLogicRandomArea._DiChengScene)
        {
            if (string.IsNullOrEmpty(sceneName))
                continue;

            if (sceneDict.ContainsKey(sceneName))
            {
                AddResourceDict(resBundels, sceneDict[sceneName], "Scene/" + sceneName);
            }
        }

        foreach (var sceneName in FightSceneLogicRandomArea._BossScene)
        {
            if (string.IsNullOrEmpty(sceneName))
                continue;

            if (sceneDict.ContainsKey(sceneName))
            {
                AddResourceDict(resBundels, sceneDict[sceneName], "Scene/" + sceneName);
            }
        }
    }

    public static void ClassifyAssets(Dictionary<string, string> resBundels, string fold)
    {
        string sysPath = Application.dataPath + "/FightCraft/BundleAssets/" + fold;
        string[] filePaths = Directory.GetFiles(sysPath, "*.*", SearchOption.AllDirectories);
        
        foreach (var sceneFile in filePaths)
        {
            if (sceneFile.EndsWith(".meta")
                || sceneFile.EndsWith(".shader"))
            {
                continue;
            }
            string bundleName = sceneFile.Replace(sysPath, "Asset/" + fold);
            bundleName = bundleName.Replace(Path.GetExtension(bundleName), "");
            //resBundels.Add(sceneFile, bundleName);
            AddResourceDict(resBundels, sceneFile, bundleName);
        }
    }

    public static void ClassifyUI(Dictionary<string, string> resBundels)
    {
        var uiPathInfos = typeof(UIConfig).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        foreach (var uiInfo in uiPathInfos)
        {
            if (uiInfo.FieldType != typeof(AssetInfo))
                continue;

            var uipathInfo = uiInfo.GetValue(null) as AssetInfo;
            string uipath = Application.dataPath + "/FightCraft/BundleAssets/UI/" + uipathInfo.AssetPath + ".prefab";
            //resBundels.Add(uipath, "UI" + uipathInfo.AssetPath);
            AddResourceDict(resBundels, uipath, "UI/" + uipathInfo.AssetPath);
        }
    }

    [MenuItem("ProTool/ClassifyRes/ClassifyAllBundles")]
    public static void ClassifyAllBundles()
    {
        Tables.TableReader.ReadTables();

        Dictionary<string, string> resBundles = new Dictionary<string, string>();
        ClassifyScene(resBundles);
        ClassifyAssets(resBundles, "Animation");
        ClassifyAssets(resBundles, "Audios");
        ClassifyAssets(resBundles, "Bullet");
        ClassifyAssets(resBundles, "Common");
        ClassifyAssets(resBundles, "Effect");
        ClassifyAssets(resBundles, "Model");
        ClassifyAssets(resBundles, "ModelBase");
        ClassifyAssets(resBundles, "SkillMotion");
        ClassifyAssets(resBundles, "Drop");
        ClassifyAssets(resBundles, "FightSceneLogic");
        ClassifyUI(resBundles);

        SetResourceBundleName(resBundles);
        AssetDatabase.Refresh();
    }

    public static string EncryptWithMD5(string source)
    {
        byte[] sor = Encoding.UTF8.GetBytes(source);
        MD5 md5 = MD5.Create();
        byte[] result = md5.ComputeHash(sor);
        StringBuilder strbul = new StringBuilder(40);
        for (int i = 0; i < result.Length; i++)
        {
            strbul.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

        }
        return strbul.ToString();
    }
    #endregion
}
