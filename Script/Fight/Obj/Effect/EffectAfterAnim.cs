using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// 残影效果
public class EffectAfterAnim : EffectController
{
    class AfterImage
    {
        public Mesh mesh;
        public Material material;
        public Matrix4x4 matrix;
        public float showStartTime;
        public float duration;  // 残影镜像存在时间
        public float alpha;
        public bool needRemove = false;
    }

    public float _Duration;
    public float _Interval;
    public float _FadeOut;

    private List<AfterImage> _imageList = new List<AfterImage>();
    
    public MeshRenderer[] _MeshRenderers;
    public SkinnedMeshRenderer[] SkinnedRenderers;
    public Material _Material;

    public override void PlayEffect()
    {
        base.PlayEffect();
        InitMesh();
        StartCoroutine(DoAddImage());
    }

    public override void PlayEffect(float speed)
    {
        base.PlayEffect();
        InitMesh();
        StartCoroutine(DoAddImage());
    }

    public override void HideEffect()
    {
        _imageList.Clear();
        base.HideEffect();
    }

    private void InitMesh()
    {
        if (_MeshRenderers.Length == 0 || SkinnedRenderers.Length == 0)
        {
            var motion = gameObject.GetComponentInParent<MotionManager>();
            //_MeshRenderers = motion.GetComponentsInChildren<MeshRenderer>();
            SkinnedRenderers = motion.GetComponentsInChildren<SkinnedMeshRenderer>();
        }
    }

    IEnumerator DoAddImage()
    {
        float startTime = Time.time;
        while (true)
        {
            CreateImage();

            if (Time.time - startTime > _Duration)
            {
                break;
            }

            yield return new WaitForSeconds(_Interval);
        }
    }

    private void CreateImage()
    {

        CombineInstance[] combineInstances = new CombineInstance[_MeshRenderers.Length + SkinnedRenderers.Length];

        Transform t = transform;
        Material mat = null;
        
        for (int i = 0; i < _MeshRenderers.Length; ++i)
        {
            var item = _MeshRenderers[i];
            t = item.transform;
            mat = new Material(_Material);
            //mat.shader = _shaderAfterImage;

            var mesh = GameObject.Instantiate<Mesh>(item.GetComponent<MeshFilter>().mesh);
            combineInstances[SkinnedRenderers.Length + i] = new CombineInstance
            {
                mesh = mesh,
                subMeshIndex = 0,
            };
        }
        for (int i = 0; i < SkinnedRenderers.Length; ++i)
        {
            var item = SkinnedRenderers[i];
            t = item.transform;
            mat = new Material(_Material);
            //mat.shader = _shaderAfterImage;

            var mesh = new Mesh();
            item.BakeMesh(mesh);
            combineInstances[i] = new CombineInstance
            {
                mesh = mesh,
                subMeshIndex = 0,
                transform = t.localToWorldMatrix,
            };
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineInstances, true, false);

        _imageList.Add(new AfterImage
        {
            mesh = combinedMesh,
            material = mat,
            matrix = t.localToWorldMatrix,
            showStartTime = Time.time,
            duration = _FadeOut,
        });
    }

    private void DrawMesh(Mesh trailMesh, Material trailMaterial)
    {
        Graphics.DrawMesh(trailMesh, Matrix4x4.identity, trailMaterial, gameObject.layer);
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        bool hasRemove = false;
        foreach (var item in _imageList)
        {
            float time = Time.time - item.showStartTime;

            if (time > item.duration)
            {
                item.needRemove = true;
                hasRemove = true;
                continue;
            }

            //if (item.material.HasProperty("_Color"))
            //{
            //    item.alpha = Mathf.Max(0, 1 - time / item.duration);
            //    Color color = item.material.GetColor("_Color");
            //    color.a = item.alpha;
            //    item.material.SetColor("_Color", color);
            //}

            Graphics.DrawMesh(item.mesh, item.matrix, item.material, gameObject.layer);
            //Graphics.DrawMesh(item.mesh, Matrix4x4.identity, item.material, gameObject.layer);
        }

        if (hasRemove)
        {
            _imageList.RemoveAll(x => x.needRemove);
        }
    }
}