using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[System.Serializable]
public class SerializeRandomEnemy
{
    public string _EnemyDataID;
    public int _Rate;
}

public class FightSceneAreaKEnemyCnt : FightSceneAreaBase
{

    public override void StartArea()
    {
        base.StartArea();

        UIFightWarning.ShowFightAsyn();
        
        StartStep();
    }

    public override void UpdateArea()
    {
        base.UpdateArea();
    }

    public override void MotionDie(MotionManager motion)
    {

        base.MotionDie(motion);

        StepMotionDie(motion);

    }

    public override Transform GetAreaTransform()
    {
        return _EnemyBornPos[0];
    }

    #region enemy step

    public Transform[] _EnemyBornPos;
    public SerializeRandomEnemy[] _EnemyMotionID;
    public int _KillEnemyCnt = 20;
    public int _FightingEnemyCnt = 6;

    private int _DeadEnemyCnt = 0;
    private int _InitPosIdx = 0;
    private int _CurEnemyCnt = 0;

    private int _RateTotle = -1;

    private void StartStep()
    {
        _DeadEnemyCnt = 0;
        _InitPosIdx = 0;
        _CurEnemyCnt = 0;
        for (int i = 0; i < _FightingEnemyCnt; ++i)
        {
            CreateEngoughEnemy();
        }

    }

    private void StepMotionDie(MotionManager motion)
    {

        ++_DeadEnemyCnt;
        --_CurEnemyCnt;
        CreateEngoughEnemy();

        if (_CurEnemyCnt == 0)
        {
            FinishArea();
        }
    }

    private void CreateEngoughEnemy()
    {
        if (_DeadEnemyCnt + _CurEnemyCnt >= _KillEnemyCnt)
        {
            return;
        }

        if (_InitPosIdx >= _EnemyBornPos.Length)
        {
            _InitPosIdx = 0;
        }

        var enemyMotion = FightManager.Instance.InitEnemy(GetRandomEnmeyID(), _EnemyBornPos[_InitPosIdx].position, _EnemyBornPos[_InitPosIdx].rotation.eulerAngles);
        AI_CloseAttack ai = enemyMotion.GetComponent<AI_CloseAttack>();
        if (ai != null)
        {
            ai._AlertRange = 1000;
        }
        ++_InitPosIdx;
        ++_CurEnemyCnt;
    }

    private string GetRandomEnmeyID()
    {
        if (_RateTotle < 0)
        {
            _RateTotle = 0;
            foreach (var random in _EnemyMotionID)
            {
                _RateTotle += random._Rate;
            }
        }

        int randomValue = Random.Range(0, _RateTotle);
        int totleRate = 0;
        foreach (var random in _EnemyMotionID)
        {
            if (random._Rate + totleRate >= randomValue)
            {
                return random._EnemyDataID;
            }
            totleRate += random._Rate;
        }
        return _EnemyMotionID[_EnemyMotionID.Length - 1]._EnemyDataID;
    }
    #endregion

    public override List<string> GetAreaMonIDs()
    {
        List<string> monIdList = new List<string>();

        for (int i = 0; i < _EnemyMotionID.Length; ++i)
        {
            if (!monIdList.Contains(_EnemyMotionID[i]._EnemyDataID))
            {
                monIdList.Add(_EnemyMotionID[i]._EnemyDataID);
            }
        }

        return monIdList;
    }
}
