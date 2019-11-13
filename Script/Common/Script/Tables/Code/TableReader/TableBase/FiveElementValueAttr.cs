using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class FiveElementValueAttrRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public int Value { get; set; }
        public AttrValueRecord Attr { get; set; }
        public int DescIdx { get; set; }
        public FiveElementValueAttrRecord(DataRecord dataRecord)
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
            if (Attr != null)
            {
                recordStrList.Add(Attr.Id);
            }
            else
            {
                recordStrList.Add("");
            }
            recordStrList.Add(TableWriteBase.GetWriteStr(DescIdx));

            return recordStrList.ToArray();
        }
    }

    public partial class FiveElementValueAttr : TableFileBase
    {
        public Dictionary<string, FiveElementValueAttrRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public FiveElementValueAttrRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("FiveElementValueAttr" + ": " + id, ex);
            }
        }

        public FiveElementValueAttr(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, FiveElementValueAttrRecord>();
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

                    FiveElementValueAttrRecord record = new FiveElementValueAttrRecord(data);
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
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[4]))
                {
                    pair.Value.Attr =  TableReader.AttrValue.GetRecord(pair.Value.ValueStr[4]);
                }
                else
                {
                    pair.Value.Attr = null;
                }
                pair.Value.DescIdx = TableReadBase.ParseInt(pair.Value.ValueStr[5]);
            }
        }
    }

}