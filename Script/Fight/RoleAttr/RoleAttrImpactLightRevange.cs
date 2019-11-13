using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class RoleAttrImpactLightRevange : RoleAttrImpactPassive
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);
        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());

        _Damage = GetValueFromTab(attrTab, args[1]);
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        ResourcePool.Instance.LoadConfig("Bullet\\Passive\\" + _ImpactName, (resName, resGO, hash) =>
        {
            var buffGO = resGO;
            buffGO.transform.SetParent(roleMotion.BuffBindPos.transform);
            var bulletScripts = buffGO.GetComponentsInChildren<ImpactBuff>();
            foreach (var buff in bulletScripts)
            {
                var subBuffs2 = buffGO.GetComponentsInChildren<BulletEmitterBase>();
                foreach (var subBuff in subBuffs2)
                {
                    if (subBuff.gameObject == buffGO)
                        continue;
                    subBuff._Damage = _Damage;
                }

                buff.ActImpact(roleMotion, roleMotion);
            }
        }, null);

        
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {

    }

    public override bool AddData(List<int> attrParam)
    {
        return true;
    }

    public new static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var attrTab = Tables.TableReader.AttrValue.GetRecord(attrDescID.ToString());
        var value1 = GetValueFromTab(attrTab, attrParams[1]);
        var strFormat = StrDictionary.GetFormatStr(attrDescID, GameDataValue.ConfigFloatToPersent(value1));
        return strFormat;
    }

    #region 

    public float _Damage;

    private static float GetValueFromTab(AttrValueRecord attrRecord, int level)
    {
        var theValue = GameDataValue.ConfigIntToFloat(attrRecord.AttrParams[0] + attrRecord.AttrParams[1] * (level - 1));
        theValue = Mathf.Min(theValue, GameDataValue.ConfigIntToFloat(attrRecord.AttrParams[2]));
        return theValue;
    }
    #endregion
}
