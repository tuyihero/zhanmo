
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;

public class UIGemSuitGemItem : UIPackItemBase
{
    public GameObject _UnClearItem; 

    private Tables.GemTableRecord _GemRecord;
    private int _MinGemLv;
    private bool _ClearGem;

    public override void Show(Hashtable hash)
    {
        base.Show();

        _GemRecord = (Tables.GemTableRecord)hash["InitObj"];
        _MinGemLv = (int)hash["MinLevel"];
        _ClearGem = (bool)hash["IsClearGem"];

        ShowGem(_GemRecord, _MinGemLv);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowGem(_GemRecord, _MinGemLv);
    }


    public void ShowGem(Tables.GemTableRecord gemRecord, int minLevel)
    {

        var gemData = GemData.Instance.GetGemClassMax(gemRecord.Class, minLevel, null);
        if (gemData == null || !gemData.IsVolid())
        {
            ClearItem();
            return;
        }

        if (_Num != null)
        {
            {
                _Num.text = "";
            }
        }
        _Icon.gameObject.SetActive(true);

        if (_DisableGO != null)
        {
            if (gemData.GemRecord.Level >= _MinGemLv)
            {
                _DisableGO.SetActive(false);
            }
            else
            {
                _DisableGO.SetActive(true);
            }
        }

        if (!_ClearGem)
        {
            if (gemData.GemRecord.Level >= _MinGemLv)
            {
                _UnClearItem.SetActive(false);
            }
            else
            {
                _UnClearItem.SetActive(true);
            }
        }
        else
        {
            _UnClearItem.SetActive(false);
        }
    }

    protected override void ClearItem()
    {
        base.ClearItem();

        if (_DisableGO != null)
        {

            _DisableGO.SetActive(false);
        }
    }

    #region 

    public override void OnItemClick()
    {
        base.OnItemClick();
    }

    #endregion
}

