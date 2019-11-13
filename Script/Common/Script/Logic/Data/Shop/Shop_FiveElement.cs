using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class Shop_FiveElement
{
    public static void BuyItem(ItemShop itemShop)
    {
        int elementLevel = GameDataValue.GetNearestLevel(RoleData.SelectRole.TotalLevel);
        if (itemShop.ShopRecord.ScriptParam[0] < 0)
        {
            foreach (var element in TableReader.FiveElement.Records.Values)
            {
                var record = TableReader.FiveElement.GetRecord(element.Id.ToString());
                var itemElement = GameDataValue.GetFiveElement(record, elementLevel);
                FiveElementData.Instance.AddElementItem(itemElement);
            }
        }
        else
        {
            var record = TableReader.FiveElement.GetRecord(itemShop.ShopRecord.ScriptParam[0].ToString());
            var itemElement = GameDataValue.GetFiveElement(record, elementLevel);
            FiveElementData.Instance.AddElementItem(itemElement);
        }
    }
}
