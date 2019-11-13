using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Tables;

public class UIGiftGetTips : UIBase
{

    #region static funs

    public static void ShowAsyn(GiftPacketRecord giftRecord)
    {
        UIGlobalBuff._ShowType = 1;
        Hashtable hash = new Hashtable();
        hash.Add("GiftRecord", giftRecord);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIGiftGetTips, UILayer.BaseUI, hash);
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

    public GiftShow GiftShows;
    public Text _Price;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        var giftRecord = (GiftPacketRecord)hash["GiftRecord"];

        var commonItem = Tables.TableReader.CommonItem.GetRecord(giftRecord.Id);
        GiftShows.Name.text = Tables.StrDictionary.GetFormatStr(commonItem.NameStrDict);

        if (giftRecord.Diamond > 0)
        {
            GiftShows.GiftItem[0].gameObject.SetActive(true);
            GiftShows.GiftItem[0].ShowAward(MONEYTYPE.DIAMOND, giftRecord.Diamond);
        }
        else
        {
            GiftShows.GiftItem[0].gameObject.SetActive(false);
        }

        if (giftRecord.Gold > 0)
        {
            GiftShows.GiftItem[1].gameObject.SetActive(true);
            GiftShows.GiftItem[1].ShowAward(MONEYTYPE.GOLD, giftRecord.Gold);
        }
        else
        {
            GiftShows.GiftItem[1].gameObject.SetActive(false);
        }

        if (giftRecord.Item[0] != null)
        {
            GiftShows.GiftItem[2].gameObject.SetActive(true);
            GiftShows.GiftItem[2].ShowAward(giftRecord.Item[0].Id, giftRecord.ItemNum[0]);
        }
        else
        {
            GiftShows.GiftItem[2].gameObject.SetActive(false);
        }

        //_Price.text = giftRecord.Price.ToString();
    }
    
    #endregion
}

