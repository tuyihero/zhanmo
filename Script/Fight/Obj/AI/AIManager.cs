using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager
{
    #region 单例

    private static AIManager _Instance;
    public static AIManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new AIManager();
            }
            return _Instance;
        }
    }

    private AIManager() { }

    #endregion

    #region 

    private List<AI_Base> _ActAIs = new List<AI_Base>();

    public void RegistAI(AI_Base ai)
    {
        if (_ActAIs.Contains(ai))
            return;

        _ActAIs.Add(ai);
    }

    public void RemoveAI(AI_Base ai)
    {
        _ActAIs.Remove(ai);
    }

    #endregion

    #region group target

    public void GroupAwake(int groupID)
    {
        foreach (var ai in _ActAIs)
        {
            if (ai.GroupID == groupID)
            {
                if (!ai.AIWake)
                {
                    ai.AIWake = true;
                }
            }
        }
    }    

    #endregion
}
