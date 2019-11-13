using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;

namespace Tables
{
    public class TableRecordBase
    {
        public virtual string[] GetRecordStr()
        {
            return new string[0];
        }

        public virtual string Id { get; set; }
    }

    public class TableFileBase
    {
        public Dictionary<string, TableRecordBase> Records { get; set; }
    }

    public class MultiTable
    {
        public string TableName;
        public string ID;
    }

    public class TableReadBase
    {
        public static int ParseInt(string value)
        {
            return int.Parse(value);
        }

        public static float ParseFloat(string value)
        {
            return float.Parse(value);
        }

        public static string ParseString(string value)
        {
            return value;
        }

        public static bool ParseBool(string value)
        {
            return bool.Parse(value);
        }

        public static MultiTable ParseMultiTable(string value)
        {
            List<string> splitValues = new List<string>(value.Split(';'));
            if (splitValues.Count < 2)
            {
                throw new Exception("ParseMultiTable error:" + value);
            }

            MultiTable multiTable = new MultiTable();
            multiTable.TableName = splitValues[0];
            multiTable.ID = splitValues[1];

            return multiTable;
        }

        public static Vector3 ParseVector3(string value)
        {
            List<string> splitValues = new List<string>(value.Split(';'));
            if (splitValues.Count < 3)
            {
                throw new Exception("ParseVector3 error:" + value);
                //return Vector3.zero;
            }

            Vector3 vector = new Vector3();
            vector.x = ParseFloat(splitValues[0]);
            vector.y = ParseFloat(splitValues[1]);
            vector.z = ParseFloat(splitValues[2]);

            return vector;
        }

        public static string GetTableText(string tableName)
        {
            //var tableAsset = ResourceManager.Instance.GetTableStr(tableName);
            var tableAsset = ResourceManager.GetTable(tableName);
            if (!string.IsNullOrEmpty(tableAsset))
            {
                return tableAsset;
            }
            else
            {
                throw new Exception("GetTableText error:" + tableName);
            }

        }
    }
}
