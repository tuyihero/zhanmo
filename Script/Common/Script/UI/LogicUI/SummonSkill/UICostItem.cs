
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;


public class UICostItem : UIItemBase
{

    public Image _ItemIcon;
    public Text _Num;

    public void ShowCost(string costItem, int costNum)
    {
        int itemCnt = BackBagPack.Instance.PageItems.GetItemCnt(costItem);
        if (itemCnt > costNum)
        {
            _Num.text = CommonDefine.GetEnableRedStr(1) +  "*" + costNum.ToString() + "</color>";
        }
        else
        {
            _Num.text = CommonDefine.GetEnableRedStr(0) + "*" + costNum.ToString() + "</color>";
        }
    }
    
}

