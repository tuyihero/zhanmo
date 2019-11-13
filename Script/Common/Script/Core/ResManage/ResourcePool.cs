using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssetBundles;

public class ResourcePool : InstanceBase<ResourcePool>
{

    void Awake()
    {
        SetInstance(this);

    }

    void Destory()
    {
        SetInstance(null);
    }

    public void InitDefaultRes()
    {
        //if (ResourceManager._ResFromBundle)
        {
            //InitEffect();
            //InitAutio();
            //InitConfig();
        }
    }

    #region effect

    public List<EffectController> _CommonHitEffect;

    public Dictionary<string, EffectController> _LoadedEffects = new Dictionary<string, EffectController>();

    private Dictionary<string, Stack<EffectController>> _IdleEffects = new Dictionary<string, Stack<EffectController>>();



    private void InitEffect()
    {
        if (_CommonHitEffect != null)
            return;

        _CommonHitEffect = new List<EffectController>();
        ResourceManager.Instance.LoadPrefab("Effect/Hit/Effect_Dead_A_Hit", InitEffectCallBack, null);

        ResourceManager.Instance.LoadPrefab("Effect/Hit/Effect_Dead_B_Hit", InitEffectCallBack, null);

        ResourceManager.Instance.LoadPrefab("Effect/Hit/Effect_Blade_Red", InitEffectCallBack, null);

        ResourceManager.Instance.LoadPrefab("Effect/Hit/Hit_Fire", InitEffectCallBack, null);

        ResourceManager.Instance.LoadPrefab("Effect/Hit/Hit_Ice", InitEffectCallBack, null);

        ResourceManager.Instance.LoadPrefab("Effect/Hit/Hit_Light", InitEffectCallBack, null);

        ResourceManager.Instance.LoadPrefab("Effect/Hit/Hit_Wind", InitEffectCallBack, null);
        
    }

    public void LoadEffect(string effectRes, LoadBundleAssetCallback<EffectController> callBack, Hashtable hash)
    {
        if (_LoadedEffects.ContainsKey(effectRes))
        {
            callBack.Invoke(effectRes, _LoadedEffects[effectRes], hash);
            return;
        }

        string effectPath = "Effect/" + effectRes;
        ResourceManager.Instance.LoadPrefab(effectPath, (effectName, effectGo, callBackHash) =>
        {
            EffectController effct = effectGo.GetComponent<EffectController>();
            effct.transform.SetParent(transform);
            effct.name = effectName;
            _LoadedEffects.Add(effectRes, effct);
            callBack.Invoke(effectRes, _LoadedEffects[effectRes], callBackHash);
        }, hash);
    }

    public EffectController GetIdleEffect(EffectController effct)
    {

        EffectController idleEffect = null;
        if (_IdleEffects.ContainsKey(effct.name))
        {
            if (_IdleEffects[effct.name].Count > 0)
            {
                idleEffect = _IdleEffects[effct.name].Pop();
            }
        }

        if (idleEffect == null)
        {
            idleEffect = GameObject.Instantiate<EffectController>(effct);
            idleEffect.name = effct.name;
        }

        idleEffect._EffectLastTime = effct._EffectLastTime;
        return idleEffect;
    }

    public void RecvIldeEffect(EffectController effct)
    {
        if (effct is EffectOutLine)
            return;

        if (effct is EffectMaterialChange)
            return;

        if (effct is EffectChangeMainMaterial)
            return;

        string effectName = effct.name;
        if (!_IdleEffects.ContainsKey(effectName))
        {
            _IdleEffects.Add(effectName, new Stack<EffectController>());
        }
        effct.transform.SetParent(transform);
        effct.HideEffect();
        effct.transform.localPosition = Vector3.zero;
        _IdleEffects[effectName].Push(effct);
    }

    public bool IsEffectInRecvl(EffectController effct)
    {
        string effectName = effct.name.Replace("(Clone)", "");
        if (!_IdleEffects.ContainsKey(effectName))
        {
            return false;
        }
        return (_IdleEffects[effectName].Contains(effct));
    }


