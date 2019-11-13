using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICommonAwardItem : UIItemBase
{

    #region 

    public Image _CurrencyIcon;
    public Text _CurrencyValue;
    public Sprite[] _CurrencySprite;

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        if (hash["InitObj"] is Tables.CommonItemRecord)
        {
            var commonItem = (Tables.CommonItemRecord)hash["InitObj"];
            ResourceManager.Instance.SetImage(_CurrencyIcon, commonItem.Icon);
            _CurrencyValue.text = "";
        }
    }

    public void SetValue(int value)
    {
        _CurrencyValue.text = value.ToString();
    }

    public void ShowAward(MONEYTYPE currencyType, int currencyValue)
    {
        ResourceManager.Instance.SetImage(_CurrencyIcon, CommonDefine.GetMoneyIcon(currencyType));

        _CurrencyValue.text = currencyValue.ToString();
    }

    public void ShowAward(MONEYTYPE currencyType, long currencyValue)
    {
        ResourceManager.Instance.SetImage(_CurrencyIcon, CommonDefine.GetMoneyIcon(currencyType));

        _CurrencyValue.text = currencyValue.ToString();
    }

    public void ShowAward(string itemID, long currencyValue)
    {
        //_CurrencyIcon.sprite = _CurrencySprite[(int)currencyType];
        var commonItem = Tables.TableReader.CommonItem.GetRecord(itemID);
        ResourceManager.Instance.SetImage(_CurrencyIcon, commonItem.Icon);
        if (currencyValue > 0)
        {
            _CurrencyValue.text = currencyValue.ToString();
        }
        else
        {
            _CurrencyValue.text = "";
        }
    }
    
    #endregion

}

