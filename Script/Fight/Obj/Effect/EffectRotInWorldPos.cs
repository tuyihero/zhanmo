using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRotInWorldPos : MonoBehaviour
{
    public Vector3 _RotSpeed;
    private Vector3 _WorldRotEuler;
	
	void Update ()
    {
        _WorldRotEuler += _RotSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(_WorldRotEuler);
	}
}
