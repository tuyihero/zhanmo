using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactDefenceArmor : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _ImpactName = "DefenceSuperArmor";

    }

    public override List<int> GetSkillImpactVal(ItemSkill skillInfo)
    {
        var valList = new List<int>();
        valList.Add(skillInfo.SkillActureLevel * skillInfo.SkillRecord.EffectValue[0]);

        return valList;
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];

        ResourcePool.Instance.LoadConfig("SkillMotion\\CommonImpact\\" + _ImpactName, (resName, resGO, hash) =>
        {
            var defenceImpact = skillMotion.GetComponentInChildren<ImpactBuffDefence>();
            resGO.transform.SetParent(defenceImpact.transform);
            resGO.transform.localPosition = Vector3.zero;
            resGO.transform.localRotation = Quaternion.Euler(0, 0, 0);
            defenceImpact._SubImpactGO = resGO;
        }, null);

    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var skillRecord = Tables.TableReader.SkillInfo.GetRecord(attrDescID.ToString());
        var strFormat = StrDictionary.GetFormatStr(skillRecord.DescStrDict, (skillRecord.EffectValue[0]), GameDataValue.ConfigIntToPersent(skillRecord.EffectValue[1]));
        return strFormat;
    }
    #region 

    public string _ImpactName;

    #endregion
}
