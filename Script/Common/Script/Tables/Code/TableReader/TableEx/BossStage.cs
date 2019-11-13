using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class BossStageRecord : TableRecordBase
    {

    }

    public partial class BossStage : TableFileBase
    {
        private Dictionary<int, List<BossStageRecord>> _BossDiffRecords;
        public Dictionary<int, List<BossStageRecord>> BossDiffRecords
        {
            get
            {
                if (_BossDiffRecords == null)
                {
                    _BossDiffRecords = new Dictionary<int, List<BossStageRecord>>();
                    foreach (var record in Records.Values)
                    {
                        if (!_BossDiffRecords.ContainsKey(record.Difficult))
                        {
                            _BossDiffRecords.Add(record.Difficult, new List<BossStageRecord>());
                        }
                        _BossDiffRecords[record.Difficult].Add(record);
                    }
                }
                return _BossDiffRecords;
            }
        }

        public List<BossStageRecord> GetBossRecordsByDiff(int diff)
        {
            if (BossDiffRecords.ContainsKey(diff))
                return BossDiffRecords[diff];
            return BossDiffRecords[10];
        }

    }

}