using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIRoleAttr : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIRoleAttr, UILayer.PopUI, hash);
    }

    #endregion

    #region 
    public Image _CharIcon;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        InitRoleAttrs();

        ResourceManager.Instance.SetImage(_CharIcon, RoleData.SelectRole.IconName);

        RefreshFuncBtn();

    }

    #endregion

    #region distrubute attr

    public RoleAttrItem _RoleLevel;

    public UIContainerBase _AttrItemContainer;

    private void InitRoleAttrs()
    {
        _RoleLevel.Show(Tables.StrDictionary.GetFormatStr(1008), RoleData.SelectRole.RoleLevel);
        
        List<AttrPair> pair = new List<AttrPair>();

        pair.Add(new AttrPair(RoleAttrEnum.Strength));
        pair.Add(new AttrPair(RoleAttrEnum.Dexterity));
        pair.Add(new AttrPair(RoleAttrEnum.Intelligence));
        pair.Add(new AttrPair(RoleAttrEnum.Vitality));
        pair.Add(new AttrPair(RoleAttrEnum.Attack));
        pair.Add(new AttrPair(RoleAttrEnum.Defense));
        pair.Add(new AttrPair(RoleAttrEnum.HPMax));
        pair.Add(new AttrPair(RoleAttrEnum.AttackSpeed));
        pair.Add(new AttrPair(RoleAttrEnum.CriticalHitChance));
        pair.Add(new AttrPair(RoleAttrEnum.CriticalHitDamge));
        pair.Add(new AttrPair(RoleAttrEnum.FireAttackAdd));
        pair.Add(new AttrPair(RoleAttrEnum.ColdAttackAdd));
        pair.Add(new AttrPair(RoleAttrEnum.LightingAttackAdd));
        pair.Add(new AttrPair(RoleAttrEnum.WindAttackAdd));
        pair.Add(new AttrPair(RoleAttrEnum.FireResistan));
        pair.Add(new AttrPair(RoleAttrEnum.ColdResistan));
        pair.Add(new AttrPair(RoleAttrEnum.LightingResistan));
        pair.Add(new AttrPair(RoleAttrEnum.WindResistan));
        pair.Add(new AttrPair(RoleAttrEnum.FireEnhance));
        pair.Add(new AttrPair(RoleAttrEnum.ColdEnhance));
        pair.Add(new AttrPair(RoleAttrEnum.LightingEnhance));
        pair.Add(new AttrPair(RoleAttrEnum.WindEnhance));
        pair.Add(new AttrPair(RoleAttrEnum.PhysicDamageEnhance));
        pair.Add(new AttrPair(RoleAttrEnum.IgnoreDefenceAttack));
        pair.Add(new AttrPair(RoleAttrEnum.FinalDamageReduse));

        _AttrItemContainer.InitContentItem(pair);
    }
    
    #endregion

    #region show attr tips

    public Vector3[] _ShowPoses;

    public void OnShowBaseAttr(int type)
    {
        RoleAttrEnum attr = (RoleAttrEnum)type;
        string showTips = "";
        int value = RoleData.SelectRole._BaseAttr.GetValue(attr);
        switch (attr)
        {
            case RoleAttrEnum.Strength:
                showTips = Tables.StrDictionary.GetFormatStr(1001000, value * GameDataValue._AttackPerStrength);
                break;
            case RoleAttrEnum.Dexterity:
                showTips = Tables.StrDictionary.GetFormatStr(1001001, value * GameDataValue._DefencePerDex);
                break;
            case RoleAttrEnum.Intelligence:
                showTips = Tables.StrDictionary.GetFormatStr(1001002, value * GameDataValue._EleAtkPerInt);
                break;
            case RoleAttrEnum.Vitality:
                showTips = Tables.StrDictionary.GetFormatStr(1001003, value * GameDataValue._HPPerVit);
                break;
        }

        UITextTip.ShowMessageTip(showTips, _ShowPoses[type - 1]);
    }

    #endregion

    #region role select

    public void ChangeRole()
    {
        UIRoleSelect2.ShowAsyn();
    }

    #endregion

    #region func open

    public Button _BtnChangeRole;

    public void RefreshFuncBtn()
    {
        //if (RoleData.SelectRole.TotalLevel >= GameDataValue.ROLE_SELECT)
        //{
        //    _BtnChangeRole.interactable = true;
        //}
        //else
        //{
        //    _BtnChangeRole.interactable = false;
        //}
    }

    #endregion
}

