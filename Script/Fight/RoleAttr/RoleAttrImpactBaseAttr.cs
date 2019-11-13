using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class RoleAttrImpactBaseAttr : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        ResourcePool.Instance.LoadConfig("Bullet\\Emitter\\Element\\" + _ImpactName, (resName, resGO, hash)=>
        {
            resGO.transform.SetParent(skillMotion.transform);
            var bulletEmitterEle = resGO.GetComponent<BulletEmitterElement>();
            bulletEmitterEle._Damage = _Damage;
        }, null);
    }

    public override bool AddData(List<int> attrParam)
    {
        return true;
    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        //Debug.Log("attrParams:" + attrParams[0]);
        string valueStr = RandomAttrs.GetAttrValueShow((RoleAttrEnum)attrParams[0], attrParams[1]);

        if ((RoleAttrEnum)attrParams[0] == RoleAttrEnum.FinalDamageReduse)
        {
            valueStr = " -" + valueStr;
        }
        else
        {
            valueStr = " +" + valueStr;
        }
        string strFormat = "";
        if ((RoleAttrEnum)attrParams[0] == RoleAttrEnum.ExGoldDrop
            || (RoleAttrEnum)attrParams[0] == RoleAttrEnum.ExMatDrop
            || (RoleAttrEnum)attrParams[0] == RoleAttrEnum.ExGemDrop
            || (RoleAttrEnum)attrParams[0] == RoleAttrEnum.ExEquipDrop)
        {
            strFormat = CommonDefine.GetQualityColorStr(ITEM_QUALITY.ORIGIN) + StrDictionary.GetFormatStr(attrParams[0], valueStr) + "</color>";
        }
        else
        {
            strFormat = RandomAttrs.GetAttrName((RoleAttrEnum)attrParams[0]) + valueStr;
        }

        return strFormat;
    }

    public static EquipExAttr GetExAttrByValue(AttrValueRecord attrRecord, int arg)
    {
        int attrValue = attrRecord.AttrParams[1] + attrRecord.AttrParams[2] * arg;

        EquipExAttr exAttr = new EquipExAttr(attrRecord.AttrImpact, 0, attrRecord.AttrParams[0], attrValue);

        return exAttr;
    }

    #region 

    public int _Damage;
    public string _ImpactName;

    
    #endregion
}
