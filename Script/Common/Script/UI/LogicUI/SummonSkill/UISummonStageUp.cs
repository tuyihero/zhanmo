using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UISummonStageUp : UIBase
{

    #region static funs

    public static void ShowAsyn(SummonMotionData summonDatas)
    {
        Hashtable hash = new Hashtable();
        hash.Add("SummonData", summonDatas);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UISummonStageUp, UILayer.Sub2PopUI, hash);
    }

    public static void ShowAddAttr(int idx)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISummonStageUp>(UIConfig.UISummonStageUp);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        //instance.UpdateExp(summonData, exp);
    }

    public static void Refresh()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISummonStageUp>(UIConfig.UISummonStageUp);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.ShowItem();
    }

    #endregion

    #region info

    public UISummonSkillItem _SummonItem;
    public UIContainerBase _AttrContainer;
    public UICostItem _StageUpCostItem;

    private SummonMotionData _SummonData;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _SummonData = (SummonMotionData)hash["SummonData"];
        ShowItem();
    }

    private void ShowItem()
    {
        _SummonItem.ShowSummonData(_SummonData);

        RefreshAttrs();
        RefreshStageUpCost();
    }

    private void RefreshAttrs()
    {
        //var maxAttr = _SummonData.GetStageAttrMax();
        //List<SummonAttrInfo> summonAttrs = new List<SummonAttrInfo>();
        //for (int i = 0; i < _SummonData.Stage + 1; ++i)
        //{
        //    SummonAttrInfo attrInfo = new SummonAttrInfo();
        //    attrInfo.AttrIdx = i;
        //    attrInfo.CurValue = _SummonData.StageAttrs[i];
        //    attrInfo.MaxValue = maxAttr[i];
        //    attrInfo.AttrEnum = SummonMotionData.StageAttrEnums[i];

        //    summonAttrs.Add(attrInfo);
        //}

        //Hashtable hash = new Hashtable();
        //hash.Add("SummonMotionData", _SummonData);
        //_AttrContainer.InitContentItem(summonAttrs, null, hash);
    }

    private void RefreshStageUpCost()
    {
        //string costItemID = "";
        //int costItemCnt = 0;
        //SummonSkillData.Instance.IsStageLvUpItemEnough(_SummonData, out costItemID, out costItemCnt);
        //_StageUpCostItem.ShowCost(costItemID, costItemCnt);
    }

    public void AddAttr(int idx)
    {
        //if (_SummonData == null)
        //    return;

        //SummonSkillData.Instance.StageAddAttr(_SummonData, idx);

        //RefreshAttrs();
        //RefreshStageUpCost();
    }

    #endregion

    #region stage UP


    public void BtnStageUp()
    {
        //SummonSkillData.Instance.StageLevelUp(_SummonData);
        //ShowItem();
    }

    #endregion


}

