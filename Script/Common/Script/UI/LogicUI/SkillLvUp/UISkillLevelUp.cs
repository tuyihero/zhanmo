using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using Tables;

public class UISkillLevelUp : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UISkillLevelUp, UILayer.PopUI, hash);
    }

    public static void RefreshSkillItems(int profession)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISkillLevelUp>(UIConfig.UISkillLevelUp);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RereshSkillItems(profession);
    }

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        InitSkillItems();
    }

    #endregion

    #region 

    public UITagPanel _TagPanel;

    private List<UISkillLevelItem> _SkillItems;

    public void InitSkillItems()
    {
        if (_SkillItems != null)
            return;

        var skilLItems = GetComponentsInChildren<UISkillLevelItem>(true);
        _SkillItems = new List<UISkillLevelItem>(skilLItems);
    }

    public void OnShowPage(int pageIdx)
    {
        
    }

    public void RereshSkillItems(int profession)
    {
        foreach (var skillItem in _SkillItems)
        {
            if (skillItem.SkillTab == null)
                continue;

            if (skillItem.SkillTab.Profession == profession)
            {
                skillItem.Refresh();
            }
        }
    }

    public void ResetSkill()
    {
        SkillData.Instance.ResetAllSkills();
        RereshSkillItems((int)RoleData.SelectRole.Profession);
    }

    #endregion

}

