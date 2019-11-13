using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerFrame : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIPlayerFrame, UILayer.BaseUI, hash);
    }

    #endregion

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        string iconName = "heroicon/Head_Hero_danshoujian";
        //if (RoleData.SelectRole.Profession == Tables.PROFESSION.BOY_DEFENCE
        //    || RoleData.SelectRole.Profession == Tables.PROFESSION.BOY_DOUGE)
        //{
        //    iconName = "heroicon/Head_Hero_shuangshoufu";
        //}
        ResourceManager.Instance.SetImage(_Icon, iconName);

        _Level.text = RoleData.SelectRole.TotalLevel.ToString();
    }

    void Update()
    {
        HpUpdate();
    }

    #region 

    public Image _Icon;
    public Text _Level;
    public Slider _HPProcess;
    public Text _HPText;

    private void HpUpdate()
    {
        if (!FightManager.Instance)
            return;

        if (!FightManager.Instance.MainChatMotion)
            return;

        _HPText.text = FightManager.Instance.MainChatMotion.RoleAttrManager.HP + "/" + FightManager.Instance.MainChatMotion.RoleAttrManager.GetBaseAttr(RoleAttrEnum.HPMax);
        _HPProcess.value = FightManager.Instance.MainChatMotion.RoleAttrManager.HPPersent;
    }

    #endregion
}
