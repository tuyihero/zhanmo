using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class GemBaseAttrRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public int Value { get; set; }
        public int LvUpCost { get; set; }
        public int LvUpCostGold { get; set; }
        public int SetAttrValue { get; set; }
        public GemBaseAttrRecord(DataRecord dataRecord)
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
            recordStrList.Add(TableWriteBase.GetWriteStr(LvUpCost));
            recordStrList.Add(TableWriteBase.GetWriteStr(LvUpCostGold));
            recordStrList.Add(TableWriteBase.GetWriteStr(SetAttrValue));

            return recordStrList.ToArray();
        }
    }

    public partial class GemBaseAttr : TableFileBase
    {
        public Dictionary<string, GemBaseAttrRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public GemBaseAttrRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("GemBaseAttr" + ": " + id, ex);
            }
        }

        public GemBaseAttr(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, GemBaseAttrRecord>();
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

                    GemBaseAttrRecord record = new GemBaseAttrRecord(data);
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
                pair.Value.LvUpCost = TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                pair.Value.LvUpCostGold = TableReadBase.ParseInt(pair.Value.ValueStr[5]);
                pair.Value.SetAttrValue = TableReadBase.ParseInt(pair.Value.ValueStr[6]);
            }
        }
    }

}