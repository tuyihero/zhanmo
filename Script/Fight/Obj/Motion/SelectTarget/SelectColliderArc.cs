using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectColliderArc : SelectCollider
{
    public float _Angle;

    void OnTriggerStay(Collider other)
    {
        var motion = other.gameObject.GetComponentInParent<MotionManager>();
        if (motion == null)
            return;

        float targetAngle = Mathf.Abs(Vector3.Angle(motion.transform.position - _ObjMotion.transform.position, _ObjMotion.transform.forward));
        if (targetAngle > _Angle)
            return;

        TriggerMotion(motion);
    }

}
