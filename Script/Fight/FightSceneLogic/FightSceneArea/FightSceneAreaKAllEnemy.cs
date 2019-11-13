using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightSceneAreaKAllEnemy : FightSceneAreaBase
{
    public override void StartArea()
    {
        base.StartArea();
        StartStep();
    }

    public override void UpdateArea()
    {
        base.UpdateArea();

        if (!_IsEnemyAlert)
        {
            foreach (var ai in _EnemyAI)
            {
                if (ai == null)
                {
                    continue;
                }

                if (Vector3.Distance(ai.transform.position, FightManager.Instance.MainChatMotion.transform.position) < _EnemyAlertDistance)
                {
                    _IsEnemyAlert = true;
                    SetAllAlert();
                }
            }
        }
    }

    public override void MotionDie(MotionManager motion)
    {
        //Debug.Log("MotionDie motion " + motion.name);

        base.MotionDie(motion);

        StepMotionDie(motion);

    }

    public override Transform GetAreaTransform()
    {
        return _EnemyBornPos[0]._EnemyTransform;
    }

    #region enemy step

    public SerializeEnemyInfo[] _EnemyBornPos;
    protected int _DeadEnemyCnt;
    public List<AI_Base> _EnemyAI = new List<AI_Base>();

    protected virtual void StartStep()
    {
        int eliteIdx = -1;

        if (ActData.Instance.GetNormalDiff() > 1)
        {
            var eliteRate = FightManager.Instance.GetEliteMonsterRate();
            var eliteRandom = Random.Range(0, GameDataValue.GetMaxRate());

            if (eliteRandom < eliteRate)
            {
                eliteIdx = Random.Range(0, _EnemyBornPos.Length);
            }
        }

        for(int i = 0; i< _EnemyBornPos.Length; ++i)
        {
            Tables.MOTION_TYPE motionType = Tables.MOTION_TYPE.Normal;
            if (eliteIdx == i)
            {
                motionType = Tables.MOTION_TYPE.Elite;
            }

            MotionManager enemy = FightManager.Instance.InitEnemy(_EnemyBornPos[i]._EnemyDataID, _EnemyBornPos[i]._EnemyTransform.position, _EnemyBornPos[i]._EnemyTransform.rotation.eulerAngles, motionType);
            
            var enemyAI = enemy.gameObject.GetComponent<AI_Base>();
            enemyAI.GroupID = AreaID;
            _EnemyAI.Add(enemyAI);
            if (_IsEnemyAlert)
            {
                enemyAI._TargetMotion = FightManager.Instance.MainChatMotion;
                enemyAI.AIWake = true;
            }
        }
    }

    private void SetAllAlert()
    {
        foreach (var ai in _EnemyAI)
        {
            if (ai == null)
            {
                continue;
            }

            ai._TargetMotion = FightManager.Instance.MainChatMotion;
        }
    }

    private void StepMotionDie(MotionManager motion)
    {
        var enemyAI = motion.gameObject.GetComponent<AI_Base>();
        if (_EnemyAI.Contains(enemyAI))
        {
            _EnemyAI.Remove(enemyAI);
            if (_EnemyAI.Count == 0)
            {
                FinishArea();
            }
        }
    }

    #endregion

    public override List<string> GetAreaMonIDs()
    {
        List<string> monIdList = new List<string>();

        for (int i = 0; i < _EnemyBornPos.Length; ++i)
        {
            if (!monIdList.Contains(_EnemyBornPos[i]._EnemyDataID))
            {
                monIdList.Add(_EnemyBornPos[i]._EnemyDataID);
            }
        }

        return monIdList;
    }

    public void ClearMonsters()
    {
        var dieMons = new List<AI_Base>(_EnemyAI);
        foreach (var fishMotion in dieMons)
        {
            fishMotion._SelfMotion.MotionDie();
        }
    }
}
