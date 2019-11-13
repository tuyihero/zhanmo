using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EffectChangeMainMaterial : EffectController
{
    public Material _ChangeMaterial;

    private bool _IsPlayingEffect = false;
    private float _StartPlayTime = 0;

    public override void PlayEffect()
    {
        if (_IsPlayingEffect)
            return;

        base.PlayEffect();
        
        ChangeMaterial();
        _IsPlayingEffect = true;
        _StartPlayTime = Time.time;
    }

    public override void PlayEffect(float speed)
    {
        base.PlayEffect(speed);

        PlayEffect();
    }

    public override void HideEffect()
    {
        if (!_IsPlayingEffect)
            return;

        base.HideEffect();

        RemoveMaterial();
        _IsPlayingEffect = false;
        _StartPlayTime = 0;
    }

    #region 

    private SkinnedMeshRenderer _SkinnedMesh;
    private Material _OrgMaterial;

    private void ChangeMaterial()
    {
        var motion = gameObject.GetComponentInParent<MotionManager>();
        var skinnedMeshes = motion.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var skinnedMesh in skinnedMeshes)
        {
            if (!skinnedMesh.name.Contains("Weapon"))
            {
                _SkinnedMesh = skinnedMesh;
            }
        }
        //foreach (SkinnedMeshRenderer curMeshRender in meshes)
        {
            _OrgMaterial = _SkinnedMesh.materials[0];
            List<Material> materials = new List<Material>(_SkinnedMesh.materials);
            materials.RemoveAt(0);
            materials.Insert(0, _ChangeMaterial);
            _SkinnedMesh.materials = materials.ToArray();
            _SkinnedMesh.materials[0].SetTexture("_MainTex", _OrgMaterial.GetTexture("_MainTex"));
            Debug.Log("AddMat:" + _SkinnedMesh.materials[0].name);
        }
    }

    private void RemoveMaterial()
    {
        //var motion = gameObject.GetComponentInParent<MotionManager>();
        //_SkinnedMesh = motion.GetComponentInChildren<SkinnedMeshRenderer>();
        //foreach (SkinnedMeshRenderer curMeshRender in meshes)
        {
            List<Material> materials = new List<Material>(_SkinnedMesh.materials);
            materials.RemoveAt(0);
            materials.Insert(0, _OrgMaterial);
            _SkinnedMesh.materials = materials.ToArray();

            Debug.Log("RemMat:" + _SkinnedMesh.materials[0].name);
        }
    }

    #endregion
}