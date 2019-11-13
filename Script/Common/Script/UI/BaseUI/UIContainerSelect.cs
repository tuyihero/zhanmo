using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class UIContainerSelect : UIContainerBase
{
    public bool _IsMultiSelect = false;                                 // 是否可多选，默认单选
    protected List<ContentPos> _Selecteds = new List<ContentPos>();       // 选中的物品列表

    public T GetSelected<T>()
    {
        if (!_IsMultiSelect && _Selecteds.Count > 0)
        {
            return (T)_Selecteds[0].Obj;
        }
        return default(T);
    }

    public void SelectIndex(int index)
    {
        if (_Selecteds.Count <= index || index<0)
            return;
        if (_Selecteds.Count <= 0)
        {
            if (_ValueList.Count > 0)
            {
                OnSelectedObj(_ValueList[0].Obj);
            }
            return;
        }
    }

    public void SelectNext()
    {
        if (_Selecteds.Count > 1)
            return;
        if(_Selecteds.Count<=0)
        {
            if(_ValueList.Count>0)
            {
                OnSelectedObj(_ValueList[0].Obj);
            }
            return;
        }
        ContentPos sel = _Selecteds[0];
        for (int i=0;i<_ValueList.Count;i++)
        {
            if(_ValueList[i] == sel)
            {
                if(i==_ValueList.Count-1)
                {
                    OnSelectedObj(_ValueList[0].Obj);
                    return;
                }
                if(i< _ValueList.Count - 1)
                {
                    OnSelectedObj(_ValueList[i+1].Obj);
                    return;
                }
            }
        }
    }

    public void SelectLast()
    {
        if (_Selecteds.Count > 1)
            return;
        if (_Selecteds.Count <= 0)
        {
            if (_ValueList.Count > 0)
            {
                OnSelectedObj(_ValueList[_ValueList.Count - 1].Obj);
            }
            return;
        }
        ContentPos sel = _Selecteds[0];
        for (int i = 0; i < _ValueList.Count; i++)
        {
            if (_ValueList[i] == sel)
            {
                if (i == 0)
                {
                    OnSelectedObj(_ValueList[_ValueList.Count - 1].Obj);
                    return;
                }
                if (i > 0)
                {
                    OnSelectedObj(_ValueList[i - 1].Obj);
                    return;
                }
            }
        }
    }

    public UIItemBase GetSelectedItem()
    {
        if (!_IsMultiSelect && _Selecteds.Count > 0)
        {
            return _Selecteds[0].ShowItem;
        }
        return null;
    }

    public List<T> GetSelecteds<T>()
    {
        List<T> selectedObjs = new List<T>();
        foreach (var selectedPos in _Selecteds)
        {
            selectedObjs.Add((T)selectedPos.Obj);
        }
        return selectedObjs;
    }

    public delegate void SelectedObjCallBack(object obj);
    protected SelectedObjCallBack _SelectedCallBack;
    protected SelectedObjCallBack _DisSelectedCallBack;

    public virtual void OnSelectedObj(object obj)
    {
        ContentPos selectPos = _ValueList.Find((pos) =>
        {
            if (pos.Obj == obj)
                return true;
            return false;
        }); 

        if (selectPos == null)
            return;

        if (!((UIItemSelect)selectPos.ShowItem).IsCanSelect())
            return;

        if (_IsMultiSelect)
        {
            if (_Selecteds.Contains(selectPos))
            {
                if (selectPos.ShowItem != null)
                {
                    ((UIItemSelect)selectPos.ShowItem).UnSelected();
                }
                _Selecteds.Remove(selectPos);
            }
            else
            {
                

                if (selectPos.ShowItem != null)
                {
                    ((UIItemSelect)selectPos.ShowItem).Selected();
                }
                _Selecteds.Add(selectPos);
            }
        }
        else
        {
            if (_Selecteds.Count > 0)
            {
                //if (_Selecteds[0] == selectPos)
                //    return;

                ((UIItemSelect)_Selecteds[0].ShowItem).UnSelected();
            }
            _Selecteds.Clear();

            _Selecteds.Add(selectPos);
            ((UIItemSelect)_Selecteds[0].ShowItem).Selected();
        }

        if (_SelectedCallBack != null && _Selecteds.Contains(selectPos))
        {
            _SelectedCallBack(obj);
        }
        else if (_DisSelectedCallBack != null && !_Selecteds.Contains(selectPos))
        {
            _DisSelectedCallBack(obj);
        }
    }

    #region 

    private IEnumerable _SelectedList = null;

    public virtual void InitSelectContent(IEnumerable list, IEnumerable selectedList, SelectedObjCallBack onSelect = null, SelectedObjCallBack onDisSelect = null, Hashtable exhash = null)
    {
        _SelectedCallBack = onSelect;
        _DisSelectedCallBack = onDisSelect;

        gameObject.SetActive(true);

        _SelectedList = selectedList;

        base.InitContentItem(list, OnSelectedObj, exhash);

        ShowItems();
    }

    public virtual void ClearSelect()
    {
        foreach (var selectPos in _Selecteds)
        {
            ((UIItemSelect)selectPos.ShowItem).UnSelected();
            if (_DisSelectedCallBack != null)
            {
                _DisSelectedCallBack(selectPos.Obj);
            }

        }

        _Selecteds.Clear();
    }

    public void SetSelect(IEnumerable selectedList)
    {
        _SelectedList = selectedList;
        _Selecteds.Clear();
        if (selectedList != null)
        {
            foreach (var selectItem in selectedList)
            {
                ContentPos selectPos = _ValueList.Find((pos) =>
                {
                    if (pos.Obj.Equals( selectItem))
                        return true;
                    return false;
                });
                if (selectPos != null)
                    _Selecteds.Add(selectPos);
            }
        }

       
        foreach (var shoItem in _ItemPrefabList)
        {
            var showObj = shoItem._InitInfo;
            ContentPos selectPos = _Selecteds.Find((pos) =>
            {
                if (pos.Obj.Equals(showObj))
                    return true;
                return false;
            });

            if (selectPos != null)
            {
                ((UIItemSelect)shoItem).Selected();
                if (_SelectedCallBack != null)
                {
                    _SelectedCallBack(selectPos.Obj);
                }
            }
            else
            {
                ((UIItemSelect)shoItem).UnSelected();
            }
        }

        if (_Selecteds.Count > 0)
        {
            StartCoroutine(ShowSelectContainPos(_Selecteds[0].ShowItem));
        }
    }

    public override void ShowItemsFinish()
    {
        base.ShowItemsFinish();
        SetSelect(_SelectedList);
    }

    public delegate void ShowContainerPosCallFunc();
    protected ShowContainerPosCallFunc callFunc;
    public void ShowContainerPos(UIItemBase selectPos, ShowContainerPosCallFunc callFunc = null)
    {
        if (selectPos == null)
            return;
        Hashtable hash = new Hashtable();
        if (callFunc != null)
        {
            hash.Add("callFunc", callFunc);
        }
        StartCoroutine(ShowSelectContainPos(selectPos, hash));
    }

    private IEnumerator ShowSelectContainPos(UIItemBase selectPos, Hashtable hash = null)
    {
        yield return new WaitForFixedUpdate();
        if (_ScrollRect != null)
        {
            if (_ScrollTransform.rect.width < _ContainerObj.rect.width)
            {
                if (_ScrollRect.horizontal == true)
                {
                    float containerMaxX = _ContainerObj.sizeDelta.x;
                    float containPosX = -selectPos.GetComponent<RectTransform>().anchoredPosition.x + _ScrollTransform.rect.width * 0.5f;
                    containPosX = Mathf.Clamp(containPosX, -(containerMaxX - _ScrollTransform.rect.width), 0);
                    _ContainerObj.anchoredPosition = new Vector2(containPosX, _ContainerObj.anchoredPosition.y);
                }
            }

            if (_ScrollTransform.rect.height < _ContainerObj.rect.height)
            {
                if (_ScrollRect.vertical == true)
                {
                    float containerMaxY = _ContainerObj.rect.height;
                    float containPosY = -selectPos.GetComponent<RectTransform>().anchoredPosition.y - _ScrollTransform.rect.height * 0.5f;
                    containPosY = Mathf.Clamp(containPosY, 0, containerMaxY - _ScrollTransform.rect.height);
                    Debug.Log("containPosY:" + containPosY);
                    _ContainerObj.anchoredPosition = new Vector2(_ContainerObj.anchoredPosition.x, containPosY);
                }
            }
        }

        if(hash != null && hash.ContainsKey("callFunc"))
        {
            var callFunc = (ShowContainerPosCallFunc)hash["callFunc"];
            callFunc();
        }
    }

    private void ResetSelectContainPos()
    {
        if (_ScrollRect != null)
        {
            if (_ScrollTransform.rect.width < _ContainerObj.rect.width)
            {
                if (_ScrollRect.horizontal == true)
                {
                    _ContainerObj.anchoredPosition = new Vector2(0, _ContainerObj.anchoredPosition.y);
                }
            }

            if (_ScrollTransform.rect.height < _ContainerObj.rect.height)
            {
                if (_ScrollRect.vertical == true)
                {
                    _ContainerObj.anchoredPosition = new Vector2(_ContainerObj.anchoredPosition.x, 0);
                }
            }
        }
    }

    public override void ShowItems()
    {
        base.ShowItems();

       
    }

    #endregion


}

