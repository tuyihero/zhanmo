using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactPassiveIceCircle : RoleAttrImpactPassive
{

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        
        ResourcePool.Instance.LoadConfig("Bullet\\Passive\\" + _ImpactName, (resName, resGO, hash) =>
        {
            var buffGO = resGO;
            var buffs = buffGO.GetComponents<ImpactBuff>();
            foreach (var buff in buffs)
            {
                buff.ActImpact(roleMotion, roleMotion);
            }
        }, null);
    }


    #region 

    
    #endregion
}
