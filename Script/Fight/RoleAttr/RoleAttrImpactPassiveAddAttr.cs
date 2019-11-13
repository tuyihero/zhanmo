using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class RoleAttrImpactPassiveAddAttr : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _AddValue = GetValueFromTab(attrTab, args[1]);
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        ResourcePool.Instance.LoadConfig("Bullet\\Passive\\" + _ImpactName, (resName, resGO, hash) =>
        {
            var buffGO = resGO;
            buffGO.transform.SetParent(roleMotion.BuffBindPos.transform);
            var buffs = buffGO.GetComponents<ImpactBuffAttrAdd>();
            foreach (var buff in buffs)
            {
                buff._AddValue = _AddValue;
                buff.ActImpact(roleMotion, roleMotion);
            }
        }, null);
    }

    public new static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var attrTab = Tables.TableReader.AttrValue.GetRecord(attrDescID.ToString());
        var lastTime = GetValueFromTab(attrTab, attrParams[1]);
        string valueFromat = "";
        if (attrTab.AttrParams[3] == 0)
        {
            valueFromat = GameDataValue.ConfigFloatToPersent(lastTime).ToString();
        }
        else if (attrTab.AttrParams[3] == 1)
        {
            valueFromat = GameDataValue.ConfigIntToPersent((int)lastTime).ToString();
        }
        else if (attrTab.AttrParams[3] == 2)
        {
            valueFromat = ((int)lastTime).ToString();
        }
        var strFormat = StrDictionary.GetFormatStr(attrDescID, valueFromat);
        return strFormat;
    }

    #region 

    private float _AddValue;

    private static float GetValueFromTab(AttrValueRecord attrRecord, int level)
    {
        var theValue = GameDataValue.ConfigIntToFloat(attrRecord.AttrParams[0] + attrRecord.AttrParams[1] * (level - 1));
        theValue = Mathf.Min(theValue, attrRecord.AttrParams[2]);
        return theValue;
    }

    #endregion
}
