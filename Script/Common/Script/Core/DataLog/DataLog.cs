using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public static class DataLog
{
    private static string _TalkingdataAppID = "744476D847A404AAA2551F02F51F8D9A";

    public static void StartLog()
    {
#if !UNITY_EDITOR
            //TalkingDataPlugin.SessionStarted(_TalkingdataAppID, ChannelInterface.GetChannelID());
#endif
    }

    public static void StopLog()
    {
#if !UNITY_EDITOR
            //TalkingDataPlugin.SessionStoped();
#endif
    }

    public static void FinishLevel(string levelID, int score)
    {
        //Dictionary<string, object> dic = new Dictionary<string, object>();
        //dic.Add("score", score.ToString());
        //TalkingDataPlugin.TrackEventWithParameters("finishLevel", levelID, dic);
        //TalkingDataPlugin.TrackEvent("finishLevel1");
        //TalkingDataPlugin.TrackEventWithLabel("finishLevel2", levelID);
    }

}

