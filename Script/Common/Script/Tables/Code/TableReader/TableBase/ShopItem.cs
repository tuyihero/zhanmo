using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class ShopItemRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public int PriceBuy { get; set; }
        public int MoneyType { get; set; }
        public int DailyLimit { get; set; }
        public bool MutiBuy { get; set; }
        public string Class { get; set; }
        public int ClassItemCnt { get; set; }
        public int Prior { get; set; }
        public string Script { get; set; }
        public List<int> ScriptParam { get; set; }
        public string ActScript { get; set; }
        public List<string> ActScriptParam { get; set; }
        public ShopItemRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            ScriptParam = new List<int>();
            ActScriptParam = new List<string>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(PriceBuy));
            recordStrList.Add(TableWriteBase.GetWriteStr(MoneyType));
            recordStrList.Add(TableWriteBase.GetWriteStr(DailyLimit));
            recordStrList.Add(TableWriteBase.GetWriteStr(MutiBuy));
            recordStrList.Add(TableWriteBase.GetWriteStr(Class));
            recordStrList.Add(TableWriteBase.GetWriteStr(ClassItemCnt));
            recordStrList.Add(TableWriteBase.GetWriteStr(Prior));
            recordStrList.Add(TableWriteBase.GetWriteStr(Script));
            foreach (var testTableItem in ScriptParam)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            recordStrList.Add(TableWriteBase.GetWriteStr(ActScript));
            foreach (var testTableItem in ActScriptParam)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }

            return recordStrList.ToArray();
        }
    }

    public partial class ShopItem : TableFileBase
    {
        public Dictionary<string, ShopItemRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public ShopItemRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("ShopItem" + ": " + id, ex);
            }
        }

        public ShopItem(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, ShopItemRecord>();
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

                    ShopItemRecord record = new ShopItemRecord(data);
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
                pair.Value.PriceBuy = TableReadBase.ParseInt(pair.Value.ValueStr[3]);
                pair.Value.MoneyType = TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                pair.Value.DailyLimit = TableReadBase.ParseInt(pair.Value.ValueStr[5]);
                pair.Value.MutiBuy = TableReadBase.ParseBool(pair.Value.ValueStr[6]);
                pair.Value.Class = TableReadBase.ParseString(pair.Value.ValueStr[7]);
                pair.Value.ClassItemCnt = TableReadBase.ParseInt(pair.Value.ValueStr[8]);
                pair.Value.Prior = TableReadBase.ParseInt(pair.Value.ValueStr[9]);
                pair.Value.Script = TableReadBase.ParseString(pair.Value.ValueStr[10]);
                pair.Value.ScriptParam.Add(TableReadBase.ParseInt(pair.Value.ValueStr[11]));
                pair.Value.ScriptParam.Add(TableReadBase.ParseInt(pair.Value.ValueStr[12]));
                pair.Value.ScriptParam.Add(TableReadBase.ParseInt(pair.Value.ValueStr[13]));
                pair.Value.ActScript = TableReadBase.ParseString(pair.Value.ValueStr[14]);
                pair.Value.ActScriptParam.Add(TableReadBase.ParseString(pair.Value.ValueStr[15]));
                pair.Value.ActScriptParam.Add(TableReadBase.ParseString(pair.Value.ValueStr[16]));
                pair.Value.ActScriptParam.Add(TableReadBase.ParseString(pair.Value.ValueStr[17]));
            }
        }
    }

}