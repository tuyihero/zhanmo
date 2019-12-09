using UnityEngine;
using System.Collections;
using System;

using Tables;
using UnityEngine.SceneManagement;

public class LogicManager
{
    #region 唯一

    private static LogicManager _Instance = null;
    public static LogicManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new LogicManager();
            }
            return _Instance;
        }
    }

    private LogicManager() { }

    #endregion

    #region start logic

    public void StartLoadLogic()
    {
        SceneManager.LoadScene(GameDefine.GAMELOGIC_SCENE_NAME);

        PlayerDataPack.Instance.LoadClass(true);
        PlayerDataPack.Instance.InitPlayerData();
    }

    public void StartLoadRole(int idx)
    {
        FunTipData.Instance.LoadClass(true);
        FunTipData.Instance.InitFunTipData();

        GemData.Instance.LoadClass(true);
        GemData.Instance.InitGemData();

        LegendaryData.Instance.LoadClass(true);
        LegendaryData.Instance.InitLegendaryEquips();

        FiveElementData.Instance.LoadClass(true);
        FiveElementData.Instance.InitFiveElementData();

        PlayerDataPack.Instance.SelectRole(idx);

        //BackBagPack.Instance.LoadClass(true);
        BackBagPack.Instance.InitBackPack();

        ShopData.Instance.LoadClass(true);
        ShopData.Instance.InitShop();

        SummonSkillData.Instance.LoadClass(true);
        SummonSkillData.Instance.InitSummonSkillData();

        ActData.Instance.LoadClass(true);
        ActData.Instance.InitActData();

        MissionData.Instance.LoadClass(true);
        MissionData.Instance.InitMissionData();

        AchievementData.Instance.LoadClass(true);
        AchievementData.Instance.InitMissionData();

        GlobalBuffData.Instance.LoadClass(true);
        GlobalBuffData.Instance.InitGlobalBuffData();

        ItemPackTest.Instance.LoadClass(true);
        ItemPackTest.Instance.Init();

        GiftData.Instance.LoadClass(true);
        GiftData.Instance.InitGiftData();

        UIMainFun.ShowAsyn();

        PurchManager.Instance.InitIAPInfo();
    }

    #endregion

    #region

    public void StartLogic()
    {
        if (PlayerDataPack.Instance._SelectedRole == null)
        {
            UIRoleSelect.ShowAsyn();
        }
        else
        {
            UIMainFun.ShowAsyn();

            //UIGiftTipPack.ShowAsyn();
        }
        GameCore.Instance._SoundManager.PlayBGMusic(GameCore.Instance._SoundManager._LogicAudio);
    }

    public void SaveGame()
    {
        //PlayerDataPack.Instance.SaveClass(false);
        //BackBagPack.Instance.SaveClass(true);
        //ShopData.Instance.SaveClass(true);
        //GemData.Instance.SaveClass(true);


        //if (PlayerDataPack.Instance._SelectedRole != null)
        //{
        //    PlayerDataPack.Instance._SelectedRole.SaveClass(false);
        //}
        //DataManager.Instance.Save();
    }

    public void QuitGame()
    {
        try
        {
            SaveGame();
            DataLog.StopLog();
            Application.Quit();
        }
        catch (Exception e)
        {
            Application.Quit();
        }
    }

    public void CleanUpSave()
    {

    }
    #endregion

    #region Fight

    private StageInfoRecord _EnterStageInfo;
    public StageInfoRecord EnterStageInfo
    {
        get
        {
            return _EnterStageInfo;
        }
    }

    public void EnterFight(StageInfoRecord enterStage)
    {
        _EnterStageInfo = enterStage;
        //var sceneLoader = GameCore.Instance.SceneManager.ChangeFightScene(_EnterStageInfo.ScenePath);

        GameCore.Instance.UIManager.HideAllUI();

        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_ENTER_STAGE, this, null);

        UILoadingScene.ShowEnterFightAsyn();
    }

    public void EnterFightFinish()
    {
        //UIHPPanel.ShowAsyn();
        //UIControlPanel.ShowAsyn();
        //UIJoyStick.ShowAsyn();
        UIDirectControl.ShowAsyn();
        UISkillBar.ShowAsyn();
        //UIDropNamePanel.ShowAsyn();
        UIPlayerFrame.ShowAsyn();
        //UIFuncInFight.ShowAsyn();

        GameCore.Instance._SoundManager.PlayBGMusic(EnterStageInfo.Audio);
    }

    public void ExitFight()
    {
        //GameCore.Instance.UIManager.DestoryAllUI();
        //GameObject.Destroy(FightManager.Instance.gameObject);
        //UILoadingScene.ShowAsyn(GameDefine.GAMELOGIC_SCENE_NAME);

        UISkillBar.HideAsyn();
        UIMainFun.ShowAsynInFight();
    }

    public void ExitFightScene()
    {
        GameCore.Instance.UIManager.DestoryAllUI();
        DestoryFightLogic();
        UILoadingScene.ShowAsyn(GameDefine.GAMELOGIC_SCENE_NAME);

        //UIMainFun.ShowAsynInFight();
    }

    public void DestoryFightLogic()
    {
        if (FightManager.Instance != null)
        {
            GameObject.Destroy(FightManager.Instance.gameObject);
        }
    }

    public void InitFightScene()
    {
        DestoryFightLogic();
        GameObject fightGO = new GameObject("FightManager");
        fightGO.AddComponent<FightManager>();
    }


    #endregion
}

