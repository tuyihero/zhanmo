using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class FiveElementAttrRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public FIVE_ELEMENT ElementType { get; set; }
        public int LevelLimit { get; set; }
        public AttrValueRecord Attr { get; set; }
        public bool CanRepeat { get; set; }
        public int MinValue { get; set; }
        public int Random { get; set; }
        public FiveElementAttrRecord(DataRecord dataRecord)
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
            recordStrList.Add(((int)ElementType).ToString());
            recordStrList.Add(TableWriteBase.GetWriteStr(LevelLimit));
            if (Attr != null)
            {
                recordStrList.Add(Attr.Id);
            }
            else
            {
                recordStrList.Add("");
            }
            recordStrList.Add(TableWriteBase.GetWriteStr(CanRepeat));
            recordStrList.Add(TableWriteBase.GetWriteStr(MinValue));
            recordStrList.Add(TableWriteBase.GetWriteStr(Random));

            return recordStrList.ToArray();
        }
    }

    public partial class FiveElementAttr : TableFileBase
    {
        public Dictionary<string, FiveElementAttrRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public FiveElementAttrRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("FiveElementAttr" + ": " + id, ex);
            }
        }

        public FiveElementAttr(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, FiveElementAttrRecord>();
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

                    FiveElementAttrRecord record = new FiveElementAttrRecord(data);
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
                pair.Value.ElementType =  (FIVE_ELEMENT)TableReadBase.ParseInt(pair.Value.ValueStr[3]);
                pair.Value.LevelLimit = TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[5]))
                {
                    pair.Value.Attr =  TableReader.AttrValue.GetRecord(pair.Value.ValueStr[5]);
                }
                else
                {
                    pair.Value.Attr = null;
                }
                pair.Value.CanRepeat = TableReadBase.ParseBool(pair.Value.ValueStr[6]);
                pair.Value.MinValue = TableReadBase.ParseInt(pair.Value.ValueStr[7]);
                pair.Value.Random = TableReadBase.ParseInt(pair.Value.ValueStr[8]);
            }
        }
    }

}