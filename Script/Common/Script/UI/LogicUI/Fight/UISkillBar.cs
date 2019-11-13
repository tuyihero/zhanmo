using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

 
public class UISkillBar : UIBase
{
    #region static

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UISkillBar, UILayer.BaseUI, hash);
    }

    public static void SetSkillUseTips(string input, float time)
    {
        if (GameCore.Instance == null)
            return;

        var instance = GameCore.Instance.UIManager.GetUIInstance<UISkillBar>(UIConfig.UISkillBar);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.SetBtnUseTip(input, time);
    }

    public static void SetAimTypeStatic(AimTarget.AimTargetType aimType)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISkillBar>(UIConfig.UISkillBar);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.SetAimType(aimType);
    }

    public static void HideAsyn()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISkillBar>(UIConfig.UISkillBar);
        if (instance == null)
            return;

        //if (!instance.isActiveAndEnabled)
        //    return;

        instance.Hide();
    }

    public void Update()
    {
        UpdateCD();
        UpdateSummonCD();
    }

    #endregion

    public UISkillBarItem _ButtonJ;
    public UISkillBarItem _ButtonK;
    public UISkillBarItem _ButtonL;
    public UISkillBarItem _ButtonU;

    private Dictionary<string, UISkillBarItem> _Buttons = new Dictionary<string, UISkillBarItem>();

    public override void Init()
    {
        base.Init();

        InitEmulate();
    }

    public override void Show()
    {
        base.Show();

        _Buttons.Add("j", _ButtonJ);
        _Buttons.Add("k", _ButtonK);
        _Buttons.Add("l", _ButtonL);
        _Buttons.Add("u", _ButtonU);

        //if (RoleData.SelectRole.Profession == Tables.PROFESSION.BOY_DEFENCE
        //    || RoleData.SelectRole.Profession == Tables.PROFESSION.GIRL_DEFENCE)
        //{
        //    _ButtonL.SetSkillIcon("skill/skillbar_defence");
        //}
        //else
        {
            _ButtonL.SetSkillIcon("skill/skillbar_dodge");
        }

        InitSummonBtns();
    }

    public bool IsKeyDown(string key)
    {
        if (_Buttons.ContainsKey(key))
        {
            return _Buttons[key]._BtnPress.IsPress;
        }
        return false;
    }

    public bool IsKeyDown()
    {
        foreach (var btn in _Buttons)
        {
            if (btn.Value._BtnPress.IsPress)
                return true;
        }
        return false;
    }

    public void SetBtnUseTip(string key, float useTipTime)
    {
        if (_Buttons.ContainsKey(key))
        {
            _Buttons[key].SetUseTips(useTipTime);
        }
    }

    #region cd
    List<string> skillInput = new List<string>() { "j", "k", "l" };

    public void UpdateCD()
    {
        if (InputManager.Instance == null)
            return;

        for (int i = 0; i < skillInput.Count; ++i)
        {
            var skillBase = InputManager.Instance.GetCharSkill(skillInput[i]);
            if (skillBase == null)
            {
                _Buttons[skillInput[i]].SetCDPro(0);
                continue;
            }

            if (skillBase._SkillCD == 0)
            {
                _Buttons[skillInput[i]].SetCDPro(0);
                continue;
            }

            var cd = Time.time - skillBase.LastUseTime;
            float cdPro = (skillBase._SkillCD - cd) / skillBase._SkillCD;
            _Buttons[skillInput[i]].SetCDPro(cdPro);


            _Buttons[skillInput[i]].SetStoreTimes(skillBase.LastUseTimes);
        }
    }

    #endregion

    #region emulate

    public TestFight _TestFight;
    public GameObject _BtnEmulate;
    public GameObject _BtnEmulateTips;

    public void InitEmulate()
    {
        if (GameCore.Instance._IsTestMode)
        {
            _BtnEmulate.SetActive(true);
        }
        else
        {
            _BtnEmulate.SetActive(false);
        }
        _BtnEmulateTips.SetActive(false);
    }

    public void OnEmulate()
    {
        if (_TestFight == null)
        {
            _TestFight = FightManager.Instance.MainChatMotion.gameObject.GetComponent<TestFight>();
            if (_TestFight == null)
                FightManager.Instance.MainChatMotion.gameObject.AddComponent<TestFight>();
            return;
        }

        _TestFight.enabled = !_TestFight.enabled;
        _BtnEmulateTips.SetActive(_TestFight.enabled);
    }

    #endregion

    #region summon

    public UISkillBarItem[] _SummonBtns;

    private int _CurSummonIdx = -1;

    public void OnBtnSummon()
    {
        SummonSkill.Instance.UseSummonSkill();
        RefreshSummonIcon();
    }

    public void InitSummonBtns()
    {
        for (int i = 0; i < SummonSkillData.USING_SUMMON_NUM; ++i)
        {
            if (SummonSkillData.Instance._UsingSummon[i] != null)
            {
                _SummonBtns[i].gameObject.SetActive(true);
            }
            else
            {
                _SummonBtns[i].gameObject.SetActive(false);
            }
        }

        RefreshSummonIcon();
    }

    public void RefreshSummonIcon()
    {
        if (_CurSummonIdx != SummonSkill.Instance.CurSummonIdx)
        {
            _CurSummonIdx = SummonSkill.Instance.CurSummonIdx;
            for (int i = 0; i < SummonSkillData.USING_SUMMON_NUM; ++i)
            {
                if (SummonSkillData.Instance._UsingSummon[_CurSummonIdx] != null)
                {
                    _SummonBtns[i].SetSkillIcon(SummonSkillData.Instance._UsingSummon[_CurSummonIdx].SummonRecord.MonsterBase.HeadIcon);
                    _SummonBtns[i]._SkillInput = SummonSkillData.Instance._UsingSummon[_CurSummonIdx].SummonRecord.Id;
                }

                ++_CurSummonIdx;
                if (_CurSummonIdx == SummonSkill.Instance.GetSummonMotionCnt())
                {
                    _CurSummonIdx = 0;
                }
            }
        }
    }

    public void UpdateSummonCD()
    {
        foreach (var skillBtn in _SummonBtns)
        {
            if (skillBtn.gameObject.activeSelf)
            {
                float skillCDPro = FightSkillManager.Instance.GetSkillCDPro(skillBtn._SkillInput);
                skillBtn.SetCDPro(skillCDPro);
            }
        }
        
    }

    #endregion

    #region switch aim

    public Text _AimText;
    public GameObject _AimOnGO;

    private int _AimType = 0;
    public void OnSwitchAimType()
    {
        //++_AimType;
        //if (_AimType >= 3)
        //    _AimType = 0;

        //AimTarget.Instance.SwitchAimType(_AimType);
        //UpdateAim();
        //AimTarget.Instance.SwitchAimTarget();
        GlobalValPack.Instance.IsRotToAnimTarget = !GlobalValPack.Instance.IsRotToAnimTarget;
    }

    public void SetAimType(AimTarget.AimTargetType aimType)
    {
        _AimType = (int)aimType;
        UpdateAim();
    }

    private void UpdateAim()
    {
        //switch ((AimTarget.AimTargetType)_AimType)
        //{
        //    case AimTarget.AimTargetType.None:
        //        _AimText.text = "N";
        //        break;
        //    case AimTarget.AimTargetType.Free:
        //        _AimText.text = "F";
        //        break;
        //    case AimTarget.AimTargetType.Lock:
        //        _AimText.text = "L";
        //        break;
        //}
        if (GlobalValPack.Instance.IsRotToAnimTarget)
        {
            _AimOnGO.SetActive(GlobalValPack.Instance.IsRotToAnimTarget);
        }
    }

    #endregion
}

