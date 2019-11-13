
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIRechargeItem : UIItemSelect
{
    public Image _Icon;
    public Text _Num;
    public Text _Price;
    
    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (RechargeRecord)hash["InitObj"];

        ResourceManager.Instance.SetImage(_Icon, showItem.Icon);
        _Num.text = "*" + showItem.Num;
        _Price.text = "$" + showItem.Price;
    }

    
}

