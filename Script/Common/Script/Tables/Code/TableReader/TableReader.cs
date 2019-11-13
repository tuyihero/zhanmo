using System.Collections;

namespace Tables
{
    public class TableReader
    {

        #region 唯一实例

        private TableReader() { }

        private TableReader _Instance = null;
        public TableReader Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new TableReader();

                return _Instance;
            }
        }

        #endregion
        #region Logic

//
        public static Achievement Achievement { get; internal set; }
//
        public static AttrValue AttrValue { get; internal set; }
//
        public static AttrValueLevel AttrValueLevel { get; internal set; }
//
        public static BossStage BossStage { get; internal set; }
//
        public static CommonItem CommonItem { get; internal set; }
//
        public static EquipBaseAttr EquipBaseAttr { get; internal set; }
//
        public static EquipItem EquipItem { get; internal set; }
//
        public static EquipSpAttr EquipSpAttr { get; internal set; }
//
        public static FightAttr FightAttr { get; internal set; }
//
        public static FiveElement FiveElement { get; internal set; }
//
        public static FiveElementAttr FiveElementAttr { get; internal set; }
//
        public static FiveElementCore FiveElementCore { get; internal set; }
//
        public static FiveElementCoreAttr FiveElementCoreAttr { get; internal set; }
//
        public static FiveElementLevel FiveElementLevel { get; internal set; }
//
        public static FiveElementValueAttr FiveElementValueAttr { get; internal set; }
//
        public static GemBaseAttr GemBaseAttr { get; internal set; }
//
        public static GemSet GemSet { get; internal set; }
//
        public static GemTable GemTable { get; internal set; }
//
        public static GiftPacket GiftPacket { get; internal set; }
//
        public static GlobalBuff GlobalBuff { get; internal set; }
//
        public static LoadingTips LoadingTips { get; internal set; }
//
        public static Mission Mission { get; internal set; }
//
        public static MonsterAttr MonsterAttr { get; internal set; }
//
        public static MonsterBase MonsterBase { get; internal set; }
//
        public static Recharge Recharge { get; internal set; }
//
        public static RoleExp RoleExp { get; internal set; }
//
        public static ShopItem ShopItem { get; internal set; }
//
        public static SkillBase SkillBase { get; internal set; }
//
        public static SkillInfo SkillInfo { get; internal set; }
//
        public static StageInfo StageInfo { get; internal set; }
//
        public static StrDictionary StrDictionary { get; internal set; }
//
        public static StrTable StrTable { get; internal set; }
//
        public static SummonSkill SummonSkill { get; internal set; }
//
        public static SummonSkillAttr SummonSkillAttr { get; internal set; }
