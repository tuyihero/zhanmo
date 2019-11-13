using UnityEngine;
using System.Collections;

public class ImpactResumeHP: ImpactBase
{
    public int _HPValue = 0;
    public float _HPPersent = 0;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        if (reciverManager.IsMotionDie)
            return;

        reciverManager.RoleAttrManager.AddHP(_HPValue);
        reciverManager.RoleAttrManager.AddHPPersent(_HPPersent);
        ActEffect();
    }

    #region act effect

    public EffectController _ActEffect;
    public int _DynamicActEffect;

    private void ActEffect()
    {
        _ActEffect._EffectLastTime = 1.0f;
        if (_ActEffect != null)
        {
            _DynamicActEffect = ReciveMotion.PlayDynamicEffect(_ActEffect);
        }
    }

    #endregion
}
