using UnityEngine;
using System.Collections;

public class ImpactBuffAttrAdd : ImpactBuff
{
    #region 

    [System.Serializable]
    public enum ADDTYPE
    {
        Value,
        Persent
    }

    #endregion

    public RoleAttrEnum _Attr;
    public float _AddValue;
    public ADDTYPE _AddType;

    private float _ExAddValue = 0;
    public float ExAddValue
    {
        get
        {
            return _ExAddValue;
        }
        set
        {
            _ExAddValue = value;
        }
    }

    private RoleAttrEnum _ModifiedAttr = RoleAttrEnum.None;
    public RoleAttrEnum ModifiedAttr
    {
        get
        {
            return _ModifiedAttr;
        }
        set
        {
            _ModifiedAttr = value;
        }
    }

    private int _RealAddValue;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        var targetAttr = _Attr;
        if (_ModifiedAttr != RoleAttrEnum.None)
        {
            targetAttr = _ModifiedAttr;
        }

        int originValue = reciverManager.RoleAttrManager.GetBaseAttr(targetAttr);
        int value = 0;
        if (_AddType == ADDTYPE.Value)
        {
            value = (int)(originValue + _AddValue + ExAddValue);
        }
        else if (_AddType == ADDTYPE.Persent)
        {
            value = (int)(originValue * (1 + _AddValue + ExAddValue));
        }

        reciverManager.RoleAttrManager.SetBaseAttr(targetAttr, value);
        _RealAddValue = reciverManager.RoleAttrManager.GetBaseAttr(targetAttr) - originValue;
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        base.RemoveBuff(reciverManager);

        var targetAttr = _Attr;
        if (_ModifiedAttr != RoleAttrEnum.None)
        {
            targetAttr = _ModifiedAttr;
        }

        var value = reciverManager.RoleAttrManager.GetBaseAttr(targetAttr) - _RealAddValue;
        reciverManager.RoleAttrManager.SetBaseAttr(targetAttr, value);
    }
}
