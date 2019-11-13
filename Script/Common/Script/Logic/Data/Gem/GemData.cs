using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tables;

public class GemLevelInfo
{
    public string MaterialData;
    public int MaterialCnt;
    public int CostMoney;
}

public class GemData : DataPackBase
{
    #region 唯一

    private static GemData _Instance = null;
    public static GemData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GemData();
            }
            return _Instance;
        }
    }

    private GemData()
    {
        _SaveFileName = "GemData";
    }

    #endregion

    public void InitGemData()
    {
        InitGemContainer();
        InitGemPack();
        
    }

    #region gem pack

    public const int MAX_GEM_EQUIP = 5;

    [SaveField(1)]
    public List<string> _EquipGemDataID;

    private List<ItemGem> _EquipedGemDatas;
    public List<ItemGem> EquipedGemDatas
    {
        get
        {
            return _EquipedGemDatas;
        }
    }

    public bool IsEquipedGem(ItemGem itemGem)
    {
        return _EquipedGemDatas.Contains(itemGem);
    }

    public void SaveEquipGem()
    {
        SaveClass(true);
    }

    public void RefreshEquipedGems()
    {
        _EquipedGemDatas = new List<ItemGem>();

        bool needSave = false;
        for (int i = 0; i < _EquipGemDataID.Count; ++i)
        {
            ItemGem packGem = _PackGemDatas._PackItems.Find((itemgem) =>
            {
                if (itemgem._SaveFileName == _EquipGemDataID[i])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });

            if (packGem == null)
            {
                packGem = _PackExtraGemDatas._PackItems.Find((itemgem) =>
                {
                    if (itemgem._SaveFileName == _EquipGemDataID[i])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
            if (packGem == null && _EquipGemDataID[i] != "-1")
            {
                _EquipGemDataID[i] = "-1";
                needSave = true;
            }
            _EquipedGemDatas.Add(packGem);
        }

        if (needSave)
        {
            SaveClass(true);
        }
        GemSuit.Instance.IsActSet();
    }

    public void RefreshEquipedIDs()
    {
        for (int i = 0; i < _EquipedGemDatas.Count; ++i)
        {
            if (_EquipedGemDatas[i] == null || !_EquipedGemDatas[i].IsVolid())
            {
                _EquipGemDataID[i] = _EquipedGemDatas[i]._SaveFileName;
            }
        }

        SaveClass(true);
        GemSuit.Instance.IsActSet();
    }

    public bool InitGemPack()
    {
        if (_EquipGemDataID == null || _EquipGemDataID.Count == 0)
        {
            _EquipGemDataID = new List<string>();
            for (int i = 0; i < MAX_GEM_EQUIP; ++i)
            {
                _EquipGemDataID.Add("-1");
            }
        }

        RefreshEquipedGems();
        return false;
    }

    public int GetPutOnIdx()
    {
        for (int i = 0; i < EquipedGemDatas.Count; ++i)
        {
            if (EquipedGemDatas[i] == null || !EquipedGemDatas[i].IsVolid())
                return i;
        }
        return -1;
    }

    public bool PutOnGem(ItemGem gem, int slot)
    {
        if (slot< 0 || slot >= MAX_GEM_EQUIP)
        {
            UIMessageTip.ShowMessageTip("gem slot error");
            return false;
        }

        if (!IsCanPutOnGem(gem))
        {
            UIMessageTip.ShowMessageTip("allready put on gem");
            return false;
        }

        _EquipGemDataID[slot] = gem._SaveFileName;

        RefreshEquipedGems();

        SaveClass(true);
        PackGemDatas.SaveClass(true);

        GemSuit.Instance.IsActSet();

        RoleData.SelectRole.CalculateAttr();

        Hashtable hash = new Hashtable();
        hash.Add("ItemGem", gem);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_PUT_ON, this, hash);

        return true;
    }

    public bool IsCanPutOnGem(ItemGem gem)
    {
        if (gem == null || !gem.IsVolid())
            return false;

        var equipedGem = EquipedGemDatas.Find((gemItem) =>
        {
            if (gemItem == gem)
            {
                return true;
            }
            else
            {
                return false;
            }
        });
        if (equipedGem != null)
        {
            
            return false;
        }

        return true;
    }

    public bool PutOff(ItemGem gem)
    {

        int equipSlot = -1;
        for (int i = 0; i < _EquipedGemDatas.Count; ++i)
        {
            if (_EquipedGemDatas[i] == gem)
            {
                equipSlot = i;
                break;
            }
        }
        if (equipSlot < 0)
            return false;

        _EquipGemDataID[equipSlot] = "-1";
        RefreshEquipedGems();

        PackGemDatas.SaveClass(true);
        SaveClass(true);

        RoleData.SelectRole.CalculateAttr();

        Hashtable hash = new Hashtable();
        hash.Add("ItemGem", gem);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_PUT_OFF, this, hash);

        return true;
    }

    public void ExchangeGem(ItemGem gem1, ItemGem gem2)
    {
        gem1.ExchangeInfo(gem2);
        GemSuit.Instance.IsActSet();

        RoleData.SelectRole.CalculateAttr();
    }

    public bool IsEquipedGem(string gemDataID)
    {
        for (int i = 0; i < _EquipGemDataID.Count; ++i)
        {
            if (_EquipGemDataID[i] == gemDataID)
            {
                return true;
            }
        }
        return false;
    }

    #endregion

    #region gem container

    public const int MAX_GEM_PACK = 10;

    private ItemPackBase<ItemGem> _PackGemDatas;
    public ItemPackBase<ItemGem> PackGemDatas
    {
        get
        {
            return _PackGemDatas;
        }
    }

    private ItemPackBase<ItemGem> _PackExtraGemDatas;
    public ItemPackBase<ItemGem> PackExtraGemDatas
    {
        get
        {
            return _PackExtraGemDatas;
        }
    }

    private void InitGemContainer()
    {
        _PackGemDatas = new ItemPackBase<ItemGem>();
        _PackGemDatas._SaveFileName = "GemDataPack";
        _PackGemDatas._PackSize = MAX_GEM_PACK;
        _PackGemDatas.LoadClass(true);

        if (_PackGemDatas._PackItems == null || _PackGemDatas._PackItems.Count == 0)
        {
            _PackGemDatas._PackItems = new List<ItemGem>();

            //var gemRecordTabs = TableReader.GemTable.Records.Values;
            //foreach (var gemRecord in gemRecordTabs)
            //{
            //    ItemGem newItemGem = new ItemGem(gemRecord.Id);
            //    newItemGem.Level = 0;
            //    _PackGemDatas._PackItems.Add(newItemGem);
            //}

            _PackGemDatas.SaveClass(true);
        }

        _PackExtraGemDatas = new ItemPackBase<ItemGem>();
        _PackExtraGemDatas._SaveFileName = "ExtraGemDataPack";
        _PackExtraGemDatas._PackSize = MAX_GEM_PACK;
        _PackExtraGemDatas.LoadClass(true);

        if (_PackExtraGemDatas._PackItems == null || _PackExtraGemDatas._PackItems.Count == 0)
        {
            _PackExtraGemDatas._PackItems = new List<ItemGem>();

            _PackExtraGemDatas.SaveClass(true);
        }
    }

    public void CreateGem(string gemDataID, int gemCnt)
    {
        PackGemDatas.AddItem(gemDataID, gemCnt);
        PackGemDatas.SaveClass(true);

        Hashtable hash = new Hashtable();
        hash.Add("AddGemData", gemDataID);
        hash.Add("AddGemNum", gemCnt);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_GET, this, hash);
    }

    public void CreateExtraGem(ItemGem itemGem)
    {
        PackExtraGemDatas.AddItem(itemGem);
        PackExtraGemDatas.SaveClass(true);

        Hashtable hash = new Hashtable();
        hash.Add("AddGemItem", itemGem);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_GET, this, hash);
    }

    public void PacketSort()
    {
        PackGemDatas.SortStack();
        PackGemDatas.SortEmpty();
        PackGemDatas.SaveClass(true);
    }

    public void DecGem(ItemGem itemGem, int num = 1)
    {

    }

    #endregion

    #region combine

    private Dictionary<GemTableRecord, List<int>> _GemFormulas;
    public Dictionary<GemTableRecord, List<int>> GemFormulas
    {
        get
        {
            InitGemFormulas();
            return _GemFormulas;
        }
    }

    private void InitGemFormulas()
    {
        if (_GemFormulas != null)
            return;

        _GemFormulas = new Dictionary<GemTableRecord, List<int>>();
        foreach (var gemRecord in TableReader.GemTable.Records)
        {
            if (gemRecord.Value.Combine[0] < 0)
                continue;

            List<int> gemIds = new List<int>();
            List<int> combines = new List<int>(gemRecord.Value.Combine);
            combines.Sort((idA, idB) =>
            {
                if (idA < idB)
                {
                    return 1;
                }
                else if (idA > idB)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            });

            for (int i = 0; i < combines.Count; ++i)
            {
                if (combines[i] > 0)
                {
                    gemIds.Add(combines[i]);
                }
            }

            _GemFormulas.Add(gemRecord.Value, gemIds);
        }
    }

    public bool GemCombine(List<ItemGem> combineGem)
    {
        if (combineGem.Count == 3 && combineGem[0].ItemDataID == combineGem[1].ItemDataID
            && combineGem[0].ItemDataID == combineGem[2].ItemDataID)
        {
            return GemCombineSame(combineGem);
        }
        else
        {
            return GemCombineFormula(combineGem);
        }
    }

    private static List<int> _CombineSameNeedNum = new List<int>() { 3, 9, 27, 81 };
    public void GemCombineSameAll(ItemGem gemDataID)
    {
        int gemMatCnt = PackGemDatas.GetItemCnt(gemDataID.ItemDataID);
        int combineLevel = GemCombineSameLevel(gemMatCnt);
        while (combineLevel > 0)
        {
            var nextGem = GetNextLevelGem(gemDataID.GemRecord.Class, combineLevel);
            if (nextGem == null)
            {
                UIMessageTip.ShowMessageTip(30006);
                return;
            }
            PackGemDatas.DecItem(gemDataID, _CombineSameNeedNum[combineLevel - 1]);
            CreateGem(nextGem.Id, 1);

            gemMatCnt = PackGemDatas.GetItemCnt(gemDataID.ItemDataID);
            combineLevel = GemCombineSameLevel(gemMatCnt);
        }
    }

    private int GemCombineSameLevel(int gemMatCnt)
    {
        for (int i = _CombineSameNeedNum.Count - 1; i >= 0; --i)
        {
            if (gemMatCnt >= _CombineSameNeedNum[i])
                return i + 1;
        }

        return -1;
    }

    private bool GemCombineSame(List<ItemGem> combineGems)
    {
        var nextGem = GetNextLevelGem(combineGems[0].GemRecord.Class, combineGems[0].GemRecord.Level);
        if (nextGem == null)
        {
            UIMessageTip.ShowMessageTip(30006);
            return false;
        }

        if (combineGems[0] == combineGems[1]
            && combineGems[0] == combineGems[2])
        {
            if (combineGems[0].ItemStackNum < 3)
                return false;

            PackGemDatas.DecItem(combineGems[0], 3);
        }
        else if (combineGems[0] == combineGems[1])
        {
            if (combineGems[0].ItemStackNum < 2 || combineGems[2].ItemStackNum < 1)
                return false;

            PackGemDatas.DecItem(combineGems[0],2);
            PackGemDatas.DecItem(combineGems[2],1);
        }
        else if (combineGems[0] == combineGems[2])
        {
            if (combineGems[0].ItemStackNum < 2 || combineGems[1].ItemStackNum < 1)
                return false;

            PackGemDatas.DecItem(combineGems[0],2);
            PackGemDatas.DecItem(combineGems[1],1);
        }
        else if (combineGems[1] == combineGems[2])
        {
            if (combineGems[1].ItemStackNum < 2 || combineGems[2].ItemStackNum < 1)
                return false;

            PackGemDatas.DecItem(combineGems[1],2);
            PackGemDatas.DecItem(combineGems[0],1);
        }
        else
        {
            if (combineGems[0].ItemStackNum < 1 || combineGems[1].ItemStackNum < 1 || combineGems[2].ItemStackNum < 1)
                return false;

            PackGemDatas.DecItem(combineGems[0],1);
            PackGemDatas.DecItem(combineGems[1], 1);
            PackGemDatas.DecItem(combineGems[2], 1);
        }
        CreateGem(nextGem.Id, 1);
        return true;
    }

    private bool GemCombineFormula(List<ItemGem> combineGems)
    {
        InitGemFormulas();

        combineGems.Sort((gemA, gemB) =>
        {
            int idA = int.Parse(gemA.ItemDataID);
            int idB = int.Parse(gemB.ItemDataID);
            if (idA < idB)
            {
                return 1;
            }
            else if (idA > idB)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        });

        foreach (var gemFormula in _GemFormulas)
        {
            if (combineGems.Count != gemFormula.Value.Count)
                continue;

            bool fitFormula = true;
            for (int i = 0; i < gemFormula.Value.Count; ++i)
            {
                if (gemFormula.Value[i] != combineGems[i].GemRecord.Class)
                {
                    fitFormula = false;
                    break;
                }
            }

            if (fitFormula)
            {
                for (int i = 0; i < combineGems.Count; ++i)
                {
                    PackGemDatas.DecItem(combineGems[i], 1);
                }
                CreateGem(gemFormula.Key.Id, 1);
                return true;
            }
        }

        return false;
    }

    #endregion

    #region combine v2

    public bool CombineV2(List<ItemGem> combines)
    {
        if (combines.Count < 3)
            return false;

        ItemGem targetGem = combines[0];
        if (!targetGem.IsGemExtra())
        {
            
            targetGem = new ItemGem(combines[0].ItemDataID);
            targetGem.Level = 1;
            targetGem.SetStackNum(1, false);
            PackExtraGemDatas.AddItem(targetGem);
            PackExtraGemDatas.SaveClass(true);

            PackGemDatas.DecItem(combines[0], 1);
            PackGemDatas.SaveClass(true);
        }

        if (targetGem.GemAttr[0].AttrParams[0] == combines[1].GemAttr[0].AttrParams[0]
            && targetGem.GemAttr[0].AttrParams[0] == combines[2].GemAttr[0].AttrParams[0])
        {
            int mat0 = Mathf.Max((targetGem.Level - 1) * 2 + 1, 1);
            int mat1 = Mathf.Max((combines[1].Level - 1) * 2 + 1, 1);
            int mat2 = Mathf.Max((combines[2].Level - 1) * 2 + 1, 1);
            int level = (mat0 + mat1 + mat2 - 1) / 2 + 1;
            targetGem.Level = level;
        }
        else if (targetGem.GemAttr.Count > 1
            && targetGem.GemAttr[1].AttrParams[0] == combines[1].GemAttr[0].AttrParams[0]
            && targetGem.GemAttr[1].AttrParams[0] == combines[2].GemAttr[0].AttrParams[0])
        {
            int mat0 = Mathf.Max((targetGem.ExAttrLevel) * 2, 1);
            int mat1 = Mathf.Max((combines[1].Level - 1) * 2 + 1, 1);
            int mat2 = Mathf.Max((combines[2].Level - 1) * 2 + 1, 1);
            int level = (mat0 + mat1 + mat2 - 1) / 2 + 1;
            targetGem.ExAttrLevel = level;
        }
        else
        {
            ItemGem extraMatGem = combines[1];
            if (combines[2].Level > extraMatGem.Level)
            {
                extraMatGem = combines[2];
            }
            else if (combines[1].GemAttr[0].AttrParams[0] != combines[2].GemAttr[0].AttrParams[0])
            {
                int randomMat = UnityEngine.Random.Range(0, 2);
                if (randomMat > 0)
                {
                    extraMatGem = combines[2];
                }
            }

            targetGem.ExAttr = extraMatGem.GemAttr[0].AttrParams[0];
            targetGem.ExAttrLevel = extraMatGem.Level;
        }
        //scene 1, mat any extra
        //if (combines[1].IsGemExtra() || combines[2].IsGemExtra())
        //{
        //    int mat0 = (targetGem.Level - 1) * 2 + 1;
        //    int mat1 = (combines[1].Level - 1) * 2 + 1;
        //    int mat2 = (combines[2].Level - 1) * 2 + 1;
        //    int level = (mat0 + mat1 + mat2 - 1) / 2 + 1;
        //    targetGem.Level = level;

        //    int exMat0 = (targetGem.ExAttrLevel - 1) * 2;
        //    exMat0 = Mathf.Max(exMat0, 0);
        //    int exMat1 = (combines[1].ExAttrLevel - 1) * 2;
        //    exMat1 = Mathf.Max(exMat1, 0);
        //    int exMat2 = (combines[2].ExAttrLevel - 1) * 2;
        //    exMat2 = Mathf.Max(exMat2, 0);
        //    int exLevel = (exMat0 + exMat1 + exMat2) / 2;

        //    List<int> extraAttrs = new List<int>();
        //    if (exMat0 > 0)
        //    {
        //        extraAttrs.Add(targetGem.ExAttr);
        //    }
        //    if (exMat1 > 0)
        //    {
        //        extraAttrs.Add(combines[1].ExAttr);
        //    }
        //    if (exMat2 > 0)
        //    {
        //        extraAttrs.Add(combines[2].ExAttr);
        //    }

        //    if (extraAttrs.Count > 0)
        //    {
        //        int idx = UnityEngine.Random.Range(0, extraAttrs.Count);
        //        targetGem.ExAttr = extraAttrs[idx];
        //        targetGem.ExAttrLevel = exLevel;
        //    }
        //}
        //else 
        //{
        //    ItemGem extraMatGem = combines[1];
        //    if (combines[1].GemAttr[0].AttrParams[0] != combines[2].GemAttr[0].AttrParams[0])
        //    {
        //        int randomMat = UnityEngine.Random.Range(0, 2);
        //        if (randomMat > 0)
        //        {
        //            extraMatGem = combines[2];
        //        }
        //    }

        //    if (extraMatGem.GemAttr[0].AttrParams[0] == targetGem.GemAttr[0].AttrParams[0])
        //    {
        //        int mat0 = (targetGem.Level - 1) * 2 + 1;
        //        int mat1 = (combines[1].Level - 1) * 2 + 1;
        //        int mat2 = (combines[2].Level - 1) * 2 + 1;
        //        int level = (mat0 + mat1 + mat2 - 1) / 2 + 1;
        //        targetGem.Level = level;
        //    }
        //    else if(targetGem.IsGemExtra())
        //    {
        //        int exMat0 = (targetGem.ExAttrLevel) * 2;
        //        exMat0 = Mathf.Max(exMat0, 0);
        //        int exMat1 = (combines[1].Level - 1) * 2 + 1;
        //        exMat1 = Mathf.Max(exMat1, 0);
        //        int exMat2 = (combines[2].Level - 1) * 2 + 1;
        //        exMat2 = Mathf.Max(exMat2, 0);
        //        int exLevel = (exMat0 + exMat1 + exMat2) / 2;

        //        targetGem.ExAttr = extraMatGem.GemAttr[0].AttrParams[0];
        //        targetGem.ExAttrLevel = exLevel;
        //    }
        //}

        bool needSave = false;
        if (GemData.Instance.PackGemDatas._PackItems.Contains(combines[1]))
        {
            GemData.Instance.PackGemDatas.DecItem(combines[1], 1);
            GemData.Instance.PackGemDatas.SaveClass(true);

            if (combines[1].ItemStackNum == 0)
            {
                int idx = EquipedGemDatas.IndexOf(combines[1]);
                if (idx >= 0)
                {
                    _EquipGemDataID[idx] = "-1";
                    needSave = true;
                }
            }
        }
        else if (GemData.Instance.PackExtraGemDatas._PackItems.Contains(combines[1]))
        {
            GemData.Instance.PackExtraGemDatas.RemoveItem(combines[1]);
            GemData.Instance.PackExtraGemDatas.SaveClass(true);

            int idx = EquipedGemDatas.IndexOf(combines[1]);
            if (idx >= 0)
            {
                _EquipGemDataID[idx] = "-1";
                needSave = true;
            }
        }


        if (GemData.Instance.PackGemDatas._PackItems.Contains(combines[2]))
        {
            GemData.Instance.PackGemDatas.DecItem(combines[2], 1);
            GemData.Instance.PackGemDatas.SaveClass(true);

            if (combines[2].ItemStackNum == 0)
            {
                int idx = EquipedGemDatas.IndexOf(combines[2]);
                if (idx >= 0)
                {
                    _EquipGemDataID[idx] = "-1";
                    needSave = true;
                }
            }
        }
        else if (GemData.Instance.PackExtraGemDatas._PackItems.Contains(combines[2]))
        {
            GemData.Instance.PackExtraGemDatas.RemoveItem(combines[2]);
            GemData.Instance.PackExtraGemDatas.SaveClass(true);

            int idx = EquipedGemDatas.IndexOf(combines[2]);
            if (idx >= 0)
            {
                _EquipGemDataID[idx] = "-1";
                needSave = true;
            }
        }


        if (needSave)
        {
            RefreshEquipedGems();
            SaveClass(true);
        }

        targetGem.RefreshGemAttr();
        targetGem.SaveClass(true);

        Hashtable hash = new Hashtable();
        hash.Add("ItemGem", targetGem);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_COMBINE, this, hash);

        GemSuit.Instance.IsActSet();

        return true;
    }

    #endregion

    #region attr

    public void SetGemAttr(RoleAttrStruct roleAttr)
    {
        for (int i = 0; i < EquipedGemDatas.Count; ++i)
        {
            if (EquipedGemDatas[i] == null)
                continue;

            if (!EquipedGemDatas[i].IsVolid())
                continue;

            foreach (var gemAttr in EquipedGemDatas[i].GemAttr)
            {
                if (gemAttr.AttrType == "RoleAttrImpactBaseAttr")
                {
                    roleAttr.AddValue((RoleAttrEnum)gemAttr.AttrParams[0], gemAttr.AttrParams[1]);
                }
                else
                {
                    roleAttr.AddExAttr(RoleAttrImpactManager.GetAttrImpact(gemAttr));
                }
            }
        }

        GemSuit.Instance.SetGemSetAttr(roleAttr);
       
    }

    public ItemGem GetGemClassMax(int classID, int minLevel, List<ItemGem> extraGems)
    {
        ItemGem classItem = null;
        foreach (var gemData in _PackExtraGemDatas._PackItems)
        {
            if (extraGems !=null && extraGems.Contains(gemData))
                continue;

            if (classID > 0)
            {
                if (gemData.IsVolid() && gemData.GemRecord.Class == classID && gemData.Level >= minLevel)
                {
                    if (classItem == null)
                    {
                        classItem = gemData;
                    }
                    else if (classItem.GemRecord.Level < gemData.GemRecord.Level)
                    {
                        classItem = gemData;
                    }
                }
            }
            else
            {
                if (gemData.IsVolid() && gemData.Level >= minLevel)
                {
                    classItem = gemData;
                }
            }
        }

        return classItem;
    }

    public GemTableRecord GetGemByClass(int classType, int level)
    {
        return TableReader.GemTable.GetGemRecordByClass(classType, level);
    }

    public GemTableRecord GetNextLevelGem(int gemClass, int curLevel)
    {
        int nextLv = curLevel + 1;
        foreach (var gemRecord in TableReader.GemTable.Records)
        {
            if (gemRecord.Value.Class == gemClass && gemRecord.Value.Level == nextLv)
            {
                return gemRecord.Value;
            }
        }
        return null;
    }

    public List<GemTableRecord> GetAllLevelGemRecords(int gemClass)
    {
        List<GemTableRecord> gemRecords = new List<GemTableRecord>();
        foreach (var gemData in PackGemDatas._PackItems)
        {
            if (gemData.IsVolid() && gemData.GemRecord.Class == gemClass)
            {
                if (!gemRecords.Contains(gemData.GemRecord))
                {
                    gemRecords.Add(gemData.GemRecord);
                }
            }
        }

        return gemRecords;
    }


    #endregion

    #region level up

    public int GetLevelCostMoney(ItemGem itemGem)
    {
        return GameDataValue.GetGemLevelUpCostMoney(itemGem.Level);
    }

    public int GetLevelCostMat(ItemGem itemGem)
    {
        return GameDataValue.GetGemLevelUpCostMat(itemGem.Level);
    }

    public bool GemLevelUp(ItemGem itemGem)
    {
        if (itemGem.Level > RoleData.SelectRole.TotalLevel)
        {
            UIMessageTip.ShowMessageTip(30009);
            return false;
        }

        int costMatCnt = GetLevelCostMat(itemGem);
        
        if (itemGem.ItemStackNum >= costMatCnt)
        {
            int stackNum = itemGem.ItemStackNum - costMatCnt;
            itemGem.SetStackNum(stackNum);
            ++itemGem.Level;
            itemGem.RefreshGemAttr();
            itemGem.SaveClass(true);
            return true;
        }

        UIMessageTip.ShowMessageTip(30003);
        return false;
    }

    public void AutoLevelAll()
    {
        foreach (var itemGem in PackGemDatas._PackItems)
        {
            bool isCanLvUp = true;
            while (isCanLvUp)
            {
                isCanLvUp = GemLevelUp(itemGem);
            }
        }
    }

    #endregion

    #region red tips

    public bool IsGemSlotCanEquip(int slotIdx)
    {
        if (EquipedGemDatas[slotIdx] == null || !EquipedGemDatas[slotIdx].IsVolid())
        {
            foreach (var itemGem in PackExtraGemDatas._PackItems)
            {
                if (IsCanPutOnGem(itemGem))
                    return true;
            }

            foreach (var itemGem in PackGemDatas._PackItems)
            {
                if (IsCanPutOnGem(itemGem))
                    return true;
            }
        }

        return false;
    }

    public bool IsGemCanLvUp(ItemGem targetGem)
    {
        if (!targetGem.IsVolid())
            return false;

        if (!targetGem.IsGemExtra())
        {
            foreach (var itemGem in PackExtraGemDatas._PackItems)
            {
                if (!itemGem.IsVolid())
                    continue;
                if (targetGem.ItemDataID == itemGem.ItemDataID)
                {
                    return false;
                }
                else if (targetGem.GemAttr[0].AttrParams[0] == itemGem.ExAttr)
                {
                    return false;
                }
            }

            if (targetGem.ItemStackNum > 2)
                return true;
            return false;
        }
        else
        {
            int matItemCnt = PackGemDatas.GetItemCnt(targetGem.ItemDataID);
            if (matItemCnt > 1)
                return true;

            foreach (var itemGem in PackGemDatas._PackItems)
            {
                if (!itemGem.IsVolid())
                    continue;
                if (itemGem.GemAttr[0].AttrParams[0] == targetGem.ExAttr)
                {
                    if (itemGem.ItemStackNum > 1)
                        return true;
                }
            }

            return false;
        }
    }

    public bool IsAnyGemGanEquip()
    {
        for (int i = 0; i < MAX_GEM_EQUIP; ++i)
        {
            if (IsGemSlotCanEquip(i))
                return true;
        }

        return false;
    }

    public bool IsAnyGemGanLvUp()
    {
        foreach (var itemGem in PackExtraGemDatas._PackItems)
        {
            if (IsGemCanLvUp(itemGem))
                return true;
        }

        foreach (var itemGem in PackGemDatas._PackItems)
        {
            if (IsGemCanLvUp(itemGem))
                return true;
        }

        return false;
    }

    public bool IsGemMatOf(ItemGem targetGem, ItemGem matGem)
    {
        if (matGem.IsGemExtra())
        {
            return false;
        }
        else if (matGem.ItemDataID == targetGem.ItemDataID)
        {
            if (matGem.ItemStackNum > 1)
                return true;
        }
        else if (targetGem.ExAttr > 0 && matGem.GemAttr[0].AttrParams[0] == targetGem.ExAttr)
        {
            if (matGem.ItemStackNum > 1)
                return true;
        }
        else if (targetGem.ExAttr <= 0 && !IsCombineByExtra(matGem))
        {
            if (matGem.ItemStackNum > 1)
                return true;
        }

        return false;
    }

    public bool IsGemMatOf(ItemGem targetGem, ItemGem matGemA, ItemGem curGem)
    {
        if (curGem.IsGemExtra())
        {
            return false;
        }
        else if (matGemA == null || !matGemA.IsVolid())
        {
            return IsGemMatOf(targetGem, curGem);
        }
        else if (curGem == matGemA)
        {
            return true;
        }

        return false;
    }

    private bool IsCombineByExtra(ItemGem curGem)
    {
        foreach (var itemGem in PackExtraGemDatas._PackItems)
        {
            if (!itemGem.IsVolid())
                continue;

            if (itemGem.ItemDataID == curGem.ItemDataID)
            {
                return true;
            }
            else if (curGem.GemAttr[0].AttrParams[0] == itemGem.ExAttr)
            {
                return true;
            }
        }
        return false;
    }

    #endregion
}
