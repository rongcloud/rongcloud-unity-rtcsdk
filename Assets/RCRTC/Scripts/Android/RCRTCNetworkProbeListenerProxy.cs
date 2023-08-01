#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

namespace cn_rongcloud_rtc_unity
{
    public class RCRTCNetworkProbeListenerProxy : AndroidJavaProxy
    {
        RCRTCNetworkProbeListener listener;
    
        public RCRTCNetworkProbeListenerProxy(RCRTCNetworkProbeListener listener)
            : base("cn.rongcloud.rtc.wrapper.listener.IRCRTCIWNetworkProbeListener")
        {
            this.listener = listener;
        }
    
        public void onNetworkProbeUpLinkStats(AndroidJavaObject stats)
        {
            RCRTCNetworkProbeStats _stats = null;
            if (stats != null)
                _stats = (RCRTCNetworkProbeStats)NetworkProbeStatsConverter.from(stats).getCSharpObject();
            if (listener != null)
                listener.OnNetworkProbeUpLinkStats(_stats);
        }
    
        public void onNetworkProbeDownLinkStats(AndroidJavaObject stats)
        {
            RCRTCNetworkProbeStats _stats = null;
            if (stats != null)
                _stats = (RCRTCNetworkProbeStats)NetworkProbeStatsConverter.from(stats).getCSharpObject();
            if (listener != null)
                listener.OnNetworkProbeDownLinkStats(_stats);
        }
    
        public void onNetworkProbeFinished(int code, string errMsg)
        {
            if (listener != null)
                listener.OnNetworkProbeFinished(code, errMsg);
        }
    }
}
#endif
