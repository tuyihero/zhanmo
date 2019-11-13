using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDragablePack
{
    void OnDragItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem);

    bool IsCanDropItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem);

    bool IsCanDragItem(UIDragableItemBase dragItem);

}
