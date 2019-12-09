using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SelectTargetType
{
    Enemy,
    Friend,
}

public class SelectTargetCommon
{
    public static MotionManager GetMainPlayer()
    {
        if (FightManager.Instance != null)
        {
            return FightManager.Instance.MainChatMotion;
        }
        else
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
                return null;
            return player.GetComponent<MotionManager>();
        }

        return null;
    }

    public static bool IsTargetEnemy(MotionManager selfMotion, MotionManager targetMotion)
    {
        if (targetMotion.gameObject.layer != FightLayerCommon.CAMP_1
            && targetMotion.gameObject.layer != FightLayerCommon.CAMP_2
            && targetMotion.gameObject.layer != FightLayerCommon.CAMP_BULLET_1
            && targetMotion.gameObject.layer != FightLayerCommon.CAMP_BULLET_2
            && targetMotion.gameObject.layer != FightLayerCommon.EVIL)
            return false;

        return selfMotion.gameObject.layer != targetMotion.gameObject.layer;
    }

    public static bool IsTargetFriend(MotionManager selfMotion, MotionManager targetMotion)
    {
        if (targetMotion.gameObject.layer != FightLayerCommon.CAMP_1
            && targetMotion.gameObject.layer != FightLayerCommon.CAMP_2
            && targetMotion.gameObject.layer != FightLayerCommon.CAMP_BULLET_1
            && targetMotion.gameObject.layer != FightLayerCommon.CAMP_BULLET_2
            && targetMotion.gameObject.layer != FightLayerCommon.EVIL)
            return false;

        return selfMotion.gameObject.layer != targetMotion.gameObject.layer;
    }

    public static bool IsTargetInType(MotionManager selfMotion, MotionManager targetMotion, SelectTargetType selectType)
    {
        if (selectType == SelectTargetType.Enemy)
        {
            return IsTargetEnemy(selfMotion, targetMotion);
        }
        else if (selectType == SelectTargetType.Friend)
        {
            return !IsTargetEnemy(selfMotion, targetMotion);
        }
        return false;
    }

    public static MotionManager GetNearMotion(MotionManager selfMotion, Vector3 startPosition, List<MotionManager> excludeMotions, SelectTargetType selectType = SelectTargetType.Enemy)
    {
        var motions = GameObject.FindObjectsOfType<MotionManager>();

        float minDistance = 0;
        MotionManager nearMotion = null;
        foreach (var motion in motions)
        {
            if (excludeMotions != null && excludeMotions.Contains(motion))
                continue;

            if (!IsTargetInType(selfMotion, motion, selectType))
            {
                continue;
            }

            float distance = Vector3.Distance(startPosition, motion.transform.position);
            if (nearMotion == null || distance < minDistance)
            {
                nearMotion = motion;
                minDistance = distance;
            }
        }

        return nearMotion;
    }

    public static List<MotionManager> GetNearMotions(MotionManager selfMotion, float length = 100)
    {
        var motions = GameObject.FindObjectsOfType<MotionManager>();

        List<MotionManager> nearMotions = new List<MotionManager>();
        foreach (var motion in motions)
        {
            if (motion == selfMotion)
                continue;

            float distance = Vector3.Distance(selfMotion.transform.position, motion.transform.position);
            if (distance > length)
                continue;

            nearMotions.Add(motion);
        }

        return nearMotions;
    }

    public enum SelectSortType
    {
        None,
        MonsterType,
        Distance,
        Angel,
    }
    public class SelectedInfo
    {
        public float _Distance;
        public float _Angle;
        public MotionManager _SelectedMotion;
    }
    public static List<SelectedInfo> GetFrontMotions(MotionManager selfMotion, float length, float angle, SelectSortType sortType, SelectTargetType targetType)
    {
        return GetDirectMotions(selfMotion, selfMotion.GetMotionForward(), length, angle, sortType, targetType);
    }

    public static List<SelectedInfo> GetFrontMotions(MotionManager selfMotion, float length)
    {
        var motions = GameObject.FindObjectsOfType<MotionManager>();

        List<SelectedInfo> nearMotions = GetFrontMotions(selfMotion, length, 30, SelectSortType.None, SelectTargetType.Enemy);

        return nearMotions;
    }

    public static List<SelectedInfo> GetDirectMotions(MotionManager selfMotion, Vector3 direct, float length, float angle, SelectSortType sortType, SelectTargetType targetType)
    {
        var motions = GameObject.FindObjectsOfType<MotionManager>();

        List<SelectedInfo> nearMotions = new List<SelectedInfo>();
        foreach (var motion in motions)
        {
            if (motion == selfMotion)
                continue;

            if (motion.IsMotionDie)
                continue;

            if (!IsTargetInType(selfMotion, motion, targetType))
                continue;

            float distance = Vector3.Distance(selfMotion.transform.position, motion.transform.position);
            if (distance > length)
                continue;

            float targetAngle = Mathf.Abs(Vector3.Angle(motion.transform.position - selfMotion.transform.position, direct));
            if (targetAngle > angle)
                continue;

            SelectedInfo selectedInfo = new SelectedInfo();
            selectedInfo._Distance = distance;
            selectedInfo._Angle = targetAngle;
            selectedInfo._SelectedMotion = motion;

            nearMotions.Add(selectedInfo);
        }

        if (sortType == SelectSortType.MonsterType)
        {
            nearMotions.Sort((motion1, motion2) =>
            {
                if ((int)motion1._SelectedMotion.RoleAttrManager.MotionType < (int)motion2._SelectedMotion.RoleAttrManager.MotionType)
                    return 1;
                else if ((int)motion1._SelectedMotion.RoleAttrManager.MotionType > (int)motion2._SelectedMotion.RoleAttrManager.MotionType)
                    return -1;
                else
                    return 0;
            });
        }
        else if (sortType == SelectSortType.Distance)
        {
            nearMotions.Sort((motion1, motion2) =>
            {
                if (motion1._Distance < motion2._Distance)
                    return -1;
                else if (motion1._Distance > motion2._Distance)
                    return 1;
                else
                    return 0;
            });
        }
        else if (sortType == SelectSortType.Angel)
        {
            nearMotions.Sort((motion1, motion2) =>
            {
                if (motion1._Angle < motion2._Angle)
                    return -1;
                else if (motion1._Angle > motion2._Angle)
                    return 1;
                else
                    return 0;
            });
        }

        return nearMotions;
    }
}
