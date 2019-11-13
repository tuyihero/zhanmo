using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class UISummonGotAnim : UIBase
{

    #region static funs

    public static void ShowAsyn(List<SummonMotionData> summonDatas, Action finishCallBack)
    {
        Hashtable hash = new Hashtable();
        hash.Add("SummonDatas", summonDatas);
        hash.Add("FinishCallBack", finishCallBack);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UISummonGotAnim, UILayer.Sub2PopUI, hash);
    }

    public static void RefreshPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISummonSkillPack>(UIConfig.UISummonGotAnim);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshItems();
    }

    #endregion

    #region pack

    public List<UISummonSkillGotAnimItem> _AnimItems;

    private List<SummonMotionData> _SummonDatas;
    private Action _FinishCallBack;
    private int _PlayingIdx = 0;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _SummonDatas = (List<SummonMotionData>)hash["SummonDatas"];
        _FinishCallBack = (Action)hash["FinishCallBack"];
        ShowItemPack();
    }

    public override void Hide()
    {
        base.Hide();

        if (_FinishCallBack != null)
        {
            _FinishCallBack.Invoke();
            _FinishCallBack = null;
        }

    }

    private void ShowItemPack()
    {
        for (int i = 0; i < _AnimItems.Count; ++i)
        {
            if (_SummonDatas.Count > i)
            {
                _AnimItems[i].gameObject.SetActive(true);
                _AnimItems[i].InitSummonMotion(_SummonDatas[i]);
            }
            else
            {
                _AnimItems[i].gameObject.SetActive(false);
            }
        }
        _PlayingIdx = 0;
        StartCoroutine(ShowAnim());
    }

    private IEnumerator ShowAnim()
    {
        while (_AnimItems.Count > _PlayingIdx && _AnimItems[_PlayingIdx].gameObject.activeSelf)
        {
            _AnimItems[_PlayingIdx].PlayShow();
            ++_PlayingIdx;
            yield return new WaitForSeconds(0.5f);
        }

    }

    #endregion


}

