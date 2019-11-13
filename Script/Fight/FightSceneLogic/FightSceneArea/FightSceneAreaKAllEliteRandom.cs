using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AreaEliteInfo
{
    public string _EliteID = "201";
    public int _EliteCnt = 1;
}

public class FightSceneAreaKAllEliteRandom : FightSceneAreaKAllEnemy
{

    #region enemy step

    [SerializeField]
    public List<AreaEliteInfo> _AreaEliteInfo;

    private string _RandomNormalId = "";
    protected override void StartStep()
    {
        var randomIdx = Random.Range(0, _AreaEliteInfo.Count);
        for (int i = 0; i< _AreaEliteInfo[randomIdx]._EliteCnt; ++i)
        {
            MotionManager enemy = FightManager.Instance.InitEnemy(_AreaEliteInfo[randomIdx]._EliteID, _EnemyBornPos[i]._EnemyTransform.position, _EnemyBornPos[i]._EnemyTransform.rotation.eulerAngles);
            
            var enemyAI = enemy.gameObject.GetComponent<AI_Base>();
            _EnemyAI.Add(enemyAI);
            if (_IsEnemyAlert)
            {
                enemyAI._TargetMotion = FightManager.Instance.MainChatMotion;
                enemyAI.AIWake = true;
            }
            enemyAI._HuntRange = 999;
            enemyAI._ReHuntRange = 999;

            var eliteAI = enemyAI as AI_HeroBase;
            if (eliteAI != null)
            {
                eliteAI._IsContainsNormalAtk = true;
            }
        }
    }
    
    #endregion
    
}
