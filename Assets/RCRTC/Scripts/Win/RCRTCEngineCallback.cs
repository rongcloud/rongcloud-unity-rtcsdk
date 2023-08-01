//
//  Copyright © 2021 RongCloud. All rights reserved.
//

#if UNITY_STANDALONE_WIN
using System;
using System.IO;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace cn_rongcloud_rtc_unity
{
    internal partial class RCRTCEngineWin
    {
        #region Stats
        [MonoPInvokeCallback(typeof(OnNetworkStats))]
        private static void on_rtc_network_stats(ref rtc_network_stats cstats)
        {
            RCRTCNetworkStats stats = new RCRTCNetworkStats((RCRTCNetworkType)cstats.type, cstats.ip,
                cstats.sendBitrate, cstats.receiveBitrate, cstats.rtt);
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
        private static void on_rtc_local_video_stats(ref rtc_local_video_stats cstats)
        {
            RCRTCLocalVideoStats stats = new RCRTCLocalVideoStats(cstats.tiny, (RCRTCVideoCodecType)cstats.codec, cstats.bitrate,
                cstats.fps, cstats.width, cstats.height, cstats.packageLostRate, cstats.rtt);
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
            RCRTCRemoteVideoStats stats = new RCRTCRemoteVideoStats((RCRTCVideoCodecType)cstats.codec, cstats.bitrate,
                cstats.fps, cstats.width, cstats.height, cstats.packageLostRate, cstats.rtt);
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
            RCRTCRemoteVideoStats stats = new RCRTCRemoteVideoStats((RCRTCVideoCodecType)cstats.codec, cstats.bitrate,
                cstats.fps, cstats.width, cstats.height, cstats.packageLostRate, cstats.rtt);
            StatsListener?.OnLiveMixVideoStats(stats);
        }
        #endregion

        #region Listener
        [MonoPInvokeCallback(typeof(OnErrorDelegate))]
        private static void on_rtc_error(int code, String errMsg)
        {
            Instance?.OnError?.Invoke(code, errMsg);
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

        [MonoPInvokeCallback(typeof(OnCameraSwitched))]
        private static void on_rtc_switch_camera(ref rtc_device camera, int code, string message)
        {
            RCRTCDevice device = new RCRTCDevice()
            {
                name = camera.name,
                id = camera.id,
                index = camera.index
            };
            Instance?.OnCameraSwitched?.Invoke(device, code, message);
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

        [MonoPInvokeCallback(typeof(OnLiveMixBackgroundColorSetDelegate))]
        private static void on_live_mix_background_color_set(int code, string errMsg)
        {
            Instance?.OnLiveMixBackgroundColorSet?.Invoke(code, errMsg);
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
        #endregion

        #region Device Change
        [MonoPInvokeCallback(typeof(OnCameraListChangeDelegate))]
        private static void on_rtc_camera_list_changed()
        {
            Instance?.OnCameraListChanged?.Invoke();
        }

        [MonoPInvokeCallback(typeof(OnMicrophoneListChangeDelegate))]
        private static void on_rtc_microphone_list_changed()
        {
            Instance?.OnMicrophoneListChanged?.Invoke();
        }

        [MonoPInvokeCallback(typeof(OnSpeakerListChangeDelegate))]
        private static void on_rtc_speaker_list_changed()
        {
            Instance?.OnSpeakerListChanged?.Invoke();
        }
        #endregion

        #region Video & Audio Listener
        [MonoPInvokeCallback(typeof(OnVideoFrame))]
        private static void on_rtc_video_frame(string uid, ref rtc_video_frame cframe)
        {
            video_frame_listeners.TryGetValue(uid, out RCRTCOnWritableVideoFrameListener lis);
            if (lis != null)
            {
                byte[] data = new byte[cframe.length];
                Marshal.Copy(cframe.data, data, 0, cframe.length);
                RCRTCVideoFrame frame = new RCRTCVideoFrame(cframe.data_y, cframe.data_u, cframe.data_v, cframe.length, cframe.stride_y, cframe.stride_u, cframe.stride_v, cframe.width, cframe.height);
                data = lis.OnVideoFrame(ref frame);
                Marshal.Copy(data, 0, cframe.data, data.Length);
            }
            video_render_listeners.TryGetValue(uid, out RCRTCOnVideoFrameListener listener);
            if (listener != null)
            {
                RCRTCVideoFrame frame = new RCRTCVideoFrame(cframe.data_y, cframe.data_u, cframe.data_v, cframe.length, cframe.stride_y, cframe.stride_u, cframe.stride_v, cframe.width, cframe.height);
                listener.OnVideoFrame(ref frame);
            }
        }
        
        [MonoPInvokeCallback(typeof(OnAudioFrame))]
        private static void on_rtc_audio_frame(string uid, ref rtc_audio_frame cframe)
        {
            byte[] data = new byte[cframe.length];
            Marshal.Copy(cframe.data, data, 0, cframe.length);
            RCRTCAudioFrame frame = new RCRTCAudioFrame(data, cframe.length, cframe.channels, cframe.sampleRate,
                cframe.bytesPerSample, cframe.samples);
            data = audio_frame_listeners[uid]?.OnAudioFrame(ref frame);
            Marshal.Copy(data, 0, cframe.data, data.Length);
        }
        #endregion

        #region CustomStream
        [MonoPInvokeCallback(typeof(OnCustomStreamPublishedDelegate))]
        private static void on_custom_stream_published(string tag, int code, string errMsg)
        {
            Instance?.OnCustomStreamPublished?.Invoke(tag, code, errMsg);
        }

        [MonoPInvokeCallback(typeof(OnCustomStreamPublishFinishedDelegate))]
        private static void on_custom_stream_publish_finished(string tag)
        {
            Instance?.OnCustomStreamPublishFinished?.Invoke(tag);
        }

        [MonoPInvokeCallback(typeof(OnCustomStreamUnpublishedDelegate))]
        private static void on_custom_stream_unpublished(string tag, int code, string errMsg)
        {
            Instance?.OnCustomStreamUnpublished?.Invoke(tag, code, errMsg);
        }

        [MonoPInvokeCallback(typeof(OnRemoteCustomStreamPublishedDelegate))]
        private static void on_remote_custom_stream_published(string roomId, string userId, string tag,
                                                               RCRTCMediaType type)
        {
            Instance?.OnRemoteCustomStreamPublished?.Invoke(roomId, userId, tag, type);
        }

        [MonoPInvokeCallback(typeof(OnRemoteCustomStreamUnpublishedDelegate))]
        private static void on_remote_custom_stream_unpublished(string roomId, string userId, string tag,
                                                               RCRTCMediaType type)
        {
            Instance?.OnRemoteCustomStreamUnpublished?.Invoke(roomId, userId, tag, type);
        }

        [MonoPInvokeCallback(typeof(OnRemoteCustomStreamStateChangedDelegate))]
        private static void on_remote_custom_stream_state_changed(string roomId, string userId, string tag,
                                                                  RCRTCMediaType type, bool disabled)
        {
            Instance?.OnRemoteCustomStreamStateChanged?.Invoke(roomId, userId, tag, type, disabled);
        }

        [MonoPInvokeCallback(typeof(OnRemoteCustomStreamFirstFrameDelegate))]
        private static void on_remote_custom_stream_first_frame(string roomId, string userId, string tag,
                                                                RCRTCMediaType type)
        {
            Instance?.OnRemoteCustomStreamFirstFrame?.Invoke(roomId, userId, tag, type);
        }

        [MonoPInvokeCallback(typeof(OnCustomStreamSubscribedDelegate))]
        private static void on_custom_stream_subscribed(string userId, string tag, RCRTCMediaType type, int code,
                                                          string errMsg)
        {
            Instance?.OnCustomStreamSubscribed?.Invoke(userId, tag, type, code, errMsg);
        }

        [MonoPInvokeCallback(typeof(OnCustomStreamUnsubscribedDelegate))]
        private static void on_custom_stream_unsubscribed(string userId, string tag, RCRTCMediaType type, int code,
                                                          string errMsg)
        {
            Instance?.OnCustomStreamUnsubscribed?.Invoke(userId, tag, type, code, errMsg);
        }

        [MonoPInvokeCallback(typeof(OnLiveRoleSwitchedDelegate))]
        private static void on_live_role_switched(RCRTCRole current, int code, string errMsg)
        {
            Instance?.OnLiveRoleSwitched?.Invoke(current, code, errMsg);
        }

        [MonoPInvokeCallback(typeof(OnRemoteLiveRoleSwitchedDelegate))]
        private static void on_remote_live_role_switched(string roomId, string userId, RCRTCRole role)
        {
            Instance?.OnRemoteLiveRoleSwitched?.Invoke(roomId, userId, role);
        }
        #endregion
    }
}
#endif