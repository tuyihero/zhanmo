
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UITestAttr : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UITestAttr, UILayer.PopUI, hash);
    }

    #endregion

    #region 

    public InputField _AttrType;
    public InputField _Value1;
    public InputField _Value2;
    public InputField _Value3;
    public InputField _Value4;

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
    }

    public override void Hide()
    {
        base.Hide();

        UIEquipTooltips.HideAsyn();
    }

    public void OnBtnOk()
    {
        var impactType = Type.GetType(_AttrType.text);
        if (impactType == null)
            return;

        var impactBase = Activator.CreateInstance(impactType) as RoleAttrImpactBase;
        if (impactBase == null)
            return;

        List<int> paramList = new List<int>();
        paramList.Add(int.Parse(_Value1.text));
        paramList.Add(int.Parse(_Value2.text));
        paramList.Add(int.Parse(_Value3.text));
        paramList.Add(int.Parse(_Value4.text));
        impactBase.InitImpact("", paramList);

        RoleData.SelectRole._BaseAttr.AddExAttr(impactBase);
    }


    #endregion

}

