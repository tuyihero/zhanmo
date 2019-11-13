using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class GiftPacketRecord  : TableRecordBase
    {
        public DataRecord ValueStr;

        public override string Id { get; set; }        public string Name { get; set; }
        public string Desc { get; set; }
        public int GroupID { get; set; }
        public int PacketType { get; set; }
        public List<CommonItemRecord> Item { get; set; }
        public List<int> ItemNum { get; set; }
        public int Gold { get; set; }
        public int Diamond { get; set; }
        public int Price { get; set; }
        public int LevelMin { get; set; }
        public int LevelMax { get; set; }
        public int Rate { get; set; }
        public string ActScript { get; set; }
        public List<string> ActScriptParam { get; set; }
        public GiftPacketRecord(DataRecord dataRecord)
        {
            if (dataRecord != null)
            {
                ValueStr = dataRecord;
                Id = ValueStr[0];

            }
            Item = new List<CommonItemRecord>();
            ItemNum = new List<int>();
            ActScriptParam = new List<string>();
        }
        public override string[] GetRecordStr()
        {
            List<string> recordStrList = new List<string>();
            recordStrList.Add(TableWriteBase.GetWriteStr(Id));
            recordStrList.Add(TableWriteBase.GetWriteStr(Name));
            recordStrList.Add(TableWriteBase.GetWriteStr(Desc));
            recordStrList.Add(TableWriteBase.GetWriteStr(GroupID));
            recordStrList.Add(TableWriteBase.GetWriteStr(PacketType));
            foreach (var testTableItem in Item)
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
            foreach (var testTableItem in ItemNum)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }
            recordStrList.Add(TableWriteBase.GetWriteStr(Gold));
            recordStrList.Add(TableWriteBase.GetWriteStr(Diamond));
            recordStrList.Add(TableWriteBase.GetWriteStr(Price));
            recordStrList.Add(TableWriteBase.GetWriteStr(LevelMin));
            recordStrList.Add(TableWriteBase.GetWriteStr(LevelMax));
            recordStrList.Add(TableWriteBase.GetWriteStr(Rate));
            recordStrList.Add(TableWriteBase.GetWriteStr(ActScript));
            foreach (var testTableItem in ActScriptParam)
            {
                recordStrList.Add(TableWriteBase.GetWriteStr(testTableItem));
            }

            return recordStrList.ToArray();
        }
    }

    public partial class GiftPacket : TableFileBase
    {
        public Dictionary<string, GiftPacketRecord> Records { get; internal set; }

        public bool ContainsKey(string key)
        {
             return Records.ContainsKey(key);
        }

        public GiftPacketRecord GetRecord(string id)
        {
            try
            {
                return Records[id];
            }
            catch (Exception ex)
            {
                throw new Exception("GiftPacket" + ": " + id, ex);
            }
        }

        public GiftPacket(string pathOrContent,bool isPath = true)
        {
            Records = new Dictionary<string, GiftPacketRecord>();
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

                    GiftPacketRecord record = new GiftPacketRecord(data);
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
                pair.Value.GroupID = TableReadBase.ParseInt(pair.Value.ValueStr[3]);
                pair.Value.PacketType = TableReadBase.ParseInt(pair.Value.ValueStr[4]);
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[5]))
                {
                    pair.Value.Item.Add( TableReader.CommonItem.GetRecord(pair.Value.ValueStr[5]));
                }
                else
                {
                    pair.Value.Item.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[6]))
                {
                    pair.Value.Item.Add( TableReader.CommonItem.GetRecord(pair.Value.ValueStr[6]));
                }
                else
                {
                    pair.Value.Item.Add(null);
                }
                if (!string.IsNullOrEmpty(pair.Value.ValueStr[7]))
                {
                    pair.Value.Item.Add( TableReader.CommonItem.GetRecord(pair.Value.ValueStr[7]));
                }
                else
                {
                    pair.Value.Item.Add(null);
                }
                pair.Value.ItemNum.Add(TableReadBase.ParseInt(pair.Value.ValueStr[8]));
                pair.Value.ItemNum.Add(TableReadBase.ParseInt(pair.Value.ValueStr[9]));
                pair.Value.ItemNum.Add(TableReadBase.ParseInt(pair.Value.ValueStr[10]));
                pair.Value.Gold = TableReadBase.ParseInt(pair.Value.ValueStr[11]);
                pair.Value.Diamond = TableReadBase.ParseInt(pair.Value.ValueStr[12]);
                pair.Value.Price = TableReadBase.ParseInt(pair.Value.ValueStr[13]);
                pair.Value.LevelMin = TableReadBase.ParseInt(pair.Value.ValueStr[14]);
                pair.Value.LevelMax = TableReadBase.ParseInt(pair.Value.ValueStr[15]);
                pair.Value.Rate = TableReadBase.ParseInt(pair.Value.ValueStr[16]);
                pair.Value.ActScript = TableReadBase.ParseString(pair.Value.ValueStr[17]);
                pair.Value.ActScriptParam.Add(TableReadBase.ParseString(pair.Value.ValueStr[18]));
                pair.Value.ActScriptParam.Add(TableReadBase.ParseString(pair.Value.ValueStr[19]));
                pair.Value.ActScriptParam.Add(TableReadBase.ParseString(pair.Value.ValueStr[20]));
            }
        }
    }

}