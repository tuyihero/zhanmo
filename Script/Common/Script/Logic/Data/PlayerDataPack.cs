using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;



public class PlayerDataPack : DataPackBase
{
    #region 单例

    private static PlayerDataPack _Instance;
    public static PlayerDataPack Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new PlayerDataPack();
            }
            return _Instance;
        }
    }

    private PlayerDataPack()
    {
        _SaveFileName = "PlayerDataPack";
    }

    #endregion

    #region money
    [SaveField(1)]
    private int _Gold = 0;
    public int Gold
    {
        get
        {
            return _Gold;
        }
    }

    [SaveField(2)]
    private int _Diamond = 0;
    public int Diamond
    {
        get
        {
            return _Diamond;
        }
    }

    public void AddGold(int value)
    {
        _Gold += value;
        SaveClass(false);
        UIMainFun.UpdateMoney();
    }

    public bool DecGold(int value)
    {
        if (_Gold < value)
        {
            UIMessageTip.ShowMessageTip(20000);
            return false;
        }

        _Gold -= value;
        SaveClass(false);
        UIMainFun.UpdateMoney();
        return true;
    }

    public void AddDiamond(int value)
    {
        _Diamond += value;
        SaveClass(false);
        UIMainFun.UpdateMoney();
    }

    public bool DecDiamond(int value)
    {
        if (_Diamond < value)
        {
            UIMessageTip.ShowMessageTip(20001);
            return false;
        }

        _Diamond -= value;
        SaveClass(false);
        UIMainFun.UpdateMoney();
        return true;
    }
    #endregion


    #region char

    [SaveField(6)]
    public int _LastSelectRole;

    public List<RoleData> _RoleList;

    public RoleData _SelectedRole;

    public const int _MAX_ROLE_CNT = 4;

    public void InitPlayerData()
    {
        if (_RoleList == null || _RoleList.Count != _MAX_ROLE_CNT)
        {
            if (_RoleList == null)
            {
                _RoleList = new List<RoleData>();
            }
            int startCnt = _RoleList.Count;
            for (int i = startCnt; i < _MAX_ROLE_CNT; ++i)
            {
                var newRole = new RoleData();
                //newRole._SaveFileName = "Role" + i;
                _RoleList.Add(newRole);
            }
            SaveClass(true);
        }

        for (int i = 0; i < _RoleList.Count; ++i)
        {
            if (i == (int)PROFESSION.WARRIOR)
            {
                _RoleList[i].MainBaseName = "CharAssasinBase";
                _RoleList[i].MotionFold = "CharAssasin";
                _RoleList[i].ModelName = "CharAssasin";
                _RoleList[i].Profession = PROFESSION.WARRIOR;
                _RoleList[i].DefaultWeaponModel = "WeaponAssasin1";
                _RoleList[i].IconName = "hero/hero_p_shuangshoufu";
            }
            else if (i == (int)PROFESSION.ASSASSIN)
            {
                _RoleList[i].MainBaseName = "CharAssasinBase";
                _RoleList[i].MotionFold = "CharAssasin";
                _RoleList[i].ModelName = "CharAssasin";
                _RoleList[i].Profession = PROFESSION.WARRIOR;
                _RoleList[i].DefaultWeaponModel = "WeaponAssasin1";
                _RoleList[i].IconName = "hero/hero_p_shuangshoufu";
            }
            else if (i == (int)PROFESSION.MAGE)
            {
                _RoleList[i].MainBaseName = "CharAssasinBase";
                _RoleList[i].MotionFold = "CharAssasin";
                _RoleList[i].ModelName = "CharAssasin";
                _RoleList[i].Profession = PROFESSION.WARRIOR;
                _RoleList[i].DefaultWeaponModel = "WeaponAssasin1";
                _RoleList[i].IconName = "hero/hero_p_shuangshoufu";
            }
            
        }
    }

    public void SelectRole(int roleIdx)
    {
        _LastSelectRole = roleIdx;
        Debug.Log("_RoleList.Count:" + _RoleList.Count);
        if (roleIdx >= 0 && roleIdx < _RoleList.Count)
        {
            _SelectedRole = _RoleList[roleIdx];
        }

        _SelectedRole.InitRoleData();

        SkillData.Instance.LoadClass(true);
        SkillData.Instance.InitSkills();

        _SelectedRole.CalculateAttr();

        SaveClass(false);
    }

    #endregion

    #region role data

    [SaveField(3)]
    public List<ItemEquip> _EquipList;

    [SaveField(4)]
    private int _RoleLevel;
    public int RoleLevel
    {
        get
        {
            return _RoleLevel;
        }
        set
        {
            _RoleLevel = value;
            SaveClass(false);
        }
    }

    [SaveField(5)]
    private int _CurExp;
    public int CurExp
    {
        get
        {
            return _CurExp;
        }
        set
        {
            _CurExp = value;
            SaveClass(false);
        }
    }

    #endregion
}

