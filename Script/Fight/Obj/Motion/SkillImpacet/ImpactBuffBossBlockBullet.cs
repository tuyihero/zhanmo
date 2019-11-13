using UnityEngine;
using System.Collections;

public class ImpactBuffBossBlockBullet : ImpactBuff
{

    public override bool IsBuffCanHit(MotionManager impactSender, ImpactHit impactHit)
    {
        if (impactHit == null)
            return true;

        if (!impactHit._IsCharSkillDamage
            && (_BuffOwner._ActionState != _BuffOwner._StateHit
                && _BuffOwner._ActionState != _BuffOwner._StateFly
                && _BuffOwner._ActionState != _BuffOwner._StateLie))
            return false;

        return true;
    }
}
