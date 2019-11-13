using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Gold
{

    public static void BuyItem(ItemShop itemShop)
    {
        PlayerDataPack.Instance.AddGold(itemShop.ShopRecord.ScriptParam[0]);
        string strTips = Tables.StrDictionary.GetFormatStr(2300088, string.Format("{0} * {1}", Tables.StrDictionary.GetFormatStr(1000002), itemShop.ShopRecord.ScriptParam[0]));
        UIMessageTip.ShowMessageTip(strTips);
    }
}
