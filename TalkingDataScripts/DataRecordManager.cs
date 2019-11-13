using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

public class DataRecordManager
{
    #region 唯一

    private static DataRecordManager _Instance = null;
    public static DataRecordManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new DataRecordManager();
            }
            return _Instance;
        }
    }

    private DataRecordManager() { }

    #endregion

    TDGAAccount account;

    public void InitDataRecord()
    {
        TalkingDataGA.OnStart("6825B11C1864443284573062E27D0463", "ggp");
        account = TDGAAccount.SetAccount(TalkingDataGA.GetDeviceId());

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_ROLE_LEVEL_UP, RoleLvUpHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_ENTER_STAGE, EnterStageHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_PASS_STAGE, PassStageHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_GET, GemGetHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, EquipRefreshHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_GAMBLING, GamblingHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_SHOP_BUY, ShopBuyHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_SOUL_LOTTERY, SoulLotteryHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_GIFT_OPEN, GiftOpenHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_GIFT_AD, GiftADHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_GIFT_BUY, GiftBuyHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_LEVELUP_SKILL, SkillLvUpHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_IAP_REQ, IAPReqHandle);
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_IAP_SUCESS, IAPSucessHandle);
    }

    public void RoleLvUpHandle(object e, Hashtable hash)
    {
        Debug.Log("RoleLvUpHandle");
        account.SetLevel(RoleData.SelectRole.TotalLevel);
    }

    public void EnterStageHandle(object e, Hashtable hash)
    {
        Debug.Log("EnterStageHandle");
        if (ActData.Instance._StageMode == Tables.STAGE_TYPE.NORMAL)
        {
            int level = ActData.Instance.GetStageLevel();
            TDGAMission.OnBegin(level.ToString());
        }
        else
        {
            TDGAMission.OnBegin("GOLD_ACT");

        }
    }

    public void PassStageHandle(object e, Hashtable hash)
    {
        Debug.Log("PassStageHandle");
        if (ActData.Instance._StageMode == Tables.STAGE_TYPE.NORMAL)
        {
            int level = ActData.Instance.GetStageLevel();
            TDGAMission.OnCompleted(level.ToString());
        }
        else
        {
            TDGAMission.OnCompleted("GOLD_ACT");

        }
    }

    public void GemGetHandle(object e, Hashtable hash)
    {
        Debug.Log("GemGetHandle");
        string gemDataID = (string)hash["AddGemData"];
        int gemCnt = (int)hash["AddGemNum"];
        
        TDGAItem.OnPurchase(gemDataID, gemCnt, 1);
    }

    public void EquipRefreshHandle(object e, Hashtable hash)
    {
        Debug.Log("EquipRefreshHandle");
        Dictionary<string, object> dic = new Dictionary<string, object>();
        TalkingDataGA.OnEvent("EquipRefresh", dic);
    }

    public void GamblingHandle(object e, Hashtable hash)
    {
        Debug.Log("GamblingHandle");
        Dictionary<string, object> dic = new Dictionary<string, object>();
        TalkingDataGA.OnEvent("Gambling", dic);
    }

    public void ShopBuyHandle(object e, Hashtable hash)
    {
        Debug.Log("ShopBuyHandle");
        var itemShop = (ItemShop)hash["ShopBuyItem"];
        var itemNum = (int)hash["ShopBuyNum"];
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("ItemID", itemShop.ItemDataID);
        dic.Add("ItemNum", itemNum);
        TalkingDataGA.OnEvent("ShopBuy", dic);
    }

    public void SoulLotteryHandle(object e, Hashtable hash)
    {
        Debug.Log("SoulLotteryHandle");
        var type = (int)hash["LotteryType"];
        var result = (SummonSkillData.LotteryResult)hash["LotteryResult"];
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("LotteryType", type);
        dic.Add("LotteryNum", result._SummonData.Count);
        TalkingDataGA.OnEvent("SoulLottery", dic);
    }

    public void GiftOpenHandle(object e, Hashtable hash)
    {
        Debug.Log("GiftOpenHandle");
        int giftGroup = (int)hash["GiftGroup"];

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("GiftGroup", giftGroup);
        TalkingDataGA.OnEvent("GiftOpen", dic);
    }

    public void GiftADHandle(object e, Hashtable hash)
    {
        Debug.Log("GiftADHandle");
        var giftRecord = (string)hash["GiftRecord"];

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("GiftID", giftRecord);
        dic.Add("GiftType", 1);
        TalkingDataGA.OnEvent("GiftAD", dic);
    }

    public void GiftBuyHandle(object e, Hashtable hash)
    {
        Debug.Log("GiftBuyHandle");
        var giftRecord = (string)hash["GiftRecord"];

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("GiftID", giftRecord);
        dic.Add("GiftType", 2);
        TalkingDataGA.OnEvent("GiftBuy", dic);
    }

    public void SkillLvUpHandle(object e, Hashtable hash)
    {
        Debug.Log("SkillLvUpHandle");
        var skillID = (string)hash["SkillID"];
        var skillLv = (int)hash["SkillLevel"];

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("SkillID", skillID);
        dic.Add("skillLv", skillLv);
        TalkingDataGA.OnEvent("SkillLvUp", dic);
    }

    public void IAPReqHandle(object e, Hashtable hash)
    {
        Debug.Log("IAPReqHandle");
        string punchID = (string)hash["PurchID"];
        string orderID = (string)hash["OrderID"];
        var chargeRecord = Tables.TableReader.Recharge.GetRecord(punchID);

        TDGAVirtualCurrency.OnChargeRequest(orderID, chargeRecord.Id, chargeRecord.Price, "CH", chargeRecord.Num, "PT");
    }

    public void IAPSucessHandle(object e, Hashtable hash)
    {
        Debug.Log("IAPSucessHandle");

        string punchID = (string)hash["PurchID"];
        string orderID = (string)hash["OrderID"];
        var chargeRecord = Tables.TableReader.Recharge.GetRecord(punchID);

        TDGAVirtualCurrency.OnChargeSuccess(orderID);
    }
    
}
