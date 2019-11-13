using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIFiveElementExtra : UIBase
{

    #region static funs

    public static void ShowAsyn(ItemFiveElement extraItem)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ExtraItem", extraItem);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFiveElementExtra, UILayer.SubPopUI, hash);
    }

    public static void RefreshPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFiveElement>(UIConfig.UIFiveElementExtra);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance._UIFiveElementExtra.RefreshInfo();
    }

    public static void RefreshCost()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFiveElement>(UIConfig.UIFiveElementExtra);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance._UIFiveElementExtra.RefreshCostMoney();
    }

    #endregion

    #region 

    public List<UIFiveElementAttrItem> _Attrs;

    public UIContainerSelect _ElementItems;

    private int _ShowingIdx;
    private ItemFiveElement _SelectedMatItem;

    public void RefreshInfo()
    {
        ShowItemInfo(_ShowingIdx);
        ShowMaterialItems();

        RefreshFilter();
    }

    public void ShowItemByIndex(int idx)
    {
        _ShowingIdx = idx;
        ShowItemInfo(_ShowingIdx);

        ResetFilter();
        ShowMaterialItems();
        _NeedRefresh = false;

        InitAttrFilter();
    }

    private void ShowItemInfo(int showIdx)
    {
        _ShowingIdx = showIdx;
        if (showIdx < 0)
        {
            _ShowingIdx = 0;
        }

        var elementItem = FiveElementData.Instance._UsingElements[showIdx];
        if (elementItem == null)
        {
            elementItem = FiveElementData.Instance._UsingElements[0];
        }

        for (int i = 0; i < _Attrs.Count; ++i)
        {
            _Attrs[i].InitAttrItem(showIdx, i);
        }
    }

    private void RefreshMaterialItems()
    { }

    private void ShowMaterialItems()
    {
        List<ItemFiveElement> matItems = new List<ItemFiveElement>();
        foreach (var itemElement in FiveElementData.Instance._PackElements._PackItems)
        {
            if (itemElement == null || !itemElement.IsVolid())
            {
                continue;
            }

            if (!IsFilterAttr(itemElement.EquipExAttrs[0].AttrParams[0]))
            {
                continue;
            }

            matItems.Add(itemElement);
        }
        matItems.Sort((element1, element2) =>
        {
            if (element1.EquipExAttrs[0].AttrParams[0] > element2.EquipExAttrs[0].AttrParams[0])
            {
                return 1;
            }
            else if (element1.EquipExAttrs[0].AttrParams[0] < element2.EquipExAttrs[0].AttrParams[0])
            {
                return -1;
            }
            else
            {
                if (element1.Level > element2.Level)
                {
                    return 1;
                }
                else if (element1.Level < element2.Level)
                {
                    return -1;
                }
                return 0;
            }
        });

        List<ItemFiveElement> selectedItems = null;
        if (matItems.Contains(_SelectedMatItem))
        {
            selectedItems = new List<ItemFiveElement>();
            selectedItems.Add(_SelectedMatItem);
        }
        else
        {
            _SelectedMatItem = null;
            if (matItems.Count > 0)
            {
                selectedItems = new List<ItemFiveElement>();
                selectedItems.Add(matItems[0]);
            }
        }

        _ElementItems.InitSelectContent(matItems, selectedItems, OnMatItemClick);
    }

    private void OnMatItemClick(object itemObj)
    {
        ItemFiveElement itemElement = itemObj as ItemFiveElement;

        _SelectedMatItem = itemElement;

        RefreshCostMoney();
    }

    #endregion

    #region filter

    public GameObject _FilterPanel;
    public UIContainerBase _FilterContainer;
    public GameObject _FilterGO;
    public Toggle _FilterActedAttr;
    public Toggle _FilterUnActedAttr;

    private List<int> _FilterAttrs = null;
    private List<int> _FilterTroggleAttrs = null;

    public void ResetFilter()
    {
        _FilterAttrs = null;
        _FilterTroggleAttrs = null;
        _FilterGO.SetActive(false);

        _FilterActedAttr.isOn = false;
        _FilterUnActedAttr.isOn = false;
    }

    public void InitAttrFilter()
    {
        _FilterContainer.InitContentItem(GameDataValue._FiveElementAttrs);
        _FilterAttrs = null;
        _FilterGO.SetActive(false);
    }

    public void OnBtnShowFilter()
    {
        _FilterPanel.gameObject.SetActive(true);
    }

    public void OnBtnHideFilter()
    {
        _FilterPanel.gameObject.SetActive(false);

        if (_FilterAttrs == null)
        {
            _FilterAttrs = new List<int>();
        }
        _FilterAttrs.Clear();

        _FilterContainer.ForeachActiveItem<UIFiveElementExtraFilterItem>((filterItem) =>
        {
            if (filterItem._Toggle.isOn)
            {
                _FilterAttrs.Add(filterItem.AttrID);
            }
        });

        ShowMaterialItems();

        if (_FilterAttrs.Count != GameDataValue._FiveElementAttrs.Count)
        {
            _FilterGO.SetActive(true);
        }
        else
        {
            _FilterGO.SetActive(false);
        }

        _FilterActedAttr.isOn = false;
        _FilterUnActedAttr.isOn = false;
    }

    public void OnToggleFilterAct(bool isOn)
    {
        if (isOn)
        {
            _FilterUnActedAttr.isOn = false;
        }
        RefreshFilter();
    }

    public void OnToggleFilterUnAct(bool isOn)
    {
        if (isOn)
        {
            _FilterActedAttr.isOn = false;
        }
        RefreshFilter();
    }

    private void RefreshFilter()
    {
        if (_FilterUnActedAttr.isOn)
        {
            FilterTroggleAttr(false);
        }
        else if (_FilterActedAttr.isOn)
        {
            FilterTroggleAttr(true);
        }
        else
        {
            if (_FilterTroggleAttrs == null)
            {
                _FilterTroggleAttrs = new List<int>();
            }
            _FilterTroggleAttrs.Clear();

            ShowMaterialItems();
        }
    }

    private void FilterTroggleAttr(bool isActed)
    {
        if (_FilterTroggleAttrs == null)
        {
            _FilterTroggleAttrs = new List<int>();
        }
        _FilterTroggleAttrs.Clear();

        foreach (var elementAttr in GameDataValue._FiveElementAttrs)
        {
            bool containsAttr = false;
            foreach (var exAttr in FiveElementData.Instance._UsingElements[_ShowingIdx].EquipExAttrs)
            {
                if (exAttr.AttrParams[0] == (int)elementAttr.AttrID)
                {
                    containsAttr = true;
                    break;
                }
            }

            if (!isActed && !containsAttr)
            {
                _FilterTroggleAttrs.Add((int)elementAttr.AttrID);
            }
            else if(isActed && containsAttr)
            {
                _FilterTroggleAttrs.Add((int)elementAttr.AttrID);
            }
        }

        ShowMaterialItems();
    }

    private void FilterActedAttr()
    {
        if (_FilterTroggleAttrs == null)
        {
            _FilterTroggleAttrs = new List<int>();
        }
        _FilterTroggleAttrs.Clear();

        foreach (var elementAttr in GameDataValue._FiveElementAttrs)
        {
            bool containsAttr = false;
            foreach (var exAttr in FiveElementData.Instance._UsingElements[_ShowingIdx].EquipExAttrs)
            {
                if (exAttr.AttrParams[0] == (int)elementAttr.AttrID)
                {
                    containsAttr = true;
                    break;
                }
            }

            if (!containsAttr)
            {
                _FilterTroggleAttrs.Add((int)elementAttr.AttrID);
            }
        }

        ShowMaterialItems();
    }

    private bool IsFilterAttr(int attrID)
    {
        bool isFilter = true;
        if (_FilterUnActedAttr.isOn || _FilterActedAttr.isOn)
        {
            isFilter &= _FilterTroggleAttrs.Contains(attrID);
        }

        if (_FilterAttrs != null)
        {
            isFilter &= _FilterAttrs.Contains(attrID);
        }

        return isFilter;
    }

    #endregion

    #region opt

    public UICurrencyItem _CostMoney;

    private bool _NeedRefresh = false;

    public void RefreshCostMoney()
    {
        if (_SelectedMatItem == null)
        {
            _CostMoney.ShowCostCurrency(MONEYTYPE.GOLD, 0);
        }
        else
        {
            var elementItem = FiveElementData.Instance._UsingElements[_ShowingIdx];
            _CostMoney.ShowCostCurrency(MONEYTYPE.GOLD, GameDataValue.GetElementExtraCostMoney(elementItem), false);
        }
    }

    public void OnBtnExtraOK()
    {
        if (_ShowingIdx < 0)
            return;

        if (_SelectedMatItem == null)
        {
            UIMessageTip.ShowMessageTip(1300001);
            return;
        }

        int selectedIdx = UIFiveElement.GetSelectedIdx();
        if (selectedIdx < 0)
            return;

        _NeedRefresh = FiveElementData.Instance.Extract(_SelectedMatItem, selectedIdx);
        if (_NeedRefresh)
        {
            RefreshInfo();
        }
    }

    public void OnBtnSell()
    {
        Dictionary<int, Vector2> selectLevels = new Dictionary<int, Vector2>();
        foreach (var element in FiveElementData.Instance._PackElements._PackItems)
        {
            int elementLevel = element.GetLevel();
            if (elementLevel <= 0)
                continue;

            if (selectLevels.ContainsKey(elementLevel))
            {
                continue;
            }

            selectLevels.Add(elementLevel, new Vector2(elementLevel, elementLevel));
        }
        UISellShopPack.ShowSellLevelSync(FiveElementData.Instance._PackElements.ToItemBases(), new List<Vector2>(selectLevels.Values), GetSellPrice, SellEquips);
    }

    public int GetSellPrice(List<ItemBase> items)
    {
        int totalMoney = 0;
        for (int i = 0; i < items.Count; ++i)
        {
            ItemFiveElement itemElement = items[i] as ItemFiveElement;
            if (itemElement != null)
            {
                totalMoney += FiveElementData.Instance.GetElementSellMoney(itemElement);
            }
        }

        return totalMoney;
    }

    public void SellEquips(List<ItemBase> items)
    {
        FiveElementData.Instance.SellElements(items);
    }
    #endregion



}

