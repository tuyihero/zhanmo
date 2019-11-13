using UnityEngine;
using System.Collections;

public class AI_IntHeroBase : AI_HeroBase
{
    protected override void Init()
    {
        base.Init();
        Debug.Log("init AI_StrengthHeroBase");
        InitSkills();
    }

    #region super armor


    #endregion

    #region stage 2

    protected override void InitCrazyBuff()
    {
        base.InitCrazyBuff();

        //var buffGO = ResourceManager.Instance.GetGameObject("SkillMotion/CommonImpact/IntShieldBuff");
        var buffGO = ResourcePool.Instance.GetConfig<Transform>(ResourcePool.ConfigEnum.IntShieldBuff);
        _Strtage2Buff = buffGO.GetComponents<ImpactBuff>();
    }
    
    #endregion
}

