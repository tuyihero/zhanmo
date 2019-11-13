using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class FightAttrRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public int AttrID { get; set; }
        public string ShowTip { get; set; }
        public int LevelMin { get; set; }
        public int LevelMax { get; set; }
        public int SlotLimit { get; set; }
        public int ProfessionLimit { get; set; }
        public int Conflict { get; set; }
        public List<Vector3> Values { get; set; }
        public FightAttrRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            Values = new List<Vector3>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(AttrID));
            recordStrList.Add(TableWriteBase.GetWriteStr(ShowTip));
            recordStrList.Add(TableWriteBase.GetWriteStr(LevelMin));
            recordStrList.Add(TableWriteBase.GetWriteStr(LevelMax));
            recordStrList.Add(TableWriteBase.GetWriteStr(SlotLimit));
            recordStrList.Add(TableWriteBase.GetWriteStr(ProfessionLimit));
            recordStrList.Add(TableWriteBase.GetWriteStr(Conflict));
            foreach (var testTableItem in Values)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }

            return recordStrList.ToArray();
        }
    }

    public partial class FightAttr : TableFileBase
    {
        public Dictionary<string, FightAttrRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public FightAttrRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("FightAttr" + ": " + id, ex);
            }
        }

        public FightAttr(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, FightAttrRecord>();
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

                    FightAttrRecord record = new FightAttrRecord(data);
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
                pair.Value.AttrID = TableReadBase.ParseInt(pair.Value.ValueStr[3]);
                pair.Value.ShowTip = TableReadBase.ParseString(pair.Value.ValueStr[4]);
                pair.Value.LevelMin = TableReadBase.ParseInt(pair.Value.ValueStr[5]);
                pair.Value.LevelMax = TableReadBase.ParseInt(pair.Value.ValueStr[6]);
                pair.Value.SlotLimit = TableReadBase.ParseInt(pair.Value.ValueStr[7]);
                pair.Value.ProfessionLimit = TableReadBase.ParseInt(pair.Value.ValueStr[8]);
                pair.Value.Conflict = TableReadBase.ParseInt(pair.Value.ValueStr[9]);
                pair.Value.Values.Add(TableReadBase.ParseVector3(pair.Value.ValueStr[10]));
                pair.Value.Values.Add(TableReadBase.ParseVector3(pair.Value.ValueStr[11]));
                pair.Value.Values.Add(TableReadBase.ParseVector3(pair.Value.ValueStr[12]));
                pair.Value.Values.Add(TableReadBase.ParseVector3(pair.Value.ValueStr[13]));
                pair.Value.Values.Add(TableReadBase.ParseVector3(pair.Value.ValueStr[14]));
            }
        }
    }

}