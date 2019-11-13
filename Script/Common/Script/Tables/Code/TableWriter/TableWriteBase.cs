using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;

namespace Tables
{
    public class TableWriteBase
    {
        public static string GetWriteStr(int value)
        {
            return value.ToString();
        }

        public static string GetWriteStr(float value)
        {
            return value.ToString();
        }

        public static string GetWriteStr(string value)
        {
            return value;
        }

        public static string GetWriteStr(bool value)
        {
            if (value)
            {
                return "TRUE";
            }
            else
            {
                return "FALSE";
            }
        }

        public static string GetWriteStr(MultiTable value)
        {
            return value.TableName + ";" + value.ID;
        }

        public static string GetWriteStr(Vector3 value)
        {
            return value.x + ";" + value.y + ";" + value.z;
        }
    }
}
