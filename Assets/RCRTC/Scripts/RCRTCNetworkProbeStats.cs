using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

namespace cn_rongcloud_rtc_unity
{
    public class RCRTCNetworkProbeStats
    {
        /// <summary>
        ///
        /// </summary>
        public RCRTCNetworkQualityLevel qualityLevel { get; set; }
    
        /// <summary>
        ///
        /// </summary>
        public int rtt { get; set; }
    
        /// <summary>
        ///
        /// </summary>
        public double packetLostRate { get; set; }
    
        public override String ToString()
        {
            return $"qualityLevel:{qualityLevel} rtt:{rtt} packetLostRate:{packetLostRate}";
        }
    }
}
