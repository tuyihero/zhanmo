using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
using UnityEngine.EventSystems;
using System;
using Tables;

public class UILegendaryItemTooltips : UIBase
{

    #region static funs

    public static void ShowAsyn(EquipItemRecord equipItem, params ToolTipFunc[] funcs)
    {
        Hashtable hash = new Hashtable();
        hash.Add("EquipItem", equipItem);
        hash.Add("ToolTipFun", funcs);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UILegendaryItemTooltips, UILayer.MessageUI, hash);
    }
    
    public static void HideAsyn()
    {
        UIManager.Instance.HideUI(UIConfig.UILegendaryItemTooltips.AssetPath);
    }

    #endregion

    #region 

    public Text _Name;
    public Text _Level;
    public Text _Desc;

    #endregion

    #region 

    private EquipItemRecord _EquipItem;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        var equipItem = hash["EquipItem"] as EquipItemRecord;
        ShowTips(equipItem);
        
    }
    

    protected virtual void ShowTips(EquipItemRecord equipItem)
    {
        if (equipItem == null)
        {
            _EquipItem = null;
            return;
        }

        _EquipItem = equipItem;
        _Name.text = CommonDefine.GetQualityColorStr(ITEM_QUALITY.ORIGIN) + StrDictionary.GetFormatStr(equipItem.NameStrDict) + "</color>";

        if (_Desc != null)
        {
            if (_EquipItem.ExAttr[0].AttrImpact.Equals("RoleAttrImpactBaseAttr"))
            {
                var attrDesc = EquipExAttr.GetAttrStr(_EquipItem.ExAttr[0].AttrImpact, _EquipItem.ExAttr[0].AttrParams);
                _Desc.text = attrDesc;
            }
            else
            {
                var attrDesc = EquipExAttr.GetAttrStr(_EquipItem.ExAttr[0].AttrImpact, new System.Collections.Generic.List<int>() { int.Parse(_EquipItem.ExAttr[0].Id), 1 });
                _Desc.text = attrDesc;
            }
        }
    }

    #endregion
    
}

