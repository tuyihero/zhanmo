using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectLastHit : SelectBase
{

    public int _HittedAudio;
    public bool _ClearLastSelect = true;
    public int _Num = -1;
    
    public override void ColliderStart()
    {
        int hitNum = _Num;
        if (hitNum < 0)
        {
            hitNum = _ObjMotion.ActingSkill._SkillHitMotions.Count;
        }
        foreach (var skillMotion in _ObjMotion.ActingSkill._SkillHitMotions)
        {
            if (skillMotion == null)
                continue;

            foreach (var impact in _ImpactList)
            {
                impact.ActImpact(_ObjMotion, skillMotion);
            }

            if (_ObjMotion._IsRoleHit)
            {
                GlobalEffect.Instance.Pause(_ObjMotion._RoleHitTime);
            }
            --hitNum;
            if (hitNum <= 0)
                break;
        }

        if (_ObjMotion.ActingSkill._SkillHitMotions.Count > 0)
        {
            if (_HittedAudio > 0)
            {
                _ObjMotion.PlayAudio(ResourcePool.Instance._CommonAudio[_HittedAudio]);
            }
        }

        if (_ClearLastSelect)
        {
            _ObjMotion.ActingSkill._SkillHitMotions.Clear();
        }
    }

    public override void ColliderFinish()
    {
        _ObjMotion.ActingSkill._SkillHitMotions.Clear();
    }

}
