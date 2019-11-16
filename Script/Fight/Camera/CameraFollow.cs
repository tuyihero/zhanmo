using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

	// Use this for initialization
	void Start () {
        InitSceneCamera2D();

    }
	
	// Update is called once per frame
	void Update () {
        UpdateCamera();
    }

    #region
    public float _Speed = 1.0f;
    public Transform _Black1;
    public Transform _Black2;
    public SceneAnimController _SceneAnimController;
    public Camera _ControlCamera;
    public GameObject _FollowObj;

    private float _MinPosX = 0, _MaxPosX = 0;
    public float MinPosX
    {
        get
        {
            if (_MinPosX <= 0)
            {
                _MinPosX = (float)Screen.width / Screen.height * _ControlCamera.orthographicSize;
                _MinPosX = Mathf.Min(_MinPosX, 5.12f);
            }
            return _MinPosX;
        }
    }
    public float MaxPosX
    {
        get
        {
            if (_MaxPosX <= 0)
            {
                _MaxPosX = _SceneAnimController._GroundLength / 100 - MinPosX;
            }
            return _MaxPosX;
        }
    }
    // Start is called before the first frame update
    void InitSceneCamera2D()
    {
        _Black1.position = new Vector3(64 * 0.2f + 5.12f, 0, 0);
        _Black2.position = -_Black1.position;
    }

    void UpdateCamera()
    {
        if (_SceneAnimController == null || _FollowObj == null)
            return;

        float posX = _FollowObj.transform.position.x;
        posX = Mathf.Clamp(posX, MinPosX, MaxPosX);
        _ControlCamera.transform.position = new Vector3(posX, _ControlCamera.transform.position.y, -10);

        _SceneAnimController.UpdateFarPos((posX - MinPosX) / (MaxPosX - MinPosX));
    }

    public void SetSceneAnim(SceneAnimController sceneAnimController)
    {
        _SceneAnimController = sceneAnimController;
    }
    #endregion
}
