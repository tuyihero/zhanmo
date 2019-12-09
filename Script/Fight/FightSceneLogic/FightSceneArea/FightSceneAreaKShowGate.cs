using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightSceneAreaKShowGate : FightSceneAreaBase
{
    public override void InitArea()
    {
        base.InitArea();
    }

    public override void StartArea()
    {
        base.StartArea();
    }

    public override void UpdateArea()
    {
        base.UpdateArea();

        if (FightManager.Instance.MainChatMotion.transform.position.x > _GatePosX.x
            && FightManager.Instance.MainChatMotion.transform.position.x < _GatePosX.y)
        {
            FinishArea();
        }
    }

    #region gate area

    public Vector2 _GatePosX;

    #endregion
}
