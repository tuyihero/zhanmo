using UnityEngine;
using System.Collections;

public class AI_EliteHeroNeZha : AI_HeroDexNormal
{
    protected override void AIUpdate()
    {
        base.AIUpdate();

        SkillUpdate();
    }

    #region skill move

    public int _MoveSkillIdx;
    public float _KeepDis = 5;

    private void SkillUpdate()
    {
        if (_SelfMotion.ActingSkill != _AISkills[_MoveSkillIdx].SkillBase)
            return;

        float distance = Vector3.Distance(transform.position, _TargetMotion.transform.position);
        if (distance < _KeepDis)
        {
            var back = _SelfMotion.transform.position - _TargetMotion.transform.position;
            var targetPos = back.normalized * _KeepDis + _SelfMotion.transform.position;
            _SelfMotion.MoveTarget(targetPos);
        }

        _SelfMotion.SetLookAt(_TargetMotion.transform.position);
    }

    #endregion
}
