using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;

public class FightSceneLogicSimple : FightSceneLogicBase
{
   

    private int _LogicStep = 0;

    public override void StartLogic()
    {
        base.StartLogic();

        _LogicStep = 0;
        StartStep0();

        //_LogicStep = 1;
        //StartStep1();
    }

    protected override void UpdateLogic()
    {
        base.UpdateLogic();

        if(_LogicStep == 1)
            UpdateBoss();
    }

    public override void MotionDie(MotionManager motion)
    {
        Debug.Log("MotionDie motion " + motion.name);
        if (motion.RoleAttrManager.MotionType == MOTION_TYPE.MainChar)
        {
            FightManager.Instance.LogicFinish(false);
            return;
        }

        base.MotionDie(motion);

        if (_LogicStep == 0)
        {
            Step0MotionDie(motion);
        }
        else if (_LogicStep == 1)
        {
            Step1MotionDie(motion);
        }
    }

    #region enemy step

    public Transform[] _EnemyBornPos;
    public string _EnemyMotionID;
    public int _KillEnemyCnt = 20;
    public int _FightingEnemyCnt = 6;

    private int _DeadEnemyCnt = 0;
    private int _InitPosIdx = 0;
    private int _CurEnemyCnt = 0;

    private void StartStep0()
    {
        _DeadEnemyCnt = 0;
        _InitPosIdx = 0;
        _CurEnemyCnt = 0;
        for (int i = 0; i < _FightingEnemyCnt; ++i)
        {
            CreateEngoughEnemy();
        }

    }

    private void Step0MotionDie(MotionManager motion)
    {
        if (motion.RoleAttrManager.MotionType == MOTION_TYPE.Elite
                || motion.RoleAttrManager.MotionType == MOTION_TYPE.Normal)
        {
            ++_DeadEnemyCnt;
            --_CurEnemyCnt;
            CreateEngoughEnemy();
        }

        if (_DeadEnemyCnt >= _KillEnemyCnt)
        {
            _LogicStep = 1;
            StartStep1();
        }
    }

    private void CreateEngoughEnemy()
    {
        if (_DeadEnemyCnt >= _KillEnemyCnt)
        {
            return;
        }

        if (_InitPosIdx >= _EnemyBornPos.Length)
        {
            _InitPosIdx = 0;
        }

        FightManager.Instance.InitEnemy(_EnemyMotionID, _EnemyBornPos[_InitPosIdx].position, _EnemyBornPos[_InitPosIdx].rotation.eulerAngles);
        ++_InitPosIdx;
        ++_CurEnemyCnt;
    }

    #endregion

    #region boss

    public Transform _BossBornPos;
    public string _BossMotionID;
    public int _BossStepEnemyCnt = 2;
    public float _BossStepEnemyInterval = 10;

    private float _CreateEnemyTimeCD = 0;
    private int _CurEnemyCnt1 = 0;

    private void Step1MotionDie(MotionManager motion)
    {
        --_CurEnemyCnt1;

        if (motion.RoleAttrManager.MotionType == MOTION_TYPE.Hero)
        {
            FightManager.Instance.LogicFinish(true);
        }
    }

    private void StartStep1()
    {
        FightManager.Instance.InitEnemy(_BossMotionID, _BossBornPos.position, _BossBornPos.rotation.eulerAngles);
        _CreateEnemyTimeCD = _BossStepEnemyInterval;
        _CurEnemyCnt1 = 0;
    }


    private void BossStepCreateEnemy()
    {
        int initPosIdx = Random.Range(0, _EnemyBornPos.Length);
        FightManager.Instance.InitEnemy(_EnemyMotionID, _EnemyBornPos[initPosIdx].position, _EnemyBornPos[initPosIdx].rotation.eulerAngles);
        ++_CurEnemyCnt1;
    }

    private void UpdateBoss()
    {
        if (_CreateEnemyTimeCD > 0)
        {
            _CreateEnemyTimeCD -= Time.fixedDeltaTime;

            if (_CreateEnemyTimeCD <= 0)
            {
                for (int i = _CurEnemyCnt1; i < _BossStepEnemyCnt; ++i)
                {
                    BossStepCreateEnemy();
                }
            }
        }
    }

    #endregion

}
