#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

namespace cn_rongcloud_rtc_unity
{
    internal class NativeConvert
    {
        internal static rtc_network_probe_stats toNetworkProbeStats(RCRTCNetworkProbeStats stats)
        {
            rtc_network_probe_stats cobject;
            cobject.qualityLevel = (int)stats.qualityLevel;
            cobject.rtt = (long)stats.rtt;
            cobject.packetLostRate = (float)stats.packetLostRate;
            return cobject;
        }
    
        internal static RCRTCNetworkProbeStats fromNetworkProbeStats(ref rtc_network_probe_stats stats)
        {
            RCRTCNetworkProbeStats obj = new RCRTCNetworkProbeStats();
            makeNetworkProbeStats(obj, ref stats);
            return obj;
        }
    
        internal static void makeNetworkProbeStats(RCRTCNetworkProbeStats stats, ref rtc_network_probe_stats cstats)
        {
            if (stats == null)
            {
                return;
            }
            stats.qualityLevel = (RCRTCNetworkQualityLevel)cstats.qualityLevel;
            stats.rtt = (int)cstats.rtt;
            stats.packetLostRate = (double)cstats.packetLostRate;
        }
    }
}
#endif
