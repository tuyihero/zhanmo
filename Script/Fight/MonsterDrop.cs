using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;

public class DropItemData
{
    public ItemEquip _ItemEquip;
    public ItemBase _ItemBase;
    public int _DropGold;
    public int _DropDiamond;
    public string _DropGemID;

    public Vector3 _DropPos;
    public Vector3 _MonsterPos;
    public bool _IsAutoPick;
}

public class MonsterDrop
{
    public static string IdentifictionItem = "50000";
    public static string[,] ResetItems = new string[5,3] { 
        { "20000","30000","40000"},
        { "20001","30001","40001"},
        { "20002","30002","40002"},
        { "20003","30003","40003"},
        { "20004","30004","40004"}};

    public static List<DropItem> _SceneDrops = new List<DropItem>();

    public static void MonsterDropItems(MotionManager monsterMotion)
    {

        //var drops = GetMonsterDrops(monsterMotion.MonsterBase, monsterMotion.RoleAttrManager.MotionType, monsterMotion.RoleAttrManager.Level, ActData.Instance._StageMode);
        //var randomPoses = GameRandom.GetIndependentRandoms(0, 16, drops.Count);
        //int posIdx = 0;
        //foreach (var drop in drops)
        //{
        //    var pos = GetDropPos(monsterMotion.transform, randomPoses[posIdx]);
        //    ++posIdx;
        //    drop._DropPos = pos;
        //    drop._MonsterPos = monsterMotion.transform.position;
        //    ResourceManager.Instance.LoadPrefab("Drop/DropItem", (resName, resGO, hash)=>
        //    {
        //        DropItem dropItem = resGO.GetComponent<DropItem>();
        //        dropItem.InitDrop(drop);
        //    }, null);
        //}
        MonsterDropItems(monsterMotion.MonsterBase, monsterMotion.RoleAttrManager.MotionType, monsterMotion.RoleAttrManager.Level, monsterMotion.transform);

        int dropExp = GameDataValue.GetMonsterExp(monsterMotion.RoleAttrManager.MotionType, monsterMotion.RoleAttrManager.Level, RoleData.SelectRole.RoleLevel, ActData.Instance._StageMode);
        RoleData.SelectRole.AddExp(dropExp);
    }

    public static void ClearAllDrops()
    {
        foreach (var dropItem in _SceneDrops)
        {
            if (dropItem != null)
            {
                ResourceManager.Instance.DestoryObj(dropItem.gameObject);
            }
        }
        _SceneDrops.Clear();
    }

    public static void MonsterDropItems(MonsterBaseRecord monsterBase, MOTION_TYPE motionType, int level, Transform dropBasePos)
    {
        var drops = GetMonsterDrops(monsterBase, motionType, level, ActData.Instance._StageMode);
        var randomPoses = GameRandom.GetIndependentRandoms(0, 16, drops.Count);
        int posIdx = 0;
        foreach (var drop in drops)
        {
            var pos = GetDropPos(dropBasePos, randomPoses[posIdx]);
            ++posIdx;
            drop._DropPos = pos;
            drop._MonsterPos = dropBasePos.position;
            ResourceManager.Instance.LoadPrefab("Drop/DropItem", (resName, resGO, hash) =>
            {
                DropItem dropItem = resGO.GetComponent<DropItem>();
                _SceneDrops.Add(dropItem);
                dropItem.InitDrop(drop);
            }, null);
        }
    }

