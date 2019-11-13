using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class UIEquipPack : UIBase,IDragablePack
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIEquipPack, UILayer.PopUI, hash);
    }

    public static void RefreshBagItems()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIEquipPack>(UIConfig.UIEquipPack);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshEquipItems();
    }

    

    #endregion

    #region 

    public UIContainerBase _EquipContainer;
    public UIBackPack _BackPack;
    public UITagPanel _TagPanel;
    public UIBackPackItem _OtherRoleWeapon;
    public Text _Combat;
    public Image _CharIcon;

    public override void Init()
    {
        base.Init();

        //_BackPack = UIBackPack.GetUIBackPackInstance(transform);
        _BackPack.OnShowPage(UIBackPack.BackPackPage.PAGE_EQUIP);
        _BackPack._OnItemSelectCallBack = ShowBackPackSelectItem;
        _BackPack._OnDragItemCallBack = OnDragItem;
        _BackPack._IsCanDropItemCallBack = IsCanDropItem;
        RefreshCombat();
        
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _TagPanel.ShowPage(0);
        ShowEquipPackItems();
        _BackPack.OnShowPage(UIBackPack.BackPackPage.PAGE_EQUIP);
        RefreshCombat();

        _OtherRoleWeapon.gameObject.SetActive(false);
        ResourceManager.Instance.SetImage(_CharIcon, RoleData.SelectRole.IconName);

        RefreshBtn();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnShowPage(int page)
    {
        if (page == 2)
        {
            _BackPack.OnShowAllEquip();
        }
        else if (page == 0)
        {
            _BackPack.OnShowPage(UIBackPack.BackPackPage.PAGE_EQUIP);
            RefreshCombat();
            //ShowEquipPackItems();
        }
        else if (page == 1)
        {
            _BackPack.OnShowPage(UIBackPack.BackPackPage.PAGE_LEGENDARY);
        }
    }

    private void ShowEquipPackItems()
    {
        Hashtable exHash = new Hashtable();
        exHash.Add("DragPack", this);

        _EquipContainer.InitContentItem(PlayerDataPack.Instance._SelectedRole.EquipList, ShowEquipPackTooltips, exHash);
        _BackPack.Show(null);
    }

    public void RefreshEquipItems()
    {
        _EquipContainer.RefreshItems();
        _BackPack.RefreshItems();

        RefreshCombat();
    }

    private void RefreshCombat()
    {
        int combatValue = 0;
        foreach (var equipItem in PlayerDataPack.Instance._SelectedRole.EquipList)
        {
            if (equipItem != null && equipItem.IsVolid())
            {
                combatValue += equipItem.CombatValue;
            }
        }

        _Combat.text = combatValue.ToString();
    }

    public void ShowBackPackSelectItem(ItemBase itemObj)
    {
        if (_UILegendaryPack.isActiveAndEnabled)
        {
            _UILegendaryPack.ShowBackPackSelectItem(itemObj);
        }
        else if (_UIEquipPackRefresh.isActiveAndEnabled)
        {
            _UIEquipPackRefresh.OnSelectedEquip(itemObj);
        }
        else
        {
            ItemEquip equipItem = itemObj as ItemEquip;
            if (equipItem != null && equipItem.IsVolid())
            {
                UIEquipTooltips.ShowAsyn(equipItem, new ToolTipFunc[1] { new ToolTipFunc(10003, PutOnEquip) });
            }
            else if (itemObj.IsVolid())
            {
                UIItemTooltips.ShowAsyn(itemObj);
            }
        }
    }

    private void ShowEquipPackTooltips(object equipObj)
    {
        ItemEquip equipItem = equipObj as ItemEquip;
        if (equipItem == null || !equipItem.IsVolid())
            return;

        UIEquipTooltips.ShowAsyn(equipItem, new ToolTipFunc[1] { new ToolTipFunc(10004, PutOffEquip) });
    }

    private void ShowOtherWeaponTooltips(object equipObj)
    {
        ItemEquip equipItem = equipObj as ItemEquip;
        if (equipItem == null || !equipItem.IsVolid())
            return;

        UIEquipTooltips.ShowAsyn(equipItem);
    }
    #endregion

    #region 

    private void ItemRefresh(object sender, Hashtable args)
    {
        RefreshEquipItems();
    }

    public bool IsCanDragItem(UIDragableItemBase dragItem)
    {
        if (!dragItem.ShowedItem.IsVolid())
            return false;
        return true;
    }

    public bool IsCanDropItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem)
    {
        if (dragItem._DragPackBase == dropItem._DragPackBase)
            return false;

        if (dragItem._DragPackBase == this)
        {
            if (!dropItem.ShowedItem.IsVolid())
                return true;

            if (dropItem.ShowedItem is ItemEquip)
            {
                var equip = (ItemEquip)dropItem.ShowedItem;
                var slot = PlayerDataPack.Instance._SelectedRole.EquipList.IndexOf(equip);
                if (slot < 0)
                    return false;

                return PlayerDataPack.Instance._SelectedRole.IsCanEquipItem((Tables.EQUIP_SLOT)slot, equip);
            }
        }
        else if (dropItem._DragPackBase == this)
        {
            if (dragItem.ShowedItem is ItemEquip)
            {
                var equip = (ItemEquip)dragItem.ShowedItem;
                var slot = PlayerDataPack.Instance._SelectedRole.EquipList.IndexOf(dropItem.ShowedItem as ItemEquip);
                if (slot < 0)
                    return false;

                return PlayerDataPack.Instance._SelectedRole.IsCanEquipItem((Tables.EQUIP_SLOT)slot, equip);
            }
        }

        return true;
    }

    public void OnDragItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem)
    {
        if (dragItem._DragPackBase == this)
        {
            var dragEquip = dragItem.ShowedItem as ItemEquip;

            if (!dropItem.ShowedItem.IsVolid())
            {
                PlayerDataPack.Instance._SelectedRole.PutOffEquip(dragEquip.EquipItemRecord.Slot, dragEquip);
                return;
            }

            if (dropItem.ShowedItem is ItemEquip)
            {
                PutOnEquip(dropItem.ShowedItem);
            }
            else
            {
                PutOffEquip(dragItem.ShowedItem);
            }
        }
        else if (dropItem._DragPackBase == this)
        {
            if (dragItem.ShowedItem is ItemEquip)
            {
                PutOnEquip(dragItem.ShowedItem);
            }
        }
    }

    private void PutOnEquip(ItemBase itemBase)
    {
        if (!itemBase.IsVolid())
            return;

        var itemEquip = itemBase as ItemEquip;
        if (itemEquip != null)
        {
            bool isNeedShowSlot = (!itemEquip.IsMatchRole(RoleData.SelectRole.Profession)) && itemEquip.EquipItemRecord.Slot == Tables.EQUIP_SLOT.WEAPON;

            if (PlayerDataPack.Instance._SelectedRole.PutOnEquip(itemEquip.EquipItemRecord.Slot, itemEquip))
            {

                if (isNeedShowSlot)
                {
                    UIMessageBox.ShowWithDontShotTodayTips(Tables.StrDictionary.GetFormatStr(1015), "EquipDontMatch");
                    var equipedItem = RoleData.SelectRole.GetEquipItemOtherWeaon();
                    {
                        Hashtable hash = new Hashtable();
                        hash.Add("InitObj", equipedItem);
                        _OtherRoleWeapon.Show(hash);
                        _OtherRoleWeapon._InitInfo = equipedItem;
                        _OtherRoleWeapon._ClickEvent += ShowOtherWeaponTooltips;
                        _OtherRoleWeapon.gameObject.SetActive(true);
                    }
                }
            }

        }
    }

    private void PutOffEquip(ItemBase itemBase)
    {
        if (!itemBase.IsVolid())
            return;

        var itemEquip = itemBase as ItemEquip;
        if (itemEquip != null)
        {
            PlayerDataPack.Instance._SelectedRole.PutOffEquip(itemEquip.EquipItemRecord.Slot, itemEquip);
        }
    }

    #endregion

    #region page collect

    public UILegendaryPack _UILegendaryPack;

    #endregion

    #region page refresh

    public UIEquipPackRefresh _UIEquipPackRefresh;

    #endregion

    #region func open

    public GameObject _RefreshLock;

    public void RefreshBtn()
    {
        if (RoleData.SelectRole.TotalLevel >= GameDataValue.EQUIP_REFRESH)
        {
            _RefreshLock.gameObject.SetActive(false);
        }
        else
        {
            _RefreshLock.gameObject.SetActive(true);
        }
    }

    #endregion
}

