using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// 残影效果
public class EffectCopyWeaponToParticle : MonoBehaviour
{
    protected Mesh WeaponMesh;
    protected List<GameObject> _BakedMeshes = new List<GameObject>();

    void OnEnable()
    {
        StartCoroutine(InitDelay());
    }

    private IEnumerator InitDelay()
    {
        yield return new WaitForSeconds(0.5f);

        InitMesh();
        CreateImage();
    }

    protected virtual void InitMesh()
    {
        if (WeaponMesh == null)
        {
            var motion = gameObject.GetComponentInParent<BulletBase>().SkillMotion;
            var skinnedMeshes = motion.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var skinnedMesh in skinnedMeshes)
            {
                if (skinnedMesh.name.Contains("Weapon"))
                {
                    WeaponMesh = skinnedMesh.sharedMesh;
                }
            }
        }
    }

    private void CreateImage()
    {
        if (WeaponMesh == null)
            return;

        ParticleSystemRenderer particleSys = gameObject.GetComponent<ParticleSystemRenderer>();
        if (particleSys != null)
        {
            particleSys.mesh = WeaponMesh;
        }
    }
}