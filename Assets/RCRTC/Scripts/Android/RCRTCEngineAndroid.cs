#if UNITY_ANDROID

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cn_rongcloud_rtc_unity
{

    internal class RCRTCEngineAndroid : RCRTCEngine
    {
        internal RCRTCEngineAndroid(RCRTCEngineSetup setup)
        {
            AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = player.GetStatic<AndroidJavaObject>("currentActivity");
            Engine = EngineClass.CallStatic<AndroidJavaObject>("newInstance", context, toEngineSetup(setup));
            Engine.Call("setListener", new ListenerProxy(this));
        }

        public override void Destroy()
        {
            if (Engine != null)
            {
                Engine.Call("destroy");
                Engine = null;
            }
            base.Destroy();
        }

        public override int JoinRoom(String roomId, RCRTCRoomSetup setup)
        {
            return Engine.Call<int>("joinRoom", roomId, toRoomSetup(setup));
        }

        public override int LeaveRoom()
        {
            return Engine.Call<int>("leaveRoom");
        }

        public override int Publish(RCRTCMediaType type)
        {
            return Engine.Call<int>("publish", toMediaType(type));
        }

        public override int Unpublish(RCRTCMediaType type)
        {
            return Engine.Call<int>("unpublish", toMediaType(type));
        }

        public override int Subscribe(String userId, RCRTCMediaType type, bool tiny)
        {
            return Engine.Call<int>("subscribe", userId, toMediaType(type), tiny);
        }

        public override int Subscribe(IList<String> userIds, RCRTCMediaType type, bool tiny)
        {
            return Engine.Call<int>("subscribe", toStringArray(userIds), toMediaType(type), tiny);
        }

        public override int SubscribeLiveMix(RCRTCMediaType type, bool tiny)
        {
            return Engine.Call<int>("subscribeLiveMix", toMediaType(type), tiny);
        }

        public override int Unsubscribe(String userId, RCRTCMediaType type)
        {
            return Engine.Call<int>("unsubscribe", userId, toMediaType(type));
        }

        public override int Unsubscribe(IList<String> userIds, RCRTCMediaType type)
        {
            return Engine.Call<int>("unsubscribe", toStringArray(userIds), toMediaType(type));
        }

        public override int UnsubscribeLiveMix(RCRTCMediaType type)
        {
            return Engine.Call<int>("unsubscribeLiveMix", toMediaType(type));
        }

        public override int SetAudioConfig(RCRTCAudioConfig config)
        {
            return Engine.Call<int>("setAudioConfig", toAudioConfig(config));
        }

        public override int SetVideoConfig(RCRTCVideoConfig config, bool tiny)
        {
            return Engine.Call<int>("setVideoConfig", toVideoConfig(config), tiny);
        }

        public override int EnableMicrophone(bool enable)
        {
            return Engine.Call<int>("enableMicrophone", enable);
        }

        public override int EnableSpeaker(bool enable)
        {
            return Engine.Call<int>("enableSpeaker", enable);
        }

        public override int EnableCamera(bool enable, RCRTCCamera camera)
        {
            return Engine.Call<int>("enableCamera", enable, toCamera(camera));
        }

        public override int SwitchCamera()
        {
            return Engine.Call<int>("switchCamera");
        }

        public override RCRTCCamera WhichCamera()
        {
            AndroidJavaObject jobject = Engine.Call<AndroidJavaObject>("WhichCamera");
            int camera = jobject.Call<int>("getCamera");
            return (RCRTCCamera)camera;
        }

        public override bool IsCameraFocusSupported()
        {
            return Engine.Call<bool>("isCameraFocusSupported");
        }

        public override bool IsCameraExposurePositionSupported()
        {
            return Engine.Call<bool>("isCameraExposurePositionSupported");
        }

        public override int SetCameraFocusPositionInPreview(double x, double y)
        {
            return Engine.Call<int>("setCameraFocusPositionInPreview", x, y);
        }

        public override int SetCameraExposurePositionInPreview(double x, double y)
        {
            return Engine.Call<int>("setCameraExposurePositionInPreview", x, y);
        }

        public override int SetCameraCaptureOrientation(RCRTCCameraCaptureOrientation orientation)
        {
            return Engine.Call<int>("setCameraCaptureOrientation", toCameraCaptureOrientation(orientation));
        }

        public override int AdjustLocalVolume(int volume)
        {
            return Engine.Call<int>("adjustLocalVolume", volume);
        }

        public override int MuteLocalStream(RCRTCMediaType type, bool mute)
        {
            return Engine.Call<int>("muteLocalStream", toMediaType(type), mute);
        }

        public override int MuteRemoteStream(String userId, RCRTCMediaType type, bool mute)
        {
            return Engine.Call<int>("muteRemoteStream", userId, toMediaType(type), mute);
        }

        public override int MuteAllRemoteAudioStreams(bool mute)
        {
            return Engine.Call<int>("muteAllRemoteAudioStreams", mute);
        }

        private AndroidJavaObject toNativeView(RCRTCView view)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = typeof(RCRTCView);
            MethodInfo method = type.GetMethod("InitNativeView", flag);
            object[] arguments = { Engine };
            method.Invoke(view, arguments);
            FieldInfo field = type.GetField("_view", flag);
            return (AndroidJavaObject) field.GetValue(view);
        }

        public override int SetLocalView(RCRTCView view)
        {
            return Engine.Call<int>("setLocalBaseView", toNativeView(view));
        }

        public override int RemoveLocalView()
        {
            return Engine.Call<int>("removeLocalView");
        }

        public override int SetRemoteView(String userId, RCRTCView view)
        {
            return Engine.Call<int>("setRemoteBaseView", userId, toNativeView(view));
        }

        public override int RemoveRemoteView(String userId)
        {
            return Engine.Call<int>("removeRemoteView", userId);
        }

        public override int SetLiveMixView(RCRTCView view)
        {
            return Engine.Call<int>("setLiveMixBaseView", toNativeView(view));
        }

        public override int RemoveLiveMixView()
        {
            return Engine.Call<int>("removeLiveMixView");
        }

        public override int AddLiveCdn(String url)
        {
            return Engine.Call<int>("addLiveCdn", url);
        }

        public override int RemoveLiveCdn(String url)
        {
            return Engine.Call<int>("removeLiveCdn", url);
        }

        public override int SetLiveMixLayoutMode(RCRTCLiveMixLayoutMode mode)
        {
            return Engine.Call<int>("setLiveMixLayoutMode", toLiveMixLayoutMode(mode));
        }

        public override int SetLiveMixRenderMode(RCRTCLiveMixRenderMode mode)
        {
            return Engine.Call<int>("setLiveMixRenderMode", toLiveMixRenderMode(mode));
        }

        public override int SetLiveMixCustomAudios(IList<String> userIds)
        {
            if (userIds == null || userIds.Count == 0)
                return -1;
            return Engine.Call<int>("setLiveMixCustomAudios", toStringList(userIds));
        }

        public override int SetLiveMixCustomLayouts(IList<RCRTCCustomLayout> layouts)
        {
            if (layouts == null || layouts.Count == 0)
                return -1;
            return Engine.Call<int>("setLiveMixCustomLayouts", toCustomLayouts(layouts));
        }

        public override int SetLiveMixAudioBitrate(int bitrate)
        {
            return Engine.Call<int>("setLiveMixAudioBitrate", bitrate);
        }

        public override int SetLiveMixVideoBitrate(int bitrate, bool tiny)
        {
            return Engine.Call<int>("setLiveMixVideoBitrate", bitrate, tiny);
        }

        public override int SetLiveMixVideoResolution(int width, int height, bool tiny)
        {
            return Engine.Call<int>("setLiveMixVideoResolution", width, height, tiny);
        }

        public override int SetLiveMixVideoFps(RCRTCVideoFps fps, bool tiny)
        {
            return Engine.Call<int>("setLiveMixVideoFps", toVideoFps(fps), tiny);
        }

        public override int SetStatsListener(RCRTCStatsListener listener)
        {
            AndroidJavaProxy proxy = listener != null ? new StatsListenerProxy(this, listener) : null;
            return Engine.Call<int>("setStatsListener", proxy);
        }

        public override int CreateAudioEffect(String path, int effectId)
        {
            AndroidJavaObject uri = new AndroidJavaObject("java.net.URI", path);
            return Engine.Call<int>("createAudioEffect", uri, effectId);
        }

        public override int ReleaseAudioEffect(int effectId)
        {
            return Engine.Call<int>("releaseAudioEffect", effectId);
        }

        public override int PlayAudioEffect(int effectId, int volume, int loop)
        {
            return Engine.Call<int>("playAudioEffect", effectId, volume, loop);
        }

        public override int PauseAudioEffect(int effectId)
        {
            return Engine.Call<int>("pauseAudioEffect", effectId);
        }

        public override int PauseAllAudioEffects()
        {
            return Engine.Call<int>("pauseAllAudioEffects");
        }

        public override int ResumeAudioEffect(int effectId)
        {
            return Engine.Call<int>("resumeAudioEffect", effectId);
        }

        public override int ResumeAllAudioEffects()
        {
            return Engine.Call<int>("resumeAllAudioEffects");
        }

        public override int StopAudioEffect(int effectId)
        {
            return Engine.Call<int>("stopAudioEffect", effectId);
        }

        public override int StopAllAudioEffects()
        {
            return Engine.Call<int>("stopAllAudioEffects");
        }

        public override int AdjustAudioEffectVolume(int effectId, int volume)
        {
            return Engine.Call<int>("adjustAudioEffectVolume", effectId, volume);
        }

        public override int GetAudioEffectVolume(int effectId)
        {
            return Engine.Call<int>("getAudioEffectVolume", effectId);
        }

        public override int AdjustAllAudioEffectsVolume(int volume)
        {
            return Engine.Call<int>("adjustAllAudioEffectsVolume", volume);
        }

        public override int StartAudioMixing(String path, RCRTCAudioMixingMode mode, bool playback, int loop)
        {
            AndroidJavaObject uri = new AndroidJavaObject("java.net.URI", path);
            return Engine.Call<int>("startAudioMixing", uri, toAudioMixMode(mode), playback, loop);
        }

        public override int StopAudioMixing()
        {
            return Engine.Call<int>("stopAudioMixing");
        }

        public override int PauseAudioMixing()
        {
            return Engine.Call<int>("pauseAudioMixing");
        }

        public override int ResumeAudioMixing()
        {
            return Engine.Call<int>("resumeAudioMixing");
        }

        public override int AdjustAudioMixingVolume(int volume)
        {
            return Engine.Call<int>("adjustAudioMixingVolume", volume);
        }

        public override int AdjustAudioMixingPlaybackVolume(int volume)
        {
            return Engine.Call<int>("adjustAudioMixingPlaybackVolume", volume);
        }

        public override int AdjustAudioMixingPublishVolume(int volume)
        {
            return Engine.Call<int>("adjustAudioMixingPublishVolume", volume);
        }

        public override int GetAudioMixingPlaybackVolume()
        {
            return Engine.Call<int>("getAudioMixingPlaybackVolume");
        }

        public override int GetAudioMixingPublishVolume()
        {
            return Engine.Call<int>("getAudioMixingPublishVolume");
        }

        public override int SetAudioMixingPosition(double position)
        {
            return Engine.Call<int>("setAudioMixingPosition", position);
        }

        public override double GetAudioMixingPosition()
        {
            return Engine.Call<int>("getAudioMixingPosition");
        }

        public override int GetAudioMixingDuration()
        {
            return Engine.Call<int>("getAudioMixingDuration");
        }

        public override int SetLocalAudioCapturedListener(RCRTCOnWritableAudioFrameListener listener)
        {
            AndroidJavaProxy proxy = listener != null ? new OnWritableAudioFrameListenerProxy(listener) : null;
            return Engine.Call<int>("setLocalAudioCapturedListener", proxy);
        }

        public override int SetRemoteAudioReceivedListener(String userId, RCRTCOnWritableAudioFrameListener listener)
        {
            AndroidJavaProxy proxy = listener != null ? new OnWritableAudioFrameListenerProxy(listener) : null;
            return Engine.Call<int>("setRemoteAudioReceivedListener", userId, proxy);
        }
        
        // public override int SetVideoListener(RCRTCOnVideoFrameListener listener)
        // {
        //     throw new NotImplementedException();
        // }

        public override String GetSessionId()
        {
            AndroidJavaObject jid = Engine.Call<AndroidJavaObject>("getSessionId");
            if (jid != null) return jid.Call<String>("toString");
            return null;
        }

        private AndroidJavaObject toEngineSetup(RCRTCEngineSetup setup)
        {
            if (setup == null) return null;
            AndroidJavaObject jobject = EngineSetupBuilderClass.CallStatic<AndroidJavaObject>("create")
                                        .Call<AndroidJavaObject>("withReconnectable", setup.IsReconnectable())
                                        .Call<AndroidJavaObject>("withStatsReportInterval", setup.GetStatsReportInterval())
                                        .Call<AndroidJavaObject>("withEnableSRTP", setup.IsEnableSRTP())
                                        .Call<AndroidJavaObject>("withAudioSetup", toAudioSetup(setup.GetAudioSetup()))
                                        .Call<AndroidJavaObject>("withVideoSetup", toVideoSetup(setup.GetVideoSetup()))
                                        .Call<AndroidJavaObject>("withEnableVersionMatch", false)
                                        .Call<AndroidJavaObject>("build");
            return jobject;
        }

        private AndroidJavaObject toAudioSetup(RCRTCAudioSetup setup)
        {
            if (setup == null) return null;
            AndroidJavaObject jobject = AudioSetupBuilderClass.CallStatic<AndroidJavaObject>("create")
                                        .Call<AndroidJavaObject>("withAudioCodecType", toAudioCodecType(setup.GetAudioCodecType()))
                                        .Call<AndroidJavaObject>("withAudioSource", setup.GetAudioSource())
                                        .Call<AndroidJavaObject>("withAudioSampleRate", setup.GetAudioSampleRate())
                                        .Call<AndroidJavaObject>("withEnableMicrophone", setup.IsEnableMicrophone())
                                        .Call<AndroidJavaObject>("withEnableStereo", setup.IsEnableStereo())
                                        .Call<AndroidJavaObject>("build");
            return jobject;
        }

        private AndroidJavaObject toVideoSetup(RCRTCVideoSetup setup)
        {
            if (setup == null) return null;
            AndroidJavaObject jobject = VideoSetupBuilderClass.CallStatic<AndroidJavaObject>("create")
                                        .Call<AndroidJavaObject>("withEnableTinyStream", setup.IsEnableTinyStream())
                                        .Call<AndroidJavaObject>("build");
            return jobject;
        }

        private AndroidJavaObject toRoomSetup(RCRTCRoomSetup setup)
        {
            AndroidJavaObject jobject = RoomSetupBuilderClass.CallStatic<AndroidJavaObject>("create")
                                        .Call<AndroidJavaObject>("withRole", toRole(setup.GetRole()))
                                        .Call<AndroidJavaObject>("withMediaType", toMediaType(setup.GetMediaType()))
                                        .Call<AndroidJavaObject>("build");
            return jobject;
        }

        private AndroidJavaObject toAudioConfig(RCRTCAudioConfig config)
        {
            AndroidJavaObject jobject = new AndroidJavaObject(AudioConfigClass);
            jobject.Call<AndroidJavaObject>("setQuality", toAudioQuality(config.GetQuality()));
            jobject.Call<AndroidJavaObject>("setScenario", toAudioScenario(config.GetScenario()));
            return jobject;
        }

        private AndroidJavaObject toVideoConfig(RCRTCVideoConfig config)
        {
            AndroidJavaObject jobject = new AndroidJavaObject(VideoConfigClass);
            jobject.Call<AndroidJavaObject>("setMinBitrate", config.GetMinBitrate());
            jobject.Call<AndroidJavaObject>("setMaxBitrate", config.GetMaxBitrate());
            jobject.Call<AndroidJavaObject>("setFps", toVideoFps(config.GetFps()));
            jobject.Call<AndroidJavaObject>("setResolution", toVideoResolution(config.GetResolution()));
            return jobject;
        }

        private AndroidJavaObject toAudioCodecType(RCRTCAudioCodecType type)
        {
            return AudioCodecTypeClass.GetStatic<AndroidJavaObject>(type.ToString());
        }

        private AndroidJavaObject toAudioQuality(RCRTCAudioQuality quality)
        {
            return AudioQualityClass.GetStatic<AndroidJavaObject>(quality.ToString());
        }

        private AndroidJavaObject toAudioScenario(RCRTCAudioScenario scenario)
        {
            return AudioScenarioClass.GetStatic<AndroidJavaObject>(scenario.ToString());
        }

        private AndroidJavaObject toCamera(RCRTCCamera camera)
        {
            return CameraClass.GetStatic<AndroidJavaObject>(camera.ToString());
        }

        private AndroidJavaObject toMediaType(RCRTCMediaType type)
        {
            return MediaTypeClass.GetStatic<AndroidJavaObject>(type.ToString());
        }

        private AndroidJavaObject toRole(RCRTCRole role)
        {
            return RoleClass.GetStatic<AndroidJavaObject>(role.ToString());
        }

        private AndroidJavaObject toAudioMixMode(RCRTCAudioMixingMode mode)
        {
            return AudioMixModeClass.GetStatic<AndroidJavaObject>(mode.ToString());
        }

        private AndroidJavaObject toVideoFps(RCRTCVideoFps fps)
        {
            return VideoFpsClass.GetStatic<AndroidJavaObject>(fps.ToString());
        }

        private AndroidJavaObject toVideoResolution(RCRTCVideoResolution resolution)
        {
            return VideoResolutionClass.GetStatic<AndroidJavaObject>(resolution.ToString());
        }

        private AndroidJavaObject toCameraCaptureOrientation(RCRTCCameraCaptureOrientation orientation)
        {
            return CameraCaptureOrientationClass.GetStatic<AndroidJavaObject>(orientation.ToString());
        }

        private AndroidJavaObject toLiveMixLayoutMode(RCRTCLiveMixLayoutMode mode)
        {
            return LiveMixLayoutModeClass.GetStatic<AndroidJavaObject>(mode.ToString());
        }

        private AndroidJavaObject toLiveMixRenderMode(RCRTCLiveMixRenderMode mode)
        {
            return LiveMixRenderModeClass.GetStatic<AndroidJavaObject>(mode.ToString());
        }

        private AndroidJavaObject toCustomLayout(RCRTCCustomLayout layout)
        {
            AndroidJavaObject jobject = null;
            if (layout.GetTag() != null)
            {
                jobject = new AndroidJavaObject(CustomLayoutClassFullName, layout.GetUserId(), layout.GetTag());
            }
            else
            {
                jobject = new AndroidJavaObject(CustomLayoutClassFullName, layout.GetUserId());
            }

            jobject.Call("setX", layout.GetX());
            jobject.Call("setY", layout.GetY());
            jobject.Call("setWidth", layout.GetWidth());
            jobject.Call("setHeight", layout.GetHeight());
            return jobject;
        }

        private AndroidJavaObject toCustomLayouts(IList<RCRTCCustomLayout> layouts)
        {
            var arrayList = new AndroidJavaObject("java.util.ArrayList");
            for (int i = 0; i < layouts.Count; ++i)
            {
                arrayList.Call<bool>("add", toCustomLayout(layouts[i]));
            }

            return arrayList;
        }

        private AndroidJavaObject toStringList(IList<String> stringList)
        {
            var arrayList = new AndroidJavaObject("java.util.ArrayList");
            for (int i = 0; i < stringList.Count; ++i)
            {
                arrayList.Call<bool>("add", stringList[i]);
            }

            return arrayList;
        }

        private AndroidJavaObject toStringArray(IList<String> strings)
        {
            AndroidJavaClass jclass = new AndroidJavaClass("java.lang.reflect.Array");
            AndroidJavaObject jobject = jclass.CallStatic<AndroidJavaObject>("newInstance", new AndroidJavaClass("java.lang.String"), strings.Count);
            for (int i = 0; i < strings.Count; ++i)
            {
                jclass.CallStatic("set", jobject, i, new AndroidJavaObject("java.lang.String", strings[i]));
            }
            return jobject;
        }

        private RCRTCMediaType toMediaType(AndroidJavaObject jtype)
        {
            int type = jtype.Call<int>("getType");
            return (RCRTCMediaType)type;
        }

        private RCRTCCamera toCamera(AndroidJavaObject jcamera)
        {
            int camera = jcamera.Call<int>("getCamera");
            return (RCRTCCamera)camera;
        }

        private RCRTCNetworkType toNetworkType(AndroidJavaObject jtype)
        {
            int type = jtype.Call<int>("getType");
            return (RCRTCNetworkType)type;
        }

        private RCRTCAudioCodecType toAudioCodecType(AndroidJavaObject jtype)
        {
            int type = jtype.Call<int>("getType");
            return (RCRTCAudioCodecType)type;
        }

        private RCRTCVideoCodecType toVideoCodecType(AndroidJavaObject jtype)
        {
            int type = jtype.Call<int>("getType");
            return (RCRTCVideoCodecType)type;
        }

        private RCRTCLocalAudioStats toLocalAudioStats(AndroidJavaObject jstats)
        {
            AndroidJavaObject jcodec = jstats.Call<AndroidJavaObject>("getCodec");
            int bitrate = jstats.Call<int>("getBitrate");
            int volume = jstats.Call<int>("getVolume");
            double packageLostRate = jstats.Call<double>("getPackageLostRate");
            int rtt = jstats.Call<int>("getRtt");
            return new RCRTCLocalAudioStats(toAudioCodecType(jcodec), bitrate, volume, packageLostRate, rtt);
        }

        private RCRTCLocalVideoStats toLocalVideoStats(AndroidJavaObject jstats)
        {
            bool tiny = jstats.Call<bool>("isTiny");
            AndroidJavaObject jcodec = jstats.Call<AndroidJavaObject>("getCodec");
            int bitrate = jstats.Call<int>("getBitrate");
            int fps = jstats.Call<int>("getFps");
            int width = jstats.Call<int>("getWidth");
            int height = jstats.Call<int>("getHeight");
            double packageLostRate = jstats.Call<double>("getPackageLostRate");
            int rtt = jstats.Call<int>("getRtt");
            return new RCRTCLocalVideoStats(tiny, toVideoCodecType(jcodec), bitrate, fps, width, height, packageLostRate, rtt);
        }

        private RCRTCRemoteAudioStats toRemoteAudioStats(AndroidJavaObject jstats)
        {
            AndroidJavaObject jcodec = jstats.Call<AndroidJavaObject>("getCodec");
            int bitrate = jstats.Call<int>("getBitrate");
            int volume = jstats.Call<int>("getVolume");
            double packageLostRate = jstats.Call<double>("getPackageLostRate");
            int rtt = jstats.Call<int>("getRtt");
            return new RCRTCRemoteAudioStats(toAudioCodecType(jcodec), bitrate, volume, packageLostRate, rtt);
        }

        private RCRTCRemoteVideoStats toRemoteVideoStats(AndroidJavaObject jstats)
        {
            AndroidJavaObject jcodec = jstats.Call<AndroidJavaObject>("getCodec");
            int bitrate = jstats.Call<int>("getBitrate");
            int fps = jstats.Call<int>("getFps");
            int width = jstats.Call<int>("getWidth");
            int height = jstats.Call<int>("getHeight");
            double packageLostRate = jstats.Call<double>("getPackageLostRate");
            int rtt = jstats.Call<int>("getRtt");
            return new RCRTCRemoteVideoStats(toVideoCodecType(jcodec), bitrate, fps, width, height, packageLostRate, rtt);
        }

        private class ListenerProxy : AndroidJavaProxy
        {
            public ListenerProxy(RCRTCEngineAndroid engine) : base("cn.rongcloud.rtc.wrapper.listener.IRCRTCIWListener")
            {
                this.engine = engine;
            }

            public void onError(int code, String message)
            {
                engine.OnError?.Invoke(code, message);
            }

            public void onKicked(String roomId, String message)
            {
                engine.OnKicked?.Invoke(roomId, message);
            }

            public void onRoomJoined(int code, String message)
            {
                engine.OnRoomJoined?.Invoke(code, message);
            }

            public void onRoomLeft(int code, String message)
            {
                engine.OnRoomLeft?.Invoke(code, message);
            }

            public void onPublished(AndroidJavaObject jtype, int code, String message)
            {
                engine.OnPublished?.Invoke(engine.toMediaType(jtype), code, message);
            }

            public void onUnpublished(AndroidJavaObject jtype, int code, String message)
            {
                engine.OnUnpublished?.Invoke(engine.toMediaType(jtype), code, message);
            }

            public void onSubscribed(String userId, AndroidJavaObject jtype, int code, String message)
            {
                engine.OnSubscribed?.Invoke(userId, engine.toMediaType(jtype), code, message);
            }

            public void onUnsubscribed(String userId, AndroidJavaObject jtype, int code, String message)
            {
                engine.OnUnsubscribed?.Invoke(userId, engine.toMediaType(jtype), code, message);
            }

            public void onLiveMixSubscribed(AndroidJavaObject jtype, int code, String message)
            {
                engine.OnLiveMixSubscribed?.Invoke(engine.toMediaType(jtype), code, message);
            }

            public void onLiveMixUnsubscribed(AndroidJavaObject jtype, int code, String message)
            {
                engine.OnLiveMixUnsubscribed?.Invoke(engine.toMediaType(jtype), code, message);
            }

            public void onCameraEnabled(bool enable, int code, String message)
            {
                engine.OnCameraEnabled?.Invoke(enable, code, message);
            }

            public void onCameraSwitched(AndroidJavaObject jcamera, int code, String message)
            {
                engine.OnCameraSwitched?.Invoke(engine.toCamera(jcamera), code, message);
            }

            public void onLiveCdnAdded(String url, int code, String message)
            {
                engine.OnLiveCdnAdded?.Invoke(url, code, message);
            }

            public void onLiveCdnRemoved(String url, int code, String message)
            {
                engine.OnLiveCdnRemoved?.Invoke(url, code, message);
            }

            public void onLiveMixLayoutModeSet(int code, String message)
            {
                engine.OnLiveMixLayoutModeSet?.Invoke(code, message);
            }

            public void onLiveMixRenderModeSet(int code, String message)
            {
                engine.OnLiveMixRenderModeSet?.Invoke(code, message);
            }

            public void onLiveMixCustomLayoutsSet(int code, String message)
            {
                engine?.OnLiveMixCustomLayoutsSet?.Invoke(code, message);
            }

            public void onLiveMixCustomAudiosSet(int code, String message)
            {
                engine.OnLiveMixCustomAudiosSet?.Invoke(code, message);
            }

            public void onLiveMixAudioBitrateSet(int code, String message)
            {
                engine.OnLiveMixAudioBitrateSet?.Invoke(code, message);
            }

            public void onLiveMixVideoBitrateSet(bool tiny, int code, String message)
            {
                engine.OnLiveMixVideoBitrateSet?.Invoke(tiny, code, message);
            }

            public void onLiveMixVideoResolutionSet(bool tiny, int code, String message)
            {
                engine.OnLiveMixVideoResolutionSet?.Invoke(tiny, code, message);
            }

            public void onLiveMixVideoFpsSet(bool tiny, int code, String message)
            {
                engine.OnLiveMixVideoFpsSet?.Invoke(tiny, code, message);
            }

            public void onAudioEffectCreated(int effectId, int code, String message)
            {
                engine.OnAudioEffectCreated?.Invoke(effectId, code, message);
            }

            public void onAudioEffectFinished(int effectId)
            {
                engine.OnAudioEffectFinished?.Invoke(effectId);
            }

            public void onAudioMixingStarted()
            {
                engine.OnAudioMixingStarted?.Invoke();
            }

            public void onAudioMixingPaused()
            {
                engine.OnAudioMixingPaused?.Invoke();
            }

            public void onAudioMixingStopped()
            {
                engine.OnAudioMixingStopped?.Invoke();
            }

            public void onAudioMixingFinished()
            {
                engine.OnAudioMixingFinished?.Invoke();
            }

            public void onUserJoined(String roomId, String userId)
            {
                engine.OnUserJoined?.Invoke(userId);
            }

            public void onUserOffline(String roomId, String userId)
            {
                engine.OnUserOffline?.Invoke(userId);
            }

            public void onUserLeft(String roomId, String userId)
            {
                engine.OnUserLeft?.Invoke(userId);
            }

            public void onRemotePublished(String roomId, String userId, AndroidJavaObject jtype)
            {
                engine.OnRemotePublished?.Invoke(userId, engine.toMediaType(jtype));
            }

            public void onRemoteUnpublished(String roomId, String userId, AndroidJavaObject jtype)
            {
                engine.OnRemoteUnpublished?.Invoke(userId, engine.toMediaType(jtype));
            }

            public void onRemoteLiveMixPublished(AndroidJavaObject jtype)
            {
                engine.OnRemoteLiveMixPublished?.Invoke(engine.toMediaType(jtype));
            }

            public void onRemoteLiveMixUnpublished(AndroidJavaObject jtype)
            {
                engine.OnRemoteLiveMixUnpublished?.Invoke(engine.toMediaType(jtype));
            }

            public void onRemoteStateChanged(String roomId, String userId, AndroidJavaObject jtype, bool disabled)
            {
                engine.OnRemoteStateChanged?.Invoke(userId, engine.toMediaType(jtype), disabled);
            }

            public void onRemoteFirstFrame(String roomId, String userId, AndroidJavaObject jtype)
            {
                engine.OnRemoteFirstFrame?.Invoke(userId, engine.toMediaType(jtype));
            }

            public void onRemoteLiveMixFirstFrame(AndroidJavaObject jtype)
            {
                engine.OnRemoteLiveMixFirstFrame?.Invoke(engine.toMediaType(jtype));
            }

            public void onMessageReceived(String roomId, AndroidJavaObject jmessage) 
            {
            }

            private RCRTCEngineAndroid engine;

        }

        private class StatsListenerProxy : AndroidJavaProxy
        {
            public StatsListenerProxy(RCRTCEngineAndroid engine, RCRTCStatsListener listener) : base("cn.rongcloud.rtc.wrapper.listener.IRCRTCIWStatsListener")
            {
                this.engine = engine;
                this.listener = listener;
            }

            public void onNetworkStats(AndroidJavaObject jstats)
            {
                AndroidJavaObject jtype = jstats.Call<AndroidJavaObject>("getType");
                String ip = jstats.Call<String>("getIp");
                int sendBitrate = jstats.Call<int>("getSendBitrate");
                int receiveBitrate = jstats.Call<int>("getReceiveBitrate");
                int rtt = jstats.Call<int>("getRtt");
                RCRTCNetworkStats stats = new RCRTCNetworkStats(engine.toNetworkType(jtype), ip, sendBitrate, receiveBitrate, rtt);
                listener.OnNetworkStats(stats);
            }

            public void onLocalAudioStats(AndroidJavaObject jstats)
            {
                listener.OnLocalAudioStats(engine.toLocalAudioStats(jstats));
            }

            public void onLocalVideoStats(AndroidJavaObject jstats)
            {
                listener.OnLocalVideoStats(engine.toLocalVideoStats(jstats));
            }

            public void onRemoteAudioStats(String roomId, String userId, AndroidJavaObject jstats)
            {
                listener.OnRemoteAudioStats(userId, engine.toRemoteAudioStats(jstats));
            }

            public void onRemoteVideoStats(String roomId, String userId, AndroidJavaObject jstats)
            {
                listener.OnRemoteVideoStats(userId, engine.toRemoteVideoStats(jstats));
            }

            public void onLiveMixAudioStats(AndroidJavaObject jstats)
            {
                listener.OnLiveMixAudioStats(engine.toRemoteAudioStats(jstats));
            }

            public void onLiveMixVideoStats(AndroidJavaObject jstats)
            {
                listener.OnLiveMixVideoStats(engine.toRemoteVideoStats(jstats));
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

            private RCRTCEngineAndroid engine;
            private RCRTCStatsListener listener;
        }

        private class OnWritableAudioFrameListenerProxy : AndroidJavaProxy
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

        private static AndroidJavaClass EngineClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.RCRTCIWEngineImpl");
        private static AndroidJavaClass EngineSetupBuilderClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.setup.RCRTCIWEngineSetup$Builder");
        private static AndroidJavaClass AudioSetupBuilderClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.setup.RCRTCIWAudioSetup$Builder");
        private static AndroidJavaClass VideoSetupBuilderClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.setup.RCRTCIWVideoSetup$Builder");
        private static AndroidJavaClass RoomSetupBuilderClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.setup.RCRTCIWRoomSetup$Builder");

        private const String AudioConfigClass = "cn.rongcloud.rtc.wrapper.config.RCRTCIWAudioConfig";
        private const String VideoConfigClass = "cn.rongcloud.rtc.wrapper.config.RCRTCIWVideoConfig";

        private static AndroidJavaClass AudioCodecTypeClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWAudioCodecType");
        private static AndroidJavaClass AudioQualityClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWAudioQuality");
        private static AndroidJavaClass AudioScenarioClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWAudioScenario");
        private static AndroidJavaClass CameraClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWCamera");
        private static AndroidJavaClass MediaTypeClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWMediaType");
        private static AndroidJavaClass RoleClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWRole");
        private static AndroidJavaClass AudioMixModeClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWAudioMixingMode");
        private static AndroidJavaClass VideoFpsClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWVideoFps");
        private static AndroidJavaClass VideoResolutionClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWVideoResolution");
        private static AndroidJavaClass CameraCaptureOrientationClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWCameraCaptureOrientation");
        private static AndroidJavaClass LiveMixLayoutModeClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWLiveMixLayoutMode");
        private static AndroidJavaClass LiveMixRenderModeClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWLiveMixRenderMode");
        private const String CustomLayoutClassFullName = "cn.rongcloud.rtc.wrapper.module.RCRTCIWCustomLayout";
        private static AndroidJavaClass CustomLayoutClass = new AndroidJavaClass(CustomLayoutClassFullName);

        private AndroidJavaObject Engine = null;

    }

}

#endif