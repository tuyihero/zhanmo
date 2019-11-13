using UnityEngine;
/// <summary>
/// 在Start或者主动调用时，将特效的尺寸按缩放为1的比例对齐
/// </summary>
public class UIParticleScaler : MonoBehaviour
{
    public bool applyScale;
    private ParticleSystemData[] particleData;
    private void Awake()
    {
        var particleSystems = GetComponentsInChildren<ParticleSystem>();
        particleData = new ParticleSystemData[particleSystems.Length];
        if (particleSystems.Length > 0)
        {
            for (int i = 0; i < particleData.Length; i++)
                particleData[i] = new ParticleSystemData(particleSystems[i]);
        }
        else
            Debug.LogError(string.Format("物体{0}的粒子缩放器无法找到粒子系统", gameObject.name));
    }
    private void Start()
    {
        MatchCurrentSize();
    }
    private void Update()
    {
        if (applyScale)
        {
            applyScale = false;
            MatchCurrentSize();
        }
    }
    public void MatchCurrentSize()
    {
        for (int i = 0; i < particleData.Length; i++)
            particleData[i].ApplyScale();
    }
}

public class ParticleSystemData
{
    public ParticleSystem particleSystem;
    private readonly float startSize;
    private readonly float startSpeed;
    private readonly float radius;
    private readonly float length;
    private readonly float normalOffset;
    private readonly Vector3 box;

    public ParticleSystemData(ParticleSystem particleSystem)
    {
        var transform = particleSystem.transform;
        float localSizeInvert = transform.localScale.y / transform.lossyScale.y;
        this.particleSystem = particleSystem;
        this.startSize = particleSystem.startSize * localSizeInvert;
        this.startSpeed = particleSystem.startSpeed * localSizeInvert;
        var shapeModule = particleSystem.shape;
        this.radius = shapeModule.radius * localSizeInvert;
        this.length = shapeModule.length * localSizeInvert;
        this.normalOffset = shapeModule.normalOffset * localSizeInvert;
        this.box = shapeModule.scale * localSizeInvert;
    }
    public void ApplyScale()
    {
        var transform = particleSystem.transform;
        float ratio = 0;
        if (particleSystem.transform.lossyScale.y != 0)
            ratio = transform.lossyScale.y / transform.localScale.y;
        particleSystem.startSize = startSize * ratio;
        particleSystem.startSpeed = startSpeed * ratio;
        var shapeModule = particleSystem.shape;
        shapeModule.radius = radius * ratio;
        shapeModule.length = length * ratio;
        shapeModule.normalOffset = normalOffset * ratio;
        shapeModule.scale = box * ratio;
    }
}