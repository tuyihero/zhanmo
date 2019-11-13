using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Gambling
{
    public static Dictionary<Tables.EQUIP_SLOT, List<int>> _LegendaryDict = new Dictionary<Tables.EQUIP_SLOT, List<int>>()
    {
        { Tables.EQUIP_SLOT.TORSO, new List<int>() { 125000, 126000, 127000, 128000 } },
        { Tables.EQUIP_SLOT.LEGS, new List<int>() { 125001, 126001, 127001, 128001 } },
        { Tables.EQUIP_SLOT.AMULET, new List<int>() { 125002, 126002, 127002, 128002 } },
        { Tables.EQUIP_SLOT.RING, new List<int>() { 125003, 126003, 127003, 128003 } },
    };

    public static void BuyItem(ItemShop itemShop)
    {
        int level = Random.Range(RoleData.SelectRole.RoleLevel - 5, RoleData.SelectRole.RoleLevel + 5);
        level = Mathf.Clamp(level, 1, RoleData.MAX_ROLE_LEVEL);

        int quality = 0;
        if (itemShop.ShopRecord.ScriptParam[0] != (int)Tables.EQUIP_SLOT.WEAPON)
        {
            quality = GameRandom.GetRandomLevel(0, 84, 15, 1);
        }
        else
        {
            quality = GameRandom.GetRandomLevel(0, 84, 15, 0);
        }

        if (quality == (int)Tables.ITEM_QUALITY.ORIGIN)
        {
            var legendaryList = _LegendaryDict[(Tables.EQUIP_SLOT)itemShop.ShopRecord.ScriptParam[0]];
            int idx = Random.Range(0, legendaryList.Count - 1);
            var equipItem = ItemEquip.CreateEquip(level, (Tables.ITEM_QUALITY)quality, legendaryList[idx], itemShop.ShopRecord.ScriptParam[0], itemShop.ShopRecord.ScriptParam[1]);
            var newEquip = BackBagPack.Instance.AddEquip(equipItem);
        }
        else
        {
            var equipItem = ItemEquip.CreateEquip(level, (Tables.ITEM_QUALITY)quality, -1, itemShop.ShopRecord.ScriptParam[0], itemShop.ShopRecord.ScriptParam[1]);
            var newEquip = BackBagPack.Instance.AddEquip(equipItem);
        }
    }
}
