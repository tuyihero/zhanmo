using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
 

public class FightSceneLogicPassTest : FightSceneLogicPassArea
{
    public override void StartLogic()
    {
        if (FightManager.Instance.MainChatMotion != null)
        {
            FightManager.Instance.MainChatMotion.SetPosition(_MainCharBornPos.position);
        }
        StartCoroutine(StartLogicDelay());
    }

    private IEnumerator StartLogicDelay()
    {
        yield return new WaitForSeconds(2.0f);
        base.StartLogic();
    }

    public override void AreaStart(FightSceneAreaBase startArea)
    {
        
        base.AreaStart(startArea);
    }

    public override void AreaFinish(FightSceneAreaBase finishArea)
    {
        
        
    }

    public override void StartNextArea()
    {
        base.StartNextArea();
    }

    protected override void UpdateLogic()
    {
        base.UpdateLogic();

        //UpdateTeleport();
    }
    
}
