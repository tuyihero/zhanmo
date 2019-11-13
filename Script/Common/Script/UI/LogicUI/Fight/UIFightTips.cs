using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIFightTips : UIPopBase
{
    #region static
    
    
    public static void ShowAsyn(int showGroup = -1)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ShowGroup", showGroup);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFightTips, UILayer.MessageUI, hash);
    }

    #endregion

    #region 

    public UIFightTipsItem _AtkTips;
    public UIFightTipsItem _SkillTips;
    public UIFightTipsItem _BuffTips;
    public UIFightTipsItem _DeBuffTips;
    public UIFightTipsItem _DodgeTips;
    public UIFightTipsItem _DefenceTips;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        int showGroup = (int)hash["ShowGroup"];

        _AtkTips.gameObject.SetActive(true);
        _SkillTips.gameObject.SetActive(true);
        _BuffTips.gameObject.SetActive(true);
        _DeBuffTips.gameObject.SetActive(true);
        _DodgeTips.gameObject.SetActive(true);
        _DefenceTips.gameObject.SetActive(true);
        if (showGroup == 1)
        {
            _BuffTips.gameObject.SetActive(false);
            _DeBuffTips.gameObject.SetActive(false);
            _DodgeTips.gameObject.SetActive(false);
            _DefenceTips.gameObject.SetActive(false);
        }
        else if(showGroup == 2)
        {
            _AtkTips.gameObject.SetActive(false);
            _SkillTips.gameObject.SetActive(false);
            _DodgeTips.gameObject.SetActive(false);
            _DefenceTips.gameObject.SetActive(false);
        }
        else if (showGroup == 3)
        {
            _AtkTips.gameObject.SetActive(false);
            _SkillTips.gameObject.SetActive(false);
            _BuffTips.gameObject.SetActive(false);
            _DeBuffTips.gameObject.SetActive(false);
        }
    }

    #endregion
}

