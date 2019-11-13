
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;


public class UIFiveElementAttrItem : UIItemBase
{
    public Text _Attr;
    public Text _AddPersent;
    public GameObject _BtnLock;
    public GameObject _BtnUnLock;

    private int _ShowingItemIdx;
    private int _ShowingAttrIdx;

    public void InitAttrItem(int itemIdx, int attrIdx)
    {
        _ShowingItemIdx = itemIdx;
        _ShowingAttrIdx = attrIdx;
        var elementItem = FiveElementData.Instance._UsingElements[_ShowingItemIdx];
        _AddPersent.text = FiveElementData.Instance.GetAttrAddRateStr(elementItem, _ShowingAttrIdx);
        
        if (elementItem.ElementExAttrs.Count > _ShowingAttrIdx)
        {
            _Attr.text = elementItem.ElementExAttrs[_ShowingAttrIdx].GetAttrStr(false);

            RefreshLockState();
        }
        else
        {
            _BtnLock.gameObject.SetActive(false);
            _BtnUnLock.gameObject.SetActive(false);
            var addRate = FiveElementData.Instance.GetAddExAttrRate(_ShowingAttrIdx);
            _Attr.text = GameDataValue.ConfigFloatToPersent(addRate).ToString() + "%";
        }
    }

    public void OnBtnLock()
    {
        var elementItem = FiveElementData.Instance._UsingElements[_ShowingItemIdx];
        if (elementItem.SetAttrLock(_ShowingAttrIdx, true))
        {
            RefreshLockState();
            UIFiveElementExtra.RefreshCost();
        }
    }

    public void OnBtnUnLock()
    {
        var elementItem = FiveElementData.Instance._UsingElements[_ShowingItemIdx];
        if (elementItem.SetAttrLock(_ShowingAttrIdx, false))
        {
            RefreshLockState();
            UIFiveElementExtra.RefreshCost();
        }
        //elementItem.ClearLock();
    }

    private void RefreshLockState()
    {
        var elementItem = FiveElementData.Instance._UsingElements[_ShowingItemIdx];
        if (elementItem.IsAttrLock(_ShowingAttrIdx))
        {
            _BtnLock.gameObject.SetActive(false);
            _BtnUnLock.gameObject.SetActive(true);
        }
        else
        {
            _BtnLock.gameObject.SetActive(true);
            _BtnUnLock.gameObject.SetActive(false);
        }
    }
}

