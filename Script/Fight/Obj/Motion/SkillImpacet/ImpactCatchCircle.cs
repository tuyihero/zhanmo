using UnityEngine;
using System.Collections;

public class ImpactCatchCircle : ImpactCatch
{
    public float _Range = 1.5f;
    public float _Speed = 10;

    #region circle

    private SelectColliderCircle _SelectCircle;

    void Start()
    {
        _SelectCircle = gameObject.GetComponentInParent<SelectColliderCircle>();
    }

    public void Update()
    {
        if (_SelectCircle == null)
            return;

        if (_SelectCircle.Collider == null || _SelectCircle.Collider.enabled == false)
            return;

        if (_SelectCircle.TrigMotions == null)
            return;

        Vector3 hitPos = _SelectCircle.transform.position + _SelectCircle.transform.forward.normalized * _Range;

        //var testGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //testGO.transform.position = hitPos;

        foreach (var trigMotion in _SelectCircle.TrigMotions)
        {
            
            Vector3 distance = hitPos - trigMotion.transform.position;

            float moveTime = distance.magnitude / _Speed;

            if (moveTime > 0.01f)
            {
                Vector3 destMove = (hitPos - trigMotion.transform.position).normalized * _Speed * moveTime;
                CatchMotion(SenderMotion, trigMotion, destMove, moveTime);
            }
        }
    }

    #endregion


}
