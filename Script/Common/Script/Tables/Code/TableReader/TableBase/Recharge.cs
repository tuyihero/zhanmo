using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class RechargeRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public string Icon { get; set; }
        public int Num { get; set; }
        public int Price { get; set; }
        public string BundleName { get; set; }
        public RechargeRecord(DataRecord dataRecord)
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
            recordStrList.Add(TableWriteBase.GetWriteStr(Icon));
            recordStrList.Add(TableWriteBase.GetWriteStr(Num));
            recordStrList.Add(TableWriteBase.GetWriteStr(Price));
            recordStrList.Add(TableWriteBase.GetWriteStr(BundleName));

            return recordStrList.ToArray();
        }
    }

    public partial class Recharge : TableFileBase
    {
        public Dictionary<string, RechargeRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public RechargeRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("Recharge" + ": " + id, ex);
            }
        }

        public Recharge(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, RechargeRecord>();
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

                    RechargeRecord record = new RechargeRecord(data);
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
                pair.Value.Icon = TableReadBase.ParseString(pair.Value.ValueStr[3]);
                pair.Value.Num = TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                pair.Value.Price = TableReadBase.ParseInt(pair.Value.ValueStr[5]);
                pair.Value.BundleName = TableReadBase.ParseString(pair.Value.ValueStr[6]);
            }
        }
    }

}