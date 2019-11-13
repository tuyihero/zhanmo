using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class RoleAttrImpactAddSkill : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _ImpactName = attrTab.StrParam[0];
        _SkillInput = attrTab.StrParam[1];
        _CD = attrTab.AttrParams[0] + attrTab.AttrParams[1] * args[1];
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        //var impactGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Emitter\\Element\\" + _ImpactName);
        //impactGO.transform.SetParent(skillMotion.transform);
        skillMotion._SkillCD = _CD;
    }

    public override bool AddData(List<int> attrParam)
    {
        return true;
    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int legendaryId = copyAttrs[0];
        copyAttrs.RemoveAt(0);
        var strFormat = StrDictionary.GetFormatStr(legendaryId, copyAttrs);
        return strFormat;
    }

    public ObjMotionSkillBase _AddSkill;
    public IEnumerator GetSkillBase()
    {
        return ResourceManager.Instance.LoadPrefab("Bullet\\Emitter\\Element\\" + _ImpactName, (resName, resGO, hash) =>
        {
            _AddSkill = resGO.GetComponent<ObjMotionSkillBase>();
        });
        
    }

    #region 

    public float _Damage;
    public string _ImpactName;
    public float _CD;
    
    #endregion
}
