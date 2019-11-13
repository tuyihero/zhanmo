using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIFiveElementValueTip : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFiveElementValueTip, UILayer.PopUI, hash);
    }

    #endregion

    #region 

    public Text _CurValue;
    public UIContainerBase _AttrContainer;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        RefreshItems();
    }

    public void RefreshItems()
    {
        int totalValue = FiveElementData.Instance.ElementsTotalValue;
        _CurValue.text = Tables.StrDictionary.GetFormatStr(1300002, totalValue);

        List<string> valueAttrDescs = new List<string>();
        var valueAttrs = Tables.TableReader.FiveElementValueAttr.Records.Values;
        foreach (var valueAttr in valueAttrs)
        {
            string attr = Tables.StrDictionary.GetFormatStr(valueAttr.DescIdx);
            if (totalValue >= valueAttr.Value)
            {
                attr = CommonDefine.GetEnableGrayStr(1) + attr + "</color>";
            }
            else
            {
                attr = CommonDefine.GetEnableGrayStr(0) + attr + "</color>";
            }
            valueAttrDescs.Add(attr);
        }
        _AttrContainer.InitContentItem(valueAttrDescs);
    }
    
    #endregion

}

