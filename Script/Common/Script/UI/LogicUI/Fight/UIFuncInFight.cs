
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;
using Tables;

public class UIFuncInFight : UIBase
{

    #region static

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFuncInFight, UILayer.BaseUI, hash);

    }

    public static void StopFightTime()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFuncInFight>(UIConfig.UIFuncInFight);
        if (instance == null)
            return;

        instance.CancelInvoke("UpdateFightTime");
        Debug.Log("FightTime:" + instance._FightSecond);
    }

    public static void UpdateSkillInfoUI()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFuncInFight>(UIConfig.UIFuncInFight);
        if (instance == null)
            return;

        instance.UpdateSkillInfo();
    }

    public static void UpdateKillMonster(int curCnt, int maxCnt)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFuncInFight>(UIConfig.UIFuncInFight);
        if (instance == null)
            return;

        instance.SetMonsterKillCnt(curCnt, maxCnt);
    }

    public static void UpdateKillEliteMonster(int curCnt, int maxCnt)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFuncInFight>(UIConfig.UIFuncInFight);
        if (instance == null)
            return;

        instance.SetEliteMonsterKillCnt(curCnt, maxCnt);
    }

    public static void UpdateGoldActInfo(int kMonster, int getGold)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFuncInFight>(UIConfig.UIFuncInFight);
        if (instance == null)
            return;

        instance.SetGoldActInfo(kMonster, getGold);
    }

    #endregion

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        UpdateSkillInfo();

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_PASS_STAGE, EventDelegate);
    }

    private void Update()
    {
        UpdateFightTime();
    }

    public override void Destory()
    {
        base.Destory();

        GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_PASS_STAGE, EventDelegate);
    }

    public void OnBtnExit()
    {
        //UIMessageBox.Show(100000, OnExitOk, null);
        //UIFightSetting.ShowAsyn();
        UISystemSetting.ShowAsyn(true);
    }

    private void OnExitOk()
    {
        Debug.Log("exit");
        LogicManager.Instance.ExitFight();
    }

    #region fight time

    public Text _FightTime;

    public int _FightSecond = 0;

    private void UpdateFightTime()
    {
        
        DateTime dateTime = new DateTime((long)(FightManager.Instance._LogicFightTime * 10000000L));
        if (FightManager.Instance._LogicFightTime > 10)
        {
            _FightTime.text = StrDictionary.GetFormatStr(2402002, string.Format("{0:mm:ss}", dateTime));
        }
        else
        {
            _FightTime.text = StrDictionary.GetFormatStr(2402003, string.Format("{0:mm:ss}", dateTime));
        }
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        Debug.Log("Fight finish time:" + _FightSecond);

    }

    #endregion

    #region  skill info

    public List<UIFightSkillInfo> _UIFightInfos;

    public void UpdateSkillInfo()
    {
        int showIdx = FightSkillManager.Instance.FightSkillDict.Count;
        for (int i = 0; i < _UIFightInfos.Count; ++i)
        {
            --showIdx;
            if (showIdx >= 0)
            {
                if (FightSkillManager.Instance.FightSkillDict[showIdx]._ShowInUI)
                {
                    _UIFightInfos[i].gameObject.SetActive(true);
                    _UIFightInfos[i].InitSkillInfo(FightSkillManager.Instance.FightSkillDict[showIdx]);
                }
                else
                {
                    --showIdx;
                }
            }
            else
            {
                _UIFightInfos[i].gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #region kill monster

    public Text _KillMonsterText;
    public Text _KillEliteText;
    public GameObject _BossConditionGO;

    private void SetMonsterKillCnt(int curCnt, int maxCnt)
    {
        _ActGoldInfo.SetActive(false);
        if (curCnt < 0)
        {
            _BossConditionGO.SetActive(false);
            return;
        }
        else
        {
            _BossConditionGO.SetActive(true);
        }

        if (maxCnt < 0 && curCnt < 0)
        {
            _KillMonsterText.text = "";
        }
        else if (maxCnt < 0 && curCnt >= 0)
        {
            _KillMonsterText.text = curCnt.ToString();
        }
        else 
        {
            _KillMonsterText.text = Tables.StrDictionary.GetFormatStr(2402000, curCnt, maxCnt);
        }
    }

    private void SetEliteMonsterKillCnt(int curCnt, int maxCnt)
    {
        _ActGoldInfo.SetActive(false);
        if (curCnt < 0)
        {
            return;
        }

        if (maxCnt < 0 && curCnt < 0)
        {
            _KillEliteText.text = "";
        }
        else if (maxCnt < 0 && curCnt >= 0)
        {
            _KillEliteText.text = curCnt.ToString();
        }
        else
        {
            _KillEliteText.text = Tables.StrDictionary.GetFormatStr(2402001, curCnt, maxCnt);
        }
    }

    #endregion

    #region gold act

    public Text _KMonsterCnt;
    public Text _GetGold;
    public GameObject _ActGoldInfo;

    public void SetGoldActInfo(int killMonster, int getGold)
    {
        _ActGoldInfo.SetActive(true);
        _BossConditionGO.SetActive(false);

        _KMonsterCnt.text = Tables.StrDictionary.GetFormatStr(2402004, killMonster);
        _GetGold.text = Tables.StrDictionary.GetFormatStr(2402005, getGold);
    }


    #endregion
}

