using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;


public class UIGemSuitItem : UIItemSelect
{
    public Text _SuitName;
    public Text _SuitDesc;
    //public UIContainerBase _SuitGems;
    public Button _BtnApply;

    private Tables.GemSetRecord _GemSetTab;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var gemInfo = (GemSetRecord)hash["InitObj"];
        ShowGem(gemInfo);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowGem(_GemSetTab);
    }


    public void ShowGem(GemSetRecord gemSet)
    {
        _GemSetTab = gemSet;

        _SuitName.text = StrDictionary.GetFormatStr(gemSet.Name);
        _SuitDesc.text = StrDictionary.GetFormatStr(gemSet.Desc);

        //Hashtable hash = new Hashtable();
        //hash.Add("MinLevel", gemSet.MinGemLv);
        //hash.Add("IsClearGem", gemSet.IsEnableDefault);
        //_SuitGems.InitContentItem(GemSuit.Instance.GetRecordGemRecords(gemSet), null, hash);

        if (GemSuit.Instance.SuitMinLevel(gemSet) > 0)
        {
            _BtnApply.interactable = true;
        }
        else
        {
            _BtnApply.interactable = false;
        }
    }

    public void OnBtnUseSet()
    {
        Debug.Log("OnBtnUseSet:" + _GemSetTab.Id);
        GemSuit.Instance.UseGemSet(_GemSetTab);
        UIGemPack.RefreshPunchPack();
    }

}

