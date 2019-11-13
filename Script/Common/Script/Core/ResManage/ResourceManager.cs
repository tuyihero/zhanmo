using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundles;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResourceManager:MonoBehaviour
{
    #region 唯一

    public static bool _ResFromBundle = false;

    private static ResourceManager _Instance = null;
    public static ResourceManager Instance
    {
        get
        {
            return _Instance;
        }
    }

    public static void InitResourceManager()
    {
        GameObject resourceGo = new GameObject("ResourceManager");
        resourceGo.AddComponent<ResourceManager>();

        
    }

    void Start()
    {
        if (_ResFromBundle)
        {
            StartCoroutine(Initialize());
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            _Instance = this;
            gameObject.AddComponent<ResourcePool>();
        }
    }

    protected IEnumerator Initialize()
    {
        // Don't destroy the game object as we base on it to run the loading script.
        DontDestroyOnLoad(gameObject);

        InitializeSourceURL();

        // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
        var request = AssetBundleManager.Initialize();
        Debug.Log("Initialize:" + Time.time);
        if (request != null)
            yield return request;
        Debug.Log("Initialize 2:" + Time.time);
        _Instance = this;

        gameObject.AddComponent<ResourcePool>();
    }

    void InitializeSourceURL()
    {
        // If ODR is available and enabled, then use it and let Xcode handle download requests.
#if ENABLE_IOS_ON_DEMAND_RESOURCES
        if (UnityEngine.iOS.OnDemandResources.enabled)
        {
            AssetBundleManager.SetSourceAssetBundleURL("odr://");
            return;
        }
#endif
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        // With this code, when in-editor or using a development builds: Always use the AssetBundle Server
        // (This is very dependent on the production workflow of the project.
        //      Another approach would be to make this configurable in the standalone player.)
        //AssetBundleManager.SetDevelopmentAssetBundleServer();
        AssetBundleManager.SetSourceAssetBundleDirectory("");
        return;
#else
        // Use the following code if AssetBundles are embedded in the project for example via StreamingAssets folder etc:
        AssetBundleManager.SetSourceAssetBundleURL(Application.streamingAssetsPath + "/");
        // Or customize the URL based on your deployment or configuration
        //AssetBundleManager.SetSourceAssetBundleURL("http://www.MyWebsite/MyAssetBundles");
        return;
#endif
    }

    #endregion

    #region load from bundle

    protected IEnumerator InstantiateGameObjectAsync(string assetBundleName, string assetName, LoadBundleAssetCallback<GameObject> callBack, Hashtable param)
    {
        if (_ResFromBundle)
        {
            AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(GameObject));
            if (request == null)
            {
                Debug.LogError("Failed AssetBundleLoadAssetOperation on " + assetName + " from the AssetBundle " + assetBundleName + ".");
                yield break;
            }
            yield return StartCoroutine(request);

            GameObject prefab = request.GetAsset<GameObject>();
            var instanceGO = GameObject.Instantiate(prefab);

            if (callBack != null)
                callBack.Invoke(assetName, instanceGO, param);
        }
        else
        {
            Debug.Log("LoadPrefab:" + assetBundleName);
            GameObject prefab = Resources.Load<GameObject>(assetBundleName);
            var instanceGO = GameObject.Instantiate(prefab);

            if (callBack != null)
                callBack.Invoke(assetName, instanceGO, param);
        }
    }

    protected IEnumerator InstantiateAudioAsync(string assetBundleName, string assetName, LoadBundleAssetCallback<AudioClip> callBack, Hashtable param)
    {
        if (_ResFromBundle)
        {
            AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(AudioClip));
            if (request == null)
            {
                Debug.LogError("Failed AssetBundleLoadAssetOperation on " + assetName + " from the AssetBundle " + assetBundleName + ".");
                yield break;
            }
            yield return StartCoroutine(request);

            AudioClip resData = request.GetAsset<AudioClip>();

            if (callBack != null)
                callBack.Invoke(assetName, resData, param);
        }
        else
        {
            AudioClip resData = Resources.Load<AudioClip>(assetBundleName);

            if (callBack != null)
                callBack.Invoke(assetName, resData, param);
        }
    }

    protected IEnumerator InstantiateAnimationAsync(string assetBundleName, string assetName, LoadBundleAssetCallback<AnimationClip> callBack, Hashtable param)
    {
        if (_ResFromBundle)
        {
            AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(AnimationClip));
            if (request == null)
            {
                Debug.LogError("Failed AssetBundleLoadAssetOperation on " + assetName + " from the AssetBundle " + assetBundleName + ".");
                yield break;
            }
            yield return StartCoroutine(request);

            AnimationClip resData = request.GetAsset<AnimationClip>();

            if (callBack != null)
                callBack.Invoke(assetName, resData, param);
        }
        else
        {
            yield return null;
            AnimationClip resData = Resources.Load<AnimationClip>(assetBundleName);

            if (callBack != null)
                callBack.Invoke(assetName, resData, param);
        }
    }

    protected IEnumerator InstantiateSpriteAsync(string assetBundleName, string assetName, LoadBundleAssetCallback<Sprite> callBack, Hashtable param)
    {
        if (_ResFromBundle)
        {
            AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(Sprite));
            if (request == null)
            {
                Debug.LogError("Failed AssetBundleLoadAssetOperation on " + assetName + " from the AssetBundle " + assetBundleName + ".");
                yield break;
            }
            yield return StartCoroutine(request);

            Sprite resData = request.GetAsset<Sprite>();

            if (callBack != null)
                callBack.Invoke(assetName, resData, param);
        }
        else
        {
            Sprite resData = Resources.Load<Sprite>(assetBundleName);

            if (callBack != null)
                callBack.Invoke(assetName, resData, param);
        }
    }

    #endregion

    #region 

    public void LoadUI(string uiName, LoadBundleAssetCallback<GameObject> callBack, Hashtable param)
    {
        string bundleName = "UI/" + uiName;
        string assetName = System.IO.Path.GetFileNameWithoutExtension(bundleName);
        if (_ResFromBundle)
        {
            bundleName = bundleName + ".common";
            bundleName = bundleName.ToLower();
        }
        StartCoroutine(InstantiateGameObjectAsync(bundleName, assetName, callBack, param));
    }

    public void LoadPrefab(string prefabName, LoadBundleAssetCallback<GameObject> callBack, Hashtable param)
    {
        string bundleName = prefabName;
        if (_ResFromBundle)
        {
            bundleName = "Asset/" + prefabName + ".common";
            bundleName = bundleName.ToLower();
        }
        string assetName = System.IO.Path.GetFileNameWithoutExtension(bundleName);
        StartCoroutine(InstantiateGameObjectAsync(bundleName, assetName, callBack, param));
    }

    public IEnumerator LoadPrefab(string prefabName, LoadBundleAssetCallback<GameObject> callBack)
    {
        string bundleName = "Asset/" + prefabName + ".common";
        string assetName = System.IO.Path.GetFileNameWithoutExtension(bundleName);

        if (_ResFromBundle)
        {
            AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(bundleName.ToLower(), assetName, typeof(GameObject));
            if (request == null)
            {
                Debug.LogError("Failed AssetBundleLoadAssetOperation on " + assetName + " from the AssetBundle " + bundleName + ".");
                yield break;
            }
            yield return StartCoroutine(request);

            GameObject prefab = request.GetAsset<GameObject>();
            Debug.Log("LoadPrefab:" + prefabName);
            var instanceGO = GameObject.Instantiate(prefab);

            callBack.Invoke(assetName, instanceGO, null);
        }
        else
        {
            Debug.Log("LoadPrefab:" + bundleName);
            GameObject resData = Resources.Load<GameObject>(prefabName);
            var instanceGO = GameObject.Instantiate(resData);

            Debug.Log("LoadPrefab instanceGO:" + prefabName);
            if (callBack != null)
                callBack.Invoke(assetName, instanceGO, null);

            Debug.Log("motion child count 5:" + instanceGO.transform.childCount);
        }
    }

    public void LoadAudio(string prefabName, LoadBundleAssetCallback<AudioClip> callBack, Hashtable param)
    {
        string bundleName = "Audios/" + prefabName;
        if (_ResFromBundle)
        {
            bundleName = "Asset/" + bundleName + ".common";
            bundleName = bundleName.ToLower();
        }
        string assetName = System.IO.Path.GetFileNameWithoutExtension(bundleName);
        StartCoroutine(InstantiateAudioAsync(bundleName, assetName, callBack, param));
    }

    public void LoadAnimation(string prefabName, LoadBundleAssetCallback<AnimationClip> callBack, Hashtable param)
    {
        string bundleName = "Animation/" + prefabName;
        if (_ResFromBundle)
        {
            bundleName = "Asset/" + bundleName + ".common";
            bundleName = bundleName.ToLower();
        }
        string assetName = System.IO.Path.GetFileNameWithoutExtension(bundleName);
        StartCoroutine(InstantiateAnimationAsync(bundleName, assetName, callBack, param));
    }

    public IEnumerator LoadLevelAsync(string levelName, bool isAdditive)
    {
        Debug.Log("LoadLevelAsync:" + levelName);
        if (_ResFromBundle)
        {
            string sceneAssetBundle = "Scene/" + levelName + ".common";

            AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(sceneAssetBundle.ToLower(), levelName, isAdditive);
            if (request == null)
                yield break;
            yield return StartCoroutine(request);
        }
        else
        {
            yield return SceneManager.LoadSceneAsync(levelName, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        }
    }

    public void SetImage(Image image, string spriteName, LoadBundleAssetCallback<Sprite> callBack = null, Hashtable hash = null)
    {
        var valid = false;
        if (image != null)
        {
            if (image.sprite == null)
            {
                image.enabled = false;
                valid = true;
            }
            else
            {
                valid = !image.sprite.name.Equals(spriteName);
            }
        }
        if (!valid)
            return;

        string bundleName = "Icon/" + spriteName;
        if (_ResFromBundle)
        {
            bundleName = "Asset/" + bundleName + ".common";
            bundleName = bundleName.ToLower();
        }
        string assetName = System.IO.Path.GetFileNameWithoutExtension(bundleName);
        StartCoroutine(InstantiateSpriteAsync(bundleName, assetName, (resName, resData, hashParam)=>
        {
            image.enabled = true;
            image.sprite = resData;
            if (callBack != null)
            {
                callBack.Invoke(resName, resData, hash);
            }
        }, null));
    }



    #endregion

    #region ResourceFile

    public const string RES_SPRITE_PATH = "Sprite/";
    public const string RES_PREFAB_PATH = "Prefab/";
    public const string RES_TABLE_PATH = "Tables/";
    public const string RES_EFFECT_PATH = "Effect/";
    public const string RES_UI_PATH = "UI/";
    public const string RES_AUDIO_PATH = "Audios/";
    public const string RES_TEXTURE_PATH = "Texture/";

    public const string RES_EDITOR_FOLD = "Assets/FightCraft/BundleAssets/";

    //public Sprite GetSprite(string resName)
    //{
    //    string resPath = RES_EDITOR_FOLD + RES_SPRITE_PATH + resName;
    //    var resource = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(resPath);
    //    if (resource == null)
    //    {
    //        Debug.LogError("Resource error:" + resPath);
    //    }
    //    return resource;
    //}

    //public Texture GetTexture(string resName)
    //{
    //    string resPath = RES_EDITOR_FOLD + RES_TEXTURE_PATH + resName;
    //    var resource = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture>(resPath);
    //    if (resource == null)
    //    {
    //        Debug.LogError("Resource error:" + resPath);
    //    }
    //    return resource;
    //}

    //public GameObject GetGameObject(string resName)
    //{
    //    string resPath = RES_EDITOR_FOLD + resName;
    //    var resource = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(resPath);
    //    if (resource == null)
    //    {
    //        Debug.LogError("Resource error:" + resPath);
    //    }
    //    return resource;
    //}

    //public GameObject GetInstanceGameObject(string resPath)
    //{
    //    string resDataPath = RES_EDITOR_FOLD + resPath;
    //    var resource = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(resDataPath);
    //    if (resource == null)
    //    {
    //        Debug.LogError("Resource error:" + resPath);
    //        return null;
    //    }
    //    var instanceGO = GameObject.Instantiate<GameObject>(resource);
    //    return instanceGO;
    //}

    public static string GetTable(string resName)
    {
        string resPath = RES_TABLE_PATH + resName;
        var resource = Resources.Load<TextAsset>(resPath);
        if (resource == null)
        {
            Debug.LogError("Resource error:" + resPath);
        }
        return resource.text;
    }

    //public GameObject GetEffect(string resName)
    //{
    //    string resPath = RES_EDITOR_FOLD + RES_EFFECT_PATH + resName;
    //    var resource = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(resPath);
    //    if (resource == null)
    //    {
    //        Debug.LogError("Resource error:" + resPath);
    //    }
    //    return resource;
    //}

    //public GameObject GetUIRes(string resName)
    //{
    //    string resPath = RES_EDITOR_FOLD + RES_UI_PATH + resName + ".prefab";
    //    var resource = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(resPath);
    //    if (resource == null)
    //    {
    //        Debug.LogError("Resource error:" + resPath);
    //    }
    //    return resource;
    //}

    //public AudioClip GetAudioClip(string resName)
    //{
    //    string resPath = RES_EDITOR_FOLD + RES_AUDIO_PATH + resName;
    //    var resource = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(resPath);
    //    if (resource == null)
    //    {
    //        Debug.LogError("Resource error:" + resPath);
    //    }
    //    return resource;
    //}

    //public AnimationClip GetAnimationClip(string resName)
    //{
    //    string resPath = RES_EDITOR_FOLD + resName;
    //    var resource = UnityEditor.AssetDatabase.LoadAssetAtPath<AnimationClip>(resPath);
    //    if (resource == null)
    //    {
    //        Debug.LogError("Resource error:" + resPath);
    //    }
    //    return resource;
    //}

    public void DestoryObj(GameObject obj)
    {
        GameObject.Destroy(obj);
    }
    #endregion
}

#region asset load callback

public delegate void LoadBundleAssetCallback<in T>(string assetName, T assetItem, Hashtable hashTable) where T : UnityEngine.Object;

#endregion
