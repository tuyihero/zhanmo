using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 

public class InputManager : InstanceBase<InputManager>
{

	// Use this for initialization
	void Start ()
    {
        SetInstance(this);
    }

    void OnDestory()
    {
        SetInstance(null);
    }
	
	// Update is called once per frame
	void FixedUpdate()
    {
        if (InputMotion == null)
            return;

        if (FightManager.Instance == null)
            return;

        if (FightManager.Instance._InitStep != FightManager.InitStep.InitFinish)
            return;

#if UNITY_EDITOR
        Axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
#endif

#if UNITY_EDITOR
        if (!_EmulateMode)
#endif
        {
            InputMotion.InputDirect(CameraAxis);
        }

        if (IsKeyHold("l"))
        {
            InputMotion.JumpState();
        }

        //if (_InputMotion.ActingSkill == null)
        {
            foreach (var skill in InputMotion._StateSkill._SkillMotions)
            {
                if (IsKeyHold(skill.Key))
                {
                    //_InputMotion.ActSkill(skill.Value);
                    InputMotion.InputSkill(skill.Key);
                }
            }
        }
        CharSkill();
        UpdateSummonSkill();

        if (GameCore.Instance._IsTestMode)
        {
            if (IsKeyHold("t"))
            {
                TestFight.TestDrop();
            }
        }
    }

    private MotionManager _InputMotion;
    public MotionManager InputMotion
    {
        get
        {
            return _InputMotion;
        }
        set
        {
            _InputMotion = value;
            _NormalAttack = _InputMotion.GetComponentInChildren<ObjMotionSkillAttack>();
        }
    }

    #region input 
    private Vector2 _LastAxis;
    private Vector2 _Axis;
    public Vector2 Axis
    {
        get
        {
            return _Axis;
        }

        set
        {
            _LastAxis = _Axis;
            _Axis = value;
        }
    }

    public Vector2 CameraAxis
    {
        get
        {
            var direct = transform.forward * Axis.y + transform.right * Axis.x;
            return new Vector2(direct.x, direct.z);
        }
    }

    private UISkillBar _UISkillBar;
    public UISkillBar UISkillBar
    {
        get
        {
            if (_UISkillBar == null)
            {
                _UISkillBar = GameObject.FindObjectOfType<UISkillBar>();
            }
            return _UISkillBar;
        }
    }

    public bool IsKeyDown(string key)
    {

        if (key.Length != 1)
            return false;
#if UNITY_EDITOR
        if (_EmulateMode)
        {
            return IsEmulateKeyDown(key);
        }
        return Input.GetKeyDown(key);
#else
        return UISkillBar.IsKeyDown(key);
#endif
    }

    public bool IsKeyHold(string key)
    {
        string realKey = key;
        if (key.Contains("k"))
        {
            realKey = "k";
        }
        if (key.Contains("u"))
        {
            realKey = "u";
        }
        
#if UNITY_EDITOR
        if (_EmulateMode)
        {
            return IsEmulateKeyDown(key);
        }
        return Input.GetKey(realKey);
#else
        if (UISkillBar != null)
        {
            return UISkillBar.IsKeyDown(key);
        }
        else
        {
            return false;
        }
#endif
    }

    public bool IsAnyHold()
    {
#if UNITY_EDITOR
        return Input.anyKey;
#else
        return UISkillBar.IsKeyDown();
#endif
    }

    #endregion

    #region emulate key

    public bool _EmulateMode = false;
    private string _EmulatePress = "";

    public void SetEmulatePress(string key)
    {
        _EmulateMode = true;
        _EmulatePress = key;
    }

    public void ReleasePress()
    {
        _EmulatePress = "";
    }

    public void FinishEmulateMode()
    {
        _EmulateMode = false;
    }

    public bool IsEmulateKeyDown(string key)
    {
        return _EmulatePress == key;
    }

    #endregion

    #region normal skill

