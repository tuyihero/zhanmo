using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;

public class ItemGem : ItemBase
{
    public ItemGem(string dataID) : base(dataID)
    {
        Level = 1;
    }

    public ItemGem() : base()
    {
        Level = 1;
    }

    #region base attr

    private static int MAX_INT_CNT = 4;
    public List<int> DynamicDataInt
    {
        get
        {
            if (_DynamicDataInt == null || _DynamicDataInt.Count == 0)
            {
                _DynamicDataInt = new List<int>() { 0, 0, 0, 0 };
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

    public const int _MaxGemLevel = 5;

    private GemTableRecord _GemRecord;
    public GemTableRecord GemRecord
    {
        get
        {
            if (_GemRecord == null)
            {
                _GemRecord = TableReader.GemTable.GetRecord(ItemDataID);
            }
            return _GemRecord;
        }
    }

    public int Level
    {
        get
        {
            if (DynamicDataInt[1] < 1)
            {
                DynamicDataInt[1] = 1;
            }
            return DynamicDataInt[1];
        }
        set
        {
            DynamicDataInt[1] = value;
        }
    }

    public int ExAttr
    {
        get
        {
            return DynamicDataInt[2];
        }
        set
        {
            DynamicDataInt[2] = value;
        }
    }

    public int ExAttrLevel
    {
        get
        {
            return DynamicDataInt[3];
        }
        set
        {
            DynamicDataInt[3] = value;
        }
    }

    #endregion 

    #region override

    public override void RefreshItemData()
    {
        base.RefreshItemData();
        _GemRecord = null;
    }

    public override void ResetItem()
    {
        base.ResetItem();
        _GemRecord = null;
    }

    #endregion

    #region fun

    private List<EquipExAttr> _GemAttr = new List<EquipExAttr>();
    public List<EquipExAttr> GemAttr
    {
        get
        {
            if (_GemAttr == null || _GemAttr.Count == 0)
            {
                RefreshGemAttr();
            }
            return _GemAttr;
        }
    }

    public void RefreshGemAttr()
    {
        _GemAttr.Clear();
        _GemAttr.Add(GameDataValue.GetGemAttr((RoleAttrEnum)GemRecord.AttrValue.AttrParams[0], GameDataValue.GetGemValue(Level)));
        if (ExAttr > 0)
        {
            _GemAttr.Add(GameDataValue.GetGemAttr((RoleAttrEnum)ExAttr, GameDataValue.GetGemValue(ExAttrLevel)));
        }
    }

    public bool IsGemExtra()
    {
        return ExAttr > 0 || Level > 1;
    }

    #endregion

}

