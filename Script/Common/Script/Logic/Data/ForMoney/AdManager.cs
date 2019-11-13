using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ByteDance.Union;

public class AdManager:MonoBehaviour
{
    #region 

    public static void InitAdManager()
    {
        GameObject go = new GameObject();
        go.AddComponent<AdManager>();
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _Instance = this;
    }

    private static AdManager _Instance;

    public static AdManager Instance
    {
        get
        {
            return _Instance;
        }
    }

    #endregion

    #region ad video

    private Action _AdVideoCallBack;

    public void PrepareVideo()
    {
        //PlatformHelper.Instance.LoadVideoAD();
        LoadRewardAd();
    }

    public void WatchAdVideo(Action finishCallBack)
    {
        _AdVideoCallBack = finishCallBack;

        if (PlatformHelper.Instance.GetLocationPermission())
        {
            //PlatformHelper.Instance.ShowVideoAD();
            ShowRewardAd();
        }
    }

    public void WatchAdVideoFinish()
    {
        if (_AdVideoCallBack != null)
        {
            _AdVideoCallBack.Invoke();
        }
    }

    #endregion

    #region ad inter

    public float _ShowInterADTime = 10.0f;
    public float _StartInterADTime;
    public bool _IsInterADPrepared = false;

    private bool _InterADExposure = false;
    private int _LoadSceneTimes = 0;
    private int _ShowAdLoadTimes = 1;
    private bool _IsShowInterAD = false;
    public bool IsShowInterAD
    {
        get
        {
            return false;
            return _IsShowInterAD;
        }
    }

    public void PrepareInterAD()
    {
        //PlatformHelper.Instance.LoadInterAD();
        LoadNativeNannerAd();
    }

    public void LoadedInterAD()
    {
        _IsInterADPrepared = true;
        _StartInterADTime = 0;
    }

    public void ShowInterAD()
    {
        _IsInterADPrepared = false;
        _StartInterADTime = Time.time;
        //_InterADExposure = true;
        //PlatformHelper.Instance.ShowInterAD();
        ShowNativeIntersititialAd();
    }

    public bool IsShowInterADFinish()
    {
        if (!_InterADExposure)
            return true;

        if (Time.time - _StartInterADTime > _ShowInterADTime)
        {
            CloseInterAD();
            return true;
        }
        return false;
    }

    public void CloseInterAD()
    {
        //PlatformHelper.Instance.CloseInterAD();
        if (mNativeAdManager == null)
        {
            mNativeAdManager = GetNativeAdManager();
        }

        if (mNativeAdManager != null)
        {
            mNativeAdManager.Call("CloseInterstitialAD");
        }
    }

    public void OnInterADExposure()
    {
        Debug.Log("OnInterADExposure:" + _InterADExposure);
        _InterADExposure = true;
    }

    public void OnInterADClosed()
    {
        Debug.Log("OnInterADExposure:" + _InterADExposure);
        _InterADExposure = false;
    }

    public void AddLoadSceneTimes()
    {
        CloseInterAD();
        ++_LoadSceneTimes;
        if (_LoadSceneTimes % _ShowAdLoadTimes == 0)
        {
            LoadedInterAD();
            _IsShowInterAD = true;
        }
        else
        {
            _IsShowInterAD = false;
        }
    }

    #endregion

    #region toutiao ad

    private AdNative adNative;
    private RewardVideoAd rewardAd;
    private FullScreenVideoAd fullScreenVideoAd;
    private NativeAd bannerAd;
    private NativeAd intersititialAd;
    private AndroidJavaObject mBannerAd;
    private AndroidJavaObject mIntersititialAd;
    private AndroidJavaObject activity;
    private AndroidJavaObject mNativeAdManager;

    private AdNative AdNative
    {
        get
        {
            if (this.adNative == null)
            {
                this.adNative = SDK.CreateAdNative();
            }

            return this.adNative;
        }
    }

