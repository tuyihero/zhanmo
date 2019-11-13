using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// 边缘高亮
public class EffectBlockOutLine : EffectController
{
    public Material _OutLineMaterial;

    private bool _IsPlayingEffect = false;
    private float _LastTime = 0.1f;
    private float _StartPlayTime = 0;

    public override void PlayEffect()
    {
        base.PlayEffect();

        AddMaterial();
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
        base.HideEffect();

        RemoveMaterial();
        _IsPlayingEffect = false;
        _StartPlayTime = 0;
    }

    void Update()
    {
        if (_LastTime > 0)
        {
            if (_IsPlayingEffect)
            {
                var deltaTime = Time.time - _StartPlayTime;
                if (_LastTime > deltaTime)
                {
                    var deltaRate = (_LastTime - deltaTime) / _LastTime;
                    _OutLineMaterial.SetColor("_OutlineColor", new Color() { r = 1, g = 1.0f * deltaRate, b = 1.0f * deltaRate, a = 1 });
                    _OutLineMaterial.SetFloat("_Outline", 0.03f * deltaRate);
                }
                else
                {
                    HideEffect();
                }
            }
        }
    }

    #region 

    private SkinnedMeshRenderer _SkinnedMesh;

    private void AddMaterial()
    {
        var motion = gameObject.GetComponentInParent<MotionManager>();
        _SkinnedMesh = motion.GetComponentInChildren<SkinnedMeshRenderer>();
        //foreach (SkinnedMeshRenderer curMeshRender in meshes)
        {
            Material[] newMaterialArray = new Material[_SkinnedMesh.materials.Length + 1];

            newMaterialArray[0] = _OutLineMaterial;
            for (int i = 0; i < _SkinnedMesh.materials.Length; i++)
            {
                if (_SkinnedMesh.materials[i].name.Contains("Outline"))
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
        if (_SkinnedMesh == null)
            return;
        //foreach (SkinnedMeshRenderer curMeshRender in meshes)
        {
            int newMaterialArrayCount = 0;
            for (int i = 0; i < _SkinnedMesh.materials.Length; i++)
            {
                if (_SkinnedMesh.materials[i].name.Contains("Outline"))
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
                    if (!_SkinnedMesh.materials[i].name.Contains("Outline"))
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