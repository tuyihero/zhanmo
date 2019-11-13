using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
using UnityEngine.EventSystems;
using System;
using Tables;

public class RefreshAttr
{
    public EquipExAttr _ShowAttr;
    public int _OrgValue;
    public bool _SetAttr;
}

public class UIEquipRefreshAttrItem : UIItemBase
{
    public Text _AttrText;
    public Text _Value;
    public Text _AddValue;

    private int _OrgValue;
    private ItemEquip _ItemEquip;
    private EquipExAttr _ShowAttr;

    public override void Init()
    {
        base.Init();

        _OrgValue = 0;
    }

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (RefreshAttr)hash["InitObj"];
        _ItemEquip = (ItemEquip)hash["ItemEquip"];

        if (showItem._ShowAttr != null)
        {
            _OrgValue = showItem._OrgValue;
            ShowAttr(showItem._ShowAttr);
        }
        else if (showItem._SetAttr)
        {
            ShowSetAttr();
        }
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowAttr(_ShowAttr);
    }

    private void SetValueDelta()
    {
        var valueDelta = _ShowAttr.Value - _OrgValue;
        if (valueDelta != 0 && _OrgValue != 0)
        {
            _AddValue.text = string.Format("+{0}", valueDelta);
        }
        else
        {
            _AddValue.text = "";
        }
    }

    public void ShowAttr(EquipExAttr attr)
    {

        _ShowAttr = attr;

        string attrStr = _ShowAttr.GetAttrStr();
        //string valueStr = "";
        //if (_ShowAttr.Value > 0)
        //{
        //    valueStr = string.Format("({0})", _ShowAttr.Value);
        //}
        _AddValue.text = "";
        var valueStr = StrDictionary.GetFormatStr(GameDataValue._ExAttrQualityStrDict[(int)_ShowAttr.AttrQuality]);
        valueStr = CommonDefine.GetQualityColorStr(attr.AttrQuality) + valueStr + "</color>";
        _Value.text = valueStr;
        if (_ItemEquip != null)
        {
            var attrColor = attr.AttrQuality;
            if (attrColor < ITEM_QUALITY.ORIGIN)
            {
                attrColor = ITEM_QUALITY.BLUE;
            }
            attrStr = CommonDefine.GetQualityColorStr(attrColor) + attrStr + "</color>";
        }
        _AttrText.text = attrStr;
        SetValueDelta();
    }

    public void ShowSetAttr()
    {
        if (_ItemEquip == null)
        {
            ClearItem();
            return;
        }

        var spAttrInfo = EquipSet.Instance.GetSetInfo(_ItemEquip.SpSetRecord);
        if (spAttrInfo == null)
        {
            ClearItem();
            return;
        }

        string setCnt = string.Format(" ({0}/5)", spAttrInfo.SetEquipCnt);
        _AttrText.text = CommonDefine.GetQualityColorStr(ITEM_QUALITY.ORIGIN) + StrDictionary.GetFormatStr(_ItemEquip.SpSetRecord.Name) + setCnt + "</color>";
    }

    private void ClearItem()
    {
        _Value.text = "";
        _AddValue.text = "";
        _AttrText.text = "";
    }
}

