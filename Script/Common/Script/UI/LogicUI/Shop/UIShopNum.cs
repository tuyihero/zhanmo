using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIShopNum : UIBase
{
    public delegate void ShopNumCallBack(ItemShop itemShop, int num);

    public static void ShowAsyn(ItemShop itemShop, ShopNumCallBack callBack)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ItemShop", itemShop);
        hash.Add("CallBack", callBack);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIShopNum, UILayer.MessageUI, hash);
    }

    #region 

    public InputField _InputField;
    public Slider _NumProcess;
    public UICurrencyItem _UICurrencyItem;

    private int _Value;
    public int Value
    {
        get
        {
            _Value = int.Parse(_InputField.text);
            return _Value;
        }
        set
        {
            _Value = value;
            _InputField.text = _Value.ToString();
        }
    }

    private int _MaxValue = -1;
    private int _MinValue = 0;
    private ShopNumCallBack _CallBack;
    private ItemShop _ItemShop;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _ItemShop = (ItemShop)hash["ItemShop"];
        _CallBack = (ShopNumCallBack)hash["CallBack"];

        int minValue = 1;
        int maxValue = 100;

        if (_ItemShop.ShopRecord.MoneyType == 0)
        {
            maxValue = PlayerDataPack.Instance.Gold / _ItemShop.BuyPrice;
        }
        else
        {
            maxValue = PlayerDataPack.Instance.Diamond / _ItemShop.BuyPrice;
        }
        maxValue = Mathf.Clamp(maxValue, 1, 100);

        Init(minValue, minValue, maxValue);
    }

    public void Init(int initValue, int minValue, int maxValue)
    {
        _MaxValue = maxValue;
        _MinValue = minValue;
        Value = initValue;

        //_NumProcess.minValue = _MinValue;
        //_NumProcess.maxValue = _MaxValue;
        //_NumProcess.value = initValue;

        _UICurrencyItem.ShowCurrency((MONEYTYPE)_ItemShop.ShopRecord.MoneyType, _ItemShop.BuyPrice * initValue);
    }

    public void OnSlide()
    {
        int num = (int)_NumProcess.value;
        _InputField.text = num.ToString();

        _UICurrencyItem.ShowCurrency((MONEYTYPE)_ItemShop.ShopRecord.MoneyType, _ItemShop.BuyPrice * num);
    }

    public void OnTextInput()
    {
        //_NumProcess.value = Value;

        int num = int.Parse(_InputField.text);
        _UICurrencyItem.ShowCurrency((MONEYTYPE)_ItemShop.ShopRecord.MoneyType, (int)(_ItemShop.BuyPrice * num));
    }

    public void OnBtnOk()
    {
        if (_CallBack != null)
        {
            _CallBack.Invoke(_ItemShop, Value);
        }

        Hide();
    }

    #endregion
}
