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

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        InitSkillClass();
        ShowSkillInfo();
    }

    #endregion

    public UISubScollMenu _SkillClass;
    public UIContainerSelect _SkillInfos;

    private Dictionary<string, List< ItemSkill>> _SkillClasses = new Dictionary<string, List<ItemSkill>>();
    private ItemSkill _SelectedSkill;

    private void InitSkillClass()
    {
        _SkillClasses.Clear();
        _SkillClass.Clear();
        foreach (var skillItem in SkillData.Instance.ProfessionSkills)
        {
            string dicStr = Tables.StrDictionary.GetFormatStr(skillItem.SkillRecord.SkillType);

            if (!_SkillClasses.ContainsKey(dicStr))
            {
                _SkillClasses.Add(dicStr, new List<ItemSkill>());
                _SkillClass.PushMenu(dicStr);
            }

            _SkillClasses[dicStr].Add(skillItem);
        }
        _SkillClass.ShowDefaultFirst();
    }

    public void SelectSkillClass(object selectGO)
    {
        var skillClass = (string)selectGO;
        InitSkillItems(skillClass);
    }

    private void InitSkillItems(string skillClass)
    {
        List<ItemSkill> selectedSkill = new List<ItemSkill>();
        selectedSkill.Add(_SkillClasses[skillClass][0]);
        _SkillInfos.InitSelectContent(_SkillClasses[skillClass], selectedSkill, SelectSkillItem);
    }

    private void SelectSkillItem(object selectItem)
    {
        var skillInfo = selectItem as ItemSkill;
        if (skillInfo == null)
            return;

        _SelectedSkill = skillInfo;
        ShowSkillInfo();
    }

    #region skill lv up

    public Text _Desc;
    public Text _NeedRoleLv;
    public Text _NeedSkillLv;
    public UICurrencyItem _MoneyCost;
    public Text _MoneyText;
    public Button _LevelUp;

    private void ShowSkillInfo()
    {
        if (_SelectedSkill == null)
        {
            _Desc.text = "";
            _NeedRoleLv.text = "";
            _NeedSkillLv.text = "";
            _MoneyCost.ShowCurrency(MONEYTYPE.GOLD, 0);
            //_LevelUp.interactable = false;
            return;
        }

        var skillTab = Tables.TableReader.SkillInfo.GetRecord(_SelectedSkill.SkillID);
        var impactType = Type.GetType(_SelectedSkill.SkillRecord.SkillAttr.AttrImpact);
        int descLevel = _SelectedSkill.SkillLevel;
        descLevel = Mathf.Clamp(_SelectedSkill.SkillLevel, 1, skillTab.MaxLevel);
        if (impactType != null)
        {

            var method = impactType.GetMethod("GetAttrDesc");
            if (method != null)
            {
                string skillDesc = method.Invoke(null, new object[] { new List<int>() { int.Parse(_SelectedSkill.SkillID), descLevel } }) as string;
                _Desc.text = skillDesc;
            }
        }
        else
        {
            var skillDesc = StrDictionary.GetFormatStr(skillTab.DescStrDict);
            _Desc.text = skillDesc;
        }

        string roleLvStr = StrDictionary.GetFormatStr(62000);
        int nextLv = skillTab.StartRoleLevel + (_SelectedSkill.SkillActureLevel) * skillTab.NextLvInterval;
        if (RoleData.SelectRole.RoleLevel >= nextLv)
        {
            roleLvStr += StrDictionary.GetFormatStr(1000005, nextLv);
        }
        else
        {
            roleLvStr += StrDictionary.GetFormatStr(1000004, nextLv);
        }
        _NeedRoleLv.text = roleLvStr;

        if (skillTab.StartPreSkill > 0)
        {
            string skillLv = StrDictionary.GetFormatStr(62001);
            var preSkillTab = TableReader.SkillInfo.GetRecord(skillTab.StartPreSkill.ToString());
            string nextSkill = StrDictionary.GetFormatStr(62005, StrDictionary.GetFormatStr(preSkillTab.NameStrDict), skillTab.StartPreSkillLv);
            var skillItem = SkillData.Instance.GetSkillInfo(preSkillTab.Id);
            if (skillItem.SkillActureLevel >= skillTab.StartPreSkillLv)
            {
                skillLv += StrDictionary.GetFormatStr(1000005, nextSkill);
            }
            else
            {
                skillLv += StrDictionary.GetFormatStr(1000004, nextSkill);
            }
            _NeedSkillLv.text = skillLv;

        }
        else
        {
            _NeedSkillLv.text = "";
        }

        if (skillTab.CostStep[0] == (int)MONEYTYPE.GOLD)
        {
            int costValue = GameDataValue.GetSkillLvUpGold(skillTab, _SelectedSkill.SkillLevel);
            _MoneyCost.ShowCurrency(MONEYTYPE.GOLD, costValue);
            if (costValue > PlayerDataPack.Instance.Gold)
            {
                _MoneyText.color = Color.red;
            }
            else
            {
                _MoneyText.color = Color.green;
            }
        }
        else
        {
            int skillItemCnt = BackBagPack.Instance.PageItems.GetItemCnt(GameDataValue._SkillItemID);
            if (skillItemCnt > 0)
            {
                _MoneyCost.ShowCurrency(GameDataValue._SkillItemID, 1);
                _MoneyText.color = Color.green;
            }
            else
            {
                int costValue = skillTab.CostStep[1];
                _MoneyCost.ShowCurrency(MONEYTYPE.DIAMOND, costValue);
                if (costValue > PlayerDataPack.Instance.Diamond)
                {
                    _MoneyText.color = Color.red;
                }
                else
                {
                    _MoneyText.color = Color.green;
                }
            }
        }

        if (skillTab.MaxLevel == _SelectedSkill.SkillLevel)
        {
            _LevelUp.interactable = false;
            _NeedRoleLv.text = StrDictionary.GetFormatStr(62006);
            _MoneyCost.gameObject.SetActive(false);
        }
        else
        {
            _LevelUp.interactable = true;
            _MoneyCost.gameObject.SetActive(true);
        }
    }

    public void OnBtnSkillLvUp()
    {
        if (_SelectedSkill == null)
            return;

        SkillData.Instance.SkillLevelUp(_SelectedSkill.SkillID);
        RefreshSkillInfos();
        ShowSkillInfo();
    }

    private void RefreshSkillInfos()
    {
        _SkillInfos.RefreshItems();
    }
    #endregion

}

