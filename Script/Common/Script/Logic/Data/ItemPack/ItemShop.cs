using UnityEngine;
using System.Collections;
using Tables;

public class ItemShop : ItemBase
{
    public ItemShop(string dataID) : base(dataID)
    {
        _BuyTimes = 0;
    }

    public ItemShop() : base()
    {
        _BuyTimes = 0;
    }

    #region base attr

    private int _BuyTimes;
    public int BuyTimes
    {
        get
        {
            return _BuyTimes;
        }
        set
        {
            _BuyTimes = value;
        }
    }

    private int _BuyPrice = 0;
    public int BuyPrice
    {
        get
        {
            if (_BuyPrice == 0)
            {
                if (ShopRecord.Script.Equals("Shop_Gambling"))
                {
                    _BuyPrice = RoleData.SelectRole.RoleLevel * 200;
                }
                else
                {
                    _BuyPrice = ShopRecord.PriceBuy;
                }
            }

            return _BuyPrice;
        }
    }

    private ShopItemRecord _ShopRecord;
    public ShopItemRecord ShopRecord
    {
        get
        {
            if (_ShopRecord == null)
            {
                _ShopRecord = TableReader.ShopItem.GetRecord(ItemDataID);
            }
            return _ShopRecord;
        }
    }

    public override string GetDesc()
    {
        string baseDesc =  base.GetDesc();
        //if (ShopRecord.DailyLimit > 0)
        //{
        //    string limit = StrDictionary.GetFormatStr(20005, BuyTimes, ShopRecord.DailyLimit);
        //    if (BuyTimes >= ShopRecord.DailyLimit)
        //    {
        //        limit = CommonDefine.GetEnableRedStr(1) + limit + "</color>";
        //    }
        //    else
        //    {
        //        limit = CommonDefine.GetEnableRedStr(0) + limit + "</color>";
        //    }
        //    baseDesc = baseDesc + "\n" + limit;
        //}
        return baseDesc;
    }

    #endregion 



}

