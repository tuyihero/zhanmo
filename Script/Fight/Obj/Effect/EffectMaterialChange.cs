using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EffectMaterialChange : EffectController
{
    public Material _ChangeMaterial;

    private bool _IsPlayingEffect = false;
    private float _LastTime = 0.1f;
    private float _StartPlayTime = 0;

    public override void PlayEffect()
    {
        //base.PlayEffect();

        //AddMaterial();
        //_IsPlayingEffect = true;
        //_StartPlayTime = Time.time;
    }

    public override void PlayEffect(float speed)
    {
        //base.PlayEffect(speed);

        //PlayEffect();
    }

    public override void HideEffect()
    {
        //base.HideEffect();

        //Debug.Log("EffectMaterialChange HideEffect");
        //RemoveMaterial();
        //_IsPlayingEffect = false;
        //_StartPlayTime = 0;
    }

    #region 

    private SkinnedMeshRenderer _SkinnedMesh;

    private void AddMaterial()
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
            Material[] newMaterialArray = new Material[_SkinnedMesh.materials.Length + 1];

            newMaterialArray[0] = _ChangeMaterial;
            for (int i = 0; i < _SkinnedMesh.materials.Length; i++)
            {
                if (_SkinnedMesh.materials[i].name.Contains(_ChangeMaterial.name))
                {
                    return;
                }
                else
                {
                    newMaterialArray[i + 1] = _SkinnedMesh.materials[i];
                    newMaterialArray[i + 1].SetInt("_ZWrite", 1);
                    newMaterialArray[i + 1].renderQueue = 2450;
                }
            }

            _SkinnedMesh.materials = newMaterialArray;

        }
    }

    private void RemoveMaterial()
    {
        //var motion = gameObject.GetComponentInParent<MotionManager>();
        //_SkinnedMesh = motion.GetComponentInChildren<SkinnedMeshRenderer>();
        //foreach (SkinnedMeshRenderer curMeshRender in meshes)
        {
            int newMaterialArrayCount = 0;
            for (int i = 0; i < _SkinnedMesh.materials.Length; i++)
            {
                if (_SkinnedMesh.materials[i].name.Contains(_ChangeMaterial.name))
                {
                    newMaterialArrayCount++;
                }
            }

            if (newMaterialArrayCount > 0)
            {
                Material[] newMaterialArray = new Material[newMaterialArrayCount];
                int curMaterialIndex = 0;
                for (int i = 0; i < _SkinnedMesh.materials.Length; i++)
                {
                    if (curMaterialIndex >= newMaterialArrayCount)
                    {
                        break;
                    }
                    if (!_SkinnedMesh.materials[i].name.Contains(_ChangeMaterial.name))
                    {
                        newMaterialArray[curMaterialIndex] = _SkinnedMesh.materials[i];
                        curMaterialIndex++;
                    }
                }

                _SkinnedMesh.materials = newMaterialArray;
            }
        }
    }

    #endregion
}