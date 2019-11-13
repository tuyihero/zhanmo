using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

 
using System.IO;
using System;
using System.Reflection;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    #region 固有

    public void Awake()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(_InputEventSystem);
        _Instance = this;
        InitUILayers();
    }

    #endregion

    #region 唯一

    private static UIManager _Instance = null;
    public static UIManager Instance
    {
        get
        {
            return _Instance;
        }
    }
    #endregion

    #region

    /// <summary>
    /// 屏幕画布
    /// </summary>
    [SerializeField]
    private Canvas _ScreenCanvas;
    public Canvas ScreenCanvas { get { return _ScreenCanvas; } }

    /// <summary>
    /// 事件系统
    /// </summary>
    [SerializeField]
    private EventSystem _InputEventSystem;

    private void InitUILayers()
    {
        foreach (var layerEnum in Enum.GetValues(typeof(UILayer)))
        {
            Transform layerTrans = transform.Find(((UILayer)layerEnum).ToString());
            if (layerTrans != null)
            {
                _UILayers.Add((UILayer)layerEnum, layerTrans.GetComponent<RectTransform>());
            }
            else
            {
                GameObject layer = new GameObject(((UILayer)layerEnum).ToString());
                layer.transform.SetParent(ScreenCanvas.transform);
                RectTransform rectTransform = layer.AddComponent<RectTransform>();
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.localScale = Vector3.one;
                rectTransform.sizeDelta = Vector2.zero;
                _UILayers.Add(((UILayer)layerEnum), rectTransform);
            }
        }
    }

    private void InitUICallBack(string uiName, GameObject uiGO, Hashtable hashtable)
    {
        UILayer uilayer = (UILayer)hashtable["UILayer"];
        string uiPath = (string)hashtable["UIPath"];

        if (_UIObjs.ContainsKey(uiPath))
        {
            _UIObjs[uiPath].Show(hashtable);
            GameObject.Destroy(uiGO);
            return;
        }

        uiGO.transform.position = Vector3.zero;
        var trans = uiGO.GetComponent<RectTransform>();
        trans.anchoredPosition = Vector2.zero;
        trans.sizeDelta = Vector2.zero;

        uiGO.transform.localScale = new Vector3(1, 1, 1);
        
        if (!hashtable.ContainsKey("IndependCanvas"))
        {
            uiGO.transform.SetParent(_UILayers[uilayer]);
        }
        else
        {
            uiGO.transform.SetParent(ResourcePool.Instance.transform);
        }

        if (trans != null)
        {
            trans.anchoredPosition = Vector2.zero;
            trans.sizeDelta = Vector2.zero;
            trans.localScale = Vector3.one;
        }
        else
        {
            uiGO.transform.position = Vector3.zero;
            uiGO.transform.localScale = Vector3.one;
        }

        var script = uiGO.GetComponent<UIBase>();
        uiGO.name = script.GetType().Name;
        script.UIPath = uiPath;
        script.UILayer = uilayer;

        _UIObjs.Add(uiPath, script);

        script.Show(hashtable);
    }

    private Dictionary<string, UIBase> _UIObjs = new Dictionary<string, UIBase>();
    private Dictionary<UILayer, RectTransform> _UILayers = new Dictionary<UILayer, RectTransform>();

    public void ShowOrCreateUI(string uiPath, UILayer uilayer, Hashtable hashtable)
    {
        UIConflict(uilayer);
        if (_UIObjs.ContainsKey(uiPath))
        {
            _UIObjs[uiPath].Show(hashtable);
        }
        else
        {
            hashtable.Add("UILayer", uilayer);
            hashtable.Add("UIPath", uiPath);
            ResourceManager.Instance.LoadUI(uiPath, InitUICallBack, hashtable);
        }

        //UIShowed(uiPath, uilayer, _UIObjs[uiPath]);
    }

    //异步显示
    public void ShowUI(AssetInfo uiinfo, UILayer uilayer, Hashtable hashtable = null)
    {
        ShowOrCreateUI(uiinfo.AssetPath, uilayer, hashtable);
    }

    //public IEnumerator ShowAsyn(string uiPath, UILayer uilayer, Hashtable hashtable)
    //{
    //    yield return new WaitForSeconds(0.01f);
    //    Debug.Log("ShowAsyn");
    //    ShowOrCreateUI(uiPath, uilayer, hashtable);
    //}

    public void HideUI(string uiPath)
    {
        if (_UIObjs.ContainsKey(uiPath))
        {
            _UIObjs[uiPath].Hide();
        }
    }

    public void DestoryUI(string uiPath)
    {
        if (_UIObjs.ContainsKey(uiPath))
        {
            GameObject.Destroy(_UIObjs[uiPath].gameObject);
            _UIObjs.Remove(uiPath);
        }
    }

    public void DestoryUI(UIBase uiBase)
    {
        DestoryUI(uiBase.UIPath);
    }

    public void HideAllUI()
    {
        foreach (var uiPair in _UIObjs)
        {
            uiPair.Value.Hide();
        }
        _UIObjs.Clear();
    }

    public void DestoryAllUI()
    {
        var uiObjs = new List<UIBase>();
        foreach (var uiPair in _UIObjs)
        {
            uiObjs.Add(uiPair.Value);
        }
        for (int i = 0; i < uiObjs.Count; ++i)
        {
            uiObjs[i].Destory();
        }
        _UIObjs.Clear();
    }

    public void HideLayer(UILayer uiLayer)
    {
        foreach (var uiPair in _UIObjs)
        {
            if (uiPair.Value.UILayer == uiLayer)
            {
                uiPair.Value.Hide();
            }
        }
    }

    public void DestoryLayer(UILayer uiLayer)
    {
        foreach (var uiPair in _UIObjs)
        {
            if (uiPair.Value.UILayer == uiLayer)
            {
                uiPair.Value.Destory();
            }
        }
    }

    public T GetUIInstance<T>(AssetInfo uiasset)
    {
        if (_UIObjs.ContainsKey(uiasset.AssetPath))
            return _UIObjs[uiasset.AssetPath].GetComponent<T>();

        return default(T);
    }

    #endregion

    #region rayCast

    //防死循环
    bool _RayCasting = false;
    public void RayCastBebind(PointerEventData eventData)
    {
        if (_RayCasting)
            return;

        _RayCasting = true;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        _InputEventSystem.RaycastAll(eventData, raycastResults);
        if (raycastResults.Count > 0)
        {
            var pointClick = raycastResults[0].gameObject.GetComponent<Graphic>();
            //if (ExecuteEvents.CanHandleEvent<IPointerClickHandler>(raycastResults[0].gameObject))
            {
                ExecuteEvents.ExecuteHierarchy<IPointerClickHandler>(raycastResults[0].gameObject, eventData, (x, y) => x.OnPointerClick(eventData));
            }
        }
        _RayCasting = false;
    }

    #endregion

    #region cameraPos

    private RectTransform _UICanvasRect;
    private Vector3 _UICameraScale;

    public Vector3 WorldToScreenPoint(Vector3 worldPos)
    {
        if (Camera.main == null)
            return Vector3.zero;

        if (_UICanvasRect == null)
        {
            _UICanvasRect = gameObject.GetComponent<RectTransform>();
        }

        //Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        //return screenPos;
        Vector3 screenPos = Camera.main.WorldToViewportPoint(worldPos);
        //1024
        //screenPos -= new Vector3(0.5f, 0.5f, 0.5f);
        return new Vector3(screenPos.x * _UICanvasRect.sizeDelta.x, screenPos.y * _UICanvasRect.sizeDelta.y, screenPos.z * 1);

    }

    #endregion

    #region ui conflict

    public void UIConflict(UILayer uilayer)
    {
        if (uilayer == UILayer.PopUI /*|| uilayer == UILayer.MessageUI*/)
        {
            HideLayer(uilayer);
        }
    }

    #endregion

}

