
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIFiveElementCoreTooltip : UIItemTooltips
{

    #region static funs

    public static void ShowAsyn(ItemFiveElementCore itemElement, params ToolTipFunc[] funcs)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ItemElementCore", itemElement);
        hash.Add("ToolTipFun", funcs);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFiveElementCoreTooltip, UILayer.MessageUI, hash);
    }

    public static void ShowAsynInType(ItemFiveElementCore itemElement, TooltipType toolTipType, params ToolTipFunc[] funcs)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ItemElementCore", itemElement);
        hash.Add("ToolTipFun", funcs);
        hash.Add("TooltipType", toolTipType);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFiveElementCoreTooltip, UILayer.MessageUI, hash);
    }

    public new static void HideAsyn()
    {
        UIManager.Instance.HideUI(UIConfig.UIFiveElementCoreTooltip.AssetPath);
    }

    #endregion

    #region 

    public UIFiveElementCoreInfo _UIElementInfo;
    public UIFiveElementCoreInfo _CompareInfo;

    private ItemFiveElementCore _ShowElement;
    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        _ShowItem = hash["ItemElementCore"] as ItemFiveElementCore;
        ToolTipFunc[] funcs = (ToolTipFunc[])hash["ToolTipFun"];
        ShowTips(_ShowItem as ItemFiveElementCore);

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
    }
    
    private void ShowTips(ItemFiveElementCore itemElement)
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

        var usingItem = FiveElementData.Instance._UsingCores[(int)_ShowElement.FiveElementCoreRecord.ElementType];
        if (usingItem == null || !usingItem.IsVolid())
            return;

        if (usingItem == _ShowElement)
            return;

        _CompareInfo.gameObject.SetActive(true);
        _CompareInfo.ShowTips(usingItem);
    }

    private void HideCompare()
    {
        _CompareInfo.gameObject.SetActive(false);
    }

    #endregion


}

