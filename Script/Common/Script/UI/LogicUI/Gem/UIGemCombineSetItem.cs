using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;


public class UIGemCombineSetItem : UIItemBase
{
    public UIGemCombineSetGemItem _ResultItem;
    public List<UIGemCombineSetGemItem> _MaterialItem;
    public GameObject _LastPlus;
    public Button _BtnApply;

    private Tables.GemTableRecord _GemRecord;
    private Tables.GemTableRecord _ResultGemRecord;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var gemInfo = (GemTableRecord)hash["InitObj"];
        ShowGem(gemInfo);
    }

    public void ShowGem(GemTableRecord gemRecord)
    {
        _GemRecord = gemRecord;

        List<List<GemTableRecord>> gemCombineRecords = new List<List<GemTableRecord>>();
        for (int i = 0; i < _GemRecord.Combine.Count; ++i)
        {
            if (_GemRecord.Combine[i] > 0)
            {
                var gemRecords = GemData.Instance.GetAllLevelGemRecords(_GemRecord.Combine[i]);
                gemCombineRecords.Add(gemRecords);
            }
        }

        int maxLevel = AllHaveLevel(gemCombineRecords);

        var resultRecord = GemData.Instance.GetGemByClass(_GemRecord.Class, maxLevel > 0?maxLevel:1);
        _ResultItem.ShowGem(resultRecord, true);

        for (int i = 0; i < _MaterialItem.Count; ++i)
        {
            if (gemCombineRecords.Count > i && gemCombineRecords[i].Count > 0)
            {
                var matGemRecord = GemData.Instance.GetGemByClass(gemCombineRecords[i][0].Class, maxLevel > 0 ? maxLevel : 1);
                if (matGemRecord != null)
                {
                    _MaterialItem[i].ShowGem(matGemRecord, true);
                }
                else
                {
                    _MaterialItem[i].ShowGem(gemCombineRecords[i][gemCombineRecords[i].Count - 1], true);
                }
            }
            else
            {
                GemTableRecord matGemRecord = null;
                if (_GemRecord.Combine[i] > 0)
                {
                    matGemRecord = TableReader.GemTable.GetRecord(_GemRecord.Combine[i].ToString());
                    _LastPlus.SetActive(true);
                }
                else
                {
                    _LastPlus.SetActive(false);
                }
                _MaterialItem[i].ShowGem(matGemRecord, false);
            }
        }

        if (maxLevel > 0)
        {
            _ResultGemRecord = resultRecord;
            _BtnApply.interactable = true;
        }
        else
        {
            _ResultGemRecord = null;
            _BtnApply.interactable = false;
        }
    }

    public void OnBtnUseSet()
    {
        if (_ResultGemRecord != null)
            UIGemPack.SetGemCombine(_ResultGemRecord);

        UIGemCombineSet.HideAsyn();
    }

    private int AllHaveLevel(List<List<GemTableRecord>>  gemRecords)
    {
        List<int> lastLevels = new List<int>();
        List<int> commonLevels = new List<int>();
        for (int i = 0; i < gemRecords[0].Count; ++i)
        {
            commonLevels.Add(gemRecords[0][i].Level);
        }

        for (int i = 1; i < gemRecords.Count; ++i)
        {
            lastLevels = new List<int>(commonLevels);
            commonLevels = new List<int>();
            for (int j = 0; j < gemRecords[i].Count; ++j)
            {
                if (lastLevels.Contains(gemRecords[i][j].Level))
                {
                    commonLevels.Add(gemRecords[i][j].Level);
                }
            }
        }

        if (commonLevels.Count == 0)
            return -1;

        commonLevels.Sort((lvA, lvB) =>
        {
            if (lvA > lvB)
                return 1;
            else if (lvA < lvB)
                return -1;
            else
                return 0;
        });

        return commonLevels[0];
    }

}

