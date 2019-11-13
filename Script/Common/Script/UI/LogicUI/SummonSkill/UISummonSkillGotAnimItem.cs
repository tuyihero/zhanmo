using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
using UnityEngine.EventSystems;
using System;
using Tables;

public class UISummonSkillGotAnimItem : UIItemSelect
{
    public Text _Name;
    public Image _Quality;
    public Image _Icon;
    public GameObject _Anchor;
    public Animation _Animation;

    public override void Show(Hashtable hash)
    {
        base.Show();
    }

    public void InitSummonMotion(SummonMotionData summonData)
    {
        _Name.text = CommonDefine.GetQualityColorStr(summonData.SummonRecord.Quality) + StrDictionary.GetFormatStr(summonData.SummonRecord.NameDict) + "</color>";
        ResourceManager.Instance.SetImage(_Icon, summonData.SummonRecord.MonsterBase.HeadIcon);
        ResourceManager.Instance.SetImage(_Quality, CommonDefine.GetQualityFramIcon(summonData.SummonRecord.Quality));

        _Anchor.SetActive(false);
    }

    public void PlayShow()
    {
        _Animation.Play("GotEffectShow");
    }

    public void ShowInfo()
    {
        _Anchor.SetActive(true);
    }
}

