using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum MONEYTYPE
{
    GOLD = 0,
    DIAMOND,
    ITEM,
}

public class UICurrencyItem : UIItemBase
{

    #region 

    public Image _CurrencyIcon;
    public Text _CurrencyValue;
    public Sprite[] _CurrencySprite;

    private MONEYTYPE _CurrencyType;
    private int _CurrencyIntValue;
    public int CurrencyIntValue
    {
        get
        {
            return _CurrencyIntValue;
        }
    }

    #endregion

    #region 

    public void SetValue(int value)
    {
        _CurrencyValue.text = value.ToString();
        _CurrencyIntValue = value;
    }

    public void ShowCurrency(MONEYTYPE currencyType, int currencyValue)
    {
        ResourceManager.Instance.SetImage(_CurrencyIcon, CommonDefine.GetMoneyIcon(currencyType));

        _CurrencyValue.text = currencyValue.ToString();
        _CurrencyIntValue = currencyValue;
        _CurrencyType = currencyType;
    }

    public void ShowCurrency(MONEYTYPE currencyType, long currencyValue)
    {
        ResourceManager.Instance.SetImage(_CurrencyIcon, CommonDefine.GetMoneyIcon(currencyType));

        _CurrencyValue.text = currencyValue.ToString();
        _CurrencyIntValue = (int)currencyValue;
        _CurrencyType = currencyType;
    }

    public void ShowCurrency(string itemID, long currencyValue)
    {
        //_CurrencyIcon.sprite = _CurrencySprite[(int)currencyType];
        var itemBase = Tables.TableReader.CommonItem.GetRecord(itemID);
        ResourceManager.Instance.SetImage(_CurrencyIcon, itemBase.Icon);

        _CurrencyValue.text = currencyValue.ToString();
        _CurrencyIntValue = (int)currencyValue;
        _CurrencyType = MONEYTYPE.ITEM;
    }

    public void ShowOwnCurrency(MONEYTYPE currencyType)
    {
        int Ownvalue = 0;
        if (currencyType == MONEYTYPE.GOLD)
        {
            Ownvalue = PlayerDataPack.Instance.Gold;
        }
        else if (currencyType == MONEYTYPE.DIAMOND)
        {
            Ownvalue = PlayerDataPack.Instance.Diamond;
        }
        ShowCurrency(currencyType, Ownvalue);
    }

    public void ShowOwnCurrency(string itemDataID)
    {
        int Ownvalue = BackBagPack.Instance.PageItems.GetItemCnt(itemDataID);
        ShowCurrency(itemDataID, Ownvalue);
    }

    public void ShowCostCurrency(MONEYTYPE currencyType, int costValue, bool showOwnValue = true)
    {
        if (showOwnValue)
        {
            int Ownvalue = 0;
            if (currencyType == MONEYTYPE.GOLD)
            {
                Ownvalue = PlayerDataPack.Instance.Gold;
            }
            else if (currencyType == MONEYTYPE.DIAMOND)
            {
                Ownvalue = PlayerDataPack.Instance.Diamond;
            }
            ShowCurrency(currencyType, Ownvalue);


            string currencyStr = "";
            if (costValue > Ownvalue)
            {
                currencyStr = CommonDefine.GetEnableRedStr(0) + Ownvalue.ToString() + "</color>/" + costValue.ToString();
            }
            else
            {
                currencyStr = CommonDefine.GetEnableRedStr(1) + Ownvalue.ToString() + "</color>/" + costValue.ToString();
            }
            _CurrencyValue.text = currencyStr;
        }
        else
        {
            int Ownvalue = 0;
            if (currencyType == MONEYTYPE.GOLD)
            {
                Ownvalue = PlayerDataPack.Instance.Gold;
            }
            else if (currencyType == MONEYTYPE.DIAMOND)
            {
                Ownvalue = PlayerDataPack.Instance.Diamond;
            }
            ShowCurrency(currencyType, costValue);


            string currencyStr = "";
            if (costValue > Ownvalue)
            {
                currencyStr = CommonDefine.GetEnableRedStr(0) + costValue.ToString() + "</color>";
            }
            else
            {
                currencyStr = CommonDefine.GetEnableRedStr(1) + costValue.ToString() + "</color>";
            }
            _CurrencyValue.text = currencyStr;
        }
    }

    public void ShowCostCurrency(string itemDataID, int costValue, int ownCnt = -1)
    {
        int Ownvalue = ownCnt;
        if (Ownvalue < 0)
        {
            Ownvalue = BackBagPack.Instance.PageItems.GetItemCnt(itemDataID);
        }
        ShowCurrency(itemDataID, Ownvalue);

        string currencyStr = "";
        if (costValue > Ownvalue)
        {
            currencyStr = CommonDefine.GetEnableRedStr(0) + Ownvalue.ToString() + "</color>/" + costValue.ToString();
        }
        else
        {
            currencyStr = CommonDefine.GetEnableRedStr(1) + Ownvalue.ToString() + "</color>/" + costValue.ToString();
        }
        _CurrencyValue.text = currencyStr;
    }

    #endregion

    public void OnBtnAddClick()
    {

        //PlayerDataPack.Instance.AddGold(50000);
        //PlayerDataPack.Instance.AddDiamond(1000);
        //UIMainFun.UpdateMoney();

        if (_CurrencyType == MONEYTYPE.GOLD)
        {
            UIShopPack.ShowAsyn();
        }
        else if (_CurrencyType == MONEYTYPE.DIAMOND)
        {
            UIRechargePack.ShowAsyn();
        }

    }
}

