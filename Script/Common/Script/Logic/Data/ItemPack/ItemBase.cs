using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
using System;

public class ItemExData
{
    [SaveField(1)]
    public List<string> _StrParams = new List<string>();
}

public class ItemBase : SaveItemBase
{
    public ItemBase()
    {
        ItemDataID = "";
    }

    public ItemBase(string itemDataID)
    {
        ItemDataID = itemDataID;
    }

    public ItemBase(string itemDataID, int num)
    {
        ItemDataID = itemDataID;
        ItemStackNum = GetVolidItemNum(num);
    }

    [SaveField(1)]
    protected string _ItemDataID;
    public string ItemDataID
    {
        get
        {
            return _ItemDataID;
        }
        set
        {
            _ItemDataID = value;
        }
    }

    protected CommonItemRecord _CommonItemRecord;
    public virtual CommonItemRecord CommonItemRecord
    {
        get
        {
            if (_CommonItemRecord == null)
            {
                if (string.IsNullOrEmpty(_ItemDataID))
                    return null;

                if (_ItemDataID == "-1")
                    return null;

                _CommonItemRecord = TableReader.CommonItem.GetRecord(_ItemDataID);
            }
            return _CommonItemRecord;
        }
    }

    public virtual string GetName()
    {
        if (CommonItemRecord == null)
            return "";
        return StrDictionary.GetFormatStr(CommonItemRecord.NameStrDict);
    }

    public virtual string GetNameWithColor()
    {
        return CommonDefine.GetQualityColorStr(CommonItemRecord.Quality) + GetName() + "</color>";
    }

    public virtual string GetDesc()
    {
        return StrDictionary.GetFormatStr(CommonItemRecord.DescStrDict);
    }

    public virtual ITEM_QUALITY GetQuality()
    {
        return CommonItemRecord.Quality;
    }

    public virtual int GetLevel()
    {
        return 1;
    }

    public bool IsVolid()
    {
        if (string.IsNullOrEmpty(_ItemDataID) || _ItemDataID == "-1")
            return false;
        return true;
    }

    [SaveField(2)]
    protected List<int> _DynamicDataInt = new List<int>();

    public int ItemStackNum
    {
        get
        {
            if (_DynamicDataInt.Count == 0)
            {
                _DynamicDataInt.Add(0);
            }
            return _DynamicDataInt[0];
        }
        protected set
        {
            if (_DynamicDataInt.Count == 0)
            {
                _DynamicDataInt.Add(0);
            }
            _DynamicDataInt[0] = value;
        }
    }

    private int GetVolidItemNum(int num)
    {
        return Math.Max(0, num);
    }

    public int SetStackNum(int num, bool needSave = true)
    {
        int temp = num;
        ItemStackNum = GetVolidItemNum(temp);
        if (needSave)
        {
            SaveClass(true);
        }
        return ItemStackNum;
    }

    public int AddStackNum(int num, bool needSave = true)
    {
        int temp = num + ItemStackNum;
        ItemStackNum = GetVolidItemNum(temp);
        if (needSave)
        {
            SaveClass(true);
        }
        return ItemStackNum;
    }

    public int DecStackNum(int num, bool needSave = true)
    {
        int temp = ItemStackNum - num;
        ItemStackNum = GetVolidItemNum(temp);
        if (ItemStackNum == 0)
        {
            ResetItem();
        }
        if (needSave)
        {
            SaveClass(true);
        }
        return ItemStackNum;
    }

    [SaveField(3)]
    protected List<ItemExData> _DynamicDataEx = new List<ItemExData>();

    #region fun

    public virtual void RefreshItemData()
    {
        _CommonItemRecord = null;
    }

    public virtual void ResetItem()
    {
        _ItemDataID = "-1";
        _DynamicDataInt = new List<int>();
        _DynamicDataEx = new List<ItemExData>();
    }

    public void ExchangeInfo(ItemBase itembase)
    {
        if (itembase == null)
            return;

        var tempId = itembase.ItemDataID;
        itembase.ItemDataID = ItemDataID;
        ItemDataID = tempId;

        var tempData = itembase._DynamicDataInt;
        itembase._DynamicDataInt = _DynamicDataInt;
        _DynamicDataInt = tempData;

        var tempDataVector = itembase._DynamicDataEx;
        itembase._DynamicDataEx = _DynamicDataEx;
        _DynamicDataEx = tempDataVector;

        itembase.RefreshItemData();
        RefreshItemData();
        SaveClass(true);
        itembase.SaveClass(true);
        //LogicManager.Instance.SaveGame();
    }

    public void CopyFrom(ItemBase itembase)
    {
        if (itembase == null)
            return;

        ItemDataID = itembase.ItemDataID;

        _DynamicDataInt = new List<int>( itembase._DynamicDataInt);

        _DynamicDataEx = new List<ItemExData>( itembase._DynamicDataEx);

        RefreshItemData();
    }

    #endregion

    #region static create item

    public static ItemBase CreateItem(string itemID)
    {
        ItemBase newItem = new ItemBase(itemID);
        return newItem;
    }

    public static void CreateItemInPack(string itemID, int num)
    {
        int itemDataID = int.Parse(itemID);

        BackBagPack.Instance.PageItems.AddItem(itemID, num);

    }

    #endregion
}

