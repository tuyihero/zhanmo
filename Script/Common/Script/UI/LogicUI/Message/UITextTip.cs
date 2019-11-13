using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UITextTip : UIBase
{
    public static void ShowMessageTip(string str, Vector3 showPos)
    {
        {
            ShowAsyn(str, showPos);
        }
    }

    public static void ShowMessageTip(int idx, Vector3 showPos)
    {
        ShowMessageTip(StrDictionary.GetFormatStr(idx), showPos);
    }

    public static void ShowAsyn(string strTip, Vector3 showPos)
    {
        Hashtable hash = new Hashtable();
        hash.Add("Message", strTip);
        hash.Add("ShowPos", showPos);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UITextTip, UILayer.MessageUI, hash);
    }

    #region 

    public RectTransform _ShowTrans;
    public Text _ShowText;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        string message = hash["Message"] as string;
        Vector3 showPos = (Vector3)hash["ShowPos"];
        _ShowTrans.transform.position = showPos;
        ShowMessage(message);
    }

    public void ShowMessage(string message)
    {
        _ShowText.text = (message);
    }


    #endregion
}