    /// <summary>
    /// Dispose the reward Ad.
    /// </summary>
    public void DisposeAds()
    {
#if UNITY_IOS
        if (this.rewardAd != null)
        {
            this.rewardAd.Dispose();
            this.rewardAd = null;
        }
        if (this.fullScreenVideoAd != null)
        {
            this.fullScreenVideoAd.Dispose();
            this.fullScreenVideoAd = null;
        }

        if (this.bannerAd != null)
        {
            this.bannerAd.Dispose();
            this.bannerAd = null;
        }
        if (this.intersititialAd != null)
        {
            this.intersititialAd.Dispose();
            this.intersititialAd = null;
        }
#else
        if (this.rewardAd != null)
        {
            this.rewardAd = null;
        }
        if (this.fullScreenVideoAd != null)
        {
            this.fullScreenVideoAd = null;
        }
        if (this.mBannerAd != null)
        {
            this.mBannerAd = null;
        }
        if (this.mIntersititialAd != null)
        {
            this.mIntersititialAd = null;
        }
#endif
    }

    public void LoadNativeNannerAd()
    {
//#if UNITY_IOS
//        if (this.bannerAd != null)
//        {
//            Debug.LogError("广告已经加载");
//            this.information.text = "广告已经加载";
//            return;
//        }
//#else
//        if (this.mBannerAd != null)
//        {
//            Debug.LogError("广告已经加载");
//            return;
//        }
//#endif

        var adSlot = new AdSlot.Builder()
#if UNITY_IOS
            .SetCodeId("934833896")
#else
            .SetCodeId("934833896")
#endif
            .SetSupportDeepLink(true)
            .SetImageAcceptedSize(600, 257)
            .SetNativeAdType(AdSlotType.InteractionAd)
            .SetAdCount(1)
            .Build();
        this.AdNative.LoadNativeAd(adSlot, new NativeAdListener(this));
    }

    /// <summary>
    /// Load the reward Ad.
    /// </summary>
    public void LoadRewardAd()
    {
        Debug.Log("start load ad");
        //if (this.rewardAd != null)
        //{
        //    Debug.LogError("广告已经加载");
        //    return;
        //}

        var adSlot = new AdSlot.Builder()
#if UNITY_IOS
            .SetCodeId("934833071")
#else
            .SetCodeId("934833071")
#endif
            .SetSupportDeepLink(true)
            .SetImageAcceptedSize(1080, 1920)
            .SetRewardName("钻石") // 奖励的名称
            .SetRewardAmount(10) // 奖励的数量
            .SetUserID(TalkingDataGA.GetDeviceId()) // 用户id,必传参数
            .SetMediaExtra("") // 附加参数，可选
            .SetOrientation(AdOrientation.Horizontal) // 必填参数，期望视频的播放方向
            .Build();

        this.AdNative.LoadRewardVideoAd(
            adSlot, new RewardVideoAdListener(this));
    }

    /// <summary>
    /// Show the reward Ad.
    /// </summary>
    public void ShowRewardAd()
    {
        if (this.rewardAd == null)
        {
            Debug.LogError("请先加载广告");
            return;
        }
        this.rewardAd.ShowRewardVideoAd();
    }

    public void ShowNativeIntersititialAd()
    {
#if UNITY_IOS
        if (intersititialAd == null)
        {
            Debug.LogError("请先加载广告");
            this.information.text = "请先加载广告";
            return;
        }
        this.intersititialAd.ShowNativeAd();
#else
        if (mIntersititialAd == null)
        {
            Debug.LogError("请先加载广告");
            return;
        }
        if (mNativeAdManager == null)
        {
            mNativeAdManager = GetNativeAdManager();
        }
        mNativeAdManager.Call("showNativeIntersititialAd", activity, mIntersititialAd);
#endif
    }

    public AndroidJavaObject GetNativeAdManager()
    {
        if (mNativeAdManager != null)
        {
            return mNativeAdManager;
        }
        if (activity == null)
        {
            var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activity = unityPlayer.GetStatic<AndroidJavaObject>(
           "currentActivity");
        }
        var jc = new AndroidJavaClass(
                    "com.bytedance.android.NativeAdManager");
        mNativeAdManager = jc.CallStatic<AndroidJavaObject>("getNativeAdManager");
        return mNativeAdManager;
    }

    private sealed class NativeAdInteractionListener : IInteractionAdInteractionListener
    {
        private AdManager example;

