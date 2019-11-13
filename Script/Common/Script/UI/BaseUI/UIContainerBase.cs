using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class UIContainerBase : UIBase
{
    // 容器每个Item的内容和位置
    public class ContentPos
    {
        public Vector2 Pos;             // 位置
        public object Obj;              // 显示信息
        public UIItemBase ShowItem;     // 显示脚本 - 内包含和 Obj 一样的显示信息
    }

    void OnDisable()
    {
        _ContainerObj.anchoredPosition = Vector2.zero;
    }

    void OnEnable()
    {
        // 仅当Conten内容被重新设置过才执行初始化。
        if (!_HasInit)
        {
            StartInitContainer();
        }
    }

    #region calculate rect

    protected float _ViewPortWidth = 0;
    protected float _ViewPortHeight = 0;
    protected float _ContentCanvasWidth = 0;
    protected float _ContentCanvasHeight = 0;
    protected int _ContentRow = 0;
    protected int _ContentColumn = 0;

    protected int _ShowRowCount = 0;
    protected int _ShowColumnCount = 0;

    public int _InitItemCount = 0;

    private void CalculateRect(ICollection items)
    {
        int itemCount = items.Count;

        _ViewPortWidth = _ScrollTransform.sizeDelta.x;
        _ViewPortHeight = _ScrollTransform.sizeDelta.y;

        _ContentColumn = 1;
        _ContentRow = 1;
        if (_LayoutGroup.constraint == GridLayoutGroup.Constraint.FixedRowCount)
        {
            _ContentColumn = _LayoutGroup.constraintCount;
            _ContentRow = itemCount / _ContentColumn;
            if (itemCount % _ContentColumn > 0)
            {
                ++_ContentRow;
            }
        }
        else if (_LayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            _ContentRow = _LayoutGroup.constraintCount;
            _ContentColumn = itemCount / _ContentRow;
            if (itemCount % _ContentRow > 0)
            {
                ++_ContentColumn;
            }
        }

        _ValueList.Clear();
        int i = 0;
        var enumerator = items.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext())
        //for (int i = 0; i < itemCount; ++i)
        {
            int column = i / _ContentRow;
            int row = i % _ContentRow;

            var contentPos = new ContentPos();
            contentPos.Pos = new Vector2(
                row * (_LayoutGroup.cellSize.x + _LayoutGroup.spacing.x) + _LayoutGroup.cellSize.x * 0.5f,
                -column * (_LayoutGroup.cellSize.y + _LayoutGroup.spacing.y) - _LayoutGroup.cellSize.y * 0.5f);
            contentPos.Obj = enumerator.Current;
            _ValueList.Add(contentPos);

            ++i;
        }

        
        _ContentCanvasWidth = _LayoutGroup.cellSize.x * _ContentRow + _LayoutGroup.spacing.x * (_ContentRow - 1);
        _ContentCanvasHeight = _LayoutGroup.cellSize.y * _ContentColumn + _LayoutGroup.spacing.y * (_ContentColumn - 1);

        if (_BG != null)
        {
            _BG.rectTransform.sizeDelta = new Vector2(_ContentCanvasWidth + 20, _ContentCanvasHeight + 20);
        }

        if (_ContainerBaseLarge)
        {
            _ContentCanvasWidth = Math.Max(_ScrollTransform.sizeDelta.x, _ContentCanvasWidth);
            _ContentCanvasHeight = Math.Max(_ScrollTransform.sizeDelta.y, _ContentCanvasHeight);
        }

        _ShowRowCount = (int)Math.Ceiling((_ViewPortWidth + _LayoutGroup.spacing.x) / (_LayoutGroup.cellSize.x + _LayoutGroup.spacing.x));
        _ShowColumnCount = (int)Math.Ceiling((_ViewPortHeight + _LayoutGroup.spacing.y) / (_LayoutGroup.cellSize.y + _LayoutGroup.spacing.y));

        _InitItemCount = _ShowColumnCount * _ShowRowCount;
        _InitItemCount = Math.Min(_InitItemCount, itemCount);

        _ContainerObj.sizeDelta = new Vector2(_ContentCanvasWidth, _ContentCanvasHeight);

        float containerPosX = 0;
        float containerPosY = 0;
        if (_ContainerObj.sizeDelta.x < _ScrollTransform.sizeDelta.x)
        {
            containerPosX = (_ScrollTransform.sizeDelta.x - _ContainerObj.sizeDelta.x) * 0.5f;
        }
        if (_ContainerObj.sizeDelta.y < _ScrollTransform.sizeDelta.y)
        {
            containerPosY = (_ContainerObj.sizeDelta.y - _ScrollTransform.sizeDelta.y) * 0.5f;
        }
        _ContainerObj.transform.localPosition = new Vector3(containerPosX, containerPosY, 0);

        //Debug.Log("CalculateRect viewPortWidth:" + _ShowRowCount + "," + _ShowColumnCount + "," + _ContentCanvasWidth + "," + _ContentCanvasHeight);

    }

    #endregion

    #region 

    public Vector3 GetItemPosition(object item)
    {
        foreach (UIItemBase uiItem in _ItemPrefabList)
        {
            if (uiItem._InitInfo == item)
            {
                return uiItem.transform.position;
            }
        }
        return Vector3.zero;
    }

    #endregion

    public ScrollRect _ScrollRect;              // 滑动框组件
    public RectTransform _ScrollTransform;      // 滑动框组件Rt
    public GridLayoutGroup _LayoutGroup;        // 自动布局组件
    public GameObject _ContainItemPre;          // Item预设体
    public RectTransform _ContainerObj;         // 自身容器Rt
    public Image _BG;
    public bool _ContainerBaseLarge = false;

    protected List<UIItemBase> _ItemPrefabList = new List<UIItemBase>();
    protected List<ContentPos> _ValueList = new List<ContentPos>();         // 内容的数组
    protected UIItemBase.ItemClick _OnClickItem;        // 单个内容Item点击回调
    protected UIItemBase.PanelClick _OnClickPanel;      // ...
    protected Action _OnInitFinish;

    public delegate void ShowItemFinish();
    protected ShowItemFinish m_ShowItemFinish = null;
    public void SetShowItemFinishCallFun(ShowItemFinish fun)
    {
        m_ShowItemFinish = fun;
    }

    public void SetLayoutPosotion(int index)
    {
        if(_LayoutGroup!=null)
        {
            _LayoutGroup.transform.localPosition = new Vector3(_LayoutGroup.transform.position.x, _LayoutGroup.cellSize.y * index, _LayoutGroup.transform.position.z);
        }
    }

    public int GetItemCount()
    {
        return _ValueList.Count;
    }

    protected Hashtable _ExHash;
    private bool _HasInit = false;

    public virtual void InitContentItem(IEnumerable valueList, UIItemBase.ItemClick onClick = null, Hashtable exhash = null, UIItemBase.PanelClick onPanelClick = null)
    {
        //CalculateRect(valueList);
        _ValueList.Clear();
        //ClearPrefab();
        int i = 0;
        if (valueList == null)
        {
            ClearPrefab();
            return;
        }
        var enumerator = valueList.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext())
        {
            var contentPos = new ContentPos();
            contentPos.Obj = enumerator.Current;
            _ValueList.Add(contentPos);

            ++i;
        }

        _InitItemCount = 50;
        _InitItemCount = Math.Min(_InitItemCount, _ValueList.Count);

        _HasInit = false;
        _OnClickItem = onClick;
        _OnClickPanel = onPanelClick;
        _ExHash = exhash;
        if (isActiveAndEnabled)
        {

            StartInitContainer();
        }

        //_LayoutGroup.enabled = false;

    }

    public void StartInitContainer()
    {
        _HasInit = true;
        if (_ValueList.Count > 0)
        {
            StartCoroutine(InitItems(_ExHash));
        }
        else
        {
            ClearPrefab();
        }
    }

    public virtual void ShowItems()
    {
        
    }

    public virtual void ShowItemsFinish()
    {
        if (m_ShowItemFinish != null)
            m_ShowItemFinish.Invoke();
    }

    private bool IsPosInView(ContentPos pos)
    {
        return (_ContainerObj.localPosition.x + pos.Pos.x >= -_LayoutGroup.cellSize.x * 0.5f
                && _ContainerObj.localPosition.x + pos.Pos.x <= _ViewPortWidth + _LayoutGroup.cellSize.x * 0.5f
                && -_ContainerObj.localPosition.y - pos.Pos.y >= -_LayoutGroup.cellSize.y * 0.5f
                && -_ContainerObj.localPosition.y - pos.Pos.y <= _ViewPortHeight + _LayoutGroup.cellSize.y * 0.5f);
    }

    private void InitItem(ContentPos contentItem, UIItemBase preItem, Hashtable exhash, int idx)
    {
        preItem.gameObject.SetActive(true);
        //preItem.transform.localPosition = new Vector3(contentItem.Pos.x, contentItem.Pos.y, 0);
        //if (preItem._InitInfo != contentItem.Obj)
        {
            Hashtable hash;
            if (exhash == null)
            {
                hash = new Hashtable();
            }
            else
            {
                hash = new Hashtable(exhash);
            }

            hash.Add("InitObj", contentItem.Obj);
            hash.Add("InitIdx", idx);
            preItem.Show(hash);
            preItem._InitInfo = contentItem.Obj;

            preItem._ClickEvent = _OnClickItem;
            preItem._PanelClickEvent = _OnClickPanel;
            contentItem.ShowItem = preItem;
        }
    }

    public virtual void RefreshItems()
    {
        foreach (var item in _ValueList)
        {
            item.ShowItem.Refresh();
        }
    }

    public virtual void RefreshItems(Hashtable hash)
    {
        foreach (var item in _ValueList)
        {
            item.ShowItem.Refresh(hash);
        }
    }

    public void PreLoadItem(int preLoadCount)
    {
        InitItems(null);
    }

    private void ClearPrefab()
    {
        foreach (var prefab in _ItemPrefabList)
        {
            //prefab.transform.localPosition = new Vector3(1000, 1000, 1);
            prefab.Hide();
            prefab._InitInfo = null;
        }
    }

    private IEnumerator InitItems(Hashtable exhash)
    {
        //先初始化可见
        //var itemPrefabList = _ContainerObj.GetComponentsInChildren<CommonItemBackPackItem>();
        //_ItemPrefabList = new List<UIItemBase>(itemPrefabList);
        //ClearPrefab();
        if (_InitItemCount > _ItemPrefabList.Count)
        {
            for (int i = _ItemPrefabList.Count; i < _InitItemCount; ++i)
            {
                GameObject obj = GameObject.Instantiate<GameObject>(_ContainItemPre);
                obj.transform.parent = _ContainerObj.transform;
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;

                obj.name = _ContainItemPre.name + _ItemPrefabList.Count;
                var itemScript = obj.GetComponent<UIItemBase>();
                //itemScript.Hide();
                _ItemPrefabList.Add(itemScript);
                //_EmpeyItemPrefab.Enqueue(itemScript);
            }
        }
        

        for (int i = 0; i < _InitItemCount; ++i)
        {
            InitItem(_ValueList[i], _ItemPrefabList[i], exhash, i);
        }

        if (_ItemPrefabList.Count > _ValueList.Count)
        {
            for (int i = _ValueList.Count; i < _ItemPrefabList.Count; ++i)
            {
                _ItemPrefabList[i].Hide();
            }
        }

        if (_InitItemCount >= _ValueList.Count)
        {
            ShowItemsFinish();
            yield break;
        }

        yield return new WaitForFixedUpdate();

        int nextNeedCount = _ValueList.Count - _InitItemCount;
        if (nextNeedCount > 0)
        {
            int needCreateCount = nextNeedCount - (_ItemPrefabList.Count - _InitItemCount);
            for (int i = 0; i < needCreateCount; ++i)
            {
                GameObject obj = GameObject.Instantiate<GameObject>(_ContainItemPre);
                obj.transform.SetParent(_ContainerObj.transform);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;
                var itemScript = obj.GetComponent<UIItemBase>();
                //itemScript.Hide();
                _ItemPrefabList.Add(itemScript);
                //_EmpeyItemPrefab.Enqueue(itemScript);
            }

            for (int i = _InitItemCount; i < _ValueList.Count; ++i)
            {
                InitItem(_ValueList[i], _ItemPrefabList[i], exhash, i);
            }
        }

        if (_ItemPrefabList.Count > _ValueList.Count)
        {
            for (int i = _ValueList.Count; i < _ItemPrefabList.Count; ++i)
            {
                _ItemPrefabList[i].Hide();
            }
        }

        ShowItemsFinish();
    }

    public T GetContainItem<T>(int idx)
    {
        return _ItemPrefabList[idx].GetComponent<T>();
    }

    public void ForeachActiveItem<T>(Action<T> action)
    {
        for (int i = 0; i < _ValueList.Count; ++i)
        {
            if (_ValueList[i].ShowItem.gameObject.activeSelf)
            {
                var component = _ValueList[i].ShowItem.gameObject.GetComponent<T>();
                if (component == null)
                    continue;

                action(component);
            }
        }
    }

    public Vector3 GetObjItemPos(object obj)
    {
        foreach (var item in _ValueList)
        {
            if (item.ShowItem._InitInfo == obj)
            {
                return item.ShowItem.transform.position;
            }
        }
        return Vector3.zero;
    }
}

