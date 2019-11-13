
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;


public class UIGemItem : /*UIDragableItemBase*/ UIPackItemBase
{
    public GameObject _UsingGO;
    public Text _TestAttrName;

    private ItemGem _ItemGem;
    public ItemGem ItemGem
    {
        get
        {
            return _ItemGem;
        }
    }

    public enum GemRefreshType
    {
        NONE,
        PUNCH,
        COMBINE,
    }

    private GemRefreshType _RefreshType;

    public override void Show(Hashtable hash)
    {
        //base.Show(hash);

        var showItem = (ItemGem)hash["InitObj"];
        GemRefreshType refreshType = GemRefreshType.NONE;
        if (hash.ContainsKey("RefreshType"))
        {
            refreshType = (GemRefreshType)hash["RefreshType"];
        }
        ShowGem(showItem, refreshType);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowGem(_ShowedItem as ItemGem, _RefreshType);
    }

    public override void Refresh(Hashtable hash)
    {
        base.Refresh();

        RefreshRedTip();
    }

    public void ShowGem(ItemGem showItem, GemRefreshType refreshType)
    {
        _ItemGem = showItem;
        _InitInfo = showItem;
        _RefreshType = refreshType;

        if (showItem == null)
        {
            ClearRedTip();
            ClearItem();
            return;
        }

        _ShowedItem = showItem;
        
        if (!showItem.IsVolid())
        {
            ClearRedTip();
            ClearItem();
            return;
        }

        RefreshNum();

        if (_UsingGO != null)
        {
            if (refreshType != GemRefreshType.NONE && GemData.Instance.EquipedGemDatas.Contains(showItem))
            {
                _UsingGO.SetActive(true);
            }
            else
            {
                _UsingGO.SetActive(false);
            }
        }

        if (_TestAttrName != null)
        {
            _TestAttrName.text = RoleAttrImpactBaseAttr.GetAttrDesc(showItem.GemAttr[0].AttrParams);
        }

        _Quality.gameObject.SetActive(true);
        if (showItem.IsGemExtra())
        {
            ResourceManager.Instance.SetImage(_Quality, CommonDefine.GetQualityIcon(Tables.ITEM_QUALITY.BLUE));
        }
        else
        {
            ResourceManager.Instance.SetImage(_Quality, CommonDefine.GetQualityIcon(Tables.ITEM_QUALITY.GREEN));
        }
        _Icon.gameObject.SetActive(true);
        ResourceManager.Instance.SetImage(_Icon, showItem.CommonItemRecord.Icon);

        RefreshRedTip();
    }

    protected override void ClearItem()
    {
        base.ClearItem();

        if (_DisableGO != null)
        {
            _Icon.gameObject.SetActive(false);
            _DisableGO.SetActive(false);
            _UsingGO.SetActive(false);
            _Quality.gameObject.SetActive(false);
        }
    }

    #region 
    public int TempNum;

    public void RefreshNum()
    {
        if (_Num == null)
        {
            return;
        }

        if (_RefreshType == GemRefreshType.COMBINE)
        {
            if (!_ItemGem.IsGemExtra())
            {
                _Num.text = "";
                TempNum = _ShowedItem.ItemStackNum;
                if (UIGemPack.GetUIGemCombine() == null)
                    return;
                var combineGems = UIGemPack.GetUIGemCombine()._CombineGems;
                if (combineGems == null)
                    return;
                for (int i = 0; i < combineGems.Count; ++i)
                {
                    if (combineGems[i] == _ShowedItem)
                    {
                        --TempNum;
                    }
                }


                if (TempNum == _ShowedItem.ItemStackNum)
                {
                    _Num.text = CommonDefine.GetEnableGrayStr(2) + TempNum.ToString() + "</color>";
                }
                else
                {
                    _Num.text = CommonDefine.GetEnableRedStr(0) + TempNum.ToString() + "</color>";

                }
            }
            else
            {
                _Num.text = "";
            }
        }
        else
        {
            _Num.text = "";
        }
    }

    #endregion

    #region redtips

    public GameObject _RedTips;

    public void RefreshRedTip()
    {
        if (_RedTips == null)
            return;

        if (_RefreshType == GemRefreshType.NONE)
        {
            _RedTips.SetActive(false);
            return;
        }

        if (_RefreshType == GemRefreshType.COMBINE)
        {
            var combineGems = UIGemPack.GetUIGemCombine()._CombineGems;
            if (combineGems == null || combineGems.Count < 3 || combineGems[0] == null || !combineGems[0].IsVolid())
            {
                _RedTips.SetActive(GemData.Instance.IsGemCanLvUp(ItemGem));
            }
            else
            {
                _RedTips.SetActive(GemData.Instance.IsGemMatOf(combineGems[0], combineGems[1], ItemGem));
            }
        }
        else
        {
            _RedTips.SetActive(false);
        }
    }

    public void ClearRedTip()
    {
        if (_RedTips == null)
            return;

        _RedTips.SetActive(false);
    }

    public void RefreshGemEquip(int idx)
    {
        if (_RedTips == null)
            return;
        _RedTips.SetActive(GemData.Instance.IsGemSlotCanEquip(idx));
    }

    #endregion
}

