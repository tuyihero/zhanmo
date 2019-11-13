using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tables;
public class RoleAttrImpactExAttack : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
        _AttackTimes = (int)args[0];
        _Damage = GameDataValue.ConfigIntToFloat(args[1]);
        _ImpactName = "ExAttack";
    }

    public override List<int> GetSkillImpactVal(ItemSkill skillInfo)
    {
        var valList = new List<int>();
        valList.Add(skillInfo.SkillRecord.EffectValue[1] + (skillInfo.SkillActureLevel - 1) * skillInfo.SkillRecord.EffectValue[2]);
        valList.Add(skillInfo.SkillActureLevel * skillInfo.SkillRecord.EffectValue[0]);

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
                var impactGO = resGO;
                impactGO.transform.SetParent(skillMotion.transform);

                var damages = skillMotion.GetComponentsInChildren<ImpactDamage>(true);
                float damageValue = 0;
                foreach (var impDamage in damages)
                {
                    if (impDamage._IsCharSkillDamage)
                        damageValue += impDamage._DamageRate;
                }

                var bulletEmitterEle = impactGO.GetComponent<ImpactExAttack>();
                bulletEmitterEle._AttackTimes = _AttackTimes;
                bulletEmitterEle._Damage = damageValue * _Damage / _AttackTimes;
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

                    var impactGO = resGO;
                    impactGO.transform.SetParent(skillMotion.transform);

                    var damages = skillMotion.GetComponentsInChildren<ImpactDamage>(true);
                    float damageValue = 0;
                    foreach (var impDamage in damages)
                    {
                        if (impDamage._IsCharSkillDamage)
                            damageValue += impDamage._DamageRate;
                    }

                    var bulletEmitterEle = impactGO.GetComponent<ImpactExAttack>();
                    bulletEmitterEle._AttackTimes = _AttackTimes;
                    bulletEmitterEle._Damage = damageValue * _Damage / _AttackTimes;
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
        var atkTimes = skillRecord.EffectValue[1] + (skillLevel - 1) * skillRecord.EffectValue[2];
        var strFormat = StrDictionary.GetFormatStr(skillRecord.DescStrDict, GameDataValue.ConfigIntToPersent(damageModify), atkTimes);
        return strFormat;
    }
    #region 

    public int _AttackTimes;
    public float _Damage;
    public string _ImpactName;
    
    #endregion
}
