
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;


public class UIFiveElementCorePackItem : /*UIDragableItemBase*/ UIPackItemBase
{

    private ItemFiveElement _ItemFiveElement;

    public override void ShowItem(ItemBase showItem)
    {
        if (showItem == null || !showItem.IsVolid())
        {
            ClearItem();
            return;
        }

        _Icon.gameObject.SetActive(true);

    }

}

