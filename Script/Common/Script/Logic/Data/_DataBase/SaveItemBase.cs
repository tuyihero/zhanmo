using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveItemBase
{
    public string _SaveFileName;
    public bool _DirtyFlag;

    public void SaveClass(bool isSaveChild)
    {
        if (string.IsNullOrEmpty(_SaveFileName))
        {
            _SaveFileName = this.GetType().ToString();
        }
        DataPackSave.SaveData(this, isSaveChild);
    }

    public void LoadClass(bool loadChild)
    {
        DataPackSave.LoadData(this, loadChild);
    }
}
