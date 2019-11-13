using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UITestBuff : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UITestBuff, UILayer.PopUI, hash);
    }

    public static void ActBuffInFight()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UITestBuff>(UIConfig.UITestBuff);
        if (instance == null)
            return;

        instance.ActBuffInFightInner();
    }

    #endregion

    #region 

    public List<string> _AttrIDs;
    public Toggle _TestBuffToggle;

    private List<EquipExAttr> _ExAttrs = new List<EquipExAttr>();

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        foreach (var attrID in _AttrIDs)
        {
            var attrRecord = Tables.TableReader.AttrValue.GetRecord(attrID);
            _ExAttrs.Add(attrRecord.GetExAttr(1));
        }
    }

    public void ActBuffInFightInner()
    {
        if (_TestBuffToggle.isOn)
        {
            foreach (var exAttr in _ExAttrs)
            {
                GlobalBuffData.Instance._ExAttrs.Add(exAttr);
            }
        }
    }

    #endregion
}

