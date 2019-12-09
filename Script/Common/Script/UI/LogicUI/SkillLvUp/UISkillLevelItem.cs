
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;
using Tables;



public class UISkillLevelItem : MonoBehaviour
{
    public Text _SkillNameText;
    public Text _SkillLevelText;
    public Image _Lock;
    public Image _Icon;

    public string _SkillID;

    private ItemSkill _SkillItem;
    private SkillInfoRecord _SkillTab;
    public SkillInfoRecord SkillTab
    {
        get
        {
            if (_SkillTab == null)
            {
                _SkillTab = Tables.TableReader.SkillInfo.GetRecord(_SkillItem.SkillID);
            }
            return _SkillTab;
        }
    }

    public void Start()
    {
        _SkillItem = SkillData.Instance.GetSkillInfo(_SkillID);
        //string skillName = Tables.StrDictionary.GetFormatStr(SkillTab.NameStrDict);
        //_SkillNameText.text = skillName;
        //ResourceManager.Instance.SetImage(_Icon, SkillTab.Icon);

        Refresh();
    }

    public void Refresh()
    {
        //_SkillLevelText.text = "Lv." + _SkillItem.SkillActureLevel + "/" + _SkillItem.SkillRecord.MaxLevel;
        if (SkillData.Instance.IsSkillConflict(SkillTab))
        {
            _Lock.gameObject.SetActive(true);
        }
        else
        {
            _Lock.gameObject.SetActive(false);
        }
    }

    #region 

    public void OnItemClick()
    {
        Debug.Log("OnItemClick:" + _SkillID);
        SkillData.Instance.SkillLevelUp(_SkillID);
        UISkillLevelUp.RefreshSkillItems(SkillTab.Profession);
    }

    #endregion
}

