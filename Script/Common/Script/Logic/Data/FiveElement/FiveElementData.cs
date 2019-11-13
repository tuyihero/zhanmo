using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tables;

public class FiveElementData : SaveItemBase
{
    #region 唯一

    private static FiveElementData _Instance = null;
    public static FiveElementData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new FiveElementData();
            }
            return _Instance;
        }
    }

    private FiveElementData()
    {
        _SaveFileName = "FiveElementData";
    }

    #endregion

    public void InitFiveElementData()
    {
        bool needSave = false;

        needSave |= InitPackElements();
        needSave |= InitUsingElements();
        needSave |= InitPackCore();
        needSave |= InitUsingCore();

        if (needSave)
        {
            SaveClass(true);
        }
    }

    #region element 

    public const int _ITEM_PACKET_CNT = 200;
    
    [SaveField(1)]
    public List<ItemFiveElement> _UsingElements;

    public ItemPackElement<ItemFiveElement> _PackElements;

    private bool InitPackElements()
    {
        _PackElements = new ItemPackElement<ItemFiveElement>();
        _PackElements._SaveFileName = "PackFiveElements";
        _PackElements._PackSize = _ITEM_PACKET_CNT;
        _PackElements.LoadClass(true);

        if (_PackElements._PackItems == null)
        {
            _PackElements._PackItems = new List<ItemFiveElement>();
            _PackElements.SaveClass(true);
            return true;
        }

        return false;
    }

    private bool InitUsingElements()
    {
        if (_UsingElements == null || _UsingElements.Count == 0)
        {
            _UsingElements = new List<ItemFiveElement>();
            _UsingElements.Add(new ItemFiveElement("1500001"));
            _UsingElements.Add(new ItemFiveElement("1500002"));
            _UsingElements.Add(new ItemFiveElement("1500003"));
            _UsingElements.Add(new ItemFiveElement("1500004"));
            _UsingElements.Add(new ItemFiveElement("1500005"));
            return true;
        }

        return false;
    }

    public void AddElementItem(ItemFiveElement elementItem)
    {
        for (int i = 0; i < _PackElements._PackItems.Count; ++i)
        {
            if (_PackElements._PackItems[i].ItemDataID == elementItem.ItemDataID
                && _PackElements._PackItems[i].Level == elementItem.Level)
            {
                _PackElements._PackItems[i].AddStackNum(1, true);
                return;
            }
        }
        
        _PackElements.AddItem(elementItem);
    }

    public void SortPack()
    {
        _PackElements._PackItems.Sort((itemA, itemB) =>
        {
            if (itemA.IsVolid() && !itemB.IsVolid())
                return -1;
            else if (!itemA.IsVolid() && itemB.IsVolid())
                return 1;
            else
                return 0;
        });
    }

    #endregion

    #region element core

    public const int _ELEMENT_CORE_PACKET_CNT = 50;

    [SaveField(2)]
    public List<ItemFiveElementCore> _UsingCores;

    public ItemPackBase<ItemFiveElementCore> _PackCores;

    private bool InitPackCore()
    {
        _PackCores = new ItemPackBase<ItemFiveElementCore>();
        _PackCores._SaveFileName = "PackFiveElementCores";
        _PackCores._PackSize = _ITEM_PACKET_CNT;
        _PackCores.LoadClass(true);

        if (_PackCores._PackItems == null)
        {
            _PackCores.InitPack();
            _PackCores.SaveClass(true);
            return true;
        }

        return false;
    }

    private bool InitUsingCore()
    {
        if (_UsingCores == null || _UsingCores.Count == 0)
        {
            _UsingCores = new List<ItemFiveElementCore>();
            _UsingCores.Add(new ItemFiveElementCore());
            _UsingCores.Add(new ItemFiveElementCore());
            _UsingCores.Add(new ItemFiveElementCore());
            _UsingCores.Add(new ItemFiveElementCore());
            _UsingCores.Add(new ItemFiveElementCore());
            return true;
        }

        return false;
    }

    public void CreateCoreItem(string coreID, int level)
    {
        var elementCore = GameDataValue.GetRandomFiveElementCore(coreID, level);
        _PackCores.AddItem(elementCore);
    }

    public void AddCoreItem(ItemFiveElementCore itemCore)
    {
        _PackCores.AddItem(itemCore);
    }

    #endregion

    #region value attr

    private int _ElementsTotalValue = -1;
    public int ElementsTotalValue
    {
        get
        {
            CalculateValue();
            return _ElementsTotalValue;
        }
    }

    private List<EquipExAttr> _ValueAttrs = new List<EquipExAttr>();
    public List<EquipExAttr> ValueAttrs
    {
        get
        {
            return _ValueAttrs;
        }
    }

    private void CalculateValue()
    {
        _ElementsTotalValue = 0;
        for (int i = 0; i < _UsingElements.Count; ++i)
        {
            _ElementsTotalValue += _UsingElements[i].CombatValue;
        }
    }

    private void CalculateAttrs()
    {
        //_ValueAttrs = new List<EquipExAttr>();
        //CalculateValue();
        //if (_ElementsTotalValue == 0)
        //    return;

        //var valueAttrs = Tables.TableReader.FiveElementValueAttr.Records.Values;
        //foreach (var valueAttr in valueAttrs)
        //{
        //    string attr = Tables.StrDictionary.GetFormatStr(valueAttr.DescIdx);
        //    if (_ElementsTotalValue >= valueAttr.Value)
        //    {
        //        _ValueAttrs.Add(TableReader.AttrValue.GetExAttr(valueAttr.Attr, _ElementsTotalValue));
        //    }
        //}

        RoleData.SelectRole.CalculateAttr();
    }

    public void SetAttr(RoleAttrStruct roleAttr)
    {
        for (int j = 0; j < _UsingCores.Count; ++j)
        {
            for (int i = 0; i < _UsingCores[j].EquipExAttrs.Count; ++i)
            {
                if (_UsingCores[j].EquipExAttrs[i].AttrType == "RoleAttrImpactBaseAttr")
                {
                    roleAttr.AddValue((RoleAttrEnum)_UsingCores[j].EquipExAttrs[i].AttrParams[0], _UsingCores[j].EquipExAttrs[i].AttrParams[1]);
                }
                else
                {
                    roleAttr.AddExAttr(RoleAttrImpactManager.GetAttrImpact(_UsingCores[j].EquipExAttrs[i]));
                }
            }
        }

        for (int i = 0; i < _UsingElements.Count; ++i)
        {
            if (_UsingElements[i] == null)
                continue;

            if (!_UsingElements[i].IsVolid())
                continue;

            for (int j = 0; j < _UsingElements[i].ElementExAttrs.Count; ++j)
            {
                if (_UsingElements[i].ElementExAttrs[j].AttrType == "RoleAttrImpactBaseAttr")
                {
                    roleAttr.AddValue((RoleAttrEnum)_UsingElements[i].ElementExAttrs[j].AttrParams[0], _UsingElements[i].ElementExAttrs[j].AttrParams[1]);
                }
                else
                {
                    roleAttr.AddExAttr(RoleAttrImpactManager.GetAttrImpact(_UsingElements[i].ElementExAttrs[j]));
                }
            }
        }

        

    }

    #endregion

    #region opt

    public void PutOnElementCore(ItemFiveElementCore itemElement)
    {
        if (!itemElement.IsVolid())
            return;

        _UsingCores[(int)itemElement.FiveElementCoreRecord.ElementType].ExchangeInfo(itemElement);
        if (!itemElement.IsVolid())
        {
            _PackCores.RemoveItem(itemElement);
            _PackCores.SaveClass(false);
        }

        CalculateAttrs();
        //RoleData.SelectRole.CalculateAttr();
    }

    public void PutOffElementCore(ItemFiveElementCore itemElement)
    {
        ItemFiveElementCore emptySlot = (ItemFiveElementCore)_PackCores.GetEmptyPos();
        if (emptySlot == null)
        {
            UIMessageTip.ShowMessageTip(10002);
            return;
        }

        emptySlot.ExchangeInfo(itemElement);

        CalculateAttrs();
        //RoleData.SelectRole.CalculateAttr();
    }

    public bool SetExtractLock(int usingIdx, int idx, bool isLock)
    {
        ItemFiveElement usingElement = _UsingElements[usingIdx];
        if (usingElement == null || !usingElement.IsVolid())
            return false;

        return usingElement.SetAttrLock(idx, isLock);
    }

    public bool Extract( ItemFiveElement itemElement, int usingIdx)
    {
        ItemFiveElement usingElement = _UsingElements[usingIdx];
        if (usingElement == null || !usingElement.IsVolid())
            return false;

        //var costMoney = GameDataValue.GetElementExtraCostMoney(itemElement.Level);
        //if (!PlayerDataPack.Instance.DecGold(costMoney))
        //{
        //    return false;
        //}

        int sameIdx = -1;
        for (int i = 0; i < usingElement.EquipExAttrs.Count; ++i)
        {
            if (usingElement.EquipExAttrs[i].AttrParams[0] == itemElement.EquipExAttrs[0].AttrParams[0])
            {
                sameIdx = i;
            }
        }

        //replace same attr
        if (sameIdx >= 0)
        {
            if (!_PackElements.DecItem(itemElement, 1))
            {
                UIMessageTip.ShowMessageTip(1350006);
                return false;
            }

            usingElement.ReplaceAttr(sameIdx, itemElement.EquipExAttrs[0]);
        }
        else
        {
            //if (!IsExtraCostEnough(usingIdx))
            //    return false;

            //ExtraCostItems(usingIdx);

            if (!_PackElements.DecItem(itemElement, 1))
            {
                UIMessageTip.ShowMessageTip(1350006);
                return false;
            }

            int goldCost = GameDataValue.GetElementExtraCostMoney(usingElement);
            if (!PlayerDataPack.Instance.DecGold(goldCost))
                return false;

            float extraRate = GetAddExAttrRate(usingElement.EquipExAttrs.Count);
            float extraRandom = UnityEngine.Random.Range(0, 1.0f);
            int refreshIdx = usingElement.EquipExAttrs.Count;
            if (extraRandom < extraRate)
            {
                usingElement.AddExAttr(itemElement.EquipExAttrs[0]);
            }
            else
            {
                List<int> randomIdxs = new List<int>();
                List<int> randomRates = new List<int>();
                for (int i = 0; i < usingElement.EquipExAttrs.Count; ++i)
                {
                    if (!usingElement.IsAttrLock(i))
                    {
                        randomIdxs.Add(i);
                        randomRates.Add(100);
                    }
                }
                int replaceIdx = GameRandom.GetRandomLevel(randomRates);
                //if (sameIdx >= 0 && replaceIdx >= sameIdx)
                //{
                //    ++replaceIdx;
                //}
                refreshIdx = randomIdxs[replaceIdx];
                usingElement.ReplaceAttr(refreshIdx, itemElement.EquipExAttrs[0]);
            }
            //usingElement.RefreshExAttr(refreshIdx);
        }

        //itemElement.ResetItem();
        

        CalculateAttrs();

        SortPack();
        SaveClass(true);
        return true;
    }

    public bool IsExtraCostEnough(int usingIdx, bool withTips = true)
    {
        ItemFiveElement usingElement = _UsingElements[usingIdx];
        int costCnt = GameDataValue.GetLockExtraCostCnt(usingElement);
        for (int i = 0; i < usingElement.EquipExAttrs.Count; ++i)
        {
            if (usingElement.IsAttrLock(i))
            {
                int attrCnt = _PackElements.GetAttrItemCnt(usingElement.ElementExAttrs[i].AttrParams[0]);
                if (attrCnt < costCnt)
                {
                    if (withTips)
                    {
                        var itemRecord = TableReader.FiveElement.GetFiveElementByAttr(usingElement.ElementExAttrs[i].AttrParams[0]);
                        if (itemRecord != null)
                        {
                            var commonItem = TableReader.CommonItem.GetRecord(itemRecord.Id);
                            UIMessageTip.ShowMessageTip(StrDictionary.GetFormatStr(1300003, StrDictionary.GetFormatStr(commonItem.NameStrDict)));
                        }
                    }
                    return false;
                }
            }
        }

        return true;
    }

    public bool ExtraCostItems(int usingIdx)
    {
        ItemFiveElement usingElement = _UsingElements[usingIdx];
        int costCnt = GameDataValue.GetLockExtraCostCnt(usingElement);

        for (int i = 0; i < usingElement.EquipExAttrs.Count; ++i)
        {
            if (usingElement.IsAttrLock(i))
            {
                bool decItems = _PackElements.DecAttrItem(usingElement.ElementExAttrs[i].AttrParams[0], costCnt);
                if (!decItems)
                    return false;
            }
        }

        return true;
    }
    

    public float GetAddExAttrRate(int idx)
    {
        float extraRate = 0;
        if (idx == 0)
        {
            extraRate = 1;
        }
        else if (idx == 1)
        {
            extraRate = 0.5f;
        }
        else if (idx == 2)
        {
            extraRate = 0.3f;
        }
        else if (idx == 3)
        {
            extraRate = 0.15f;
        }
        else if (idx == 4)
        {
            extraRate = 0.05f;
        }
        else if (idx == 5)
        {
            extraRate = 0.03f;
        }

        return extraRate;
    }

    public float GetAttrRate(ItemFiveElement usingItem, int idx)
    {
        int usingIdx = _UsingElements.IndexOf(usingItem);
        if (usingIdx < 0)
            return 1;
        float curValue = GameDataValue._FiveElementValueRate[idx];
        if (_UsingCores[usingIdx] != null && _UsingCores[usingIdx].IsVolid())
        {
            for (int i = 0; i < _UsingCores[usingIdx].EquipExAttrs.Count; ++i)
            {
                if (_UsingCores[usingIdx].ConditionState(i) > 0)
                {
                    if (_UsingCores[usingIdx].EquipExAttrs[i].AttrType == "RoleAttrImpactEleCorePos")
                    {
                        var attrTab = TableReader.AttrValue.GetRecord(_UsingCores[usingIdx].EquipExAttrs[i].AttrParams[0].ToString());
                        var targetPos = RoleAttrImpactEleCorePos.GetPosFromTab(attrTab);
                        if (targetPos == idx)
                        {
                            curValue += RoleAttrImpactEleCorePos.GetValueFromTab(attrTab) * curValue;
                        }
                    }
                    else if (_UsingCores[usingIdx].EquipExAttrs[i].AttrType == "RoleAttrImpactEleCoreAttr")
                    {
                        var attrTab = TableReader.AttrValue.GetRecord(_UsingCores[usingIdx].EquipExAttrs[i].AttrParams[0].ToString());
                        var targetAttr = RoleAttrImpactEleCoreAttr.GetAttrFromTab(attrTab);
                        if (targetAttr == usingItem.EquipExAttrs[i].AttrParams[0])
                        {
                            curValue += curValue * RoleAttrImpactEleCoreAttr.GetValueFromTab(attrTab);
                        }
                    }
                }
            }
        }
        return curValue;
    }

    public string GetAttrAddRateStr(ItemFiveElement usingItem, int idx)
    {
        float curValue = GetAttrRate(usingItem, idx);
        //float curValue = GameDataValue._FiveElementValueRate[idx];
        string addPersent = GameDataValue.ConfigFloatToPersent(curValue - 1) + "%";
        return addPersent;
    }

    public int GetElementSellMoney(ItemFiveElement eleItem)
    {
        var singleMoney = GameDataValue.GetElementSellGold(eleItem.Level);
        return singleMoney * eleItem.ItemStackNum;
    }

    public int GetCoreSellMoney(ItemFiveElementCore eleCore)
    {
        return GameDataValue.GetCoreSellGold(eleCore);
    }

    public void SellElement(ItemFiveElement eleItem)
    {
        int sellMoney = GetElementSellMoney(eleItem);
        _PackElements.RemoveItem(eleItem);
        _PackElements.SaveClass(true);
        PlayerDataPack.Instance.AddGold(sellMoney);
    }

    public void SellElements(List<ItemBase> eleItems)
    {
        int sellMoney = 0;
        for (int i = 0; i < eleItems.Count; ++i)
        {
            ItemFiveElement itemElement = eleItems[i] as ItemFiveElement;
            if(itemElement != null)
            {
                sellMoney += GetElementSellMoney(itemElement);
                _PackElements.RemoveItem(itemElement);
            }
        }
        _PackElements.SaveClass(true);
        PlayerDataPack.Instance.AddGold(sellMoney);
    }

    public void SellCore(ItemFiveElementCore eleCore)
    {
        int sellMoney = GetCoreSellMoney(eleCore);
        _PackCores.RemoveItem(eleCore);
        _PackCores.SaveClass(true);
        PlayerDataPack.Instance.AddGold(sellMoney);
    }

    public void SellCores(List<ItemBase> eleCores)
    {
        int sellMoney = 0;
        for (int i = 0; i < eleCores.Count; ++i)
        {
            ItemFiveElementCore itemElement = eleCores[i] as ItemFiveElementCore;
            if (itemElement != null)
            {
                sellMoney += GetCoreSellMoney(itemElement);
                _PackCores.RemoveItem(itemElement);
            }
        }
        _PackCores.SaveClass(true);
        PlayerDataPack.Instance.AddGold(sellMoney);
    }

    #endregion

    #region static create

    public static ItemFiveElement CreateElementItem(int level)
    {
        var elementItem = GameDataValue.GetRandomFiveElement(level);
        FiveElementData.Instance.AddElementItem(elementItem);
        return elementItem;
    }

    #endregion

    

}
