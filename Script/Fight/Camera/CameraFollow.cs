using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public float _MaxMoveSpeed = 6.0f;
    public Transform _Black1;
    public Transform _Black2;
    public SceneAnimController _SceneAnimController;
    public Camera _ControlCamera;
    public GameObject _FollowObj;
    public List<float> _SwitchZ = new List<float>() { -3.5f, -5.0f };

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

    
    private float _MinPosY = 0, _MaxPosY = 0;
    
    // Start is called before the first frame update
    void InitSceneCamera2D()
    {
        _Black1.localPosition = new Vector3(64 * 0.2f + 5.12f, 0, 0);
        _Black2.localPosition = -_Black1.position;
    }

    void UpdateCamera()
    {
        if (_SceneAnimController == null || _FollowObj == null)
            return;

        float posX = _FollowObj.transform.position.x;
        if (posX - _ControlCamera.transform.position.x > _MaxMoveSpeed * Time.deltaTime)
        {
            posX = _ControlCamera.transform.position.x + _MaxMoveSpeed * Time.deltaTime;
        }
        else if (posX - _ControlCamera.transform.position.x < -_MaxMoveSpeed * Time.deltaTime)
        {
            posX = _ControlCamera.transform.position.x - _MaxMoveSpeed * Time.deltaTime;
        }

        posX = Mathf.Clamp(posX, MinPosX, MaxPosX);
        if (_LookPosX > 0)
        {
            posX = Mathf.Clamp(posX, _CameraLookXMin, _CameraLookXMax);
        }

        float step = (_FollowObj.transform.position.z - _SceneAnimController.SceneZMin) / (_SceneAnimController.SceneZMax - _SceneAnimController.SceneZMin);
        //step = Mathf.Clamp((step - 0.5f) * 2, 0, 1);
        step = Mathf.Clamp((step - 0.5f), 0, 1);
        float cameraMoveY = step * (_MaxPosY - _MinPosY) + _MinPosY;
        _ControlCamera.transform.position = _ControlCamera.transform.up * cameraMoveY;
        _ControlCamera.transform.position += _ControlCamera.transform.forward * -100;

        _ControlCamera.transform.position = new Vector3(posX, _ControlCamera.transform.position.y, _ControlCamera.transform.position.z);


        _SceneAnimController.UpdateFarPos((posX - MinPosX) / (MaxPosX - MinPosX));
    }

    public void SetSceneAnim(SceneAnimController sceneAnimController)
    {
        _SceneAnimController = sceneAnimController;
        transform.transform.position = _SceneAnimController.transform.position;
        transform.rotation = _SceneAnimController.transform.rotation;
        transform.transform.position += transform.transform.up * -(_ControlCamera.orthographicSize * 2 * _SceneAnimController.transform.localScale.y - _ControlCamera.orthographicSize);
        transform.transform.position += transform.transform.forward * -100;

        _MaxPosY = -_ControlCamera.orthographicSize;
        _MinPosY = -(_ControlCamera.orthographicSize * 2 * _SceneAnimController.transform.localScale.y - _ControlCamera.orthographicSize);



        ResetLookPos();
    }
    #endregion

    #region look

    private float _LookPosX;
    private float _CameraLookXMin, _CameraLookXMax;
    private float _MainMovePosXMin, _MainMovePosXMax;

    public float MainMovePosXMin
    {
        get
        {
            return _MainMovePosXMin;
        }
    }

    public float MainMovePosXMax
    {
        get
        {
            return _MainMovePosXMax;
        }
    }

    public void SetLookPosX(float lookPosX)
    {
        
        _LookPosX = lookPosX;
        if (_LookPosX < 0)
        {
            _CameraLookXMin = MinPosX;
            _CameraLookXMax = MaxPosX;

            _MainMovePosXMin = 0.5f;
            _MainMovePosXMax = MaxPosX + MinPosX - 0.5f;
            return;
        }
        if (_LookPosX < MinPosX)
        {
            _LookPosX = MinPosX;
        }

        if (MinPosX >= 2.56f)
        {
            _CameraLookXMin = _LookPosX;
            _CameraLookXMax = _LookPosX;

            _MainMovePosXMin = _LookPosX - MinPosX + 0.5f;
            _MainMovePosXMax = _LookPosX + MinPosX - 0.5f;
        }
        else
        {
            _CameraLookXMin = _LookPosX- (2.56f - MinPosX);
            _CameraLookXMax = _LookPosX + (2.56f - MinPosX);

            _MainMovePosXMin = _LookPosX - 2.56f + 0.5f;
            _MainMovePosXMax = _LookPosX + 2.56f - 0.5f;
        }
    }

    public void ResetLookPos()
    {
        SetLookPosX(0);
    }

    #endregion
}
