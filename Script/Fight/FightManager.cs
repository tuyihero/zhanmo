using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Tables;

public class FightManager : InstanceBase<FightManager>
{

    // Use this for initialization
    void Awake ()
    {
        SetInstance(this);
        DontDestroyOnLoad(this);
        _InitProcess = 0;
        _InitStep = InitStep.InitScene;
    }

    void Update()
    {
        if (_InitStep != InitStep.InitFinish)
        {
            InitUpdate();
        }
        else
        {
            //WarningUpdate();
            if (_CountDown)
            {
                _LogicFightTime = (int)(_LogicPassTime - (Time.time - _LogicStartTime));
                if (_LogicFightTime < 0)
                    _LogicFightTime = 0;
            }

            UpdateCombo();
        }

    }

    public enum InitStep
    {
        None,
        InitScene,
        InitSceneWait,
        InitSceneFinish,

        InitMainRole,
        InitMainRoleWait,
        InitMainRoleFinish,
        
        InitCamera,
        InitCameraWait,
        InitCameraFinish,

        InitUI,
        InitUIWait,
        InitUIFinish,

        InitMonster,
        InitMonsterWait,
        InitMonsterFinish,

        InitFinish
    }
    public InitStep _InitStep;

    public float _LogicFightTime = 0;
    public float _LogicPassTime = 0;
    public float _LogicStartTime = 0;
    public bool _CountDown = true;
    void InitUpdate()
    {

        if (_InitStep == InitStep.InitScene)
        {
            _CountDown = false;
            _LogicFightTime = -1;
            _LogicPassTime = -1;
            _InitStep = InitStep.InitSceneWait;
            StartCoroutine(InitScene());
            _InitProcess = 0.0f;
        }
        else if (_InitStep == InitStep.InitSceneWait)
        {
            _InitProcess += 0.01f;
            _InitProcess = Mathf.Min(_InitProcess, 0.7f);
        }
        else if (_InitStep == InitStep.InitSceneFinish)
        {
            ResourcePool.Instance.InitDefaultRes();
            InitCamera();
            _InitStep = InitStep.InitMainRoleWait;
            StartCoroutine(InitMainRole());
            _InitProcess = 0.8f;
        }
        else if (_InitStep == InitStep.InitMainRoleWait)
        {
            _InitProcess += 0.01f;
            _InitProcess = Mathf.Min(_InitProcess, 0.8f);
        }
        else if (_InitStep == InitStep.InitMainRoleFinish)
        {
            UIDamagePanel.ShowAsyn();
            AimTargetPanel.ShowAsyn();

            _InitStep = InitStep.InitMonsterWait;
            StartCoroutine(InitMonsterPrefab());

            
            _InitProcess = 0.81f;
        }
        else if (_InitStep == InitStep.InitMonsterWait)
        {
            Debug.Log("_InitStep InitMonsterWait:" + _InitProcess);
            _InitProcess += 0.01f;
            _InitProcess = Mathf.Min(_InitProcess, 0.99f);
        }
        else if (_InitStep == InitStep.InitMonsterFinish)
        {
            //StopAllCoroutines();
            StartSceneLogic();
            _InitProcess = 1.0f;
            _InitStep = InitStep.InitFinish;

            _LogicPassTime = TableReader.AttrValueLevel.GetSpValue(ActData.Instance._ProcessStageIdx, 35);
            _LogicFightTime = _LogicPassTime;
            _LogicStartTime = Time.time;
            _CountDown = true;

            TimeManager.Instance.Init();
        }
    }

    #region Init

    public CameraFollow _CameraFollow;
    private int _ActingRegion;
    private List<GameObject> _SceneSPObj = new List<GameObject>();

    private float _InitProcess = 0;
    public float InitProcess
    {
        get
        {
            return _InitProcess;
        }
    }

