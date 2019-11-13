using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using Tables;

public enum LoadSceneStep
{
    StartLoad,
    LoadSceneRes,
    InitScene,
}

public class UILoadingScene : UIBase
{
    #region static funs

    public static void ShowAsyn(string sceneName)
    {
        Hashtable hash = new Hashtable();
        hash.Add("SceneName", sceneName);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UILoadingScene, UILayer.TopUI, hash);
    }

    public static void ShowEnterFightAsyn()
    {
        Hashtable hash = new Hashtable();
        
        GameCore.Instance.UIManager.ShowUI(UIConfig.UILoadingScene, UILayer.TopUI, hash);
    }

    #endregion

    #region 

    public Image _BG;
    public Text _NameText;
    public Text _Tips;
    public Slider _LoadProcess;

    private string _LoadingSceneName;
    private bool _IsEnterFight;
    private float _StartTime;
    private float _ShowADTime;
    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        
        ShowBG();

        _StartTime = Time.time;
        if (hash.ContainsKey("SceneName"))
        {
            _IsEnterFight = false;
            _LoadingSceneName = (string)hash["SceneName"];
            //StartCoroutine(InitializeLevelAsync(_LoadSceneName, true, LoadSceneResFinish, hash));
            StartCoroutine( ResourceManager.Instance.LoadLevelAsync(_LoadingSceneName, false));
        }
        else
        {
            _IsEnterFight = true;
            LogicManager.Instance.InitFightScene();
        }

        _ShowADTime = 0;
        AdManager.Instance.PrepareInterAD();
    }
    
    public void FixedUpdate()
    {
        if (_IsEnterFight)
        {
            _LoadProcess.value = FightManager.Instance.InitProcess;
            if (AdManager.Instance.IsShowInterAD)
            {
                //Debug.LogError("AdManager.Instance.ShowInterAD");
                if (FightManager.Instance._InitStep > FightManager.InitStep.InitSceneFinish && _ShowADTime == 0)
                {
                    _ShowADTime = Time.time;
                    AdManager.Instance.ShowInterAD();
                }

                if (_ShowADTime > 0 && AdManager.Instance.IsShowInterADFinish()&& FightManager.Instance.InitProcess == 1)
                {
                    AdManager.Instance.AddLoadSceneTimes();

                    LogicManager.Instance.EnterFightFinish();
                    //AdManager.Instance.DisposeAds();
                    base.Destory();
                }
            }
            else if (FightManager.Instance.InitProcess == 1)
            {
                AdManager.Instance.AddLoadSceneTimes();

                LogicManager.Instance.EnterFightFinish();
                base.Destory();
            }
        }
        else
        {
            _LoadProcess.value = (Time.time - _StartTime) * 0.66f;
            if (AdManager.Instance.IsShowInterAD)
            {
                if (_LoadProcess.value >= 1 && SceneManager.GetActiveScene().name == _LoadingSceneName)
                {
                    if (FightManager.Instance._InitStep > FightManager.InitStep.InitSceneFinish && _ShowADTime == 0)
                    {
                        _ShowADTime = Time.time;
                        AdManager.Instance.ShowInterAD();
                    }

                    if (_ShowADTime > 0 && AdManager.Instance.IsShowInterADFinish())
                    {
                        AdManager.Instance.AddLoadSceneTimes();

                        LogicManager.Instance.StartLogic();
                        base.Destory();
                    }
                }
            }
            else
            {
                AdManager.Instance.AddLoadSceneTimes();

                LogicManager.Instance.StartLogic();
                base.Destory();
            }
        }
    }
    
    #endregion

    #region 

    public void ShowBG()
    {
        var imageTips = TableReader.LoadingTips.GetRandomImageTips();
        var textTips = TableReader.LoadingTips.GetRandomTextTips(RoleData.SelectRole.TotalLevel);
        ResourceManager.Instance.SetImage(_BG, imageTips.ImagePath);
        _NameText.text = StrDictionary.GetFormatStr(imageTips.ImageName);
        _Tips.text = StrDictionary.GetFormatStr(textTips.TipsStr);
    }

    #endregion
}

