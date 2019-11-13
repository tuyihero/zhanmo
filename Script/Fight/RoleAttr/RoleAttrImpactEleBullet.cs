using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactEleBullet: RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        _SkillInput = skillInput;
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];

        ResourcePool.Instance.LoadConfig("Bullet\\Emitter\\Element\\" + _ImpactName, (resName, resGO, hash) =>
        {
            resGO.transform.SetParent(skillMotion.transform);

            var bulletEmitterEle = resGO.GetComponent<BulletEmitterElement>();
            bulletEmitterEle._Rate = _Rate;
            bulletEmitterEle._Damage = _Damage;
        }, null);
    }

    #region 

    public int _Rate;
    public int _Damage;
    public string _ImpactName;

    //public static EquipExAttr GetRandomExAttr(int level, int equipValue, Tables.ITEM_QUALITY quality)
    //{
    //    int rate = (int)(level * 0.1f * 500);
    //    rate = Mathf.Clamp(rate, 500, 5000);

    //    float randomValue = (equipValue * Random.Range(0.85f, 1.15f));
    //    int damage = (int)(100 * randomValue);

    //    int attrIDX = Random.Range((int)RoleAttrEnum.Skill1FireBoom, (int)RoleAttrEnum.Skill3WindAimTarget + 1);

    //    EquipExAttr exAttr = new EquipExAttr("RoleAttrImpactEleBullet", attrIDX, rate, damage);

    //    return exAttr;
    //}

    //public void InitEleBullet(RoleAttrEnum attrType, int rate, int damage)
    //{
    //    _Rate = rate;
    //    _Damage = damage;
    //    switch (attrType)
    //    {
    //        case RoleAttrEnum.Skill1FireBoom:
    //            _SkillInput = "1";
    //            _ImpactName = "EleMidBoomFire";
    //            break;
    //        case RoleAttrEnum.Skill1FireBall:
    //            _SkillInput = "1";
    //            _ImpactName = "EleRangeBallFire";
    //            break;
    //        case RoleAttrEnum.Skill1FireExplore:
    //            _SkillInput = "1";
    //            _ImpactName = "EleLineBoomFire";
    //            break;
    //        case RoleAttrEnum.Skill1FireRandomBoom:
    //            _SkillInput = "1";
    //            _ImpactName = "EleRandomBoomFire";
    //            break;
    //        case RoleAttrEnum.Skill1FireAimTarget:
    //            _SkillInput = "1";
    //            _ImpactName = "EleTargetBoomFire";
    //            break;
    //        case RoleAttrEnum.Skill1IceBoom:
    //            _SkillInput = "1";
    //            _ImpactName = "EleMidBoomIce";
    //            break;
    //        case RoleAttrEnum.Skill1IceBall:
    //            _SkillInput = "1";
    //            _ImpactName = "EleRangeBallIce";
    //            break;
    //        case RoleAttrEnum.Skill1IceExplore:
    //            _SkillInput = "1";
    //            _ImpactName = "EleLineBoomIce";
    //            break;
    //        case RoleAttrEnum.Skill1IceRandomBoom:
    //            _SkillInput = "1";
    //            _ImpactName = "EleRandomBoomIce";
    //            break;
    //        case RoleAttrEnum.Skill1IceAimTarget:
    //            _SkillInput = "1";
    //            _ImpactName = "EleTargetBoomIce";
    //            break;
    //        case RoleAttrEnum.Skill1LightBoom:
    //            _SkillInput = "1";
    //            _ImpactName = "EleMidBoomLight";
    //            break;
    //        case RoleAttrEnum.Skill1LightBall:
    //            _SkillInput = "1";
    //            _ImpactName = "EleRangeBallLight";
    //            break;
    //        case RoleAttrEnum.Skill1LightExplore:
    //            _SkillInput = "1";
    //            _ImpactName = "EleLineBoomLight";
    //            break;
    //        case RoleAttrEnum.Skill1LightRandomBoom:
    //            _SkillInput = "1";
    //            _ImpactName = "EleRandomBoomLight";
    //            break;
    //        case RoleAttrEnum.Skill1LightAimTarget:
    //            _SkillInput = "1";
    //            _ImpactName = "EleTargetBoomLight";
    //            break;
    //        case RoleAttrEnum.Skill1WindBoom:
    //            _SkillInput = "1";
    //            _ImpactName = "EleMidBoomWind";
    //            break;
    //        case RoleAttrEnum.Skill1WindBall:
    //            _SkillInput = "1";
    //            _ImpactName = "EleRangeBallWind";
    //            break;
    //        case RoleAttrEnum.Skill1WindExplore:
    //            _SkillInput = "1";
    //            _ImpactName = "EleLineBoomWind";
    //            break;
    //        case RoleAttrEnum.Skill1WindRandomBoom:
    //            _SkillInput = "1";
    //            _ImpactName = "EleRandomBoomWind";
    //            break;
    //        case RoleAttrEnum.Skill1WindAimTarget:
    //            _SkillInput = "1";
    //            _ImpactName = "EleTargetBoomWind";
    //            break;
    //        case RoleAttrEnum.Skill2FireBoom:
    //            _SkillInput = "2";
    //            _ImpactName = "EleMidBoomFire";
    //            break;
    //        case RoleAttrEnum.Skill2FireBall:
    //            _SkillInput = "2";
    //            _ImpactName = "EleRangeBallFire";
    //            break;
    //        case RoleAttrEnum.Skill2FireExplore:
    //            _SkillInput = "2";
    //            _ImpactName = "EleLineBoomFire";
    //            break;
    //        case RoleAttrEnum.Skill2FireRandomBoom:
    //            _SkillInput = "2";
    //            _ImpactName = "EleRandomBoomFire";
    //            break;
    //        case RoleAttrEnum.Skill2FireAimTarget:
    //            _SkillInput = "2";
    //            _ImpactName = "EleTargetBoomFire";
    //            break;
    //        case RoleAttrEnum.Skill2IceBoom:
    //            _SkillInput = "2";
    //            _ImpactName = "EleMidBoomIce";
    //            break;
    //        case RoleAttrEnum.Skill2IceBall:
    //            _SkillInput = "2";
    //            _ImpactName = "EleRangeBallIce";
    //            break;
    //        case RoleAttrEnum.Skill2IceExplore:
    //            _SkillInput = "2";
    //            _ImpactName = "EleLineBoomIce";
    //            break;
    //        case RoleAttrEnum.Skill2IceRandomBoom:
    //            _SkillInput = "2";
    //            _ImpactName = "EleRandomBoomIce";
    //            break;
    //        case RoleAttrEnum.Skill2IceAimTarget:
    //            _SkillInput = "2";
    //            _ImpactName = "EleTargetBoomIce";
    //            break;
    //        case RoleAttrEnum.Skill2LightBoom:
    //            _SkillInput = "2";
    //            _ImpactName = "EleMidBoomLight";
    //            break;
    //        case RoleAttrEnum.Skill2LightBall:
    //            _SkillInput = "2";
    //            _ImpactName = "EleRangeBallLight";
    //            break;
    //        case RoleAttrEnum.Skill2LightExplore:
    //            _SkillInput = "2";
    //            _ImpactName = "EleLineBoomLight";
    //            break;
    //        case RoleAttrEnum.Skill2LightRandomBoom:
    //            _SkillInput = "2";
    //            _ImpactName = "EleRandomBoomLight";
    //            break;
    //        case RoleAttrEnum.Skill2LightAimTarget:
    //            _SkillInput = "2";
    //            _ImpactName = "EleTargetBoomLight";
    //            break;
    //        case RoleAttrEnum.Skill2WindBoom:
    //            _SkillInput = "2";
    //            _ImpactName = "EleMidBoomWind";
    //            break;
    //        case RoleAttrEnum.Skill2WindBall:
    //            _SkillInput = "2";
    //            _ImpactName = "EleRangeBallWind";
    //            break;
    //        case RoleAttrEnum.Skill2WindExplore:
    //            _SkillInput = "2";
    //            _ImpactName = "EleLineBoomWind";
    //            break;
    //        case RoleAttrEnum.Skill2WindRandomBoom:
    //            _SkillInput = "2";
    //            _ImpactName = "EleRandomBoomWind";
    //            break;
    //        case RoleAttrEnum.Skill2WindAimTarget:
    //            _SkillInput = "2";
    //            _ImpactName = "EleTargetBoomWind";
    //            break;
    //        case RoleAttrEnum.Skill3FireBoom:
    //            _SkillInput = "3";
    //            _ImpactName = "EleMidBoomFire";
    //            break;
    //        case RoleAttrEnum.Skill3FireBall:
    //            _SkillInput = "3";
    //            _ImpactName = "EleRangeBallFire";
    //            break;
    //        case RoleAttrEnum.Skill3FireExplore:
    //            _SkillInput = "3";
    //            _ImpactName = "EleLineBoomFire";
    //            break;
    //        case RoleAttrEnum.Skill3FireRandomBoom:
    //            _SkillInput = "3";
    //            _ImpactName = "EleRandomBoomFire";
    //            break;
    //        case RoleAttrEnum.Skill3FireAimTarget:
    //            _SkillInput = "3";
    //            _ImpactName = "EleTargetBoomFire";
    //            break;
    //        case RoleAttrEnum.Skill3IceBoom:
    //            _SkillInput = "3";
    //            _ImpactName = "EleMidBoomIce";
    //            break;
    //        case RoleAttrEnum.Skill3IceBall:
    //            _SkillInput = "3";
    //            _ImpactName = "EleRangeBallIce";
    //            break;
    //        case RoleAttrEnum.Skill3IceExplore:
    //            _SkillInput = "3";
    //            _ImpactName = "EleLineBoomIce";
    //            break;
    //        case RoleAttrEnum.Skill3IceRandomBoom:
    //            _SkillInput = "3";
    //            _ImpactName = "EleRandomBoomIce";
    //            break;
    //        case RoleAttrEnum.Skill3IceAimTarget:
    //            _SkillInput = "3";
    //            _ImpactName = "EleTargetBoomIce";
    //            break;
    //        case RoleAttrEnum.Skill3LightBoom:
    //            _SkillInput = "3";
    //            _ImpactName = "EleMidBoomLight";
    //            break;
    //        case RoleAttrEnum.Skill3LightBall:
    //            _SkillInput = "3";
    //            _ImpactName = "EleRangeBallLight";
    //            break;
    //        case RoleAttrEnum.Skill3LightExplore:
    //            _SkillInput = "3";
    //            _ImpactName = "EleLineBoomLight";
    //            break;
    //        case RoleAttrEnum.Skill3LightRandomBoom:
    //            _SkillInput = "3";
    //            _ImpactName = "EleRandomBoomLight";
    //            break;
    //        case RoleAttrEnum.Skill3LightAimTarget:
    //            _SkillInput = "3";
    //            _ImpactName = "EleTargetBoomLight";
    //            break;
    //        case RoleAttrEnum.Skill3WindBoom:
    //            _SkillInput = "3";
    //            _ImpactName = "EleMidBoomWind";
    //            break;
    //        case RoleAttrEnum.Skill3WindBall:
    //            _SkillInput = "3";
    //            _ImpactName = "EleRangeBallWind";
    //            break;
    //        case RoleAttrEnum.Skill3WindExplore:
    //            _SkillInput = "3";
    //            _ImpactName = "EleLineBoomWind";
    //            break;
    //        case RoleAttrEnum.Skill3WindRandomBoom:
    //            _SkillInput = "3";
    //            _ImpactName = "EleRandomBoomWind";
    //            break;
    //        case RoleAttrEnum.Skill3WindAimTarget:
    //            _SkillInput = "3";
    //            _ImpactName = "EleTargetBoomWind";
    //            break;
    //    }
    //}

    #endregion
}
