using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Tables;
using System;

public class ItemFiveElement : ItemBase
{
    public ItemFiveElement(string datID) : base(datID)
    {

    }

    public ItemFiveElement():base()
    {

    }

    #region elevemt data

    private static int MAX_INT_CNT = 5;
    public List<int> DynamicDataInt
    {
        get
        {
            if (_DynamicDataInt == null || _DynamicDataInt.Count == 0)
            {
                _DynamicDataInt = new List<int>();
                for (int i = 0; i < MAX_INT_CNT; ++i)
                {
                    _DynamicDataInt.Add(0);
                }
            }
            else if (_DynamicDataInt.Count < MAX_INT_CNT)
            {
                for (int i = 0; i < MAX_INT_CNT; ++i)
                {
                    _DynamicDataInt.Add(0);
                }
            }
            return _DynamicDataInt;
        }
    }

    public int Level
    {
        get
        {
            return DynamicDataInt[1];
        }
        set
        {
            DynamicDataInt[1] = value;
        }
    }

    public static int _MAX_LOCK_NUM = 3;
    private List<int> _AttrLock;
    public List<int> AttrLock
    {
        get
        {
            if(_AttrLock == null)
            {
                _AttrLock = new List<int>();
                for (int i = 0; i < _MAX_LOCK_NUM; ++i)
                {
                    _AttrLock.Add(DynamicDataInt[2 + i]);
                }
            }
            return _AttrLock;
        }
    }
    public bool IsAttrLock(int idx)
    {
        return AttrLock.Contains(idx + 1);
    }
    public void ClearLock()
    {
        for (int i = 0; i < AttrLock.Count; ++i)
        {
            AttrLock[i] = 0;
        }
    }
    public bool SetAttrLock(int idx, bool isLock)
    {
        int index = idx + 1;
        if (isLock)
        {
            if (IsAttrLock(idx))
                return false;
            for (int i = 0; i < AttrLock.Count; ++i)
            {
                if (AttrLock[i] == 0)
                {
                    AttrLock[i] = index;
                    DynamicDataInt[2 + i] = AttrLock[i];
                    SaveClass(false);
                    return true;
                }
            }
        }
        else
        {
            if (!IsAttrLock(idx))
                return false;
            for (int i = 0; i < AttrLock.Count; ++i)
            {
                if (AttrLock[i] == index)
                {
                    AttrLock[i] = 0;
                    DynamicDataInt[2 + i] = AttrLock[i];
                    SaveClass(false);
                    return true;
                }
            }
        }

        return false;
    }
    

    private FiveElementRecord _FiveElementRecord;
    public FiveElementRecord FiveElementRecord
    {
        get
        {
            if (_FiveElementRecord == null)
            {
                if (string.IsNullOrEmpty(_ItemDataID))
                    return null;

                if (_ItemDataID == "-1")
                    return null;

                _FiveElementRecord = TableReader.FiveElement.GetRecord(_ItemDataID);
            }
            return _FiveElementRecord;
        }
    }

    protected int _CombatValue = 0;
    public int CombatValue
    {
        get
        {
            return _CombatValue;
        }
    }

    public virtual void CalculateCombatValue()
    {
        _CombatValue = 0;

        foreach (var exAttrs in ElementExAttrs)
        {
            if (exAttrs.AttrType == "RoleAttrImpactBaseAttr")
            {
                _CombatValue += GameDataValue.GetAttrValue((RoleAttrEnum)exAttrs.AttrParams[0], exAttrs.AttrParams[1]);
            }
            else
            {
                _CombatValue += 0;
            }
        }
    }

    public ITEM_QUALITY GetElementQuality()
    {
        if (EquipExAttrs.Count == 1)
        {
            return ITEM_QUALITY.WHITE;
        }
        else if (EquipExAttrs.Count == 2)
        {
            return ITEM_QUALITY.GREEN;
        }
        else if (EquipExAttrs.Count == 3)
        {
            return ITEM_QUALITY.BLUE;
        }
        else if (EquipExAttrs.Count == 4)
        {
            return ITEM_QUALITY.PURPER;
        }
        else if (EquipExAttrs.Count == 5)
        {
            return ITEM_QUALITY.ORIGIN;
        }

        return ITEM_QUALITY.WHITE;
    }

    public virtual string GetElementNameWithColor()
    {
        string equipName = StrDictionary.GetFormatStr(CommonItemRecord.NameStrDict);
        return CommonDefine.GetQualityColorStr(GetElementQuality()) + equipName + "</color>";
    }

    #endregion

    #region attr

