using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaGate : MonoBehaviour
{

    private void Start()
    {
        StartAtNavMesh();
    }

    // Update is called once per frame
    void Update ()
    {
        TeleportUpdate();
    }

    #region teleport

    public Transform _DestPos;
    public bool _IsTransScene = true;
    public bool _NeedInitEffect = true;
    public static float _TeleDistance = 3;
    public static float _TeleAlertDistance = 5;
    public static float _TeleProcessTime = 1;
    public static float _TeleTipsTime = 5;

    protected bool _Teleporting = false;
    protected float _StartingTime = 0;
    protected float _TipsStart = 0;

    protected virtual void TeleportUpdate()
    {
        if (FightManager.Instance == null)
            return;

        var mainChar = FightManager.Instance.MainChatMotion;
        if (mainChar == null)
            return;

        var distance = Vector3.Distance(transform.position, mainChar.transform.position);
        if (mainChar._ActionState == mainChar._StateIdle &&
            distance < _TeleDistance)
        {
            if (!_Teleporting)
            {
                _Teleporting = true;
                _StartingTime = Time.time;

            }
            UpdateTeleProcesing();
        }
        else if(distance < _TeleAlertDistance)
        {
            if (_Teleporting)
            {
                _Teleporting = false;
                _StartingTime = 0;

            }
            UpdateTeleProcesing();
        }

        if (distance < _TeleDistance && Time.time - _TipsStart > _TeleTipsTime)
        {
            _TipsStart = Time.time;
            UIMessageTip.ShowMessageTip(2300082);
        }

    }

    protected virtual void UpdateTeleProcesing()
    {
        if (_Teleporting)
        {
            var timeDelta = Time.time - _StartingTime;
            FightManager.Instance.MainChatMotion.SkillProcessing = timeDelta / _TeleProcessTime;
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

    protected virtual void TeleportAct()
    {
        if (_DestPos == null)
            return;

        FightManager.Instance.TeleportToNextRegion(_DestPos, _IsTransScene);
    }

    private void StartAtNavMesh()
    {
        UnityEngine.AI.NavMeshHit navMeshHit = new UnityEngine.AI.NavMeshHit();
        if (UnityEngine.AI.NavMesh.SamplePosition(transform.position, out navMeshHit, 10, UnityEngine.AI.NavMesh.AllAreas))
            transform.position = navMeshHit.position;

       
    }

    public void InitEffect()
    {
        ResourceManager.Instance.LoadPrefab("Common/Teleport", (resName, resGO, hash) =>
        {
            resGO.transform.SetParent(transform);
            resGO.transform.localPosition = new Vector3(0, 0.2f, 0);
            resGO.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }, null);
    }

    #endregion
}
