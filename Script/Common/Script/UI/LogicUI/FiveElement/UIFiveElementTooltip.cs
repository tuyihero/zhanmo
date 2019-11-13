
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIFiveElementTooltip : UIItemTooltips
{

    #region static funs

    public static void ShowAsyn(ItemFiveElement itemElement, params ToolTipFunc[] funcs)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ItemElement", itemElement);
        hash.Add("ToolTipFun", funcs);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFiveElementTooltip, UILayer.MessageUI, hash);
    }

    public static void ShowAsynInType(ItemFiveElement itemElement, TooltipType toolTipType, params ToolTipFunc[] funcs)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ItemElement", itemElement);
        hash.Add("ToolTipFun", funcs);
        hash.Add("TooltipType", toolTipType);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFiveElementTooltip, UILayer.MessageUI, hash);
    }

    public new static void ShowShopAsyn(ItemFiveElement itemElement, bool isBuy, MONEYTYPE priceType, int priceValue, params ToolTipFunc[] funcs)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ItemElement", itemElement);
        hash.Add("IsBuy", isBuy);
        hash.Add("PriceType", priceType);
        hash.Add("PriceValue", priceValue);
        hash.Add("ToolTipFun", funcs);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFiveElementTooltip, UILayer.MessageUI, hash);
    }

    public new static void HideAsyn()
    {
        UIManager.Instance.HideUI("LogicUI/FiveElement/UIFiveElementTooltips");
    }

    #endregion

    #region 

    public UIFiveElementInfo _UIElementInfo;
    public UIFiveElementInfo _CompareInfo;

    private ItemFiveElement _ShowElement;
    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        _ShowItem = hash["ItemElement"] as ItemFiveElement;
        ToolTipFunc[] funcs = (ToolTipFunc[])hash["ToolTipFun"];
        ShowTips(_ShowItem as ItemFiveElement);

        if (hash.Contains("TooltipType"))
        {
            var toolTipType = (TooltipType)hash["TooltipType"];
            if (toolTipType == TooltipType.Compare)
            {
                ShowCompare();
            }
            else
            {
                HideCompare();
            }
        }
        else
        {
            ShowCompare();
        }
        ShowFuncs(funcs);

        if (hash.ContainsKey("IsBuy"))
        {
            var isBuy = (bool)hash["IsBuy"];
            var priceType = (MONEYTYPE)hash["PriceType"];
            var priceValue = (int)hash["PriceValue"];
            _UIElementInfo.ShowPrice(isBuy, priceType, priceValue);
        }
    }
    
    private void ShowTips(ItemFiveElement itemElement)
    {
        if (itemElement == null)
        {
            _ShowItem = null;
            return;
        }
        _ShowItem = itemElement;
        _ShowElement = itemElement;

        _UIElementInfo.ShowTips(_ShowElement);
    }

    private void ShowCompare()
    {
        HideCompare();
        if (_ShowElement == null)
            return;

        //var usingItem = FiveElementData.Instance._UsingElements[(int)_ShowElement.FiveElementRecord.EvelemtType];
        //if (usingItem == null || !usingItem.IsVolid())
        //    return;

        //if (usingItem == _ShowElement)
        //    return;

        //_CompareInfo.gameObject.SetActive(true);
        //_CompareInfo.ShowTips(usingItem);
    }

    private void HideCompare()
    {
        _CompareInfo.gameObject.SetActive(false);
    }

    #endregion


}

