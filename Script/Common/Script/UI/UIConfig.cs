using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum UILayer
{
    ControlUI,
    BaseUI,
    MainFunUI,
    PopUI,
    SubPopUI,
    Sub2PopUI,
    MessageUI,
    TopUI
}

public class AssetInfo
{
    public string AssetPath;

    public AssetInfo(string assetPath)
    {
        AssetPath = assetPath;
    }
}

public class UIConfig
{
    public static AssetInfo UILogin = new AssetInfo("SystemUI/UILogin");
    public static AssetInfo UILoadingScene = new AssetInfo("SystemUI/UILoadingScene");
    public static AssetInfo UIMessageBox = new AssetInfo("SystemUI/UIMessageBox");
    public static AssetInfo UIDiamondEnsureMsgBox = new AssetInfo("SystemUI/UIDiamondEnsureMsgBox");

    public static AssetInfo UISystemSetting = new AssetInfo("LogicUI/UISystemSetting");
    public static AssetInfo UIRoleSelect = new AssetInfo("LogicUI/UIRoleSelect");
    public static AssetInfo UIRoleSelect2 = new AssetInfo("LogicUI/RoleAttr/UIRoleSelect2");
    public static AssetInfo UIMainFun = new AssetInfo("LogicUI/UIMainFun");
    public static AssetInfo UILoadingTips = new AssetInfo("LogicUI/UILoadingTips");
    public static AssetInfo UIControlPanel = new AssetInfo("LogicUI/UIControlPanel");
    public static AssetInfo UIAchievement = new AssetInfo("LogicUI/Achievement/UIAchievement");
    public static AssetInfo UIDamagePanel = new AssetInfo("LogicUI/DamagePanel/UIDamagePanel");
    public static AssetInfo UIDropNamePanel = new AssetInfo("LogicUI/DropNamePanel/UIDropNamePanel");
    public static AssetInfo UIDragPanel = new AssetInfo("LogicUI/BagPack/UIDragPanel");
    public static AssetInfo UIEquipPack = new AssetInfo("LogicUI/BagPack/UIEquipPack");
    public static AssetInfo UIEquipTooltips = new AssetInfo("LogicUI/BagPack/UIEquipTooltips");
    public static AssetInfo UIGemTooltips = new AssetInfo("LogicUI/Gem/UIGemTooltips");
    public static AssetInfo UIItemTooltips = new AssetInfo("LogicUI/BagPack/UIItemTooltips");
    public static AssetInfo UILegendaryItemTooltips = new AssetInfo("LogicUI/BagPack/UILegendaryItemTooltips");
    public static AssetInfo UISellShopPack = new AssetInfo("LogicUI/BagPack/UISellShopPack");
    public static AssetInfo UIFightFinish = new AssetInfo("LogicUI/Fight/UIFightFinish");
    public static AssetInfo UIFightWarning = new AssetInfo("LogicUI/Fight/UIFightWarning");
    public static AssetInfo UIFightSetting = new AssetInfo("LogicUI/Fight/UIFightSetting");
    public static AssetInfo UIFightTips = new AssetInfo("LogicUI/Fight/UIFightTips");
    public static AssetInfo UIStageDiffTips = new AssetInfo("LogicUI/Fight/UIStageDiffTips");
    public static AssetInfo UIFuncInFight = new AssetInfo("LogicUI/Fight/UIFuncInFight");
    public static AssetInfo UIJoyStick = new AssetInfo("LogicUI/Fight/UIJoyStick");
    public static AssetInfo UIDirectControl = new AssetInfo("LogicUI/Fight/UIDirectControl");
    public static AssetInfo UISkillBar = new AssetInfo("LogicUI/Fight/UISkillBar");
    public static AssetInfo UIFiveElement = new AssetInfo("LogicUI/FiveElement/UIFiveElement");
    public static AssetInfo UIFiveElementCoreTooltip = new AssetInfo("LogicUI/FiveElement/UIFiveElementCoreTooltip");
    public static AssetInfo UIFiveElementExtra = new AssetInfo("LogicUI/FiveElement/UIFiveElementExtra");
    public static AssetInfo UIFiveElementTooltip = new AssetInfo("LogicUI/FiveElement/UIFiveElementTooltip");
    public static AssetInfo UIPlayerFrame = new AssetInfo("LogicUI/Frame/UIPlayerFrame");
    public static AssetInfo UITargetFrame = new AssetInfo("LogicUI/Frame/UITargetFrame");
    public static AssetInfo UICostGemPanel = new AssetInfo("LogicUI/Gem/UICostGemPanel");
    public static AssetInfo UIGemCombineSet = new AssetInfo("LogicUI/Gem/UIGemCombineSet");
    public static AssetInfo UIGemPack = new AssetInfo("LogicUI/Gem/UIGemPack");
    public static AssetInfo UIGemSuitPack = new AssetInfo("LogicUI/Gem/UIGemSuitPack");
    public static AssetInfo UIGiftPack = new AssetInfo("LogicUI/Gift/UIGiftTipPack");
    public static AssetInfo UIGiftGetTips = new AssetInfo("LogicUI/Gift/UIGiftGetTips");
    public static AssetInfo UIGlobalBuff = new AssetInfo("LogicUI/GlobalBuff/UIGlobalBuff");
    public static AssetInfo UIHPPanel = new AssetInfo("LogicUI/HPPanel/UIHPPanel");
    public static AssetInfo UIMessageTip = new AssetInfo("LogicUI/Message/UIMessageTip");
    public static AssetInfo UITextTip = new AssetInfo("LogicUI/Message/UITextTip");
    public static AssetInfo UIDailyMission = new AssetInfo("LogicUI/Mission/UIDailyMission");
    public static AssetInfo UIRoleAttr = new AssetInfo("LogicUI/RoleAttr/UIRoleAttr");
    public static AssetInfo UIShopNum = new AssetInfo("LogicUI/Shop/UIShopNumInput");
    public static AssetInfo UIShopPack = new AssetInfo("LogicUI/Shop/UIShopPack");
    public static AssetInfo UIRechargePack = new AssetInfo("LogicUI/Shop/UIRechargePack");
    public static AssetInfo UISkillLevelUp = new AssetInfo("LogicUI/SkillLvUp/UISkillLevelUp");
    public static AssetInfo UISoulPack = new AssetInfo("LogicUI/Soul/UISoulPack");
    public static AssetInfo UIActPanel = new AssetInfo("LogicUI/Stage/UIActPanel");
    public static AssetInfo UIBossStageSelect = new AssetInfo("LogicUI/Stage/UIBossStageSelect");
    public static AssetInfo UIStageSelect = new AssetInfo("LogicUI/Stage/UIStageSelect");
    public static AssetInfo UISummonCollections = new AssetInfo("LogicUI/SummonSkill/UISummonCollections");
    public static AssetInfo UISummonGotAnim = new AssetInfo("LogicUI/SummonSkill/UISummonGotAnim");
    public static AssetInfo UISummonLevelUpSelect = new AssetInfo("LogicUI/SummonSkill/UISummonLevelUpSelect");
    public static AssetInfo UISummonLotteryReturn = new AssetInfo("LogicUI/SummonSkill/UISummonLotteryReturn");
    public static AssetInfo UISummonSkillLottery = new AssetInfo("LogicUI/SummonSkill/UISummonSkillLottery");
    public static AssetInfo UISummonSkillPack = new AssetInfo("LogicUI/SummonSkill/UISummonSkillPack");
    public static AssetInfo UISummonSkillToolTips = new AssetInfo("LogicUI/SummonSkill/UISummonSkillToolTips");
    public static AssetInfo UISummonStageUp = new AssetInfo("LogicUI/SummonSkill/UISummonStageUp");
    public static AssetInfo UIFiveElementValueTip = new AssetInfo("LogicUI/FiveElement/UIFiveElementValueTip");

    public static AssetInfo UITestAttr = new AssetInfo("LogicUI/BagPack/UITestAttr");
    public static AssetInfo UITestEquip = new AssetInfo("LogicUI/BagPack/UITestEquip");
    public static AssetInfo UITestBuff = new AssetInfo("LogicUI/GlobalBuff/UITestBuff");


    public static AssetInfo AimTargetPanel = new AssetInfo("LogicUI/AimTarget/AimTargetPanel");
    public static AssetInfo DamagePanel = new AssetInfo("UI/LogicUI/DamagePanel/DamagePanel");


}
