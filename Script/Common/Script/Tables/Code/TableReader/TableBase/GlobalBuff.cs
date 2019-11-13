using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class GlobalBuffRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public string Icon { get; set; }
        public GLOABL_BUFF_TYPE Type { get; set; }
        public SkillInfoRecord TelentID { get; set; }
        public AttrValueRecord ExAttr { get; set; }
        public AttrValueRecord ExAttrDiamond { get; set; }
        public int DiamondCost { get; set; }
        public int LastTime { get; set; }
        public GlobalBuffRecord(DataRecord dataRecord)
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
            recordStrList.Add(((int)Type).ToString());
            if (TelentID != null)
            {
                recordStrList.Add(TelentID.Id);
            }
            else
            {
                recordStrList.Add("");
            }
            if (ExAttr != null)
            {
                recordStrList.Add(ExAttr.Id);
            }
            else
            {
                recordStrList.Add("");
            }
            if (ExAttrDiamond != null)
            {
                recordStrList.Add(ExAttrDiamond.Id);
            }
            else
            {
                recordStrList.Add("");
            }
            recordStrList.Add(TableWriteBase.GetWriteStr(DiamondCost));
            recordStrList.Add(TableWriteBase.GetWriteStr(LastTime));

            return recordStrList.ToArray();
        }
    }

    public partial class GlobalBuff : TableFileBase
    {
        public Dictionary<string, GlobalBuffRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public GlobalBuffRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("GlobalBuff" + ": " + id, ex);
            }
        }

        public GlobalBuff(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, GlobalBuffRecord>();
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

                    GlobalBuffRecord record = new GlobalBuffRecord(data);
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
                pair.Value.Type =  (GLOABL_BUFF_TYPE)TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[5]))
                {
                    pair.Value.TelentID =  TableReader.SkillInfo.GetRecord(pair.Value.ValueStr[5]);
                }
                else
                {
                    pair.Value.TelentID = null;
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[6]))
                {
                    pair.Value.ExAttr =  TableReader.AttrValue.GetRecord(pair.Value.ValueStr[6]);
                }
                else
                {
                    pair.Value.ExAttr = null;
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[7]))
                {
                    pair.Value.ExAttrDiamond =  TableReader.AttrValue.GetRecord(pair.Value.ValueStr[7]);
                }
                else
                {
                    pair.Value.ExAttrDiamond = null;
                }
                pair.Value.DiamondCost = TableReadBase.ParseInt(pair.Value.ValueStr[8]);
                pair.Value.LastTime = TableReadBase.ParseInt(pair.Value.ValueStr[9]);
            }
        }
    }

}