    private void InitCamera()
    {
        GameObject cameraRoot = Camera.main.gameObject;

        Camera sceneCamera = Camera.main;
        if (sceneCamera == null)
        {
            GameObject go = new GameObject("Camera");
            sceneCamera = go.AddComponent<Camera>();
            go.tag = "MainCamera";
        }
        cameraRoot.transform.position = sceneCamera.transform.position;
        cameraRoot.transform.rotation = sceneCamera.transform.rotation;

        sceneCamera.transform.SetParent(cameraRoot.transform);
        //sceneCamera.transform.localPosition = Vector3.zero;
        //sceneCamera.transform.localRotation = Quaternion.Euler(Vector3.zero);
        sceneCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("UI"));
        sceneCamera.nearClipPlane = 1.0f;
        //cameraRoot.AddComponent<AudioListener>();

        //var subUICamera = ResourceManager.Instance.GetInstanceGameObject("Common/SubUICamera");
        //subUICamera.transform.SetParent(sceneCamera.transform);
        //subUICamera.transform.localPosition = Vector3.zero;
        //subUICamera.transform.localRotation = Quaternion.Euler(Vector3.zero);

        _ActingRegion = 0;
        _CameraFollow = cameraRoot.GetComponent<CameraFollow>();
        _CameraFollow.SetSceneAnim(GameObject.FindObjectOfType<SceneAnimController>());
        _CameraFollow._SceneAnimController.InitSceneObjPos();
        //_CameraFollow._FollowObj = _MainChatMotion.gameObject;
        //_CameraFollow._Distance = LogicManager.Instance.EnterStageInfo.CameraOffset[_ActingRegion];

        var globalEffect = cameraRoot.AddComponent<GlobalEffect>();
        var inputManager = cameraRoot.AddComponent<InputManager>();
        var aimManager = cameraRoot.AddComponent<AimTarget>();
        //inputManager._InputMotion = _MainChatMotion;

        //for (int i = 0; i < LogicManager.Instance.EnterStageInfo.ValidScenePath.Count; ++i)
        //{
        //    var spGO = GameObject.Find(LogicManager.Instance.EnterStageInfo.ValidScenePath[i] + "_SP");
        //    if (spGO == null)
        //        Debug.LogError("spGO none:" + LogicManager.Instance.EnterStageInfo.ValidScenePath[i]);
        //    _SceneSPObj.Add(spGO);

        //    if (i > 0)
        //    {
        //        _SceneSPObj[i].SetActive(false);
        //    }
        //}
        //if (_SceneSPObj.Count > 0)
        //{
        //    _SceneSPObj[0].SetActive(true);
        //}


        _FightLevel = ActData.Instance.GetStageLevel();

        _InitStep = InitStep.InitCameraFinish;

