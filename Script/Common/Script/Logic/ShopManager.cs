using UnityEngine;
using System.Collections;



public class ShopManager
{
    #region 单例

    private static ShopManager _Instance;
    public static ShopManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new ShopManager();
            }
            return _Instance;
        }
    }

    private ShopManager() { }

    #endregion

}

