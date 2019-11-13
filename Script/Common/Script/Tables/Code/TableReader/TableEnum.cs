using System.Collections;

namespace Tables
{

    //
    public enum ATTR_EFFECT
    {
        None = 0, //None,枚举必须保留0值
        DAMAGE = 10001, //ATTACK_DAMAGE,ATTACK_DAMAGE
        SPEED = 10002, //ATTACK_SPEED,ATTACK_SPEED
        RANGE = 10003, //ATTACK_RANGE,ATTACK_RANGE
        SKILL_ACCUMULATE = 10101, //SKILL_ACCUMULATE,SKILL_ACCUMULATE
        SKILL_SHADOWARRIOR = 10102, //SKILL_SHADOWARRIOR,SKILL_SHADOWARRIOR
        SKILL_EXATTACK = 10103, //SKILL_EXATTACK,SKILL_EXATTACK
        SKILL_ANOTHERUSE = 10104, //SKILL_ACCUMULATE,SKILL_ACCUMULATE
        SKILL_HW = 10105, //SKILL_HW,SKILL_HW
        SKILL_SPCEILSKILL = 10106, //SKILL_SPCEILSKILL,SKILL_SPCEILSKILL
        SKILL_ACTBUFF = 10201, //SKILL_ACTBUFF,SKILL_ACTBUFF
    }

    //
    public enum AWARD_TYPE
    {
        None = 0, //None,枚举必须保留0值
        Item = 1, //Item,Item
        Diamond = 2, //Diamond,Diamond
        Gold = 3, //Gold,Gold
    }

    //
    public enum EQUIP_CLASS
    {
        None = 0, //None,枚举必须保留0值
        Normal = 1, //Normal,Normal
        Legendary = 2, //Legendary,Legendary
    }

    //
    public enum EQUIP_SLOT
    {
        WEAPON = 0, //WEAPON,武器
        TORSO = 1, //TORSO,装甲
        LEGS = 2, //LEGS,护腿
        AMULET = 3, //AMULET,项链
        RING = 4, //RING,戒指
    }

    //
    public enum FIVE_ELEMENT
    {
        METAL = 0, //METAL,枚举必须保留0值
        WOOD = 1, //WOOD,WOOD
        WATER = 2, //WATER,WATER
        FIRE = 3, //FIRE,FIRE
        EARTH = 4, //EARTH,EARTH
    }

    //
    public enum GLOABL_BUFF_TYPE
    {
        None = 0, //None,枚举必须保留0值
        Talent = 1, //Talent,Talent
        ExTalent = 2, //ExTalent,ExTalent
        ExAttr = 3, //ExAttr,ExAttr
    }

    //
    public enum ITEM_QUALITY
    {
        WHITE = 0, //WHITE,枚举必须保留0值
        GREEN = 1, //GREEN,GREEN
        BLUE = 2, //BLUE,BLUE
        PURPER = 3, //PURPER,PURPER
        ORIGIN = 4, //ORIGIN,ORIGIN
    }

    //
    public enum MOTION_TYPE
    {
        MainChar = 0, //MainChar,枚举必须保留0值
        Hero = 1, //Hero,
        Elite = 2, //Elite,
        Normal = 3, //Normal,
        ExElite = 4, //ExElite,ExElite
    }

    //
    public enum PROFESSION
    {
        WARRIOR = 0, //WARRIOR,WARRIOR
        ASSASSIN = 1, //ASSASSIN,ASSASSIN
        MAGE = 2, //MAGE,MAGE
        MAX = 3, //MAX,MAX
        NONE = -1, //NONE,NONE
    }

    //
    public enum SKILL_CLASS
    {
        None = 0, //None,None
        NORMAL_ATTACK = 1, //NORMAL_ATTACK,NORMAL_ATTACK
        EX_SKILL = 2, //EX_SKILL,EX_SKILL
        BUFF = 3, //BUFF,BUFF
        DEFENCE = 4, //DEFENCE,DEFENCE
        DODGE = 5, //DODGE,DODGE
        BASE_ATTRS = 6, //BASE_ATTRS,BASE_ATTRS
        SKILL1 = 10, //SKILL1,SKILL1
        SKILL2 = 11, //SKILL2,SKILL2
        SKILL3 = 12, //SKILL3,SKILL3
    }

    //
    public enum SKILL_EFFECT
    {
        None = 0, //None,枚举必须保留0值
        SPEED = 1, //SPEED,
        DAMAGE = 2, //DAMAGE,
        RANGE = 3, //RANGE,
        TIME = 4, //TIME,
    }

    //
    public enum STAGE_TYPE
    {
        NORMAL = 0, //NORMAL,枚举必须保留0值
        BOSS = 1, //BOSS,BOSS
        DOUBLE_BOSS = 2, //DOUBLE_BOSS,DOUBLE_BOSS
        ACT_GOLD = 3, //ACT_GOLD,ACT_GOLD
        ACT_GEM = 4, //ACT_GEM,ACT_GEM
    }


}