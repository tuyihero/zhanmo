
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;



public class AttrPair
{
    public string AttrName;
    public string AttrValue;

    public AttrPair(string attrName, string attrValue)
    {
        AttrName = attrName;
        AttrValue = attrValue;
    }

    public AttrPair(RoleAttrEnum roleAttr)
    {
        int value = RoleData.SelectRole._BaseAttr.GetValue(roleAttr);
        AttrName = RandomAttrs.GetAttrName(roleAttr);
        AttrValue = RandomAttrs.GetAttrValueShow(roleAttr, value);
    }
}

public class RoleAttrItem : UIItemBase
{

    public Text _AttrName;
    public Text _AttrValue;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var attrPair = (AttrPair)hash["InitObj"];
        if (attrPair == null)
            return;

        _AttrName.text = attrPair.AttrName;
        _AttrValue.text = attrPair.AttrValue;
    }

    public void Show(string attrName, int value)
    {
        _AttrName.text = attrName.ToString();
        _AttrValue.text = value.ToString();
    }
}

