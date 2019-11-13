using UnityEngine;
using System.Collections;

public class ImpactExAttack : ImpactBase
{

    void Update()
    {
        if (SenderMotion == null || SenderMotion.ActingSkill!= SkillMotion)
        {
            FinishImpact(SenderMotion);
            return;
        }

        UpdateInput();
    }


    private ObjMotionSkillBase _ExAttackSkill;
    public int _AttackTimes;
    public float _Damage;

    public override void Init(ObjMotionSkillBase skillMotion, SelectBase selector)
    {
        base.Init(skillMotion, selector);

        string skillPath = "SkillMotion/" + skillMotion.MotionManager._MotionAnimPath + "/AttackEx";
        ResourcePool.Instance.LoadConfig(skillPath, (resName, resGO, hash) =>
        {
            _ExAttackSkill = resGO.GetComponent<ObjMotionSkillBase>();
            var hitCollider = resGO.GetComponentInChildren<SelectCollider>();
            if (hitCollider != null)
            {
                while (hitCollider._EventFrame.Count > _AttackTimes)
                {
                    hitCollider._EventFrame.RemoveAt(hitCollider._EventFrame.Count - 1);
                }
            }
            _ExAttackSkill.transform.SetParent(skillMotion.transform.parent);
            _ExAttackSkill.Init();
        }, null);

    }

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        this.enabled = true;
    }

    public override void FinishImpact(MotionManager reciverManager)
    {
        base.FinishImpact(reciverManager);

        this.enabled = false;
    }

    private void UpdateInput()
    {
        if (InputManager.Instance.IsKeyHold("j"))
        {
            SenderMotion.ActSkill(_ExAttackSkill);
        }
    }

}
