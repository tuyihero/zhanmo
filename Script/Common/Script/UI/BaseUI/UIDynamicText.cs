using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;


public class UIDynamicText : UIBase
{
    public Text _Text;
    public RectTransform _ContainerTrans;
    public RectTransform _ScollTrans;

    //public void Start()
    //{
    //    SetText("sdfsdfsdf\nadsfasdfasdfasdfsdf\t\t\t\t\t\tsdfsdfsdfsdfsfsdf\t\t\ttsdfsdfsdfs\n\n\n\n\n\nn\n\asdasdasd\n\n\n\nnsdfsdfsdfsdf\n\n\n\nsdfsdfsddf\n\nnsdfsdf\n\n");
    //}

    public void SetText(string text)
    {
        _Text.text = text;
        //_Text.CalculateLayoutInputHorizontal();
        //_Text.CalculateLayoutInputVertical();

        //_ContainerTrans.sizeDelta = new Vector2(_ScollTrans.sizeDelta.x, _Text.preferredHeight);
    }
}

