using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Lottery
{

    #region 单例

    private static Lottery m_Instance;
    public static Lottery Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new Lottery();
            }
            return m_Instance;
        }
    }

    #endregion
}
