using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        UpdatePos();
    }

    #region 

    public GameObject _FollowObj;
    //public Vector3 _Distance;

    public void UpdatePos()
    {
        if (_FollowObj == null)
            return;

        transform.position = _FollowObj.transform.position + new Vector3(0,0,-10);
        //transform.LookAt(_FollowObj.transform);
    }

    #endregion
}