    public void ClearEffects()
    {
        foreach (var idleEffectKeys in _IdleEffects.Values)
        {
            foreach (var idleEffect in idleEffectKeys)
            {
                GameObject.Destroy(idleEffect);
            }
        }
        _IdleEffects = new Dictionary<string, Stack<EffectController>>();
    }

    public void PlaySceneEffect(EffectController effct, Vector3 position, Vector3 rotation)
    {
        var effectInstance = GetIdleEffect(effct);
        effectInstance.transform.SetParent(transform);
        effectInstance.transform.position = position;
        effectInstance.transform.rotation = Quaternion.Euler(rotation);
        effectInstance.PlayEffect();
    }

    private void InitEffectCallBack(string uiName, GameObject effectGO, Hashtable hashtable)
    {
        _CommonHitEffect.Add(effectGO.GetComponent<EffectController>());
        effectGO.SetActive(false);
        effectGO.transform.SetParent(transform);
        effectGO.transform.localPosition = Vector3.zero;
    }
    #endregion

    #region bullet

    private Dictionary<string, Stack<BulletBase>> _IdleBullets = new Dictionary<string, Stack<BulletBase>>();

    public BulletBase GetIdleBullet(BulletBase bullet)
    {
        BulletBase idleBullet = null;
        if (_IdleBullets.ContainsKey(bullet.name))
        {
            if (_IdleBullets[bullet.name].Count > 0)
            {
                idleBullet = _IdleBullets[bullet.name].Pop();
            }
        }

        if (idleBullet == null)
        {
            idleBullet = GameObject.Instantiate<BulletBase>(bullet);
        }

        return idleBullet;
    }

    public void RecvIldeBullet(BulletBase bullet)
    {
        string BulletName = bullet.name.Replace("(Clone)", "");
        if (!_IdleBullets.ContainsKey(BulletName))
        {
            _IdleBullets.Add(BulletName, new Stack<BulletBase>());
        }
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(transform);
        _IdleBullets[BulletName].Push(bullet);
    }

    public void ClearBullets()
    {
        foreach (var idleResKeys in _IdleBullets.Values)
        {
            foreach (var idleRes in idleResKeys)
            {
                GameObject.Destroy(idleRes);
            }
        }
        _IdleBullets = new Dictionary<string, Stack<BulletBase>>();
    }

    #endregion

    #region motion object

    private Dictionary<string, Stack<GameObject>> _IdleModel = new Dictionary<string, Stack<GameObject>>();

    private Dictionary<string, GameObject> _MonsterBasePrefab = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> _MonsterModels = new Dictionary<string, GameObject>();

    private GameObject _ShaowPanel;

    public void ClearMonsterPrefab(List<string> monBaseIds, List<string> monModelIds)
    {
        List<string> needRemoveId = new List<string>();
        foreach(var monID in _MonsterBasePrefab.Keys)
        {
            if (!monBaseIds.Contains(monID))
            {
                needRemoveId.Add(monID);
            }
        }

        foreach (var monID in needRemoveId)
        {
            if (_MonsterBasePrefab.ContainsKey(monID))
            {
                GameObject.Destroy(_MonsterBasePrefab[monID]);
                _MonsterBasePrefab.Remove(monID);
            }
        }

        needRemoveId = new List<string>();
        foreach (var monID in _MonsterModels.Keys)
        {
            if (!monModelIds.Contains(monID))
            {
                needRemoveId.Add(monID);
            }
        }

        foreach (var monID in needRemoveId)
        {
            if (_MonsterModels.ContainsKey(monID))
            {
                GameObject.Destroy(_MonsterModels[monID]);
                _MonsterModels.Remove(monID);
            }
        }
    }

