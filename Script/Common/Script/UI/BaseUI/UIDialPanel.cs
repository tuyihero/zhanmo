using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;


public class UIDialPanel : UIBase
{
    public UIContainerSlideStep _NumContainer;

    private int _NumMin = 0;

    public void InitNum(int min, int max)
    {
        _NumMin = min;
        List<int> numList = new List<int>();
        for (int i = min; i <= max; ++i)
        {
            numList.Add(i);
        }

        _NumContainer.InitContentItem(numList);
    }

    public void SetSelectNum(int selectNum)
    {
        int selectIdx = selectNum - _NumMin;
        if (selectIdx < 0)
        {
            _NumContainer.SelectedStep = 0;
            return;
        }

        _NumContainer.SelectedStep = selectNum - _NumMin;
    }

    public int GetSelectedNum()
    {
        return _NumContainer.SelectedStep + _NumMin;
    }
}

