
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;


public class UIFiveElementValueTipItem : UIItemBase
{
    public Text _AttrDesc;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var showItem = (string)hash["InitObj"];
        _AttrDesc.text = showItem;
    }
}

