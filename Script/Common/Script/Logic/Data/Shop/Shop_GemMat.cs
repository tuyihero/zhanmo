using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class Shop_GemMat
{
    public static void BuyItem(ItemShop itemShop)
    {
        if (itemShop.ShopRecord.ScriptParam[0] < 0)
        {
            foreach (var gemRecord in TableReader.GemTable.Records.Values)
            {
                GemData.Instance.CreateGem(gemRecord.Id, 1);

                var itemRecord = TableReader.CommonItem.GetRecord(gemRecord.Id.ToString());
                string strTips = Tables.StrDictionary.GetFormatStr(2300088, string.Format("{0} * {1}", Tables.StrDictionary.GetFormatStr(itemRecord.NameStrDict), 1));
                UIMessageTip.ShowMessageTip(strTips);
            }
            
        }
        else
        {
            var gemRecord = TableReader.GemTable.GetRecord(itemShop.ShopRecord.ScriptParam[0].ToString());
            GemData.Instance.CreateGem(gemRecord.Id, 1);

            var itemRecord = TableReader.CommonItem.GetRecord(itemShop.ShopRecord.ScriptParam[0].ToString());
            string strTips = Tables.StrDictionary.GetFormatStr(2300088, string.Format("{0} * {1}", Tables.StrDictionary.GetFormatStr(itemRecord.NameStrDict), 1));
            UIMessageTip.ShowMessageTip(strTips);
        }
    }
}
