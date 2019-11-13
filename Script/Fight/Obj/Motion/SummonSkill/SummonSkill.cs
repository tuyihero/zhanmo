using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSkill
{

    #region 唯一

    private static SummonSkill _Instance = null;
    public static SummonSkill Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new SummonSkill();
            }
            return _Instance;
        }
    }

    #endregion

    #region summon in fight

    private static string _ModelShader = "Mobile/Particles/Additive Culled";
    private static Color _ColorRed = CommonDefine.HexToColor("90141480");
    private static Color _ColorOrigin = CommonDefine.HexToColor("D4990080");
    private static Color _ColorPurple = CommonDefine.HexToColor("60149080");
    private static Color _ColorBlue = CommonDefine.HexToColor("1136BD80");

    private Dictionary<string, AI_SummonSkill> _SummonMotions = new Dictionary<string, AI_SummonSkill>();

    public int GetSummonMotionCnt()
    {
        return _SummonMotions.Count;
    }

    public IEnumerator InitSummonMotions()
    {
        _CurSummonIdx = 0;
        _SummonMotions.Clear();

        for (int i = 0; i < SummonSkillData.Instance._UsingSummon.Count; ++i)
        {
            if (SummonSkillData.Instance._UsingSummon[i] == null)
                continue;

            yield return ResourcePool.Instance.InitSummonMotions(SummonSkillData.Instance._UsingSummon[i].SummonRecord.MonsterBase);

            var monsterBase = SummonSkillData.Instance._UsingSummon[i].SummonRecord.MonsterBase;
            var summonMotion = ResourcePool.Instance.GetIdleMotion(SummonSkillData.Instance._UsingSummon[i].SummonRecord.MonsterBase);
            summonMotion.InitRoleAttr(SummonSkillData.Instance._UsingSummon[i]);
            summonMotion.InitMotion();
            var shader = Shader.Find(_ModelShader);
            var meshRenders = summonMotion.Animation.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var meshRender in meshRenders)
            {
                meshRender.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                var mainTex = meshRender.material.GetTexture("_MainTex");
                meshRender.material.shader = shader;
                meshRender.material.SetTexture("_MainTex", mainTex);
                if (SummonSkillData.Instance._UsingSummon[i].SummonRecord.Quality == Tables.ITEM_QUALITY.BLUE)
                {
                    meshRender.material.SetColor("_TintColor", _ColorBlue);
                }
                else if (SummonSkillData.Instance._UsingSummon[i].SummonRecord.Quality == Tables.ITEM_QUALITY.PURPER)
                {
                    meshRender.material.SetColor("_TintColor", _ColorPurple);
                }
                else if (SummonSkillData.Instance._UsingSummon[i].SummonRecord.Quality == Tables.ITEM_QUALITY.ORIGIN)
                {
                    meshRender.material.SetColor("_TintColor", _ColorOrigin);
                }
            }

            FightLayerCommon.SetFriendLayer(summonMotion);
            var summonAI = summonMotion.GetComponent<AI_SummonSkill>();
            summonAI.InitSkillDamageRate(GameDataValue.GetSummonDamageRate(SummonSkillData.Instance.SummonLevel));
            _SummonMotions.Add(SummonSkillData.Instance._UsingSummon[i].SummonRecord.Id, summonAI);
            summonMotion.Animation.transform.localScale = summonMotion.Animation.transform.localScale * summonAI._ModelSizeFixed;
            
        }
    }

    public bool SummonAndSkill(int idx, MotionManager masterMotion)
    {
        if (SummonSkillData.Instance._UsingSummon[idx] != null)
        {
            return SummonAndSkill(SummonSkillData.Instance._UsingSummon[idx], SummonSkillData.Instance._UsingSummon[idx].SummonRecord.ActSkillIdx, masterMotion);
        }
        return false;
    }

    public bool SummonAndSkill(SummonMotionData summonData, int skillIdx, MotionManager masterMotion)
    {
        AI_SummonSkill summonAI = null;
        if (_SummonMotions.ContainsKey(summonData.SummonRecord.Id))
        {
            summonAI = _SummonMotions[summonData.SummonRecord.Id];
        }
        else
        {
            var monsterBase = Tables.TableReader.SummonSkill.GetRecord(summonData.SummonRecord.Id).MonsterBase;
            var summonMotion = ResourcePool.Instance.GetIdleMotion(monsterBase);
            summonMotion.InitRoleAttr(monsterBase, Tables.MOTION_TYPE.Normal);
            summonMotion.InitMotion();
            FightLayerCommon.SetFriendLayer(summonMotion);
            summonAI = summonMotion.GetComponent<AI_SummonSkill>();
            summonAI.InitSkillDamageRate(GameDataValue.GetSummonDamageRate(summonData.Level));
        }

        Vector3 pos = masterMotion.transform.position + masterMotion.transform.forward * summonAI._SummonPosZ;
        summonAI._SelfMotion._CanBeSelectByEnemy = false;
        summonAI._SelfMotion.transform.position = pos;
        summonAI._SelfMotion.transform.LookAt(masterMotion.transform);
        summonAI._SelfMotion.gameObject.SetActive(true);
        summonAI.UseSkill(skillIdx);

        return true;
    }

    public void HideSummonMotion(AI_SummonSkill summonAI)
    {
        summonAI._SelfMotion.transform.position = new Vector3(0, -10000, 0);
    }

    #endregion

    #region act skill

    private int _CurSummonIdx = 0;
    public int CurSummonIdx
    {
        get
        {
            return _CurSummonIdx;
        }
    }

    public void UseSummonSkill()
    {
        if (SummonSkillData.Instance._UsingSummon.Count <= _CurSummonIdx)
            return;

        if (SummonSkillData.Instance._UsingSummon[_CurSummonIdx] == null)
            return;

        if (FightSkillManager.Instance.IsSummonSkillInCD(SummonSkillData.Instance._UsingSummon[_CurSummonIdx]))
            return;

        if (SummonSkill.Instance.SummonAndSkill(_CurSummonIdx, FightManager.Instance.MainChatMotion))
        {
            int curIdx = _CurSummonIdx;
            ++_CurSummonIdx;
            if (_CurSummonIdx == SummonSkill.Instance.GetSummonMotionCnt())
            {
                _CurSummonIdx = 0;
            }
            for (int i = 0; i < SummonSkillData.USING_SUMMON_NUM; ++i)
            {
                if (SummonSkillData.Instance._UsingSummon[i] != null && i != curIdx)
                {
                    FightSkillManager.Instance.SetSummonCD(SummonSkillData.Instance._UsingSummon[i], SummonSkillData._SummonCommonCD, true);
                }
            }
            FightSkillManager.Instance.SetSummonCD(SummonSkillData.Instance._UsingSummon[curIdx], SummonSkillData.Instance.SummonSkillCD, false);
            //FightSkillManager.Instance.SetSummonCD(SummonSkillData.Instance._UsingSummon[_CurSummonIdx], SummonSkillData._SummonCommonCD, true);
        }


    }

    #endregion

}
