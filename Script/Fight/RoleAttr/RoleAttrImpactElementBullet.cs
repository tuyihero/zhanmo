using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class RoleAttrImpactElementBullet : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _ImpactName = attrTab.StrParam[0];
        _SkillInput = attrTab.StrParam[1];
        _Damage = GameDataValue.ConfigIntToFloat(attrTab.AttrParams[0] + attrTab.AttrParams[1] * (args[1] - 1));
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];

        ResourcePool.Instance.LoadConfig("Bullet\\Emitter\\Element\\" + _ImpactName, (resName, resGO, hash) =>
        {
            resGO.transform.SetParent(skillMotion.transform);
            resGO.transform.localPosition = Vector3.zero;
            resGO.transform.localRotation = Quaternion.Euler(Vector3.zero);
            var bulletEmitterEle = resGO.GetComponentsInChildren<BulletEmitterBase>();
            foreach (var bulletEmitter in bulletEmitterEle)
            {
                bulletEmitter._Damage = _Damage;
            }
        }, null);
    }

    public override bool AddData(List<int> attrParam)
    {
        return true;
    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var attrTab = Tables.TableReader.AttrValue.GetRecord(attrDescID.ToString());
        var damage = attrTab.AttrParams[0] + attrTab.AttrParams[1] * (attrParams[1] - 1);
        var strFormat = StrDictionary.GetFormatStr(attrTab.StrParam[2], GameDataValue.ConfigIntToPersent(damage));
        return strFormat;
    }

    #region 

    public float _Damage;
    public string _ImpactName;

    
    #endregion
}
