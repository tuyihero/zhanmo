using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// 残影效果
public class EffectCopyModelScale : EffectCopyModel
{
    public float _CopyScale = 1;
    public Vector3 _PosOffset = Vector3.zero;

    private MotionManager _ParentMotion;
    protected override void InitMesh()
    {
        List<SkinnedMeshRenderer> weaponMesh = new List<SkinnedMeshRenderer>();
        if ((SkinnedRenderers == null || SkinnedRenderers.Length == 0))
        {
            _ParentMotion = gameObject.GetComponentInParent<MotionManager>();
            var skinnedMeshes = _ParentMotion.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var skinnedMesh in skinnedMeshes)
            {
                if (skinnedMesh.name.Contains("Weapon"))
                { 
                    weaponMesh.Add(skinnedMesh);
                }
            }

            SkinnedRenderers = weaponMesh.ToArray();
        }
    }

    protected override void InitEffectObj(GameObject initObj, Transform originTransform)
    {
        initObj.transform.localScale = new Vector3(_CopyScale, _CopyScale, _CopyScale);
        initObj.transform.position = originTransform.position;
        initObj.transform.rotation = originTransform.rotation;
    }


}