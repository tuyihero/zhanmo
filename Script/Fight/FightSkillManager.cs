using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FightSkillInfo
{
    public string _SkillInput;
    public string _SkillIcon;
    public float _CDTime;
    public float _LastActCD;
    public float _StoreCnt;
    public float _StoreCntLimit;
    public float _ReuseTime;
    public float _LastReuse;

    public bool _ShowInUI = true;
}

public class FightSkillManager
{

    #region static 

    public static FightSkillManager Instance
    {
        get
        {
            if (FightManager.Instance._FightSkillManager == null)
            {
                FightManager.Instance._FightSkillManager = new FightSkillManager();
            }
            return FightManager.Instance._FightSkillManager;
        }
    }

    private FightSkillManager()
    {
        InitReuseSkill();
    }

    #endregion

    #region skill cd


    private List<FightSkillInfo> _FightSkillDict = new List<FightSkillInfo>();
    public List<FightSkillInfo> FightSkillDict
    {
        get
        {
            return _FightSkillDict;
        }
    }

    private ObjMotionSkillBase _ReuseSkillBase;
    public ObjMotionSkillBase ReuseSkillBase
    {
        get
        {
            return _ReuseSkillBase;
        }
    }

    private FightSkillInfo _NewSkillInfo;
    public FightSkillInfo NewSkillInfo
    {
        get
        {
            return _NewSkillInfo;
        }
    }

    public void SetSkillCD(ObjMotionSkillBase skillBase)
    {
        if (skillBase._SkillCD == 0)
            return;
        var skillInfo = _FightSkillDict.Find((skillinfo) =>
        {
            if (skillinfo._SkillInput == skillBase._ActInput)
            {
                return true;
            }
            return false;
        });
        if (skillInfo == null)
        {
            skillInfo = new FightSkillInfo();
            _FightSkillDict.Add(skillInfo);
            skillInfo._SkillInput = skillBase._ActInput;
            skillInfo._SkillIcon = SkillData.Instance.GetSkillIcon(skillInfo._SkillInput);
            skillInfo._StoreCntLimit = skillBase._StoreUseTimes;
            skillInfo._StoreCnt = skillInfo._StoreCntLimit - 1;
            skillInfo._CDTime = skillBase._SkillCD;
            skillInfo._LastActCD = TimeManager.Instance.FightTime;
        }
        else
        {
            if (skillInfo._StoreCnt > 0)
            {
                --skillInfo._StoreCnt;
            }
            else
            {
                skillInfo._CDTime = skillBase._SkillCD;
                skillInfo._LastActCD = TimeManager.Instance.FightTime;
            }
        }
        UIFuncInFight.UpdateSkillInfoUI();
    }

    public bool IsSkillInCD(ObjMotionSkillBase skillBase)
    {
        if (skillBase._SkillCD == 0)
            return false;

        var skillInfo = _FightSkillDict.Find((skillinfo) =>
        {
            if (skillinfo._SkillInput == skillBase._ActInput)
            {
                return true;
            }
            return false;
        });

        if (skillInfo == null)
        {
            return false;
        }

        if (skillInfo._StoreCnt > 0)
            return false;

        if (TimeManager.Instance.FightTime - skillInfo._LastActCD > skillInfo._CDTime)
            return false;

        return true;
    }

    public void SetReuse(ObjMotionSkillBase skillBase, float reuseTime)
    {
        

        if (skillBase == null)
        {
            for (int i = 0; i < _FightSkillDict.Count; ++i)
            {
                if (_FightSkillDict[i]._LastReuse > 0)
                {
                    _FightSkillDict[i]._LastReuse = 1;
                    _FightSkillDict[i]._ReuseTime = 0;
                }
            }
            UpdateSkillInfo();
        }
        else if (skillBase == _ReuseSkillBase)
        {
            var skillInfo = _FightSkillDict.Find((skillinfo) =>
            {
                if (skillinfo._SkillInput == skillBase._ActInput)
                {
                    return true;
                }
                return false;
            });
            if (TimeManager.Instance.FightTime - skillInfo._LastReuse > skillInfo._ReuseTime)
            {
                skillInfo._ReuseTime = reuseTime;
                skillInfo._LastReuse = TimeManager.Instance.FightTime;
            }
            else
            {
                skillInfo._ReuseTime = 0;
                skillInfo._LastReuse = 1;
            }

            UpdateSkillInfo();
        }
        else
        {
            if (skillBase._ActInput.Equals("e"))
                return;

            var skillInfo = _FightSkillDict.Find((skillinfo) =>
            {
                if (skillinfo._SkillInput == skillBase._ActInput)
                {
                    return true;
                }
                return false;
            });
            if (skillInfo == null)
            {
                skillInfo = new FightSkillInfo();
                _FightSkillDict.Add(skillInfo);
                skillInfo._SkillInput = skillBase._ActInput;
                skillInfo._SkillIcon = SkillData.Instance.GetSkillIcon(skillInfo._SkillInput);

                skillInfo._StoreCntLimit = skillBase._StoreUseTimes;
            }

            skillInfo._ReuseTime = reuseTime;
            skillInfo._LastReuse = TimeManager.Instance.FightTime;

            _ReuseSkillBase = skillBase;
            UIFuncInFight.UpdateSkillInfoUI();
        }
    }

    public bool IsSkillCanReuse(ObjMotionSkillBase skillBase)
    {
        var skillInfo = _FightSkillDict.Find((skillinfo) =>
        {
            if (skillinfo._SkillInput == skillBase._ActInput)
            {
                return true;
            }
            return false;
        });

        if (skillInfo == null)
        {
            return false;
        }

        if (TimeManager.Instance.FightTime - skillInfo._LastReuse > skillInfo._ReuseTime)
            return false;

        return true;
    }

