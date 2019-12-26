using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// 残影效果
public class EffectAfterAnim2D : EffectAfterAnim
{
    class AfterImage2D
    {
        public Sprite sprite;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public float showStartTime;
        public float duration;  // 残影镜像存在时间
        public float alpha;
        public bool needRemove = false;
        public GameObject imageGO;
    }

    private List<AfterImage2D> _imageList = new List<AfterImage2D>();

    public SpriteRenderer _MotionSprite;

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
        foreach (var image in _imageList)
        {
            GameObject.Destroy(image.imageGO);
        }
        _imageList.Clear();
        base.HideEffect();
    }

    private void InitMesh()
    {
        if (_MotionSprite == null)
        {
            var motion = gameObject.GetComponentInParent<MotionManager>();
            _MotionSprite = motion.Animation.gameObject.GetComponentInChildren<SpriteRenderer>();
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
        _imageList.Add(new AfterImage2D
        {
            sprite = _MotionSprite.sprite,
            position = new Vector3(_MotionSprite.transform.position.x, _MotionSprite.transform.position.y, _MotionSprite.transform.position.z + 0.01f),
            rotation = _MotionSprite.transform.rotation.eulerAngles,
            scale = _MotionSprite.transform.lossyScale,
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
                GameObject.Destroy(item.imageGO);
                continue;
            }

            if (item.imageGO == null)
            {
                item.imageGO = new GameObject("shadowImage");
                item.imageGO.transform.SetParent(transform);

                var spriterender = item.imageGO.AddComponent<SpriteRenderer>();
                spriterender.sprite = item.sprite;
                spriterender.color = new Color(1, 1, 1, 0.7f);
                item.imageGO.transform.position = item.position;
                item.imageGO.transform.rotation = Quaternion.Euler(item.rotation);
                item.imageGO.transform.localScale = item.scale;
            }
        }

        if (hasRemove)
        {

            _imageList.RemoveAll(x => x.needRemove);
        }
    }
}