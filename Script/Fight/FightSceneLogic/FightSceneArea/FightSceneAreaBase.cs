using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class SerializeEnemyInfo
{
    public Transform _EnemyTransform;
    public string _EnemyDataID;
}

public enum AreaState
{
    None,
    Wait,
    Acting,
    Hiding,
    Finished,
}

public class FightSceneAreaBase : MonoBehaviour
{
    public int AreaID { get; set; }

    protected AreaState _AreaState = AreaState.None;

    public AreaState AreaState
    {
        get
        {
            return _AreaState;
        }
    }
    
    public virtual void InitArea()
    { }

    public virtual void StartArea()
    {
        _AreaState = AreaState.Acting;
        UpdateEnemyAlert();
        InitFightLogic();
        CloseAllDoor();
    }

    public virtual void FinishArea()
    {
        _AreaState = AreaState.Finished;
        OpenAllDoor();
        _FightSceneLogic.AreaFinish(this);
    }

    public virtual void UpdateArea()
    {
        UpdateEnemyAlert();
    }

    public virtual void MotionDie(MotionManager motion)
    {
        

    }

    #region FightSceneLogic

    public FightSceneLogicPassArea _FightSceneLogic;

    private void InitFightLogic()
    {
        _FightSceneLogic = GetComponentInParent<FightSceneLogicPassArea>();
    }

    #endregion

    #region collider

    public enum TrigType
    {
        TRIG_AUTO,
        TRIG_EVENT,
    }

    protected float _EnemyAlertDistance = 8;
    public GameObject[] _AreaDoors;
    public TrigType _TrigAreaType;

    protected bool _IsEnemyAlert = false;

    private void UpdateEnemyAlert()
    {
        if (_IsEnemyAlert)
            return;

        if (_EnemyAlertDistance <= 0)
        {
            _IsEnemyAlert = true;
            return;
        }
    }

    private void CloseAllDoor()
    {
        foreach (var door in _AreaDoors)
        {
            door.SetActive(true);
        }
    }

    private void OpenAllDoor()
    {
        foreach (var door in _AreaDoors)
        {
            door.SetActive(false);
        }
    }
    #endregion

    public virtual Transform GetAreaTransform()
    {
        return null;
    }

    public virtual List<string> GetAreaMonIDs()
    {
        return null;
    }
}