    private ObjMotionSkillAttack _NormalAttack;
    public void CharSkill()
    {
        if (_NormalAttack == null)
            return;

        if (IsKeyHold("j"))
        {
            if (InputMotion._ActionState == InputMotion._StateJump
                || InputMotion._ActionState == InputMotion._StateJumpIdle)
            {
                InputMotion.ActSkill(InputMotion._StateSkill._SkillMotions["7"]);
            }
            else
            {
                InputMotion.ActSkill(InputMotion._StateSkill._SkillMotions["j"]);
            }
        }

        if (IsKeyHold("k"))
        {
            if (InputMotion.ActingSkill == _NormalAttack && _NormalAttack.CurStep > 0 && _NormalAttack.CurStep < 4 && _NormalAttack.CanNextInput)
            {
                string inputKey = (_NormalAttack.CurStep).ToString();
                if (InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                {
                    SetRotate();
                    InputMotion.ActSkill(InputMotion._StateSkill._SkillMotions[inputKey]);
                }
            }
            else
            {
                if (FightSkillManager.Instance.CanReuseSkill() && FightSkillManager.Instance.ReuseSkillBase.IsCanActSkill())
                {
                    SetRotate();
                    InputMotion.ActSkill(FightSkillManager.Instance.ReuseSkillBase);
                    FightSkillManager.Instance.ResetReuseSkill();
                    return;
                }

                //if (!string.IsNullOrEmpty(_BuffSkillInput) && _InputMotion._StateSkill._SkillMotions[_BuffSkillInput].IsCanActSkill())
                //{
                //    SetRotate();
                //    _InputMotion.ActSkill(_InputMotion._StateSkill._SkillMotions[_BuffSkillInput]);
                //    _BuffSkillInput = null;
                //}

                string inputKey = "k";
                if (InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey) && InputMotion._StateSkill._SkillMotions[inputKey].IsCanActSkill())
                {
                    SetRotate();
                    InputMotion.ActSkill(InputMotion._StateSkill._SkillMotions[inputKey]);
                }

            }
        }

        if (IsKeyHold("u"))
        {
            if (InputMotion.ActingSkill== _NormalAttack && _NormalAttack.CurStep > 0 && _NormalAttack.CurStep < 4 /*&& _NormalAttack.CanNextInput*/)
            {
                string inputKey = "6";
                Hashtable hash = new Hashtable();
                hash.Add("AttackStep", _NormalAttack.CurStep);

                if (InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                {
                    InputMotion.ActSkill(InputMotion._StateSkill._SkillMotions[inputKey], hash);
                }
            }
            else if(InputMotion.ActingSkill != _NormalAttack)
            {
                string inputKey = "5";
                if (InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey) && InputMotion._StateSkill._SkillMotions[inputKey].IsCanActSkill())
                {
                    InputMotion.ActSkill(InputMotion._StateSkill._SkillMotions[inputKey]);
                }
            }
        }

        //if (IsKeyHold("l"))
        //{
        //    if (InputMotion._ActionState == InputMotion._StateHit
        //        || InputMotion._ActionState == InputMotion._StateFly
        //        || InputMotion._ActionState == InputMotion._StateLie
        //        || InputMotion._ActionState == InputMotion._StateRise)
        //    {
        //        string inputKey = "8";
        //        if (InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
        //        {
        //            InputMotion.ActSkill(InputMotion._StateSkill._SkillMotions[inputKey]);
        //        }
        //    }
        //    else
        //    {
        //        string inputKey = "4";
        //        if (InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
        //        {
        //            InputMotion.ActSkill(InputMotion._StateSkill._SkillMotions[inputKey]);
        //        }
        //        else
        //        {
        //            inputKey = "7";
        //            if (InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
        //            {
        //                InputMotion.ActSkill(InputMotion._StateSkill._SkillMotions[inputKey]);
        //            }
        //        }
        //    }
        //}
    }

    public void SetRotate()
    {
        if (AimTarget.Instance == null)
            return;

        if (GlobalValPack.Instance.IsRotToAnimTarget && AimTarget.Instance.LockTarget != null)
        {
            InputMotion.SetLookAt(AimTarget.Instance.LockTarget.transform.position);
        }
        else
        {
            if (CameraAxis != Vector2.zero)
            {
                //InputMotion.SetLookRotate(new Vector3(0, 0, 0));
                if (CameraAxis.x > 0)
                {
                    InputMotion.SetRotate(Vector3.zero);
                }
                else
                {
                    InputMotion.SetRotate(new Vector3(0,180,0));
                }
            }
        }
    }

