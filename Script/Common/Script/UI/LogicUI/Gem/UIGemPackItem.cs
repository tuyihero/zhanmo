
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;

public class UIGemPackItem : UIItemSelect
{
    public Image _Icon;
    public Image _Quality;
    public Text _Name;
    public Text _Attr;
    public Text _ExAttr;
    public Text _Level;
    public GameObject _UsingGO;

    private ItemGem _ItemGem;
    public ItemGem ItemGem
    {
        get
        {
            return _ItemGem;
        }
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var showItem = (ItemGem)hash["InitObj"];
        ShowGem(showItem);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowGem(_ItemGem);
    }

    public void ShowGem(ItemGem showItem)
    {
        _ItemGem = showItem;

        _Icon.gameObject.SetActive(true);
        _Quality.gameObject.SetActive(false);
        _Name.text = Tables.StrDictionary.GetFormatStr(30010, _ItemGem.ItemStackNum);
        _Attr.text = showItem.GemAttr[0].GetAttrStr();
        if (showItem.IsGemExtra())
        {
            _ExAttr.gameObject.SetActive(true);
            _ExAttr.text = showItem.GemAttr[1].GetAttrStr();
        }
        else
        {
            _ExAttr.gameObject.SetActive(false);
        }
        _Level.text = "Lv." + showItem.Level.ToString();

        _UsingGO.SetActive(GemData.Instance.IsEquipedGem(_ItemGem));
    }
    
}

