#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

namespace cn_rongcloud_rtc_unity
{
    internal partial class RCRTCEngineIOS
    {
        [MonoPInvokeCallback(typeof(OnNetworkStats))]
        private static void on_rtc_network_stats(ref rtc_network_stats cstats)
        {
            RCRTCNetworkStats stats = new RCRTCNetworkStats((RCRTCNetworkType)cstats.type, cstats.ip, cstats.sendBitrate,
                                                            cstats.receiveBitrate, cstats.rtt);
            StatsListener?.OnNetworkStats(stats);
        }
    
        [MonoPInvokeCallback(typeof(OnLocalAudioStats))]
        private static void on_rtc_local_audio_stats(ref rtc_local_audio_stats cstats)
        {
            RCRTCLocalAudioStats stats = new RCRTCLocalAudioStats((RCRTCAudioCodecType)cstats.codec, cstats.bitrate,
                                                                  cstats.volume, cstats.packageLostRate, cstats.rtt);
            StatsListener?.OnLocalAudioStats(stats);
        }
    
        [MonoPInvokeCallback(typeof(OnLocalVideoStats))]
        static void on_rtc_local_video_stats(ref rtc_local_video_stats cstats)
        {
            RCRTCLocalVideoStats stats =
                new RCRTCLocalVideoStats(cstats.tiny, (RCRTCVideoCodecType)cstats.codec, cstats.bitrate, cstats.fps,
                                         cstats.width, cstats.height, cstats.packageLostRate, cstats.rtt);
            StatsListener?.OnLocalVideoStats(stats);
        }
    
        [MonoPInvokeCallback(typeof(OnRemoteAudioStats))]
        private static void on_rtc_remote_audio_stats(string userId, ref rtc_remote_audio_stats cstats)
        {
            RCRTCRemoteAudioStats stats = new RCRTCRemoteAudioStats((RCRTCAudioCodecType)cstats.codec, cstats.bitrate,
                                                                    cstats.volume, cstats.packageLostRate, cstats.rtt);
            StatsListener?.OnRemoteAudioStats(userId, stats);
        }
    
