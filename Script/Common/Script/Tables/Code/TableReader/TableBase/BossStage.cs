using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class BossStageRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public int Difficult { get; set; }
        public int Level { get; set; }
        public int Combat { get; set; }
        public List<string> FightLogic { get; set; }
        public MonsterBaseRecord BossID { get; set; }
        public BossStageRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            FightLogic = new List<string>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(Difficult));
            recordStrList.Add(TableWriteBase.GetWriteStr(Level));
            recordStrList.Add(TableWriteBase.GetWriteStr(Combat));
            foreach (var testTableItem in FightLogic)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            if (BossID != null)
            {
                recordStrList.Add(BossID.Id);
            }
            else
            {
                recordStrList.Add("");
            }

            return recordStrList.ToArray();
        }
    }

    public partial class BossStage : TableFileBase
    {
        public Dictionary<string, BossStageRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public BossStageRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("BossStage" + ": " + id, ex);
            }
        }

        public BossStage(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, BossStageRecord>();
            if(isPath)
            {
                string[] lines = File.ReadAllLines(pathOrContent);
                lines[0] = lines[0].Replace("\r\n", "\n");
                ParserTableStr(string.Join("\n", lines));
            }
            else
            {
                ParserTableStr(pathOrContent.Replace("\r\n", "\n"));
            }
        }

        private void ParserTableStr(string content)
        {
            StringReader rdr = new StringReader(content);
            using (var reader = new CsvReader(rdr))
            {
                HeaderRecord header = reader.ReadHeaderRecord();
                while (reader.HasMoreRecords)
                {
                    DataRecord data = reader.ReadDataRecord();
                    if (data[0].StartsWith("#"))
                        continue;

                    BossStageRecord record = new BossStageRecord(data);
                    Records.Add(record.Id, record);
                }
            }
        }

        public void CoverTableContent()
        {
            foreach (var pair in Records)
            {
                pair.Value.Name = TableReadBase.ParseString(pair.Value.ValueStr[1]);
                pair.Value.Desc = TableReadBase.ParseString(pair.Value.ValueStr[2]);
                pair.Value.Difficult = TableReadBase.ParseInt(pair.Value.ValueStr[3]);
                pair.Value.Level = TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                pair.Value.Combat = TableReadBase.ParseInt(pair.Value.ValueStr[5]);
                pair.Value.FightLogic.Add(TableReadBase.ParseString(pair.Value.ValueStr[6]));
                pair.Value.FightLogic.Add(TableReadBase.ParseString(pair.Value.ValueStr[7]));
                pair.Value.FightLogic.Add(TableReadBase.ParseString(pair.Value.ValueStr[8]));
                pair.Value.FightLogic.Add(TableReadBase.ParseString(pair.Value.ValueStr[9]));
                pair.Value.FightLogic.Add(TableReadBase.ParseString(pair.Value.ValueStr[10]));
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[11]))
                {
                    pair.Value.BossID =  TableReader.MonsterBase.GetRecord(pair.Value.ValueStr[11]);
                }
                else
                {
                    pair.Value.BossID = null;
                }
            }
        }
    }

}