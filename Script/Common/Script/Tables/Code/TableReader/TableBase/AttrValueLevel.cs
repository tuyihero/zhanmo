using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class AttrValueLevelRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public List<int> Values { get; set; }
        public AttrValueLevelRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            Values = new List<int>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            foreach (var testTableItem in Values)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }

            return recordStrList.ToArray();
        }
    }

    public partial class AttrValueLevel : TableFileBase
    {
        public Dictionary<string, AttrValueLevelRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public AttrValueLevelRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("AttrValueLevel" + ": " + id, ex);
            }
        }

        public AttrValueLevel(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, AttrValueLevelRecord>();
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

                    AttrValueLevelRecord record = new AttrValueLevelRecord(data);
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
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[3]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[4]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[5]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[6]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[7]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[8]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[9]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[10]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[11]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[12]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[13]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[14]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[15]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[16]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[17]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[18]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[19]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[20]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[21]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[22]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[23]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[24]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[25]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[26]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[27]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[28]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[29]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[30]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[31]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[32]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[33]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[34]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[35]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[36]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[37]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[38]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[39]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[40]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[41]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[42]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[43]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[44]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[45]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[46]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[47]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[48]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[49]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[50]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[51]));
                pair.Value.Values.Add(TableReadBase.ParseInt(pair.Value.ValueStr[52]));
            }
        }
    }

}