    public IEnumerator InitSummonMotions(Tables.MonsterBaseRecord motionTab)
    {
        if (_ShaowPanel == null)
        {
            yield return ResourceManager.Instance.LoadPrefab("Common/ShadowPlane", (resName, resGO, hash) =>
            {
                _ShaowPanel = resGO;
                _ShaowPanel.SetActive(false);
                _ShaowPanel.transform.SetParent(transform);
            });
        }

        //foreach(var motionTab in motionTabs)
        {
            if (!_MonsterBasePrefab.ContainsKey(motionTab.MotionPath))
            {
                yield return ResourceManager.Instance.LoadPrefab("ModelBase/" + motionTab.MotionPath, (resName, resGO, hash) =>
                {
                    resGO.gameObject.SetActive(false);
                    _MonsterBasePrefab.Add(motionTab.MotionPath, resGO);
                    resGO.transform.SetParent(transform);
                });
            }

            if (!_MonsterModels.ContainsKey(motionTab.ModelPath))
            {
                yield return ResourceManager.Instance.LoadPrefab("Model/" + motionTab.ModelPath, (resName, resData, hash) =>
                {
                    resData.gameObject.SetActive(false);
                    _MonsterModels.Add(motionTab.ModelPath, resData);
                    resData.transform.SetParent(transform);
                });
            }
        }
    }

    public IEnumerator InitMonsterBase(List<string> monIds)
    {
        List<string> initMontionIDs = new List<string>();
        List<string> initModelIDs = new List<string>();

        Debug.Log("InitMonsterBase count:" + monIds.Count);
        //if (_ShaowPanel == null)
        //{
        //    Debug.Log("InitMonsterBase ShadowPlane:");
        //    yield return ResourceManager.Instance.LoadPrefab("Common/ShadowPlane", (resName, resGO, hash) =>
        //    {
        //        _ShaowPanel = resGO;
        //        _ShaowPanel.SetActive(false);
        //        _ShaowPanel.transform.SetParent(transform);
        //    });
        //}


        for (int i = 0; i < monIds.Count; ++i)
        {

            var monsterTab = Tables.TableReader.MonsterBase.GetRecord(monIds[i]);
            initMontionIDs.Add(monsterTab.MotionPath);
            initModelIDs.Add(monsterTab.ModelPath);
            if (!_MonsterBasePrefab.ContainsKey(monsterTab.MotionPath))
            {
                Debug.Log("InitMonsterBase motion:" + monsterTab.MotionPath);
                yield return ResourceManager.Instance.LoadPrefab("ModelBase/" + monsterTab.MotionPath, (resName, resGO, hash) =>
                {
                    resGO.gameObject.SetActive(false);
                    _MonsterBasePrefab.Add(monsterTab.MotionPath, resGO);
                    resGO.transform.SetParent(transform);
                });
            }

            if (!_MonsterModels.ContainsKey(monsterTab.ModelPath))
            {
                Debug.Log("InitMonsterBase model:" + monsterTab.ModelPath);
                yield return ResourceManager.Instance.LoadPrefab("Model/" + monsterTab.ModelPath, (resName, resData, hash) =>
                {
                    resData.gameObject.SetActive(false);
                    _MonsterModels.Add(monsterTab.ModelPath, resData);
                    resData.transform.SetParent(transform);
                });
            }

            var monElite = Tables.TableReader.MonsterBase.GetGroupMonType(monsterTab, Tables.MOTION_TYPE.Elite);
            if (monElite != null)
            {
                
                initMontionIDs.Add(monElite.MotionPath);
                initModelIDs.Add(monElite.ModelPath);
            }
            if (monElite != null && !_MonsterBasePrefab.ContainsKey(monElite.MotionPath))
            {
                Debug.Log("InitMonsterBase motionElite:" + monElite.MotionPath);
                yield return ResourceManager.Instance.LoadPrefab("ModelBase/" + monElite.MotionPath, (resName, resData, hashParam) =>
                {
                    resData.gameObject.SetActive(false);
                    _MonsterBasePrefab.Add(monElite.MotionPath, resData);
                    resData.transform.SetParent(transform);
                });
            }
            if (monElite != null && !_MonsterModels.ContainsKey(monElite.ModelPath))
            {
                Debug.Log("InitMonsterBase modelElite:" + monElite.ModelPath);
                yield return ResourceManager.Instance.LoadPrefab("Model/" + monElite.ModelPath, (resName, resData, hashParam) =>
                {
                    resData.gameObject.SetActive(false);
                    _MonsterModels.Add(monElite.ModelPath, resData);
                    resData.transform.SetParent(transform);
                });
            }

            var monEx = Tables.TableReader.MonsterBase.GetGroupMonType(monsterTab, Tables.MOTION_TYPE.ExElite);
            if (monEx != null)
            {
                initMontionIDs.Add(monEx.MotionPath);
                initModelIDs.Add(monEx.ModelPath);
            }
            if (monEx != null && !_MonsterBasePrefab.ContainsKey(monEx.MotionPath))
            {
                Debug.Log("InitMonsterBase motionEx:" + monEx.MotionPath);
                yield return ResourceManager.Instance.LoadPrefab("ModelBase/" + monEx.MotionPath, (resName, resData, hashParam) =>
                {
                    resData.gameObject.SetActive(false);
                    _MonsterBasePrefab.Add(monEx.MotionPath, resData);
                    resData.transform.SetParent(transform);
                });
            }
            if (monEx != null && !_MonsterModels.ContainsKey(monEx.ModelPath))
            {
                Debug.Log("InitMonsterBase modelEx:" + monEx.ModelPath);
                yield return ResourceManager.Instance.LoadPrefab("Model/" + monEx.ModelPath, (resName, resData, hashParam) =>
                {
                    resData.gameObject.SetActive(false);
                    _MonsterModels.Add(monEx.ModelPath, resData);
                    resData.transform.SetParent(transform);
                });
            }
        }

        Debug.Log("Clear monsters");
        ClearMonsterPrefab(initMontionIDs, initModelIDs);
    }

