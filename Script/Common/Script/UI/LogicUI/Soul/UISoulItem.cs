
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;

public class UISoulItem : UIItemSelect
{
    public Image _BG;
    public Image _Icon;
    public Image _Quality;
    public Text _Lv;

    protected ItemBase _ShowItem;
    public ItemBase ShowItem
    {
        get
        {
            return _ShowItem;
        }
    }

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (ItemBase)hash["InitObj"];
        ShowEquip(showItem);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowEquip(_ShowItem);
    }

    public void ShowEquip(ItemBase showItem)
    {
        if (showItem == null)
        {
            ClearItem();
            return;
        }

        _ShowItem = showItem;
        if (!showItem.IsVolid())
        {
            ClearItem();
            return;
        }

        if (_Lv != null)
        {
            {
                _Lv.text = _ShowItem.ItemStackNum.ToString();
            }
        }
        _Icon.gameObject.SetActive(true);
    }

    private void ClearItem()
    {
        _Icon.gameObject.SetActive(false);
        _Quality.gameObject.SetActive(false);
        if (_Lv != null)
        {
            _Lv.text = "";
        }
    }

    #region 

    public override void OnItemClick()
    {
        base.OnItemClick();
    }

    #endregion
}