        [MonoPInvokeCallback(typeof(OnRemoteVideoStats))]
        private static void on_rtc_remote_video_stats(string userId, ref rtc_remote_video_stats cstats)
        {
            RCRTCRemoteVideoStats stats =
                new RCRTCRemoteVideoStats((RCRTCVideoCodecType)cstats.codec, cstats.bitrate, cstats.fps, cstats.width,
                                          cstats.height, cstats.packageLostRate, cstats.rtt);
            StatsListener?.OnRemoteVideoStats(userId, stats);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixAudioStats))]
        private static void on_rtc_live_mix_audio_stats(ref rtc_remote_audio_stats cstats)
        {
            RCRTCRemoteAudioStats stats = new RCRTCRemoteAudioStats((RCRTCAudioCodecType)cstats.codec, cstats.bitrate,
                                                                    cstats.volume, cstats.packageLostRate, cstats.rtt);
            StatsListener?.OnLiveMixAudioStats(stats);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixVideoStats))]
        private static void on_rtc_live_mix_video_stats(ref rtc_remote_video_stats cstats)
        {
            RCRTCRemoteVideoStats stats =
                new RCRTCRemoteVideoStats((RCRTCVideoCodecType)cstats.codec, cstats.bitrate, cstats.fps, cstats.width,
                                          cstats.height, cstats.packageLostRate, cstats.rtt);
            StatsListener?.OnLiveMixVideoStats(stats);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixMemberAudioStats))]
        private static void on_rtc_live_mix_member_audio_stats(string userId, int volume)
        {
            StatsListener?.OnLiveMixMemberAudioStats(userId, volume);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixMemberCustomAudioStats))]
        private static void on_rtc_live_mix_member_custom_audio_stats(string userId, string tag, int volume)
        {
            StatsListener?.OnLiveMixMemberCustomAudioStats(userId, tag, volume);
        }
    
        [MonoPInvokeCallback(typeof(OnAudioFrame))]
        private static void on_rtc_audio_frame(string id, ref rtc_audio_frame cframe)
        {
            byte[] data = new byte[cframe.length];
            Marshal.Copy(cframe.data, data, 0, cframe.length);
            RCRTCAudioFrame frame = new RCRTCAudioFrame(data, cframe.length, cframe.channels, cframe.sampleRate,
                                                        cframe.bytesPerSample, cframe.samples);
            data = AudioListeners[id]?.OnAudioFrame(ref frame);
            Marshal.Copy(data, 0, cframe.data, data.Length);
        }
    
        [MonoPInvokeCallback(typeof(OnErrorDelegate))]
        private static void on_rtc_error(int code, string message)
        {
            Instance?.OnError?.Invoke(code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnKickedDelegate))]
        private static void on_rtc_kicked(string roomId, string message)
        {
            Instance?.OnKicked?.Invoke(roomId, message);
        }
    
        [MonoPInvokeCallback(typeof(OnRoomJoinedDelegate))]
        private static void on_rtc_room_joined(int code, string message)
        {
            Debug.Log("on_rtc_room_joined code = " + code + ", message = " + message);
            Instance?.OnRoomJoined?.Invoke(code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnRoomLeftDelegate))]
        private static void on_rtc_room_left(int code, string message)
        {
            Instance?.OnRoomLeft?.Invoke(code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnPublishedDelegate))]
        private static void on_rtc_published(RCRTCMediaType type, int code, string message)
        {
            Instance?.OnPublished?.Invoke(type, code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnUnpublishedDelegate))]
        private static void on_rtc_unpublished(RCRTCMediaType type, int code, string message)
        {
            Instance?.OnUnpublished?.Invoke(type, code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnSubscribedDelegate))]
        private static void on_rtc_subscribed(string id, RCRTCMediaType type, int code, string message)
        {
            Instance?.OnSubscribed?.Invoke(id, type, code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnUnsubscribedDelegate))]
        private static void on_rtc_unsubscribed(string id, RCRTCMediaType type, int code, string message)
        {
            Instance?.OnUnsubscribed?.Invoke(id, type, code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixSubscribedDelegate))]
        private static void on_rtc_live_mix_subscribed(RCRTCMediaType type, int code, string message)
        {
            Instance?.OnLiveMixSubscribed?.Invoke(type, code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixUnsubscribedDelegate))]
        private static void on_rtc_live_mix_unsubscribed(RCRTCMediaType type, int code, string message)
        {
            Instance?.OnLiveMixUnsubscribed?.Invoke(type, code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnCameraEnabledDelegate))]
        private static void on_rtc_enable_camera(bool enable, int code, string message)
        {
            Instance?.OnCameraEnabled?.Invoke(enable, code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnCameraSwitchedDelegate))]
        private static void on_rtc_switch_camera(RCRTCCamera camera, int code, string message)
        {
            Instance?.OnCameraSwitched?.Invoke(camera, code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveCdnAddedDelegate))]
        private static void on_rtc_live_cdn_added(string url, int code, string message)
        {
            Instance?.OnLiveCdnAdded?.Invoke(url, code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveCdnRemovedDelegate))]
        private static void on_rtc_live_cdn_removed(string url, int code, string message)
        {
            Instance?.OnLiveCdnRemoved?.Invoke(url, code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixLayoutModeSetDelegate))]
        private static void on_rtc_live_mix_layout_mode_set(int code, string message)
        {
            Instance?.OnLiveMixLayoutModeSet?.Invoke(code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixRenderModeSetDelegate))]
        private static void on_rtc_live_mix_render_mode_set(int code, string message)
        {
            Instance?.OnLiveMixRenderModeSet?.Invoke(code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixCustomLayoutsSetDelegate))]
        private static void on_rtc_live_mix_custom_layouts_set(int code, string message)
        {
            Instance?.OnLiveMixCustomLayoutsSet?.Invoke(code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixCustomAudiosSetDelegate))]
        private static void on_rtc_live_mix_custom_audios_set(int code, string message)
        {
            Instance?.OnLiveMixCustomAudiosSet?.Invoke(code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixVideoBitrateSetDelegate))]
        private static void on_rtc_live_mix_video_bitrate_set(bool tiny, int code, string message)
        {
            Instance?.OnLiveMixVideoBitrateSet?.Invoke(tiny, code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixVideoResolutionSetDelegate))]
        private static void on_rtc_live_mix_video_resolution_set(bool tiny, int code, string message)
        {
            Instance?.OnLiveMixVideoResolutionSet?.Invoke(tiny, code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixVideoFpsSetDelegate))]
        private static void on_rtc_live_mix_video_fps_set(bool tiny, int code, string message)
        {
            Instance?.OnLiveMixVideoFpsSet?.Invoke(tiny, code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixAudioBitrateSetDelegate))]
        private static void on_rtc_live_mix_audio_bitrate_set(int code, string message)
        {
            Instance?.OnLiveMixAudioBitrateSet?.Invoke(code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnAudioEffectCreatedDelegate))]
        private static void on_rtc_audio_effect_created(int id, int code, string message)
        {
            Instance?.OnAudioEffectCreated?.Invoke(id, code, message);
        }
    
        [MonoPInvokeCallback(typeof(OnAudioEffectFinishedDelegate))]
        private static void on_rtc_audio_effect_finished(int id)
        {
            Instance?.OnAudioEffectFinished?.Invoke(id);
        }
    
        [MonoPInvokeCallback(typeof(OnAudioMixingStartedDelegate))]
        private static void on_rtc_audio_mixing_started()
        {
            Instance?.OnAudioMixingStarted?.Invoke();
        }
    
        [MonoPInvokeCallback(typeof(OnAudioMixingPausedDelegate))]
        private static void on_rtc_audio_mixing_paused()
        {
            Instance?.OnAudioMixingPaused?.Invoke();
        }
    
        [MonoPInvokeCallback(typeof(OnAudioMixingStoppedDelegate))]
        private static void on_rtc_audio_mixing_stopped()
        {
            Instance?.OnAudioMixingStopped?.Invoke();
        }
    
        [MonoPInvokeCallback(typeof(OnAudioMixingFinishedDelegate))]
        private static void on_rtc_audio_mixing_finished()
        {
            Instance?.OnAudioMixingFinished?.Invoke();
        }
    
        [MonoPInvokeCallback(typeof(OnUserJoinedDelegate))]
        private static void on_rtc_user_joined(string roomId, string userId)
        {
            Instance?.OnUserJoined?.Invoke(roomId, userId);
        }
    
        [MonoPInvokeCallback(typeof(OnUserOfflineDelegate))]
        private static void on_rtc_user_offline(string roomId, string userId)
        {
            Instance?.OnUserOffline?.Invoke(roomId, userId);
        }
    
        [MonoPInvokeCallback(typeof(OnUserLeftDelegate))]
        private static void on_rtc_user_left(string roomId, string userId)
        {
            Instance?.OnUserLeft?.Invoke(roomId, userId);
        }
    
        [MonoPInvokeCallback(typeof(OnRemotePublishedDelegate))]
        private static void on_rtc_remote_published(string roomId, string userId, RCRTCMediaType type)
        {
            Instance?.OnRemotePublished?.Invoke(roomId, userId, type);
        }
    
        [MonoPInvokeCallback(typeof(OnRemoteUnpublishedDelegate))]
        private static void on_rtc_remote_unpublished(string roomId, string userId, RCRTCMediaType type)
        {
            Instance?.OnRemoteUnpublished?.Invoke(roomId, userId, type);
        }
    
        [MonoPInvokeCallback(typeof(OnRemoteLiveMixPublishedDelegate))]
        private static void on_rtc_remote_live_mix_published(RCRTCMediaType type)
        {
            Instance?.OnRemoteLiveMixPublished?.Invoke(type);
        }
    
        [MonoPInvokeCallback(typeof(OnRemoteLiveMixUnpublishedDelegate))]
        private static void on_rtc_remote_live_mix_unpublished(RCRTCMediaType type)
        {
            Instance?.OnRemoteLiveMixUnpublished?.Invoke(type);
        }
    
        [MonoPInvokeCallback(typeof(OnRemoteStateChangedDelegate))]
        private static void on_rtc_remote_state_changed(string roomId, string userId, RCRTCMediaType type, bool disabled)
        {
            Instance?.OnRemoteStateChanged?.Invoke(roomId, userId, type, disabled);
        }
    
        [MonoPInvokeCallback(typeof(OnRemoteFirstFrameDelegate))]
        private static void on_rtc_remote_first_frame(string roomId, string userId, RCRTCMediaType type)
        {
            Instance?.OnRemoteFirstFrame?.Invoke(roomId, userId, type);
        }
    
        [MonoPInvokeCallback(typeof(OnRemoteLiveMixFirstFrameDelegate))]
        private static void on_rtc_remote_live_mix_first_frame(RCRTCMediaType type)
        {
            Instance?.OnRemoteLiveMixFirstFrame?.Invoke(type);
        }
    
