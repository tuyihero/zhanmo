using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class GemTableRecord : TableRecordBase
    {
        
    }

    public partial class GemTable : TableFileBase
    {
        public List<GemTableRecord> _RecordList;

        public GemTableRecord GetGemRecordByClass(int classType, int level)
        {
            int recordID = classType + level - 1;
            return GetRecord(recordID.ToString());
        }

        public GemTableRecord GetRandomRecord()
        {
            if (_RecordList == null)
            {
                _RecordList = new List<GemTableRecord>(Records.Values);
            }

            int randomIdx = UnityEngine.Random.Range(0, _RecordList.Count);
            return _RecordList[randomIdx];
        }

    }
}