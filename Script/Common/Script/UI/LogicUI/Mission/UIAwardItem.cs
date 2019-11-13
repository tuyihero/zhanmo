using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIAwardItem : UIItemBase
{

    public Image _AwardIcon;
    public Image _AwardQuality;
    public Text _AwardCnt;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var commonItem = (CommonItemRecord)hash["InitObj"];

        ResourceManager.Instance.SetImage(_AwardIcon, commonItem.Icon);
        ResourceManager.Instance.SetImage(_AwardQuality, CommonDefine.GetQualityIcon(commonItem.Quality));
    }

}
