using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class AttrValueRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public string AttrImpact { get; set; }
        public List<string> StrParam { get; set; }
        public List<int> AttrParams { get; set; }
        public AttrValueRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            StrParam = new List<string>();
            AttrParams = new List<int>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(AttrImpact));
            foreach (var testTableItem in StrParam)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in AttrParams)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }

            return recordStrList.ToArray();
        }
    }

    public partial class AttrValue : TableFileBase
    {
        public Dictionary<string, AttrValueRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public AttrValueRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("AttrValue" + ": " + id, ex);
            }
        }

        public AttrValue(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, AttrValueRecord>();
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

                    AttrValueRecord record = new AttrValueRecord(data);
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
                pair.Value.AttrImpact = TableReadBase.ParseString(pair.Value.ValueStr[3]);
                pair.Value.StrParam.Add(TableReadBase.ParseString(pair.Value.ValueStr[4]));
                pair.Value.StrParam.Add(TableReadBase.ParseString(pair.Value.ValueStr[5]));
                pair.Value.StrParam.Add(TableReadBase.ParseString(pair.Value.ValueStr[6]));
                pair.Value.AttrParams.Add(TableReadBase.ParseInt(pair.Value.ValueStr[7]));
                pair.Value.AttrParams.Add(TableReadBase.ParseInt(pair.Value.ValueStr[8]));
                pair.Value.AttrParams.Add(TableReadBase.ParseInt(pair.Value.ValueStr[9]));
                pair.Value.AttrParams.Add(TableReadBase.ParseInt(pair.Value.ValueStr[10]));
                pair.Value.AttrParams.Add(TableReadBase.ParseInt(pair.Value.ValueStr[11]));
                pair.Value.AttrParams.Add(TableReadBase.ParseInt(pair.Value.ValueStr[12]));
                pair.Value.AttrParams.Add(TableReadBase.ParseInt(pair.Value.ValueStr[13]));
                pair.Value.AttrParams.Add(TableReadBase.ParseInt(pair.Value.ValueStr[14]));
                pair.Value.AttrParams.Add(TableReadBase.ParseInt(pair.Value.ValueStr[15]));
                pair.Value.AttrParams.Add(TableReadBase.ParseInt(pair.Value.ValueStr[16]));
            }
        }
    }

}