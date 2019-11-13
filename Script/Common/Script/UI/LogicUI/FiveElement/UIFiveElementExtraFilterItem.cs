
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIFiveElementExtraFilterItem : UIItemBase
{

    public Text _Desc;
    public Toggle _Toggle;

    private int _AttrID;
    public int AttrID
    {
        get
        {
            return _AttrID;
        }
        set
        {
            _AttrID = value;
        }
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var showItem = (GameDataValue.EquipExAttrRandom)hash["InitObj"];
        _AttrID = (int)showItem.AttrID;
        _Desc.text = StrDictionary.GetFormatStr(AttrID);
        _Toggle.isOn = true;
    }

}

