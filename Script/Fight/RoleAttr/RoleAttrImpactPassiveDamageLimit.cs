using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class RoleAttrImpactPassiveDamageLimit : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _LimitPersent = GameDataValue.ConfigIntToFloat( attrTab.AttrParams[0] + attrTab.AttrParams[1] * (args[1] - 1));
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        ResourcePool.Instance.LoadConfig("Bullet\\Passive\\" + _ImpactName, (resName, resGO, hash) =>
        {
            var buffGO = resGO;
            buffGO.transform.SetParent(roleMotion.BuffBindPos.transform);
            var buff = buffGO.GetComponent<ImpactBuffDamageLimit>();
            buff._LimitHPPersent = _LimitPersent;
            buff.ActImpact(roleMotion, roleMotion);
        }, null);
    }

    public new static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var attrTab = Tables.TableReader.AttrValue.GetRecord(attrDescID.ToString());
        var limit = attrTab.AttrParams[0] + attrTab.AttrParams[1] * (attrParams[1] - 1);
        var strFormat = StrDictionary.GetFormatStr(attrDescID, GameDataValue.ConfigIntToPersent(limit));
        return strFormat;
    }

    #region 

    private float _LimitPersent = 0;

    #endregion
}
