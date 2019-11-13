using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;

public class FightLayerCommon
{
    public const int CAMP_1 = 8;
    public const int CAMP_2 = 9;
    public const int CAMP_BULLET_1 = 12;
    public const int CAMP_BULLET_2 = 13;
    public const int DROP = 14;
    public const int EVIL = 17;

    public static void SetPlayerLayer(MotionManager playerMotion)
    {
        SetFriendLayer(playerMotion);
        playerMotion.gameObject.tag = "Player";
        //playerMotion.RoleAttrManager.MotionType = MOTION_TYPE.MainChar;
    }

    public static void SetFriendLayer(MotionManager playerMotion)
    {
        var skillColliders = playerMotion.GetComponentsInChildren<Collider>(true);
        foreach (var collider in skillColliders)
        {
            collider.gameObject.layer = CAMP_2;

        }

        playerMotion.TriggerCollider.gameObject.layer = CAMP_1;
        playerMotion.gameObject.layer = CAMP_1;
        //playerMotion.RoleAttrManager.MotionType = MOTION_TYPE.MainChar;
    }

    public static void SetEnemyLayer(MotionManager motion)
    {
        var skillColliders = motion.GetComponentsInChildren<Collider>(true);
        foreach (var collider in skillColliders)
        {
            collider.gameObject.layer = CAMP_1;
        }

        motion.TriggerCollider.gameObject.layer = CAMP_2;
        motion.gameObject.layer = CAMP_2;
        //motion.RoleAttrManager.MotionType = MOTION_TYPE.Normal;
    }

    public static int GetBulletLayer(MotionManager motion)
    {
        if (motion.TriggerCollider.gameObject.layer == CAMP_1)
            return CAMP_BULLET_2;
        else
            return CAMP_BULLET_1;
    }
}
