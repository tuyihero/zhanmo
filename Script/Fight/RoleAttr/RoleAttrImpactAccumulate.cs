using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactAccumulate : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _AccumulateTime = GameDataValue.ConfigIntToFloat(args[0]);
        _DamageEnhance = GameDataValue.ConfigIntToFloat(args[1]);
        _ImpactName = "Accumulate";
    }

    public override List<int> GetSkillImpactVal(ItemSkill skillInfo)
    {
        var valList = new List<int>();
        valList.Add(skillInfo.SkillRecord.EffectValue[1] - (skillInfo.SkillActureLevel - 1) * skillInfo.SkillRecord.EffectValue[2]);
        valList.Add((skillInfo.SkillActureLevel) * skillInfo.SkillRecord.EffectValue[0]);

        return valList;
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        if (!_SkillInput.Equals("-1"))
        {
            if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
                return;

            var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
            ResourcePool.Instance.LoadConfig("SkillMotion\\CommonImpact\\" + _ImpactName, (resName, resGO, hash) =>
            {
                resGO.transform.SetParent(skillMotion.transform);
                resGO.transform.localPosition = Vector3.zero;
                var bulletEmitterEle = resGO.GetComponent<ImpactAccumulate>();
                bulletEmitterEle._AccumulateTime = _AccumulateTime;
                bulletEmitterEle._AccumulateDamage = _DamageEnhance;
            }, null);

            ResourcePool.Instance.LoadConfig("SkillMotion\\CommonImpact\\" + _ImpactName + "Hit", (resName, resGO, hash) =>
            {
                resGO.transform.SetParent(skillMotion.transform);
                resGO.transform.localPosition = Vector3.zero;
            }, null);

        }
        else
        {
            foreach (var skillMotion in roleMotion._StateSkill._SkillMotions.Values)
            {
                if (!skillMotion._ActInput.Equals("1")
                    && !skillMotion._ActInput.Equals("2")
                    && !skillMotion._ActInput.Equals("3"))
                    continue;

                ResourcePool.Instance.LoadConfig("SkillMotion\\CommonImpact\\" + _ImpactName, (resName, resGO, hash) =>
                {
                    resGO.transform.SetParent(skillMotion.transform);
                    resGO.transform.localPosition = Vector3.zero;
                    var bulletEmitterEle = resGO.GetComponent<ImpactAccumulate>();
                    bulletEmitterEle._AccumulateTime = _AccumulateTime;
                    bulletEmitterEle._AccumulateDamage = _DamageEnhance;
                }, null);

                ResourcePool.Instance.LoadConfig("SkillMotion\\CommonImpact\\" + _ImpactName + "Hit", (resName, resGO, hash) =>
                {
                    resGO.transform.SetParent(skillMotion.transform);
                    resGO.transform.localPosition = Vector3.zero;
                }, null);
            }
        }
    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var skillRecord = Tables.TableReader.SkillInfo.GetRecord(attrDescID.ToString());
        int skillLevel = Mathf.Max(1, attrParams[1]);
        var damageModify = (skillLevel) * skillRecord.EffectValue[0];
        var accumulateTime = skillRecord.EffectValue[1] - (skillLevel - 1) * skillRecord.EffectValue[2];
        var strFormat = StrDictionary.GetFormatStr(skillRecord.DescStrDict, GameDataValue.ConfigIntToPersent(damageModify), GameDataValue.ConfigIntToFloat(accumulateTime));
        return strFormat;
    }


    #region 

    public float _AccumulateTime;
    public float _DamageEnhance;
    public string _ImpactName;
    
    #endregion
}
