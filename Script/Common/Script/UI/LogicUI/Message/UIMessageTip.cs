using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIMessageTip : UIBase
{
    public static void ShowMessageTip(string str)
    {
        //var instance = GameCore.Instance.UIManager.GetUIInstance<UIMessageTip>("LogicUI/Message/UIMessageTip");
        //if (instance != null)
        //{
        //    instance.ShowMessage(str);
        //}
        //else
        {
            ShowAsyn(str);
        }
    }

    public static void ShowMessageTip(int idx)
    {
        ShowMessageTip(StrDictionary.GetFormatStr(idx));
    }

    public static void ShowAsyn(string strTip)
    {
        Hashtable hash = new Hashtable();
        hash.Add("Message", strTip);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIMessageTip, UILayer.MessageUI, hash);
    }

    #region 

    public UIMessageTipItem _ItemPrefab;
    public int _KeepItemCnt;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        string message = hash["Message"] as string;
        ShowMessage(message);
    }

    public void ShowMessage(string message)
    {
        var idleItem = ResourcePool.Instance.GetIdleUIItem<UIMessageTipItem>(_ItemPrefab.gameObject, transform);
        idleItem.SetMessage(message);
    }


    #endregion
}
