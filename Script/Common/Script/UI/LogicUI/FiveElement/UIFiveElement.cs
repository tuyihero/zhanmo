using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIFiveElement : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFiveElement, UILayer.PopUI, hash);
    }

    public static void RefreshPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFiveElement>(UIConfig.UIFiveElement);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshUsingCoreItems();

    }

    public static int GetSelectedIdx()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFiveElement>(UIConfig.UIFiveElement);
        if (instance == null)
            return -1;

        if (!instance.isActiveAndEnabled)
            return -1;

        return instance._SelectedIdx;

    }

    public static void SetSelectedIdx(int idx)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFiveElement>(UIConfig.UIFiveElement);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.SetSelectIdx(idx);

    }

    #endregion

    #region element pack

    public List<UIFiveElementItem> _UsingElement = new List<UIFiveElementItem>();
    public UIFiveElementExtra _UIFiveElementExtra;
    public UIFiveElementCorePack _UIFiveElementCorePack;

    public int _SelectedIdx = 0;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        RefreshUsingCoreItems();
        OnElementCoreClick(_UsingElement[0]);
    }

    public void RefreshUsingCoreItems()
    {
        for (int i = 0; i < _UsingElement.Count; ++i)
        {
            _UsingElement[i].ShowItem(FiveElementData.Instance._UsingCores[i]);
            Hashtable hash = new Hashtable();
            hash.Add("InitObj", FiveElementData.Instance._UsingCores[i]);
            hash.Add("DragPack", this);
            _UsingElement[i].Show(hash);
            _UsingElement[i]._InitInfo = FiveElementData.Instance._UsingCores[i];
            //_UsingElement[i]._ClickEvent += ShowTooltipsLeft;
            _UsingElement[i]._PanelClickEvent += OnElementCoreClick;
        }
    }
    
    private void OnElementCoreClick(UIItemBase elementItem)
    {
        for (int i = 0; i < _UsingElement.Count; ++i)
        {
            if (_UsingElement[i] == elementItem)
            {
                if (_UIFiveElementExtra.isActiveAndEnabled)
                {
                    _UIFiveElementExtra.ShowItemByIndex(i);
                }
                else if (_UIFiveElementCorePack.isActiveAndEnabled)
                {
                    _UIFiveElementCorePack.ShowItemByIndex(i);
                }
                _UsingElement[i].Selected();
                _SelectedIdx = i;
            }
            else
            {
                _UsingElement[i].UnSelected();
            }
        }
    }

    public void SwitchElement(int direct)
    {
        int switchPos = (int)UIFiveElement.GetSelectedIdx() + direct;
        if (switchPos < 0)
        {
            switchPos = 4;
        }
        if (switchPos > 4)
        {
            switchPos = 0;
        }

        var itemUsing = _UsingElement[switchPos];
        OnElementCoreClick(itemUsing);
    }

    public void SetSelectIdx(int idx)
    {
        OnElementCoreClick(_UsingElement[idx]);
    }

    #endregion


}