    public MotionManager GetIdleMotion(Tables.MonsterBaseRecord monsterTab)
    {
        if (monsterTab == null)
            return null;

        GameObject motion = null;
        
        if (_MonsterBasePrefab.ContainsKey(monsterTab.MotionPath))
        {
            motion = GameObject.Instantiate(_MonsterBasePrefab[monsterTab.MotionPath]);
        }
        else
        {
            Debug.LogError("GetIdleMotion error:" + monsterTab.MotionPath);
            return null;
        }
        var motionScript = motion.GetComponent<MotionManager>();
        var aiScript = motion.GetComponent<AI_Base>();
        aiScript.InitSkillGoes(motionScript);

        GameObject modelObj = null;
        if (_IdleModel.ContainsKey(monsterTab.ModelPath))
        {
            if (_IdleModel[monsterTab.ModelPath].Count > 0)
            {
                modelObj = _IdleModel[monsterTab.ModelPath].Pop();
            }
        }
        if (modelObj == null)
        {
            if (_MonsterModels.ContainsKey(monsterTab.ModelPath))
            {
                modelObj = GameObject.Instantiate(_MonsterModels[monsterTab.ModelPath]);
            }
            else
            {
                Debug.LogError("GetIdleModel error:" + monsterTab.MotionPath);
                return null;
            }
            modelObj.name = monsterTab.ModelPath;
            var animation = modelObj.GetComponent<Animation>();
            if (animation == null)
            {
                modelObj.AddComponent<Animation>();
            }
        }

        var animEvent = modelObj.GetComponent<AnimationEventManager>();
        if (animEvent != null)
        {
            GameObject.DestroyImmediate(animEvent);
        }
        modelObj.AddComponent<AnimationEventManager>();

        modelObj.gameObject.SetActive(true);
        modelObj.transform.SetParent(motion.transform);
        modelObj.transform.localPosition = Vector3.zero;
        modelObj.transform.localRotation = Quaternion.Euler(Vector3.zero);

        if (_ShaowPanel != null)
        {
            var shadow = GameObject.Instantiate(_ShaowPanel);
            shadow.transform.SetParent(motion.transform);
            shadow.transform.localPosition = Vector3.zero;
            shadow.transform.localRotation = Quaternion.Euler(Vector3.zero);
            shadow.gameObject.SetActive(true);
        }

        return motion.GetComponent<MotionManager>();
    }

