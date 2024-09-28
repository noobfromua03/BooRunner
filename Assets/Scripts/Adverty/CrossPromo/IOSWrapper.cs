using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CrossPromo
{
    public class IOSWrapper : MonoBehaviour, ICrossPromoWrapper
    {
        #region Instance
        private static IOSWrapper _instance;

        public static IOSWrapper Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Назву не міняти, по ній приходить зворотній виклик з нативної частини
                    GameObject instObj = new GameObject("__SmokokoCPWrapper");
                    _instance = instObj.AddComponent<IOSWrapper>();
                    DontDestroyOnLoad(instObj);
                }
                return _instance;
            }
        }

        #endregion

        public const string LINKID = "https://tool.smokoko.com/admin/project/getconfig/11/48/settings.json";
        public const string DPPERCENTS = "https://tool.smokoko.com/admin/project/getconfig/11/398/settings.json";


#if UNITY_IOS && !UNITY_EDITOR

        [DllImport("__Internal")]
        private static extern void SmokokoCPInit(string config, string percents);

        [DllImport("__Internal")]
        private static extern void SmokokoCPRequestVideo();

        [DllImport("__Internal")]
        private static extern void SmokokoCPShowReward();

        [DllImport("__Internal")]
        private static extern void SmokokoCPShowInterstitial();

        [DllImport("__Internal")]
        private static extern bool SmokokoCPVideoIsReady();

        [DllImport("__Internal")]
        private static extern bool SmokokoCPVideoQueueReady();

#endif

        public Action OnAdClosed { get; set; }
        public Action OnRewardGranted { get; set; }

        public bool IsInited { get; private set; } = false;

        public bool AdIsReady
        {
            get
            {
                if (!IsInited)
                {
                    return false;
                }

#if UNITY_IOS && !UNITY_EDITOR
                return SmokokoCPVideoIsReady();
#else
                return false;
#endif
            }
        }

        public bool AdShowRateReady
        {
            get
            {
                if (!IsInited)
                {
                    return false;
                }

#if UNITY_IOS && !UNITY_EDITOR
                return SmokokoCPVideoQueueReady();
#else
                return false;
#endif
            }
        }

        public void Init()
        {
            if (!IsInited)
            {
#if UNITY_IOS && !UNITY_EDITOR
                SmokokoCPInit(LINKID, DPPERCENTS);
#endif
                RequestVideo();
                IsInited = true;
            }
        }

        public void ShowRewardedVideo()
        {
#if UNITY_IOS && !UNITY_EDITOR
            SmokokoCPShowReward();
#endif
        }

        public void ShowInterstitial()
        {
#if UNITY_IOS && !UNITY_EDITOR
            SmokokoCPShowInterstitial();
#endif
        }

        public void ResetShowRate() { }

        public void IncrementShowRate() { }

        public void RequestVideo()
        {
#if UNITY_IOS && !UNITY_EDITOR
            SmokokoCPRequestVideo();
#endif
        }

        // Назву не міняти, по ній приходить зворотній виклик з нативної частини
        public void SmokokoCPClosedAd(string str)
        {
            OnAdClosed?.Invoke();
        }

        // Назву не міняти, по ній приходить зворотній виклик з нативної частини
        public void SmokokoCPRewardGranted(string str)
        {
            OnRewardGranted?.Invoke();
            Debug.Log("Reward granted");
        }

    }
}
