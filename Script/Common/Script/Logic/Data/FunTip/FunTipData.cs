using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tables;

public class FunTipData : DataPackBase
{
    #region 唯一

    private static FunTipData _Instance = null;
    public static FunTipData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new FunTipData();
            }
            return _Instance;
        }
    }

    private FunTipData()
    {
        _SaveFileName = "FunTipData";
    }

    #endregion

    public enum FunTipType
    {
        Gem,
        Lv5Eqiup,
        Lv10Equip,

        MaxType,
    }

    public void InitFunTipData()
    {
        int maxnum = (int)FunTipType.MaxType;
        if (FunTipLogs == null)
        {
            FunTipLogs = new List<int>();
        }
        if (FunTipLogs.Count < maxnum)
        {
            int appendNum = maxnum - FunTipLogs.Count;
            for (int i = FunTipLogs.Count; i < appendNum; ++i)
            {
                FunTipLogs.Add(0);
            }
        }
    }

    #region 

    [SaveField(1)]
    private List<int> FunTipLogs;

    public int GetFunTip(FunTipType funTipType)
    {
        return FunTipLogs[(int)funTipType];
    }

    public void SetFunTip(FunTipType funTipType, int value)
    {
        FunTipLogs[(int)funTipType] = value;
        SaveClass(true);
    }

    #endregion
}
