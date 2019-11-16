using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
using System;

public class SkillData : SaveItemBase
{
    #region 单例

    private static SkillData _Instance;
    public static SkillData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new SkillData();
            }
            return _Instance;
        }
    }

    private SkillData()
    {
        _SaveFileName = "SkillData";
    }

    #endregion

    public void InitSkills()
    {
        InitSkill();
    }

    #region skill

    [SaveField(1)]
    private List<ItemSkill> _SkillItems = new List<ItemSkill>();

    private List<ItemSkill> _ProfessionSkills;
    public List<ItemSkill> ProfessionSkills
    {
        get
        {
            if (_ProfessionSkills == null)
            {
                InitSkill();
            }
            return _ProfessionSkills;
        }
    }

    public string GetSkillIcon(string input)
    {
        foreach (var itemSkill in ProfessionSkills)
        {
            if (itemSkill.SkillRecord.SkillInput.Equals(input))
            {
                return itemSkill.SkillRecord.Icon;
            }
        }

        return "";
    }

    private bool InitSkill()
    {
        if (Tables.TableReader.SkillInfo == null)
            return false;

        if (RoleData.SelectRole == null)
            return false;

        Debug.Log("_SkillItems:" + _SkillItems.Count);
        bool isNeedSave = false;
        _ProfessionSkills = new List<ItemSkill>();
        foreach (var skillPair in Tables.TableReader.SkillInfo.Records)
        {
            if (skillPair.Value.Profession > 0 &&
            ((skillPair.Value.Profession >> (int)RoleData.SelectRole.Profession) & 1) != 0)
            {
                var skillInfo = GetSkillInfo(skillPair.Value.Id, ref isNeedSave);
                _ProfessionSkills.Add(skillInfo);

                if (skillInfo.SkillRecord.SkillAttr.AttrImpact == "RoleAttrImpactSkillDamage"
                    || skillInfo.SkillRecord.SkillAttr.AttrImpact == "RoleAttrImpactBuffRate"
                    || skillInfo.SkillRecord.SkillAttr.AttrImpact == "RoleAttrImpactBuffActSkill" /*默认学会*/)
                {
                    if (skillInfo.SkillLevel == 0)
                    {
                        //SkillLevelUp(skillInfo.SkillID);
                        skillInfo.SetStackNum(1);
                        isNeedSave = true;
                    }
                }


            }
        }

        if(isNeedSave)
        {
            SaveClass(true);
        }

        return isNeedSave;
    }

    public List<string> GetRoleSkills()
    {
        List<string> skillMotions = new List<string>() { "Attack","Skill1", "Skill2", "Skill3", "Skill4", "Skill5", "Skill6", "JumpAtk" };
        return skillMotions;
        //if (RoleData.SelectRole.Profession == PROFESSION.BOY_DEFENCE || RoleData.SelectRole.Profession == PROFESSION.GIRL_DEFENCE)
        //{
        //    skillMotions.Add("Defence");
        //}
        //else
        {
            skillMotions.Add("Roll");
        }

        bool spSkill1 = false;
        bool spSkill2 = false;
        bool spSkill3 = false;
        foreach (var skillInfo in _SkillItems)
        {
            if (skillInfo.SkillRecord.Profession > 0 &&
            ((skillInfo.SkillRecord.Profession >> (int)RoleData.SelectRole.Profession) & 1) == 0)
                continue;

            if (skillInfo.SkillActureLevel == 0)
                continue;

            if (skillInfo.SkillRecord.SkillInput == "5")
            {
                //if ((RoleData.SelectRole.Profession == PROFESSION.BOY_DEFENCE || RoleData.SelectRole.Profession == PROFESSION.GIRL_DOUGE))
                //{
                //    if (!skillMotions.Contains("Buff1"))
                //    {
                //        skillMotions.Add("Buff1");
                //    }
                //}
                //else
                {
                    if (!skillMotions.Contains("Buff1_2"))
                    {
                        skillMotions.Add("Buff1_2");
                    }
                }
            }
            

            if (skillInfo.SkillRecord.SkillInput == "6")
            {
                //if ((RoleData.SelectRole.Profession == PROFESSION.BOY_DEFENCE || RoleData.SelectRole.Profession == PROFESSION.GIRL_DOUGE))
                //{
                //    if (!skillMotions.Contains("Buff2"))
                //    {
                //        skillMotions.Add("Buff2");
                //    }
                //}
                //else
                {
                    if (!skillMotions.Contains("Buff2_2"))
                    {
                        skillMotions.Add("Buff2_2");
                    }
                }
            }
            

            if (skillInfo.SkillRecord.SkillAttr.AttrImpact == "RoleAttrImpactSP")
            {
                if (skillInfo.SkillRecord.SkillInput == "1")
                {
                    spSkill1 = true;
                }
                if (skillInfo.SkillRecord.SkillInput == "2")
                {
                    spSkill2 = true;
                }
                if (skillInfo.SkillRecord.SkillInput == "3")
                {
                    spSkill3 = true;
                }
            }

            if (skillInfo.SkillRecord.SkillAttr.AttrImpact == "RoleAttrImpactBuffInHit")
            {
                skillMotions.Add("BuffInHit");
            }
        }

        //if (RoleData.SelectRole.Profession == PROFESSION.GIRL_DOUGE || RoleData.SelectRole.Profession == PROFESSION.BOY_DEFENCE)
        //{
        //    if (spSkill1)
        //    {
        //        skillMotions.Add("Skill1_1_2");
        //    }
        //    else
        //    {
        //        skillMotions.Add("Skill1");
        //    }

        //    if (spSkill2)
        //    {
        //        skillMotions.Add("Skill2_1_2");
        //    }
        //    else
        //    {
        //        skillMotions.Add("Skill2");
        //    }

        //    if (spSkill3)
        //    {
        //        skillMotions.Add("Skill3_1_2");
        //    }
        //    else
        //    {
        //        skillMotions.Add("Skill3");
        //    }
        //}
        //else
        {
            if (spSkill1)
            {
                skillMotions.Add("Skill1_2_2");
            }
            else
            {
                skillMotions.Add("Skill1_2");
            }

            if (spSkill2)
            {
                skillMotions.Add("Skill2_2_2");
            }
            else
            {
                skillMotions.Add("Skill2_2");
            }

            if (spSkill3)
            {
                skillMotions.Add("Skill3_2_2");
            }
            else
            {
                skillMotions.Add("Skill3_2");
            }
        }


        return skillMotions;
    }

    public ItemSkill GetSkillInfo(string skillID)
    {
        bool needSave = false;
        return GetSkillInfo(skillID, ref needSave);
    }

    public ItemSkill GetSkillInfo(string skillID, ref bool isNeedSave)
    {
        var skillItem = _SkillItems.Find((skillInfo) =>
        {
            if (skillInfo.SkillID == skillID)
            {
                return true;
            }
            return false;
        });

        if (skillItem == null)
        {
            skillItem = new ItemSkill(skillID, 0);
            _SkillItems.Add(skillItem);
            isNeedSave |= true;
        }

        return skillItem;
    }

    public ItemSkill GetSkillByInput(string skillInput)
    {
        var skillItem = ProfessionSkills.Find((skillInfo) =>
        {
            if (skillInfo.SkillRecord.SkillInput == skillInput)
            {
                return true;
            }
            return false;
        });
        return skillItem;
    }

    public bool IsCanSkillLvUP(ItemSkill skillItem, bool isTip = true)
    {
        var skillTab = skillItem.SkillRecord;
        if (skillTab.MaxLevel <= skillItem.SkillLevel)
        {
            if (isTip)
            {
                UIMessageTip.ShowMessageTip(62004);
            }
            return false;
        }

        //cost
        int nextLv = skillTab.StartRoleLevel + skillItem.SkillLevel * skillTab.NextLvInterval;
        if (RoleData.SelectRole.RoleLevel < nextLv)
        {
            if (isTip)
            {
                UIMessageTip.ShowMessageTip(62002);
            }
            return false;
        }

        if (skillTab.StartPreSkill > 0)
        {
            var preSkillTab = TableReader.SkillInfo.GetRecord(skillTab.StartPreSkill.ToString());
            var skillPreItem = SkillData.Instance.GetSkillInfo(preSkillTab.Id);
            if (skillPreItem.SkillActureLevel < skillTab.StartPreSkillLv)
            {
                if (isTip)
                {
                    UIMessageTip.ShowMessageTip(62003);
                }
                return false;
            }

        }

        if (skillTab.CostStep[0] == (int)MONEYTYPE.GOLD)
        {
            int costValue = GameDataValue.GetSkillLvUpGold(skillTab, skillItem.SkillLevel);
            if (PlayerDataPack.Instance.Gold < (costValue))
                return false;
        }
        else
        {
            int skillItemCnt = BackBagPack.Instance.PageItems.GetItemCnt(GameDataValue._SkillItemID);
            if (skillItemCnt > 0)
            {
                if (BackBagPack.Instance.PageItems.GetItemCnt(GameDataValue._SkillItemID) < 1)
                    return false;
            }
            else
            {
                int costValue = skillTab.CostStep[1];
                if (PlayerDataPack.Instance.Diamond < costValue)
                    return false;
            }
        }

        if (skillTab.MaxLevel <= skillItem.SkillLevel)
        {
            return false;
        }

        return true;
    }

    public bool IsCanAnySkillLvUp()
    {
        foreach (var professionSkill in ProfessionSkills)
        {
            if (professionSkill.SkillRecord.SkillType.Equals("61000"))
            {
                if (IsCanSkillLvUP(professionSkill, false))
                    return true;
            }
        }
        return false;
    }

    public void SkillLevelUp(string skillID)
    {
        var findSkill = _SkillItems.Find((skillInfo) =>
        {
            if (skillInfo.SkillID == skillID)
            {
                return true;
            }
            return false;
        });

        var skillTab = Tables.TableReader.SkillInfo.GetRecord(skillID);
        if (skillTab.MaxLevel <= findSkill.SkillLevel)
        {
            UIMessageTip.ShowMessageTip(62004);
            return;
        }

        //cost
        int nextLv = skillTab.StartRoleLevel + findSkill.SkillLevel * skillTab.NextLvInterval;
        if (RoleData.SelectRole.RoleLevel < nextLv)
        {
            UIMessageTip.ShowMessageTip(62002);
            return;
        }

        if (skillTab.StartPreSkill > 0)
        {
            var preSkillTab = TableReader.SkillInfo.GetRecord(skillTab.StartPreSkill.ToString());
            var skillItem = SkillData.Instance.GetSkillInfo(preSkillTab.Id);
            if (skillItem.SkillActureLevel < skillTab.StartPreSkillLv)
            {
                UIMessageTip.ShowMessageTip(62003);
                return;
            }

        }

        if (skillTab.CostStep[0] == (int)MONEYTYPE.GOLD)
        {
            int costValue = GameDataValue.GetSkillLvUpGold(skillTab, findSkill.SkillLevel);
            if (!PlayerDataPack.Instance.DecGold(costValue))
                return;
        }
        else
        {
            int skillItemCnt = BackBagPack.Instance.PageItems.GetItemCnt(GameDataValue._SkillItemID);
            if (skillItemCnt > 0)
            {
                if (!BackBagPack.Instance.PageItems.DecItem(GameDataValue._SkillItemID, 1))
                    return;
            }
            else
            {
                int costValue = skillTab.CostStep[1];
                if (!PlayerDataPack.Instance.DecDiamond(costValue))
                    return;
            }
        }

        if (findSkill == null)
        {
            findSkill = new ItemSkill(skillID);
            _SkillItems.Add(findSkill);

            SaveClass(false);
        }

        if (skillTab.MaxLevel > findSkill.SkillLevel)
        {
            findSkill.LevelUp();
        }

        RoleData.SelectRole.CalculateAttr();

        Hashtable eventHash = new Hashtable();
        eventHash.Add("SkillID", findSkill.SkillID);
        eventHash.Add("SkillLevel", findSkill.SkillLevel);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_LEVELUP_SKILL, this, eventHash);
    }

    public void SetSkillAttr(RoleAttrStruct _BaseAttr)
    {
        foreach (var skillItem in _SkillItems)
        {
            if (skillItem.SkillActureLevel == 0)
                continue;

            if (skillItem.SkillRecord.Profession > 0 &&
            ((skillItem.SkillRecord.Profession >> (int)RoleData.SelectRole.Profession) & 1) != 0)
            {
                var attrImpact = RoleAttrImpactManager.GetAttrImpact(skillItem);
                if (attrImpact != null)
                {
                    _BaseAttr.AddExAttr(attrImpact);
                }
            }
        }
    }

    #endregion
}