        public NativeAdInteractionListener(AdManager example)
        {
            this.example = example;
        }

        public void OnAdShow()
        {
            Debug.Log("NativeAd show");
            this.example.OnInterADExposure();
        }

        public void OnAdClicked()
        {
            Debug.Log("NativeAd click");
        }

        public void OnAdDismiss()
        {
            Debug.Log("NativeAd close");
            this.example.OnInterADClosed();
        }
    }

    private sealed class NativeAdListener : INativeAdListener
    {
        private AdManager example;

        public NativeAdListener(AdManager example)
        {
            this.example = example;
        }

        public void OnError(int code, string message)
        {
            Debug.LogError("OnNativeAdError: " + message);
        }

        public void OnNativeAdLoad(AndroidJavaObject list, NativeAd ad)
        {

#if UNITY_IOS
            this.example.bannerAd = ad;
            this.example.intersititialAd = ad;
            ad.SetNativeAdInteractionListener(
                new NativeAdInteractionListener(this.example)
            );
#else

            var size = list.Call<int>("size");

            if (size > 0)
            {
                this.example.mBannerAd = list.Call<AndroidJavaObject>("get", 0);
                this.example.mIntersititialAd = list.Call<AndroidJavaObject>("get", 0);
            }
#endif
            //if (ads == null && ads.[0])
            //{
            //    return;
            //}
            ////this.example.bannerAd = ads.[0];
            //this.example.bannerAd = ads.[0];
            Debug.Log("OnNativeAdLoad");

            example.LoadedInterAD();
            //bannerAd.;
            //bannerAd.SetDownloadListener(
            //new AppDownloadListener(this.example));

        }
    }

    private sealed class RewardVideoAdListener : IRewardVideoAdListener
    {
        private AdManager example;

        public RewardVideoAdListener(AdManager example)
        {
            this.example = example;
        }

        public void OnError(int code, string message)
        {
            Debug.LogError("OnRewardError: " + message);
        }

        public void OnRewardVideoAdLoad(RewardVideoAd ad)
        {
            Debug.Log("OnRewardVideoAdLoad");

            ad.SetRewardAdInteractionListener(
                new RewardAdInteractionListener(this.example));
            ad.SetDownloadListener(
                new AppDownloadListener(this.example));

            this.example.rewardAd = ad;
        }

        public void OnRewardVideoCached()
        {
            Debug.Log("OnRewardVideoCached");
        }
    }

    private sealed class RewardAdInteractionListener : IRewardAdInteractionListener
    {
        private AdManager example;

        public RewardAdInteractionListener(AdManager example)
        {
            this.example = example;
        }

        public void OnAdShow()
        {
            Debug.Log("rewardVideoAd show");
        }

        public void OnAdVideoBarClick()
        {
            Debug.Log("rewardVideoAd bar click");
        }

        public void OnAdClose()
        {
            Debug.Log("rewardVideoAd close");
        }

        public void OnVideoComplete()
        {
            Debug.Log("rewardVideoAd complete");
            example.WatchAdVideoFinish();
        }

        public void OnVideoError()
        {
            Debug.LogError("rewardVideoAd error");
        }

        public void OnRewardVerify(
            bool rewardVerify, int rewardAmount, string rewardName)
        {
            Debug.Log("verify:" + rewardVerify + " amount:" + rewardAmount +
                " name:" + rewardName);
        }
    }

    private sealed class AppDownloadListener : IAppDownloadListener
    {
        private AdManager example;

        public AppDownloadListener(AdManager example)
        {
            this.example = example;
        }

        public void OnIdle()
        {
        }

        public void OnDownloadActive(
            long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.Log("下载中，点击下载区域暂停");
        }

        public void OnDownloadPaused(
            long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.Log("下载暂停，点击下载区域继续");
        }

        public void OnDownloadFailed(
            long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.LogError("下载失败，点击下载区域重新下载");
        }

        public void OnDownloadFinished(
            long totalBytes, string fileName, string appName)
        {
            Debug.Log("下载完成，点击下载区域重新下载");
        }

        public void OnInstalled(string fileName, string appName)
        {
            Debug.Log("安装完成，点击下载区域打开");
        }
    }

    #endregion
}
