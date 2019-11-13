using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIFightTipsItem : UIItemBase
{

    #region 

    public Text _Text;
    public int _TipsID;

    private void OnEnable()
    {
        _Text.text = Tables.StrDictionary.GetFormatStr(_TipsID); 
    }

    #endregion
}

