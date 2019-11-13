
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;
using Tables;



public class UIEquipAttrItem : UIItemBase
{
    public Text _AttrText;

    private ItemEquip _ItemEquip;
    private EquipExAttr _ShowAttr;

    public enum AttrItemDisplayMode
    {
        None,
        Normal,
        ZeroDisable,
    }
    private AttrItemDisplayMode _DisplayMode = AttrItemDisplayMode.Normal;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (EquipExAttr)hash["InitObj"];
        _ItemEquip = (ItemEquip)hash["ItemEquip"];
        if (hash.ContainsKey("DisplayMode"))
        {
            _DisplayMode = (AttrItemDisplayMode)hash["DisplayMode"];
        }

        ShowAttr(showItem);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowAttr(_ShowAttr);
    }

    public void ShowAttr(EquipExAttr attr)
    {

        _ShowAttr = attr;

        string attrStr = _ShowAttr.GetAttrStr();
        string valueStr = "";
        //if (_ShowAttr.AttrQuality > 0)
        {
            valueStr = string.Format("({0})", StrDictionary.GetFormatStr(GameDataValue._ExAttrQualityStrDict[(int)_ShowAttr.AttrQuality]));
            
        }

        if (_DisplayMode == AttrItemDisplayMode.ZeroDisable)
        {
            if (attr.Value == 0)
            {
                attrStr = CommonDefine.GetEnableGrayStr(0) + attrStr + "</color>";
            }
            else
            {
                attrStr = CommonDefine.GetEnableGrayStr(1) + attrStr + "</color>";
            }
        }
        else
        {
            if (_ItemEquip != null)
            {
                var attrColor = attr.AttrQuality;
                if (attrColor < ITEM_QUALITY.ORIGIN)
                {
                    attrColor = ITEM_QUALITY.BLUE;
                }
                attrStr = CommonDefine.GetQualityColorStr(attrColor) + attrStr + valueStr + "</color>";
            }
            else
            {
                //if (attr.AttrQuality >= 0)
                //{
                //    valueStr = CommonDefine.GetQualityColorStr(attr.AttrQuality) + valueStr + "</color>";
                //}
                attrStr = CommonDefine.GetQualityColorStr(attr.AttrQuality) + attrStr + "</color>";
            }
        }
        
        _AttrText.text = attrStr;
    }


}

