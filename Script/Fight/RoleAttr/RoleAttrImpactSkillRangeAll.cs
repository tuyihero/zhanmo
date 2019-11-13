using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactSkillRangeAll : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _RangeSModify = GameDataValue.ConfigIntToFloat(args[1]);
    }

    public override List<int> GetSkillImpactVal(ItemSkill skillInfo)
    {
        var valList = new List<int>();
        valList.Add(skillInfo.SkillActureLevel * skillInfo.SkillRecord.EffectValue[0]);

        return valList;
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        foreach (var skillMotion in roleMotion._StateSkill._SkillMotions.Values)
        {
            skillMotion.ModifyColliderRange(_RangeSModify);
            skillMotion.SetEffectSize(1 + _RangeSModify);
        }
    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var attrTab = Tables.TableReader.AttrValue.GetRecord(attrDescID.ToString());
        var value1 = GameDataValue.ConfigIntToFloat(attrTab.AttrParams[1]);
        var strFormat = StrDictionary.GetFormatStr(160000, GameDataValue.ConfigFloatToPersent(value1));
        return strFormat;
    }

    #region 

    public float _RangeSModify;
    
    #endregion
}
