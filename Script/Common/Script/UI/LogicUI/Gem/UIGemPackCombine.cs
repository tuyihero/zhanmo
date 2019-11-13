using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIGemPackCombine : UIBase
{

    #region 
    

    #endregion

    #region 

    public void OnEnable()
    {
        ShowPackItems();
        InitCopyPack();

        _BtnCombineAll.SetActive(false);
        foreach (var gemItem in GemData.Instance.PackGemDatas._PackItems)
        {
            if (gemItem.ItemStackNum >= 10)
            {
                _BtnCombineAll.SetActive(true);
                break;
            }
        }

        //UIGemPack.RefreshPack();
    }

    private void ShowPackItems()
    {
        Hashtable exHash = new Hashtable();
        exHash.Add("DragPack", this);

        for (int i = 0; i < _CombinePack.Count; ++i)
        {
            Hashtable hash = new Hashtable();
            hash.Add("InitObj", null);
            hash.Add("DragPack", this);
            _CombinePack[i].Show(hash);
            _CombinePack[i]._InitInfo = null;
            _CombinePack[i]._PanelClickEvent += ShowGemTooltipsLeft;
        }
        //_BackPack.Show(null);
    }

    public void RefreshItems()
    {
        UIGemPack.RefreshPack();
    }

    private void ShowGemTooltipsLeft(UIItemBase uiItem)
    {
        UIGemItem uiGemItem = uiItem as UIGemItem;
        if (uiGemItem == null || uiGemItem.ItemGem == null)
            return;

        int idx = _CombinePack.IndexOf(uiGemItem);
        _CopyPack[idx] = null;
        _CombineGems[idx] = null;
        uiGemItem.ShowGem(null, 0);

        RefreshItems();
    }

    public void ShowGemTooltipsRight(UIGemItem gemItem)
    {
        if (gemItem.TempNum == 0 && !gemItem.ItemGem.IsGemExtra())
            return;

        if (gemItem.ItemGem.IsGemExtra() && _CopyPack.Contains(gemItem))
            return;

        int emptyPos = -1;
        for (int i = 0; i < _CombinePack.Count; ++i)
        {
            if (_CombinePack[i].ItemGem == null)
            {
                emptyPos = i;
                break;
            }
        }

        if (emptyPos < 0)
            return;

        _CombinePack[emptyPos].ShowGem(gemItem.ItemGem, 0);
        _CombineGems[emptyPos] = gemItem.ItemGem;
        _CopyPack[emptyPos] = gemItem;
        RefreshItems();
    }

    public void AutoFitCombine(Tables.GemTableRecord resultGemRecord)
    {
        var gemPack = UIGemPack.GetGemPack();
        if (gemPack == null)
            return;

        ResetPacket();
        for (int i = 0; i < resultGemRecord.Combine.Count; ++i)
        {
            if (resultGemRecord.Combine[i] > 0)
            {
                string matGemData = resultGemRecord.Combine[i].ToString();
                gemPack.ForeachActiveItem<UIGemItem>((uiGemItem) =>
                {
                    if (uiGemItem.ItemGem.ItemDataID == matGemData)
                    {
                        ShowGemTooltipsRight(uiGemItem);
                        return;
                    }
                });
            }
        }
    }

    #endregion

    #region 

    public List<UIGemItem> _CombinePack;
    public List<ItemGem> _CombineGems;
    public GameObject _BtnCombineAll;

    private List<UIGemItem> _CopyPack;

    private void InitCopyPack()
    {
        //if (_CopyPack == null)
        {
            _CopyPack = new List<UIGemItem>();
            _CombineGems = new List<ItemGem>();
            for (int i = 0; i < _CombinePack.Count; ++i)
            {
                _CopyPack.Add(null);
                _CombineGems.Add(null);
            }
        }
    }

    private void ResetPacket()
    {
        for (int i = 0; i < _CombinePack.Count; ++i)
        {
            _CombinePack[i].ShowGem(null, 0);
        }

        InitCopyPack();
        RefreshItems();
        
    }

    public void OnBtnCombine()
    {
        List<ItemGem> combines = new List<ItemGem>();
        for (int i = 0; i < _CopyPack.Count; ++i)
        {
            if (_CopyPack[i] != null)
            {
                combines.Add(_CopyPack[i].ItemGem);
            }
        }

        if (GemData.Instance.CombineV2(combines))
        {
            ResetPacket();
        }
    }

    public void OnBtnShowFormulas()
    {
        UIGemCombineSet.ShowAsyn();
    }

    public void OnBtnCombineAll()
    {
        //if (_CopyPack[0].ItemGem != null && _CopyPack[0].ItemGem.IsVolid()
        //    && _CopyPack[0].ItemGem.ItemDataID == _CopyPack[1].ItemGem.ItemDataID
        //    && _CopyPack[0].ItemGem.ItemDataID == _CopyPack[2].ItemGem.ItemDataID
        //    )
        //{
        //    string dictStr = Tables.StrDictionary.GetFormatStr(30007, Tables.StrDictionary.GetFormatStr(_CopyPack[0].ItemGem.CommonItemRecord.NameStrDict));
        //    UIMessageBox.Show(dictStr, () =>
        //    {
        //        GemData.Instance.GemCombineSameAll(_CopyPack[0].ItemGem);
        //        ResetPacket();
        //    }, null);

        //}
        //else
        //{
        //    UIMessageTip.ShowMessageTip(30008);
        //}

        CombineAll();
        ResetPacket();
    }

    public static void CombineAll()
    {
        foreach (var gemItem in GemData.Instance.PackExtraGemDatas._PackItems)
        {
            CombineAll(gemItem);
        }
    }

    private static List<List<string>> _DefaultCombine = new List<List<string>>()
    {
        new List<string>() { "70001", "70002" },
        new List<string>() { "70007", "70003" },
        new List<string>() { "70008", "70004" },
        new List<string>() { "70009", "70005" },
        new List<string>() { "70010", "70006" },
    };

    private static void CombineAll(ItemGem baseGem)
    {
        List<ItemGem> combines = new List<ItemGem>();
        string exGemID = "";
        foreach (var gemRecord in Tables.TableReader.GemTable.Records)
        {
            if (gemRecord.Value.AttrValue.AttrParams[0] == baseGem.ExAttr)
            {
                exGemID = gemRecord.Value.Id;
            }
        }
        while (true)
        {
            combines.Clear();
            ItemGem gemItem1 = GemData.Instance.PackExtraGemDatas.GetItem(baseGem.ItemDataID);
            ItemGem gemItemMat1 = GemData.Instance.PackGemDatas.GetItem(baseGem.ItemDataID);
            ItemGem gemItemMat11 = GemData.Instance.PackGemDatas.GetItem(exGemID);

            if (gemItem1 == null)
            {
                if (gemItemMat1 != null && gemItemMat1.ItemStackNum >= 3)
                {
                    combines.Add(gemItemMat1);
                    combines.Add(gemItemMat1);
                    combines.Add(gemItemMat1);
                    GemData.Instance.CombineV2(combines);
                    continue;
                }
            }
            else
            {
                if (gemItemMat1 != null && gemItemMat1.ItemStackNum >= 2)
                {
                    combines.Add(gemItem1);
                    combines.Add(gemItemMat1);
                    combines.Add(gemItemMat1);
                    GemData.Instance.CombineV2(combines);
                    continue;
                }
                else if (gemItemMat11 != null && gemItemMat11.ItemStackNum >= 2)
                {
                    combines.Add(gemItem1);
                    combines.Add(gemItemMat11);
                    combines.Add(gemItemMat11);
                    GemData.Instance.CombineV2(combines);
                    continue;
                }
            }
            return;
        }
    }
    #endregion

}

