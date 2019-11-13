using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemPackTest : ItemPackBase<ItemEquip> 
{
    #region 单例

    private static ItemPackTest _Instance;
    public static ItemPackTest Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new ItemPackTest();
            }
            return _Instance;
        }
    }

    private ItemPackTest()
    {
        GuiTextDebug.debug("ItemPackTest init");
        _SaveFileName = "ItemPackTest";
    }

    #endregion

    public void Init()
    {
        Debug.Log("ItemPackTest:" + _PackItems.Count);
        for (int i = 0; i < 10; ++i)
        {
            _PackItems.Add(new ItemEquip() { });
        }
        SaveClass(true);
    }

    /*
    public bool AddItem(T equip)
    {
        var emptyPos = GetEmptyPageEquip();
        if (emptyPos == null)
            return false;

        emptyPos.ExchangeInfo(equip);

        Hashtable hash = new Hashtable();
        hash.Add("EquipInfo", emptyPos);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GET, this, hash);

        return true;
    }

    public bool AddItem(ItemBase item)
    {
        if (PageItems.Count >= _BAG_PAGE_SLOT_CNT)
        {
            return false;
        }

        PageItems.Add(item);
        LogicManager.Instance.SaveGame();
        return true;
    }

    public bool AddItem(string itemID, int itemCnt)
    {
        var itemPos = GetItemPos(itemID);
        if (itemPos == null)
            return false;

        if (!itemPos.IsVolid())
        {
            itemPos.ItemDataID = itemID;
            itemPos.SetStackNum(itemCnt);
        }
        else
        {
            itemPos.AddStackNum(itemCnt);
        }
        itemPos.SaveClass(true);
        return true;
    }

    public bool DecItem(string itemID, int itemCnt)
    {
        if (itemCnt == 0)
            return true;

        var item = GetItem(itemID);
        if (item == null)
            return false;
        if (item.ItemStackNum < itemCnt)
        {
            UIMessageTip.ShowMessageTip(30003);
            return false;
        }
        item.DecStackNum(itemCnt);
        return true;
    }

    public ItemBase GetValidPos()
    {
        for (int i = 0; i < _PackItems.Count; ++i)
        {
            var itembase = _PackItems[i] as ItemBase;
            if (!itembase.IsVolid())
            {
                return itembase;
            }
        }

        UIMessageTip.ShowMessageTip(10002);
        return null;
    }

    public bool IsPackFull()
    {
        if (_PackSize > 0)
        {
            return _PackItems.Count < _PackSize;
        }
        return false;
    }

    public ItemBase GetItemPos(string itemID)
    {
        for (int i = 0; i < PageItems.Count; ++i)
        {
            if (PageItems[i].IsVolid() && PageItems[i].ItemDataID == itemID)
            {
                return PageItems[i];
            }
        }
        return GetEmptyPageItem();
    }

    public ItemBase GetItem(string itemID)
    {
        for (int i = 0; i < PageItems.Count; ++i)
        {
            if (PageItems[i].IsVolid() && PageItems[i].ItemDataID == itemID)
            {
                return PageItems[i];
            }
        }
        return null;
    }

    public int GetItemCnt(string itemID)
    {
        int itemCnt = 0;
        for (int i = 0; i < PageItems.Count; ++i)
        {
            if (PageItems[i].IsVolid() && PageItems[i].ItemDataID == itemID)
            {
                itemCnt += PageItems[i].ItemStackNum;
            }
        }

        return itemCnt;
    }

    public ItemBase GetEmptyPageItem()
    {
        for (int i = 0; i < PageItems.Count; ++i)
        {
            if (!PageItems[i].IsVolid())
            {
                return PageItems[i];
            }
        }
        return null;
    }

    public ItemEquip AddNewEquip(ItemEquip itemEquip)
    {
        for (int i = 0; i < PageEquips.Count; ++i)
        {
            if (string.IsNullOrEmpty(PageEquips[i].ItemDataID) || PageEquips[i].ItemDataID == "-1")
            {
                PageEquips[i].ExchangeInfo(itemEquip);
                return PageEquips[i];
            }
        }
        return null;
    }

    public void SortEquip()
    {
        _PageEquips.Sort((equipA, equipB) =>
        {
            if (equipA.EquipQuality > equipB.EquipQuality)
                return -1;
            else if (equipA.EquipQuality < equipB.EquipQuality)
                return 1;
            else
            {
                if (equipA.EquipLevel > equipB.EquipLevel)
                    return -1;
                else if (equipA.EquipLevel < equipB.EquipLevel)
                    return 1;
                else
                    return 0;
            }
        });
        SaveClass(true);
    }
    */
}

