using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public enum BtnType
{
    None,
    OKBTN,
    YESNOBTN,
}

public class UIMessageBox : UIPopBase
{
    #region static

    public static void Show(int dictIdx, Action okAction, Action cancelAction, BtnType btnType = BtnType.YESNOBTN, bool clickBackHide = false)
    {
        Show(Tables.StrDictionary.GetFormatStr(dictIdx), okAction, cancelAction, btnType, clickBackHide);
    }

    public static void Show(string message, Action okAction, Action cancelAction, BtnType btnType = BtnType.YESNOBTN, bool clickBackHide = false)
    {
        Hashtable hash = new Hashtable();
        hash.Add("Message", message);
        hash.Add("OkAction", (Action)okAction);
        hash.Add("CancelAction", (Action)cancelAction);
        hash.Add("BtnType", btnType);
        hash.Add("ClickBackHide", clickBackHide);
        //GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_UI_SHOW_MESSAGEBOX, null, hash);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIMessageBox, UILayer.MessageUI, hash);
    }

    public static void ShowWithDontShotTodayTips(string message, string showType, Action okAction = null, Action cancelAction = null, BtnType btnType = BtnType.OKBTN, bool clickBackHide = true)
    {
        if (_DontShowTypes.Contains(showType))
        {
            return;
        }
        else
        {
            //_DontShowTypes.Add(showType);
        }

        Hashtable hash = new Hashtable();
        hash.Add("Message", message);
        hash.Add("OkAction", (Action)okAction);
        hash.Add("CancelAction", (Action)cancelAction);
        hash.Add("BtnType", btnType);
        hash.Add("ClickBackHide", clickBackHide);
        hash.Add("WithDontShowToday", showType);
        //GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_UI_SHOW_MESSAGEBOX, null, hash);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIMessageBox, UILayer.MessageUI, hash);
    }

    public static void Show(string message, Action okAction, Action cancelAction, string okText, string cancelText, BtnType btnType = BtnType.YESNOBTN, bool clickBackHide = false)
    {
        Hashtable hash = new Hashtable();
        hash.Add("Message", message);
        hash.Add("OkAction", (Action)okAction);
        hash.Add("CancelAction", (Action)cancelAction);
        hash.Add("OkText", okText);
        hash.Add("CancelText", cancelText);
        hash.Add("BtnType", btnType);
        hash.Add("ClickBackHide", clickBackHide);
        //GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_UI_SHOW_MESSAGEBOX, null, hash);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIMessageBox, UILayer.MessageUI, hash);
    }

    #endregion

    #region 

    public Text _MsgText;
    public Text _OkText;
    public Text _CancelText;

    #endregion

    #region base override

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _OkAction = null;
        if (hash.ContainsKey("OkAction"))
        {
            _OkAction = (Action)hash["OkAction"];
        }

        _CancelAction = null;
        if (hash.ContainsKey("CancelAction"))
        {
            _CancelAction = (Action)hash["CancelAction"];
        }

        if (hash.ContainsKey("Message"))
        {
            _MsgText.text = (string)hash["Message"];
        }
        if (hash.ContainsKey("BtnType"))
        {
            HideBtn((BtnType)hash["BtnType"]);
        }

        if (hash.ContainsKey("OkText"))
        {
            _OkText.text = (string)hash["OkText"];
        }
        if (hash.ContainsKey("CancelText"))
        {
            _CancelText.text = (string)hash["CancelText"];
        }

        if (hash.ContainsKey("ClickBackHide"))
        {
            _ClickBackHide = (bool)hash["ClickBackHide"];
        }

        _ShowType = "";
        if (hash.ContainsKey("WithDontShowToday"))
        {
            _ShowType = (string)hash["WithDontShowToday"];
            _DontShowToday.gameObject.SetActive(true);
            _DontShowToday.isOn = false;
            _DontShowTips.text = Tables.StrDictionary.GetFormatStr(1014);
        }
        else
        {
            _DontShowToday.gameObject.SetActive(false);
        }

        transform.SetAsLastSibling();
    }

    public override void Hide()
    {
        base.Hide();

        if (_DontShowToday.gameObject.activeSelf && _DontShowToday.isOn && !string.IsNullOrEmpty(_ShowType))
        {
            _DontShowTypes.Add(_ShowType);
        }
    }

    public void HideBtn(BtnType btnType)
    {
        if (m_btnOK != null)
        {
            m_btnOK.SetActive(btnType == BtnType.OKBTN);
        }
        if (m_btnYesNo != null)
        {
            m_btnYesNo.SetActive(btnType == BtnType.YESNOBTN);
        }

        
    }

    #endregion

    #region interaction

    private Action _OkAction;
    private Action _CancelAction;
    private bool _ClickBackHide;

    public GameObject m_btnYesNo;
    public GameObject m_btnOK;
    public void BtnOkEvent()
    {
        if (_OkAction != null)
            _OkAction();

        Hide();
    }

    public void BtnCancelEvent()
    {
        if (_CancelAction != null)
            _CancelAction();

        Hide();
    }

    public void BtnBackClick()
    {
        if (_ClickBackHide)
        {
            Hide();
        }
    }


    #endregion

    #region dont show today

    public Toggle _DontShowToday;
    public Text _DontShowTips;

    private string _ShowType;
    private static List<string> _DontShowTypes = new List<string>();



    #endregion
}

