using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class MonsterBaseRecord : TableRecordBase
    {
        private List<CommonItemRecord> _ValidSpDrops;
        public List<CommonItemRecord> ValidSpDrops
        {
            get
            {
                if (_ValidSpDrops == null)
                {
                    _ValidSpDrops = new List<CommonItemRecord>();
                    foreach (var spDrop in SpDrops)
                    {
                        if (spDrop != null)
                        {
                            _ValidSpDrops.Add(spDrop);
                        }
                    }
                }
                return _ValidSpDrops;
            }
        }
    }

    public partial class MonsterBase : TableFileBase
    {
        Dictionary<int, List<MonsterBaseRecord>> _MonsterGroups;

        public MonsterBaseRecord GetGroupMonType(MonsterBaseRecord monsterBase, MOTION_TYPE motionType)
        {
            if (_MonsterGroups == null)
            {
                _MonsterGroups = new Dictionary<int, List<MonsterBaseRecord>>();
                foreach (var record in Records)
                {
                    if (!_MonsterGroups.ContainsKey(record.Value.MotionGroup))
                    {
                        _MonsterGroups.Add(record.Value.MotionGroup, new List<MonsterBaseRecord>());
                    }
                    _MonsterGroups[record.Value.MotionGroup].Add(record.Value);
                }

            }

            var monsterGroup = _MonsterGroups[monsterBase.MotionGroup];
            for (int i = 0; i < monsterGroup.Count; ++i)
            {
                if (monsterGroup[i].MotionType == motionType)
                    return monsterGroup[i];
            }

            return monsterBase;
        }

    }
}