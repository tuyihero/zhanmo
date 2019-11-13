using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
 

public class FightSceneLogicPassBoss : FightSceneLogicPassArea
{

    public List<Transform> _PlayerTeleportPoses;
    public List<AreaGate> _AreaGates;

    public override void StartLogic()
    {
        var bossStage = TableReader.BossStage.GetRecord(ActData.Instance._ProcessStageIdx.ToString());
        for (int i = 0; i < _FightArea.Count; ++i)
        {
            ResourcePool.Instance.LoadConfig("FightSceneLogic/BossStage/" + bossStage.FightLogic[i], (resName, resGO, callbackHash)=>
            {

                var sceneGO = resGO;
                _FightArea[i] = sceneGO.GetComponent<FightSceneAreaBase>();
                sceneGO.SetActive(true);
                sceneGO.transform.SetParent(_PlayerTeleportPoses[i].parent);
                sceneGO.transform.position = _PlayerTeleportPoses[i].position;

                if (_FightArea[i] is FightSceneAreaKBossWithFish)
                {
                    var bossArea = _FightArea[i] as FightSceneAreaKBossWithFish;
                    bossArea._BossMotionID = bossStage.BossID.Id;
                    bossArea.SetBossAILevel(bossStage.Difficult);
                }
            }, null);

            
        }

        for (int i = 0; i < _AreaGates.Count; ++i)
        {
            _AreaGates[i].gameObject.SetActive(false);
        }

        //base.StartLogic();
        StartCoroutine(StartLogicDelay());
    }

    private IEnumerator StartLogicDelay()
    {
        yield return new WaitForSeconds(2.0f);
        base.StartLogic();
    }

    public override void AreaStart(FightSceneAreaBase startArea)
    {
        
        //_IsTeleporting = false;
        FightManager.Instance.MoveToNewScene(LogicManager.Instance.EnterStageInfo.ValidScenePath[_RunningIdx]);
        if (_RunningIdx > 0)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(LogicManager.Instance.EnterStageInfo.ValidScenePath[_RunningIdx - 1]);
        }

        base.AreaStart(startArea);
        //LogicManager.Instance.EnterStageInfo.ValidScenePath[_RunningIdx]
    }

    public override void AreaFinish(FightSceneAreaBase finishArea)
    {
        //if (_RunningIdx + 1 < _PlayerTeleportPoses.Count)
        //{
        //    _IsTeleporting = true;
        //    return;
        //}
        //else
        //{
        //    base.AreaFinish(finishArea);
        //}

        if (_RunningIdx == _AreaGates.Count - 1)
        {
            base.AreaFinish(finishArea);
        }
        else
        {
            _AreaGates[_RunningIdx].gameObject.SetActive(true);
        }
        
    }

    public override void StartNextArea()
    {
        base.StartNextArea();
    }

    protected override void UpdateLogic()
    {
        base.UpdateLogic();

        //UpdateTeleport();
    }

    #region telepor

    public static float _TeleDistance = 3;
    public static float _TeleProcessTime = 1;

    private bool _Teleporting = false;
    private float _StartingTime = 0;

    private bool _IsTeleporting = false;

    private void UpdateTeleport()
    {
        if (!_IsTeleporting)
            return;

        var timeDelta = Time.time - _StartingTime;
        FightManager.Instance.MainChatMotion.SkillProcessing = timeDelta / _TeleProcessTime;
        if (FightManager.Instance.MainChatMotion.SkillProcessing >= 1)
        {
            FightManager.Instance.TeleportToNextRegion(_PlayerTeleportPoses[_RunningIdx + 1], true);
            FightManager.Instance.MainChatMotion.SkillProcessing = 0;
            _Teleporting = false;
        }
    }

    #endregion
}
