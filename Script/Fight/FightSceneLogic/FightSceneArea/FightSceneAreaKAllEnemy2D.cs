using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightSceneAreaKAllEnemy2D : FightSceneAreaBase
{
    public override void StartArea()
    {
        base.StartArea();
        StartStep();
    }

    public override void UpdateArea()
    {
        base.UpdateArea();
        
    }

    public override void MotionDisapear(MotionManager motion)
    {
        //Debug.Log("MotionDie motion " + motion.name);

        base.MotionDie(motion);

        StepMotionDie(motion);

    }

    public override Transform GetAreaTransform()
    {
        return transform;
    }

    public override void FinishArea()
    {
        FightManager.Instance._CameraFollow.SetLookPosX(-1);
        base.FinishArea();
    }

    #region enemy step

    public List<SerializeEnemyInfo> _EnemyBornPos;
    public float _AreaPosX = 0;

    protected int _DeadEnemyCnt;
    protected List<AI_Base> _EnemyAI = new List<AI_Base>();
    public List<AI_Base> EnemyAI
    {
        get
        {
            return _EnemyAI;
        }
    }

    protected bool _IsLastNormalMonster = false;

    protected virtual void StartStep()
    {
        FightManager.Instance._CameraFollow.SetLookPosX(_AreaPosX * 0.01f);
        RefreshMonster();
    }



    protected void RefreshMonster()
    {
        for(int i = 0; i< _EnemyBornPos.Count; ++i)
        {
            Vector3 bornPos = new Vector3(_EnemyBornPos[i]._EnemyPos2D.x, FightManager.Instance._CameraFollow._SceneAnimController.SceneY, _EnemyBornPos[i]._EnemyPos2D.y);
            if (_EnemyBornPos[i]._EnemyPos2D.x > 0)
            {
                bornPos.x = FightManager.Instance._CameraFollow.MainMovePosXMax + 2;
            }
            else
            {
                bornPos.x = FightManager.Instance._CameraFollow.MainMovePosXMin - 2;
            }
            MotionManager enemy = FightManager.Instance.InitEnemy(_EnemyBornPos[i]._EnemyDataID, bornPos, Vector3.zero);

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

        for (int i = 0; i < _EnemyBornPos.Count; ++i)
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
