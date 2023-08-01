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
        private static AndroidJavaClass EngineClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.RCRTCIWEngineImpl");
        private static AndroidJavaClass UtilClass = new AndroidJavaClass("cn.rongcloud.rtc.unity.Util");
        private AndroidJavaObject Engine = null;
    
        internal RCRTCEngineAndroid(RCRTCEngineSetup setup)
        {
            AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = player.GetStatic<AndroidJavaObject>("currentActivity");
            Engine =
                EngineClass.CallStatic<AndroidJavaObject>("newInstance", context, ArgumentAdapter.toEngineSetup(setup));
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
            return Engine.Call<int>("joinRoom", roomId, ArgumentAdapter.toRoomSetup(setup));
        }
    
        public override int LeaveRoom()
        {
            return Engine.Call<int>("leaveRoom");
        }
    
        public override int Publish(RCRTCMediaType type)
        {
            return Engine.Call<int>("publish", ArgumentAdapter.toMediaType(type));
        }
    
        public override int Unpublish(RCRTCMediaType type)
        {
            return Engine.Call<int>("unpublish", ArgumentAdapter.toMediaType(type));
        }
    
        public override int Subscribe(String userId, RCRTCMediaType type, bool tiny)
        {
            return Engine.Call<int>("subscribe", userId, ArgumentAdapter.toMediaType(type), tiny);
        }
    
        public override int Subscribe(IList<String> userIds, RCRTCMediaType type, bool tiny)
        {
            return Engine.Call<int>("subscribe", ArgumentAdapter.toStringArray(userIds), ArgumentAdapter.toMediaType(type),
                                    tiny);
        }
    
        public override int SubscribeLiveMix(RCRTCMediaType type, bool tiny)
        {
            return Engine.Call<int>("subscribeLiveMix", ArgumentAdapter.toMediaType(type), tiny);
        }
    
        public override int Unsubscribe(String userId, RCRTCMediaType type)
        {
            return Engine.Call<int>("unsubscribe", userId, ArgumentAdapter.toMediaType(type));
        }
    
        public override int Unsubscribe(IList<String> userIds, RCRTCMediaType type)
        {
            return Engine.Call<int>("unsubscribe", ArgumentAdapter.toStringArray(userIds),
                                    ArgumentAdapter.toMediaType(type));
        }
    
        public override int UnsubscribeLiveMix(RCRTCMediaType type)
        {
            return Engine.Call<int>("unsubscribeLiveMix", ArgumentAdapter.toMediaType(type));
        }
    
        public override int SetAudioConfig(RCRTCAudioConfig config)
        {
            return Engine.Call<int>("setAudioConfig", ArgumentAdapter.toAudioConfig(config));
        }
    
        public override int SetVideoConfig(RCRTCVideoConfig config, bool tiny)
        {
            return Engine.Call<int>("setVideoConfig", ArgumentAdapter.toVideoConfig(config), tiny);
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
            return Engine.Call<int>("enableCamera", enable, ArgumentAdapter.toCamera(camera));
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
            return Engine.Call<int>("setCameraCaptureOrientation", ArgumentAdapter.toCameraCaptureOrientation(orientation));
        }
    
        public override int AdjustLocalVolume(int volume)
        {
            return Engine.Call<int>("adjustLocalVolume", volume);
        }
    
        public override int MuteLocalStream(RCRTCMediaType type, bool mute)
        {
            return Engine.Call<int>("muteLocalStream", ArgumentAdapter.toMediaType(type), mute);
        }
    
        public override int MuteRemoteStream(String userId, RCRTCMediaType type, bool mute)
        {
            return Engine.Call<int>("muteRemoteStream", userId, ArgumentAdapter.toMediaType(type), mute);
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
            return (AndroidJavaObject)field.GetValue(view);
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
    
        public override int SetLiveMixInnerCdnStreamView(RCRTCView view)
        {
            return Engine.Call<int>("setLiveMixInnerCdnStreamBaseView", toNativeView(view));
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
            return Engine.Call<int>("setLiveMixLayoutMode", ArgumentAdapter.toLiveMixLayoutMode(mode));
        }
    
        public override int SetLiveMixRenderMode(RCRTCLiveMixRenderMode mode)
        {
            return Engine.Call<int>("setLiveMixRenderMode", ArgumentAdapter.toLiveMixRenderMode(mode));
        }
    
        public override int SetLiveMixCustomAudios(IList<String> userIds)
        {
            if (userIds == null || userIds.Count == 0)
                return -1;
            return Engine.Call<int>("setLiveMixCustomAudios", ArgumentAdapter.toStringList(userIds));
        }
    
        public override int SetLiveMixCustomLayouts(IList<RCRTCCustomLayout> layouts)
        {
            if (layouts == null || layouts.Count == 0)
                return -1;
            return Engine.Call<int>("setLiveMixCustomLayouts", ArgumentAdapter.toCustomLayouts(layouts));
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
            return Engine.Call<int>("setLiveMixVideoFps", ArgumentAdapter.toVideoFps(fps), tiny);
        }
    
        public override int SetStatsListener(RCRTCStatsListener listener)
        {
            AndroidJavaProxy proxy = listener != null ? new StatsListenerProxy(listener) : null;
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
            return Engine.Call<int>("startAudioMixing", uri, ArgumentAdapter.toAudioMixMode(mode), playback, loop);
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
            if (jid != null)
                return jid.Call<String>("toString");
            return null;
        }
    
        public override int SetLocalCustomStreamView(string tag, RCRTCView view)
        {
            RCUnityLogger.getInstance().log("SetLocalCustomStreamView", "");
            int ret = Engine.Call<int>("setLocalCustomStreamBaseView", tag, toNativeView(view));
            return ret;
        }
    
        public override int SetRemoteCustomStreamView(string userId, string tag, RCRTCView view)
        {
            RCUnityLogger.getInstance().log("SetRemoteCustomStreamView", $"userId={userId} tag={tag}");
            int ret = Engine.Call<int>("setRemoteCustomStreamBaseView", userId, tag, toNativeView(view));
            return ret;
        }
    
        public override int StartNetworkProbe(RCRTCNetworkProbeListener listener)
        {
            RCUnityLogger.getInstance().log("StartNetworkProbe", "");
            int ret = Engine.Call<int>("startNetworkProbe", new RCRTCNetworkProbeListenerProxy(listener));
            return ret;
        }
    
        public override int SetWatermark(string path, double x, double y, double zoom)
        {
            RCUnityLogger.getInstance().log("SetWatermark", $"path={path} x={x} y={y} zoom={zoom}");
            AndroidJavaObject bitmap = UtilClass.CallStatic<AndroidJavaObject>("loadBitMap", path);
            AndroidJavaObject point = new AndroidJavaObject("android.graphics.PointF", (float)x, (float)y);
            int ret = Engine.Call<int>("setWatermark", bitmap, point, (float)zoom);
            return ret;
        }
    
        public override int CreateCustomStreamFromFile(string path, string tag, bool replace, bool playback)
        {
            RCUnityLogger.getInstance().log("createCustomStreamFromFile",
                                            $"path={path} tag={tag} replace={replace} playback={playback}");
            AndroidJavaObject uri = UtilClass.CallStatic<AndroidJavaObject>("toUri", path);
            int ret = Engine.Call<int>("createCustomStreamFromFile", uri, tag, replace, playback);
            return ret;
        }
    
        public override int SetCustomStreamVideoConfig(string tag, RCRTCVideoConfig config)
        {
            RCUnityLogger.getInstance().log("SetCustomStreamVideoConfig", $"tag={tag}");
            int ret = Engine.Call<int>("setCustomStreamVideoConfig", tag, ArgumentAdapter.toVideoConfig(config));
            return ret;
        }
    
        public override int MuteLiveMixStream(RCRTCMediaType type, bool mute)
        {
            RCUnityLogger.getInstance().log("MuteLiveMixStream", $"type={type},mute={mute}");
            AndroidJavaObject _type = new MediaTypeConverter(type).getAndroidObject();
            int ret = Engine.Call<int>("muteLiveMixStream", _type, mute);
            return ret;
        }
    
        public override int SetLiveMixBackgroundColor(int color)
        {
            RCUnityLogger.getInstance().log("SetLiveMixBackgroundColor", $"color={color}");
            int ret = Engine.Call<int>("setLiveMixBackgroundColor", color);
            return ret;
        }
    
        public override int MuteLocalCustomStream(string tag, bool mute)
        {
            RCUnityLogger.getInstance().log("MuteLocalCustomStream", $"tag={tag},mute={mute}");
            int ret = Engine.Call<int>("muteLocalCustomStream", tag, mute);
            return ret;
        }
    
        public override int RemoveLocalCustomStreamView(string tag)
        {
            RCUnityLogger.getInstance().log("RemoveLocalCustomStreamView", $"tag={tag}");
            int ret = Engine.Call<int>("removeLocalCustomStreamView", tag);
            return ret;
        }
    
        public override int PublishCustomStream(string tag)
        {
            RCUnityLogger.getInstance().log("PublishCustomStream", $"tag={tag}");
            int ret = Engine.Call<int>("publishCustomStream", tag);
            return ret;
        }
    
        public override int UnpublishCustomStream(string tag)
        {
            RCUnityLogger.getInstance().log("UnpublishCustomStream", $"tag={tag}");
            int ret = Engine.Call<int>("unpublishCustomStream", tag);
            return ret;
        }
    
        public override int MuteRemoteCustomStream(string userId, string tag, RCRTCMediaType type, bool mute)
        {
            RCUnityLogger.getInstance().log("MuteRemoteCustomStream", $"userId={userId},tag={tag},type={type},mute={mute}");
            AndroidJavaObject _type = new MediaTypeConverter(type).getAndroidObject();
            int ret = Engine.Call<int>("muteRemoteCustomStream", userId, tag, _type, mute);
            return ret;
        }
    
        public override int RemoveRemoteCustomStreamView(string userId, string tag)
        {
            RCUnityLogger.getInstance().log("RemoveRemoteCustomStreamView", $"userId={userId},tag={tag}");
            int ret = Engine.Call<int>("removeRemoteCustomStreamView", userId, tag);
            return ret;
        }
    
        public override int SubscribeCustomStream(string userId, string tag, RCRTCMediaType type, bool tiny)
        {
            RCUnityLogger.getInstance().log("SubscribeCustomStream", $"userId={userId},tag={tag},type={type},tiny={tiny}");
            AndroidJavaObject _type = new MediaTypeConverter(type).getAndroidObject();
            int ret = Engine.Call<int>("subscribeCustomStream", userId, tag, _type, tiny);
            return ret;
        }
    
        public override int UnsubscribeCustomStream(string userId, string tag, RCRTCMediaType type)
        {
            RCUnityLogger.getInstance().log("UnsubscribeCustomStream", $"userId={userId},tag={tag},type={type}");
            AndroidJavaObject _type = new MediaTypeConverter(type).getAndroidObject();
            int ret = Engine.Call<int>("unsubscribeCustomStream", userId, tag, _type);
            return ret;
        }
    
        public override int RequestJoinSubRoom(string roomId, string userId, bool autoLayout, string extra)
        {
            RCUnityLogger.getInstance().log("RequestJoinSubRoom",
                                            $"roomId={roomId},userId={userId},autoLayout={autoLayout},extra={extra}");
            int ret = Engine.Call<int>("requestJoinSubRoom", roomId, userId, autoLayout, extra);
            return ret;
        }
    
        public override int CancelJoinSubRoomRequest(string roomId, string userId, string extra)
        {
            RCUnityLogger.getInstance().log("CancelJoinSubRoomRequest", $"roomId={roomId},userId={userId},extra={extra}");
            int ret = Engine.Call<int>("cancelJoinSubRoomRequest", roomId, userId, extra);
            return ret;
        }
    
        public override int ResponseJoinSubRoomRequest(string roomId, string userId, bool agree, bool autoLayout,
                                                       string extra)
        {
            RCUnityLogger.getInstance().log(
                "ResponseJoinSubRoomRequest",
                $"roomId={roomId},userId={userId},agree={agree},autoLayout={autoLayout},extra={extra}");
            int ret = Engine.Call<int>("responseJoinSubRoomRequest", roomId, userId, agree, autoLayout, extra);
            return ret;
        }
    
        public override int JoinSubRoom(string roomId)
        {
            RCUnityLogger.getInstance().log("JoinSubRoom", $"roomId={roomId}");
            int ret = Engine.Call<int>("joinSubRoom", roomId);
            return ret;
        }
    
        public override int LeaveSubRoom(string roomId, bool disband)
        {
            RCUnityLogger.getInstance().log("LeaveSubRoom", $"roomId={roomId},disband={disband}");
            int ret = Engine.Call<int>("leaveSubRoom", roomId, disband);
            return ret;
        }
    
        public override int SwitchLiveRole(RCRTCRole role)
        {
            RCUnityLogger.getInstance().log("SwitchLiveRole", $"role={role}");
            AndroidJavaObject _role = new RoleConverter(role).getAndroidObject();
            int ret = Engine.Call<int>("switchLiveRole", _role);
            return ret;
        }
    
        public override int EnableLiveMixInnerCdnStream(bool enable)
        {
            RCUnityLogger.getInstance().log("EnableLiveMixInnerCdnStream", $"enable={enable}");
            int ret = Engine.Call<int>("enableLiveMixInnerCdnStream", enable);
            return ret;
        }
    
        public override int SubscribeLiveMixInnerCdnStream()
        {
            RCUnityLogger.getInstance().log("SubscribeLiveMixInnerCdnStream", $"");
            int ret = Engine.Call<int>("subscribeLiveMixInnerCdnStream");
            return ret;
        }
    
        public override int UnsubscribeLiveMixInnerCdnStream()
        {
            RCUnityLogger.getInstance().log("UnsubscribeLiveMixInnerCdnStream", $"");
            int ret = Engine.Call<int>("unsubscribeLiveMixInnerCdnStream");
            return ret;
        }
    
        public override int RemoveLiveMixInnerCdnStreamView()
        {
            RCUnityLogger.getInstance().log("RemoveLiveMixInnerCdnStreamView", $"");
            int ret = Engine.Call<int>("removeLiveMixInnerCdnStreamView");
            return ret;
        }
    
        public override int SetLocalLiveMixInnerCdnVideoResolution(int width, int height)
        {
            RCUnityLogger.getInstance().log("SetLocalLiveMixInnerCdnVideoResolution", $"width={width},height={height}");
            int ret = Engine.Call<int>("setLocalLiveMixInnerCdnVideoResolution", width, height);
            return ret;
        }
    
        public override int SetLocalLiveMixInnerCdnVideoFps(RCRTCVideoFps fps)
        {
            RCUnityLogger.getInstance().log("SetLocalLiveMixInnerCdnVideoFps", $"fps={fps}");
            AndroidJavaObject _fps = new VideoFpsConverter(fps).getAndroidObject();
            int ret = Engine.Call<int>("setLocalLiveMixInnerCdnVideoFps", _fps);
            return ret;
        }
    
        public override int MuteLiveMixInnerCdnStream(bool mute)
        {
            RCUnityLogger.getInstance().log("MuteLiveMixInnerCdnStream", $"mute={mute}");
            int ret = Engine.Call<int>("muteLiveMixInnerCdnStream", mute);
            return ret;
        }
    
        public override int RemoveWatermark()
        {
            RCUnityLogger.getInstance().log("RemoveWatermark", $"");
            int ret = Engine.Call<int>("removeWatermark");
            return ret;
        }
    
        public override int StopNetworkProbe()
        {
            RCUnityLogger.getInstance().log("StopNetworkProbe", $"");
            int ret = Engine.Call<int>("stopNetworkProbe");
            return ret;
        }
    
        public override int StartEchoTest(int timeInterval)
        {
            RCUnityLogger.getInstance().log("StartEchoTest", $"timeInterval={timeInterval}");
            int ret = Engine.Call<int>("startEchoTest", timeInterval);
            return ret;
        }
    
        public override int StopEchoTest()
        {
            RCUnityLogger.getInstance().log("StopEchoTest", $"");
            int ret = Engine.Call<int>("stopEchoTest");
            return ret;
        }
    
        public override int EnableSei(bool enable)
        {
            RCUnityLogger.getInstance().log("EnableSei", $"enable={enable}");
            int ret = Engine.Call<int>("enableSei", enable);
            return ret;
        }
    
        public override int SendSei(string sei)
        {
            RCUnityLogger.getInstance().log("SendSei", $"sei={sei}");
            int ret = Engine.Call<int>("sendSei", sei);
            return ret;
        }
    
        public override int PreconnectToMediaServer()
        {
            RCUnityLogger.getInstance().log("PreconnectToMediaServer", $"");
            int ret = Engine.Call<int>("preconnectToMediaServer");
            return ret;
        }
    
        private class ListenerProxy : AndroidJavaProxy
        {
            private RCRTCEngineAndroid engine;
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
                engine.OnPublished?.Invoke(ArgumentAdapter.toMediaType(jtype), code, message);
            }
    
            public void onUnpublished(AndroidJavaObject jtype, int code, String message)
            {
                engine.OnUnpublished?.Invoke(ArgumentAdapter.toMediaType(jtype), code, message);
            }
    
            public void onSubscribed(String userId, AndroidJavaObject jtype, int code, String message)
            {
                engine.OnSubscribed?.Invoke(userId, ArgumentAdapter.toMediaType(jtype), code, message);
            }
    
            public void onUnsubscribed(String userId, AndroidJavaObject jtype, int code, String message)
            {
                engine.OnUnsubscribed?.Invoke(userId, ArgumentAdapter.toMediaType(jtype), code, message);
            }
    
            public void onLiveMixSubscribed(AndroidJavaObject jtype, int code, String message)
            {
                engine.OnLiveMixSubscribed?.Invoke(ArgumentAdapter.toMediaType(jtype), code, message);
            }
    
            public void onLiveMixUnsubscribed(AndroidJavaObject jtype, int code, String message)
            {
                engine.OnLiveMixUnsubscribed?.Invoke(ArgumentAdapter.toMediaType(jtype), code, message);
            }
    
            public void onCameraEnabled(bool enable, int code, String message)
            {
                engine.OnCameraEnabled?.Invoke(enable, code, message);
            }
    
            public void onCameraSwitched(AndroidJavaObject jcamera, int code, String message)
            {
                engine.OnCameraSwitched?.Invoke(ArgumentAdapter.toCamera(jcamera), code, message);
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
                engine.OnUserJoined?.Invoke(roomId, userId);
            }
    
            public void onUserOffline(String roomId, String userId)
            {
                engine.OnUserOffline?.Invoke(roomId, userId);
            }
    
            public void onUserLeft(String roomId, String userId)
            {
                engine.OnUserLeft?.Invoke(roomId, userId);
            }
    
            public void onRemotePublished(String roomId, String userId, AndroidJavaObject jtype)
            {
                engine.OnRemotePublished?.Invoke(roomId, userId, ArgumentAdapter.toMediaType(jtype));
            }
    
            public void onRemoteUnpublished(String roomId, String userId, AndroidJavaObject jtype)
            {
                engine.OnRemoteUnpublished?.Invoke(roomId, userId, ArgumentAdapter.toMediaType(jtype));
            }
    
            public void onRemoteLiveMixPublished(AndroidJavaObject jtype)
            {
                engine.OnRemoteLiveMixPublished?.Invoke(ArgumentAdapter.toMediaType(jtype));
            }
    
            public void onRemoteLiveMixUnpublished(AndroidJavaObject jtype)
            {
                engine.OnRemoteLiveMixUnpublished?.Invoke(ArgumentAdapter.toMediaType(jtype));
            }
    
            public void onRemoteStateChanged(String roomId, String userId, AndroidJavaObject jtype, bool disabled)
            {
                engine.OnRemoteStateChanged?.Invoke(roomId, userId, ArgumentAdapter.toMediaType(jtype), disabled);
            }
    
            public void onRemoteFirstFrame(String roomId, String userId, AndroidJavaObject jtype)
            {
                engine.OnRemoteFirstFrame?.Invoke(roomId, userId, ArgumentAdapter.toMediaType(jtype));
            }
    
            public void onRemoteLiveMixFirstFrame(AndroidJavaObject jtype)
            {
                engine.OnRemoteLiveMixFirstFrame?.Invoke(ArgumentAdapter.toMediaType(jtype));
            }
    
            public void onMessageReceived(String roomId, AndroidJavaObject jmessage)
            {
            }
    
            public void onLiveMixBackgroundColorSet(int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnLiveMixBackgroundColorSet", $"code={code},errMsg={errMsg}");
                engine.OnLiveMixBackgroundColorSet?.Invoke(code, errMsg);
            }
    
            public void onAudioMixingProgressReported(float progress)
            {
                RCUnityLogger.getInstance().log("OnAudioMixingProgressReported", $"progress={progress}");
                engine.OnAudioMixingProgressReported?.Invoke(progress);
            }
    
            public void onCustomStreamPublished(string tag, int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnCustomStreamPublished", $"tag={tag},code={code},errMsg={errMsg}");
                engine.OnCustomStreamPublished?.Invoke(tag, code, errMsg);
            }
    
            public void onCustomStreamPublishFinished(string tag)
            {
                RCUnityLogger.getInstance().log("OnCustomStreamPublishFinished", $"tag={tag}");
                engine.OnCustomStreamPublishFinished?.Invoke(tag);
            }
    
            public void onCustomStreamUnpublished(string tag, int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnCustomStreamUnpublished", $"tag={tag},code={code},errMsg={errMsg}");
                engine.OnCustomStreamUnpublished?.Invoke(tag, code, errMsg);
            }
    
            public void onRemoteCustomStreamPublished(string roomId, string userId, string tag, AndroidJavaObject type)
            {
                RCRTCMediaType _type = new MediaTypeConverter(type).getCSharpObject();
                RCUnityLogger.getInstance().log("OnRemoteCustomStreamPublished",
                                                $"roomId={roomId},userId={userId},tag={tag},type={_type}");
                engine.OnRemoteCustomStreamPublished?.Invoke(roomId, userId, tag, _type);
            }
    
            public void onRemoteCustomStreamUnpublished(string roomId, string userId, string tag, AndroidJavaObject type)
            {
                RCRTCMediaType _type = new MediaTypeConverter(type).getCSharpObject();
                RCUnityLogger.getInstance().log("OnRemoteCustomStreamUnpublished",
                                                $"roomId={roomId},userId={userId},tag={tag},type={_type}");
                engine.OnRemoteCustomStreamUnpublished?.Invoke(roomId, userId, tag, _type);
            }
    
            public void onRemoteCustomStreamStateChanged(string roomId, string userId, string tag, AndroidJavaObject type,
                                                         bool disabled)
            {
                RCRTCMediaType _type = new MediaTypeConverter(type).getCSharpObject();
                RCUnityLogger.getInstance().log(
                    "OnRemoteCustomStreamStateChanged",
                    $"roomId={roomId},userId={userId},tag={tag},type={_type},disabled={disabled}");
                engine.OnRemoteCustomStreamStateChanged?.Invoke(roomId, userId, tag, _type, disabled);
            }
    
            public void onRemoteCustomStreamFirstFrame(string roomId, string userId, string tag, AndroidJavaObject type)
            {
                RCRTCMediaType _type = new MediaTypeConverter(type).getCSharpObject();
                RCUnityLogger.getInstance().log("OnRemoteCustomStreamFirstFrame",
                                                $"roomId={roomId},userId={userId},tag={tag},type={_type}");
                engine.OnRemoteCustomStreamFirstFrame?.Invoke(roomId, userId, tag, _type);
            }
    
            public void onCustomStreamSubscribed(string userId, string tag, AndroidJavaObject type, int code, string errMsg)
            {
                RCRTCMediaType _type = new MediaTypeConverter(type).getCSharpObject();
                RCUnityLogger.getInstance().log("OnCustomStreamSubscribed",
                                                $"userId={userId},tag={tag},type={_type},code={code},errMsg={errMsg}");
                engine.OnCustomStreamSubscribed?.Invoke(userId, tag, _type, code, errMsg);
            }
    
            public void onCustomStreamUnsubscribed(string userId, string tag, AndroidJavaObject type, int code,
                                                   string errMsg)
            {
                RCRTCMediaType _type = new MediaTypeConverter(type).getCSharpObject();
                RCUnityLogger.getInstance().log("OnCustomStreamUnsubscribed",
                                                $"userId={userId},tag={tag},type={_type},code={code},errMsg={errMsg}");
                engine.OnCustomStreamUnsubscribed?.Invoke(userId, tag, _type, code, errMsg);
            }
    
            public void onJoinSubRoomRequested(string roomId, string userId, int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnJoinSubRoomRequested",
                                                $"roomId={roomId},userId={userId},code={code},errMsg={errMsg}");
                engine.OnJoinSubRoomRequested?.Invoke(roomId, userId, code, errMsg);
            }
    
            public void onJoinSubRoomRequestCanceled(string roomId, string userId, int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnJoinSubRoomRequestCanceled",
                                                $"roomId={roomId},userId={userId},code={code},errMsg={errMsg}");
                engine.OnJoinSubRoomRequestCanceled?.Invoke(roomId, userId, code, errMsg);
            }
    
            public void onJoinSubRoomRequestResponded(string roomId, string userId, bool agree, int code, string errMsg)
            {
                RCUnityLogger.getInstance().log(
                    "OnJoinSubRoomRequestResponded",
                    $"roomId={roomId},userId={userId},agree={agree},code={code},errMsg={errMsg}");
                engine.OnJoinSubRoomRequestResponded?.Invoke(roomId, userId, agree, code, errMsg);
            }
    
            public void onJoinSubRoomRequestReceived(string roomId, string userId, string extra)
            {
                RCUnityLogger.getInstance().log("OnJoinSubRoomRequestReceived",
                                                $"roomId={roomId},userId={userId},extra={extra}");
                engine.OnJoinSubRoomRequestReceived?.Invoke(roomId, userId, extra);
            }
    
            public void onCancelJoinSubRoomRequestReceived(string roomId, string userId, string extra)
            {
                RCUnityLogger.getInstance().log("OnCancelJoinSubRoomRequestReceived",
                                                $"roomId={roomId},userId={userId},extra={extra}");
                engine.OnCancelJoinSubRoomRequestReceived?.Invoke(roomId, userId, extra);
            }
    
            public void onJoinSubRoomRequestResponseReceived(string roomId, string userId, bool agree, string extra)
            {
                RCUnityLogger.getInstance().log("OnJoinSubRoomRequestResponseReceived",
                                                $"roomId={roomId},userId={userId},agree={agree},extra={extra}");
                engine.OnJoinSubRoomRequestResponseReceived?.Invoke(roomId, userId, agree, extra);
            }
    
            public void onSubRoomJoined(string roomId, int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnSubRoomJoined", $"roomId={roomId},code={code},errMsg={errMsg}");
                engine.OnSubRoomJoined?.Invoke(roomId, code, errMsg);
            }
    
            public void onSubRoomLeft(string roomId, int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnSubRoomLeft", $"roomId={roomId},code={code},errMsg={errMsg}");
                engine.OnSubRoomLeft?.Invoke(roomId, code, errMsg);
            }
    
            public void onSubRoomBanded(string roomId)
            {
                RCUnityLogger.getInstance().log("OnSubRoomBanded", $"roomId={roomId}");
                engine.OnSubRoomBanded?.Invoke(roomId);
            }
    
            public void onSubRoomDisband(string roomId, string userId)
            {
                RCUnityLogger.getInstance().log("OnSubRoomDisband", $"roomId={roomId},userId={userId}");
                engine.OnSubRoomDisband?.Invoke(roomId, userId);
            }
    
            public void onLiveRoleSwitched(AndroidJavaObject current, int code, string errMsg)
            {
                RCRTCRole _current = new RoleConverter(current).getCSharpObject();
                RCUnityLogger.getInstance().log("OnLiveRoleSwitched", $"current={_current},code={code},errMsg={errMsg}");
                engine.OnLiveRoleSwitched?.Invoke(_current, code, errMsg);
            }
    
            public void onRemoteLiveRoleSwitched(string roomId, string userId, AndroidJavaObject role)
            {
                RCRTCRole _role = new RoleConverter(role).getCSharpObject();
                RCUnityLogger.getInstance().log("OnRemoteLiveRoleSwitched",
                                                $"roomId={roomId},userId={userId},role={_role}");
                engine.OnRemoteLiveRoleSwitched?.Invoke(roomId, userId, _role);
            }
    
            public void onLiveMixInnerCdnStreamEnabled(bool enable, int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnLiveMixInnerCdnStreamEnabled",
                                                $"enable={enable},code={code},errMsg={errMsg}");
                engine.OnLiveMixInnerCdnStreamEnabled?.Invoke(enable, code, errMsg);
            }
    
            public void onRemoteLiveMixInnerCdnStreamPublished()
            {
                RCUnityLogger.getInstance().log("OnRemoteLiveMixInnerCdnStreamPublished", $"");
                engine.OnRemoteLiveMixInnerCdnStreamPublished?.Invoke();
            }
    
            public void onRemoteLiveMixInnerCdnStreamUnpublished()
            {
                RCUnityLogger.getInstance().log("OnRemoteLiveMixInnerCdnStreamUnpublished", $"");
                engine.OnRemoteLiveMixInnerCdnStreamUnpublished?.Invoke();
            }
    
            public void onLiveMixInnerCdnStreamSubscribed(int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnLiveMixInnerCdnStreamSubscribed", $"code={code},errMsg={errMsg}");
                engine.OnLiveMixInnerCdnStreamSubscribed?.Invoke(code, errMsg);
            }
    
            public void onLiveMixInnerCdnStreamUnsubscribed(int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnLiveMixInnerCdnStreamUnsubscribed", $"code={code},errMsg={errMsg}");
                engine.OnLiveMixInnerCdnStreamUnsubscribed?.Invoke(code, errMsg);
            }
    
            public void onLocalLiveMixInnerCdnVideoResolutionSet(int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnLocalLiveMixInnerCdnVideoResolutionSet", $"code={code},errMsg={errMsg}");
                engine.OnLocalLiveMixInnerCdnVideoResolutionSet?.Invoke(code, errMsg);
            }
    
            public void onLocalLiveMixInnerCdnVideoFpsSet(int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnLocalLiveMixInnerCdnVideoFpsSet", $"code={code},errMsg={errMsg}");
                engine.OnLocalLiveMixInnerCdnVideoFpsSet?.Invoke(code, errMsg);
            }
    
            public void onWatermarkSet(int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnWatermarkSet", $"code={code},errMsg={errMsg}");
                engine.OnWatermarkSet?.Invoke(code, errMsg);
            }
    
            public void onWatermarkRemoved(int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnWatermarkRemoved", $"code={code},errMsg={errMsg}");
                engine.OnWatermarkRemoved?.Invoke(code, errMsg);
            }
    
            public void onNetworkProbeStarted(int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnNetworkProbeStarted", $"code={code},errMsg={errMsg}");
                engine.OnNetworkProbeStarted?.Invoke(code, errMsg);
            }
    
            public void onNetworkProbeStopped(int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnNetworkProbeStopped", $"code={code},errMsg={errMsg}");
                engine.OnNetworkProbeStopped?.Invoke(code, errMsg);
            }
    
            public void onSeiEnabled(bool enable, int code, string errMsg)
            {
                RCUnityLogger.getInstance().log("OnSeiEnabled", $"enable={enable},code={code},errMsg={errMsg}");
                engine.OnSeiEnabled?.Invoke(enable, code, errMsg);
            }
    
            public void onSeiReceived(string roomId, string userId, string sei)
            {
                RCUnityLogger.getInstance().log("OnSeiReceived", $"roomId={roomId},userId={userId},sei={sei}");
                engine.OnSeiReceived?.Invoke(roomId, userId, sei);
            }
    
            public void onLiveMixSeiReceived(string sei)
            {
                RCUnityLogger.getInstance().log("OnLiveMixSeiReceived", $"sei={sei}");
                engine.OnLiveMixSeiReceived?.Invoke(sei);
            }
        }
    }
}
    
#endif