using UnityEngine;
using System.Collections;

public class MonsterTestMonager : MonoBehaviour
{

    public string _CreateMonsterID;
    public Transform _Position;

    public void CreateTestMonster()
    {
        var monsterBase = Tables.TableReader.MonsterBase.GetRecord(_CreateMonsterID);
        if (monsterBase == null)
            return;

        var mainBase = ResourcePool.Instance.GetIdleMotion(monsterBase);
        mainBase.SetPosition(_Position.position);
        mainBase.SetRotate(_Position.rotation.eulerAngles);

        mainBase.InitRoleAttr(monsterBase, Tables.MOTION_TYPE.Normal);
        mainBase.InitMotion();
        FightLayerCommon.SetEnemyLayer(mainBase);

    }
}
