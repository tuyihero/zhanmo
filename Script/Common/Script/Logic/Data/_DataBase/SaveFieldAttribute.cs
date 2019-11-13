using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



[AttributeUsage(
AttributeTargets.Field |
AttributeTargets.Property,
AllowMultiple = true)]

public class SaveField : System.Attribute
{
    public int _SaveIDX = 0;
    public List<string> _SaveFieldNames = new List<string>();

    public SaveField(int idx)
    {
        _SaveIDX = idx;
    }

    public SaveField(int idx, params string[] customField)
    {
        _SaveIDX = idx;
        foreach (var fieldName in customField)
        {
            _SaveFieldNames.Add(fieldName);
        }
    }

}

