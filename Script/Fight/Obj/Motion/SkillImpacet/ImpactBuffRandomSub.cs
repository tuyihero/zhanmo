using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffRandomSub : ImpactBuffSub
{
    public List<GameObject> _RandomSubList;
    public int _ActSubCnt = 1;

    private List<GameObject> _ActSubObjs = new List<GameObject>();
    public List<GameObject> ActSubObjs
    {
        get
        {
            return _ActSubObjs;
        }
    }

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        for (int i = 0; i < _ActSubCnt; ++i)
        {
            int randomIdx = Random.Range(0, _RandomSubList.Count);
            _SubImpacts = new List<ImpactBase>(_RandomSubList[randomIdx].GetComponents<ImpactBase>());
            //_ActSubObjs.Add(_RandomSubList[randomIdx]);
            _BuffOwner.AddBuffName(Tables.StrDictionary.GetFormatStr(_RandomSubList[randomIdx].GetComponent<UIText>()._StrDict));
            ActSubImpacts();
        }
    }
}
