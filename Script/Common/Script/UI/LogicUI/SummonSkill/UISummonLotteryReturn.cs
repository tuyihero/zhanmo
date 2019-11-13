using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Tables;

public class UISummonLotteryReturn : UIBase
{

    #region static funs

    public static void ShowAsyn(string itemID, int itemNum)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ItemID", itemID);
        hash.Add("ItemNum", itemNum);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UISummonLotteryReturn, UILayer.MessageUI, hash);
    }

    #endregion

    #region 

    public Text _Tips;
    public Image _Icon;
    public Text _ItemNum;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        string itemID = (string)hash["ItemID"];
        int itemNum = (int)hash["ItemNum"];

        var commonItem = TableReader.CommonItem.GetRecord(itemID);
        string itemName = CommonDefine.GetQualityItemName(itemID, true) + "*" + itemNum.ToString();
        string tipInfo = "";
        if (itemNum == 1)
        {
            tipInfo = StrDictionary.GetFormatStr(1260002, itemName);
        }
        else
        {
            tipInfo = StrDictionary.GetFormatStr(1260001, itemName);
        }
        _Tips.text = tipInfo;

        ResourceManager.Instance.SetImage(_Icon, commonItem.Icon);
        _ItemNum.text = "*" + itemNum.ToString();
    }

    #endregion

}

