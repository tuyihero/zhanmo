using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
using UnityEngine.EventSystems;
using System;

public class UISummonCollectItem : UIItemSelect
{
    public GameObject _InfoPanel;
    public Text _Name;
    public Image _Quality;
    public Image _Icon;
    public GameObject[] _Stars;
    public GameObject _NotCollectedGO;


    public override void Show(Hashtable hash)
    {
        base.Show();

        var summonCollect = (SummonCollectItem)hash["InitObj"];
        if (summonCollect == null)
            return;

        ShowSummonRecord(summonCollect);

    }

    public void ShowSummonRecord(SummonCollectItem summonCollect)
    {
        int star = summonCollect._Star;

        if (star >= 0)
        {
            _NotCollectedGO.SetActive(false);
        }
        else
        {
            _NotCollectedGO.SetActive(true);
        }

        _InfoPanel.SetActive(true);
        _Name.text = CommonDefine.GetQualityColorStr(summonCollect._SummonRecord.Quality) + summonCollect._SummonRecord.Name + "</color>";
        for (int i = 0; i < _Stars.Length; ++i)
        {
            if (i < star)
            {
                _Stars[i].gameObject.SetActive(true);
            }
            else
            {
                _Stars[i].gameObject.SetActive(false);
            }
        }
    }
}

