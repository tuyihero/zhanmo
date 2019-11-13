using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemPackBase<T> : DataPackBase where T : ItemBase,new()
{
    [SaveField(1)]
    public List<T> _PackItems;

    public List<ItemBase> ToItemBases()
    {
        List<ItemBase> itemBases = new List<ItemBase>();
        for (int i = 0; i < _PackItems.Count; ++i)
        {
            if (_PackItems[i] != null && _PackItems[i].IsVolid())
            {
                itemBases.Add(_PackItems[i]);
            }
        }
        return itemBases;
    }

    public int _PackSize = -1;

    public void InitPack()
    {
        if (_PackItems == null)
        {
            _PackItems = new List<T>();
        }
    }

    public ItemBase GetEmptyPos()
    {
        ItemBase emptyItem = null;
        for (int i = 0; i < _PackItems.Count; ++i)
        {
            var itembase = _PackItems[i] as ItemBase;
            if (!itembase.IsVolid())
            {
                emptyItem = itembase;
                break;
            }
        }

        if (emptyItem != null)
        {
            SaveClass(false);
        }
        //UIMessageTip.ShowMessageTip(10002);
        return emptyItem;
    }

    public int GetEmptyPosCnt()
    {
        if (_PackSize < 0)
            return -1;

        int emptyCnt = 0;
        for (int i = 0; i < _PackItems.Count; ++i)
        {
            var itembase = _PackItems[i] as ItemBase;
            if (!itembase.IsVolid())
            {
                ++emptyCnt;
            }
        }

        if (_PackSize < 0 || _PackItems.Count < _PackSize)
        {
            emptyCnt += _PackSize - _PackItems.Count;
        }
        return emptyCnt;
    }

    public T GetItem(string itemID)
    {
        if (string.IsNullOrEmpty(itemID))
            return null;

        for (int i = 0; i < _PackItems.Count; ++i)
        {
            if (_PackItems[i].IsVolid() && _PackItems[i].ItemDataID == itemID)
            {
                return _PackItems[i];
            }
        }
        return null;
    }

    public List<T> GetItems(string itemID)
    {
        List<T> items = new List<T>();
        for (int i = 0; i < _PackItems.Count; ++i)
        {
            if (_PackItems[i].IsVolid() && _PackItems[i].ItemDataID == itemID)
            {
                items.Add(_PackItems[i]);
            }
        }
        return items;
    }

    public int GetItemCnt(string itemID)
    {
        var items = GetItems(itemID);
        int itemCnt = 0;
        for (int i = 0; i < items.Count; ++i)
        {
            itemCnt += items[i].ItemStackNum;
        }
        return itemCnt;
    }

    public T AddItem(T itemBase)
    {
        var emptyPos = GetEmptyPos();
        if (emptyPos == null)
        {
            if (_PackSize < 0 || _PackItems.Count < _PackSize)
            {
                _PackItems.Add(itemBase);
                SaveClass(true);
                return itemBase;
            }
            else
            {
                return null;
            }
        }

        emptyPos.ExchangeInfo(itemBase);

        return emptyPos as T;
    }

    public bool AddItem(string itemDataID, int itemCnt)
    {
        var itemRecord = Tables.TableReader.CommonItem.GetRecord(itemDataID);

        bool isPacksizeEnough = false;
        int leaveItemCnt = itemCnt;
        var containsItems = GetItems(itemDataID);
        for (int i = 0; i < containsItems.Count; ++i)
        {
            int stackCnt = itemRecord.StackNum - containsItems[i].ItemStackNum;
            leaveItemCnt -= stackCnt;
        }

        if (leaveItemCnt > 0)
        {
            var emptyCnt = GetEmptyPosCnt();
            if (emptyCnt > 0)
            {
                while (emptyCnt > 0)
                {
                    leaveItemCnt -= itemRecord.StackNum;
                    --emptyCnt;
                    if (leaveItemCnt < 0)
                    {
                        isPacksizeEnough = true;
                        break;
                    }
                }
            }
            else if (emptyCnt == -1)
            {
                isPacksizeEnough = true;
            }
        }
        else
        {
            isPacksizeEnough = true;
        }

        if (!isPacksizeEnough)
        {
            UIMessageTip.ShowMessageTip(10002);
            return false;
        }

        leaveItemCnt = itemCnt;
        for (int i = 0; i < containsItems.Count; ++i)
        {
            int stackCnt = itemRecord.StackNum - containsItems[i].ItemStackNum;
            int addCnt = 0;
            if (leaveItemCnt > stackCnt)
            {
                addCnt = stackCnt;
            }
            else
            {
                addCnt = leaveItemCnt;
            }
            leaveItemCnt -= addCnt;
            containsItems[i].AddStackNum(addCnt);
        }
        if (leaveItemCnt > 0)
        {
            var emptyCnt = GetEmptyPosCnt();
            while (leaveItemCnt > 0)
            {
                int newItemCnt = 0;
                if (leaveItemCnt < itemRecord.StackNum)
                {
                    newItemCnt = leaveItemCnt;
                }
                else
                {
                    newItemCnt = itemRecord.StackNum;
                }
                leaveItemCnt -= itemRecord.StackNum;

                var emptyPos = GetEmptyPos();
                if (emptyPos != null)
                {
                    
                    emptyPos.ItemDataID = itemDataID;
                    emptyPos.AddStackNum(newItemCnt);
                }
                else
                {
                    var newItem = new T();
                    newItem.ItemDataID = itemDataID;
                    newItem.SetStackNum(newItemCnt);
                    _PackItems.Add(newItem);
                }
            }
        }

        return true;
    }

    public bool DecItem(string itemDataID, int decCnt)
    {
        var items = GetItems(itemDataID);
        int itemCnt = 0;
        for (int i = 0; i < items.Count; ++i)
        {
            itemCnt += items[i].ItemStackNum;
        }

        if (itemCnt < decCnt)
            return false;

        int needCnt = decCnt;
        for (int i = 0; i < items.Count; ++i)
        {
            if (needCnt > 0)
            {
                int decNum = 0;
                if (items[i].ItemStackNum >= needCnt)
                {
                    decNum = needCnt;
                }
                else
                {
                    decNum = items[i].ItemStackNum;
                }
                items[i].DecStackNum(decNum);
                if (_PackSize < 0 && items[i].ItemStackNum == 0)
                {
                    _PackItems.Remove(items[i]);
                }
                needCnt -= decNum;
            }
            else
            {
                break;
            }
        }
        return true;
    }

    public bool DecItem(T item, int cnt)
    {
        if (item.ItemStackNum < cnt)
            return false;

        item.DecStackNum(cnt);
        if (item.ItemStackNum == 0)
        {
            if (_PackSize < 0)
            {
                _PackItems.Remove(item);
            }
            else
            {
                item.ResetItem();
            }
        }
        return true;
    }

    public bool DecItem(T item)
    {
        if (_PackSize < 0)
        {
            _PackItems.Remove(item);
        }
        else
        {
            item.ResetItem();
        }
        return true;
    }

    public bool RemoveItem(T removeItem)
    {
        if (_PackItems.Contains(removeItem))
        {
            _PackItems.Remove(removeItem);
            return true;
        }
        return false;
    }

    public void SortStack()
    {
        List<string> sortedItemID = new List<string>();
        List<T> emptyItem = new List<T>();
        foreach (var item in _PackItems)
        {
            if (!item.IsVolid())
                continue;

            if (sortedItemID.Contains(item.ItemDataID))
                continue;

            var items = GetItems(item.ItemDataID);
            if (items.Count > 1)
            {
                for (int i = 0; i < items.Count - 1; ++i)
                {

                    for (int j = i + 1; j < items.Count; ++j)
                    {
                        int addStackNum = items[i].CommonItemRecord.StackNum - items[i].ItemStackNum;
                        if (addStackNum > items[j].ItemStackNum)
                        {
                            addStackNum = items[j].ItemStackNum;
                        }
                        items[j].DecStackNum(addStackNum);
                        items[i].AddStackNum(addStackNum);

                        if (items[j].ItemStackNum == 0)
                        {
                            emptyItem.Add(items[j]);
                        }

                        if (items[i].CommonItemRecord.StackNum == items[i].ItemStackNum)
                        {
                            break;
                        }
                    }

                }
            }

            sortedItemID.Add(item.ItemDataID);
        }

        foreach (var item in emptyItem)
        {
            _PackItems.Remove(item);
        }
    }

    public void SortEmpty()
    {
        _PackItems.Sort((itemA, itemB) =>
        {
            if (itemA.IsVolid() && !itemB.IsVolid())
                return -1;
            else if (!itemA.IsVolid() && itemB.IsVolid())
                return 1;
            else if (itemA.IsVolid() && itemB.IsVolid())
            {
                int dataIDA = int.Parse(itemA.ItemDataID);
                int dataIDB = int.Parse(itemB.ItemDataID);
                if (dataIDA > dataIDB)
                {
                    return -1;
                }
                else if (dataIDA < dataIDB)
                {
                    return 1;
                }
                else
                {
                    if (itemA.ItemStackNum > itemB.ItemStackNum)
                    {
                        return -1;
                    }
                    else if (itemA.ItemStackNum < itemB.ItemStackNum)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else
                return 0;
        });
    }   
}

