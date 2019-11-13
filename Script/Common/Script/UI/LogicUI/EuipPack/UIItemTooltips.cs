
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;
using Tables;

public class ToolTipFunc
{
    public ToolTipFunc(int strIDX, CallBackFunc func)
    {
        _FuncName = StrDictionary.GetFormatStr(strIDX);
        _Func = func;
    }
    public string _FuncName;
    public delegate void CallBackFunc(ItemBase itemBase);
    public CallBackFunc _Func;
}

public class UIItemTooltips : UIBase
{

    #region static funs

    public static void ShowAsyn(ItemBase itembase, params ToolTipFunc[] funcs)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ItemBase", itembase);
        hash.Add("ToolTipFun", funcs);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIItemTooltips, UILayer.MessageUI, hash);
    }

    public static void ShowShopAsyn(ItemBase itembase, bool isBuy, MONEYTYPE priceType, int priceValue,  params ToolTipFunc[] funcs)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ItemBase", itembase);
        hash.Add("IsBuy", isBuy);
        hash.Add("PriceType", priceType);
        hash.Add("PriceValue", priceValue);
        hash.Add("ToolTipFun", funcs);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIItemTooltips, UILayer.MessageUI, hash);
    }

    public static void HideAsyn()
    {
        UIManager.Instance.HideUI(UIConfig.UIItemTooltips.AssetPath);
    }

    #endregion

    #region 

    public UIItemInfo _UIItemInfo;

    public GameObject _BtnPanel;

    public Button[] _BtnGO;
    public Text[] _BtnText;

    #endregion

    #region 

    protected ItemBase _ShowItem;
    protected ToolTipFunc[] _ShowFuncs;
    protected bool _HideAfterBtn = true;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        _ShowItem = hash["ItemBase"] as ItemBase;
        ToolTipFunc[] showType = (ToolTipFunc[])hash["ToolTipFun"];
        ShowTips(_ShowItem);
        ShowFuncs(showType);

        if (_UIItemInfo != null)
        {
            if (hash.ContainsKey("IsBuy"))
            {
                var isBuy = (bool)hash["IsBuy"];
                var priceType = (MONEYTYPE)hash["PriceType"];
                var priceValue = (int)hash["PriceValue"];
                _UIItemInfo.ShowPrice(isBuy, priceType, priceValue);
            }
        }
    }

    protected virtual void ShowFuncs(ToolTipFunc[] funcs)
    {
        _ShowFuncs = funcs;
        if (funcs.Length == 0)
        {
            SetGOActive(_BtnPanel, false);
        }
        else
        {
            SetGOActive(_BtnPanel, true);
            for (int i = 0; i < _BtnGO.Length; ++i)
            {
                if (i < funcs.Length)
                {
                    SetGOActive(_BtnGO[i], true);
                    _BtnText[i].text = funcs[i]._FuncName;
                }
                else
                {
                    SetGOActive(_BtnGO[i], false);
                }
            }
        }
    }

    protected virtual void ShowTips(ItemBase itemBase)
    {
        if (itemBase == null)
        {
            _ShowItem = null;
            return;
        }
        _ShowItem = itemBase;

        if (_UIItemInfo != null)
        {
            _UIItemInfo.ShowTips(_ShowItem);
        }
    }

    #endregion

    #region operate

    public void OnBtnFunc(int idx)
    {
        _ShowFuncs[idx]._Func.Invoke(_ShowItem);
        if (_HideAfterBtn)
        {
            Hide();
        }
    }

    #endregion

}

