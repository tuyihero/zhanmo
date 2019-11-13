using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

 
using Tables;
 

 

    public class SaveRoot
    {
        public List<SaveNode> _ChildNode = new List<SaveNode>();
        public string _SaveKey;

    }

    public class SaveNode
    {

        public List<SaveNode> _ChildNode = new List<SaveNode>();

        public int _SaveIDX;
        public string _SaveValue;
        public string _SaveType;
        public FieldInfo _SaveField;

        public string ToSaveString(int depth)
        {
            string saveStr = "";
            if (_ChildNode.Count > 0)
            {
                foreach (var child in _ChildNode)
                {
                    saveStr += DataPackSave.SaveSplitChars[depth] + child.ToSaveString(depth + 1);
                }
                saveStr = saveStr.Substring(1);
            }
            else
            {
                saveStr = _SaveValue;
            }
            return saveStr;
        }
    }

public class DataPackSave
{
    public static char[] SaveSplitChars = new char[] { ';', ',', '~', '!', '@', '#', '$', '%', '^', '&', '*', '|' };

    #region save
    public static void SaveData(SaveItemBase dataObj, bool isSaveChild = true)
    {
        var dataType = dataObj.GetType();

        List<SaveItemBase> saveChildItems = new List<SaveItemBase>();

        SaveRoot saveRoot = new SaveRoot();
        saveRoot._SaveKey = dataObj._SaveFileName;
        if (string.IsNullOrEmpty(dataObj._SaveFileName))
        {
            Debug.LogError("SaveKey is Empty:" + dataObj.ToString());
        }
        saveRoot._ChildNode = GetNodesForSave(dataObj, saveChildItems);
        SaveRoot(saveRoot);

        if (isSaveChild)
        {
            while (saveChildItems.Count > 0)
            {
                var childItem = saveChildItems[0];
                saveChildItems.RemoveAt(0);

                SaveRoot saveChild = new SaveRoot();
                saveChild._SaveKey = childItem._SaveFileName;
                saveChild._ChildNode = GetNodesForSave(childItem, saveChildItems);
                SaveRoot(saveChild);
            }
        }
    }

