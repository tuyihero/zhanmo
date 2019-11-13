using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
using UnityEngine.EventSystems;
using System;
using Tables;

public class UIGlobalBuffItem : UIItemBase
{
    public Image _Icon;
    public Text _Name;
    public Text _Desc;
    public UITimeCountDown _Refresh;
    public UITimeCountDown _TimeOut;
    public Button _BtnActByMovie;
    public Button _BtnActByDiamond;
    public Button _BtnDispare;

    private GlobalBuffData.GlobalBuffItem _BuffItem;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (GlobalBuffData.GlobalBuffItem)hash["InitObj"];
        ShowStage(showItem);
    }

    public void ShowStage(GlobalBuffData.GlobalBuffItem showItem)
    {
        _BuffItem = showItem;
        var globalRecord = TableReader.GlobalBuff.GetRecord(showItem._RecordID);
        _Name.text = StrDictionary.GetFormatStr(globalRecord.Name);
        _Desc.text = StrDictionary.GetFormatStr(globalRecord.Desc);
        if (GlobalBuffData.Instance.IsBuffActing(showItem))
        {
            _BtnActByMovie.gameObject.SetActive(false);
            _BtnActByDiamond.gameObject.SetActive(false);
            _BtnDispare.gameObject.SetActive(true);

            _Refresh.gameObject.SetActive(false);
            _TimeOut.gameObject.SetActive(true);
            var buffRecrod = TableReader.GlobalBuff.GetRecord(showItem._RecordID);
            DateTime actTime = DateTime.Parse(showItem._LastActTime);
            var deltaTime = (DateTime.Now - actTime).TotalSeconds;
            int lastSecond = buffRecrod.LastTime - (int)deltaTime;
            _TimeOut.SetCountDownSecond(lastSecond, UITimeCountDown.CountDownType.None, CountDownFinish);
        }
        else
        {
            _BtnActByMovie.gameObject.SetActive(true);
            _BtnActByDiamond.gameObject.SetActive(true);
            _BtnDispare.gameObject.SetActive(false);

            _Refresh.gameObject.SetActive(true);
            _TimeOut.gameObject.SetActive(false);
            DateTime refreshTime = DateTime.Parse(showItem._LastRefreshTime);
            var deltaTime = (DateTime.Now - refreshTime).TotalSeconds;
            int lastSecond = GlobalBuffData._RefreshBuffSecond - (int)deltaTime;
            _Refresh.SetCountDownSecond(lastSecond, UITimeCountDown.CountDownType.None, CountDownFinish);
        }
    }

    public void CountDownFinish()
    {
        Debug.Log("CountDownFinish!!!");
    }

    public void OnBtnActMovie()
    {
        GlobalBuffData.Instance.ActByAd(_BuffItem);
    }

    public void OnBtnActDiamond()
    {
        GlobalBuffData.Instance.ActByDiamond(_BuffItem);
    }

    public void OnBtnDispare()
    {
        GlobalBuffData.Instance.Dispare(_BuffItem);
    }


}

