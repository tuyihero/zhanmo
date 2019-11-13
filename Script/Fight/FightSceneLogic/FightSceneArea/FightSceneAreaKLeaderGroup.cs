using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class FightSceneAreaKLeaderGroup : FightSceneAreaKBossWithFish
{
    public List<string> _ExEliteRandomIDs = new List<string>()
    {"201","202","203","204","205","205","206","207","208","209", "210", "211", "212", "213", "214"};
    public List<string> _NormaRandomIDs = new List<string>()
    {"21","23","25","27","33","37","48","50","31","39"};
    public int _LeaderCnt = 1;

    public override void StartArea()
    {
        base.StartArea();

        UIFightWarning.ShowBossAsyn();

        StartStep();
    }
}
