using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class Shop_CommonItem
{
    public static void BuyItem(ItemShop itemShop)
    {
        BackBagPack.Instance.PageItems.AddItem(itemShop.ShopRecord.ScriptParam[0].ToString(), itemShop.ShopRecord.ScriptParam[1]);

        string strTips = Tables.StrDictionary.GetFormatStr(2300088, string.Format("{0} * {1}", Tables.StrDictionary.GetFormatStr(itemShop.CommonItemRecord.NameStrDict), 1));
        UIMessageTip.ShowMessageTip(strTips);
    }
}
