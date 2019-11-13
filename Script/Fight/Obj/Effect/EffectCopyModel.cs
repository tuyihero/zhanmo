using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// 残影效果
public class EffectCopyModel : EffectController
{
    public float _MoveSpeed = 30;
    public Material _Material;

    protected SkinnedMeshRenderer[] SkinnedRenderers;

    protected List<GameObject> _BakedMeshes = new List<GameObject>();

    void FixedUpdate()
    {
        transform.position += transform.forward * _MoveSpeed * Time.fixedDeltaTime;
    }

    public override void PlayEffect()
    {
        base.PlayEffect();
        InitMesh();
        CreateImage();
    }

    public override void PlayEffect(float speed)
    {
        base.PlayEffect();
        InitMesh();
        CreateImage();
    }

    public override void HideEffect()
    {
        foreach(var meshGO in _BakedMeshes)
        {
            GameObject.Destroy(meshGO);
        }
        _BakedMeshes.Clear();
        base.HideEffect();
    }

    protected virtual void InitMesh()
    {
        if ((SkinnedRenderers == null || SkinnedRenderers.Length == 0))
        {
            var motion = gameObject.GetComponentInParent<MotionManager>();
            SkinnedRenderers = motion.GetComponentsInChildren<SkinnedMeshRenderer>();
        }
    }

    private void CreateImage()
    {

        var materials = new List<Mesh>();

        Transform t = transform;

        for (int i = 0; i < SkinnedRenderers.Length; ++i)
        {
            var item = SkinnedRenderers[i];
            var mesh = new Mesh();
            item.BakeMesh(mesh);
            BakeMesh(mesh, item.gameObject);
        }

    }

    private void BakeMesh(Mesh mesh, GameObject originGO)
    {
        GameObject bakeMeshGo = new GameObject("BakeMesh");
        bakeMeshGo.transform.SetParent(transform);
        InitEffectObj(bakeMeshGo, originGO.transform);
        var bakeMesh = bakeMeshGo.AddComponent<MeshFilter>();
        bakeMesh.mesh = mesh;
        var meshRender = bakeMeshGo.AddComponent<MeshRenderer>();
        meshRender.materials = new Material[1] { new Material(_Material) };

        _BakedMeshes.Add(bakeMeshGo);
    }

    protected virtual void InitEffectObj(GameObject initObj, Transform originTransform)
    {
        initObj.transform.position = originTransform.position;
        initObj.transform.rotation = originTransform.rotation;
    }
}