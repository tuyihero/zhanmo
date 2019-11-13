using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tables;

public class GiftData : DataPackBase
{
    #region 唯一

    private static GiftData _Instance = null;
    public static GiftData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GiftData();
            }
            return _Instance;
        }
    }

    private GiftData()
    {
        _SaveFileName = "GiftData";
    }

    #endregion

    public void InitGiftData()
    {
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_PASS_STAGE, StagePassEvent);

        RefreshGift(true);
        _LastRefreshTimes = 0;
        _NextRefreshInterval = UnityEngine.Random.Range(_RefreshIntervalMin, _RefreshIntervalMax);
    }

    #region refresh gift

    public static int _RefreshIntervalMin = 2;
    public static int _RefreshIntervalMax = 4+1;
    public static int _GiftLiveTime = 1;

    private int _LastRefreshTimes = -1;
    private int _NextRefreshInterval = -1;

    public void StagePassEvent(object go, Hashtable eventArgs)
    {
        if (_LastRefreshTimes < 0)
        {
            RefreshGift();
            _LastRefreshTimes = 1;
            _NextRefreshInterval = UnityEngine.Random.Range(_RefreshIntervalMin, _RefreshIntervalMax);
        }
        else 
        {
            ++_LastRefreshTimes;
            if (_LastRefreshTimes == _NextRefreshInterval)
            {
                RefreshGift();
                _LastRefreshTimes = 0;
                _NextRefreshInterval = UnityEngine.Random.Range(_RefreshIntervalMin, _RefreshIntervalMax);
            }
            else
            {
                RefreshGift(true);
            }
        }
    }



    #endregion

    #region gift

    public List<GiftPacketRecord> _GiftItems = null;
    public bool _IsShowDefaultGift = true;

    private GiftPacketRecord _LockingGift;
    public GiftPacketRecord LockingGift
    {
        get
        {
            return _LockingGift;
        }
    }

    private void RefreshGift(bool isDefaultGift = false)
    {
        _IsShowDefaultGift = isDefaultGift;
        if (isDefaultGift)
        {
            if (IsCanShowGift(TableReader.GiftPacket.GiftPacketGroup[11][0]))
            {
                _GiftItems = TableReader.GiftPacket.GiftPacketGroup[11];
            }
        }
        else
        {
            List<int> randomGift = new List<int>();
            foreach (var giftRecrod in TableReader.GiftPacket.Records.Values)
            {
                if (giftRecrod.GroupID == 11)
                    continue;
                if (IsCanShowGift(giftRecrod))
                {
                    if (!randomGift.Contains(giftRecrod.GroupID))
                    {
                        randomGift.Add(giftRecrod.GroupID);
                    }
                }
                else
                {
                    if (randomGift.Contains(giftRecrod.GroupID))
                    {
                        randomGift.Remove(giftRecrod.GroupID);
                    }
                }
            }

            if (randomGift.Count == 0)
                return;

            int randomGroup = UnityEngine.Random.Range(0, randomGift.Count);
            _GiftItems = TableReader.GiftPacket.GiftPacketGroup[randomGift[randomGroup]];
        }

        UIMainFun.RefreshGift();
    }

    private void ClearGift()
    {
        _GiftItems = null;
    }

    public void SetLockingGift(bool isAd)
    {
        if (isAd)
        {
            _LockingGift = _GiftItems[0];
        }
        else
        {
            _LockingGift = _GiftItems[1];
        }
    }

    public void BuyGift()
    {
        //buy to do
        if (_LockingGift.PacketType == 1)
        {
            AdFinish();
        }
        else if (_LockingGift.PacketType == 2)
        {
            PurchFinish();
        }
    }

    public void PurchFinish()
    {
        PickGift();
    }

    public void AdFinish()
    {
        PickGift();
    }

    private void PickGift()
    {
        if (_LockingGift != null)
        {
            if (_LockingGift.Gold > 0)
            {
                PlayerDataPack.Instance.AddGold(_LockingGift.Gold);
            }

            if (_LockingGift.Diamond > 0)
            {
                PlayerDataPack.Instance.AddDiamond(_LockingGift.Diamond);
            }

            for (int i = 0; i < _LockingGift.Item.Count; ++i)
            {
                if (_LockingGift.Item[i] != null)
                {
                    BackBagPack.Instance.PageItems.AddItem(_LockingGift.Item[i].Id, _LockingGift.ItemNum[i]);
                }
            }

            SetGiftGroupBuy(_LockingGift);

            ClearGift();
            UIMainFun.RefreshGift();
        }
    }

    #endregion

    #region buy record

    [SaveField(1)]
    public List<int> _GiftBuyRecord = new List<int>();

    public bool IsGiftGroupPicked(int giftGroup)
    {
        return _GiftBuyRecord.Contains(giftGroup);
    }

    public void SetGiftGroupBuy(GiftPacketRecord giftRecord)
    {
        if (giftRecord.ActScript.Equals("BuyOneTime")
            && !IsGiftGroupPicked(giftRecord.GroupID))
        {
            _GiftBuyRecord.Add(giftRecord.GroupID);
            SaveClass(true);
        }        
    }

    public bool IsCanShowGift(GiftPacketRecord giftRecord)
    {
        if (giftRecord.ActScript.Equals("BuyOneTime")
            && IsGiftGroupPicked(giftRecord.GroupID))
        {
            return false;
        }

        if (giftRecord.LevelMin > RoleData.SelectRole.TotalLevel
            || (giftRecord.LevelMax > 0 && giftRecord.LevelMax < RoleData.SelectRole.TotalLevel))
        {
            return false;
        }

        return true;
    }

    #endregion
}
