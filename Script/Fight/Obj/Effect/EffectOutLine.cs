using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// 边缘高亮
public class EffectOutLine : EffectController
{
    public Material _OutLineMaterial;

    public override void PlayEffect()
    {
        base.PlayEffect();

        AddMaterial();
    }

    public override void PlayEffect(float speed)
    {
        base.PlayEffect(speed);

        AddMaterial();
    }

    public override void HideEffect()
    {
        base.HideEffect();

        if (_SkinnedMesh != null)
        {
            RemoveMaterial();
            
        }
    }

    public void Update()
    {
        UpdateMatAnim();
    }

    #region 

    private SkinnedMeshRenderer _SkinnedMesh;
    private static Dictionary<SkinnedMeshRenderer, int> _RenderMatCnt = new Dictionary<SkinnedMeshRenderer, int>();
    public Material _MatInstance;

    private UnityEngine.Rendering.ShadowCastingMode _OrgShadowMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    private void AddMaterial()
    {
        if (_SkinnedMesh == null)
        {
            var motion = gameObject.GetComponentInParent<MotionManager>();

            var go = transform.Find("ForCharRim");
            if (go != null)
            {
                _SkinnedMesh = gameObject.GetComponent<SkinnedMeshRenderer>();
                if (_SkinnedMesh.shadowCastingMode != UnityEngine.Rendering.ShadowCastingMode.Off)
                {
                    _OrgShadowMode = _SkinnedMesh.shadowCastingMode;
                }
                //_SkinnedMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
            else
            {
                var objMeshes = motion.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (var objMesh in objMeshes)
                {
                    if (objMesh.name.Contains("Weapon"))
                        continue;

                    _SkinnedMesh = objMesh;
                    if (_SkinnedMesh.shadowCastingMode != UnityEngine.Rendering.ShadowCastingMode.Off)
                    {
                        _OrgShadowMode = _SkinnedMesh.shadowCastingMode;
                    }
                    //_SkinnedMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
            }
            //_SkinnedMesh = motion.GetComponentInChildren<SkinnedMeshRenderer>();
        }

        var matCnt = AddMatCnt();
        if (matCnt > 1)
            return;
        //foreach(SkinnedMeshRenderer curMeshRender in meshes)
        {
            if (_MatInstance == null)
            {
                _MatInstance = GameObject.Instantiate<Material>(_OutLineMaterial);
            }
            Material[] newMaterialArray = new Material[_SkinnedMesh.materials.Length + 1];
            newMaterialArray[0] = _MatInstance;
            for (int i = 1; i < _SkinnedMesh.materials.Length + 1; i++)
            {
                //if (_SkinnedMesh.materials[i].name.Contains("Outline"))
                //{
                //    return;
                //}
                //else
                {
                    newMaterialArray[i] = _SkinnedMesh.materials[i - 1];
                    newMaterialArray[i].SetInt("_ZWrite", 1);
                    newMaterialArray[i].renderQueue = 2450;
                }
            }

            _SkinnedMesh.materials = newMaterialArray;
        }
    }

    private void RemoveMaterial()
    {
        var matHandleCnt = DecMatCnt();

        if (matHandleCnt > 0)
            return;

        //foreach (SkinnedMeshRenderer curMeshRender in meshes)
        {
            int newMaterialArrayCount = 0;
            for (int i = 0; i < _SkinnedMesh.materials.Length; i++)
            {
                if (!_SkinnedMesh.materials[i].name.Contains("Outline"))
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
                _SkinnedMesh.shadowCastingMode = _OrgShadowMode;
            }

            _SkinnedMesh = null;
        }
    }

    private int AddMatCnt()
    {
        if (!_RenderMatCnt.ContainsKey(_SkinnedMesh))
        {
            _RenderMatCnt.Add(_SkinnedMesh, 0);
        }

        ++_RenderMatCnt[_SkinnedMesh];
        
        return _RenderMatCnt[_SkinnedMesh];
    }

    private int DecMatCnt()
    {
        if (_SkinnedMesh == null || !_RenderMatCnt.ContainsKey(_SkinnedMesh))
        {
            return 0;
        }

        --_RenderMatCnt[_SkinnedMesh];
        return _RenderMatCnt[_SkinnedMesh];
    }

    #endregion

    #region mat anim

    public Animator _Animator;
    public Color _MatColor;
    public float _MatWidthRate;
    public float _MatBaseWidth;
    
    private void UpdateMatAnim()
    {
        if (_MatInstance == null)
            return;

        _MatInstance.SetColor("_OutlineColor", _MatColor);
        _MatInstance.SetFloat("_Outline", _MatWidthRate * _MatBaseWidth);
    }

    public void PlayHitted()
    {
        _Animator.Play("EffectOutlineAnimHitted");
    }

    #endregion
}