    private List<EquipExAttr> _EquipExAttrs;
    public List<EquipExAttr> EquipExAttrs
    {
        get
        {
            if (_EquipExAttrs == null)
            {
                _EquipExAttrs = new List<global::EquipExAttr>();
                foreach (var strParam in _DynamicDataEx)
                {
                    EquipExAttr exAttr = new global::EquipExAttr();
                    exAttr.AttrType = strParam._StrParams[0];
                    exAttr.Value = int.Parse(strParam._StrParams[1]);
                    for (int i = 2; i < strParam._StrParams.Count; ++i)
                    {
                        exAttr.AttrParams.Add(int.Parse(strParam._StrParams[i]));
                    }
                    _EquipExAttrs.Add(exAttr);
                }
                CalculateCombatValue();
            }
            return _EquipExAttrs;
        }
        set
        {
            _EquipExAttrs = value;
            CalculateCombatValue();
            BakeExAttr();
        }
    }

    private List<EquipExAttr> _ElementExAttrs;
    public List<EquipExAttr> ElementExAttrs
    {
        get
        {
            if (_ElementExAttrs == null)
            {
                _ElementExAttrs = new List<global::EquipExAttr>();
                for(int i = 0; i< _DynamicDataEx.Count; ++i)
                {
                    EquipExAttr exAttr = new global::EquipExAttr();
                    exAttr.AttrType = _DynamicDataEx[i]._StrParams[0];
                    exAttr.Value = int.Parse(_DynamicDataEx[i]._StrParams[1]);
                    for (int j = 2; j < _DynamicDataEx[i]._StrParams.Count; ++j)
                    {
                        exAttr.AttrParams.Add(int.Parse(_DynamicDataEx[i]._StrParams[j]));
                    }
                    exAttr.AttrParams[1] = (int)(FiveElementData.Instance.GetAttrRate(this, i) * exAttr.AttrParams[1]);
                    _ElementExAttrs.Add(exAttr);
                }
                CalculateCombatValue();
            }
            return _ElementExAttrs;
        }
        set
        {
            _ElementExAttrs = value;
            CalculateCombatValue();
            //BakeExAttr();
        }
    }

    public void RefreshElementAttr()
    {
        _ElementExAttrs = null;
    }

    public void BakeExAttr()
    {
        _DynamicDataEx.Clear();
        foreach (var exAttr in EquipExAttrs)
        {
            ItemExData exData = new ItemExData();
            exData._StrParams.Add(exAttr.AttrType);
            exData._StrParams.Add(exAttr.Value.ToString());
            for (int i = 0; i < exAttr.AttrParams.Count; ++i)
            {
                exData._StrParams.Add(exAttr.AttrParams[i].ToString());
            }
            _DynamicDataEx.Add(exData);
        }
        SaveClass(true);

        _ElementExAttrs = null;
    }

    public void ReplaceAttr(int idx, EquipExAttr attr)
    {
        if (EquipExAttrs.Count < idx)
            return;

        ItemExData exData = new ItemExData();
        exData._StrParams.Add(attr.AttrType);
        exData._StrParams.Add(attr.Value.ToString());
        for (int i = 0; i < attr.AttrParams.Count; ++i)
        {
            exData._StrParams.Add(attr.AttrParams[i].ToString());
        }

        _DynamicDataEx[idx] = exData;
        _EquipExAttrs = null;
        _ElementExAttrs = null;

        CalculateCombatValue();
        //BakeExAttr();
    }

    public void AddExAttr(EquipExAttr attr)
    {
        EquipExAttrs.Add(attr);
        ItemExData exData = new ItemExData();
        exData._StrParams.Add(attr.AttrType);
        exData._StrParams.Add(attr.Value.ToString());
        for (int i = 0; i < attr.AttrParams.Count; ++i)
        {
            exData._StrParams.Add(attr.AttrParams[i].ToString());
        }
        _EquipExAttrs = null;
        _DynamicDataEx.Add(exData);
        _ElementExAttrs = null;

        CalculateCombatValue();
    }

    public void RefreshExAttr(int idx)
    {
        GameDataValue.RefreshElementExAttr(EquipExAttrs[idx], idx);
        BakeExAttr();
    }

    #endregion

    #region fun

    public override void RefreshItemData()
    {
        base.RefreshItemData();
        _FiveElementRecord = null;
        _EquipExAttrs = null;
    }

    public override void ResetItem()
    {
        base.ResetItem();
        _FiveElementRecord = null;
        _EquipExAttrs = null;
    }

    public override int GetLevel()
    {
        return Level;
    }
    #endregion


}

