using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Tables;

public class TestData : SaveItemBase
{
    #region 唯一

    private static TestData _Instance = null;
    public static TestData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new TestData();
            }
            return _Instance;
        }
    }

    private TestData()
    {
        _SaveFileName = "TestData";
    }

    #endregion

    #region damage 

    public Dictionary<string, float> _DamageInfos = new Dictionary<string, float>();

    public void StartFight()
    {
        _DamageInfos.Clear();
    }

    public void SetDamage(ImpactBase impact, int finalDamage)
    {
#if UNITY_EDITOR
        ImpactHit impactHit = impact as ImpactHit;
        if (impactHit != null)
        {
            string skillName = "";
            if (impactHit._IsCharSkillDamage)
            {
                skillName = impactHit.SkillMotion._ActInput;
            }
            else
            {
                skillName = impactHit.gameObject.name;
            }

            if (!_DamageInfos.ContainsKey(skillName))
            {
                _DamageInfos.Add(skillName, 0);
            }
            _DamageInfos[skillName] += finalDamage;

            //if (impact.transform.parent != null)
            //{
            //    Debug.Log("Skill Damage:" + impactHit.transform.parent.name + "." + impactHit.gameObject.name + ", value:" + finalDamage);
            //}
            //else
            //{
            //    Debug.Log("Skill Damage:" + impactHit.gameObject.name + ", value:" + finalDamage);
            //}
        }
#endif

    }

    public void FinishFight(string stageName)
    {
        var time = DateTime.Now.Ticks;
        string fileName = "Stage_" + stageName + time.ToString();
        string path = Application.dataPath + "/DamageLog/" + fileName + ".txt";
        var fileStream = File.Create(path);
        var streamWriter = new StreamWriter(fileStream);
        foreach (var damageInfo in _DamageInfos)
        {
            streamWriter.WriteLine(damageInfo.Key + "\t" + damageInfo.Value.ToString());
        }
        streamWriter.Close();
    }

    #endregion
}