    public ObjMotionSkillBase GetCharSkill(string input)
    {
        if (_NormalAttack == null)
            return null;

        if (input == ("j"))
        {
            return InputMotion._StateSkill._SkillMotions["j"];
        }

        if (input == ("k"))
        {
            if (InputMotion.ActingSkill == _NormalAttack && _NormalAttack.CurStep > 0 && _NormalAttack.CurStep < 4 && _NormalAttack.CanNextInput)
            {
                string inputKey = (_NormalAttack.CurStep).ToString();
                if (InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                {
                    return InputMotion._StateSkill._SkillMotions[inputKey];
                }
            }
            else if (InputMotion.ActingSkill == null)
            {
                if (FightSkillManager.Instance.ReuseSkillBase != null)
                {
                    return FightSkillManager.Instance.ReuseSkillBase;
                }

                {
                    string inputKey = "k0";
                    if (InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                    {
                        return  (InputMotion._StateSkill._SkillMotions[inputKey]);
                    }
                }
            }
        }

        if (input == ("u"))
        {
            if (InputMotion.ActingSkill == _NormalAttack && _NormalAttack.CurStep > 0 && _NormalAttack.CurStep < 4 && _NormalAttack.CanNextInput)
            {
                string inputKey = "6";
                Hashtable hash = new Hashtable();
                hash.Add("AttackStep", _NormalAttack.CurStep);

                if (InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                {
                    return  (InputMotion._StateSkill._SkillMotions[inputKey]);
                }
            }
            else if (InputMotion.ActingSkill == null)
            {
                string inputKey = "5";
                if (InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                {
                    return  (InputMotion._StateSkill._SkillMotions[inputKey]);
                }
            }
        }

        if (input == ("l"))
        {
            if (InputMotion._ActionState == InputMotion._StateHit
                || InputMotion._ActionState == InputMotion._StateFly
                || InputMotion._ActionState == InputMotion._StateLie
                || InputMotion._ActionState == InputMotion._StateRise)
            {
                string inputKey = "8";
                if (InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                {
                    return (InputMotion._StateSkill._SkillMotions[inputKey]);
                }
            }
            else
            {
                string inputKey = "4";
                if (InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                {
                    return (InputMotion._StateSkill._SkillMotions[inputKey]);
                }
                else
                {
                    inputKey = "7";
                    if (InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                    {
                        return (InputMotion._StateSkill._SkillMotions[inputKey]);
                    }
                }
            }
        }

        return null;
    }

    #endregion

    #region use skill again

    //private string _ReuseSkillInput;
    //private string _ReuseSkillConfig = "0";
    //public string ReuseSkillConfig
    //{
    //    get
    //    {
    //        return _ReuseSkillConfig;
    //    }
    //}
    //private int _ReuseTimes = 0;
    //private float _ReuseStartTime = 0;
    //private float _ReuseLast = 2.0f;

    //public void InitReuseSkill()
    //{
    //    foreach (var skillInfo in SkillData.Instance.ProfessionSkills)
    //    {
    //        if (skillInfo.SkillRecord.SkillAttr.AttrImpact == "RoleAttrImpactAnotherUse" && skillInfo.SkillActureLevel > 0)
    //        {
    //            _ReuseSkillConfig = skillInfo.SkillRecord.SkillInput;
    //            break;
    //        }
    //    }
    //}

    //public void SkillNextInput(ObjMotionSkillBase motionSkill)
    //{
    //    //if (_ReuseTimes > 0)
    //    //{
    //    //    if (motionSkill._ActInput == "j"
    //    //        || motionSkill._ActInput == "1"
    //    //        || motionSkill._ActInput == "2"
    //    //        || motionSkill._ActInput == "3")
    //    //    {
    //    //        _ReuseTimes = 0;
    //    //        UISkillBar.SetSkillUseTips("k", 0);
    //    //        return;
    //    //    }
    //    //}
    //    if (_InputMotion == FightManager.Instance.MainChatMotion)
    //    {
    //        if (_ReuseSkillConfig == "-1")
    //        {
    //            if (motionSkill._ActInput == "1"
    //                || motionSkill._ActInput == "2"
    //                || motionSkill._ActInput == "3")
    //            {
    //                ++_ReuseTimes;
    //                if (_ReuseTimes > 0)
    //                {
    //                    _ReuseStartTime = Time.time;
    //                    //UISkillBar.SetSkillUseTips("k", _ReuseLast);
    //                    FightSkillManager.Instance.SetReuse(motionSkill, _ReuseLast);
    //                    _ReuseSkillInput = motionSkill._ActInput;
    //                }
    //            }
    //        }
    //        else if (motionSkill._ActInput == _ReuseSkillConfig)
    //        {
    //            _ReuseSkillInput = _ReuseSkillConfig;
    //            ++_ReuseTimes;
    //            if (_ReuseTimes > 0)
    //            {
    //                _ReuseStartTime = Time.time;
    //                //UISkillBar.SetSkillUseTips("k", _ReuseLast);
    //                FightSkillManager.Instance.SetReuse(motionSkill, _ReuseLast);
    //            }
    //        }
    //    }
    //}

    //public void ResetReuseSkill()
    //{
    //    _ReuseTimes = 0;
    //    //UISkillBar.SetSkillUseTips("k", 0);
    //    FightSkillManager.Instance.SetReuse(null, 0);
    //    return;
    //}

    //private bool CanReuseSkill()
    //{
    //    if (_ReuseTimes > 0 && Time.time - _ReuseStartTime < _ReuseLast)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    #endregion

    #region set buff skill

    private string _BuffSkillInput;
    private float _BuffResuseLast = 2.0f;

    public void SetReuseSkill(string skillInput)
    {
        _BuffSkillInput = skillInput;
        //FightSkillManager.Instance.SetReuse(_InputMotion._StateSkill._SkillMotions[skillInput], _BuffResuseLast);
    }

    #endregion

    #region summon skill

    public void UpdateSummonSkill()
    {
        if (IsKeyHold("h"))
        {
            SummonSkill.Instance.UseSummonSkill();
            UISkillBar.RefreshSummonIcon();
        }
    }

    #endregion
}
