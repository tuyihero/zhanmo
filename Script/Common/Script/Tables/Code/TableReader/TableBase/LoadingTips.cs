﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class LoadingTipsRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        public string TipsStr { get; set; }
        public int MinLevel { get; set; }
        public LoadingTipsRecord(DataRecord dataRecord)
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
            recordStrList.Add(TableWriteBase.GetWriteStr(ImagePath));
            recordStrList.Add(TableWriteBase.GetWriteStr(ImageName));
            recordStrList.Add(TableWriteBase.GetWriteStr(TipsStr));
            recordStrList.Add(TableWriteBase.GetWriteStr(MinLevel));

            return recordStrList.ToArray();
        }
    }

    public partial class LoadingTips : TableFileBase
    {
        public Dictionary<string, LoadingTipsRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public LoadingTipsRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("LoadingTips" + ": " + id, ex);
            }
        }

        public LoadingTips(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, LoadingTipsRecord>();
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

                    LoadingTipsRecord record = new LoadingTipsRecord(data);
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
                pair.Value.ImagePath = TableReadBase.ParseString(pair.Value.ValueStr[3]);
                pair.Value.ImageName = TableReadBase.ParseString(pair.Value.ValueStr[4]);
                pair.Value.TipsStr = TableReadBase.ParseString(pair.Value.ValueStr[5]);
                pair.Value.MinLevel = TableReadBase.ParseInt(pair.Value.ValueStr[6]);
            }
        }
    }

}