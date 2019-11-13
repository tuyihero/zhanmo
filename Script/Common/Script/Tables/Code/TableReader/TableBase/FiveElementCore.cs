using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class FiveElementCoreRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public FIVE_ELEMENT ElementType { get; set; }
        public ITEM_QUALITY Quality { get; set; }
        public List<int> PosAttrLimit { get; set; }
        public List<int> PosCondition { get; set; }
        public List<FiveElementCoreAttrRecord> CoreAttr { get; set; }
        public FiveElementCoreRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            PosAttrLimit = new List<int>();
            PosCondition = new List<int>();
            CoreAttr = new List<FiveElementCoreAttrRecord>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(((int)ElementType).ToString());
            recordStrList.Add(((int)Quality).ToString());
            foreach (var testTableItem in PosAttrLimit)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in PosCondition)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in CoreAttr)
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

    public partial class FiveElementCore : TableFileBase
    {
        public Dictionary<string, FiveElementCoreRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public FiveElementCoreRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("FiveElementCore" + ": " + id, ex);
            }
        }

        public FiveElementCore(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, FiveElementCoreRecord>();
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

                    FiveElementCoreRecord record = new FiveElementCoreRecord(data);
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
                pair.Value.Quality =  (ITEM_QUALITY)TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                pair.Value.PosAttrLimit.Add(TableReadBase.ParseInt(pair.Value.ValueStr[5]));
                pair.Value.PosAttrLimit.Add(TableReadBase.ParseInt(pair.Value.ValueStr[6]));
                pair.Value.PosAttrLimit.Add(TableReadBase.ParseInt(pair.Value.ValueStr[7]));
                pair.Value.PosAttrLimit.Add(TableReadBase.ParseInt(pair.Value.ValueStr[8]));
                pair.Value.PosAttrLimit.Add(TableReadBase.ParseInt(pair.Value.ValueStr[9]));
                pair.Value.PosAttrLimit.Add(TableReadBase.ParseInt(pair.Value.ValueStr[10]));
                pair.Value.PosCondition.Add(TableReadBase.ParseInt(pair.Value.ValueStr[11]));
                pair.Value.PosCondition.Add(TableReadBase.ParseInt(pair.Value.ValueStr[12]));
                pair.Value.PosCondition.Add(TableReadBase.ParseInt(pair.Value.ValueStr[13]));
                pair.Value.PosCondition.Add(TableReadBase.ParseInt(pair.Value.ValueStr[14]));
                pair.Value.PosCondition.Add(TableReadBase.ParseInt(pair.Value.ValueStr[15]));
                pair.Value.PosCondition.Add(TableReadBase.ParseInt(pair.Value.ValueStr[16]));
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[17]))
                {
                    pair.Value.CoreAttr.Add( TableReader.FiveElementCoreAttr.GetRecord(pair.Value.ValueStr[17]));
                }
                else
                {
                    pair.Value.CoreAttr.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[18]))
                {
                    pair.Value.CoreAttr.Add( TableReader.FiveElementCoreAttr.GetRecord(pair.Value.ValueStr[18]));
                }
                else
                {
                    pair.Value.CoreAttr.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[19]))
                {
                    pair.Value.CoreAttr.Add( TableReader.FiveElementCoreAttr.GetRecord(pair.Value.ValueStr[19]));
                }
                else
                {
                    pair.Value.CoreAttr.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[20]))
                {
                    pair.Value.CoreAttr.Add( TableReader.FiveElementCoreAttr.GetRecord(pair.Value.ValueStr[20]));
                }
                else
                {
                    pair.Value.CoreAttr.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[21]))
                {
                    pair.Value.CoreAttr.Add( TableReader.FiveElementCoreAttr.GetRecord(pair.Value.ValueStr[21]));
                }
                else
                {
                    pair.Value.CoreAttr.Add(null);
                }
            }
        }
    }

}