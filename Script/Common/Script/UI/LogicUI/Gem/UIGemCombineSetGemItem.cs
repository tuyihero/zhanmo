
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;

public class UIGemCombineSetGemItem : UIPackItemBase
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

        ShowGem(_GemRecord, _ClearGem);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowGem(_GemRecord, _ClearGem);
    }


    public void ShowGem(Tables.GemTableRecord gemRecord, bool isClear)
    {
        if (gemRecord == null)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }
        _Icon.gameObject.SetActive(true);

        if (!isClear)
        {
            if (_DisableGO != null)
                _DisableGO.SetActive(true);

        }
        else
        {
            if (_DisableGO != null)
                _DisableGO.SetActive(false);

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

