using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class RoleAttrImpactPassiveHPResume : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _ResumeHP = GetValueFromTab(attrTab, args[1]);
        _ActCD = GetValue2FromTab(attrTab, args[1]);
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;
        
        ResourcePool.Instance.LoadConfig("Bullet\\Passive\\" + _ImpactName, (resName, resGO, hash) =>
        {
            var buffGO = resGO;
            buffGO.transform.SetParent(roleMotion.BuffBindPos.transform);
            var buffs = buffGO.GetComponents<ImpactBuff>();
            foreach (var buff in buffs)
            {
                var subBuffs = buffGO.GetComponentsInChildren<ImpactBuffHpLowSub>();
                foreach (var subBuff in subBuffs)
                {
                    subBuff._ActCD = _ActCD;
                }

                var subBuffs2 = buffGO.GetComponentsInChildren<ImpactResumeHP>();
                foreach (var subBuff in subBuffs2)
                {
                    if (subBuff.gameObject == buffGO)
                        continue;
                    subBuff._HPPersent = _ResumeHP;
                }
                buff.ActImpact(roleMotion, roleMotion);
            }
        }, null);
    }

    public new static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var attrTab = Tables.TableReader.AttrValue.GetRecord(attrDescID.ToString());
        var value1 = GetValueFromTab(attrTab, attrParams[1]);
        var value2 = GetValue2FromTab(attrTab, attrParams[1]);
        var strFormat = StrDictionary.GetFormatStr(attrDescID, GameDataValue.ConfigFloatToPersent(value1), value2);
        return strFormat;
    }

    #region 

    private float _ActCD;
    private float _ResumeHP;

    private static float GetValueFromTab(AttrValueRecord attrRecord, int level)
    {
        var theValue = GameDataValue.ConfigIntToFloat(attrRecord.AttrParams[0] + attrRecord.AttrParams[1] * (level - 1));
        theValue = Mathf.Min(theValue, GameDataValue.ConfigIntToFloat(attrRecord.AttrParams[2]));
        return theValue;
    }

    private static float GetValue2FromTab(AttrValueRecord attrRecord, int level)
    {
        var theValue = GameDataValue.ConfigIntToFloat(attrRecord.AttrParams[3] + attrRecord.AttrParams[4] * (level - 1));
        theValue = Mathf.Max(theValue, GameDataValue.ConfigIntToFloat(attrRecord.AttrParams[5]));
        return theValue;
    }
    #endregion
}