    public static int DropGold = 0;
    public static List<DropItemData> GetMonsterDrops(Tables.MonsterBaseRecord monsterRecord, MOTION_TYPE monsterType, int level, STAGE_TYPE stageType = STAGE_TYPE.NORMAL)
    {
        List<DropItemData> dropList = new List<DropItemData>();

        if (stageType == STAGE_TYPE.NORMAL || stageType == STAGE_TYPE.BOSS)
        {//only normal and boss stage drop equip
            List<ItemEquip> dropEquips = GameDataValue.GetMonsterDropEquip(monsterType, monsterRecord, level, stageType);
            for (int i = 0; i < dropEquips.Count; ++i)
            {
                DropItemData dropItem = new DropItemData();
                dropItem._ItemEquip = dropEquips[i];
                dropList.Add(dropItem);
            }
        }

        var goldCnt = GameDataValue.GetGoldDropCnt(monsterType, level, stageType);
        for (int i = 0; i < goldCnt; ++i)
        {
            var goldNum = GameDataValue.GetGoldDropNum(level);
            DropItemData dropItem = new DropItemData();
            dropItem._DropGold = goldNum;
            dropList.Add(dropItem);

            DropGold += goldNum;
        }

        var dropGem = GameDataValue.GetGemMonsterDrop(monsterType, level, stageType);
        if (dropGem != null)
        {
            DropItemData dropItem = new DropItemData();
            dropItem._ItemBase = dropGem;
            dropList.Add(dropItem);

            //Debug.Log("Drop Gem !!");
        }

        var dropElement = GameDataValue.GetMonsterDropElement(level, stageType);
        if (dropElement != null)
        {
            DropItemData dropItem = new DropItemData();
            dropItem._ItemBase = dropElement;
            dropList.Add(dropItem);

            //Debug.Log("Drop Element !!");
        }

        var dropElementCore = GameDataValue.GetMonsterDropElementCore(level, stageType);
        if (dropElementCore != null)
        {
            DropItemData dropItem = new DropItemData();
            dropItem._ItemBase = dropElementCore;
            dropList.Add(dropItem);
        }

        if (monsterType == MOTION_TYPE.Hero)
        { 
            var dropBossTicket = GameDataValue.GetMonsterDropBossTicket(level, stageType);
            if (dropBossTicket != null)
            {
                DropItemData dropItem = new DropItemData();
                dropItem._ItemBase = dropBossTicket;
                dropList.Add(dropItem);
            }

            var dropActTicket = GameDataValue.GetMonsterDropActTicket(level, stageType);
            if (dropActTicket != null)
            {
                DropItemData dropItem = new DropItemData();
                dropItem._ItemBase = dropActTicket;
                dropList.Add(dropItem);
            }
        }

        return dropList;
    }
    
    private static Vector3 GetDropPos(Transform monsterMotion, int posIdx)
    {
        int rangeParam = posIdx / 8;
        int angleParam = posIdx % 8;

        float range = (rangeParam + 1) * 1;
        float angle = angleParam * 45;

        Vector3 pos = new Vector3(0, monsterMotion.transform.position.y, 0);
        pos.x = monsterMotion.transform.position.x + Mathf.Sin(angle) * range;
        pos.z = monsterMotion.transform.position.z + Mathf.Cos(angle) * range;

        UnityEngine.AI.NavMeshHit navMeshHit;
        if (UnityEngine.AI.NavMesh.SamplePosition(pos, out navMeshHit, range, UnityEngine.AI.NavMesh.AllAreas))
        {
            return navMeshHit.position;
        }
        return pos;
    }

    public static void PickItem(DropItemData dropItemData)
    {
        if (dropItemData == null)
            return;

        if (dropItemData._DropGold > 0)
        {
            PlayerDataPack.Instance.AddGold(dropItemData._DropGold);
        }
        else if (dropItemData._ItemEquip != null)
        {
            if (!BackBagPack.Instance.AddEquip(dropItemData._ItemEquip))
                return;
        }
        else if (dropItemData._ItemBase != null)
        {
            if (dropItemData._ItemBase is ItemGem)
            {
                GemData.Instance.CreateGem(dropItemData._ItemBase.ItemDataID, dropItemData._ItemBase.ItemStackNum);
            }
            else if (dropItemData._ItemBase is ItemFiveElementCore)
            {
                FiveElementData.Instance.AddCoreItem(dropItemData._ItemBase as ItemFiveElementCore);
            }
            else if (dropItemData._ItemBase is ItemFiveElement)
            {
                FiveElementData.Instance.AddElementItem(dropItemData._ItemBase as ItemFiveElement);
            }
            else
            {
                BackBagPack.Instance.PageItems.AddItem(dropItemData._ItemBase.ItemDataID, dropItemData._ItemBase.ItemStackNum);
            }

        }
        else
        {
            Debug.Log("Drop Empty");
        }
        
    }
}
