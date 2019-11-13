using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaGroup : MonoBehaviour
{
    [System.Serializable]
    public class AreaPasses
    {
        public List<int> _PassAreaes;
    }

    #region 

    public List<FightSceneAreaRandom> _FightAreas;

    public List<FightSceneAreaRandom> _TeleAreas;

    public List<AreaPasses> _AreaPasses;

    public GameObject _LightGO;

    #endregion


}
