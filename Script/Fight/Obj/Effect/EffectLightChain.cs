using UnityEngine;
using System.Collections;

public class EffectLightChain : EffectController
{

    public ParticleSystem _LightParticleSys;
    public float _LightLength;

    public override void PlayEffect(Hashtable hash)
    {
        base.PlayEffect(hash);

        Vector3 posStart = (Vector3)hash["PosStart"];
        Vector3 posEnd = (Vector3)hash["PosEnd"];

        float distance = Vector3.Distance(posStart, posEnd);
        float scale = distance / _LightLength;

        _LightParticleSys.transform.localScale = new Vector3(scale, 1, 1);
        transform.position = posStart + (posEnd - posStart) * 0.5f;
        transform.rotation = Quaternion.LookRotation(posStart - posEnd);
    }
	
}