    public void RecvIldeMotion(MotionManager objMotion)
    {
        objMotion.RecvAllEffects();
        var model = objMotion.AnimationEvent.gameObject;
        string objName = objMotion.MonsterBase.ModelPath;
        if (!_IdleModel.ContainsKey(objName))
        {
            _IdleModel.Add(objName, new Stack<GameObject>());
        }
        //objMotion.gameObject.SetActive(false);
        model.transform.SetParent(transform);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.Euler(Vector3.zero);
        _IdleModel[objName].Push(model);

        GameObject.Destroy(objMotion.gameObject);
    }

    public void ClearObjs()
    {
        foreach (var idleResKeys in _IdleModel.Values)
        {
            foreach (var idleRes in idleResKeys)
            {
                GameObject.Destroy(idleRes);
            }
        }
        _IdleModel = new Dictionary<string, Stack<GameObject>>();
    }

    private void InitModelBaseCallBack(string resName, GameObject resGO, Hashtable hashtable)
    {
        string monID = (string)hashtable["MonsterID"];
        resGO.gameObject.SetActive(false);
        _MonsterBasePrefab.Add(monID, resGO);
    }

    #endregion

    #region char model

    public IEnumerator LoadCharModel(string modelName, string weaponName, LoadBundleAssetCallback<GameObject> callBack, Hashtable hash)
    {
        Debug.Log("LoadCharModel");
        string resPath = "Model/" + weaponName;
        GameObject weaponGO = null;
        yield return ResourceManager.Instance.LoadPrefab(resPath, (subResName, subResGO, subCallBack) =>
        {
            weaponGO = subResGO;
        });

        resPath = "Model/" + modelName;
        GameObject modelGO = null;
        yield return ResourceManager.Instance.LoadPrefab(resPath, (subResName, subResGO, subCallBack) =>
        {
            modelGO = subResGO;
        });

        var weaponTrans = modelGO.transform.Find("center/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/righthand/rightweapon");
        var weaponTransChild = weaponTrans.GetComponentsInChildren<Transform>();
        for (int i = weaponGO.transform.childCount - 1; i >= 0; --i)
        {
            weaponGO.transform.GetChild(i).SetParent(weaponTrans.parent);
        }
        foreach (var oldWeaponChild in weaponTransChild)
        {
            GameObject.Destroy(oldWeaponChild.gameObject);
        }
        GameObject.Destroy(weaponGO.gameObject);

        callBack.Invoke(modelName, modelGO, hash);
    }

    public IEnumerator LoadCharModel(string mainBase, string modelName, string weaponName, LoadBundleAssetCallback<MotionManager> callBack, Hashtable hash)
    {
        string resPath = "Model/" + weaponName;
        GameObject weaponGO = null;
        yield return ResourceManager.Instance.LoadPrefab(resPath, (subResName, subResGO, subCallBack) =>
        {
            weaponGO = subResGO;
            weaponGO.name = "Weapon";
        });

        resPath = "Model/" + modelName;
        GameObject modelGO = null;
        yield return ResourceManager.Instance.LoadPrefab(resPath, (subResName, subResGO, subCallBack) =>
        {
            modelGO = subResGO;
            modelGO.name = "Body";
        });

        //var weaponTrans = modelGO.transform;
        //var weaponTransChild = weaponTrans.GetComponentsInChildren<Transform>();
        //for (int i = weaponGO.transform.childCount - 1; i >= 0; --i)
        //{
        //    weaponGO.transform.GetChild(i).SetParent(weaponTrans.parent);
        //}
        //foreach (var oldWeaponChild in weaponTransChild)
        //{
        //    GameObject.Destroy(oldWeaponChild.gameObject);
        //}
        //GameObject.Destroy(weaponGO.gameObject);

        resPath = "ModelBase/" + mainBase;
        MotionManager motion = null;
        yield return ResourceManager.Instance.LoadPrefab(resPath, (mainBaseName, mainBaseGO, mainCallbackHash) =>
        {
            var mainCharMotion = mainBaseGO.GetComponent<MotionManager>();
            mainCharMotion.tag = "Player";
            motion = mainCharMotion;
            resPath = "Model/" + weaponName;
            modelGO.transform.SetParent(mainCharMotion.transform);
            Debug.Log("modelGO:" + modelGO.name + ",modelGO parent:" + modelGO.transform.parent.name);
            modelGO.transform.localPosition = Vector3.zero;
            modelGO.transform.localRotation = Quaternion.Euler(Vector3.zero);

            weaponGO.transform.SetParent(modelGO.transform.Find("Weapon"));
            weaponGO.transform.localPosition = Vector3.zero;
            weaponGO.transform.localRotation = Quaternion.Euler(Vector3.zero);
            callBack.Invoke(modelName, mainCharMotion, hash);
            Debug.Log("motion child count 3:" + mainCharMotion.transform.childCount);
        });
        Debug.Log("motion child count 6:" + motion.transform.childCount);
    }

