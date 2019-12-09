using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 

public class FightSceneLogicPassArea : FightSceneLogicBase
{

    #region 


    public List<FightSceneAreaBase> _FightArea;

    #endregion

    protected FightSceneAreaBase _RunningArea;
    protected int _RunningIdx;

    public override void StartLogic()
    {
        base.StartLogic();

        if (FightManager.Instance.MainChatMotion != null)
        {
            FightManager.Instance.MainChatMotion.SetPosition(_MainCharBornPos.position);
        }

        for (int i = 0; i < _FightArea.Count; ++i)
        {
            _FightArea[i].InitArea();
            _FightArea[i].AreaID = i;
        }

        _RunningIdx = -1;

        StartNextArea();

    }

    protected override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_RunningArea != null)
            _RunningArea.UpdateArea();
    }

    public override void MotionDie(MotionManager motion)
    {
        base.MotionDie(motion);

        foreach (var area in _FightArea)
        {
            if (area.AreaState == AreaState.Acting)
            {
                if (area is FightSceneAreaKShowTeleport)
                    continue;

                area.MotionDie(motion);
            }
        }
        
    }

    public override void MotionDisapear(MotionManager motion)
    {
        base.MotionDisapear(motion);

        foreach (var area in _FightArea)
        {
            if (area.AreaState == AreaState.Acting)
            {
                if (area is FightSceneAreaKShowTeleport)
                    continue;

                area.MotionDisapear(motion);
            }
        }

    }

    public Vector3 GetNextAreaPos()
    {
        FightSceneAreaBase nextArea = _RunningArea;
        if (nextArea == null)
        {
            if (_RunningIdx + 1 < _FightArea.Count)
            {
                nextArea = _FightArea[_RunningIdx + 1];
            }
        }

        if (nextArea is FightSceneAreaKAllEnemy)
        {
            return (nextArea as FightSceneAreaKAllEnemy)._EnemyBornPos[0]._EnemyTransform.position;
        }
        else if (nextArea is FightSceneAreaKEnemyCnt)
        {
            return (nextArea as FightSceneAreaKEnemyCnt)._EnemyBornPos[0].position;
        }
        else if (nextArea is FightSceneAreaKBossWithFish)
        {
            return (nextArea as FightSceneAreaKBossWithFish)._BossBornPos.position;
        }
        else if (nextArea is FightSceneAreaKShowTeleport)
        {
            return (nextArea as FightSceneAreaKShowTeleport)._Teleport.transform.position;
        }
        return Vector3.zero;
    }

    #region 

    public virtual void AreaStart(FightSceneAreaBase startArea)
    {
        _RunningArea = startArea;
        startArea.StartArea();
    }

    public virtual void AreaFinish(FightSceneAreaBase finishArea)
    {
        if (finishArea == _RunningArea)
        {
            _RunningArea = null;
        }
        StartNextArea();
    }

    public virtual void StartNextArea()
    {
        ++_RunningIdx;
        if (_RunningIdx < _FightArea.Count)
        {
            //if (_FightArea[_RunningIdx] is FightSceneAreaKShowTeleport || _FightArea[_RunningIdx] is FightSceneAreaKBossWithFish)
            {
                AreaStart(_FightArea[_RunningIdx]);
            }

            if(FightManager.Instance.MainChatMotion != null)
                UIFightWarning.ShowDirectAsyn(FightManager.Instance.MainChatMotion.transform, _FightArea[_RunningIdx].GetAreaTransform());
        }
        else
        {
            LogicFinish(true);
        }
    }



    #endregion

    public override List<string> GetLogicMonIDs()
    {
        List<string> monIds = new List<string>();

        for (int i = 0; i < _FightArea.Count; ++i)
        {
            if (_FightArea[i] == null)
                continue;

            var areaIds = _FightArea[i].GetAreaMonIDs();

            if (areaIds == null)
                continue;

            for (int j = 0; j < areaIds.Count; ++j)
            {
                if (!monIds.Contains(areaIds[j]))
                {
                    monIds.Add(areaIds[j]);
                }
            }
        }

        return monIds;
    }
}
