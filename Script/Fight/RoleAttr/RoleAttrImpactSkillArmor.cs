using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactSkillArmor : RoleAttrImpactPassive
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _ImpactName = "SkillSuperArmor";

    }

    public override List<int> GetSkillImpactVal(ItemSkill skillInfo)
    {
        var valList = new List<int>();
        valList.Add(skillInfo.SkillActureLevel * skillInfo.SkillRecord.EffectValue[0]);

        return valList;
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        ResourcePool.Instance.LoadConfig("SkillMotion\\CommonImpact\\" + _ImpactName, (resName, resGO, hash) =>
        {
            var impactGO = resGO;
            impactGO.transform.SetParent(roleMotion.BuffBindPos.transform);
            var buffs = impactGO.GetComponents<ImpactBuff>();
            foreach (var buff in buffs)
            {
                buff.ActImpact(roleMotion, roleMotion);
            }
        }, null);


        

    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var skillRecord = Tables.TableReader.SkillInfo.GetRecord(attrDescID.ToString());
        var strFormat = StrDictionary.GetFormatStr(skillRecord.DescStrDict);
        return strFormat;
    }
    #region 

    public string _ImpactName;

    #endregion
}