    #endregion

    #region ui

    private Dictionary<string, Stack<GameObject>> _IdleUIItems = new Dictionary<string, Stack<GameObject>>();

    public T GetIdleUIItem<T>(GameObject itemPrefab, Transform parentTrans = null)
    {
        GameObject idleItem = null;
        if (_IdleUIItems.ContainsKey(itemPrefab.name))
        {
            if (_IdleUIItems[itemPrefab.name].Count > 0)
            {
                idleItem = _IdleUIItems[itemPrefab.name].Pop();
            }
        }

        if (idleItem == null)
        {
            idleItem = GameObject.Instantiate<GameObject>(itemPrefab);
        }
        idleItem.gameObject.SetActive(true);
        if (parentTrans != null)
        {
            idleItem.transform.SetParent(parentTrans);
            idleItem.transform.localPosition = Vector3.zero;
            idleItem.transform.localRotation = Quaternion.Euler(Vector3.zero);
            idleItem.transform.localScale = Vector3.one;
        }
        return idleItem.GetComponent<T>();
    }

    public void RecvIldeUIItem(GameObject itemBase)
    {
        string itemName = itemBase.name.Replace("(Clone)", "");
        if (!_IdleUIItems.ContainsKey(itemName))
        {
            _IdleUIItems.Add(itemName, new Stack<GameObject>());
        }
        itemBase.gameObject.SetActive(false);
        itemBase.transform.SetParent(transform);
        if (!_IdleUIItems[itemName].Contains(itemBase))
        {
            _IdleUIItems[itemName].Push(itemBase);
        }
    }

    public void ClearUIItems()
    {
        foreach (var idleResKeys in _IdleUIItems.Values)
        {
            foreach (var idleRes in idleResKeys)
            {
                GameObject.Destroy(idleRes);
            }
        }
        _IdleUIItems = new Dictionary<string, Stack<GameObject>>();
    }

    #endregion

    #region audio

    public Dictionary<int, AudioClip> _CommonAudio;
    public int _HitSuperArmor = 0;

    private void InitAutio()
    {
        if (_CommonAudio != null)
            return;

        _CommonAudio = new Dictionary<int, AudioClip>();
        //var audio1 = ResourceManager.Instance.GetAudioClip("common/HitArmor");
        //_CommonAudio.Add(0, audio1);
        ResourceManager.Instance.LoadAudio("common/HitArmor", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(0, resData);
        }, null);

