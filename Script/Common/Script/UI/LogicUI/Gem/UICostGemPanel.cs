using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UICostGemPanel : UIBase
{

    #region static funs

    public static void ShowAsyn(SelectGemCallBack selectGemCallBack)
    {
        Hashtable hash = new Hashtable();
        hash.Add("SelectGemCallBack", selectGemCallBack);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UICostGemPanel, UILayer.SubPopUI, hash);
    }

    #endregion

    #region 

    public delegate void SelectGemCallBack(ItemGem itemGem);
    private SelectGemCallBack _SelectGemCallBack;

    public UIContainerBase _GemContainer;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        _SelectGemCallBack = (SelectGemCallBack)hash["SelectGemCallBack"];
        List<ItemGem> matGems = new List<ItemGem>();
        foreach (var gemData in GemData.Instance.PackGemDatas._PackItems)
        {
            if (gemData.IsVolid() && gemData.GemRecord.Level == ItemGem._MaxGemLevel)
            {
                matGems.Add(gemData);
            }
        }
        _GemContainer.InitContentItem(matGems, OnPackItemClick);
    }

    private void OnPackItemClick(object gemItemObj)
    {
        ItemGem gemItem = gemItemObj as ItemGem;
        if (gemItem == null)
            return;

        if (_SelectGemCallBack != null)
        {
            _SelectGemCallBack.Invoke(gemItem);
        }
        Hide();
    }

    #endregion
}

