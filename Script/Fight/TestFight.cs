using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class TestFight : MonoBehaviour
{
    #region 

    public TestFight _Instance = null;

    #endregion

    // Update is called once per frame
    void Update ()
    {
        //if (_EnemyMotion == null)
        {
            if (!FindEnemy())
            {
                FindNextArea();
            }
        }

        //if (_EnemyMotion != null)
        {
            CloseUpdate();
        }

        UpdatePick();
    }

    void Start()
    {
        _NormalAttack = gameObject.GetComponentInChildren<ObjMotionSkillAttack>();
        InitDefence();

        _Instance = this;
    }

    private void OnEnable()
    {
        InputManager.Instance._EmulateMode = true;
    }

    private void OnDisable()
    {
        InputManager.Instance._EmulateMode = false;
    }

    #region find target

    private MotionManager _EnemyMotion;
    private Vector3 _NextAreaPos;

    private bool FindEnemy()
    {
        if (_EnemyMotion != null && !_EnemyMotion.IsMotionDie)
        {
            float distance = Vector3.Distance(transform.position, _EnemyMotion.transform.position);
            if (distance < 2.0f)
                return true;
        }

        _EnemyMotion = null;
        var motions = GameObject.FindObjectsOfType<MotionManager>();
        float tarDistance = 20;
        foreach (var motion in motions)
        {
            if (motion.RoleAttrManager.MotionType != MOTION_TYPE.MainChar && !motion.IsMotionDie)
            {
                float distance = Vector3.Distance(transform.position, motion.transform.position);
                if (distance < tarDistance)
                {
                    _EnemyMotion = motion;
                    tarDistance = distance;
                }
            }
        }

        return _EnemyMotion != null;
    }

    private bool FindNextArea()
    {
        var fightManager = GameObject.FindObjectOfType<FightSceneLogicPassArea>();
        if (fightManager != null)
        {
            _NextAreaPos = fightManager.GetNextAreaPos();
            if (_NextAreaPos == Vector3.zero)
            {
                return false;
            }
            return true;
        }

        var fightRandom = GameObject.FindObjectOfType<AreaGateRandom>();
        _NextAreaPos = fightRandom.transform.position;
        if (_NextAreaPos == Vector3.zero)
        {
            return false;
        }
        return true;
    }

    #endregion

    #region control

    private float _CloseRange = 2.0f;
    private float _SkillRange = 3.0f;
    private int _RandomSkillIdx = 0;
    private ObjMotionSkillAttack _NormalAttack;
    private int _WeaponSkill = -1;

    private void InitWeaponSkill()
    {
        if (_WeaponSkill != -1)
            return;

        var weaponItem = RoleData.SelectRole.GetEquipItem(EQUIP_SLOT.WEAPON);
        if (weaponItem == null || !weaponItem.IsVolid())
            return;

        foreach (var exAttrItem in weaponItem.EquipExAttrs)
        {
            if (exAttrItem.AttrType != "RoleAttrImpactBaseAttr")
            {
                var attrTab = TableReader.AttrValue.GetRecord(exAttrItem.AttrParams[0].ToString());
                _WeaponSkill = int.Parse(attrTab.StrParam[1]);
            }
        }
    }

    private void CloseUpdate()
    {

        //if (FightManager.Instance.MainChatMotion.ActingSkill != null)
        //{
        //    StartSkill();
        //    return;
        //}

        var destPos = Vector3.zero;
        if (_NextAreaPos != Vector3.zero)
        {
            destPos = _NextAreaPos;
        }
        if (_EnemyMotion != null && !_EnemyMotion.IsMotionDie)
        {
            destPos = _EnemyMotion.transform.position;
        }
        if (destPos == Vector3.zero)
            return;

        float distance = Vector3.Distance(transform.position, destPos);
        if (FightManager.Instance.MainChatMotion._ActionState != FightManager.Instance.MainChatMotion._StateSkill && distance > _CloseRange)
        {
            FightManager.Instance.MainChatMotion.StartMoveState(destPos);
            ReleaseSkill();
        }
        else
        {
            FightManager.Instance.MainChatMotion.StopMoveState();
            
            StartSkill(destPos);
        }
    }

    private bool StartSkill(Vector3 destPos)
    {
        if (_EnemyMotion == null)
        {
            InputManager.Instance.ReleasePress();
            return false;
        }

        if (_EnemyMotion.IsMotionDie)
        {
            InputManager.Instance.ReleasePress();
            return false;
        }

        InitWeaponSkill();

        if (FightManager.Instance.MainChatMotion.ActingSkill == null)
        {
            ++_RandomSkillIdx;
            if (_RandomSkillIdx > 3)
            {
                _RandomSkillIdx = 1;
            }
            //FightManager.Instance.MainChatMotion.ActSkill(FightManager.Instance.MainChatMotion._SkillMotions["j"]);
            transform.LookAt(_EnemyMotion.transform.position);
            InputManager.Instance.SetEmulatePress("j");
        }

        else if (FightManager.Instance.MainChatMotion.ActingSkill == _NormalAttack)
        {
            if (_WeaponSkill > 0)
            {
                if (_NormalAttack.CurStep > 0 && _NormalAttack.CurStep == _WeaponSkill)
                {
                    transform.LookAt(_EnemyMotion.transform.position);
                    InputManager.Instance.SetEmulatePress("k");
                }
            }
            else if (_NormalAttack.CurStep > 0 && _NormalAttack.CurStep == _RandomSkillIdx)
            {
                //if (_NormalAttack.CanNextInput)
                {
                    transform.LookAt(_EnemyMotion.transform.position);
                    InputManager.Instance.SetEmulatePress("k");
                    Debug.Log("emulate key k:" + _RandomSkillIdx);
                    ++_RandomSkillIdx;
                    if (_RandomSkillIdx > 3)
                    {
                        _RandomSkillIdx = 1;
                    }
                }
            }
        }
        return true;
    }

    private void ReleaseSkill()
    {
        InputManager.Instance.ReleasePress();
    }
    #endregion

    #region defence skill



    public void InitDefence()
    {
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_SOMEONE_SUPER_ARMOR, SomeOneSuperArmor);
    }

    private void SomeOneSuperArmor(object go, Hashtable eventArgs)
    {
        if (!InputManager.Instance._EmulateMode)
            return;
        MotionManager motion = (MotionManager)eventArgs["Motion"];
        if (motion == null || motion == FightManager.Instance.MainChatMotion)
            return;

        CancelInvoke("CancleDefence");

        Debug.Log("Use defence skill");
        InputManager.Instance.SetEmulatePress("l");

        Invoke("CancleDefence", 1);
    }

    private void CancleDefence()
    {
        InputManager.Instance.ReleasePress();
    }

    #endregion

    #region pick items

    public void UpdatePick()
    {
        if (!UIDropNamePanel.Instance)
            return;

        foreach (var dropItem in UIDropNamePanel.Instance._DropItems)
        {
            dropItem.OnItemClick();
        }
    }

    public static int _DropOrangeEquipCnt = 0;
    public static void DelAllEquip()
    {
        //equip
        foreach (var itemEquip in BackBagPack.Instance.PageEquips._PackItems)
        {
            if (BackBagPack.Instance.IsEquipBetter(itemEquip))
            {
                RoleData.SelectRole.PutOnEquip(itemEquip.EquipItemRecord.Slot, itemEquip);
            }
        }

        //destory
        ItemEquip storeWeapon = null;
        foreach (var itemEquip in BackBagPack.Instance.PageEquips._PackItems)
        {
            if (!itemEquip.IsVolid())
                continue;

            if (itemEquip.EquipQuality == ITEM_QUALITY.ORIGIN)
            {
                ++_DropOrangeEquipCnt;
                if (LegendaryData.Instance.IsCollectBetter(itemEquip))
                {
                    LegendaryData.Instance.PutInEquip(itemEquip);
                    continue;
                }
            }
        }

        BackBagPack.Instance.SellEquips(BackBagPack.Instance.PageEquips._PackItems);
    }

    public static void DelLevel()
    {
        while (RoleData.SelectRole.UnDistrubutePoint > 0)
        {
            RoleData.SelectRole.DistributePoint(1, 1);
        }

        //SkillData.Instance.SkillLevelUp
    }

    public static void DelSkill()
    {
        foreach (var skillInfo in SkillData.Instance.ProfessionSkills)
        {
            if (skillInfo.SkillRecord.SkillType.Equals("61000"))
            {
                SkillData.Instance.SkillLevelUp(skillInfo.SkillRecord.Id.ToString());
            }
        }
    }

    public static void DelSkill(int type)
    {
        var skillID = GetTestSkill(type);
        var skillData = SkillData.Instance.GetSkillInfo(skillID.ToString());
        SkillData.Instance.SkillLevelUp(skillID.ToString());
    }

    public static int GetTestSkill(int type)
    {
        int skillID = 0;
        //if (RoleData.SelectRole.Profession == PROFESSION.BOY_DEFENCE
        //    || RoleData.SelectRole.Profession == PROFESSION.BOY_DOUGE)
        //{
        //    switch (type)
        //    {
        //        case 0:
        //            skillID = 10001;
        //            break;
        //        case 1:
        //            skillID = 10002;
        //            break;
        //        case 2:
        //            skillID = 10003;
        //            break;
        //        case 3:
        //            skillID = 10004;
        //            break;
        //    }
        //}
        //else
        {
            switch (type)
            {
                case 0:
                    skillID = 10005;
                    break;
                case 1:
                    skillID = 10006;
                    break;
                case 2:
                    skillID = 10007;
                    break;
                case 3:
                    skillID = 10008;
                    break;
            }
        }

        return skillID;
    }

    public static void DelRefresh()
    {
        var weaponItem = RoleData.SelectRole.GetEquipItem(EQUIP_SLOT.WEAPON);
        if (weaponItem == null)
            return;

    }

    public static void DelGem()
    {
        UIGemPackCombine.CombineAll();

        if (GemSuit.Instance.ActSet == null)
        {
            var gemSetTab = TableReader.GemSet.GetRecord("120008");
            GemSuit.Instance.UseGemSet(gemSetTab);
        }

        if (GemSuit.Instance.ActSet == null)
        {
            for (int i = 0; i < GemData.Instance.EquipedGemDatas.Count; ++i)
            {
                foreach (var itemGem in GemData.Instance.PackExtraGemDatas._PackItems)
                {
                    if (GemData.Instance.IsEquipedGem(itemGem))
                        continue;

                    GemData.Instance.PutOnGem(itemGem, i);
                }

                foreach (var itemGem in GemData.Instance.PackGemDatas._PackItems)
                {
                    if (GemData.Instance.IsEquipedGem(itemGem))
                        continue;

                    GemData.Instance.PutOnGem(itemGem, i);
                }
            }
        }

        
    }

    #endregion

    #region combat log

    public static float GetDamageTotalRate()
    {
        int normalAtk = 0;
        int skilltotal = 0;
        int buffTotal = 0;
        foreach (var skillInfo in SkillData.Instance.ProfessionSkills)
        {
            if (skillInfo.SkillRecord.SkillType.Equals("61000"))
            {
                if (skillInfo.SkillRecord.SkillInput.Equals("j"))
                {
                    normalAtk = GameDataValue.GetSkillDamageRate(skillInfo.SkillActureLevel, skillInfo.SkillRecord.EffectValue);
                }
                else if (skillInfo.SkillRecord.SkillInput.Equals("1")
                    || skillInfo.SkillRecord.SkillInput.Equals("2")
                    || skillInfo.SkillRecord.SkillInput.Equals("3"))
                {
                    skilltotal += GameDataValue.GetSkillDamageRate(skillInfo.SkillActureLevel, skillInfo.SkillRecord.EffectValue);
                }
                else if (skillInfo.SkillRecord.SkillInput.Equals("5")
                    || skillInfo.SkillRecord.SkillInput.Equals("6"))
                {
                    buffTotal += Mathf.Abs( GameDataValue.GetSkillDamageRate(skillInfo.SkillActureLevel, skillInfo.SkillRecord.EffectValue));
                }
            }
        }
        float skillRate = GameDataValue.ConfigIntToFloat(normalAtk + skilltotal);
        float buffRate = GameDataValue.ConfigIntToFloat(buffTotal) + 1;

        float attrRate = 0;
        foreach (var attr in RoleData.SelectRole._BaseAttr._ExAttr)
        {
            if (attr is RoleAttrImpactElementBullet)
            {
                var attrBullet = attr as RoleAttrImpactElementBullet;
                attrRate += attrBullet._Damage;
            }
            else if (attr is RoleAttrImpactPassiveDamageArea
                || attr is RoleAttrImpactPassiveHitEnemy
                || attr is RoleAttrImpactPassiveLightCircleBuff)
            {
                attrRate += 0.2f;
            }
            else if (attr is RoleAttrImpactPassiveShadowHit)
            {
                var attrBullet = attr as RoleAttrImpactPassiveShadowHit;
                float enhance = attrBullet._ShadowHitCnt * attrBullet._HitDamage;
                attrRate += skilltotal * enhance;
            }
        }

        return (skillRate + attrRate)* buffRate;
    }

    public static int GetDamageAttr()
    {
        var phyEnhance = 1 + GameDataValue.ConfigIntToFloat(RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.PhysicDamageEnhance));
        float physic = RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Attack) * phyEnhance;

        var fireEnhance = 1 + GameDataValue.ConfigIntToFloat(RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.FireEnhance));
        float fire = RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.FireAttackAdd) * fireEnhance;

        var coldEnhance = 1 + GameDataValue.ConfigIntToFloat(RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.ColdEnhance));
        float cold = RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.ColdAttackAdd) * coldEnhance;

        var lightEnhance = 1 + GameDataValue.ConfigIntToFloat(RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.LightingEnhance));
        float light = RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.LightingAttackAdd) * lightEnhance;

        var windEnhance = 1 + GameDataValue.ConfigIntToFloat(RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.WindEnhance));
        float wind = RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.WindAttackAdd) * windEnhance;

        return (int)(physic + fire + cold + light + wind);
    }

    #endregion

    public static void TestDrop()
    {
        var monRecord = TableReader.MonsterBase.GetRecord("1");

        MonsterDrop.MonsterDropItems(monRecord, MOTION_TYPE.Hero, 10, FightManager.Instance.MainChatMotion.transform);
    }

    public static bool TestMode = false;

}
