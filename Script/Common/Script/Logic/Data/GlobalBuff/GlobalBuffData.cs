using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tables;

public class GlobalBuffData : SaveItemBase
{
    #region 唯一

    private static GlobalBuffData _Instance = null;
    public static GlobalBuffData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GlobalBuffData();
            }
            return _Instance;
        }
    }

    private GlobalBuffData()
    {
        _SaveFileName = "GlobalBuffData";
    }

    #endregion

    public void InitGlobalBuffData()
    {
        bool needSave = false;

        needSave |= RefreshTelantBuffs();
        needSave |= RefreshAttrBuffs();

        InitAttrBuff();

        if (needSave)
        {
            SaveClass(true);
        }
    }

    #region telant buff

    public const int _RefreshBuffSecond = 1800;
    public const int _ShowingBuffCnt = 3;

    public class GlobalBuffItem
    {
        [SaveField(1)]
        public string _RecordID = "";
        [SaveField(2)]
        public string _LastActTime = "";
        [SaveField(3)]
        public string _LastRefreshTime = "";
        [SaveField(4)]
        public bool _ActByDiamond = false;

        private GlobalBuffRecord _BuffReford;
        public GlobalBuffRecord BuffReford
        {
            get
            {
                if (_BuffReford == null)
                {
                    _BuffReford = TableReader.GlobalBuff.GetRecord(_RecordID);
                }
                return _BuffReford;
            }
        }
        
    }

    [SaveField(1)]
    public List<GlobalBuffItem> _BuffTelantItems = new List<GlobalBuffItem>();

    public void RefreshTelant()
    {
        bool isNeedSave = RefreshTelantBuffs();
        if (isNeedSave)
        {
            SaveClass(true);
        }
    }

    private bool RefreshTelantBuffs()
    {
        if (_BuffTelantItems == null || _BuffTelantItems.Count < _ShowingBuffCnt)
        {
            _BuffTelantItems.Clear();
            FillTelantBuffItems();
            return true;
        }
        else
        {
            List<GlobalBuffItem> needRemoveItems = new List<GlobalBuffItem>();
            foreach (var buffItem in _BuffTelantItems)
            {
                if (IsBuffActed(buffItem) && !IsBuffActing(buffItem))
                {
                    needRemoveItems.Add(buffItem);
                }
                else
                {
                    DateTime refreshTime = DateTime.Parse(buffItem._LastRefreshTime);
                    var deltaTime = (DateTime.Now - refreshTime).TotalSeconds;
                    if (deltaTime > _RefreshBuffSecond)
                    {
                        needRemoveItems.Add(buffItem);
                    }
                }
            }
            foreach (var removeBuff in needRemoveItems)
            {
                _BuffTelantItems.Remove(removeBuff);
            }

            if (_BuffTelantItems.Count < _ShowingBuffCnt)
            {
                FillTelantBuffItems();
                return true;
            }
        }
        return false;
    }

    private void FillTelantBuffItems()
    {
        List<GlobalBuffRecord> buffRecords = new List<GlobalBuffRecord>();
        foreach (var globalBuff in TableReader.GlobalBuff.Records.Values)
        {
            if (globalBuff.Type == GLOABL_BUFF_TYPE.Talent)
            {
                if (globalBuff.TelentID.Profession > 0 &&
                ((globalBuff.TelentID.Profession >> (int)RoleData.SelectRole.Profession) & 1) == 0)
                    continue;

                var skillInfo = SkillData.Instance.GetSkillInfo(globalBuff.TelentID.Id);
                if (skillInfo.SkillLevel == 0)
                {
                    buffRecords.Add(globalBuff);
                }
            }
            else if (globalBuff.Type == GLOABL_BUFF_TYPE.ExTalent)
            {
                buffRecords.Add(globalBuff);
            }
        }

        foreach (var showingBuff in _BuffTelantItems)
        {
            var buffRecord = TableReader.GlobalBuff.GetRecord(showingBuff._RecordID);
            buffRecords.Remove(buffRecord);
        }

        int needCreateCnt = _ShowingBuffCnt - _BuffTelantItems.Count;
        var randomList = GameRandom.GetIndependentRandoms(0, buffRecords.Count, needCreateCnt);
        foreach (var randomID in randomList)
        {
            GlobalBuffItem buffItem = new GlobalBuffItem();
            buffItem._RecordID = buffRecords[randomID].Id;
            buffItem._LastActTime = "";
            buffItem._LastRefreshTime = DateTime.Now.ToString();
            _BuffTelantItems.Add(buffItem);
        }
    }

    #endregion

    #region attr buff

    [SaveField(2)]
    public List<GlobalBuffItem> _BuffAttrItems = new List<GlobalBuffItem>();

    public void RefreshAttr()
    {
        bool isNeedSave = RefreshAttrBuffs();
        if (isNeedSave)
        {
            SaveClass(true);
        }
    }

    private bool RefreshAttrBuffs()
    {
        if (_BuffAttrItems == null || _BuffAttrItems.Count < _ShowingBuffCnt)
        {
            _BuffAttrItems.Clear();
            FillAttrBuffItems();
            return true;
        }
        else
        {
            List<GlobalBuffItem> needRemoveItems = new List<GlobalBuffItem>();
            foreach (var buffItem in _BuffAttrItems)
            {
                if (IsBuffActed(buffItem) && !IsBuffActing(buffItem))
                {
                    needRemoveItems.Add(buffItem);
                }
                else
                {
                    DateTime refreshTime = DateTime.Parse(buffItem._LastRefreshTime);
                    var deltaTime = (DateTime.Now - refreshTime).TotalSeconds;
                    if (deltaTime > _RefreshBuffSecond)
                    {
                        needRemoveItems.Add(buffItem);
                    }
                }
            }
            foreach (var removeBuff in needRemoveItems)
            {
                _BuffAttrItems.Remove(removeBuff);
            }

            if (_BuffAttrItems.Count < _ShowingBuffCnt)
            {
                FillAttrBuffItems();
                return true;
            }
        }
        return false;
    }

    private void FillAttrBuffItems()
    {
        List<GlobalBuffRecord> buffRecords = new List<GlobalBuffRecord>();
        foreach (var globalBuff in TableReader.GlobalBuff.Records.Values)
        {
            if (globalBuff.Type == GLOABL_BUFF_TYPE.ExAttr)
            {
                buffRecords.Add(globalBuff);
            }
        }

        foreach (var showingBuff in _BuffAttrItems)
        {
            var buffRecord = TableReader.GlobalBuff.GetRecord(showingBuff._RecordID);
            buffRecords.Remove(buffRecord);
        }

        int needCreateCnt = _ShowingBuffCnt - _BuffAttrItems.Count;
        var randomList = GameRandom.GetIndependentRandoms(0, buffRecords.Count, needCreateCnt);
        foreach (var randomID in randomList)
        {
            GlobalBuffItem buffItem = new GlobalBuffItem();
            buffItem._RecordID = buffRecords[randomID].Id;
            buffItem._LastActTime = "";
            buffItem._LastRefreshTime = DateTime.Now.ToString();
            _BuffAttrItems.Add(buffItem);
        }
    }

    #endregion

    #region act buff

    private GlobalBuffItem _ActBuffItem;
    public List<EquipExAttr> _ExAttrs = new List<EquipExAttr>();

    public void ActByAd(GlobalBuffItem buffItem)
    {
        _ActBuffItem = buffItem;
        AdManager.Instance.WatchAdVideo(ActBuff);
    }

    public void ActByDiamond(GlobalBuffItem buffItem)
    {
        var buffRecrod = TableReader.GlobalBuff.GetRecord(buffItem._RecordID);
        if (!PlayerDataPack.Instance.DecDiamond(buffRecrod.DiamondCost))
        {
            return;
        }

        _ActBuffItem = buffItem;
        _ActBuffItem._ActByDiamond = true;
        ActBuff();
    }

    public void ActBuff()
    {
        if (_ActBuffItem != null)
        {
            _ActBuffItem._LastActTime = DateTime.Now.ToString();
            if (_ActBuffItem.BuffReford.Type == GLOABL_BUFF_TYPE.ExAttr)
            {
                if (_ActBuffItem._ActByDiamond)
                {
                    _ExAttrs.Add(_ActBuffItem.BuffReford.ExAttrDiamond.GetExAttr(1));
                }
                else
                {
                    _ExAttrs.Add(_ActBuffItem.BuffReford.ExAttr.GetExAttr(1));
                }

                RoleData.SelectRole.CalculateAttr();
            }
            

            _ActBuffItem = null;
            SaveClass(true);

            UIGlobalBuff.RefreshBuffs();

        } 
    }

    public void ActBuff(string buffID, int second = -1)
    {

    }

    public void ActBuffInFight()
    {
        _ExAttrs.Clear();
        foreach (var showingBuff in _BuffTelantItems)
        {
            if (IsBuffActing(showingBuff))
            {
                if (showingBuff.BuffReford.Type == GLOABL_BUFF_TYPE.Talent)
                {
                    var telentInfo = SkillData.Instance.GetSkillInfo(showingBuff.BuffReford.TelentID.Id);
                    telentInfo.AddExLevel();
                }
                else if (showingBuff.BuffReford.Type == GLOABL_BUFF_TYPE.ExTalent)
                {
                    _ExAttrs.Add(showingBuff.BuffReford.ExAttr.GetExAttr(1));
                }
            }
        }

        foreach (var showingBuff in _BuffAttrItems)
        {
            if (IsBuffActing(showingBuff))
            {
                if (showingBuff.BuffReford.Type == GLOABL_BUFF_TYPE.ExAttr)
                {
                    if (showingBuff._ActByDiamond)
                    {
                        _ExAttrs.Add(showingBuff.BuffReford.ExAttrDiamond.GetExAttr(1));
                    }
                    else
                    {
                        _ExAttrs.Add(showingBuff.BuffReford.ExAttr.GetExAttr(1));
                    }
                }
            }
        }

        RoleData.SelectRole.CalculateAttr();
    }

    public void InitAttrBuff()
    {
        _ExAttrs.Clear();
        foreach (var showingBuff in _BuffAttrItems)
        {
            if (IsBuffActing(showingBuff))
            {
                if (showingBuff.BuffReford.Type == GLOABL_BUFF_TYPE.ExAttr)
                {
                    if (showingBuff._ActByDiamond)
                    {
                        _ExAttrs.Add(showingBuff.BuffReford.ExAttrDiamond.GetExAttr(1));
                    }
                    else
                    {
                        _ExAttrs.Add(showingBuff.BuffReford.ExAttr.GetExAttr(1));
                    }
                }
            }
        }

        RoleData.SelectRole.CalculateAttr();
    }

    public void SetAttr(RoleAttrStruct roleAttr)
    {
        for (int i = 0; i < _ExAttrs.Count; ++i)
        {
            if (_ExAttrs[i].AttrType == "RoleAttrImpactBaseAttr")
            {
                roleAttr.AddValue((RoleAttrEnum)_ExAttrs[i].AttrParams[0], _ExAttrs[i].AttrParams[1]);
            }
            else
            {
                roleAttr.AddExAttr(RoleAttrImpactManager.GetAttrImpact(_ExAttrs[i]));
            }
        }
    }

    public void DeactBuffOutFight()
    {
        foreach (var showingBuff in _BuffTelantItems)
        {
            if (IsBuffActing(showingBuff))
            {
                if (showingBuff.BuffReford.Type == GLOABL_BUFF_TYPE.Talent)
                {
                    var telentInfo = SkillData.Instance.GetSkillInfo(showingBuff.BuffReford.TelentID.Id);
                    telentInfo.DecExLevel();
                }
            }
        }
    }

    public void Dispare(GlobalBuffItem buffItem)
    {
        if (_BuffTelantItems.Contains(buffItem))
        {
            _BuffTelantItems.Remove(buffItem);
            FillTelantBuffItems();
        }
        else if (_BuffAttrItems.Contains(buffItem))
        {
            _BuffAttrItems.Remove(buffItem);
            FillAttrBuffItems();
        }

        UIGlobalBuff.RefreshBuffs();
    }

    public bool IsBuffActed(GlobalBuffItem buffItem)
    {
        if (string.IsNullOrEmpty(buffItem._LastActTime))
            return false;

        return true;
    }

    public bool IsBuffActing(GlobalBuffItem buffItem)
    {
        if (string.IsNullOrEmpty(buffItem._LastActTime))
            return false;

        var buffRecrod = TableReader.GlobalBuff.GetRecord(buffItem._RecordID);
        DateTime actTime = DateTime.Parse(buffItem._LastActTime);
        var deltaTime = (DateTime.Now - actTime).TotalSeconds;
        if (deltaTime > buffRecrod.LastTime)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    #endregion

}
