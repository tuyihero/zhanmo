using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIStageDiffTips : UIPopBase
{
    #region static

    public static int _LastShowDiff = -1;
    public static bool _IsOnlyShowDiff;
    
    public static void ShowAsyn(Action callBack = null)
    {
        Hashtable hash = new Hashtable();
        hash.Add("CallBack", callBack);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIStageDiffTips, UILayer.MessageUI, hash);
    }

    public static void ShowForEnsure(int stageID, Action callBack)
    {
        int stageDiff = GameDataValue.GetStageDiff(stageID, Tables.STAGE_TYPE.NORMAL);
        if (_IsOnlyShowDiff)
        {
            if (_LastShowDiff < stageDiff)
            {
                _LastShowDiff = stageDiff;
                _LastShowDiff = Mathf.Clamp(_LastShowDiff, 0, 9);
                ShowAsyn(callBack);
            }
            else
            {
                if (callBack != null)
                {
                    callBack.Invoke();
                }
            }
        }
        else
        {
            _LastShowDiff = stageDiff;
            _LastShowDiff = Mathf.Clamp(_LastShowDiff, 0, 9);
            ShowAsyn(callBack);
        }
    }

    #endregion

    #region 

    public UIContainerSelect _TipsContainer;
    public Toggle _OnlyShowDiff;
    public Action _CallBack;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        if (hash.ContainsKey("CallBack"))
        {
            _CallBack = (Action)hash["CallBack"];
        }
        List<int> strTips = new List<int>();
        for (int i = 0; i <= 9; ++i)
        {
            strTips.Add(i);
        }
        _TipsContainer.InitSelectContent(strTips, new List<int>() { _LastShowDiff });

        _OnlyShowDiff.isOn = _IsOnlyShowDiff;
    }

    public void CloseTip()
    {
        if (_CallBack != null)
        {
            _CallBack.Invoke();
        }

        if (gameObject.activeSelf)
        {
            StartCoroutine(HideAfterWile());
        }
    }

    public IEnumerator HideAfterWile()
    {
        yield return null;
        base.Hide();
    }

    public void OnOnlyShowDiffOn()
    {
        _IsOnlyShowDiff = _OnlyShowDiff.isOn;
    }

    #endregion
}

