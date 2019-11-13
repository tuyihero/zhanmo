using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class GemSetRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public bool IsEnableDefault { get; set; }
        public int MinGemLv { get; set; }
        public List<int> Gems { get; set; }
        public List<AttrValueRecord> Attrs { get; set; }
        public GemSetRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            Gems = new List<int>();
            Attrs = new List<AttrValueRecord>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(IsEnableDefault));
            recordStrList.Add(TableWriteBase.GetWriteStr(MinGemLv));
            foreach (var testTableItem in Gems)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in Attrs)
            {
                if (testTableItem != null)
                {
                    recordStrList.Add(testTableItem.Id);
                }
                else
                {
                    recordStrList.Add("");
                }
            }

            return recordStrList.ToArray();
        }
    }

    public partial class GemSet : TableFileBase
    {
        public Dictionary<string, GemSetRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public GemSetRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("GemSet" + ": " + id, ex);
            }
        }

        public GemSet(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, GemSetRecord>();
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

                    GemSetRecord record = new GemSetRecord(data);
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
                pair.Value.IsEnableDefault = TableReadBase.ParseBool(pair.Value.ValueStr[3]);
                pair.Value.MinGemLv = TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                pair.Value.Gems.Add(TableReadBase.ParseInt(pair.Value.ValueStr[5]));
                pair.Value.Gems.Add(TableReadBase.ParseInt(pair.Value.ValueStr[6]));
                pair.Value.Gems.Add(TableReadBase.ParseInt(pair.Value.ValueStr[7]));
                pair.Value.Gems.Add(TableReadBase.ParseInt(pair.Value.ValueStr[8]));
                pair.Value.Gems.Add(TableReadBase.ParseInt(pair.Value.ValueStr[9]));
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[10]))
                {
                    pair.Value.Attrs.Add( TableReader.AttrValue.GetRecord(pair.Value.ValueStr[10]));
                }
                else
                {
                    pair.Value.Attrs.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[11]))
                {
                    pair.Value.Attrs.Add( TableReader.AttrValue.GetRecord(pair.Value.ValueStr[11]));
                }
                else
                {
                    pair.Value.Attrs.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[12]))
                {
                    pair.Value.Attrs.Add( TableReader.AttrValue.GetRecord(pair.Value.ValueStr[12]));
                }
                else
                {
                    pair.Value.Attrs.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[13]))
                {
                    pair.Value.Attrs.Add( TableReader.AttrValue.GetRecord(pair.Value.ValueStr[13]));
                }
                else
                {
                    pair.Value.Attrs.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[14]))
                {
                    pair.Value.Attrs.Add( TableReader.AttrValue.GetRecord(pair.Value.ValueStr[14]));
                }
                else
                {
                    pair.Value.Attrs.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[15]))
                {
                    pair.Value.Attrs.Add( TableReader.AttrValue.GetRecord(pair.Value.ValueStr[15]));
                }
                else
                {
                    pair.Value.Attrs.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[16]))
                {
                    pair.Value.Attrs.Add( TableReader.AttrValue.GetRecord(pair.Value.ValueStr[16]));
                }
                else
                {
                    pair.Value.Attrs.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[17]))
                {
                    pair.Value.Attrs.Add( TableReader.AttrValue.GetRecord(pair.Value.ValueStr[17]));
                }
                else
                {
                    pair.Value.Attrs.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[18]))
                {
                    pair.Value.Attrs.Add( TableReader.AttrValue.GetRecord(pair.Value.ValueStr[18]));
                }
                else
                {
                    pair.Value.Attrs.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[19]))
                {
                    pair.Value.Attrs.Add( TableReader.AttrValue.GetRecord(pair.Value.ValueStr[19]));
                }
                else
                {
                    pair.Value.Attrs.Add(null);
                }
            }
        }
    }

}