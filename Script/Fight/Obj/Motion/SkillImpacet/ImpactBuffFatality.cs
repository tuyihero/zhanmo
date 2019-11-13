using UnityEngine;
using System.Collections;

public class ImpactBuffFatality : ImpactBuff
{

    public int _Rate;
    public float _DmgRate;

    public override void CastDamage(RoleAttrManager.DamageClass orgDamage, ImpactBase damageImpact)
    {
        base.CastDamage(orgDamage, damageImpact);

        var enemyRoleAttr = damageImpact.SkillMotion.MotionManager.RoleAttrManager;
        if (enemyRoleAttr.MotionType == Tables.MOTION_TYPE.Hero)
            return;

        if (enemyRoleAttr.MotionType == Tables.MOTION_TYPE.ExElite)
            _Rate = (int)(_Rate * 0.5f);

        if (!GameRandom.IsInRate(_Rate))
            return;

        if (orgDamage.DamageType < RoleAttrManager.ShowDamageType.Fatality)
        {
            orgDamage.DamageType = RoleAttrManager.ShowDamageType.Fatality;
        }
        orgDamage.TotalDamageValue = (int)(enemyRoleAttr.HP * 100);
    }


}