    public void UpdateSkillInfo()
    {
        List<FightSkillInfo> removeList = new List<FightSkillInfo>();
        foreach (var skillInfoPair in _FightSkillDict)
        {
            if (skillInfoPair._LastActCD > 0 && TimeManager.Instance.FightTime - skillInfoPair._LastActCD > skillInfoPair._CDTime)
            {
                if (skillInfoPair._StoreCnt < skillInfoPair._StoreCntLimit)
                {
                    ++skillInfoPair._StoreCnt;
                    if (skillInfoPair._StoreCnt == skillInfoPair._StoreCntLimit)
                    {
                        removeList.Add(skillInfoPair);
                    }
                    else
                    {
                        skillInfoPair._LastActCD = TimeManager.Instance.FightTime;
                    }
                }
                else
                {
                    removeList.Add(skillInfoPair);
                }
            }

            if (skillInfoPair._LastReuse > 0 && TimeManager.Instance.FightTime - skillInfoPair._LastReuse > skillInfoPair._ReuseTime)
                removeList.Add(skillInfoPair);
        }

        foreach (var removeKeys in removeList)
        {
            _FightSkillDict.Remove(removeKeys);

            if (_ReuseSkillBase != null && _ReuseSkillBase._ActInput == removeKeys._SkillInput)
            {
                _ReuseSkillBase = null;
            }
        }

        UIFuncInFight.UpdateSkillInfoUI();
    }

    #endregion

    #region skill reuse


    private string _ReuseSkillInput;
    private string _ReuseSkillConfig = "0";
    public string ReuseSkillConfig
    {
        get
        {
            return _ReuseSkillConfig;
        }
    }
    private ObjMotionSkillBase _LastUseSkill;

    private static float _ReuseLast = 2.0f;

    public void InitReuseSkill()
    {
        foreach (var skillInfo in SkillData.Instance.ProfessionSkills)
        {
            if (skillInfo.SkillRecord.SkillAttr.AttrImpact == "RoleAttrImpactAnotherUse" && skillInfo.SkillActureLevel > 0)
            {
                _ReuseSkillConfig = skillInfo.SkillRecord.SkillInput;
                break;
            }
        }
    }

    public void SkillNextInput(ObjMotionSkillBase motionSkill)
    {
        if (_ReuseSkillConfig == "-1")
        {
            if (motionSkill._ActInput == "1"
                || motionSkill._ActInput == "2"
                || motionSkill._ActInput == "3")
            {
                if (_LastUseSkill != motionSkill)
                {
                    SetReuse(motionSkill, _ReuseLast);
                }
            }
        }
        else if (motionSkill._ActInput == _ReuseSkillConfig)
        {
            if (_LastUseSkill != motionSkill)
            {
                _ReuseSkillInput = _ReuseSkillConfig;
                SetReuse(motionSkill, _ReuseLast);
            }
            _LastUseSkill = motionSkill;
        }

        if (!motionSkill._ActInput.Equals("e"))
        {
            _LastUseSkill = motionSkill;
        }
    }

    public void ResetLastUseSkill()
    {
        _LastUseSkill = null;
    }

    public void ResetReuseSkill()
    {
        Instance.SetReuse(null, 0);
    }

    public bool CanReuseSkill()
    {
        if (_ReuseSkillBase == null)
            return false;

        return IsSkillCanReuse(_ReuseSkillBase);
    }

    #endregion

    #region summon skill

    public void SetSummonCD(SummonMotionData summonData, float cdTime, bool isCommonCD)
    {
        var skillInfo = _FightSkillDict.Find((skillinfo) =>
        {
            if (skillinfo._SkillInput == summonData.SummonRecordID)
            {
                return true;
            }
            return false;
        });
        if (skillInfo == null)
        {
            skillInfo = new FightSkillInfo();
            _FightSkillDict.Add(skillInfo);
            skillInfo._SkillInput = summonData.SummonRecordID;
            skillInfo._SkillIcon = summonData.SummonRecord.MonsterBase.HeadIcon;
            skillInfo._CDTime = cdTime;
            skillInfo._LastActCD = TimeManager.Instance.FightTime;
            skillInfo._ShowInUI = !isCommonCD;
        }
        else
        {
            var nowCD = skillInfo._CDTime - (TimeManager.Instance.FightTime - skillInfo._LastActCD);
            if (cdTime > nowCD)
            {
                skillInfo._CDTime = cdTime;
                skillInfo._LastActCD = TimeManager.Instance.FightTime;
                skillInfo._ShowInUI = !isCommonCD;
            }
        }
        UIFuncInFight.UpdateSkillInfoUI();
    }

    public bool IsSummonSkillInCD(SummonMotionData summonData)
    {

        var skillInfo = _FightSkillDict.Find((skillinfo) =>
        {
            if (skillinfo._SkillInput == summonData.SummonRecordID)
            {
                return true;
            }
            return false;
        });

        if (skillInfo == null)
        {
            return false;
        }

        if (skillInfo._StoreCnt > 0)
            return false;

        if (TimeManager.Instance.FightTime - skillInfo._LastActCD > skillInfo._CDTime)
            return false;

        return true;
    }

    public float GetSkillCDPro(string input)
    {

        var skillInfo = _FightSkillDict.Find((skillinfo) =>
        {
            if (skillinfo._SkillInput == input)
            {
                return true;
            }
            return false;
        });

        if (skillInfo == null)
        {
            return 0;
        }

        if (skillInfo._StoreCnt > 0)
            return 0;

        float cdPro = (skillInfo._CDTime - (TimeManager.Instance.FightTime - skillInfo._LastActCD)) / skillInfo._CDTime;

        return cdPro;
    }

    #endregion
}
