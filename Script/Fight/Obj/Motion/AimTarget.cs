using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AimTarget : InstanceBase<AimTarget>
{

    void Start()
    {
        SetInstance(this);
        UIControlPanel.AddClickEvent(OnTargetPointClick);
        UISkillBar.SetAimTypeStatic(_AimType);
    }

    void OnDestory()
    {
        SetInstance(null);
    }

    void Update()
    {
        //FreeAimUpdate();
    }

    public enum AimTargetType
    {
        None,
        Free,
        Lock,
    }

    public AimTargetType _AimType = AimTargetType.Free;

    public void SwitchAimType(int aimType)
    {
        _AimType = (AimTargetType)aimType;
        if (_AimType == AimTargetType.None)
        {
            AimTargetPanel.HideAimTarget();
        }
    }

    
    #region lock target

    private MotionManager _LockTarget;
    public MotionManager LockTarget
    {
        get
        {
            return _LockTarget;
        }
    }

    private EffectController _AnimEffectInstance;

    private void OnTargetPointClick(PointerEventData eventData)
    {
        if (_AimType == AimTargetType.None)
            return;

        Debug.Log("OnTargetPointClick");
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, 1 << FightLayerCommon.CAMP_2))
        {
            var targetMotion = hit.collider.GetComponentInParent<MotionManager>();
            if (targetMotion != null && !targetMotion.IsMotionDie)
            {
                _AimType = AimTargetType.Lock;
                _LockTarget = targetMotion;
            }
        }

        if (_LockTarget == null)
            return;

        UISkillBar.SetAimTypeStatic(_AimType);
        AimTargetPanel.ShowAimTarget(_LockTarget, AimType.Lock);

    }

    //private void ShowAimEffect()
    //{
    //    if (_AnimEffectInstance == null)
    //    {
    //        var effectPrefab = ResourceManager.Instance.GetEffect("BuffAlert2");
    //        var effectSingle = effectPrefab.GetComponent<EffectSingle>();
    //        var _AnimEffectInstance = FightManager.Instance.MainChatMotion.PlayDynamicEffect(effectSingle);
    //    }

    //    _AnimEffectInstance.transform.SetParent(_LockTarget.GetBindTransform(_AnimEffectInstance._BindPos));
    //}

    //private void HideAnimEffect()
    //{

    //}

    #endregion

    #region free aim

    private void FreeAimUpdate()
    {
        if (_AimType != AimTargetType.Free)
            return;

        _LockTarget = null;
        ReSelectTargets();

        if (_LockTarget == null)
        {
            AimTargetPanel.HideAimTarget();
            return;
        }

        AimTargetPanel.ShowAimTarget(_LockTarget, AimType.Free);
    }

    #endregion

    #region switch target 

    private List<SelectTargetCommon.SelectedInfo> _SelectedTarget = new List<SelectTargetCommon.SelectedInfo>();
    private float _LastSelectTime = 0;
    private int _LastSelectIdx = 0;
    private static float _ChangeSelectInterval = 3.0f;

    public void SwitchAimTarget()
    {
        if (Time.time - _LastSelectTime > _ChangeSelectInterval)
        {
            _LastSelectTime = Time.time;
            ReSelectTargets();
        }
        else
        {
            ++_LastSelectIdx;
            if (_LastSelectIdx == _SelectedTarget.Count || _SelectedTarget[_LastSelectIdx]._SelectedMotion == null)
            {
                ReSelectTargets();
            }
            else
            {
                _LockTarget = _SelectedTarget[_LastSelectIdx]._SelectedMotion;
            }
        }
    }

    private void ReSelectTargets()
    {
        if (FightManager.Instance.MainChatMotion == null)
            return;

        _SelectedTarget = SelectTargetCommon.GetFrontMotions(FightManager.Instance.MainChatMotion, 8, 180, SelectTargetCommon.SelectSortType.Distance, SelectTargetType.Enemy);
        if (_SelectedTarget.Count == 0)
            return;

        _LockTarget = _SelectedTarget[0]._SelectedMotion;
    }

    #endregion
}
