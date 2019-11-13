using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

public class UITagPanel : MonoBehaviour
{
    [SerializeField]
    public List<Toggle> _Tags = new List<Toggle>();

    [SerializeField]
    public List<GameObject> _TagPanels = new List<GameObject>();

    public ToggleGroup _ToggleGroup;

    [Serializable]
    public class TagSelect : UnityEvent<int>
    {
        public TagSelect() { }
    }

    [SerializeField]
    public TagSelect _TagSelectCallBack;

    public int GetShowingPage()
    {
        if (_TagPanels.Count > 0)
        {
            var toggle = _ToggleGroup.ActiveToggles();
            for (int i = 0; i < _TagPanels.Count; ++i)
            {
                if (_TagPanels[i].activeInHierarchy)
                    return i;
            }
            return -1;
        }
        else
        {
            for (int i = 0; i < _Tags.Count; ++i)
            {
                if (_Tags[i].isOn == true)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    public void OnToggleOn(bool isOn)
    {
        if (!isOn)
            return;

        int page = -1;
        for(int i = 0; i< _Tags.Count; ++i)
        {
            if (_Tags[i].isOn)
            {
                page = i;
                break;
            }
        }
        if (page < 0)
            return;

        for (int i = 0; i < _TagPanels.Count; ++i)
        {
            if (i == page)
            {
                _TagPanels[i].SetActive(true);
            }
            else
            {
                _TagPanels[i].SetActive(false);
            }
        }

        if (_TagSelectCallBack != null)
        {
            _TagSelectCallBack.Invoke(page);
        }
    }

    public void ShowPage(int page)
    {
        for (int i = 0; i < _Tags.Count; ++i)
        {
            if (i == page)
            {
                _Tags[i].isOn = true;
            }
            else
            {
                _Tags[i].isOn = false;
            }
        }
    }
}
