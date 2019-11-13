using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactDodgeNoDamage : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _ImpactName = "DodgeDexImpact";
        _BuffLastTime = GameDataValue.ConfigIntToFloat(args[0]);
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

            var impactGO = resGO;
            impactGO.transform.SetParent(skillMotion.transform);
            impactGO.transform.localPosition = Vector3.zero;
            impactGO.transform.localRotation = Quaternion.Euler(0, 0, 0);

            var buff = impactGO.GetComponentInChildren<ImpactBuff>();
            buff._LastTime = _BuffLastTime;
        }, null);

    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var skillRecord = Tables.TableReader.SkillInfo.GetRecord(attrDescID.ToString());
        var strFormat = StrDictionary.GetFormatStr(skillRecord.DescStrDict, GameDataValue.ConfigIntToFloat(skillRecord.EffectValue[0]));
        return strFormat;
    }

    #region 

    public string _ImpactName;
    public float _BuffLastTime;
    
    #endregion
}