//
        public static SummonSkillLottery SummonSkillLottery { get; internal set; }

        public static void ReadTables()
        {
            //读取所有表
            Achievement = new Achievement(TableReadBase.GetTableText("Achievement"), false);
            AttrValue = new AttrValue(TableReadBase.GetTableText("AttrValue"), false);
            AttrValueLevel = new AttrValueLevel(TableReadBase.GetTableText("AttrValueLevel"), false);
            BossStage = new BossStage(TableReadBase.GetTableText("BossStage"), false);
            CommonItem = new CommonItem(TableReadBase.GetTableText("CommonItem"), false);
            EquipBaseAttr = new EquipBaseAttr(TableReadBase.GetTableText("EquipBaseAttr"), false);
            EquipItem = new EquipItem(TableReadBase.GetTableText("EquipItem"), false);
            EquipSpAttr = new EquipSpAttr(TableReadBase.GetTableText("EquipSpAttr"), false);
            FightAttr = new FightAttr(TableReadBase.GetTableText("FightAttr"), false);
            FiveElement = new FiveElement(TableReadBase.GetTableText("FiveElement"), false);
            FiveElementAttr = new FiveElementAttr(TableReadBase.GetTableText("FiveElementAttr"), false);
            FiveElementCore = new FiveElementCore(TableReadBase.GetTableText("FiveElementCore"), false);
            FiveElementCoreAttr = new FiveElementCoreAttr(TableReadBase.GetTableText("FiveElementCoreAttr"), false);
            FiveElementLevel = new FiveElementLevel(TableReadBase.GetTableText("FiveElementLevel"), false);
            FiveElementValueAttr = new FiveElementValueAttr(TableReadBase.GetTableText("FiveElementValueAttr"), false);
            GemBaseAttr = new GemBaseAttr(TableReadBase.GetTableText("GemBaseAttr"), false);
            GemSet = new GemSet(TableReadBase.GetTableText("GemSet"), false);
            GemTable = new GemTable(TableReadBase.GetTableText("GemTable"), false);
            GiftPacket = new GiftPacket(TableReadBase.GetTableText("GiftPacket"), false);
            GlobalBuff = new GlobalBuff(TableReadBase.GetTableText("GlobalBuff"), false);
            LoadingTips = new LoadingTips(TableReadBase.GetTableText("LoadingTips"), false);
            Mission = new Mission(TableReadBase.GetTableText("Mission"), false);
            MonsterAttr = new MonsterAttr(TableReadBase.GetTableText("MonsterAttr"), false);
            MonsterBase = new MonsterBase(TableReadBase.GetTableText("MonsterBase"), false);
            Recharge = new Recharge(TableReadBase.GetTableText("Recharge"), false);
            RoleExp = new RoleExp(TableReadBase.GetTableText("RoleExp"), false);
            ShopItem = new ShopItem(TableReadBase.GetTableText("ShopItem"), false);
            SkillBase = new SkillBase(TableReadBase.GetTableText("SkillBase"), false);
            SkillInfo = new SkillInfo(TableReadBase.GetTableText("SkillInfo"), false);
            StageInfo = new StageInfo(TableReadBase.GetTableText("StageInfo"), false);
            StrDictionary = new StrDictionary(TableReadBase.GetTableText("StrDictionary"), false);
            StrTable = new StrTable(TableReadBase.GetTableText("StrTable"), false);
            SummonSkill = new SummonSkill(TableReadBase.GetTableText("SummonSkill"), false);
            SummonSkillAttr = new SummonSkillAttr(TableReadBase.GetTableText("SummonSkillAttr"), false);
            SummonSkillLottery = new SummonSkillLottery(TableReadBase.GetTableText("SummonSkillLottery"), false);

            //初始化所有表
            Achievement.CoverTableContent();
            AttrValue.CoverTableContent();
            AttrValueLevel.CoverTableContent();
            BossStage.CoverTableContent();
            CommonItem.CoverTableContent();
            EquipBaseAttr.CoverTableContent();
            EquipItem.CoverTableContent();
            EquipSpAttr.CoverTableContent();
            FightAttr.CoverTableContent();
            FiveElement.CoverTableContent();
            FiveElementAttr.CoverTableContent();
            FiveElementCore.CoverTableContent();
            FiveElementCoreAttr.CoverTableContent();
            FiveElementLevel.CoverTableContent();
            FiveElementValueAttr.CoverTableContent();
            GemBaseAttr.CoverTableContent();
            GemSet.CoverTableContent();
            GemTable.CoverTableContent();
            GiftPacket.CoverTableContent();
            GlobalBuff.CoverTableContent();
            LoadingTips.CoverTableContent();
            Mission.CoverTableContent();
            MonsterAttr.CoverTableContent();
            MonsterBase.CoverTableContent();
            Recharge.CoverTableContent();
            RoleExp.CoverTableContent();
            ShopItem.CoverTableContent();
            SkillBase.CoverTableContent();
            SkillInfo.CoverTableContent();
            StageInfo.CoverTableContent();
            StrDictionary.CoverTableContent();
            StrTable.CoverTableContent();
            SummonSkill.CoverTableContent();
            SummonSkillAttr.CoverTableContent();
            SummonSkillLottery.CoverTableContent();
        }

        #endregion
    }
}