        gameObject.AddComponent<TimeManager>();
    }

    private void InitResourcePool()
    {
        //gameObject.AddComponent<ResourcePool>();
    }

    private IEnumerator InitMonsterPrefab()
    {
        var initMonList = _FightScene.GetLogicMonIDs();
        yield return ResourcePool.Instance.InitMonsterBase(initMonList);

        _InitStep = InitStep.InitMonsterFinish;
    }

    #endregion

    #region Objects

    public int _FightLevel;

    public int GetEliteMonsterRate()
    {
        return GameDataValue.GetMaxRate();
    }

    private MotionManager _MainChatMotion;
    public MotionManager MainChatMotion
    {
        get
        {
            return _MainChatMotion;
        }  
        set
        {
            _MainChatMotion = value;
        }
    }

    private IEnumerator InitMainRole()
    {
        //while (ResourcePool.Instance._ConfigPrefabs == null)
        //{
        //    yield return null;
        //}

        ResourcePool.Instance.ClearObjs();
        ResourcePool.Instance.ClearEffects();
        ResourcePool.Instance.ClearBullets();
        ResourcePool.Instance.ClearUIItems();

        string mainBaseName = PlayerDataPack.Instance._SelectedRole.MainBaseName;
        string modelName = PlayerDataPack.Instance._SelectedRole.ModelName;
        string weaponName = PlayerDataPack.Instance._SelectedRole.GetWeaponModelName();

        yield return (ResourcePool.Instance.LoadCharModel(mainBaseName, modelName, weaponName, (resName, mainMotion, hash) =>
       {
           MainChatMotion = mainMotion;

       }, null));
        var motionTran = MainChatMotion.transform.Find("AnimTrans/Motion");
        GlobalBuffData.Instance.ActBuffInFight();
        UITestEquip.ActBuffInFight();

        
        yield return SummonSkill.Instance.InitSummonMotions();

        List<string> skillMotions = SkillData.Instance.GetRoleSkills();
        Debug.Log("motion child count:" + MainChatMotion.transform.childCount);
        foreach (var skillMotion in skillMotions)
        {
            GameObject motionObj = null;
            yield return ResourceManager.Instance.LoadPrefab("SkillMotion/" + PlayerDataPack.Instance._SelectedRole.MotionFold + "/" + skillMotion, (subResName, subResGO, subCallBack) =>
            {
                motionObj = subResGO;
            });

            if (motionObj != null)
            {
                motionObj.transform.SetParent(motionTran);
                motionObj.transform.localPosition = Vector3.zero;
                motionObj.SetActive(true);
                var skillBase = motionObj.GetComponent<ObjMotionSkillBase>();
                if (skillBase == null)
                    continue;

                SetSkillElement(skillMotion, skillBase);

                for (int i = 0; i < skillBase._NextEffect.Count; ++i)
                {
                    if (skillBase._NextEffect[i] != null)
                    {
                        skillBase._NextEffect[i].SetEffectColor(ElementType.Physic);
                    }
                }
            }
        }
        foreach (var impact in RoleData.SelectRole._BaseAttr._ExAttr)
        {
            if (impact is RoleAttrImpactAddSkill)
            {
                var addSkillImpact = (impact as RoleAttrImpactAddSkill);
                yield return StartCoroutine(addSkillImpact.GetSkillBase());
                addSkillImpact._AddSkill.transform.SetParent(motionTran);
                addSkillImpact._AddSkill.transform.localPosition = Vector3.zero;
                addSkillImpact._AddSkill.gameObject.SetActive(true);
            }
        }
        MainChatMotion.InitRoleAttr(null, MOTION_TYPE.MainChar);
        MainChatMotion.InitMotion();
        //MainChatMotion.SetPosition(new Vector3(2, -1, 0));
        FightLayerCommon.SetPlayerLayer(MainChatMotion);
        //UIHPPanel.ShowHPItem(_MainChatMotion);

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_ROLE_LEVEL_UP, RoleLevelUp);

        _InitStep = InitStep.InitMainRoleFinish;

        if (_CameraFollow != null)
        {
            _CameraFollow._FollowObj = MainChatMotion.gameObject;
            InputManager.Instance.InputMotion = MainChatMotion;
        }

        UIHPPanel.ShowHPItem(MainChatMotion, false);
    }

    private void SetSkillElement(string skillName, ObjMotionSkillBase skillBase)
    {
        

    }

    #endregion

    #region scene obj

    public List<MotionManager> _MonMotion = new List<MotionManager>();

    private int _SceneEnemyCnt = 0;
    public int SceneEnemyCnt
    {
        get
        {
            return _SceneEnemyCnt;
        }
    }

    public MotionManager InitEnemy(string monsterID, Vector3 pos, Vector3 rot, Tables.MOTION_TYPE motionType = Tables.MOTION_TYPE.Normal)
    {
        Tables.MonsterBaseRecord monsterBase = Tables.TableReader.MonsterBase.GetRecord(monsterID);
        if (motionType != Tables.MOTION_TYPE.Normal&& motionType != Tables.MOTION_TYPE.Hero)
        {
            monsterBase = Tables.TableReader.MonsterBase.GetGroupMonType(monsterBase, motionType);
        }
        if (monsterBase == null)
            return null;

        var mainBase = ResourcePool.Instance.GetIdleMotion(monsterBase);
        mainBase.SetPosition(pos);
        mainBase.SetRotate(rot);

        mainBase.InitRoleAttr(monsterBase, motionType);
        //mainBase.RoleAttrManager.MotionType = motionType;
        mainBase.InitMotion();
        FightLayerCommon.SetEnemyLayer(mainBase);

        AI_Base aiBase = mainBase.GetComponent<AI_Base>();

        _MonMotion.Add(mainBase);

        if (monsterBase.MotionType == Tables.MOTION_TYPE.Elite)
        {
            aiBase.InitSkillDamageRate(0.6f);
            //mainBase.Animation.transform.localScale = Vector3.one * monsterBase.ModelScale * 1.1f;
        }
        else if (monsterBase.MotionType == Tables.MOTION_TYPE.ExElite)
        {
            aiBase.InitSkillDamageRate(0.6f);
            //mainBase.Animation.transform.localScale = Vector3.one * monsterBase.ModelScale * 1.2f;
        }
        else if(monsterBase.MotionType == Tables.MOTION_TYPE.Hero)
        {
            aiBase.InitSkillDamageRate(1.0f);
            //mainBase.Animation.transform.localScale = Vector3.one * monsterBase.ModelScale;
            //mainBase.Animation.transform.localScale = mainBase.Animation.transform.localScale * 0.8f;
            //mainBase.NavAgent.radius = mainBase.NavAgent.radius * mainBase.Animation.transform.localScale.x * 0.8f;
        }
        else
        {
            aiBase.InitSkillDamageRate(0.3f);
            //mainBase.Animation.transform.localScale = Vector3.one * monsterBase.ModelScale;
        }
        aiBase.InitMonsterDamageRate();

        aiBase.SetCombatLevel(1);

        if (motionType == Tables.MOTION_TYPE.Hero)
        {
            UITargetFrame.ShowAsyn(mainBase);
        }

        ++_SceneEnemyCnt;

        return mainBase;
    }

    public void ObjDisapear(MotionManager objMotion)
    {
        _FightScene.MotionDisapear(objMotion);

        _MonMotion.Remove(objMotion);

        ResourcePool.Instance.RecvIldeMotion(objMotion);
        
        --_SceneEnemyCnt;
    }

    public void ObjCorpse(MotionManager objMotion)
    {
        
    }

    public void OnObjDie(MotionManager objMotion)
    {
        _FightScene.MotionDie(objMotion);

        --_SceneEnemyCnt;

        if (objMotion.MonsterBase != null)
        {
            Hashtable hash = new Hashtable();
            hash.Add("MonsterInfo", objMotion);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_KILL_MONSTER, this, hash);
        }
    }

    public void KillAllMotion()
    {
        foreach (var motion in _MonMotion)
        {
            if(!motion.IsMotionDie)
                motion.MotionDie();
        }
    }


    #endregion

    #region scene

    private FightSceneLogicBase _FightScene;

    public Dictionary<string, AreaGroup> _AreaGroups = new Dictionary<string, AreaGroup>();

    private IEnumerator InitScene()
    {
        yield return ResourceManager.Instance.LoadPrefab("FightSceneLogic/" + LogicManager.Instance.EnterStageInfo.FightLogicPath, (subResName, subResGO, subCallBack) =>
        {
            _FightScene = subResGO.GetComponent<FightSceneLogicBase>();
        });

        _FightScene.transform.SetParent(transform);
        _FightScene.transform.localPosition = Vector3.zero;
        var needLoadScene = _FightScene.GetLogicScenes();
        if (needLoadScene == null)
        {
            needLoadScene = LogicManager.Instance.EnterStageInfo.GetValidScenePath();
        }

        yield return ResourceManager.Instance.LoadLevelAsync("FightScene", false);
        //var actGroup = GameObject.Find(needLoadScene[0] + "_RandomAreas").GetComponent<AreaGroup>();
        //_AreaGroups.Add(needLoadScene[0], actGroup);
        for (int i = 0; i < needLoadScene.Count; ++i)
        {
            yield return ResourceManager.Instance.LoadPrefab("Scene/" + needLoadScene[i], (resName, resGO, hash) =>
            {

                resGO.SetActive(true);
                resGO.transform.position = Vector3.zero;
                resGO.transform.rotation = Quaternion.Euler(45, 0, 0);
                //actGroup = GameObject.Find(needLoadScene[i] + "_RandomAreas").GetComponent<AreaGroup>();
                //actGroup._LightGO.SetActive(false);
                //actGroup.transform.parent.gameObject.SetActive(false);
                //_AreaGroups.Add(needLoadScene[i], actGroup);
            });
            
        }

        _InitStep = InitStep.InitSceneFinish;
    }

    private void StartSceneLogic()
    {
        _FightScene.gameObject.SetActive(true);
        _FightScene.StartLogic();

        InitWarning();
    }

    

    public void StagePass()
    {
        ActData.Instance.PassStage(LogicManager.Instance.EnterStageInfo.StageType);
        _CountDown = false;
    }

    public void LogicFinish(bool isWin)
    {
        Debug.Log("LogicFinish");
        LogicManager.Instance.ExitFight();

        Hashtable hash = new Hashtable();
        hash.Add("IsWin", isWin);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EXIT_STAGE, this, hash);

        GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_ROLE_LEVEL_UP, RoleLevelUp);

        GlobalBuffData.Instance.DeactBuffOutFight();
    }



    #endregion

    #region combo

    public float _ComboResetTime = 1.5f;
    public float _LastComboUpdate;

    private int _Combo = 0;
    public int Combo
    {
        get
        {
            return _Combo;
        }
    }

    public void SetCombo(int comboValue)
    {
        if (comboValue > _Combo)
        {
            _Combo = comboValue;
            _LastComboUpdate = TimeManager.Instance.FightTime;
        }
    }

    public void UpdateCombo()
    {
        if (TimeManager.Instance.FightTime - _LastComboUpdate > _ComboResetTime)
        {
            _Combo = 0;
        }
    }

    #endregion

    #region region teleport

    public string _PlayBornEffect = "Born2";

    public void TeleportToNextRegion(Transform destTrans, bool transScene = true)
    {
        FightManager.Instance.MainChatMotion.SetPosition(destTrans.position);
        FightManager.Instance.MainChatMotion.SetRotate(destTrans.rotation.eulerAngles);
        _SceneSPObj[_ActingRegion].SetActive(false);
        if (transScene)
        {
            ++_ActingRegion;
        }
        _SceneSPObj[_ActingRegion].SetActive(true);
        //_CameraFollow._Distance = LogicManager.Instance.EnterStageInfo.CameraOffset[_ActingRegion];

        //var effectPrefab = ResourceManager.Instance.GetEffect("Born2");
        //var effectSingle = effectPrefab.GetComponent<EffectSingle>();
        //var effectInstance = FightManager.Instance.MainChatMotion.PlayDynamicEffect(effectSingle);
        //effectInstance.transform.position = destTrans.position;


        if (_FightScene is FightSceneLogicPassArea)
        {
            (_FightScene as FightSceneLogicPassArea).StartNextArea();
        }
    }

    public void RoleLevelUp(object sender, Hashtable arg)
    {
        Hashtable hash = new Hashtable();
        if (FightManager.Instance != null && FightManager.Instance.MainChatMotion != null)
        {
            //hash.Add("WorldPos", FightManager.Instance.MainChatMotion.transform.position);
            //var effectID = FightManager.Instance.MainChatMotion.PlayDynamicEffect(_PlayBornEffect, hash);
        }
        
    }

    public void MoveToNewScene(string sceneName)
    {
        MonsterDrop.ClearAllDrops();
        var sceneCnt = SceneManager.sceneCount;
        //for (int i = 0; i < sceneCnt; ++i)
        {
            var sceneInfo = SceneManager.GetSceneByName(sceneName);

            //var spGO = GameObject.Find(sceneName + "_SP");
            //spGO.gameObject.SetActive(true);
            SceneManager.MoveGameObjectToScene(_CameraFollow.gameObject, sceneInfo);
            SceneManager.MoveGameObjectToScene(MainChatMotion.gameObject, sceneInfo);
            SceneManager.MoveGameObjectToScene(gameObject, sceneInfo);
            //SceneManager.MoveGameObjectToScene(_FightScene.gameObject, sceneInfo);

            SceneManager.SetActiveScene(sceneInfo);

        }
    }

    #endregion

    #region main

    public FightSkillManager _FightSkillManager;

    #endregion

    #region warning update 

    private MotionManager _EnemyMotion;
    private float _EnemyDistance;
    private Transform _NextAreaPos;
    private float _AreaDistance;
    private static float _WarningDistance = 12.0f;
    private float _StopFightTime = 0;
    private static float _WarningShowAfterFight = 6.0f;

    private void InitWarning()
    {
        _StopFightTime = Time.time;
    }

    private bool FindWarningEnemy()
    {
        if (_EnemyMotion != null && !_EnemyMotion.IsMotionDie)
        {
            float distance = Vector3.Distance(MainChatMotion.transform.position, _EnemyMotion.transform.position);
            if (distance < _WarningDistance)
            {
                _EnemyDistance = 0;
                _EnemyMotion = null;
                return false;
            }
        }

        _EnemyMotion = null;
        //var motions = GameObject.FindObjectsOfType<MotionManager>();
        _EnemyDistance = 99;
        foreach (var motion in _MonMotion)
        {
            if (!motion.IsMotionDie)
            {
                float distance = Vector3.Distance(MainChatMotion.transform.position, motion.transform.position);
                if (distance < _WarningDistance)
                    return false;

                if (distance < _EnemyDistance)
                {
                    _EnemyMotion = motion;
                    _EnemyDistance = distance;
                }
            }
        }

        return _EnemyMotion != null;
    }

    private bool FindNextArea()
    {
        var fightRandom = GameObject.FindObjectsOfType<AreaGateRandom>();
        _AreaDistance = 99;
        foreach (var areaGate in fightRandom)
        {
            float distance = Vector3.Distance(MainChatMotion.transform.position, areaGate.transform.position);
            if (distance < _WarningDistance)
            {
                _WarningDistance = 0;
                return false;
            }

            if (distance < _AreaDistance)
            {
                _NextAreaPos = areaGate.transform;
                _AreaDistance = distance;
            }

        }

        if (_NextAreaPos == null)
        {
            return false;
        }
        return true;
    }

    public void WarningUpdate()
    {
        if (MainChatMotion._ActionState != MainChatMotion._StateIdle
            && MainChatMotion._ActionState != MainChatMotion._StateMove)
        {
            _StopFightTime = Time.time;
            return;
        }

        if (_WarningShowAfterFight > Time.time - _StopFightTime)
            return;

        bool findWarningEnemy = FindWarningEnemy();
        bool findWarningArea = FindNextArea();

        Transform warningPos = null;
        if (findWarningEnemy && findWarningArea)
        {
            if (_AreaDistance > _EnemyDistance)
            {
                warningPos = _EnemyMotion.transform;
            }
            else
            {
                warningPos = _NextAreaPos;
            }
        }
        else if (findWarningEnemy && _WarningDistance > 0)
        {
            warningPos = _EnemyMotion.transform;
        }
        else if (findWarningArea && _EnemyDistance > 0)
        {
            warningPos = _NextAreaPos;
        }

        if (FightManager.Instance.MainChatMotion != null)
        {
            UIFightWarning.ShowDirectAsyn(FightManager.Instance.MainChatMotion.transform, warningPos);
            _StopFightTime = Time.time;
        }
    }

    #endregion
}
