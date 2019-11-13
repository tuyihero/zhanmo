using UnityEngine;
using System.Collections;

public class AI_EliteHeroJianZiYa : AI_HeroIntNormal
{
    protected override void Init()
    {
        base.Init();

        SuperArmorPrefab.ActBuffInstance(_SelfMotion, _SelfMotion, 3600);
    }
}
