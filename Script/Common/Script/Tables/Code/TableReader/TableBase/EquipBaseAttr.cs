using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class EquipBaseAttrRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public int Value { get; set; }
        public int Atk { get; set; }
        public int HP { get; set; }
        public int Defence { get; set; }
        public int DefenceStandar { get; set; }
        public EquipBaseAttrRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(Value));
            recordStrList.Add(TableWriteBase.GetWriteStr(Atk));
            recordStrList.Add(TableWriteBase.GetWriteStr(HP));
            recordStrList.Add(TableWriteBase.GetWriteStr(Defence));
            recordStrList.Add(TableWriteBase.GetWriteStr(DefenceStandar));

            return recordStrList.ToArray();
        }
    }

    public partial class EquipBaseAttr : TableFileBase
    {
        public Dictionary<string, EquipBaseAttrRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public EquipBaseAttrRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("EquipBaseAttr" + ": " + id, ex);
            }
        }

        public EquipBaseAttr(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, EquipBaseAttrRecord>();
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

                    EquipBaseAttrRecord record = new EquipBaseAttrRecord(data);
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
                pair.Value.Value = TableReadBase.ParseInt(pair.Value.ValueStr[3]);
                pair.Value.Atk = TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                pair.Value.HP = TableReadBase.ParseInt(pair.Value.ValueStr[5]);
                pair.Value.Defence = TableReadBase.ParseInt(pair.Value.ValueStr[6]);
                pair.Value.DefenceStandar = TableReadBase.ParseInt(pair.Value.ValueStr[7]);
            }
        }
    }

}