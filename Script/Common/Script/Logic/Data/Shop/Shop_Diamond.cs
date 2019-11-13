using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Diamond
{
    public static void BuyItem(ItemShop itemShop)
    {
        Debug.Log("buyItem:" + itemShop.ItemDataID);
        var randomVal = Random.Range(itemShop.ShopRecord.ScriptParam[0], itemShop.ShopRecord.ScriptParam[1]);
        PlayerDataPack.Instance.AddDiamond(randomVal);

        string strTips = Tables.StrDictionary.GetFormatStr(2300088, string.Format("{0} * {1}", Tables.StrDictionary.GetFormatStr(1000003), itemShop.ShopRecord.ScriptParam[0]));
        UIMessageTip.ShowMessageTip(strTips);
    }
}
