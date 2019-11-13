using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletEmitterPosPair
{
    public int colliderID;
    public Vector3 pos;
}

public class BulletEmitterBasePos : MonoBehaviour
{
    public List<BulletEmitterPosPair> _EmitterPoses;

    public Vector3 GetEmitterPos(int colliderID)
    {
        if (_EmitterPoses == null || _EmitterPoses.Count == 0)
        {
            return Vector3.zero;
        }
        foreach (var emitterPos in _EmitterPoses)
        {
            if (emitterPos.colliderID == colliderID)
            {
                return emitterPos.pos;
            }
        }

        return Vector3.zero;
    }
}
