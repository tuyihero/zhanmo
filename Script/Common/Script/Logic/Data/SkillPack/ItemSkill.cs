using UnityEngine;
using System.Collections;



public class ItemSkill : ItemBase
{
    public string SkillID
    {
        get
        {
            return ItemDataID;
        }
        set
        {
            ItemDataID = value;
        }
    }

    public int SkillLevel
    {
        get
        {
            return ItemStackNum;
        }
        private set
        {
            ItemStackNum = value;
        }
    }

    public int _SkillExLevel = 0;

    public int SkillActureLevel
    {
        get
        {
            return _SkillExLevel + SkillLevel;
        }
    }

    private Tables.SkillInfoRecord _SkillRecord;
    public Tables.SkillInfoRecord SkillRecord
    {
        get
        {
            if (_SkillRecord == null)
            {
                _SkillRecord = Tables.TableReader.SkillInfo.GetRecord(SkillID);
            }
            return _SkillRecord;
        }
    }

    public int LevelUp()
    {
        SkillLevel += 1;
        SaveClass(true);
        return SkillLevel;
    }

    public ItemSkill()
    {
        SkillID = "-1";
        SkillLevel = 0;
    }

    public ItemSkill(string id, int level = 1)
    {
        SkillID = id;
        SkillLevel = level;
    }

    public void AddExLevel(int level = 1)
    {
        _SkillExLevel += level;
        _SkillExLevel = Mathf.Clamp(_SkillExLevel, 0, 9999);
    }

    public void DecExLevel(int level = 1)
    {
        _SkillExLevel -= level;
        _SkillExLevel = Mathf.Clamp(_SkillExLevel, 0, 9999);
    }
}

