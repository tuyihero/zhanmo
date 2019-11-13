using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class StoragePack : DataPackBase
{
    #region 单例

    private static StoragePack _Instance;
    public static StoragePack Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new StoragePack();
            }
            return _Instance;
        }
    }

    private StoragePack() { }

    #endregion

    [SaveField(1)]
    private List<ItemBase> _ItemBase;
}

