using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICameraCopy : MonoBehaviour
{

    #region static

    public static int _ControllingCamera;
    private static RenderTexture _CameraTexture2D;
    public static int _CameraCullingMask;

    #endregion

    public RawImage _BackImage;

    public void OnEnable()
    {
        if (_ControllingCamera == 0)
        {
            //StartCoroutine(ControllerCamera());
            SetCameraCulling();
        }
        ++_ControllingCamera;
    }

    public void OnDisable()
    {
        --_ControllingCamera;
        if (_ControllingCamera == 0)
        {
            //ReleaseCamera();
            ResetCameraCulling();
        }
    }

    #region copy

    private IEnumerator ControllerCamera()
    {
        if (_CameraTexture2D == null)
        {
            _CameraTexture2D = new RenderTexture((int)(Screen.width * 0.5f), (int)(Screen.height * 0.5f), 16);
        }


        Camera.main.targetTexture = _CameraTexture2D;
        _CameraCullingMask = Camera.main.cullingMask;
        _BackImage.texture = _CameraTexture2D;

        yield return new WaitForSeconds(0.1f);

        Camera.main.targetTexture = null;
        Camera.main.cullingMask = 0;
    }

    private void ReleaseCamera()
    {
        Camera.main.cullingMask = _CameraCullingMask;
        Camera.main.targetTexture = null;
    }

    #endregion

    #region camera culling

    private void SetCameraCulling()
    {
        _BackImage.enabled = false;
        _CameraCullingMask = Camera.main.cullingMask;
        Camera.main.cullingMask = Camera.main.cullingMask & (~(1 << LayerMask.NameToLayer("MainPlayer")));
        Camera.main.cullingMask = Camera.main.cullingMask & (~(1 << LayerMask.NameToLayer("LogicObj")));
    }

    private void ResetCameraCulling()
    {
        Camera.main.cullingMask = _CameraCullingMask;
    }

    #endregion
}
