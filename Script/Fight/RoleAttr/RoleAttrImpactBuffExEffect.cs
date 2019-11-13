using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactBuffExEffect : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        var skillRecord = TableReader.SkillInfo.GetRecord(args[0].ToString());
        _ImpactName = skillRecord.SkillAttr.StrParam[0];
    }

    public override List<int> GetSkillImpactVal(ItemSkill skillInfo)
    {
        var valList = new List<int>();
        valList.Add(int.Parse(skillInfo.SkillID));

        return valList;
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        ResourcePool.Instance.LoadConfig("SkillMotion\\CommonImpact\\" + _ImpactName, (resName, resGO, hash) =>
        {
            var impactGO = resGO;
            impactGO.transform.SetParent(skillMotion.transform);
        }, null);

    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var skillRecord = Tables.TableReader.SkillInfo.GetRecord(attrDescID.ToString());
        var strFormat = StrDictionary.GetFormatStr(skillRecord.DescStrDict, GameDataValue.ConfigIntToPersent(skillRecord.EffectValue[0]), GameDataValue.ConfigIntToPersent(skillRecord.EffectValue[1]));
        return strFormat;
    }
    #region 

    public string _ImpactName;

    #endregion
}
