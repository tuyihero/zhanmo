using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

    public enum LocalSaveType
    {
        TIMER_PACK,

        GOLD_COUNT,
        DIAMOND_COUNT,
        TILI_COUNT,
        TILI_BUY,
        HEIGHT_SCORE,
        CAR_PACK,
        CAR_USING_ID,
        DRIVER_PACK,
        DRIVER_USING_ID,
        EQUIP_PACK,
        EQUIP_GUID,
        ITEM_PACK,
        BENISON_PACK,
        MISSION_PACK,
        MISSION_DAYLY_REFRESH_TIME,
        SIGN_YEAR,
        SIGN_DAY,
        SIGN_TIMES,
        CHARGE_STATE,
        STAGE_CHALLENGE,
        STAGE_EX_AWARD,

        SAVE_MAX,
    }

public static class LocalSave
{
    public static void CleanUpAllData()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void Save(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static string Load(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static void Save(LocalSaveType saveType, int value)
    {
        PlayerPrefs.SetInt(saveType.ToString(), value);
        PlayerPrefs.Save();
    }
    public static int LoadInt(LocalSaveType saveType)
    {
        return PlayerPrefs.GetInt(saveType.ToString());
    }

    public static void Save(LocalSaveType saveType, float value)
    {
        PlayerPrefs.SetFloat(saveType.ToString(), value);
        PlayerPrefs.Save();
    }
    public static float LoadFloat(LocalSaveType saveType)
    {
        return PlayerPrefs.GetFloat(saveType.ToString());
    }

    public static void Save(LocalSaveType saveType, string value)
    {
        PlayerPrefs.SetString(saveType.ToString(), value);
        PlayerPrefs.Save();
    }
    public static string LoadString(LocalSaveType saveType)
    {
        return PlayerPrefs.GetString(saveType.ToString());
    }

    public static void Save(LocalSaveType saveType, bool value)
    {
        int intValue = value ? 1 : 0;
        PlayerPrefs.SetInt(saveType.ToString(), intValue);
        PlayerPrefs.Save();
    }
    public static bool LoadBool(LocalSaveType saveType)
    {
        int intValue = PlayerPrefs.GetInt(saveType.ToString());
        return intValue > 0 ? true : false;
    }

    public static void Save(LocalSaveType saveType, System.Object obj)
    {
        Type type = obj.GetType();
        FieldInfo[] fieldInfos = type.GetFields();

        for (int i = 0; i < fieldInfos.Length; ++i)
        {
            string fieldName = saveType.ToString() + "." + fieldInfos[i].Name;

            try
            {
                switch (fieldInfos[i].FieldType.ToString())
                {
                    case "System.Int32":
                        var intValue = (int)fieldInfos[i].GetValue(obj);
                        PlayerPrefs.SetInt(fieldName, intValue);
                        break;
                    case "System.Boolean":
                        var boolValue = (bool)fieldInfos[i].GetValue(obj);
                        int boolInt = boolValue ? 1 : 0;
                        PlayerPrefs.SetInt(fieldName, boolInt);
                        break;
                    case "System.Single":
                        var floatValue = (float)fieldInfos[i].GetValue(obj);
                        PlayerPrefs.SetFloat(fieldName, floatValue);
                        break;
                    case "System.String":
                        var stringValue = (string)fieldInfos[i].GetValue(obj);
                        PlayerPrefs.SetString(fieldName, stringValue);
                        break;
                    case "System.Collections.Generic.List`1[System.Int32]":
                        var listIntValue = fieldInfos[i].GetValue(obj) as List<int>;
                        PlayerPrefs.SetString(fieldName, GetStrFromList(listIntValue));
                        break;
                    case "System.Collections.Generic.List`1[System.Single]":
                        var listFloatValue = fieldInfos[i].GetValue(obj) as List<float>;
                        PlayerPrefs.SetString(fieldName, GetStrFromList(listFloatValue));
                        break;
                    case "System.Collections.Generic.List`1[System.String]":
                        var listStringValue = fieldInfos[i].GetValue(obj) as List<string>;
                        PlayerPrefs.SetString(fieldName, GetStrFromList(listStringValue));
                        break;
                    default:
                        Debug.LogError("LocalSave type error type" + type.ToString() +
                            "name:" + fieldInfos[i].Name +
                            " ,type:" + fieldInfos[i].FieldType.ToString());
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString() + " type:" + fieldInfos[i].FieldType.ToString());
            }
        }
        PlayerPrefs.Save();
    }

    public static T Load<T>(LocalSaveType saveType)
    {
        Type type = typeof(T);
        T obj = (T)Activator.CreateInstance(type);
        FieldInfo[] fieldInfos = type.GetFields();

        for (int i = 0; i < fieldInfos.Length; ++i)
        {
            string fieldName = saveType.ToString() + "." + fieldInfos[i].Name;

            try
            {
                switch (fieldInfos[i].FieldType.ToString())
                {
                    case "System.Int32":
                        var intValue = PlayerPrefs.GetInt(fieldName);
                        fieldInfos[i].SetValue(obj, intValue);
                        break;
                    case "System.Boolean":
                        var boolValue = PlayerPrefs.GetInt(fieldName);
                        bool boolInt = boolValue > 0 ? true : false;
                        fieldInfos[i].SetValue(obj, boolInt);
                        break;
                    case "System.Single":
                        var floatValue = PlayerPrefs.GetFloat(fieldName);
                        fieldInfos[i].SetValue(obj, floatValue);
                        break;
                    case "System.String":
                        var stringValue = PlayerPrefs.GetString(fieldName);
                        fieldInfos[i].SetValue(obj, stringValue);
                        break;
                    case "System.Collections.Generic.List`1[System.Int32]":
                        var listIntStr = PlayerPrefs.GetString(fieldName);
                        List<int> listInt = GetIntListFromStr(listIntStr);
                        fieldInfos[i].SetValue(obj, listInt);
                        break;
                    case "System.Collections.Generic.List`1[System.Single]":
                        var listFloatStr = PlayerPrefs.GetString(fieldName);
                        List<float> listFloat = GetFloatListFromStr(listFloatStr);
                        fieldInfos[i].SetValue(obj, listFloat);
                        break;
                    case "System.Collections.Generic.List`1[System.String]":
                        var listStrStr = PlayerPrefs.GetString(fieldName);
                        List<string> listStr = GetStrListFromStr(listStrStr);
                        fieldInfos[i].SetValue(obj, listStr);
                        break;
                    default:
                        Debug.LogError("LocalSave type error type" + type.ToString() +
                            "name:" + fieldInfos[i].Name +
                            " ,type:" + fieldInfos[i].FieldType.ToString());
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString() + " type:" + fieldInfos[i].FieldType.ToString());
            }
        }
        return obj;
    }

    #region 

    private static string GetStrFromList(List<int> valueList)
    {
        string intStr = "";
        foreach (var value in valueList)
        {
            intStr += value.ToString() + ";";
        }
        return intStr;
    }

    private static string GetStrFromList(List<float> valueList)
    {
        string intStr = "";
        foreach (var value in valueList)
        {
            intStr += value.ToString() + ";";
        }
        return intStr;
    }

    private static string GetStrFromList(List<string> valueList)
    {
        string intStr = "";
        foreach (var value in valueList)
        {
            intStr += value.ToString() + ";";
        }
        return intStr;
    }

    private static List<int> GetIntListFromStr(string str)
    {
        string[] strArrary = str.Split(';');
        List<int> intList = new List<int>();

        foreach (var strValue in strArrary)
        {
            if (!string.IsNullOrEmpty(strValue))
                intList.Add(int.Parse(strValue));
        }
        return intList;
    }

    private static List<float> GetFloatListFromStr(string str)
    {
        string[] strArrary = str.Split(';');
        List<float> intList = new List<float>();

        foreach (var strValue in strArrary)
        {
            if (!string.IsNullOrEmpty(strValue))
                intList.Add(float.Parse(strValue));
        }
        return intList;
    }

    private static List<string> GetStrListFromStr(string str)
    {
        string[] strArrary = str.Split(';');
        List<string> intList = new List<string>();

        foreach (var strValue in strArrary)
        {
            if (!string.IsNullOrEmpty(strValue))
                intList.Add(strValue);
        }
        return intList;
    }
    #endregion

}

