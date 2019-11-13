using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tables;

public class ShopData : SaveItemBase
{
    #region 唯一

    private static ShopData _Instance = null;
    public static ShopData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new ShopData();
            }
            return _Instance;
        }
    }

    private ShopData()
    {
        _SaveFileName = "ShopData";
    }

    #endregion

    #region shop refresh
    
    public void InitShop()
    {
        InitShopItem();
        InitGambling();

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_PASS_STAGE, EventPassStage);
    }

    private void EventPassStage(object go, Hashtable eventArgs)
    {
        _RefreshShopItemsFlag = true;
    }

    public void SellItem(ItemBase sellItem, bool isNeedEnsure = true)
    {
        if (isNeedEnsure)
        {
            if (sellItem is ItemEquip)
            {
                ItemEquip itemEquip = sellItem as ItemEquip;
                if (itemEquip.EquipRefreshCostMatrial > 0)
                {
                    UIMessageBox.Show(20002, null, null, BtnType.OKBTN);
                    return;
                }
            }

            if (sellItem.CommonItemRecord.Quality == ITEM_QUALITY.ORIGIN
                || sellItem.CommonItemRecord.Quality == ITEM_QUALITY.PURPER)
            {
                UIMessageBox.Show(20003, () => { SellItemOK(sellItem); }, null);
                return;
            }
        }

        SellItemOK(sellItem);

    }

    public void SellItemOK(ItemBase sellItem)
    {
        int gold = 1;
        if (sellItem is ItemEquip)
        {
            gold = GameDataValue.GetEquipSellGold((ItemEquip)sellItem);
        }
        sellItem.ResetItem();
        PlayerDataPack.Instance.AddGold(gold);
        sellItem.SaveClass(true);
    }

    

    #endregion

    #region item shop

    public class ShopRandomGroup
    {
        public string GroupName;
        public int GroupItemCnt;
        public int GroupItemRateTotal;
        public List<ShopItemRecord> ShopItems;
    }

    private List<ShopRandomGroup> _ShopGroups;
    
    public List<ItemShop> _ShopItems = new List<ItemShop>();

    public bool _RefreshShopItemsFlag = true;

    public void InitShopGroup()
    {
        if (_ShopGroups != null)
            return;

        _ShopGroups = new List<ShopRandomGroup>();
        foreach (var shopItem in TableReader.ShopItem.Records.Values)
        {
            if (shopItem.ClassItemCnt <= 0)
            {
                ShopRandomGroup shopGroup = new ShopRandomGroup();
                shopGroup.GroupName = shopItem.Class;
                shopGroup.GroupItemCnt = 1;
                shopGroup.GroupItemRateTotal = 10000;
                shopGroup.ShopItems = new List<ShopItemRecord>() { shopItem };

                _ShopGroups.Add(shopGroup);
            }
            else
            {
                ShopRandomGroup shopGroup = _ShopGroups.Find((groupInfo) =>
                {
                    if (groupInfo.GroupName == shopItem.Class)
                        return true;
                    return false;
                });
                if (shopGroup == null)
                {
                    shopGroup = new ShopRandomGroup();
                    shopGroup.GroupName = shopItem.Class;
                    shopGroup.GroupItemCnt = shopItem.ClassItemCnt;
                    shopGroup.GroupItemRateTotal = shopItem.Prior;
                    shopGroup.ShopItems = new List<ShopItemRecord>() { shopItem };

                    _ShopGroups.Add(shopGroup);
                }
                else
                {
                    shopGroup.GroupItemRateTotal += shopItem.Prior;
                    shopGroup.ShopItems.Add(shopItem);
                }
            }
        }
    }

    public void InitShopItem()
    {
        RefreshShopItem();
    }

    public ShopItemRecord GetRandomShopRecord(List<ShopItemRecord> randomList)
    {
        int[] randomRate = new int[randomList.Count];
        for (int i = 0; i < randomList.Count; ++i)
        {
            randomRate[i] = randomList[i].Prior;
        }
        int idx = GameRandom.GetRandomLevel(randomRate);
        return randomList[idx];
    }

    public void RefreshShopItem()
    {
        InitShopGroup();

        _ShopItems.Clear();
        foreach (var shopGroup in _ShopGroups)
        {
            if (shopGroup.ShopItems.Count == 1)
            {
                if (shopGroup.ShopItems[0].Prior <= 0)
                {
                    _ShopItems.Add(new ItemShop(shopGroup.ShopItems[0].Id));
                }
                if (GameRandom.IsInRate(shopGroup.ShopItems[0].Prior))
                {
                    _ShopItems.Add(new ItemShop(shopGroup.ShopItems[0].Id));
                }
            }
            else
            {
                List<ShopItemRecord> tempShopItems = new List<ShopItemRecord>(shopGroup.ShopItems);
                for (int i = 0; i < shopGroup.GroupItemCnt; ++i)
                {
                    var shopRecord = GetRandomShopRecord(tempShopItems);
                    _ShopItems.Add(new ItemShop(shopRecord.Id));
                    tempShopItems.Remove(shopRecord);
                }
            }
        }

        _RefreshShopItemsFlag = false;
    }

    public bool BuyItem(ItemShop shopItem, int buyNum = 1)
    {
        if (shopItem.ShopRecord.DailyLimit > 0)
        {
            int lastNumCnt = shopItem.ShopRecord.DailyLimit - shopItem.BuyTimes;
            if (lastNumCnt < buyNum)
            {
                UIMessageTip.ShowMessageTip(20004);
                return false;
            }
        }

        var totalPrice = buyNum * shopItem.BuyPrice;
        if (shopItem.ShopRecord.MoneyType == 0)
        {
            if (!PlayerDataPack.Instance.DecGold(totalPrice))
                return false;
        }
        else
        {
            if (!PlayerDataPack.Instance.DecDiamond(totalPrice))
                return false;
        }

        var scriptType = Type.GetType(shopItem.ShopRecord.Script);
        var buyMethod = scriptType.GetMethod("BuyItem", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        for (int i = 0; i < buyNum; ++i)
        {
            buyMethod.Invoke(null, new object[1] { shopItem });

            ++shopItem.BuyTimes;
        }

        Debug.Log("BuyItem:" + shopItem.ItemDataID);

        Hashtable eventHash = new Hashtable();
        eventHash.Add("ShopBuyItem", shopItem);
        eventHash.Add("ShopBuyNum", buyNum);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_SHOP_BUY, this, eventHash);
        return true;
    }

    #endregion

    #region gambling

    public List<ItemEquip> _GamblingEquips = new List<ItemEquip>();
    public List<ItemFiveElementCore> _GamblingCores = new List<ItemFiveElementCore>();
    public static int _MAX_RANDOM_EQUIP_CNT = 5;
    public static string _GAMBLING_COST_ITEM_ID = "1600002";

    public void InitGambling()
    {
        RefreshGambling();
    }

    public void RefreshGambling()
    {
        _GamblingEquips.Clear();
        int proLimit = 5;
        //if (RoleData.SelectRole.Profession == PROFESSION.GIRL_DEFENCE
        //    || RoleData.SelectRole.Profession == PROFESSION.GIRL_DOUGE)
        //    proLimit = 10;
        var weaponQuality = GameDataValue.GetGamblingEquipQuality(RoleData.SelectRole.TotalLevel);
        ItemEquip weapon = ItemEquip.CreateEquip(RoleData.SelectRole.TotalLevel, weaponQuality, -1, 0, proLimit);
        _GamblingEquips.Add(weapon);
        for (int i = 0; i < _MAX_RANDOM_EQUIP_CNT; ++i)
        {
            var itemQuality = GameDataValue.GetGamblingEquipQuality(RoleData.SelectRole.TotalLevel);
            var randomSlot = GameDataValue.GetRandomItemSlot(itemQuality);
            var legendaryID = GameDataValue.GetGamblingEquipLegendary(RoleData.SelectRole.TotalLevel, randomSlot);
            ItemEquip equip = ItemEquip.CreateEquip(RoleData.SelectRole.TotalLevel, itemQuality, legendaryID, (int)randomSlot, proLimit);
            _GamblingEquips.Add(equip);
        }

        _GamblingCores.Clear();
        //for (int i = 0; i < (int)FIVE_ELEMENT.EARTH + 1; ++i)
        //{
        //    int elementLevel = GameDataValue.GetNearestLevel(RoleData.SelectRole.TotalLevel);
        //    var itemQuality = GameDataValue.GetGamblingCoreQuality(RoleData.SelectRole.TotalLevel);
        //    var coreRecord = TableReader.FiveElementCore.GetElementCoreRecord(itemQuality, (FIVE_ELEMENT)i);
        //    var coreItem = GameDataValue.GetRandomFiveElementCore(coreRecord.Id, elementLevel);
        //    _GamblingCores.Add(coreItem);
        //}
        
    }

    public int GetGamblingEquipCost(ItemEquip itemEqiup)
    {
        return 10000;
    }

    public int GetGamblingCoreCost(ItemFiveElementCore itemCore)
    {
        return 10000;
    }

    public bool BuyGambling(ItemEquip itemEquip)
    {
        if (BackBagPack.Instance.PageEquips.GetEmptyPos() == null)
        {
            UIMessageTip.ShowMessageTip(10002);
            return false;
        }

        int itemCnt = BackBagPack.Instance.PageItems.GetItemCnt(_GAMBLING_COST_ITEM_ID);
        if (itemCnt > 0)
        {
            if (!BackBagPack.Instance.PageItems.DecItem(_GAMBLING_COST_ITEM_ID, 1))
                return false;
        }
        else
        {
            if (!PlayerDataPack.Instance.DecGold(GetGamblingEquipCost(itemEquip)))
                return false;
        }

        string strTips = Tables.StrDictionary.GetFormatStr(2300088, string.Format("{0} * {1}", itemEquip.GetEquipNameWithColor(), 1));
        UIMessageTip.ShowMessageTip(strTips);

        BackBagPack.Instance.AddEquip(itemEquip);
        //_GamblingEquips.Remove(itemEquip);
        

        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_GAMBLING, this, null);

        return true;
    }

    public bool BuyGambling(ItemFiveElementCore itemCore)
    {
        if (!PlayerDataPack.Instance.DecGold(GetGamblingCoreCost(itemCore)))
            return false;

        FiveElementData.Instance.AddCoreItem(itemCore);
        _GamblingCores.Remove(itemCore);
        return true;
    }
    #endregion

    #region sell data

    public List<ITEM_QUALITY> _SellQualityTemp = new List<ITEM_QUALITY>();
    public List<Vector2> _SellLevelTemp = new List<Vector2>();

    #endregion
}
