using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class FightAttrRecord : TableRecordBase
    {

    }

    public partial class FightAttr : TableFileBase
    {
        public enum FightAttrType
        {
            //base
            HP = 1,
            HP_PERSENT,
            ATTACK,
            ATTACK_PERSENT,
            DEFENCE,
            DEFENCE_PERSENT,
            MOVE_SPEED,
            ATTACK_SPEED,
            LEVEL_REQUIRE,

            //damage
            REDUSE_DAMAGE = 100,
            REDUSE_DAMAGE_PERSENT,
            ENHANCE_DAMAGE,
            ENHANCE_DAMAGE_PERSENT,
            INCREASE_DAMAGE_TO_BOSS,
            INCREASE_DAMAGE_TO_BOSS_PERSENT,
            INCREASE_DAMAGE_WHEN_ENEMY_SINGLE,
            INCREASE_DAMAGE_WHEN_ENEMY_SINGLE_PERSENT,
            INCREASE_DAMAGE_WHEN_ENEMY_MORE_THAN_3,
            INCREASE_DAMAGE_WHEN_ENEMY_MORE_THAN_3_PERSENT,
            INCREASE_DAMAGE_AS_COMBOS_GO_UP,
            INCREASE_DAMAGE_AS_COMBOS_GO_UP_PERSENT,
            INCREASE_DAMAGE_WHILE_LOWERING_HP,
            INCREASE_DAMAGE_WHILE_LOWERING_HP_PERSENT,
            INCREASE_DAMAGE_TO_TARGET_HP_MORE_THEN_60,
            INCREASE_DAMAGE_TO_TARGET_HP_MORE_THEN_60_PERSENT,
            INCREASE_DAMAGE_TO_TARGET_HP_LESS_THEN_40,
            INCREASE_DAMAGE_TO_TARGET_HP_LESS_THEN_40_PERSENT,

            //other
            INCREASE_DEFENCE_TO_BOSS = 200,
            INCREASE_DEFENCE_TO_ELITE,
            INCREASE_DEFENCE_WHILE_LOWERING_HP,
            INCREASE_DEFENCE_WHILE_LOWERING_HP_PERSENT,
            ATTACKER_TAKES_DAMAGE,
            ATTACKER_TAKES_DAMAGE_PERSENT,
            ADD_EXPERIENCE,
            HEAL_AFTER_KILL,
            PREVENT_MONSTER_HEAL,
            IGNORE_TARGET_DEFENSE,

            //element weapon
            SKILL1_FIRE_DAMAGE = 1000,
            SKILL1_FIRE_BUFF1,
            SKILL1_FIRE_BUFF2,
            SKILL1_FIRE_SPECIL,
            SKILL2_FIRE_DAMAGE,
            SKILL2_FIRE_BUFF1,
            SKILL2_FIRE_BUFF2,
            SKILL2_FIRE_SPECIL,
            SKILL3_FIRE_DAMAGE,
            SKILL3_FIRE_BUFF1,
            SKILL3_FIRE_BUFF2,
            SKILL3_FIRE_SPECIL,

            SKILL1_COLD_DAMAGE,
            SKILL1_COLD_BUFF1,
            SKILL1_COLD_BUFF2,
            SKILL1_COLD_SPECIL,
            SKILL2_COLD_DAMAGE,
            SKILL2_COLD_BUFF1,
            SKILL2_COLD_BUFF2,
            SKILL2_COLD_SPECIL,
            SKILL3_COLD_DAMAGE,
            SKILL3_COLD_BUFF1,
            SKILL3_COLD_BUFF2,
            SKILL3_COLD_SPECIL,

            SKILL1_LIGHTING_DAMAGE,
            SKILL1_LIGHTING_BUFF1,
            SKILL1_LIGHTING_BUFF2,
            SKILL1_LIGHTING_SPECIL,
            SKILL2_LIGHTING_DAMAGE,
            SKILL2_LIGHTING_BUFF1,
            SKILL2_LIGHTING_BUFF2,
            SKILL2_LIGHTING_SPECIL,
            SKILL3_LIGHTING_DAMAGE,
            SKILL3_LIGHTING_BUFF1,
            SKILL3_LIGHTING_BUFF2,
            SKILL3_LIGHTING_SPECIL,

            SKILL1_WIND_DAMAGE,
            SKILL1_WIND_BUFF1,
            SKILL1_WIND_BUFF2,
            SKILL1_WIND_SPECIL,
            SKILL2_WIND_DAMAGE,
            SKILL2_WIND_BUFF1,
            SKILL2_WIND_BUFF2,
            SKILL2_WIND_SPECIL,
            SKILL3_WIND_DAMAGE,
            SKILL3_WIND_BUFF1,
            SKILL3_WIND_BUFF2,
            SKILL3_WIND_SPECIL,

            //element common
            FIRE_ATTACK = 2000,
            FIRE_DEFENCE,
            IGNORE_TARGET_FIRE_DEFENCE,
            IGNORE_TARGET_FIRE_DEFENCE_PERSENT,
            FIRE_DAMAGE_ENHANCE,
            FIRE_DAMAGE_ENHANCE_PERSENT,
            FIRE_DAMAGE_REDUSE,
            FIRE_DAMAGE_REDUSE_PERSENT,
            FIRE_ABSORBS,
            FIRE_ABSORBS_PERSENT,

            COLD_ATTACK,
            COLD_DEFENCE,
            IGNORE_TARGET_COLD_DEFENCE,
            IGNORE_TARGET_COLD_DEFENCE_PERSENT,
            COLD_DAMAGE_ENHANCE,
            COLD_DAMAGE_ENHANCE_PERSENT,
            COLD_DAMAGE_REDUSE,
            COLD_DAMAGE_REDUSE_PERSENT,
            COLD_ABSORBS,
            COLD_ABSORBS_PERSENT,

            LIGHTING_ATTACK,
            LIGHTING_DEFENCE,
            IGNORE_TARGET_LIGHTING_DEFENCE,
            IGNORE_TARGET_LIGHTING_DEFENCE_PERSENT,
            LIGHTING_DAMAGE_ENHANCE,
            LIGHTING_DAMAGE_ENHANCE_PERSENT,
            LIGHTING_DAMAGE_REDUSE,
            LIGHTING_DAMAGE_REDUSE_PERSENT,
            LIGHTING_ABSORBS,
            LIGHTING_ABSORBS_PERSENT,

            WIND_ATTACK,
            WIND_DEFENCE,
            IGNORE_TARGET_WIND_DEFENCE,
            IGNORE_TARGET_WIND_DEFENCE_PERSENT,
            WIND_DAMAGE_ENHANCE,
            WIND_DAMAGE_ENHANCE_PERSENT,
            WIND_DAMAGE_REDUSE,
            WIND_DAMAGE_REDUSE_PERSENT,
            WIND_ABSORBS,
            WIND_ABSORBS_PERSENT,

            //specil skill
            PRO1_SPECIL_SKILL1 = 4000,
            PRO1_SPECIL_SKILL2,
            PRO1_SPECIL_SKILL3,
            PRO2_SPECIL_SKILL1,
            PRO2_SPECIL_SKILL2,
            PRO2_SPECIL_SKILL3,

        }

        public List<FightAttrRecord> GetAttrLevelRequire(int level)
        {
            List<FightAttrRecord> levelRecord = new List<FightAttrRecord>();
            foreach (var record in Records)
            {
                if (record.Value.LevelMin > 0 && record.Value.LevelMin > level)
                    continue;

                if (record.Value.LevelMax > 0 && record.Value.LevelMax < level)
                    continue;

                levelRecord.Add(record.Value);
            }

            return levelRecord;
        }
        
    }

}