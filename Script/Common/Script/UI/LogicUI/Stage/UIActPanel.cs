
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

 
public class UIActPanel : UIBase
{

    #region static funs

    public static void ShowAsyn(bool isShowTip = false)
    {
        Hashtable hash = new Hashtable();
        hash.Add("IsShowTip", isShowTip);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIActPanel, UILayer.PopUI, hash);
    }

    #endregion

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        OnHideTipTicket();
        if (hash.ContainsKey("IsShowTip"))
        {
            if ((bool)hash["IsShowTip"])
            {
                OnShowTipTicket();
            }
        }

        Tips1.text = StrDictionary.GetFormatStr(2300064, CommonDefine.GetQualityItemName(ActData._ACT_TICKET, true));
        Tips2.text = StrDictionary.GetFormatStr(2300065, CommonDefine.GetQualityItemName(ActData._ACT_TICKET, true));
        _TextPrice.text = ActData._ACT_TICKET_PRICE.ToString();
        _TextItemCnt.text = StrDictionary.GetFormatStr(2300069) + string.Format("({0})", BackBagPack.Instance.PageItems.GetItemCnt(ActData._ACT_TICKET));
    }

    #region 

    public GameObject _TipTicket;
    public GameObject _TipGetTicket;
    public Text Tips1;
    public Text Tips2;
    public Text _TextItemCnt;
    public Text _TextPrice;

    #endregion

    #region 

    public void OnShowTipTicket()
    {
        _TipTicket.SetActive(false);
        _TipGetTicket.SetActive(true);
    }

    public void OnHideTipTicket()
    {
        _TipTicket.SetActive(true);
        _TipGetTicket.SetActive(false);
    }

    public void OnBtnNormalEnter()
    {
        ActData.Instance.StartStage(1, STAGE_TYPE.ACT_GOLD, false);
        Hide();
    }

    public void OnBtnTicketEnter()
    {
        if (BackBagPack.Instance.PageItems.DecItem(ActData._ACT_TICKET, 1))
        {
            ActData.Instance.StartStage(1, STAGE_TYPE.ACT_GOLD, true);
            Hide();
        }
        else
        {
            OnShowTipTicket();
        }
    }

    public void OnBtnAdTicket()
    {
        AdManager.Instance.WatchAdVideo(OnAdTicket);
    }

    public void OnAdTicket()
    {
        ActData.Instance.AddActTicket();
        OnHideTipTicket();
        _TextItemCnt.text = StrDictionary.GetFormatStr(2300069) + string.Format("({0})", BackBagPack.Instance.PageItems.GetItemCnt(ActData._ACT_TICKET));
    }

    public void OnBtnBuyTicket()
    {
        if (PlayerDataPack.Instance.DecDiamond(ActData._ACT_TICKET_PRICE))
        {
            ActData.Instance.AddActTicket();
            OnHideTipTicket();
        }
    }

    #endregion

}

