using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExImpactBase
{

    public ImpactBase _ExImpact;
    public float _CD;
    public int _ActRate;

    private float _LastActTime;

    public void ActExImpact(MotionManager senderMotion, MotionManager reciveMotion)
    {
        if (Time.time - _LastActTime < _CD)
            return;

        var randomVal = Random.Range(0, 10000);
        if (randomVal > _ActRate)
            return;

        _LastActTime = Time.time;
        _ExImpact.ActImpact(senderMotion, reciveMotion);
    }
    
    
}
