using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

 



public class UIDropNameItem : UIItemBase
{
    public Text _DropName;

    private RectTransform _RectTransform;
    private DropItem _DropItem;
    private Transform _FollowTransform;
    private Vector3 _HeightDelta;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _DropItem = hash["InitObj"] as DropItem;
        if (_DropItem == null)
        {
            UIDropNamePanel.HideDropItem(this);
            return;
        }
        _RectTransform = GetComponent<RectTransform>();
        _FollowTransform = _DropItem.transform;

        _HeightDelta.x = 0;
        _HeightDelta.z = 0;
        _HeightDelta.y = 1.5f;

        _DropName.text = _DropItem._DropName;
        _Picked = false;
    }


    public void Update()
    {
        if (_FollowTransform == null)
        {
            UIDropNamePanel.HideDropItem(this);
        }
        else
        {
            _RectTransform.anchoredPosition = UIManager.Instance.WorldToScreenPoint(_FollowTransform.position + _HeightDelta);
        }
    }

    #region 

    private bool _Picked = false;

    public override void OnItemClick()
    {
        if (_Picked)
            return;

        _Picked = true;
        base.OnItemClick();

        _DropItem.PickDropItem();
    }

    #endregion
}

