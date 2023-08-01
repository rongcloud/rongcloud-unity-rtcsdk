#if UNITY_ANDROID

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cn_rongcloud_rtc_unity
{
    class StatsListenerProxy : AndroidJavaProxy
    {
        private RCRTCStatsListener listener;

        public StatsListenerProxy( RCRTCStatsListener listener) : base("cn.rongcloud.rtc.wrapper.listener.IRCRTCIWStatsListener")
        {
            this.listener = listener;
        }

        public void onNetworkStats(AndroidJavaObject jstats)
        {
            AndroidJavaObject jtype = jstats.Call<AndroidJavaObject>("getType");
            String ip = jstats.Call<String>("getIp");
            int sendBitrate = jstats.Call<int>("getSendBitrate");
            int receiveBitrate = jstats.Call<int>("getReceiveBitrate");
            int rtt = jstats.Call<int>("getRtt");
            RCRTCNetworkStats stats = new RCRTCNetworkStats(ArgumentAdapter.toNetworkType(jtype), ip, sendBitrate, receiveBitrate, rtt);
            listener.OnNetworkStats(stats);
        }

        public void onLocalAudioStats(AndroidJavaObject jstats)
        {
            listener.OnLocalAudioStats(ArgumentAdapter.toLocalAudioStats(jstats));
        }

        public void onLocalVideoStats(AndroidJavaObject jstats)
        {
            listener.OnLocalVideoStats(ArgumentAdapter.toLocalVideoStats(jstats));
        }

        public void onRemoteAudioStats(String roomId, String userId, AndroidJavaObject jstats)
        {
            listener.OnRemoteAudioStats(userId, ArgumentAdapter.toRemoteAudioStats(jstats));
        }

        public void onRemoteVideoStats(String roomId, String userId, AndroidJavaObject jstats)
        {
            listener.OnRemoteVideoStats(userId, ArgumentAdapter.toRemoteVideoStats(jstats));
        }

        public void onLiveMixAudioStats(AndroidJavaObject jstats)
        {
            listener.OnLiveMixAudioStats(ArgumentAdapter.toRemoteAudioStats(jstats));
        }

        public void onLiveMixVideoStats(AndroidJavaObject jstats)
        {
            listener.OnLiveMixVideoStats(ArgumentAdapter.toRemoteVideoStats(jstats));
        }

        public void onLocalCustomAudioStats(String tag, AndroidJavaObject jstats)
        {
        }

        public void onLocalCustomVideoStats(String tag, AndroidJavaObject jstats)
        {
        }

        public void onRemoteCustomAudioStats(String roomId, String userId, String tag, AndroidJavaObject jstats)
        {
        }

        public void onRemoteCustomVideoStats(String roomId, String userId, String tag, AndroidJavaObject jstats)
        {
        }
    }
}

#endif