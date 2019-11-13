using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 
 
using System;



public class UIFightFinish : UIBase
{

    #region static funs

    public static void ShowAsyn(bool isWin)
    {
        Hashtable hash = new Hashtable();
        hash.Add("IsWin", isWin);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFightFinish, UILayer.BaseUI, hash);
    }

    #endregion

    #region 



    #endregion

    #region 

    public GameObject _WinGO;
    public GameObject _LoseGO;
    public Animation _Animation;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        bool isWin = (bool)hash["IsWin"];
        if (isWin)
        {
            _WinGO.SetActive(true);
            _LoseGO.SetActive(false);
            _Animation.Play("UIFightFinishWin");
        }
        else
        {
            _WinGO.SetActive(false);
            _LoseGO.SetActive(true);
            _Animation.Play("UIFightFinishLose");
        }

        UIFuncInFight.StopFightTime();
    }

    public void OnEnable()
    {

    }

    #endregion

    #region event

    public void OnBtnExitFight()
    {
        FightManager.Instance.LogicFinish(true);
        Hide();
    }

    #endregion
}

