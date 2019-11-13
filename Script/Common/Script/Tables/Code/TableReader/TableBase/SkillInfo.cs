using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class SkillInfoRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public int NameStrDict { get; set; }
        public int DescStrDict { get; set; }
        public string Icon { get; set; }
        public int Profession { get; set; }
        public string SkillInput { get; set; }
        public string SkillType { get; set; }
        public AttrValueRecord SkillAttr { get; set; }
        public int StartRoleLevel { get; set; }
        public int StartPreSkill { get; set; }
        public int StartPreSkillLv { get; set; }
        public int NextLvInterval { get; set; }
        public int MaxLevel { get; set; }
        public List<int> EffectValue { get; set; }
        public List<int> CostStep { get; set; }
        public int Pos { get; set; }
        public SkillInfoRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            EffectValue = new List<int>();
            CostStep = new List<int>();
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
            recordStrList.Add(TableWriteBase.GetWriteStr(Profession));
            recordStrList.Add(TableWriteBase.GetWriteStr(SkillInput));
            recordStrList.Add(TableWriteBase.GetWriteStr(SkillType));
            if (SkillAttr != null)
            {
                recordStrList.Add(SkillAttr.Id);
            }
            else
            {
                recordStrList.Add("");
            }
            recordStrList.Add(TableWriteBase.GetWriteStr(StartRoleLevel));
            recordStrList.Add(TableWriteBase.GetWriteStr(StartPreSkill));
            recordStrList.Add(TableWriteBase.GetWriteStr(StartPreSkillLv));
            recordStrList.Add(TableWriteBase.GetWriteStr(NextLvInterval));
            recordStrList.Add(TableWriteBase.GetWriteStr(MaxLevel));
            foreach (var testTableItem in EffectValue)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            foreach (var testTableItem in CostStep)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            recordStrList.Add(TableWriteBase.GetWriteStr(Pos));

            return recordStrList.ToArray();
        }
    }

    public partial class SkillInfo : TableFileBase
    {
        public Dictionary<string, SkillInfoRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public SkillInfoRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("SkillInfo" + ": " + id, ex);
            }
        }

        public SkillInfo(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, SkillInfoRecord>();
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

                    SkillInfoRecord record = new SkillInfoRecord(data);
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
                pair.Value.Profession = TableReadBase.ParseInt(pair.Value.ValueStr[6]);
                pair.Value.SkillInput = TableReadBase.ParseString(pair.Value.ValueStr[7]);
                pair.Value.SkillType = TableReadBase.ParseString(pair.Value.ValueStr[8]);
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[9]))
                {
                    pair.Value.SkillAttr =  TableReader.AttrValue.GetRecord(pair.Value.ValueStr[9]);
                }
                else
                {
                    pair.Value.SkillAttr = null;
                }
                pair.Value.StartRoleLevel = TableReadBase.ParseInt(pair.Value.ValueStr[10]);
                pair.Value.StartPreSkill = TableReadBase.ParseInt(pair.Value.ValueStr[11]);
                pair.Value.StartPreSkillLv = TableReadBase.ParseInt(pair.Value.ValueStr[12]);
                pair.Value.NextLvInterval = TableReadBase.ParseInt(pair.Value.ValueStr[13]);
                pair.Value.MaxLevel = TableReadBase.ParseInt(pair.Value.ValueStr[14]);
                pair.Value.EffectValue.Add(TableReadBase.ParseInt(pair.Value.ValueStr[15]));
                pair.Value.EffectValue.Add(TableReadBase.ParseInt(pair.Value.ValueStr[16]));
                pair.Value.EffectValue.Add(TableReadBase.ParseInt(pair.Value.ValueStr[17]));
                pair.Value.CostStep.Add(TableReadBase.ParseInt(pair.Value.ValueStr[18]));
                pair.Value.CostStep.Add(TableReadBase.ParseInt(pair.Value.ValueStr[19]));
                pair.Value.CostStep.Add(TableReadBase.ParseInt(pair.Value.ValueStr[20]));
                pair.Value.Pos = TableReadBase.ParseInt(pair.Value.ValueStr[21]);
            }
        }
    }

}