using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactDefenceRate : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _CD = GameDataValue.ConfigIntToFloat(args[0]);
        _DefenceRate = GameDataValue.ConfigIntToFloat(args[1]);
    }

    public override List<int> GetSkillImpactVal(ItemSkill skillInfo)
    {
        var valList = new List<int>();
        valList.Add(skillInfo.SkillRecord.EffectValue[0]);
        valList.Add(skillInfo.SkillRecord.EffectValue[1]);

        return valList;
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        skillMotion._SkillCD = _CD;
        var defenceBuff = (skillMotion as ObjMotionSkillDefence)._BuffDefence;
        defenceBuff._DefenceRate = _DefenceRate;
    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var skillRecord = Tables.TableReader.SkillInfo.GetRecord(attrDescID.ToString());
        var strFormat = StrDictionary.GetFormatStr(skillRecord.DescStrDict, GameDataValue.ConfigIntToFloat(skillRecord.EffectValue[0]), GameDataValue.ConfigIntToPersent(skillRecord.EffectValue[1]));
        return strFormat;
    }
    #region 

    public float _CD;
    public float _DefenceRate;

    #endregion
}