#region IRCRTCIWListener
        [MonoPInvokeCallback(typeof(OnLiveMixBackgroundColorSet))]
        private static void on_live_mix_background_color_set(int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnLiveMixBackgroundColorSet", $"code={code},errMsg={errMsg}");
            instance?.OnLiveMixBackgroundColorSet?.Invoke(code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnAudioMixingProgressReported))]
        private static void on_audio_mixing_progress_reported(float progress)
        {
            RCUnityLogger.getInstance().log("OnAudioMixingProgressReported", $"progress={progress}");
            instance?.OnAudioMixingProgressReported?.Invoke(progress);
        }
    
        [MonoPInvokeCallback(typeof(OnCustomStreamPublished))]
        private static void on_custom_stream_published(string tag, int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnCustomStreamPublished", $"tag={tag},code={code},errMsg={errMsg}");
            instance?.OnCustomStreamPublished?.Invoke(tag, code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnCustomStreamPublishFinished))]
        private static void on_custom_stream_publish_finished(string tag)
        {
            RCUnityLogger.getInstance().log("OnCustomStreamPublishFinished", $"tag={tag}");
            instance?.OnCustomStreamPublishFinished?.Invoke(tag);
        }
    
        [MonoPInvokeCallback(typeof(OnCustomStreamUnpublished))]
        private static void on_custom_stream_unpublished(string tag, int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnCustomStreamUnpublished", $"tag={tag},code={code},errMsg={errMsg}");
            instance?.OnCustomStreamUnpublished?.Invoke(tag, code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnRemoteCustomStreamPublished))]
        private static void on_remote_custom_stream_published(string roomId, string userId, string tag, RCRTCMediaType type)
        {
            RCUnityLogger.getInstance().log("OnRemoteCustomStreamPublished",
                                            $"roomId={roomId},userId={userId},tag={tag},type={type}");
            instance?.OnRemoteCustomStreamPublished?.Invoke(roomId, userId, tag, type);
        }
    
        [MonoPInvokeCallback(typeof(OnRemoteCustomStreamUnpublished))]
        private static void on_remote_custom_stream_unpublished(string roomId, string userId, string tag,
                                                                RCRTCMediaType type)
        {
            RCUnityLogger.getInstance().log("OnRemoteCustomStreamUnpublished",
                                            $"roomId={roomId},userId={userId},tag={tag},type={type}");
            instance?.OnRemoteCustomStreamUnpublished?.Invoke(roomId, userId, tag, type);
        }
    
        [MonoPInvokeCallback(typeof(OnRemoteCustomStreamStateChanged))]
        private static void on_remote_custom_stream_state_changed(string roomId, string userId, string tag,
                                                                  RCRTCMediaType type, bool disabled)
        {
            RCUnityLogger.getInstance().log("OnRemoteCustomStreamStateChanged",
                                            $"roomId={roomId},userId={userId},tag={tag},type={type},disabled={disabled}");
            instance?.OnRemoteCustomStreamStateChanged?.Invoke(roomId, userId, tag, type, disabled);
        }
    
        [MonoPInvokeCallback(typeof(OnRemoteCustomStreamFirstFrame))]
        private static void on_remote_custom_stream_first_frame(string roomId, string userId, string tag,
                                                                RCRTCMediaType type)
        {
            RCUnityLogger.getInstance().log("OnRemoteCustomStreamFirstFrame",
                                            $"roomId={roomId},userId={userId},tag={tag},type={type}");
            instance?.OnRemoteCustomStreamFirstFrame?.Invoke(roomId, userId, tag, type);
        }
    
        [MonoPInvokeCallback(typeof(OnCustomStreamSubscribed))]
        private static void on_custom_stream_subscribed(string userId, string tag, RCRTCMediaType type, int code,
                                                        string errMsg)
        {
            RCUnityLogger.getInstance().log("OnCustomStreamSubscribed",
                                            $"userId={userId},tag={tag},type={type},code={code},errMsg={errMsg}");
            instance?.OnCustomStreamSubscribed?.Invoke(userId, tag, type, code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnCustomStreamUnsubscribed))]
        private static void on_custom_stream_unsubscribed(string userId, string tag, RCRTCMediaType type, int code,
                                                          string errMsg)
        {
            RCUnityLogger.getInstance().log("OnCustomStreamUnsubscribed",
                                            $"userId={userId},tag={tag},type={type},code={code},errMsg={errMsg}");
            instance?.OnCustomStreamUnsubscribed?.Invoke(userId, tag, type, code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnJoinSubRoomRequested))]
        private static void on_join_sub_room_requested(string roomId, string userId, int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnJoinSubRoomRequested",
                                            $"roomId={roomId},userId={userId},code={code},errMsg={errMsg}");
            instance?.OnJoinSubRoomRequested?.Invoke(roomId, userId, code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnJoinSubRoomRequestCanceled))]
        private static void on_join_sub_room_request_canceled(string roomId, string userId, int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnJoinSubRoomRequestCanceled",
                                            $"roomId={roomId},userId={userId},code={code},errMsg={errMsg}");
            instance?.OnJoinSubRoomRequestCanceled?.Invoke(roomId, userId, code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnJoinSubRoomRequestResponded))]
        private static void on_join_sub_room_request_responded(string roomId, string userId, bool agree, int code,
                                                               string errMsg)
        {
            RCUnityLogger.getInstance().log("OnJoinSubRoomRequestResponded",
                                            $"roomId={roomId},userId={userId},agree={agree},code={code},errMsg={errMsg}");
            instance?.OnJoinSubRoomRequestResponded?.Invoke(roomId, userId, agree, code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnJoinSubRoomRequestReceived))]
        private static void on_join_sub_room_request_received(string roomId, string userId, string extra)
        {
            RCUnityLogger.getInstance().log("OnJoinSubRoomRequestReceived",
                                            $"roomId={roomId},userId={userId},extra={extra}");
            instance?.OnJoinSubRoomRequestReceived?.Invoke(roomId, userId, extra);
        }
    
        [MonoPInvokeCallback(typeof(OnCancelJoinSubRoomRequestReceived))]
        private static void on_cancel_join_sub_room_request_received(string roomId, string userId, string extra)
        {
            RCUnityLogger.getInstance().log("OnCancelJoinSubRoomRequestReceived",
                                            $"roomId={roomId},userId={userId},extra={extra}");
            instance?.OnCancelJoinSubRoomRequestReceived?.Invoke(roomId, userId, extra);
        }
    
        [MonoPInvokeCallback(typeof(OnJoinSubRoomRequestResponseReceived))]
        private static void on_join_sub_room_request_response_received(string roomId, string userId, bool agree,
                                                                       string extra)
        {
            RCUnityLogger.getInstance().log("OnJoinSubRoomRequestResponseReceived",
                                            $"roomId={roomId},userId={userId},agree={agree},extra={extra}");
            instance?.OnJoinSubRoomRequestResponseReceived?.Invoke(roomId, userId, agree, extra);
        }
    
        [MonoPInvokeCallback(typeof(OnSubRoomJoined))]
        private static void on_sub_room_joined(string roomId, int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnSubRoomJoined", $"roomId={roomId},code={code},errMsg={errMsg}");
            instance?.OnSubRoomJoined?.Invoke(roomId, code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnSubRoomLeft))]
        private static void on_sub_room_left(string roomId, int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnSubRoomLeft", $"roomId={roomId},code={code},errMsg={errMsg}");
            instance?.OnSubRoomLeft?.Invoke(roomId, code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnSubRoomBanded))]
        private static void on_sub_room_banded(string roomId)
        {
            RCUnityLogger.getInstance().log("OnSubRoomBanded", $"roomId={roomId}");
            instance?.OnSubRoomBanded?.Invoke(roomId);
        }
    
        [MonoPInvokeCallback(typeof(OnSubRoomDisband))]
        private static void on_sub_room_disband(string roomId, string userId)
        {
            RCUnityLogger.getInstance().log("OnSubRoomDisband", $"roomId={roomId},userId={userId}");
            instance?.OnSubRoomDisband?.Invoke(roomId, userId);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveRoleSwitched))]
        private static void on_live_role_switched(RCRTCRole current, int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnLiveRoleSwitched", $"current={current},code={code},errMsg={errMsg}");
            instance?.OnLiveRoleSwitched?.Invoke(current, code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnRemoteLiveRoleSwitched))]
        private static void on_remote_live_role_switched(string roomId, string userId, RCRTCRole role)
        {
            RCUnityLogger.getInstance().log("OnRemoteLiveRoleSwitched", $"roomId={roomId},userId={userId},role={role}");
            instance?.OnRemoteLiveRoleSwitched?.Invoke(roomId, userId, role);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixInnerCdnStreamEnabled))]
        private static void on_live_mix_inner_cdn_stream_enabled(bool enable, int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnLiveMixInnerCdnStreamEnabled",
                                            $"enable={enable},code={code},errMsg={errMsg}");
            instance?.OnLiveMixInnerCdnStreamEnabled?.Invoke(enable, code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnRemoteLiveMixInnerCdnStreamPublished))]
        private static void on_remote_live_mix_inner_cdn_stream_published()
        {
            RCUnityLogger.getInstance().log("OnRemoteLiveMixInnerCdnStreamPublished", $"");
            instance?.OnRemoteLiveMixInnerCdnStreamPublished?.Invoke();
        }
    
        [MonoPInvokeCallback(typeof(OnRemoteLiveMixInnerCdnStreamUnpublished))]
        private static void on_remote_live_mix_inner_cdn_stream_unpublished()
        {
            RCUnityLogger.getInstance().log("OnRemoteLiveMixInnerCdnStreamUnpublished", $"");
            instance?.OnRemoteLiveMixInnerCdnStreamUnpublished?.Invoke();
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixInnerCdnStreamSubscribed))]
        private static void on_live_mix_inner_cdn_stream_subscribed(int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnLiveMixInnerCdnStreamSubscribed", $"code={code},errMsg={errMsg}");
            instance?.OnLiveMixInnerCdnStreamSubscribed?.Invoke(code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixInnerCdnStreamUnsubscribed))]
        private static void on_live_mix_inner_cdn_stream_unsubscribed(int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnLiveMixInnerCdnStreamUnsubscribed", $"code={code},errMsg={errMsg}");
            instance?.OnLiveMixInnerCdnStreamUnsubscribed?.Invoke(code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnLocalLiveMixInnerCdnVideoResolutionSet))]
        private static void on_local_live_mix_inner_cdn_video_resolution_set(int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnLocalLiveMixInnerCdnVideoResolutionSet", $"code={code},errMsg={errMsg}");
            instance?.OnLocalLiveMixInnerCdnVideoResolutionSet?.Invoke(code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnLocalLiveMixInnerCdnVideoFpsSet))]
        private static void on_local_live_mix_inner_cdn_video_fps_set(int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnLocalLiveMixInnerCdnVideoFpsSet", $"code={code},errMsg={errMsg}");
            instance?.OnLocalLiveMixInnerCdnVideoFpsSet?.Invoke(code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnWatermarkSet))]
        private static void on_watermark_set(int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnWatermarkSet", $"code={code},errMsg={errMsg}");
            instance?.OnWatermarkSet?.Invoke(code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnWatermarkRemoved))]
        private static void on_watermark_removed(int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnWatermarkRemoved", $"code={code},errMsg={errMsg}");
            instance?.OnWatermarkRemoved?.Invoke(code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnNetworkProbeStarted))]
        private static void on_network_probe_started(int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnNetworkProbeStarted", $"code={code},errMsg={errMsg}");
            instance?.OnNetworkProbeStarted?.Invoke(code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnNetworkProbeStopped))]
        private static void on_network_probe_stopped(int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnNetworkProbeStopped", $"code={code},errMsg={errMsg}");
            instance?.OnNetworkProbeStopped?.Invoke(code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnSeiEnabled))]
        private static void on_sei_enabled(bool enable, int code, string errMsg)
        {
            RCUnityLogger.getInstance().log("OnSeiEnabled", $"enable={enable},code={code},errMsg={errMsg}");
            instance?.OnSeiEnabled?.Invoke(enable, code, errMsg);
        }
    
        [MonoPInvokeCallback(typeof(OnSeiReceived))]
        private static void on_sei_received(string roomId, string userId, string sei)
        {
            RCUnityLogger.getInstance().log("OnSeiReceived", $"roomId={roomId},userId={userId},sei={sei}");
            instance?.OnSeiReceived?.Invoke(roomId, userId, sei);
        }
    
        [MonoPInvokeCallback(typeof(OnLiveMixSeiReceived))]
        private static void on_live_mix_sei_received(string sei)
        {
            RCUnityLogger.getInstance().log("OnLiveMixSeiReceived", $"sei={sei}");
            instance?.OnLiveMixSeiReceived?.Invoke(sei);
        }
    
