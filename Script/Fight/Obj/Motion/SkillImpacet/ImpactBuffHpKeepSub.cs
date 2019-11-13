using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffHpKeepSub : ImpactBuffSub
{

    public override void UpdateBuff()
    {
        base.UpdateBuff();

        if (_BuffOwner.RoleAttrManager.HPPersent >= _HPRateLow && _BuffOwner.RoleAttrManager.HPPersent <= _HPRateHeight)
        {
            ActBuffs();
        }
        else
        {
            RemoveBuffs();
        }
    }

    #region hp rate

    public float _HPRateLow = 0f;
    public float _HPRateHeight = 0.5f;

    #endregion

    #region buff

    private List<ImpactBuff> _ActingBuffInstance = new List<ImpactBuff>();
    private List<ImpactBuff> _ActBuffs;

    private void ActBuffs()
    {
        if (_ActingBuffInstance.Count != 0)
            return;

        if (_ActBuffs == null)
        {
            _ActBuffs = new List<ImpactBuff>(_SubImpactGO.GetComponentsInChildren<ImpactBuff>());
        }

        foreach (var buff in _ActBuffs)
        {
            var buffInstance = buff.ActBuffInstance(_BuffOwner, _BuffOwner);
            _ActingBuffInstance.Add(buffInstance);
        }
    }

    private void RemoveBuffs()
    {
        if (_ActingBuffInstance.Count == 0)
            return;

        foreach (var buffInstance in _ActingBuffInstance)
        {
            _BuffOwner.RemoveBuff(buffInstance);
        }

        _ActingBuffInstance.Clear();
    }

    #endregion

}
