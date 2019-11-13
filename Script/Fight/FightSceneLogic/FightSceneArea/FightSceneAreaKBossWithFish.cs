using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class FightSceneAreaKBossWithFish : FightSceneAreaBase
{

    public override void StartArea()
    {
        base.StartArea();

        UIFightWarning.ShowBossAsyn();

        StartStep();
    }

    public override void UpdateArea()
    {
        base.UpdateArea();

        UpdateFish();
    }

    public override void MotionDie(MotionManager motion)
    {
        Debug.Log("MotionDie motion " + motion.name);

        base.MotionDie(motion);

        StepMotionDie(motion);

    }

    public override Transform GetAreaTransform()
    {
        return _BossBornPos;
    }

    #region enemy step

    public Transform _BossBornPos;
    public string _BossMotionID;
    public bool _FishBornFixPos = true;
    public Transform[] _EnemyBornPos;
    public SerializeRandomEnemy[] _EnemyMotionID;
    public float _BossStepEnemyInterval = 10;
    public int _FightingEnemyCnt = 2;

    private int _RateTotle = -1;
    private int _LivingEnemyCnt = 0;
    private float _LastUpdateFishTime = 0;

    protected void StartStep()
    {
        _BossStepEnemyInterval = 30;
        var bossMotion = FightManager.Instance.InitEnemy(_BossMotionID, _BossBornPos.position, _BossBornPos.rotation.eulerAngles);
        AI_Base ai = bossMotion.GetComponent<AI_Base>();
        if (ai != null)
        {
            ai._TargetMotion = FightManager.Instance.MainChatMotion;
            ai.AIWake = true;

            InitBossAILevel(ai as AI_HeroBase);
        }

        for (int i = 0; i < _FightingEnemyCnt; ++i)
        {
            CreateEngoughEnemy();
            _LastUpdateFishTime = Time.time;
        }
    }

    private void UpdateFish()
    {
        if (Time.time - _LastUpdateFishTime > _BossStepEnemyInterval)
        {
            if (_LivingEnemyCnt < _FightingEnemyCnt)
            {
                CreateEngoughEnemy();
                _LastUpdateFishTime = Time.time;
            }
        }
    }

    private void StepMotionDie(MotionManager motion)
    {
        if (motion.MonsterBase == null)
            return;

        if (motion.MonsterBase.Id == _BossMotionID)
        {
            FinishArea();
            FightManager.Instance.KillAllMotion();
        }
        
        --_LivingEnemyCnt;
    }

    private void CreateEngoughEnemy()
    {
        if (_EnemyMotionID.Length == 0)
            return;

        MotionManager enemyMotion;
        if (_FishBornFixPos)
        {
            int randomPos = Random.Range(0, _EnemyBornPos.Length);
            enemyMotion = FightManager.Instance.InitEnemy(GetRandomEnmeyID(), _EnemyBornPos[randomPos].position, _EnemyBornPos[randomPos].rotation.eulerAngles);
        }
        else
        {
            enemyMotion = FightManager.Instance.InitEnemy(GetRandomEnmeyID(), GetFishRandomPos(), Vector3.zero);
        }
        AI_Base ai = enemyMotion.GetComponent<AI_Base>();
        if (ai != null)
        {
            ai._HuntRange = 999;
            ai._TargetMotion = FightManager.Instance.MainChatMotion;
            ai.AIWake = true;
        }
        ++_LivingEnemyCnt;
    }

    private static List<Vector3> _FishRandomPosDelta = new List<Vector3>()
    {
        new Vector3(-10,0,0),
        new Vector3(-7.5f,0,7.5f),
        new Vector3(0,0,10),
        new Vector3(7.5f,0,7.5f),
        new Vector3(10,0,0),
        new Vector3(7.5f,0,-7.5f),
        new Vector3(0,0,-10),
        new Vector3(-7.5f,0,-7.5f),
    };
    private static float _FarDistance = 7.0f;
    private Vector3 GetFishRandomPos()
    {
        List<Vector3> farPoses = new List<Vector3>();
        for (int i = 0; i < _FishRandomPosDelta.Count; ++i)
        {
            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(_FishRandomPosDelta[i] + FightManager.Instance.MainChatMotion.transform.position, out navMeshHit, 100, -1))
            {
                if (Vector3.Distance(navMeshHit.position, FightManager.Instance.MainChatMotion.transform.position) > _FarDistance)
                {
                    farPoses.Add(navMeshHit.position);
                }
            }
        }
        int randomIdx = Random.Range(0, farPoses.Count);
        var randomPos = farPoses[randomIdx];

        return randomPos;

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

    #region boss ai

    private int _BossAILevel = 0;

    public void SetBossAILevel(int aiLevel)
    {
        _BossAILevel = aiLevel;
    }

    public void InitBossAILevel(AI_HeroBase aiBoss)
    {
        if (aiBoss == null)
            return;
        switch (_BossAILevel)
        {
            case 0:
                aiBoss._IsRiseBoom = false;
                break;
            case 1:
                aiBoss._IsRiseBoom = true;
                break;
            case 2:
                aiBoss.InitProtectTimes(1);
                break;
            case 3:
                aiBoss.InitProtectTimes(1);
                aiBoss._StageBuffHpPersent.Add(0.5f);
                break;
            case 4:
                aiBoss.InitProtectTimes(2);
                aiBoss._StageBuffHpPersent.Add(0.5f);
                break;
            case 5:
                aiBoss.InitProtectTimes(2);
                aiBoss._StageBuffHpPersent.Add(0.6f);
                aiBoss.IsCancelNormalAttack = true;
                break;
            default:
                aiBoss.InitProtectTimes(2);
                aiBoss._StageBuffHpPersent.Add(2f);
                aiBoss._StageBuffHpPersent.Add(0.6f);
                aiBoss.IsCancelNormalAttack = true;
                break;
        }
    }

    #endregion

    public override List<string> GetAreaMonIDs()
    {
        List<string> monIdList = new List<string>();

        monIdList.Add(_BossMotionID);
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
