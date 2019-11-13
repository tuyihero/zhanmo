using UnityEngine;
using System.Collections;

public class ImpactBuffDebuff : ImpactBuffAttrAdd
{
    private int _MaxStack = 1;
    public int MaxStack
    {
        get
        {
            return _MaxStack;
        }
        set
        {
            _MaxStack = value;
        }
    }

    private int _Stack = 1;

    public override bool IsCanAddBuff(ImpactBuff newBuff)
    {
        //if (newBuff.GetType() == this.GetType())
        //{
        //    if (_Stack < MaxStack)
        //        return true;
        //    else
        //    {
        //        ImpactFlyAway impact = new ImpactFlyAway();
        //        impact._FlyHeight = 1;
        //        impact._Time = 0.5f;
        //        impact._Speed = 10;
        //        impact._DamageRate = 0;
        //        impact.ActImpact(_BuffSender, _BuffOwner);
        //        return false;
        //    }
        //}
        return true;
    }
}
