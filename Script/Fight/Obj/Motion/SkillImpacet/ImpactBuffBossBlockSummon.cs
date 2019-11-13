using UnityEngine;
using System.Collections;

public class ImpactBuffBossBlockSummon : ImpactBuff
{

    public override bool IsBuffCanHit(MotionManager impactSender, ImpactHit impactHit)
    {
        if (impactHit == null)
            return true;

        if ((impactHit.SkillMotion == null || impactHit.SkillMotion.MotionManager ==null || impactHit.SkillMotion.MotionManager.IsSummonMotion)
            && (_BuffOwner._ActionState != _BuffOwner._StateHit
                && _BuffOwner._ActionState != _BuffOwner._StateFly
                && _BuffOwner._ActionState != _BuffOwner._StateLie))
            return false;

        return true;
    }
}
