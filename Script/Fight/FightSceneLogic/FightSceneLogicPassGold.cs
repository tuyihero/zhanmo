using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
 

public class FightSceneLogicPassGold : FightSceneLogicPassArea
{
    public float _NextWaveTime = 5.0f;
    public int _LargeCircleWave = 10;
    public int _ExitEnemyCnt = 10;
    public int _LastTime = 180;

    private int _WaveCnt = 0;
    private float _LastWaveTime = 0;
    private bool _IsCanStartNextWave = false;

    private string _RandomMonID1 = "21";
    private string _RandomMonID2 = "30";

    public override void StartLogic()
    {
        if (FightManager.Instance.MainChatMotion != null)
        {
            FightManager.Instance.MainChatMotion.SetPosition(_MainCharBornPos.position);
            FightManager.Instance.MainChatMotion.SetRotate(_MainCharBornPos.rotation.eulerAngles);
        }
        var actGroup = FightManager.Instance._AreaGroups[LogicManager.Instance.EnterStageInfo.ValidScenePath[0]];
        actGroup._LightGO.SetActive(true);

        

        StartCoroutine(StartLogicDelay());

        
    }

    private IEnumerator StartLogicDelay()
    {
        yield return new WaitForSeconds(0.5f);

        AreaStart(_FightArea[0]);

        _IsStart = true;
        StartTimmer();
        FightManager.Instance._LogicPassTime = _LastTime;
        InitActInfo();
    }

    public override void AreaStart(FightSceneAreaBase startArea)
    {
        ++_WaveCnt;
        _LastWaveTime = Time.time;
        base.AreaStart(startArea);
    }

    public override void AreaFinish(FightSceneAreaBase finishArea)
    {
        _IsCanStartNextWave = true;  
    }

    public override void StartNextArea()
    {
        base.StartNextArea();
    }

    public override void MotionDie(MotionManager motion)
    {
        base.MotionDie(motion);

        UpdateMonsterDie(motion);
    }

    public override List<string> GetLogicMonIDs()
    {
        int randomIdx = Random.Range(0, FightSceneLogicRandomArea._NormalMon.Count);
        _RandomMonID1 = FightSceneLogicRandomArea._NormalMon[randomIdx].ToString();
        int randomIdx2 = Random.Range(0, FightSceneLogicRandomArea._MagicMon.Count);
        _RandomMonID2 = FightSceneLogicRandomArea._MagicMon[randomIdx2].ToString();


        var kallenemyArea1 = _FightArea[0] as FightSceneAreaKAllEnemy;
        if (kallenemyArea1 != null)
        {
            foreach (var enemyInfo in kallenemyArea1._EnemyBornPos)
            {
                enemyInfo._EnemyDataID = _RandomMonID1;
            }
        }

        var kallenemyArea2 = _FightArea[1] as FightSceneAreaKAllEnemy;
        if (kallenemyArea2 != null)
        {
            foreach (var enemyInfo in kallenemyArea2._EnemyBornPos)
            {
                enemyInfo._EnemyDataID = _RandomMonID2;
            }
        }

        List<string> monsterIDs = new List<string>();
        monsterIDs.Add(_RandomMonID1);
        monsterIDs.Add(_RandomMonID2);
        return monsterIDs;
    }

    protected override void UpdateLogic()
    {
        var kenemyArea = _FightArea[0] as FightSceneAreaKAllEnemy;
        var kenemyArea1 = _FightArea[1] as FightSceneAreaKAllEnemy;
        if (FightManager.Instance._LogicFightTime <= 0)
        {
            kenemyArea.ClearMonsters();
            kenemyArea1.ClearMonsters();
            LogicFinish(true);
            enabled = false;
            return;
        }
        
        if (kenemyArea._EnemyAI.Count + kenemyArea1 ._EnemyAI.Count < _ExitEnemyCnt && Time.time - _LastWaveTime > _NextWaveTime)
        {
            int wave = _WaveCnt % _LargeCircleWave;

            if (wave == 0)
            {
                AreaStart(_FightArea[1]);
            }
            else
            {
                AreaStart(_FightArea[0]);
            }
        }
        UpdateGold();
        //UpdateTeleport();
    }

    #region act info

    private int _OrgGold;
    private int _DieMonster;
    private int _LastGold;

    private void InitActInfo()
    {
        _OrgGold = PlayerDataPack.Instance.Gold;
        _LastGold = _OrgGold;
        _DieMonster = 0;

        UIFuncInFight.UpdateGoldActInfo(_DieMonster, _LastGold - _OrgGold);
    }

    private void UpdateMonsterDie(MotionManager motion)
    {
        if (motion.MotionType != MOTION_TYPE.MainChar)
        {
            ++_DieMonster;
        }

        UIFuncInFight.UpdateGoldActInfo(_DieMonster, _LastGold - _OrgGold);
    }

    private void UpdateGold()
    {
        if (PlayerDataPack.Instance.Gold > _LastGold)
        {
            _LastGold = PlayerDataPack.Instance.Gold;
        }

        UIFuncInFight.UpdateGoldActInfo(_DieMonster, _LastGold - _OrgGold);
    }


    #endregion

}
