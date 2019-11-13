using UnityEngine;
using System.Collections;

public class AI_DexHeroBase : AI_HeroBase
{
    protected override void Init()
    {
        base.Init();
        Debug.Log("init AI_StrengthHeroBase");
        InitSkills();
        //InitBlockSkill();
    }

    #region initSkill


    #endregion

    #region stage 2

    protected override void InitCrazyBuff()
    {
        base.InitCrazyBuff();

        //var buffGO = ResourceManager.Instance.GetGameObject("SkillMotion/CommonImpact/DexAccelateBuff");
        var buffGO = ResourcePool.Instance.GetConfig<Transform>(ResourcePool.ConfigEnum.DexAccelateBuff);
        _Strtage2Buff = buffGO.GetComponents<ImpactBuff>();


    }
    

    #endregion

    #region 

    public AnimationClip _BlockAnim;
    public int _AfterBlockSkill = -1;
    private ObjMotionSkillBase _SkillBlock;
    private float _BlockCD = 10;
    private float _LastBlockTime;

    protected int _NextSkillIdx = -1;

    private void InitBlockSkill()
    {
        ResourcePool.Instance.LoadConfig("SkillMotion/BlockSkill", (resName, resGO, hash) =>
        {
            var blockSkill = resGO;
            var motionTrans = _SelfMotion.transform.Find("Motion");
            blockSkill.transform.SetParent(motionTrans);
            _SkillBlock = blockSkill.GetComponent<ObjMotionSkillBase>();
            _SkillBlock._NextAnim[0] = _BlockAnim;
            _SkillBlock.Init();

            _LastBlockTime = -_BlockCD;
        }, null);
        
    }

    private void HitEvent(object sender, Hashtable eventArgs)
    {
        Debug.Log("HitEvent");
        if (Time.time - _LastBlockTime > _BlockCD)
        {
            _LastBlockTime = Time.time;

            _SkillBlock.ActSkill(null);

            _NextSkillIdx = _AfterBlockSkill;
        }
    }

    #endregion
}

