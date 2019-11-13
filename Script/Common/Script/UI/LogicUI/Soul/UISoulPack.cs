using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UISoulPack : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UISoulPack, UILayer.PopUI, hash);
    }

    public static void RefreshBagItems()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISoulPack>(UIConfig.UISoulPack);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshItems();
    }

    #endregion

    #region 

    public UIContainerBase _SoulContainer;
    //public UIContainerBase _MaterialContainer;

    public UISoulItem[] _SoulPack;

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        ShowPackItems();
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void ShowPackItems()
    {
        Hashtable exHash = new Hashtable();
        exHash.Add("UIBagPack", this);

        //_EquipContainer.InitContentItem(PlayerDataPack.Instance._SelectedRole._EquipList, ShowEquipPackTooltips, exHash);
        //_BackPack.Show(null);
    }

    public void RefreshItems()
    {
        //_EquipContainer.RefreshItems();
        //_BackPack.RefreshItems();
    }

    private void ShowGemTooltipsLeft(object equipObj)
    {
        ItemEquip equipItem = equipObj as ItemEquip;
        if (equipItem == null || !equipItem.IsVolid())
            return;

        //UIEquipTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInEquipPack);
    }

    private void ShowGemTooltipsRight(object equipObj)
    {
        ItemEquip equipItem = equipObj as ItemEquip;
        if (equipItem == null || !equipItem.IsVolid())
            return;

        //UIEquipTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInEquipPack);
    }

    private void ShowMaterialTooltips(object equipObj)
    {
        ItemEquip equipItem = equipObj as ItemEquip;
        if (equipItem == null || !equipItem.IsVolid())
            return;

        //UIEquipTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInEquipPack);
    }
    #endregion

    #region 

    private void ItemRefresh(object sender, Hashtable args)
    {
        RefreshItems();
    }

    #endregion

}