        //var audio2 = ResourceManager.Instance.GetAudioClip("common/HitSwordNone");
        //_CommonAudio.Add(1,audio2);
        ResourceManager.Instance.LoadAudio("common/HitSwordNone", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(1, resData);
        }, null);

        //var audio3 = ResourceManager.Instance.GetAudioClip("common/HitSwordBody");
        //_CommonAudio.Add(2,audio3);
        ResourceManager.Instance.LoadAudio("common/HitSwordBody", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(2, resData);
        }, null);

        //var audio4 = ResourceManager.Instance.GetAudioClip("common/HitSwordSlap");
        //_CommonAudio.Add(3,audio4);
        ResourceManager.Instance.LoadAudio("common/HitSwordSlap", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(3, resData);
        }, null);

        //var audio5 = ResourceManager.Instance.GetAudioClip("common/HitSwordSlap2");
        //_CommonAudio.Add(4,audio5);
        ResourceManager.Instance.LoadAudio("common/HitSwordSlap2", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(4, resData);
        }, null);

        //var audio10 = ResourceManager.Instance.GetAudioClip("common/HitHwNone");
        //_CommonAudio.Add(10, audio10);
        ResourceManager.Instance.LoadAudio("common/HitHwNone", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(10, resData);
        }, null);

        //var audio11 = ResourceManager.Instance.GetAudioClip("common/HitHwBody");
        //_CommonAudio.Add(11, audio11);
        ResourceManager.Instance.LoadAudio("common/HitHwBody", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(11, resData);
        }, null);

        //var audio12 = ResourceManager.Instance.GetAudioClip("common/HwAtk");
        //_CommonAudio.Add(12, audio12);
        ResourceManager.Instance.LoadAudio("common/HwAtk", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(12, resData);
        }, null);

        //var audioEle = ResourceManager.Instance.GetAudioClip("common/AtkFire");
        //_CommonAudio.Add(100, audioEle);
        ResourceManager.Instance.LoadAudio("common/AtkFire", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(100, resData);
        }, null);

        //audioEle = ResourceManager.Instance.GetAudioClip("common/AtkIce");
        //_CommonAudio.Add(101, audioEle);
        ResourceManager.Instance.LoadAudio("common/AtkIce", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(101, resData);
        }, null);

        //audioEle = ResourceManager.Instance.GetAudioClip("common/AtkLighting");
        //_CommonAudio.Add(102, audioEle);
        ResourceManager.Instance.LoadAudio("common/AtkLighting", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(102, resData);
        }, null);

        //audioEle = ResourceManager.Instance.GetAudioClip("common/AtkStone");
        //_CommonAudio.Add(103, audioEle);
        ResourceManager.Instance.LoadAudio("common/AtkStone", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(103, resData);
        }, null);

        //audioEle = ResourceManager.Instance.GetAudioClip("common/AtkWind");
        //_CommonAudio.Add(104, audioEle);
        ResourceManager.Instance.LoadAudio("common/AtkWind", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(104, resData);
        }, null);

        //audioEle = ResourceManager.Instance.GetAudioClip("common/HitFire");
        //_CommonAudio.Add(110, audioEle);
        ResourceManager.Instance.LoadAudio("common/HitFire", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(110, resData);
        }, null);

        //audioEle = ResourceManager.Instance.GetAudioClip("common/HitIce");
        //_CommonAudio.Add(111, audioEle);
        ResourceManager.Instance.LoadAudio("common/HitFire", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(111, resData);
        }, null);

        //audioEle = ResourceManager.Instance.GetAudioClip("common/HitLighting");
        //_CommonAudio.Add(112, audioEle);
        ResourceManager.Instance.LoadAudio("common/HitLighting", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(112, resData);
        }, null);

        //audioEle = ResourceManager.Instance.GetAudioClip("common/HitStone");
        //_CommonAudio.Add(113, audioEle);
        ResourceManager.Instance.LoadAudio("common/HitStone", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(113, resData);
        }, null);

        //audioEle = ResourceManager.Instance.GetAudioClip("common/HitWind");
        //_CommonAudio.Add(114, audioEle);
        ResourceManager.Instance.LoadAudio("common/HitWind", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(114, resData);
        }, null);

        //audioEle = ResourceManager.Instance.GetAudioClip("common/AtkFire2");
        //_CommonAudio.Add(120, audioEle);
        ResourceManager.Instance.LoadAudio("common/AtkFire2", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(120, resData);
        }, null);

        //var audioMon = ResourceManager.Instance.GetAudioClip("common/AtkBow");
        //_CommonAudio.Add(200, audioMon);
        ResourceManager.Instance.LoadAudio("common/AtkBow", (resName, resData, callBackHash) =>
        {
            _CommonAudio.Add(200, resData);
        }, null);
    }


    #endregion

    #region config prefab common

    public enum ConfigEnum
    {
        HitProtectedBuff,
        DexAccelateBuff,
        SuperArmor,
        SuperArmorBlock,
        BlockBullet,
        IntShieldBuff,
        StrBuff,
        ResourceConfig,
        RandomBuff,
        BlockSummon

    }

    public static Dictionary<ConfigEnum, string> ConfigPrefabs = new Dictionary<ConfigEnum, string>()
    {
        { ConfigEnum.HitProtectedBuff, "SkillMotion/CommonImpact/HitProtectedBuff"},
        { ConfigEnum.DexAccelateBuff, "SkillMotion/CommonImpact/DexAccelateBuff"},
        { ConfigEnum.SuperArmor, "SkillMotion/CommonImpact/SuperArmor"},
        { ConfigEnum.SuperArmorBlock, "SkillMotion/CommonImpact/SuperArmorBlock"},
        { ConfigEnum.BlockBullet, "SkillMotion/CommonImpact/BlockBullet"},
        { ConfigEnum.IntShieldBuff, "SkillMotion/CommonImpact/IntShieldBuff"},
        { ConfigEnum.StrBuff, "SkillMotion/CommonImpact/StrBuff"},
        { ConfigEnum.ResourceConfig, "Common/ResourceConfig"},
        { ConfigEnum.RandomBuff, "SkillMotion/CommonImpact/EliteRandomBuff"},
        { ConfigEnum.BlockSummon, "SkillMotion/CommonImpact/BlockSummon"},
    };

    public Dictionary<string, GameObject> _ConfigPrefabs;

    public T GetConfig<T>(ConfigEnum configType)
    {
        string configName = ConfigPrefabs[configType];
        if (_ConfigPrefabs.ContainsKey(configName))
        {
            return _ConfigPrefabs[configName].GetComponent<T>();
        }

        return default(T);
    }

    public void InitConfig()
    {
        if (_ConfigPrefabs != null)
            return;

        _ConfigPrefabs = new Dictionary<string, GameObject>();
        foreach (var configName in ConfigPrefabs.Values)
        {
            Hashtable hash = new Hashtable();
            hash.Add("ConfigName", configName);
            ResourceManager.Instance.LoadPrefab(configName, InitConfigPrefabCallBack, hash);
        }
    }

    private void InitConfigPrefabCallBack(string resName, GameObject resGO, Hashtable hashtable)
    {
        string configName = (string)hashtable["ConfigName"];
        _ConfigPrefabs.Add(configName, resGO);
        resGO.transform.SetParent(transform);
        //resGO.SetActive(false);
        resGO.transform.localPosition = Vector3.zero;
    }

    #endregion

    #region config prefab

    public Dictionary<string, GameObject> _LoadedConfig = new Dictionary<string, GameObject>();

    public void LoadConfig(string resName, LoadBundleAssetCallback<GameObject> callBack, Hashtable hash)
    {
        if (_LoadedConfig.ContainsKey(resName))
        {
            GameObject configInstance = GameObject.Instantiate(_LoadedConfig[resName]);
            callBack.Invoke(resName, configInstance, hash);
            return;
        }

        string resPath = resName;
        ResourceManager.Instance.LoadPrefab(resPath, (effectName, effectGo, callBackHash) =>
        {
            
            effectGo.transform.SetParent(transform);
            effectGo.name = effectName;
            if (!_LoadedConfig.ContainsKey(resName))
                _LoadedConfig.Add(resName, effectGo);

            GameObject configInstance = GameObject.Instantiate(_LoadedConfig[resName]);
            callBack.Invoke(resName, configInstance, hash);
        }, hash);
    }

    #endregion

}
