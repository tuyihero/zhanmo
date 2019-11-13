using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// 游戏核心
/// </summary>
public class GameCore : MonoBehaviour
{
    #region 固有

    public void Awake()
    {
        DontDestroyOnLoad(this);
        Application.runInBackground = false;
        Application.targetFrameRate = 60;
        _Instance = this;
    }

    public void Start()
    {
        ResourceManager.InitResourceManager();
        StartCoroutine(UpdateInit());
    }

    public void Update()
    {
        UpdateQuit();
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.C))
        {
            LogicManager.Instance.CleanUpSave();
        }

#endif
    }

    void UpdateQuit()
    {
        if ((Application.platform == RuntimePlatform.Android
        || Application.platform == RuntimePlatform.WindowsPlayer
        || Application.platform == RuntimePlatform.WindowsEditor) && (Input.GetKeyDown(KeyCode.Escape)))
        {
            if (FightManager.Instance != null)
            {
                if (FightManager.Instance.MainChatMotion != null
                    && (FightManager.Instance.MainChatMotion._ActionState == FightManager.Instance.MainChatMotion._StateMove
                    || FightManager.Instance.MainChatMotion._ActionState == FightManager.Instance.MainChatMotion._StateSkill))
                { }
                else
                {
                    UIMessageBox.Show(1000007, () =>
                    {
                        FightManager.Instance.LogicFinish(true);
                        Debug.Log("save data");
                    }, null);
                }
            }
            else
            {
                UIMessageBox.Show(1000006, () =>
                {
                    LogicManager.Instance.QuitGame();
                    Debug.Log("save data");
                }, null);
            }

        }
    }

    

    void OnApplicationQuit()
    {
        LogicManager.Instance.QuitGame();
    }
    #endregion

    #region start logic

    IEnumerator UpdateInit()
    {
        while (!_HasInitLogic)
        {
            if (ResourceManager.Instance != null && ResourcePool.Instance != null)
            {
                StartGameLogic();
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    bool _HasInitLogic = false;
    void StartGameLogic()
    {
        InitLanguage();
        Tables.TableReader.ReadTables();
        UILogin.ShowAsyn();
        DataRecordManager.Instance.InitDataRecord();
        AdManager.InitAdManager();
    }

    #endregion

    #region 唯一

    private static GameCore _Instance = null;
    public static GameCore Instance
    {
        get
        {
            return _Instance;
        }
    }

    #endregion

    #region 管理者

    /// <summary>
    /// 主UI画布
    /// </summary>
    [SerializeField]
    private UIManager _UIManager;
    public UIManager UIManager { get { return _UIManager; } }

    [SerializeField]
    private EventController _EventController;
    public EventController EventController { get { return _EventController; } }

    public SoundManager _SoundManager;

    #endregion

    #region 

    public int _StrVersion = 0;
    public bool _IsTestMode = true;

    public void InitLanguage()
    {
//#if UNITY_EDITOR
//        _StrVersion = 0;
//        return;
//#else
        if (Application.systemLanguage == SystemLanguage.Chinese
            || Application.systemLanguage == SystemLanguage.ChineseSimplified)
        {
            _StrVersion = 1;
        }
        else if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
        {
            _StrVersion = 2;
        }
        else
        {
            _StrVersion = 0;
        }
//#endif
    }

#endregion

}

