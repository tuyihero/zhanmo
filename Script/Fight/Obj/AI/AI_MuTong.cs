using UnityEngine;
using System.Collections;

public class AI_MuTong : AI_Summon
{
    public ImpactBuff _InitBuff;

    protected override void Init()
    {
        base.Init();

        _InitBuff.ActBuff(_SelfMotion);
        _SelfMotion.RoleAttrManager.SetBaseAttr(RoleAttrEnum.HPMax, 1);
    }

    public override void OnStateChange(StateBase orgState, StateBase newState)
    {
        base.OnStateChange(orgState, newState);

        if (orgState == null)
            return;

        StartSkill(_AISkills[0], true);
        Disapear();
    }

    private IEnumerator Disapear()
    {
        yield return new WaitForSeconds(2.0f);

        _SelfMotion.MotionDisappear();
    }
}
