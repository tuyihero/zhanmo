using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
using UnityEngine.EventSystems;
using System;

public class SummonAttrInfo
{
    public int AttrIdx;
    public int CurValue;
    public int MaxValue;
    public RoleAttrEnum AttrEnum;
}

public class UISummonStageUpAttrItem : UIItemBase
{
    public Text _AttrName;
    public Text _AttrValue;
    public Text _AddValue;
    public UICostItem _CostItem;
    public Slider _AttrProcess;
    public Button _BtnAddAttr;

    private SummonAttrInfo _SummonAttrInfo;
    public SummonAttrInfo SummonAttrInfo
    {
        get
        {
            return _SummonAttrInfo;
        }
    }

    private SummonMotionData _SummonMotionData;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var attrInfo = (SummonAttrInfo)hash["InitObj"];
        if (attrInfo == null)
            return;

        if (hash.ContainsKey("SummonMotionData"))
        {
            _SummonMotionData = (SummonMotionData)hash["SummonMotionData"];
        }

        ShowSummonData(attrInfo);

    }

    public override void Refresh()
    {
        base.Refresh();

        ShowSummonData(_SummonAttrInfo);
    }

    public void ShowSummonData(SummonAttrInfo summonAttr)
    {
        //if (_ArraySelect != null)
        //{
        //    _ArraySelect.SetActive(false);
        //}

        _SummonAttrInfo = summonAttr;

        _AttrName.text = Tables.StrDictionary.GetFormatStr((int)_SummonAttrInfo.AttrEnum);
        _AttrValue.text = _SummonAttrInfo.CurValue.ToString();
        _AttrProcess.value = ((float)_SummonAttrInfo.CurValue) / _SummonAttrInfo.MaxValue;

        if (_SummonAttrInfo.CurValue == _SummonAttrInfo.MaxValue)
        {
            _BtnAddAttr.enabled = false;
        }
        else
        {
            _BtnAddAttr.enabled = true;
        }

        //_AddValue.text = "+" + SummonMotionData.StageAttrAdd[_SummonAttrInfo.AttrIdx];
        //_CostItem.ShowCost(_SummonMotionData.GetAttrCostItem(_SummonAttrInfo.AttrIdx), 1);
    }

    #region 

    public void AddAttr()
    {
        //SummonSkillData.Instance.StageAddAttr(_SummonMotionData, _SummonAttrInfo.AttrIdx);

        //_SummonAttrInfo.CurValue = _SummonMotionData.StageAttrs[_SummonAttrInfo.AttrIdx];
        Refresh();
    }

    #endregion
}

