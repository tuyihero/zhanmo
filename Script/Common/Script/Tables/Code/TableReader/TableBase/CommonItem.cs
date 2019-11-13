using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class CommonItemRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public int NameStrDict { get; set; }
        public int DescStrDict { get; set; }
        public string Icon { get; set; }
        public string Model { get; set; }
        public ITEM_QUALITY Quality { get; set; }
        public string DropItem { get; set; }
        public float DropScale { get; set; }
        public int StackNum { get; set; }
        public CommonItemRecord(DataRecord dataRecord)
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
            recordStrList.Add(TableWriteBase.GetWriteStr(NameStrDict));
            recordStrList.Add(TableWriteBase.GetWriteStr(DescStrDict));
            recordStrList.Add(TableWriteBase.GetWriteStr(Icon));
            recordStrList.Add(TableWriteBase.GetWriteStr(Model));
            recordStrList.Add(((int)Quality).ToString());
            recordStrList.Add(TableWriteBase.GetWriteStr(DropItem));
            recordStrList.Add(TableWriteBase.GetWriteStr(DropScale));
            recordStrList.Add(TableWriteBase.GetWriteStr(StackNum));

            return recordStrList.ToArray();
        }
    }

    public partial class CommonItem : TableFileBase
    {
        public Dictionary<string, CommonItemRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public CommonItemRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("CommonItem" + ": " + id, ex);
            }
        }

        public CommonItem(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, CommonItemRecord>();
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

                    CommonItemRecord record = new CommonItemRecord(data);
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
                pair.Value.NameStrDict = TableReadBase.ParseInt(pair.Value.ValueStr[3]);
                pair.Value.DescStrDict = TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                pair.Value.Icon = TableReadBase.ParseString(pair.Value.ValueStr[5]);
                pair.Value.Model = TableReadBase.ParseString(pair.Value.ValueStr[6]);
                pair.Value.Quality =  (ITEM_QUALITY)TableReadBase.ParseInt(pair.Value.ValueStr[7]);
                pair.Value.DropItem = TableReadBase.ParseString(pair.Value.ValueStr[8]);
                pair.Value.DropScale = TableReadBase.ParseFloat(pair.Value.ValueStr[9]);
                pair.Value.StackNum = TableReadBase.ParseInt(pair.Value.ValueStr[10]);
            }
        }
    }

}