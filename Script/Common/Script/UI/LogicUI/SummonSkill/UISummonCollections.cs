using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Tables;

public class UISummonCollections : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UISummonCollections, UILayer.SubPopUI, hash);
    }

    #endregion

    #region 

    public UIContainerBase _SummonContainer;
    public Text _Tips;
    public Text _CurTips;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        ShowCollectPanel();
    }

    private void ShowCollectPanel()
    {
        _SummonContainer.InitContentItem(SummonSkillData.Instance._CollectionItems);
        _Tips.text = StrDictionary.GetFormatStr(1200004);
        _CurTips.text = StrDictionary.GetFormatStr(1200005, SummonSkillData.Instance._TotalCollectStars * 0.1f);
    }

    public void RefreshItems()
    {
        
    }

    #endregion

}

