using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemPackElement<T> : ItemPackBase<T> where T : ItemFiveElement,new()
{
    public List<T> GetAttrItems(int attrID)
    {
        List<T> items = new List<T>();
        for (int i = 0; i < _PackItems.Count; ++i)
        {
            if (_PackItems[i].IsVolid() && _PackItems[i].ElementExAttrs[0].AttrParams[0] == attrID)
            {
                items.Add(_PackItems[i]);
            }
        }
        return items;
    }

    public int GetAttrItemCnt(int attrID)
    {
        var items = GetAttrItems(attrID);
        int itemCnt = 0;
        for (int i = 0; i < items.Count; ++i)
        {
            itemCnt += items[i].ItemStackNum;
        }
        return itemCnt;
    }

    public bool DecAttrItem(int attrID, int decCnt)
    {
        var items = GetAttrItems(attrID);
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
}

