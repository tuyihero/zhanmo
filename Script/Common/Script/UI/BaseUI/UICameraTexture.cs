using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class UICameraTexture : UIBase, IDragHandler
{
    #region 

    public class FakeShowObj
    {
        public Camera _ObjCamera;
        public Transform _ObjTransorm;
        public RenderTexture _ObjTexture;
        public GameObject _ShowingModel;
    }

    //private static Stack<FakeShowObj> _IdleFakeList = new Stack<FakeShowObj>();
    //private FakeShowObj GetIdleFakeShow()
    //{
    //    if (_IdleFakeList != null && _IdleFakeList.Count > 0)
    //    {
    //        var popFakeObj = _IdleFakeList.Pop();
    //        if (popFakeObj._ObjCamera != null)
    //        {
    //            popFakeObj._ObjCamera.gameObject.SetActive(true);
    //            return popFakeObj;
    //        }
    //    }

    //    return CreateNewFake();
    //}

    private FakeShowObj CreateNewFake()
    {
        FakeShowObj fakeObj = new FakeShowObj();

        GameObject cameraGO = Instantiate(_CameraPrefab);
        cameraGO.transform.SetParent(null);
        fakeObj._ObjCamera = cameraGO.GetComponent<Camera>();
        cameraGO.transform.position = new Vector3(1000 + 100 * _CallTimes, -1000, 0);
        cameraGO.transform.rotation = Camera.main.transform.rotation;
        fakeObj._ObjTransorm = cameraGO.transform.Find("GameObject");
        fakeObj._ObjTexture = new RenderTexture(1024, 1024, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        fakeObj._ObjTexture.depth = 32;
        fakeObj._ObjCamera.orthographicSize = (cameraSize <= 0 ? 1 : cameraSize);
        fakeObj._ObjCamera.targetTexture = fakeObj._ObjTexture;
        //fakeObj._ObjCamera.cullingMask = _CameraPrefab.layer;
        fakeObj._ObjCamera.depthTextureMode = DepthTextureMode.Depth;
        ++_CallTimes;

        return fakeObj;
    }

    //private void CGFakeObj(FakeShowObj fakeObj)
    //{
    //    if(fakeObj!=null)
    //    {
    //        if(fakeObj._ObjTransorm!=null)
    //        {
    //            fakeObj._ObjCamera = null;
    //            fakeObj._ObjTransorm.gameObject.SetActive(false);
    //            GameObject.Destroy(fakeObj._ObjTransorm.gameObject);
    //            fakeObj._ObjTransorm = null;
    //        }
    //    }
    //    //_IdleFakeList.Push(fakeObj);
    //}

    //private static FakeShowObj _FakeMainPlayer = null;

    #endregion

    public GameObject _CameraPrefab;
    public RawImage _RawImage;

    public FakeShowObj _FakeObj;
    public float cameraSize = 1;
    public Vector3 ModelPos;
    private static int _CallTimes = 0;
    private void InitImage()
    {
        if (_FakeObj == null || _FakeObj._ObjTransorm==null)
        {
            _RawImage.color = new Color(1, 1, 1, 0);
            _FakeObj = CreateNewFake();
            if (_FakeObj != null && _FakeObj._ObjCamera != null)
            {
                _FakeObj._ObjCamera.transform.SetParent(null);
            }
            _RawImage.texture = _FakeObj._ObjTexture;
            //_RawImage.material = Resources.Load<Material>("Material/UIRenderTex");
            _RawImage.color = new Color(1, 1, 1, 1);
        }
    }

    void OnEnable()
    {
        if (_RawImage != null)
        {
            if (_FakeObj == null || _FakeObj._ShowingModel == null)
            {
                _RawImage.gameObject.SetActive(false);
                return;
            }
        }
        _RawImage.gameObject.SetActive(true);
    }

    public void OnDestroy()
    {
        //if (_FakeObj != null)
        //{
        //    CGFakeObj(_FakeObj);
        //}
        _FakeObj = null;
    }

    public void ShowRawImage()
    {
        _RawImage.gameObject.SetActive(true);
    }

    public void InitShowGO(GameObject showObj)
    {
        InitImage();
        
        if (_FakeObj == null)
            return;
        if (_FakeObj._ObjTransorm == null)
        {
            return;
        }
        if (_FakeObj._ShowingModel != null && showObj.name == _FakeObj._ShowingModel.name)
            return;

        if (_FakeObj._ShowingModel != null)
        {
            _FakeObj._ShowingModel.SetActive(false);
        }

        _FakeObj._ObjCamera.transform.SetParent(transform, true);
        _FakeObj._ShowingModel = showObj;
        _FakeObj._ShowingModel.SetActive(true);
        _FakeObj._ShowingModel.transform.SetParent(_FakeObj._ObjTransorm);
        _FakeObj._ShowingModel.transform.localPosition = Vector3.zero;
        _FakeObj._ShowingModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
        _RawImage.gameObject.SetActive(true);
    }
    
    private static Vector3 StrToVector3(string strValue)
    {
        var splitStrs = strValue.Split(';');
        if (splitStrs.Length != 3)
        {
            Debug.LogError("StrToVector3 error:" + strValue);
            return Vector3.zero;
        }

        return new Vector3(float.Parse(splitStrs[0]), float.Parse(splitStrs[1]), float.Parse(splitStrs[2]));
    }

    #region opt

    public void OnDrag(PointerEventData eventData)
    {
        if (_FakeObj == null)
            return;

        if (_FakeObj._ShowingModel == null)
            return;

        Vector3 newObjAngle = _FakeObj._ShowingModel.transform.localRotation.eulerAngles;
        newObjAngle.y -= eventData.delta.x;
        _FakeObj._ShowingModel.transform.localRotation = Quaternion.Euler(newObjAngle);
    }

    #endregion

}
