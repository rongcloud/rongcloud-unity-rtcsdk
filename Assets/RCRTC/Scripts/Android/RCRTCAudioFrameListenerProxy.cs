#if UNITY_ANDROID

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cn_rongcloud_rtc_unity
{

    class OnWritableAudioFrameListenerProxy : AndroidJavaProxy
    {
        public OnWritableAudioFrameListenerProxy(RCRTCOnWritableAudioFrameListener listener) : base("cn.rongcloud.rtc.wrapper.listener.RCRTCIWOnWritableAudioFrameListener")
        {
            this.listener = listener;
        }

        public byte[] onAudioFrame(AndroidJavaObject jframe)
        {
            byte[] data = jframe.Call<byte[]>("getBytes");
            int length = jframe.Call<int>("getLength");
            int channels = jframe.Call<int>("getChannels");
            int sampleRate = jframe.Call<int>("getSampleRate");
            int bytesPerSample = jframe.Call<int>("getBytesPerSample");
            int samples = jframe.Call<int>("getSamples");
            RCRTCAudioFrame frame = new RCRTCAudioFrame(data, length, channels, sampleRate, bytesPerSample, samples);
            return listener.OnAudioFrame(ref frame);
        }

        private RCRTCOnWritableAudioFrameListener listener;
    }
}

#endif