#endregion
#region RCRTCIWNetworkProbeListener
        [MonoPInvokeCallback(typeof(IOSNetworkProbeProxyOnNetworkProbeUpLinkStats))]
        private static void ios_network_probe_proxy_on_network_probe_up_link_stats(IntPtr stats, Int64 handle)
        {
            RCRTCNetworkProbeStats stats_cls = null;
            if (stats != IntPtr.Zero)
            {
                var cstats = NativeUtils.GetStructByPtr<rtc_network_probe_stats>(stats);
                stats_cls = NativeConvert.fromNetworkProbeStats(ref cstats);
            }
            RCUnityLogger.getInstance().log("OnNetworkProbeUpLinkStats", $"stats_cls={stats_cls}");
            var cb = NativeUtils.TakeCallback<RCRTCNetworkProbeListener>(handle);
            cb?.OnNetworkProbeUpLinkStats(stats_cls);
        }
        [MonoPInvokeCallback(typeof(IOSNetworkProbeProxyOnNetworkProbeDownLinkStats))]
        private static void ios_network_probe_proxy_on_network_probe_down_link_stats(IntPtr stats, Int64 handle)
        {
            RCRTCNetworkProbeStats stats_cls = null;
            if (stats != IntPtr.Zero)
            {
                var cstats = NativeUtils.GetStructByPtr<rtc_network_probe_stats>(stats);
                stats_cls = NativeConvert.fromNetworkProbeStats(ref cstats);
            }
            RCUnityLogger.getInstance().log("OnNetworkProbeDownLinkStats", $"stats_cls={stats_cls}");
            var cb = NativeUtils.TakeCallback<RCRTCNetworkProbeListener>(handle);
            cb?.OnNetworkProbeDownLinkStats(stats_cls);
        }
        [MonoPInvokeCallback(typeof(IOSNetworkProbeProxyOnNetworkProbeFinished))]
        private static void ios_network_probe_proxy_on_network_probe_finished(int code, string errMsg, Int64 handle)
        {
            RCUnityLogger.getInstance().log("OnNetworkProbeFinished", $"code={code},errMsg={errMsg}");
            var cb = NativeUtils.TakeCallback<RCRTCNetworkProbeListener>(handle);
            cb?.OnNetworkProbeFinished(code, errMsg);
        }
        internal static ios_network_probe_proxy toIOSNetworkProbeProxy(RCRTCNetworkProbeListener listener)
        {
            ios_network_probe_proxy proxy;
            proxy.handle = NativeUtils.AddCallback(listener);
            proxy.onNetworkProbeUpLinkStats = ios_network_probe_proxy_on_network_probe_up_link_stats;
            proxy.onNetworkProbeDownLinkStats = ios_network_probe_proxy_on_network_probe_down_link_stats;
            proxy.onNetworkProbeFinished = ios_network_probe_proxy_on_network_probe_finished;
            return proxy;
        }
#endregion
    }
}
#endif
