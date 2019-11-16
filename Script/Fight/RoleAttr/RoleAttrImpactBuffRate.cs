using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactBuffRate : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _ValueModify = GameDataValue.ConfigIntToFloat(args[0]);
    }

    public override List<int> GetSkillImpactVal(ItemSkill skillInfo)
    {
        var valList = new List<int>();

        var buffValue = skillInfo.SkillRecord.EffectValue[0] + (skillInfo.SkillActureLevel - 1) * skillInfo.SkillRecord.EffectValue[1];
        valList.Add(buffValue);

        return valList;
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        var impactBuff = skillMotion.GetComponentInChildren<ImpactBuffAttrAdd>(true);
        //for (int i = 0; i < impactBuffs.Length; ++i)
        if(impactBuff != null)
        {
            impactBuff._AddValue = _ValueModify;
        }
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

    public float _ValueModify;
    
    #endregion
}
