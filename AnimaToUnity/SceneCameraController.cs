using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCameraController : MonoBehaviour
{
    public float _Speed = 1.0f;
    public Transform _Black1;
    public Transform _Black2;
    public SceneAnimController _SceneAnimController;
    public Camera _ControlCamera;

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
    void Start()
    {
        _Black1.position = new Vector3(64*0.2f+ 5.12f, 0, 0);
        _Black2.position = -_Black1.position;
    }

    void Update()
    {
        if (_SceneAnimController == null)
            return;

        var axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float posX = transform.position.x + axis.x * _Speed;
        posX = Mathf.Clamp(posX, MinPosX, MaxPosX);
        _ControlCamera.transform.position = new Vector3(posX, _ControlCamera.transform.position.y, _ControlCamera.transform.position.z);

        _SceneAnimController.UpdateFarPos((posX - MinPosX)/ (MaxPosX - MinPosX));
    }

    public void SetSceneAnim(SceneAnimController sceneAnimController)
    {
        _SceneAnimController = sceneAnimController;
    }
}
