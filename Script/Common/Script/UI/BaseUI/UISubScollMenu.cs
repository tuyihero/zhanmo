using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;


public class UISubScollMenu : UIBase
{
    public RectTransform MenuContainer;
    public RectTransform _ContainerBG;
    public GameObject Sub1BtnPrefab;
    public GameObject Sub2BtnPrefab;

    private Dictionary<UISubMenuItem, List<UISubMenuItem>> _SubBtns = new Dictionary<UISubMenuItem, List<UISubMenuItem>>();
    public Dictionary<UISubMenuItem, List<UISubMenuItem>> SubBtns
    {
        get
        {
            return _SubBtns;
        }
    }

    private UISubMenuItem _SelectedSub1;
    public object SelectedSub1
    {
        get
        {
            if (_SelectedSub1 == null)
                return null;
            return _SelectedSub1.MenuObj;
        }
    }

    private UISubMenuItem _SelectedSub2;
    public object SelectedSub2
    {
        get
        {
            if (_SelectedSub2 == null)
                return null;
            return _SelectedSub2.MenuObj;
        }
    }

    private Queue<UISubMenuItem> _Prefabs1 = new Queue<UISubMenuItem>();
    private Queue<UISubMenuItem> _Prefabs2 = new Queue<UISubMenuItem>();

    private int subIndex = 0;
    [Serializable]
    public class MenuClickEvent : UnityEvent<object>
    {
        public MenuClickEvent()
        {
            
        }
    }

    [SerializeField]
    private MenuClickEvent _MenuClick;

    void Update()
    {
        SetBGRect();
    }

    public void Clear()
    {
        subIndex = 0;
        foreach (var keyValue in _SubBtns)
        {
            _Prefabs1.Enqueue(keyValue.Key);
            keyValue.Key.gameObject.SetActive(false);
            foreach (var item in keyValue.Value)
            {
                _Prefabs2.Enqueue(item);
                item.gameObject.SetActive(false);
            }
        }
        MenuContainer.sizeDelta = Vector2.zero;
        _IsInitBG = false;

        _SubBtns.Clear();
        _SelectedSub1 = null;
        _SelectedSub2 = null;
    }

    public void PushMenu(object sub1Tx)
    {
        PushMenu(sub1Tx, new object[] { });
    }

    public void PushMenu(object sub1Tx, int[] sub2Txs)
    {
        var sub1 = CreateNewMenu(sub1Tx, 1);
        sub1.gameObject.SetActive(true);
        sub1.SubLevel = 1;
        sub1.UnSelected();
        sub1.CloseSubGO();

        List<UISubMenuItem> sub2 = new List<UISubMenuItem>();
        foreach (var sub2Tx in sub2Txs)
        {
            var sub2Item = CreateNewMenu(sub2Tx, 2);
            sub2Item.SubLevel = 2;
            sub2Item.gameObject.SetActive(false);
            sub2Item.UnSelected();
            sub2Item.CloseSubGO();
            sub2.Add(sub2Item);
        }

        _SubBtns.Add(sub1, sub2);
    }

    public void PushMenu(object sub1Tx, object[] sub2Txs)
    {
        var sub1 = CreateNewMenu(sub1Tx, 1);
        sub1.gameObject.SetActive(true);
        sub1.SubLevel = 1;
        sub1.UnSelected();
        sub1.CloseSubGO();

        List<UISubMenuItem> sub2 = new List<UISubMenuItem>();
        foreach (var sub2Tx in sub2Txs)
        {
            var sub2Item = CreateNewMenu(sub2Tx, 2);
            sub2Item.SubLevel = 2;
            sub2Item.gameObject.SetActive(false);
            sub2Item.UnSelected();
            sub2Item.CloseSubGO();
            sub2.Add(sub2Item);
        }
        sub2.Reverse();

        _SubBtns.Add(sub1, sub2);
    }

    public void ShowDefaultFirst()
    {
        //_SelectedSub1 = null;
        //_SelectedSub2 = null;
        if (_SubBtns.Count > 0)
        {
            foreach (var keyValue in _SubBtns)
            {
                if (!keyValue.Key.isActiveAndEnabled)
                    continue;

                MenuClick(keyValue.Key);
                if (keyValue.Value != null && keyValue.Value.Count > 0)
                {
                    for(int i = 0; i< keyValue.Value.Count; ++i)
                    {
                        if (!keyValue.Value[i].ShowMenu())
                            continue;
                        MenuClick(keyValue.Value[i]);
                    } 
                }
                break;
            }
        }
        else
        {
            MenuClick(null);
        }
    }

    public void HightLightMenu(object showObj)
    {
        if (_SubBtns.Count > 0)
        {
            foreach (var keyValue in _SubBtns)
            {
                if (showObj == keyValue.Key.MenuObj)
                {
                    keyValue.Key.Selected();
                    break;
                }
            }
        }
    }