    public static List<SaveNode> GetNodesForSave(object dataObj, List<SaveItemBase> saveChilds = null)
    {
        var dataType = dataObj.GetType();
        List<SaveNode> saveNodes = new List<SaveNode>();

        List<FieldInfo> fieldList = new List<FieldInfo>();

        var fields = dataType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
        fieldList.AddRange(fields);


        foreach (var fieldInfo in fieldList)
        {
            SaveField[] attributes = (SaveField[])fieldInfo.GetCustomAttributes(typeof(SaveField), false);
            if (attributes != null && attributes.Length > 0)
            {
                SaveNode baseNode = new SaveNode();
                baseNode._SaveIDX = attributes[0]._SaveIDX;
                saveNodes.Add(baseNode);

                if (IsBaseType(fieldInfo.FieldType))
                {
                    baseNode._SaveType = fieldInfo.FieldType.Name;
                    baseNode._SaveValue = fieldInfo.GetValue(dataObj).ToString();
                }
                else if (fieldInfo.FieldType.Name.Contains("List"))
                {
                    baseNode._SaveType = fieldInfo.FieldType.ToString();
                    var valueList = fieldInfo.GetValue(dataObj) as IList;

                    int idx1 = baseNode._SaveType.IndexOf('[');
                    int idx2 = baseNode._SaveType.IndexOf(']');
                    string childTypeName = baseNode._SaveType.Substring(idx1 + 1, idx2 - idx1 - 1);

                    Type childType = null;
                    if (valueList == null)
                        continue;

                    //Debug.Log("SaveList cnt:" + valueList.Count);
                    for (int j = 0; j < valueList.Count; ++j)
                    {
                        if (childType == null)
                        {
                            childType = Type.GetType(childTypeName);
                        }

                        SaveNode childNode = new SaveNode();
                        childNode._SaveIDX = j;
                        childNode._SaveType = childType.Name;

                        baseNode._ChildNode.Add(childNode);

                        if (IsBaseType(childType))
                        {
                            childNode._SaveValue = valueList[j].ToString();
                        }
                        else if (valueList[j] is SaveItemBase)
                        {
                            var saveItem = (SaveItemBase)valueList[j];
                            if (dataObj is SaveItemBase)
                            {
                                saveItem._SaveFileName = ((SaveItemBase)dataObj)._SaveFileName + fieldInfo.Name + j.ToString();
                            }
                            else
                            {
                                saveItem._SaveFileName = dataObj.ToString() + fieldInfo.Name + j.ToString();
                            }
                            saveChilds.Add(saveItem);
                            childNode._SaveValue = saveItem._SaveFileName;
                        }
                        else
                        {
                            childNode._ChildNode = GetNodesForSave(valueList[j]);
                        }
                    }

                }
                else if (fieldInfo.FieldType.IsEnum)
                {
                    baseNode._SaveType = fieldInfo.FieldType.Name;
                    baseNode._SaveValue = ((int)fieldInfo.GetValue(dataObj)).ToString();
                }
                else if (fieldInfo.FieldType.BaseType == typeof(Tables.TableRecordBase))
                {
                    var record = fieldInfo.GetValue(dataObj) as TableRecordBase;

                    if (record != null)
                    {
                        baseNode._SaveType = typeof(string).Name;
                        baseNode._SaveValue = record.Id;
                    }
                }
                else if (fieldInfo.GetValue(dataObj) is SaveItemBase)
                {
                    baseNode._SaveType = fieldInfo.FieldType.Name;
                    var saveItem = (SaveItemBase)fieldInfo.GetValue(dataObj);
                    saveItem._SaveFileName = dataObj.ToString() + fieldInfo.Name;
                    saveChilds.Add(saveItem);
                    baseNode._SaveValue = saveItem._SaveFileName;
                }
                else
                {
                    baseNode._SaveType = fieldInfo.FieldType.Name;
                    var dataValue = fieldInfo.GetValue(dataObj);
                    if (dataValue != null)
                        baseNode._ChildNode = GetNodesForSave(dataValue);
                }
            }
        }
        saveNodes.Sort((node1, node2) =>
        {
            if (node1._SaveIDX > node2._SaveIDX)
                return 1;
            else if (node1._SaveIDX < node2._SaveIDX)
                return -1;
            return 0;
        });
        return saveNodes;
    }

    public static void SaveRoot(SaveRoot root)
    {
        string saveStr = "";

        foreach (var child in root._ChildNode)
        {
            saveStr += DataPackSave.SaveSplitChars[0] + child.ToSaveString(1);
        }
        saveStr = saveStr.Substring(1);
        //Debug.Log("Save:" + root._SaveKey + "," + saveStr);
        LocalSave.Save(root._SaveKey, saveStr);

    }
    
    #endregion

    #region load

    public static void LoadData(SaveItemBase dataObj, bool isLoadChild)
    {
        var dataType = dataObj.GetType();

        string loadStr = LocalSave.Load(dataObj._SaveFileName);

        LoadNodes(dataObj, 0, loadStr, isLoadChild);
    }

