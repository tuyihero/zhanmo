
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIFiveElementCoreInfo : UIItemInfo
{

    #region 

    public Text _Value;
    public UIContainerBase _ElementCoreContainer;

    #endregion

    #region 

    private ItemFiveElementCore _ShowElementItem;

    public void ShowTips(ItemFiveElementCore itemElement)
    {
        if (itemElement == null || !itemElement.IsVolid())
        {
            _ShowElementItem = null;
            return;
        }

        _ShowElementItem = itemElement;


        _Name.text = _ShowElementItem.GetElementNameWithColor();
        _Level.text = _ShowElementItem.Level.ToString();


        bool isConditionComplate = true;
        List<EleCoreConditionInfo> conditionDesc = new List<EleCoreConditionInfo>();
        for (int i = 0; i < _ShowElementItem.FiveElementCoreRecord.PosCondition.Count; ++i)
        {
            var conState = _ShowElementItem.ConditionState(i);
            if (conState < 0)
                break;

            if (conState == 0)
            {
                isConditionComplate = false;
            }

            EleCoreConditionInfo conditionInfo = new EleCoreConditionInfo();
            var conDesc = _ShowElementItem.GetConditionDesc(i);
            conditionInfo._Desc = conDesc;
            conditionInfo._IsAct = conState > 0 && isConditionComplate;
            conditionInfo._Attr = _ShowElementItem.EquipExAttrs[i];
            conditionDesc.Add(conditionInfo);
        }
        
        Hashtable hash = new Hashtable();
        _ElementCoreContainer.InitContentItem(conditionDesc, null, hash);
    }
    #endregion



}

