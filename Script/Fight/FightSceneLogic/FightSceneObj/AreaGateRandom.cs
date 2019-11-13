using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaGateRandom : AreaGate
{

    protected override void UpdateTeleProcesing()
    {
        if (_Teleporting)
        {
            //AddNextScene();
            var timeDelta = Time.time - _StartingTime;
            //if (_AsyncLoad != null && _AsyncLoad.progress == 1)
            //{
            //    var process = Mathf.Min(_AsyncLoad.progress, timeDelta / _TeleProcessTime);
            //    FightManager.Instance.MainChatMotion.SkillProcessing = process;
            //    if (FightManager.Instance.MainChatMotion.SkillProcessing >= 1)
            //    {
            //        TeleportAct();
            //        FightManager.Instance.MainChatMotion.SkillProcessing = 0;
            //        _Teleporting = false;
            //    }
            //}
            float process = timeDelta / _TeleportTime;
            FightManager.Instance.MainChatMotion.SkillProcessing = process;
            if (FightManager.Instance.MainChatMotion.SkillProcessing >= 1)
            {
                TeleportAct();
                FightManager.Instance.MainChatMotion.SkillProcessing = 0;
                _Teleporting = false;
            }
        }
        else
        {
            FightManager.Instance.MainChatMotion.SkillProcessing = 0;
        }
    }

    protected override void TeleportAct()
    {
        Debug.Log("TeleportAct");

        //FightManager.Instance.TeleportToNextRegion(_DestPos, _IsTransScene);
        RandomLogic.TeleportToNext();
    }

    #region teleport time

    public float _TeleportTime = 1.5f;

    #endregion

    #region act next scene

    private bool _IsAddNextScene = false;
    private AsyncOperation _AsyncLoad;

    public FightSceneLogicRandomArea RandomLogic { get; set; }

    public void AddNextScene()
    {
        if (_IsAddNextScene)
            return;

        if (RandomLogic == null)
            return;

        _IsAddNextScene = true;
        _StartingTime = Time.time;
        string nextScene = RandomLogic.GetNextScene();
        _AsyncLoad = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
    }

    #endregion
}