    public static void LoadNodes(object dataObj, int depth, string dataStr, bool isLoadChild)
    {
        var dataType = dataObj.GetType();
        List<SaveNode> saveNodes = new List<SaveNode>();

        List<FieldInfo> fieldList = new List<FieldInfo>();
        //if (fieldNames == null || fieldNames.Count == 0)
        {
            var fields = dataType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            fieldList.AddRange(fields);
        }


        Dictionary<int, FieldInfo> loadFields = new Dictionary<int, FieldInfo>();
        List<FieldInfo> sortedFields = new List<FieldInfo>();
        foreach (var fieldInfo in fieldList)
        {
            SaveField[] attributes = (SaveField[])fieldInfo.GetCustomAttributes(typeof(SaveField), false);
            if (attributes != null && attributes.Length > 0)
            {
                loadFields.Add(attributes[0]._SaveIDX - 1, fieldInfo);
            }
        }
        for (int i = 0; i < loadFields.Count; ++i)
        {
            sortedFields.Add(loadFields[i]);
        }

        string[] valueStrs = dataStr.Split(SaveSplitChars[depth]);
        if (valueStrs.Length != sortedFields.Count)
            return;

        for (int i = 0; i < sortedFields.Count; ++i)
        {
            if (IsBaseType(sortedFields[i].FieldType))
            {
                SetBaseField(dataObj, sortedFields[i], valueStrs[i]);
            }
            else if (sortedFields[i].FieldType.Name.Contains("List"))
            {
                string typeStr = sortedFields[i].FieldType.ToString();
                var valueList = Activator.CreateInstance(sortedFields[i].FieldType) as IList;

                int idx1 = typeStr.IndexOf('[');
                int idx2 = typeStr.IndexOf(']');
                string childTypeName = typeStr.Substring(idx1 + 1, idx2 - idx1 - 1);
                string[] listValues = valueStrs[i].Split(SaveSplitChars[depth + 1]);
                List<string> notEmptyList = new List<string>(listValues);
                ListTool.ExcludeEmptyStr(notEmptyList);
                //Debug.Log("LoadLise cnt:" + notEmptyList.Count);
                Type childType = null;
                for (int j = 0; j < notEmptyList.Count; ++j)
                {
                    if (childType == null)
                    {
                        childType = Type.GetType(childTypeName);
                    }

                    if (IsBaseType(childType))
                    {
                        var hash = GetBaseFieldValue(childType, notEmptyList[j]);
                        valueList.Add(hash[childType.Name.ToString()]);
                    }
                    else
                    {
                        var listItem = Activator.CreateInstance(childType);
                        if (listItem is SaveItemBase)
                        {
                            string saveFileName = notEmptyList[j];
                            ((SaveItemBase)listItem)._SaveFileName = saveFileName;
                            if (isLoadChild)
                            {
                                string loadStr = LocalSave.Load(saveFileName);
                                LoadNodes(listItem, 0, loadStr, isLoadChild);
                            }
                            valueList.Add(listItem);
                        }
                        else
                        {
                            LoadNodes(listItem, depth + 2, notEmptyList[j], true);
                            valueList.Add(listItem);
                        }
                    }
                }

                sortedFields[i].SetValue(dataObj, valueList);
            }
            else if (sortedFields[i].FieldType.IsEnum)
            {
                sortedFields[i].SetValue(dataObj, int.Parse(valueStrs[i]));
            }
            else if (sortedFields[i].FieldType.BaseType == typeof(Tables.TableRecordBase))
            {
                string recordID = valueStrs[i];

                var record = TableReadEx.GetTableRecord(sortedFields[i].FieldType.Name.Replace("Record", ""), recordID);
                if (record == null)
                {
                    //Debug.LogError("Load Record error:" + sortedFields[i].FieldType.Name + "," + recordID);
                }
                else
                {
                    sortedFields[i].SetValue(dataObj, record);
                }
            }
            else if (sortedFields[i].FieldType.BaseType == typeof(SaveItemBase))
            {
                string saveFileName = valueStrs[i];
                var constumValue = Activator.CreateInstance(sortedFields[i].FieldType);
                ((SaveItemBase)constumValue)._SaveFileName = saveFileName;
                if (isLoadChild)
                {
                    string loadStr = LocalSave.Load(saveFileName);
                    LoadNodes(constumValue, 0, loadStr, isLoadChild);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(valueStrs[i]))
                {
                    var constumValue = Activator.CreateInstance(sortedFields[i].FieldType);
                    LoadNodes(constumValue, depth + 1, valueStrs[i], true);
                    sortedFields[i].SetValue(dataObj, constumValue);
                }
            }

        }

        var iniMethod = dataType.GetMethod("InitFromLoad");
        if (iniMethod != null)
        {
            iniMethod.Invoke(dataObj, new object[] { });
        }

    }

    #endregion


    public static bool IsBaseType(Type fieldType)
    {
        return fieldType == typeof(int) ||
               fieldType == typeof(short) ||
               fieldType == typeof(string) ||
               fieldType == typeof(byte) ||
               fieldType == typeof(long) ||
               fieldType == typeof(float) ||
               fieldType == typeof(double) ||
               fieldType == typeof(bool) ||
               fieldType == typeof(decimal) ||
               fieldType == typeof(DateTime) ||
               fieldType == typeof(TimeSpan) ||
               fieldType == typeof(ulong) ||
               fieldType == typeof(uint) ||
               fieldType == typeof(ushort);
    }