    public void ShowMenu(object showObj, object showsubObj = null)
    {
        if (_SubBtns.Count > 0)
        {
            foreach (var keyValue in _SubBtns)
            {
                if (showObj == keyValue.Key.MenuObj)
                {
                    MenuClick(keyValue.Key);
                    if (keyValue.Value.Count > 0)
                    {
                        foreach (var subMenu in keyValue.Value)
                        {
                            if(showsubObj == subMenu.MenuObj)
                                MenuClick(keyValue.Key);
                            break;
                        }
                    }
                    break;
                }

                
            }
        }
        else
        {
            MenuClick(null);
        }
    }

    public void Refresh()
    {
        foreach (var menuBtn in _SubBtns)
        {
            menuBtn.Key.Refresh();
            foreach (var subMenu in menuBtn.Value)
            {
                subMenu.Refresh();
            }
        }
    }

    private UISubMenuItem CreateNewMenu(object tx, int level)
    {
        GameObject go;
        UISubMenuItem menuScript;

        if (level == 1)
        {
            if (_Prefabs1.Count > 0)
            {
                menuScript = _Prefabs1.Dequeue();
                go = menuScript.gameObject;
            }
            else
            {
                go = GameObject.Instantiate<GameObject>(Sub1BtnPrefab.gameObject);
                menuScript = go.GetComponent<UISubMenuItem>();
            }
        }
        else
        {
            if (_Prefabs2.Count > 0)
            {
                menuScript = _Prefabs2.Dequeue();
                go = menuScript.gameObject;
            }
            else
            {
                go = GameObject.Instantiate<GameObject>(Sub2BtnPrefab.gameObject);
                menuScript = go.GetComponent<UISubMenuItem>();
            }
        }

        menuScript.InitMenu(tx);
        go.transform.SetParent(MenuContainer.transform);
        go.transform.SetSiblingIndex(subIndex);
        ++subIndex;
        go.transform.localScale = Vector3.one;
        menuScript._PanelClickEvent = MenuClick;
        return menuScript;
    }

    public void MenuClick(UIItemBase itemBase)
    {
        UISubMenuItem menuItem = itemBase as UISubMenuItem;
        if (menuItem == null)
            return;

        
        if (menuItem.SubLevel == 1)
        {
            if (menuItem != _SelectedSub1)
            {
                ClearSelect(1);
                ClearSelect(2);
                _SelectedSub1 = menuItem;
                _SelectedSub1.Selected();
                if (_SubBtns[_SelectedSub1].Count > 0)
                {
                    _SelectedSub1.OpenSubGO();
                    //foreach (var sub2 in _SubBtns[_SelectedSub1])
                    //{
                    //    sub2.ShowMenu();
                    //}
                    for (int i = 0; i < _SubBtns[_SelectedSub1].Count; ++i)
                    {
                        if (!_SubBtns[_SelectedSub1][i].ShowMenu())
                            continue;

                        //if(_SelectedSub2 == null)
                            _SelectedSub2 = _SubBtns[_SelectedSub1][i];
                    }

                    if (_SelectedSub2 != null)
                    {
                        MenuClick(_SelectedSub2);
                        _SelectedSub2.Selected();
                        _MenuClick.Invoke(_SelectedSub2.MenuObj);
                        return;
                    }
                }
            }
            else if(_SubBtns[_SelectedSub1].Count > 0)
            {
                ClearSelect(1);
            }
        }
        else
        {
            if (menuItem != _SelectedSub2)
            {
                ClearSelect(2);
                _SelectedSub2 = menuItem;
                _SelectedSub2.Selected();
            }
        }

        _MenuClick.Invoke(menuItem.MenuObj);
    }

    private void ClearSelect(int layer)
    {
        if (layer == 2 && _SelectedSub2 != null)
        {
            _SelectedSub2.CloseSubGO();
            _SelectedSub2.UnSelected();
            _SelectedSub2 = null;
        }

        if (layer == 1 && _SelectedSub1 != null)
        {
            _SelectedSub1.CloseSubGO();
            _SelectedSub1.UnSelected();
            foreach (var sub2 in _SubBtns[_SelectedSub1])
            {
                sub2.gameObject.SetActive(false);
            }
            _SelectedSub1 = null;
        }
    }

    #region bg

    private bool _IsInitBG = false;

    public void SetBGRect()
    {
        if (_ContainerBG == null)
            return;

        if (_IsInitBG)
            return;

        if (MenuContainer.sizeDelta.y > 1)
        {
            _IsInitBG = true;
            _ContainerBG.sizeDelta = new Vector2(_ContainerBG.sizeDelta.x, MenuContainer.sizeDelta.y + 10);
        }
    }

    #endregion
}

