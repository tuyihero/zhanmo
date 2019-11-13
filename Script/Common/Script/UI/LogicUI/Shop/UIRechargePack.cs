using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class UIRechargePack : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIRechargePack, UILayer.PopUI, hash);
    }

    #endregion

    #region 

    public UIContainerSelect _ItemContainer;

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _ItemContainer.InitContentItem(Tables.TableReader.Recharge.Records.Values, BuyItem);

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_IAP_SUCESS, IAPSucessHandle);
    }

    public override void Hide()
    {
        base.Hide();

        GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_IAP_SUCESS, IAPSucessHandle);
    }

    public void IAPSucessHandle(object e, Hashtable hash)
    {
        var purchID = (string)hash["PurchID"];
        var chargeRecord = Tables.TableReader.Recharge.GetRecord(purchID);

        UIMessageTip.ShowMessageTip(Tables.StrDictionary.GetFormatStr(2300075, chargeRecord.Num));
    }

    public void BuyItem(object rechargeGO)
    {
        Tables.RechargeRecord rechargeRecord = rechargeGO as Tables.RechargeRecord;
        PurchManager.Instance.Purch(rechargeRecord.Id, null);
    }

    public void BtnTest()
    {
        PurchManager.Instance.PurchFinish("6");
    }
    
    #endregion

    
    
}

