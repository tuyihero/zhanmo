using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class UIContainerPage : UIContainerBase
{
    public float _ItemWidth;
    public float _ItemCountPrePage;
    public float _SlideSpeed;

    public UISliderBtn _SliderBtn;

    private float _TotalWidth;
    private int _MaxPage;

    private int _ShowingPage;
    public int ShowingPage { get { return _ShowingPage; } }

    #region 

    public override void InitContentItem(IEnumerable list, UIItemBase.ItemClick onClick = null, Hashtable exhash = null, UIItemBase.PanelClick onPanelClick = null)
    {
        base.InitContentItem(list, onClick, exhash, onPanelClick);

        //_MaxPage = (int)(_ActivedItems.Count / _ItemCountPrePage);
        //if (_ActivedItems.Count % _ItemCountPrePage > 0)
        //{
        //    ++_MaxPage;
        //}
        _TotalWidth = _MaxPage * _ItemCountPrePage * _ItemWidth;
    }

    public void ShowPage(int page)
    {
        _ShowingPage = Mathf.Clamp(page, 0, _MaxPage - 1);
        ShowToPage(false);
    }

    public void ShowToPage(bool isConsideSlide = true)
    {
        if (_SlideSpeed != 0 && isConsideSlide)
        {
            var destPos = GetContentPagePos(_ShowingPage);

            Hashtable hash = new Hashtable();
            hash.Add("speed", _SlideSpeed);
            hash.Add("position", destPos);
            hash.Add("islocal", true);

        }
        else
        {
            _ContainerObj.transform.localPosition = GetContentPagePos(_ShowingPage);
        }
    }

    public Vector3 GetContentPagePos(int page)
    {
        return new Vector3(_TotalWidth * 0.5f - _ItemWidth * _ItemCountPrePage * page, 0, 0);
    }

    #endregion

    #region ui event

    public void BtnLeft()
    {
        --_ShowingPage;
        _ShowingPage = Mathf.Clamp(_ShowingPage, 0, _MaxPage - 1);

        ShowToPage();
    }

    public void BtnRight()
    {
        ++_ShowingPage;
        _ShowingPage = Mathf.Clamp(_ShowingPage, 0, _MaxPage - 1);

        ShowToPage();
    }

    public void BtnSlider(Vector2 direct)
    {
        if (direct.x > 0)
        {
            BtnLeft();
        }
        else
        {
            BtnRight();
        }
    }
    #endregion
}

