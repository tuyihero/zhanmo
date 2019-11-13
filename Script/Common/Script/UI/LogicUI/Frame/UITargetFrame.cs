using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITargetFrame : UIBase
{

    #region static funs

    public static void ShowAsyn(MotionManager motion)
    {
        Hashtable hash = new Hashtable();
        hash.Add("Motion", motion);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UITargetFrame, UILayer.BaseUI, hash);
    }

    #endregion

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        _TargetMotion = (MotionManager)hash["Motion"];
        if (_TargetMotion == null)
            return;
        ResourceManager.Instance.SetImage(_Icon, _TargetMotion.RoleAttrManager.MonsterRecord.HeadIcon);
    }

    void Update()
    {
        HpUpdate();
        ProtectPanelUpdate();
    }

    #region HP

    public GameObject _FrameRoot;
    public Slider _HPProcess;
    public Text _HPText;
    public Image _Icon;

    private MotionManager _TargetMotion;

    private void HpUpdate()
    {
        if (!AimTarget.Instance)
            return;

        //if (AimTarget.Instance.LockTarget != null
        //    && _TargetMotion != AimTarget.Instance.LockTarget)
        //{
        //    _TargetMotion = AimTarget.Instance.LockTarget;
        //    _TargetAI = null;
        //}

        if (_TargetMotion != null)
        {
            _FrameRoot.SetActive(true);
            _HPText.text = _TargetMotion.RoleAttrManager.HP + "/" + _TargetMotion.RoleAttrManager.GetBaseAttr(RoleAttrEnum.HPMax);
            _HPProcess.value = _TargetMotion.RoleAttrManager.HPPersent;
        }
        else
        {
            _FrameRoot.SetActive(false);
        }
    }

    #endregion

    #region Hit Protect

    [System.Serializable]
    public class HitProtectInfos
    {
        public string _SkillInput;
        public List<GameObject> _HitProtectGOs;
    }

    [SerializeField]
    public List<HitProtectInfos> _HitPretects;

    public GameObject _HitProtectPanel;

    private AI_Base _TargetAI;

    public void ProtectPanelUpdate()
    {
        if (_TargetMotion == null)
            return;

        if (_TargetAI == null)
        {
            _TargetAI = _TargetMotion.GetComponent<AI_Base>();
            _TargetAI.ProtectTimesDirty = true;
        }

        if (_TargetAI == null)
            return;

        if (_TargetAI._ProtectTimes.Count == 0)
        {
            _HitProtectPanel.SetActive(false);
            return;
        }
        else
        {
            _HitProtectPanel.SetActive(true);
        }

        if (!_TargetAI.ProtectTimesDirty)
            return;

        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < _HitPretects[i]._HitProtectGOs.Count; ++j)
            {
                if (_TargetAI._ProtectTimes.ContainsKey(_HitPretects[i]._SkillInput)
                    && j == _TargetAI._ProtectTimes[_HitPretects[i]._SkillInput] - 1)
                {
                    _HitPretects[i]._HitProtectGOs[j].SetActive(true);
                }
                else
                {
                    _HitPretects[i]._HitProtectGOs[j].SetActive(false);
                }
            }
        }
    }



    #endregion
}
