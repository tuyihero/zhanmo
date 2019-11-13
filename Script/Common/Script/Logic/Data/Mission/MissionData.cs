using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class MissionData : DataPackBase
{

    #region 唯一

    private static MissionData _Instance = null;
    public static MissionData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new MissionData();
            }
            return _Instance;
        }
    }

    private MissionData()
    {
        _SaveFileName = "MissionData";
    }

    #endregion

    #region 

    public const int _SimpleMissionCnt = 5;
    public const int _ChallengeMissionCnt = 5;

    [SaveField(1)]
    private System.DateTime _RefreshTime;

    [SaveField(2)]
    public List<MissionItem> _MissionItems;

    [SaveField(3)]
    public List<MissionItem> _ChallengeItems;

    public void InitMissionData()
    {
        var timeSpan = System.DateTime.Now - _RefreshTime;
        if (timeSpan.Days > 0)
        {
            RefreshMissions();
            _RefreshTime = System.DateTime.Now;
            SaveClass(true);
        }

        foreach (var mission in _MissionItems)
        {
            mission.InitMissionItem();
        }

        foreach (var mission in _ChallengeItems)
        {
            mission.InitMissionItem();
        }
    }

    private void RefreshMissions()
    {
        List<MissionRecord> simpleMission = new List<MissionRecord>();
        List<MissionRecord> hardMission = new List<MissionRecord>();
        foreach (var missionRecord in TableReader.Mission.Records)
        {
            if (missionRecord.Value.Achieve == 1)
                continue;

            if (missionRecord.Value.HardStar == 1)
            {
                simpleMission.Add(missionRecord.Value);
            }
            else
            {
                hardMission.Add(missionRecord.Value);
            }
        }

        var simpleRandoms = GameRandom.GetIndependentRandoms(0, simpleMission.Count, _SimpleMissionCnt);
        var hardRandoms = GameRandom.GetIndependentRandoms(0, hardMission.Count, _ChallengeMissionCnt);

        _MissionItems = new List<MissionItem>();
        for (int i = 0; i < simpleRandoms.Count; ++i)
        {
            MissionItem missionItem = new MissionItem(simpleMission[simpleRandoms[i]]);
            _MissionItems.Add(missionItem);
        }

        _ChallengeItems = new List<MissionItem>();
        for (int i = 0; i < hardRandoms.Count; ++i)
        {
            MissionItem missionItem = new MissionItem(hardMission[hardRandoms[i]]);
            _ChallengeItems.Add(missionItem);
        }
    }

    #endregion

}
