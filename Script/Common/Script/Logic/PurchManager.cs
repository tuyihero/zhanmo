using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class PurchManager: IStoreListener
{

    #region 唯一

    private static PurchManager _Instance = null;
    public static PurchManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new PurchManager();
            }
            return _Instance;
        }
    }

    private PurchManager() { }

    public void InitIAPInfo()
    {
        _ProductBundleID = new Dictionary<string, string>();
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        Debug.Log("InitIAPInfo " + Tables.TableReader.Recharge.Records.Count);
        foreach (var chargeTab in Tables.TableReader.Recharge.Records.Values)
        {
            Debug.Log("InitIAPInfo:" + chargeTab.Id + ";" + chargeTab.BundleName);
            builder.AddProduct(chargeTab.BundleName, ProductType.Consumable, new IDs
            {
                {chargeTab.BundleName, GooglePlay.Name }
            });
            _ProductBundleID.Add(chargeTab.BundleName, chargeTab.Id);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    #endregion

    #region purch

    private IStoreController controller;
    private IExtensionProvider extensions;

    private Action _PurchCallback;
    private Dictionary<string, string> _ProductBundleID;
         

    public void Purch(string idx, Action callBack)
    {
        

        try
        {
            string bundleID = Tables.TableReader.Recharge.GetRecord(idx).BundleName;
            Debug.Log("Purch:" + bundleID);
            controller.InitiatePurchase(bundleID);

            _PurchCallback = callBack;
            //watch movie

            var chargeRecord = Tables.TableReader.Recharge.GetRecord(idx);
            Hashtable eventHash = new Hashtable();
            eventHash.Add("OrderID", idx);
            eventHash.Add("PurchID", idx);
            eventHash.Add("PurchPrice", chargeRecord.Price);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_IAP_REQ, this, eventHash);

            UILoadingTips.ShowAsyn();
        }
        catch (Exception e)
        {
            UILoadingTips.HideAsyn();
        }

#if UNITY_EDITOR
        UILoadingTips.HideAsyn();
        PurchFinish(idx);
#endif

        //GameCore.Instance.StartCoroutine(PurchFinish(idx));
        //PurchFinish(idx);
    }

    public void PurchFinish(string idx)
    {
        //yield return new WaitForSeconds(1.0f);

        UILoadingTips.HideAsyn();

        if (_PurchCallback != null)
            _PurchCallback.Invoke();

        var chargeRecord = Tables.TableReader.Recharge.GetRecord(idx);
        PlayerDataPack.Instance.AddDiamond(chargeRecord.Num);

        Hashtable eventHash = new Hashtable();
        eventHash.Add("OrderID", idx);
        eventHash.Add("PurchID", idx);
        eventHash.Add("PurchPrice", chargeRecord.Price);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_IAP_SUCESS, this, eventHash);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("IAP OnInitializeFailed error:" + error.ToString());
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        bool validPurchase = true; // Presume valid for platforms with no R.V.

        // Unity IAP's validation logic is only included on these platforms.
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
        // Prepare the validator with the secrets we prepared in the Editor
        // obfuscation window.
        var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
            AppleTangle.Data(), Application.identifier);

        try
        {
            // On Google Play, result has a single product ID.
            // On Apple stores, receipts contain multiple products.
            var result = validator.Validate(e.purchasedProduct.receipt);
            // For informational purposes, we list the receipt(s)
            Debug.Log("Receipt is valid. Contents:");
            foreach (IPurchaseReceipt productReceipt in result)
            {
                Debug.Log(productReceipt.productID);
                Debug.Log(productReceipt.purchaseDate);
                Debug.Log(productReceipt.transactionID);
                if (e.purchasedProduct.definition.id == productReceipt.productID)
                {
                    validPurchase = true;
                }
            }
            
        }
        catch (IAPSecurityException)
        {
            Debug.Log("Invalid receipt, not unlocking content");
            validPurchase = false;
            
        }
#endif


        if (validPurchase)
        {
            // Unlock the appropriate content here.

            PurchFinish(_ProductBundleID[e.purchasedProduct.definition.id]);
        }

        return PurchaseProcessingResult.Complete;

    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        UILoadingTips.HideAsyn();
        Debug.LogError("IAP OnPurchaseFailed error:" + i.transactionID + "," + p.ToString());
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;

        Debug.LogError("IAP OnInitialized");
    }

    #endregion


}
