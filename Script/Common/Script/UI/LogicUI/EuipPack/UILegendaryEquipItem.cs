
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UILegendaryEquipItem : UIItemBase
{

    public Text _Name;
    public UIBackPackItem _BackpackItem;
    public ItemEquip _EquipItem;

    public EquipItemRecord _LegendaryRecord;
    private UIBase _DragPanel;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (EquipItemRecord)hash["InitObj"];
        var dragPack = (UIBase)hash["DragPack"];
        ShowItem(showItem, dragPack);
    }

    private void ShowItem(EquipItemRecord legendaryEquip, UIBase dragPanel)
    {
        _EquipItem = LegendaryData.Instance._LegendaryEquipDict[legendaryEquip];
        _LegendaryRecord = legendaryEquip;
        _DragPanel = dragPanel;

        if (_EquipItem.IsVolid())
        {
            _Name.text = _EquipItem.GetEquipLegandaryName() + CommonDefine.GetQualityColorStr(ITEM_QUALITY.ORIGIN) + " · " + "</color>" + _EquipItem.GetEquipNameWithColor();
        }
        else
        {
            _Name.text = StrDictionary.GetFormatStr(legendaryEquip.NameStrDict);
        }
        _BackpackItem.ShowItem(_EquipItem);

        

        Hashtable hash = new Hashtable();
        hash.Add("InitObj", _EquipItem);
        hash.Add("DragPack", dragPanel);
        _BackpackItem.Show(hash);
        _BackpackItem._InitInfo = _EquipItem;
        _BackpackItem._ClickEvent += OnEquipItemClick;
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowItem(_LegendaryRecord, _DragPanel);
    }

    public void OnEquipItemClick(object equipItem)
    {
        OnItemClick();
    }
}