    public static void SetBaseField(object obj, FieldInfo field, string valueStr)
    {
        if (string.IsNullOrEmpty(valueStr))
            return;

        if (field.FieldType == typeof(int))
        {
            field.SetValue(obj, int.Parse(valueStr));
        }
        else if (field.FieldType == typeof(short))
        {
            field.SetValue(obj, short.Parse(valueStr));
        }
        else if (field.FieldType == typeof(string))
        {
            field.SetValue(obj, (valueStr));
        }
        else if (field.FieldType == typeof(byte))
        {
            field.SetValue(obj, byte.Parse(valueStr));
        }
        else if (field.FieldType == typeof(long))
        {
            field.SetValue(obj, long.Parse(valueStr));
        }
        else if (field.FieldType == typeof(double))
        {
            field.SetValue(obj, double.Parse(valueStr));
        }
        else if (field.FieldType == typeof(bool))
        {
            field.SetValue(obj, bool.Parse(valueStr));
        }
        else if (field.FieldType == typeof(decimal))
        {
            field.SetValue(obj, decimal.Parse(valueStr));
        }
        else if (field.FieldType == typeof(DateTime))
        {
            field.SetValue(obj, DateTime.Parse(valueStr));
        }
        else if (field.FieldType == typeof(TimeSpan))
        {
            field.SetValue(obj, TimeSpan.Parse(valueStr));
        }
        else if (field.FieldType == typeof(ulong))
        {
            field.SetValue(obj, ulong.Parse(valueStr));
        }
        else if (field.FieldType == typeof(uint))
        {
            field.SetValue(obj, uint.Parse(valueStr));
        }
        else if (field.FieldType == typeof(ushort))
        {
            field.SetValue(obj, ushort.Parse(valueStr));
        }
    }

    public static Hashtable GetBaseFieldValue(Type valueType, string valueStr)
    {
        Hashtable hash = new Hashtable();
        if (valueType == typeof(int))
        {
            if (string.IsNullOrEmpty(valueStr))
            {
                hash.Add(valueType.Name.ToString(), 0);
            }
            else
            {
                hash.Add(valueType.Name.ToString(), int.Parse(valueStr));
            }
        }
        else if (valueType == typeof(short))
        {
            hash.Add(valueType.Name.ToString(), short.Parse(valueStr));
        }
        else if (valueType == typeof(string))
        {
            hash.Add(valueType.Name.ToString(), (valueStr));
        }
        else if (valueType == typeof(byte))
        {
            hash.Add(valueType.Name.ToString(), byte.Parse(valueStr));
        }
        else if (valueType == typeof(long))
        {
            hash.Add(valueType.Name.ToString(), long.Parse(valueStr));
        }
        else if (valueType == typeof(double))
        {
            hash.Add(valueType.Name.ToString(), double.Parse(valueStr));
        }
        else if (valueType == typeof(bool))
        {
            hash.Add(valueType.Name.ToString(), bool.Parse(valueStr));
        }
        else if (valueType == typeof(decimal))
        {
            hash.Add(valueType.Name.ToString(), decimal.Parse(valueStr));
        }
        else if (valueType == typeof(DateTime))
        {
            hash.Add(valueType.Name.ToString(), DateTime.Parse(valueStr));
        }
        else if (valueType == typeof(TimeSpan))
        {
            hash.Add(valueType.Name.ToString(), TimeSpan.Parse(valueStr));
        }
        else if (valueType == typeof(ulong))
        {
            hash.Add(valueType.Name.ToString(), ulong.Parse(valueStr));
        }
        else if (valueType == typeof(uint))
        {
            hash.Add(valueType.Name.ToString(), uint.Parse(valueStr));
        }
        else if (valueType == typeof(ushort))
        {
            hash.Add(valueType.Name.ToString(), ushort.Parse(valueStr));
        }

        return hash;
    }

}
