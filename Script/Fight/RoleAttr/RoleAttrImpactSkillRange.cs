using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactSkillRange : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _RangeSModify = GameDataValue.ConfigIntToFloat(args[0]);
    }

    public override List<int> GetSkillImpactVal(ItemSkill skillInfo)
    {
        var valList = new List<int>();
        valList.Add((skillInfo.SkillActureLevel - 1) * skillInfo.SkillRecord.EffectValue[0]);

        return valList;
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        skillMotion.ModifyColliderRange(_RangeSModify);
        skillMotion.SetEffectSize(1 + _RangeSModify);
    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var skillRecord = Tables.TableReader.SkillInfo.GetRecord(attrDescID.ToString());
        int skillLevel = Mathf.Max(1, attrParams[1]);
        var damageModify = (skillLevel - 1) * skillRecord.EffectValue[1] + skillRecord.EffectValue[0];
        var strFormat = StrDictionary.GetFormatStr(skillRecord.DescStrDict, GameDataValue.ConfigIntToPersent(damageModify));
        return strFormat;
    }

    #region 

    public float _RangeSModify;
    
    #endregion
}
