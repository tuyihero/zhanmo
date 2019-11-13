using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class EquipRefresh : DataPackBase
{
    #region 单例

    private static EquipRefresh _Instance;
    public static EquipRefresh Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new EquipRefresh();
            }
            return _Instance;
        }
    }

    private EquipRefresh()
    {
        _SaveFileName = "EquipRefresh";
    }

    #endregion

    #region equip refresh
    
    public bool EquipRefreshMat(ItemEquip itemEquip, ItemGem costGem)
    {
        if (itemEquip.EquipQuality == ITEM_QUALITY.WHITE)
            return false;

        if (costGem.ItemStackNum == 0)
            return false;

        if (GemData.Instance.PackGemDatas.DecItem(costGem, 1))
        {
            RandomAttrs.RefreshEquipExAttrValue(itemEquip);

            Hashtable hash = new Hashtable();
            hash.Add("EquipInfo", itemEquip);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, this, hash);

            return true;
        }

        return false;
    }

    public bool EquipRefreshGold(ItemEquip itemEquip)
    {
        if (itemEquip.EquipQuality == ITEM_QUALITY.WHITE)
            return false;

        int costValue = GameDataValue.GetEquipRefreshGold(itemEquip);

        if (PlayerDataPack.Instance.DecGold(costValue))
        {
            RandomAttrs.RefreshEquipExAttrs(itemEquip);

            Hashtable hash = new Hashtable();
            hash.Add("EquipInfo", itemEquip);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, this, hash);

            return true;
        }

        return false;
    }

    public bool EquipRefreshDiamond(ItemEquip itemEquip)
    {
        if (itemEquip.EquipQuality == ITEM_QUALITY.WHITE)
            return false;

        int costValue = GameDataValue.GetEquipRefreshDiamond(itemEquip);

        if (PlayerDataPack.Instance.DecDiamond(costValue))
        {
            RandomAttrs.RefreshEquipExAttrValue(itemEquip);

            Hashtable hash = new Hashtable();
            hash.Add("EquipInfo", itemEquip);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, this, hash);

            return true;
        }

        return false;
    }

    #endregion
}
