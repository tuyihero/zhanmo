using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class PlatformHelper : MonoBehaviour
{
    #region 
    private static PlatformHelper _Instance;

    public static PlatformHelper Instance
    {
        get
        {
            return _Instance;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        _Instance = this;
    }
    #endregion

#if UNITY_ANDROID 
    public string PLATFORM_CLASS = "";
    public string CallAndroid(string func, string param)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (string.IsNullOrEmpty(PLATFORM_CLASS))
        {
            PLATFORM_CLASS = Application.identifier + ".PlatformHelper";
        }
        Debug.LogError("OnCallAndroid:" + func + "," + param);
        using (AndroidJavaClass cls = new AndroidJavaClass(PLATFORM_CLASS))
        {   
            string ret = cls.CallStatic<string>("jniCall", func, param);
            return ret;
        }
#endif
        return "0";
    }

    public void OnAndroidCall(string jsonstr)
    {
        Debug.LogError("OnCallResult : " + jsonstr);
        JsonData jsonobj = JsonMapper.ToObject(jsonstr);
        string func = (string)jsonobj["func"];

        if (func.Equals("OnVideoADLoad"))
        {
            OnVideoADLoaded();
        }
        else if (func.Equals("OnVideoReward"))
        {
            OnVideoADReward();
        }
        else if (func.Equals("OnVideoComplete"))
        {
            OnVideoADComplate();
        }
        else if (func.Equals("OnInterADReceive"))
        {
            OnInterADLoaded();
        }
        else if (func.Equals("OnInterADExpusure"))
        {
            OnInterADExposure();
        }
        else if (func.Equals("OnInterADClose"))
        {
            OnInterADClosed();
        }
    }

#endif

        #region AD

    public bool GetLocationPermission()
    {
        //string permissionStr = "android.permission.ACCESS_FINE_LOCATION";
        //return CallAndroid("RequstePermission", permissionStr).Equals("1");
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            // The user denied permission to use the microphone.
            // Display a message explaining why you need it with Yes/No buttons.
            // If the user says yes then present the request again
            // Display a dialog here.
            Permission.RequestUserPermission(Permission.FineLocation);
            return false;
        }
        else
        {
            return true;
        }
#endif
    }

    public void LoadVideoAD()
    {
        CallAndroid("LoadVideoAD", "");
    }

    public void ShowVideoAD()
    {
        CallAndroid("ShowVideoAD", "");
    }

    public void LoadInterAD()
    {
        CallAndroid("LoadInterstitialAD", "");
    }

    public void ShowInterAD()
    {
        CallAndroid("ShowInterstitialAD", "");
    }

    public void CloseInterAD()
    {
        CallAndroid("CloseInterstitialAD", "");
    }

    public void OnVideoADLoaded()
    {
        //result.text = "OnVideoADLoaded";
    }

    public void OnVideoADReward()
    {
        AdManager.Instance.WatchAdVideoFinish();
    }

    public void OnVideoADComplate()
    {
        //result.text = "OnVideoADComplate";
    }

    public void OnInterADLoaded()
    {
        //result.text = "OnInterADLoaded";
    }

    public void OnInterADExposure()
    {
        AdManager.Instance.OnInterADExposure();
    }

    public void OnInterADClosed()
    {
        AdManager.Instance.OnInterADClosed();
    }

    #endregion



}
