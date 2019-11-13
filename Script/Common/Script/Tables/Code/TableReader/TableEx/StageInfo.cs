using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class StageInfoRecord : TableRecordBase
    {
        private List<string> _ValidScenePath;
        public List<string> ValidScenePath
        {
            get
            {
                if (_ValidScenePath == null)
                {
                    _ValidScenePath = new List<string>();
                    foreach (var scenePath in ScenePath)
                    {
                        if (!string.IsNullOrEmpty(scenePath))
                        {
                            _ValidScenePath.Add(scenePath);
                        }
                    }
                }
                return _ValidScenePath;
            }
        }

        public List<string> GetValidScenePath()
        {
            if (ValidScenePath.Count == 0)
            {
                List<string> randomScenes = new List<string>();
                //randomScenes.Add("Stage_ShaMo_09");
                randomScenes.Add(FightSceneLogicRandomArea.GetRandomScene());
                return randomScenes;
            }
            else
            {
                return ValidScenePath;
            }
        }
    }

    public partial class StageInfo : TableFileBase
    {

        private int _MaxNormalStageID = -1;

        public int GetMaxNormalStageID()
        {
            if (_MaxNormalStageID > 0)
                return _MaxNormalStageID;

            foreach (var stageRecord in Records)
            {
                if (stageRecord.Value.StageType == STAGE_TYPE.NORMAL)
                {
                    int stageID = int.Parse(stageRecord.Key);
                    if (stageID > _MaxNormalStageID)
                    {
                        _MaxNormalStageID = stageID;
                    }
                }
            }

            return _MaxNormalStageID;
        }

    }
}