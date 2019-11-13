using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class UIContainerSlideStep : UIContainerBase
{
    #region 

    private float _StepHeight = 0;
    private float _StepHeightHalf = 0;
    private float _ContentYMin = 0;
    private float _ContentYMax = 0;
    private int _StepMin = 0;
    private int _StepMax = 0;
    private int _SelectedStepDelta = 0;

    private int _SelectedStep = 0;
    public int SelectedStep
    {
        get
        {
            return _SelectedStep;
        }
        set
        {
            _SelectedStep = value;
            SetSelectedIdx(_SelectedStep);
        }
    }

    #endregion

    public override void InitContentItem(IEnumerable valueList, UIItemBase.ItemClick onClick = null, Hashtable exhash = null, UIItemBase.PanelClick onPanelClick = null)
    {
        base.InitContentItem(valueList, onClick, exhash, onPanelClick);

        _StepHeight = _LayoutGroup.cellSize.y + _LayoutGroup.spacing.y;
        _StepHeightHalf = _LayoutGroup.cellSize.y * 0.5f;
        _ContentYMin = -_ScrollTransform.sizeDelta.y * 0.5f;
        _StepMin = (int)(_ContentYMin / _StepHeight);

        _StepMax = _StepMin + _ValueList.Count - 1;
        _ContentYMax = _StepMax * _StepHeight;

        _SelectedStepDelta = -_StepMin;

        _SelectedStep = _SelectedStepDelta;
    }

    public override void ShowItems()
    {
        int step = (int)(_ContainerObj.localPosition.y / _StepHeight);
        step = Mathf.Clamp(step, _StepMin, _StepMax);
        _SelectedStep = step + _SelectedStepDelta;
        float posY = Mathf.Clamp(step * _StepHeight, _ContentYMin, _ContentYMax);
        _ContainerObj.localPosition = new Vector3(_ContainerObj.localPosition.x, posY, _ContainerObj.localPosition.z);

        base.ShowItems();
    }

    public void SetSelectedIdx(int idx)
    {
        int step = idx - _SelectedStepDelta;
        step = Mathf.Clamp(step, _StepMin, _StepMax);
        Debug.Log("ShowItems selectedStep:" + (_SelectedStep + 40).ToString());
        float posY = Mathf.Clamp(step * _StepHeight, _ContentYMin, _ContentYMax);
        _ContainerObj.localPosition = new Vector3(_ContainerObj.localPosition.x, posY, _ContainerObj.localPosition.z);

        base.ShowItems();
    }

}

