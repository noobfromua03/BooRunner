using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossPromo
{
    public class AndroidWrapper : MonoBehaviour, ICrossPromoWrapper
    {
        #region Instance
        private static AndroidWrapper _instance;

        public static AndroidWrapper Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Назву не міняти, по ній приходить зворотній виклик з нативної частини
                    GameObject instObj = new GameObject("_Cross");
                    _instance = instObj.AddComponent<AndroidWrapper>();
                    DontDestroyOnLoad(instObj);
                }
                return _instance;
            }
        }
        #endregion

        public const string LINKID = "https://tool.smokoko.com/admin/project/getconfig/11/46/settings.json";
        public const string DPPERCENTS = "https://tool.smokoko.com/admin/project/getconfig/11/397/settings.json";

        private const string pluginName = "com.smokoko.promo.Promo";

#if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaClass _pluginClass;
        private static AndroidJavaObject _pluginInstance;
#endif
        public int _sessionShowRate = 0;

        public bool IsInited { get; private set; } = false;
        public Action OnAdClosed { get; set; }
        public Action OnRewardGranted { get; set; }

        public static AndroidJavaClass PluginClass
        {
            get
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                if (_pluginClass == null)
                {
                    _pluginClass = new AndroidJavaClass(pluginName);
                }
                return _pluginClass;
#else
                return null;
#endif
            }
        }

        public static AndroidJavaObject PluginInstance
        {
            get
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                if (_pluginInstance == null)
                {
                    _pluginInstance = PluginClass.CallStatic<AndroidJavaObject>("getInstance");
                }
                return _pluginInstance;
#else
                return null;
#endif
            }
        }

        private int ShowRate
        {
            get
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                return PluginInstance.GetStatic<int>("ShowRate");
#else
                return -1;
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

                return _sessionShowRate >= ShowRate;
            }
        }

        public bool AdIsReady
        {
            get
            {
                if (!IsInited)
                {
                    return false;
                }

#if UNITY_ANDROID && !UNITY_EDITOR
                return PluginInstance.GetStatic<bool>("IsReadyVideo");
#else
                return false;
#endif
            }
        }

        public void Init()
        {
            if (!IsInited)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                PluginInstance.Call("Init", LINKID, DPPERCENTS);
#endif
                IsInited = true;
            }
        }

        public void ShowRewardedVideo()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            PluginInstance.CallStatic("SetInterFalse");
            Show();
#endif
        }

        public void ShowInterstitial()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            PluginInstance.CallStatic("SetInterTrue");
            Show();
#endif
        }

        public async void RequestVideo()
        {
            await UniTask.Delay(1500, true);
#if UNITY_ANDROID && !UNITY_EDITOR
            PluginInstance.CallStatic("Load");
#endif
        }

        private void Show()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (AdIsReady)
            {
                var androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var jo = androidJC.GetStatic<AndroidJavaObject>("currentActivity");

                PluginClass.CallStatic("Call", jo);
            }
            else
            {
                Debug.Log("Dont Load video");
            }
#endif
        }

        public void ResetShowRate()
        {
            _sessionShowRate = 0;
        }

        public void IncrementShowRate()
        {
            _sessionShowRate++;
        }

        // Назву не міняти, по ній приходить зворотній виклик з нативної частини
        private void InvokeRewardSuccess(string str)
        {
            OnRewardGranted?.Invoke();
        }

        // Назву не міняти, по ній приходить зворотній виклик з нативної частини
        public void InvokeCloseCross(string str)
        {
            OnAdClosed?.Invoke();
        }
    }
}
