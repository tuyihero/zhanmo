using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDie : StateBase
{
    public override void StartState(params object[] args)
    {
        _MotionManager.StartCoroutine(MotionDie());

        if (_MotionManager._DeadAudio != null)
        {
            _MotionManager.PlayAudio(_MotionManager._DeadAudio);
        }
    }

    #region 

    private static float _BodyDisappearTime = 0.8f;

    private IEnumerator MotionDie()
    {
        _MotionManager.MotionCorpse();

        yield return new WaitForSeconds(_BodyDisappearTime);

        _MotionManager.MotionDisappear();
    }

    #endregion

}
