using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

namespace Tables
{
    public class TableReadEx
    {
        public static string GetTablePropty(string tableName, string recordID, string proName)
        {
            var tableRecord = GetTableRecord(tableName, recordID);

            var proField = tableRecord.GetType().GetProperty(proName, BindingFlags.Instance | BindingFlags.Public);

            return proField.GetValue(tableRecord, null).ToString();
        }

        public static object GetTableRecord(string tableName, string recordID)
        {
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(recordID))
                return null;

            var tableReadType = typeof(TableReader);

            var tableField = tableReadType.GetProperty(tableName, BindingFlags.Static | BindingFlags.Public);

            var tableValue = tableField.GetValue(null, null);

            var getRecordFun = tableValue.GetType().GetMethod("GetRecord");

            var tableRecord = getRecordFun.Invoke(tableValue, new object[] { recordID });

            return tableRecord;
        }
    }
}
