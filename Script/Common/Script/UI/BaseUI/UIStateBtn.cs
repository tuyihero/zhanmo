using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIStateBtn : MonoBehaviour
{
    [SerializeField]
    public List<Button> _StateBtns = new List<Button>();

    [SerializeField]
    public List<GameObject> _StatePanels = new List<GameObject>();

    [Serializable]
    public class TagSelect : UnityEvent<int>
    {
        public TagSelect() { }
    }

    [SerializeField]
    public TagSelect _TagSelectCallBack;

    public int GetShowingState()
    {
        for(int i = 0; i< _StatePanels.Count; ++i )
        {
            if (_StatePanels[i].activeInHierarchy)
                return i;
        }
        return -1;
    }

    public void OnBtnState(int page)
    {
        ShowNextPage(page);

        if (_TagSelectCallBack != null)
        {
            _TagSelectCallBack.Invoke(page);
        }
    }

    public void ShowNextPage(int page)
    {
        int nextPate = page + 1;
        if (nextPate == _StateBtns.Count)
            nextPate = 0;
        ShowPage(nextPate);
    }

    public void ShowPage(int state)
    {
        for (int i = 0; i < _StateBtns.Count; ++i)
        {
            if (i == state)
            {
                _StateBtns[i].gameObject.SetActive(true);
                if (_StatePanels.Count > i && _StatePanels[i] != null)
                {
                    _StatePanels[i].SetActive(true);
                }
            }
            else
            {
                _StateBtns[i].gameObject.SetActive(false);
                if (_StatePanels.Count > i && _StatePanels[i] != null)
                {
                    _StatePanels[i].SetActive(false);
                }
            }

            
        }
    }
}
