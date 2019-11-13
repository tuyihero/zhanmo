using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffUsingSkillSub : ImpactBuff
{
    public ImpactBuff _SubBuff;
    public List<string> _ActingSkills = new List<string>();

    private ImpactBuff _SubBuffInstance;

    public override void UpdateBuff()
    {
        if (_BuffOwner.ActingSkill == null)
        {
            DispatchSub();
            return;
        }

        if (!_ActingSkills.Contains(_BuffOwner.ActingSkill._ActInput))
        {
            DispatchSub();
            return;
        }

        PatchSub();
    }

    private void PatchSub()
    {
        if (_SubBuffInstance != null)
            return;

        _SubBuffInstance = _SubBuff.ActBuffInstance(_BuffOwner, _BuffOwner);
    }

    private void DispatchSub()
    {
        if (_SubBuffInstance == null)
            return;

        _BuffOwner.RemoveBuff(_SubBuffInstance);
    }

}
