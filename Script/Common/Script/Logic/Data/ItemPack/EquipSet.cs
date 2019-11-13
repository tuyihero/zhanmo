using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;

public class EquipSetInfo
{
    public int SetEquipCnt;
    public int SetEquipValue;
}

public class EquipSet
{
    #region 唯一

    private static EquipSet _Instance = null;
    public static EquipSet Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new EquipSet();
            }
            return _Instance;
        }
    }

    private EquipSet()
    {
        
    }

    #endregion

    public Dictionary<EquipSpAttrRecord, EquipSetInfo> _ActingEquipSpAttr = new Dictionary<EquipSpAttrRecord, EquipSetInfo>();

    public EquipSetInfo GetSetInfo(EquipSpAttrRecord spAttrRecord)
    {
        if (_ActingEquipSpAttr.ContainsKey(spAttrRecord))
            return _ActingEquipSpAttr[spAttrRecord];

        return null;
    }

    public void RemoveActingSpAttr(EquipSpAttrRecord spAttr, int equipValue)
    {
        if (_ActingEquipSpAttr.ContainsKey(spAttr))
        {
            --_ActingEquipSpAttr[spAttr].SetEquipCnt;
            _ActingEquipSpAttr[spAttr].SetEquipValue -= equipValue;
            if (_ActingEquipSpAttr[spAttr].SetEquipCnt == 0)
            {
                _ActingEquipSpAttr.Remove(spAttr);
            }
        }
    }

    public void ActingSpAttr(EquipSpAttrRecord spAttr, int equipValue)
    {
        if (!_ActingEquipSpAttr.ContainsKey(spAttr))
        {
            _ActingEquipSpAttr.Add(spAttr, new EquipSetInfo());
        }
        _ActingEquipSpAttr[spAttr].SetEquipCnt = 1;
        _ActingEquipSpAttr[spAttr].SetEquipValue = equipValue;
    }

    #region attr

    private Dictionary<EquipSpAttrRecord, List<EquipExAttr>> _SpRecordAttrs = null;
    public Dictionary<EquipSpAttrRecord, List<EquipExAttr>> SpRecordAttrs
    {
        get
        {
            if (_SpRecordAttrs == null)
            {
                InitRecordAttrs();
            }
            return _SpRecordAttrs;
        }
    }

    public List<EquipExAttr> GetEquipAttr(EquipSpAttrRecord spRecord)
    {
        return SpRecordAttrs[spRecord];
    }

    private void InitRecordAttrs()
    {
        _SpRecordAttrs = new Dictionary<EquipSpAttrRecord, List<EquipExAttr>>();
        foreach (var spAttrRecord in TableReader.EquipSpAttr.Records.Values)
        {
            _SpRecordAttrs.Add(spAttrRecord, new List<EquipExAttr>());
            foreach (var attrParam in spAttrRecord.Attr)
            {
                if (attrParam > 0)
                {
                    var exAttr = TableReader.AttrValue.GetExAttr(attrParam.ToString(), 1);
                    _SpRecordAttrs[spAttrRecord].Add(exAttr);
                }
            }
        }
    }

    #endregion
}
