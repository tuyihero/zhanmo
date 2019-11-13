using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIGiftPack : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        UIGlobalBuff._ShowType = 1;
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIGiftPack, UILayer.BaseUI, hash);
    }

    public static void Refresh()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGiftPack>(UIConfig.UIGiftPack);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        
    }

    #endregion

    #region 

    [System.Serializable]
    public class GiftShow
    {
        public Text Name;
        public UICommonAwardItem[] GiftItem;
        public Text Tips;
    }

    public GiftShow[] GiftShows;
    public Text _Price;
    public Text Tips;
    public Text ItemTips;

    public override void Show(Hashtable hash)
    {
        Debug.Log("UIGiftPack Show");
        AdManager.Instance.PrepareVideo();
        base.Show(hash);

        for (int i = 0; i < GiftData.Instance._GiftItems.Count; ++i)
        {
            var commonItem = Tables.TableReader.CommonItem.GetRecord(GiftData.Instance._GiftItems[i].Id);
            GiftShows[i].Name.text = Tables.StrDictionary.GetFormatStr(commonItem.NameStrDict);
            //if (GiftData.Instance._GiftItems[i].ActScript.Equals("BuyOneTime"))
            //{
            //    GiftShows[i].Tips.text = Tables.StrDictionary.GetFormatStr(1720000);
            //}
            //else
            //{
            //    GiftShows[i].Tips.text = "";
            //}
            
            if (GiftData.Instance._GiftItems[i].Diamond > 0)
            {
                GiftShows[i].GiftItem[0].gameObject.SetActive(true);
                GiftShows[i].GiftItem[0].ShowAward(MONEYTYPE.DIAMOND, GiftData.Instance._GiftItems[i].Diamond);
            }
            else
            {
                GiftShows[i].GiftItem[0].gameObject.SetActive(false);
            }

            if (GiftData.Instance._GiftItems[i].Gold > 0)
            {
                GiftShows[i].GiftItem[1].gameObject.SetActive(true);
                GiftShows[i].GiftItem[1].ShowAward(MONEYTYPE.GOLD, GiftData.Instance._GiftItems[i].Gold);
            }
            else
            {
                GiftShows[i].GiftItem[1].gameObject.SetActive(false);
            }

            if (GiftData.Instance._GiftItems[i].Item[0] != null)
            {
                GiftShows[i].GiftItem[2].gameObject.SetActive(true);
                GiftShows[i].GiftItem[2].ShowAward(GiftData.Instance._GiftItems[i].Item[0].Id, GiftData.Instance._GiftItems[i].ItemNum[0]);
            }
            else
            {
                GiftShows[i].GiftItem[2].gameObject.SetActive(false);
            }
        }

        //_Price.text = GiftData.Instance._GiftItems[1].Price.ToString();

        if (GiftData.Instance._GiftItems[1].Item[0] != null)
        {
            ItemTips.text = Tables.StrDictionary.GetFormatStr(GiftData.Instance._GiftItems[1].Item[0].DescStrDict);
        }
        else
        {
            ItemTips.text = "";
        }

        Hashtable eventHash = new Hashtable();
        eventHash.Add("GiftGroup", GiftData.Instance._GiftItems[0].GroupID);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_GIFT_OPEN, this, eventHash);
    }

    public override void Hide()
    {
        base.Hide();

        GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_IAP_SUCESS, OnChargeSucess);
    }

    public void OnBtnAdGift()
    {
        GiftData.Instance.SetLockingGift(true);
        AdManager.Instance.WatchAdVideo(OnAdGift);
    }

    public void OnAdGift()
    {
        UIGiftGetTips.ShowAsyn(GiftData.Instance.LockingGift);

        Hashtable eventHash = new Hashtable();
        eventHash.Add("GiftID", GiftData.Instance.LockingGift.Id);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_GIFT_AD, this, eventHash);

        GiftData.Instance.BuyGift();
        Hide();
    }

    public void OnBtnPurchGift()
    {
        GiftData.Instance.SetLockingGift(false);
        UIRechargePack.ShowAsyn();

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_IAP_SUCESS, OnChargeSucess);

    }

    public void OnChargeSucess(object e, Hashtable hash)
    {
        UIGiftGetTips.ShowAsyn(GiftData.Instance.LockingGift);

        Hashtable eventHash = new Hashtable();
        eventHash.Add("GiftID", GiftData.Instance.LockingGift.Id);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_GIFT_BUY, this, eventHash);

        GiftData.Instance.BuyGift();
        Hide();
    }

    #endregion

    #region test 

    public void OnBtnLoad()
    {
        AdManager.Instance.PrepareInterAD();
    }

    public void OnBtnShow()
    {
        GiftData.Instance.SetLockingGift(false);
        UIGiftGetTips.ShowAsyn(GiftData.Instance.LockingGift);
        GiftData.Instance.BuyGift();

    }

    #endregion
}

