using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Tables;

public class UIGemCombineSet : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIGemCombineSet, UILayer.SubPopUI, hash);
    }

    public static void HideAsyn()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGemCombineSet>(UIConfig.UIGemCombineSet);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.Hide();
    }

    #endregion

    #region 

    public UIContainerBase _GemSuitContainer;

    #endregion

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        ShowPackItems();
    }

    private void ShowPackItems()
    {
        List<GemTableRecord> formulas = new List<GemTableRecord>();
        foreach (var gemRecord in GemData.Instance.GemFormulas.Keys)
        {
            if (gemRecord.Level == 1)
            {
                formulas.Add(gemRecord);
            }
        }
        _GemSuitContainer.InitContentItem(formulas);
    }
    
}

