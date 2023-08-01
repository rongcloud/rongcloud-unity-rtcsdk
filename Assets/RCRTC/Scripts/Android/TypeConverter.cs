#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

namespace cn_rongcloud_rtc_unity
{
    
    public class Converter
    {
        protected object c_intance;
        protected AndroidJavaObject j_intance;
        private static AndroidJavaClass UtilsClass = new AndroidJavaClass("cn.rongcloud.im.wrapper.unity.Utils");
        public static bool isInstance(AndroidJavaObject obj, string javaClass)
        {
            return UtilsClass.CallStatic<bool>("isInstanceOf", obj, javaClass);
        }
    
        public AndroidJavaObject getAndroidObject()
        {
            return this.j_intance;
        }
        public object getCSharpObject()
        {
            return this.c_intance;
        }
    
        public virtual void toAndroidObject()
        {
        }
        public virtual void toCSharpObject()
        {
        }
    }
    public class NetworkProbeStatsConverter : Converter
    {
        public NetworkProbeStatsConverter(RCRTCNetworkProbeStats stats, string javaClass)
        {
            this.c_intance = stats;
            this.j_intance = new AndroidJavaObject(javaClass);
            this.toAndroidObject();
        }
    
        public NetworkProbeStatsConverter(AndroidJavaObject stats, object _c_intance)
        {
            this.j_intance = stats;
            this.c_intance = _c_intance;
            this.toCSharpObject();
        }
    
        public static NetworkProbeStatsConverter from(RCRTCNetworkProbeStats stats)
        {
            NetworkProbeStatsConverter converter =
                new NetworkProbeStatsConverter(stats, "cn.rongcloud.rtc.wrapper.constants.RCRTCIWNetworkProbeStats");
            return converter;
        }
    
        public static NetworkProbeStatsConverter from(AndroidJavaObject stats)
        {
            NetworkProbeStatsConverter converter = new NetworkProbeStatsConverter(stats, new RCRTCNetworkProbeStats());
            return converter;
        }
    
        public override void toAndroidObject()
        {
            base.toAndroidObject();
            RCRTCNetworkProbeStats __instance = (RCRTCNetworkProbeStats)this.c_intance;
            RCRTCNetworkQualityLevel _qualityLevel = __instance.qualityLevel;
            int _rtt = __instance.rtt;
            double _packetLostRate = __instance.packetLostRate;
    
            j_intance.Set<AndroidJavaObject>("qualityLevel",
                                             new NetworkQualityLevelConverter(_qualityLevel).getAndroidObject());
            j_intance.Set<int>("rtt", _rtt);
            j_intance.Set<double>("packetLostRate", _packetLostRate);
        }
    
        public override void toCSharpObject()
        {
            base.toCSharpObject();
            RCRTCNetworkProbeStats __instance = (RCRTCNetworkProbeStats)this.c_intance;
            AndroidJavaObject _qualityLevel = j_intance.Get<AndroidJavaObject>("qualityLevel");
            if (_qualityLevel != null)
                __instance.qualityLevel = new NetworkQualityLevelConverter(_qualityLevel).getCSharpObject();
            __instance.rtt = j_intance.Get<int>("rtt");
            __instance.packetLostRate = j_intance.Get<double>("packetLostRate");
        }
    }
    
    public class NetworkQualityLevelConverter
    {
        private AndroidJavaClass javaClass =
            new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWNetworkQualityLevel");
        private RCRTCNetworkQualityLevel c_intance;
        private AndroidJavaObject j_intance;
        public NetworkQualityLevelConverter(AndroidJavaObject level)
        {
            this.j_intance = level;
            int index = level.Call<int>("ordinal");
            c_intance = (RCRTCNetworkQualityLevel)Enum.Parse(typeof(RCRTCNetworkQualityLevel), index.ToString());
        }
    
        public NetworkQualityLevelConverter(RCRTCNetworkQualityLevel level)
        {
            this.c_intance = level;
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            j_intance = values[(int)level];
        }
    
        public AndroidJavaObject getAndroidObject()
        {
            return this.j_intance;
        }
    
        public RCRTCNetworkQualityLevel getCSharpObject()
        {
            return this.c_intance;
        }
    }
    
    public class RoleConverter
    {
        private AndroidJavaClass javaClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWRole");
        private RCRTCRole c_intance;
        private AndroidJavaObject j_intance;
        public RoleConverter(AndroidJavaObject role)
        {
            this.j_intance = role;
            int index = role.Call<int>("ordinal");
            c_intance = (RCRTCRole)Enum.Parse(typeof(RCRTCRole), index.ToString());
        }
    
        public RoleConverter(RCRTCRole role)
        {
            this.c_intance = role;
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            j_intance = values[(int)role];
        }
    
        public AndroidJavaObject getAndroidObject()
        {
            return this.j_intance;
        }
    
        public RCRTCRole getCSharpObject()
        {
            return this.c_intance;
        }
    }
    
    public class VideoFpsConverter
    {
        private AndroidJavaClass javaClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWVideoFps");
        private RCRTCVideoFps c_intance;
        private AndroidJavaObject j_intance;
        public VideoFpsConverter(AndroidJavaObject fps)
        {
            this.j_intance = fps;
            int index = fps.Call<int>("ordinal");
            c_intance = (RCRTCVideoFps)Enum.Parse(typeof(RCRTCVideoFps), index.ToString());
        }
    
        public VideoFpsConverter(RCRTCVideoFps fps)
        {
            this.c_intance = fps;
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            j_intance = values[(int)fps];
        }
    
        public AndroidJavaObject getAndroidObject()
        {
            return this.j_intance;
        }
    
        public RCRTCVideoFps getCSharpObject()
        {
            return this.c_intance;
        }
    }
    
    public class MediaTypeConverter
    {
        private AndroidJavaClass javaClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWMediaType");
        private RCRTCMediaType c_intance;
        private AndroidJavaObject j_intance;
        public MediaTypeConverter(AndroidJavaObject type)
        {
            this.j_intance = type;
            int index = type.Call<int>("ordinal");
            c_intance = (RCRTCMediaType)Enum.Parse(typeof(RCRTCMediaType), index.ToString());
        }
    
        public MediaTypeConverter(RCRTCMediaType type)
        {
            this.c_intance = type;
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            j_intance = values[(int)type];
        }
    
        public AndroidJavaObject getAndroidObject()
        {
            return this.j_intance;
        }
    
        public RCRTCMediaType getCSharpObject()
        {
            return this.c_intance;
        }
    }
    
}
#endif
