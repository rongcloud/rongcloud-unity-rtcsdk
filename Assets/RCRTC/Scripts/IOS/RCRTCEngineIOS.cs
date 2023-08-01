#if UNITY_IOS

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

namespace cn_rongcloud_rtc_unity
{
    internal partial class RCRTCEngineIOS : RCRTCEngine
    {
    
        internal RCRTCEngineIOS(RCRTCEngineSetup setup)
        {
            rtc_set_engine_listeners(
                on_rtc_error, on_rtc_kicked, on_rtc_room_joined, on_rtc_room_left, on_rtc_published, on_rtc_unpublished,
                on_rtc_subscribed, on_rtc_unsubscribed, on_rtc_live_mix_subscribed, on_rtc_live_mix_unsubscribed,
                on_rtc_enable_camera, on_rtc_switch_camera, on_rtc_live_cdn_added, on_rtc_live_cdn_removed,
                on_rtc_live_mix_layout_mode_set, on_rtc_live_mix_render_mode_set, on_rtc_live_mix_custom_layouts_set,
                on_rtc_live_mix_custom_audios_set, on_rtc_live_mix_video_bitrate_set, on_rtc_live_mix_video_resolution_set,
                on_rtc_live_mix_video_fps_set, on_rtc_live_mix_audio_bitrate_set, on_rtc_audio_effect_created,
                on_rtc_audio_effect_finished, on_rtc_audio_mixing_started, on_rtc_audio_mixing_paused,
                on_rtc_audio_mixing_stopped, on_rtc_audio_mixing_finished, on_rtc_user_joined, on_rtc_user_offline,
                on_rtc_user_left, on_rtc_remote_published, on_rtc_remote_unpublished, on_rtc_remote_live_mix_published,
                on_rtc_remote_live_mix_unpublished, on_rtc_remote_state_changed, on_rtc_remote_first_frame,
                on_rtc_remote_live_mix_first_frame);
            NativeIOS.rtc_set_IRCRTCIWListener(
                on_live_mix_background_color_set, on_audio_mixing_progress_reported, on_custom_stream_published,
                on_custom_stream_publish_finished, on_custom_stream_unpublished, on_remote_custom_stream_published,
                on_remote_custom_stream_unpublished, on_remote_custom_stream_state_changed,
                on_remote_custom_stream_first_frame, on_custom_stream_subscribed, on_custom_stream_unsubscribed,
                on_join_sub_room_requested, on_join_sub_room_request_canceled, on_join_sub_room_request_responded,
                on_join_sub_room_request_received, on_cancel_join_sub_room_request_received,
                on_join_sub_room_request_response_received, on_sub_room_joined, on_sub_room_left, on_sub_room_banded,
                on_sub_room_disband, on_live_role_switched, on_remote_live_role_switched,
                on_live_mix_inner_cdn_stream_enabled, on_remote_live_mix_inner_cdn_stream_published,
                on_remote_live_mix_inner_cdn_stream_unpublished, on_live_mix_inner_cdn_stream_subscribed,
                on_live_mix_inner_cdn_stream_unsubscribed, on_local_live_mix_inner_cdn_video_resolution_set,
                on_local_live_mix_inner_cdn_video_fps_set, on_watermark_set, on_watermark_removed, on_network_probe_started,
                on_network_probe_stopped, on_sei_enabled, on_sei_received, on_live_mix_sei_received);
    
            if (setup == null)
            {
                engine = NativeIOS.rtc_create_engine();
            }
            else
            {
                rtc_engine_setup cobject = toEngineSetup(setup);
                engine = NativeIOS.rtc_create_engine_with_setup(ref cobject);
                if (cobject.audioSetup != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(cobject.audioSetup);
                }
                if (cobject.videoSetup != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(cobject.videoSetup);
                }
            }
            AudioListeners = new Dictionary<String, RCRTCOnWritableAudioFrameListener>();
            Instance = this;
        }
    
        public override void Destroy()
        {
            if (engine != IntPtr.Zero)
            {
                NativeIOS.rtc_release_engine(engine);
                engine = IntPtr.Zero;
            }
    
            Instance = null;
            StatsListener = null;
            AudioListeners.Clear();
            AudioListeners = null;
            Listeners.Clear();
            base.Destroy();
        }
    
        public override int JoinRoom(String roomId, RCRTCRoomSetup setup)
        {
            rtc_room_setup cobject = toRoomSetup(setup);
            return NativeIOS.rtc_join_room(engine, roomId, ref cobject);
        }
    
        public override int LeaveRoom()
        {
            return NativeIOS.rtc_leave_room(engine);
        }
    
        public override int Publish(RCRTCMediaType type)
        {
            return NativeIOS.rtc_publish(engine, (int)type);
        }
    
        public override int Unpublish(RCRTCMediaType type)
        {
            return NativeIOS.rtc_unpublish(engine, (int)type);
        }
    
        public override int Subscribe(String userId, RCRTCMediaType type, bool tiny)
        {
            return NativeIOS.rtc_subscribe(engine, userId, (int)type, tiny);
        }
    
        public override int Subscribe(IList<String> userIds, RCRTCMediaType type, bool tiny)
        {
            List<String> list = new List<string>(userIds);
            return NativeIOS.rtc_subscribe_with_user_ids(engine, list.ToArray(), list.Count, (int)type, tiny);
        }
    
        public override int SubscribeLiveMix(RCRTCMediaType type, bool tiny)
        {
            return NativeIOS.rtc_subscribe_live_mix(engine, (int)type, tiny);
        }
    
        public override int Unsubscribe(String userId, RCRTCMediaType type)
        {
            return NativeIOS.rtc_unsubscribe(engine, userId, (int)type);
        }
    
        public override int Unsubscribe(IList<String> userIds, RCRTCMediaType type)
        {
            List<String> list = new List<string>(userIds);
            return NativeIOS.rtc_unsubscribe_with_user_ids(engine, list.ToArray(), list.Count, (int)type);
        }
    
        public override int UnsubscribeLiveMix(RCRTCMediaType type)
        {
            return NativeIOS.rtc_unsubscribe_live_mix(engine, (int)type);
        }
    
        public override int SetAudioConfig(RCRTCAudioConfig config)
        {
            rtc_audio_config cobject = toAudioConfig(config);
            return NativeIOS.rtc_set_audio_config(engine, ref cobject);
        }
    
        public override int SetVideoConfig(RCRTCVideoConfig config, bool tiny)
        {
            rtc_video_config cobject = toVideoConfig(config);
            return NativeIOS.rtc_set_video_config(engine, ref cobject, tiny);
        }
    
        public override int EnableMicrophone(bool enable)
        {
            return NativeIOS.rtc_enable_microphone(engine, enable);
        }
    
        public override int EnableSpeaker(bool enable)
        {
            return NativeIOS.rtc_enable_speaker(engine, enable);
        }
    
        public override int AdjustLocalVolume(int volume)
        {
            return NativeIOS.rtc_adjust_local_volume(engine, volume);
        }
    
        public override int EnableCamera(bool enable, RCRTCCamera camera)
        {
            return NativeIOS.rtc_enable_camera(engine, enable, (int)camera);
        }
    
        public override int SwitchCamera()
        {
            return NativeIOS.rtc_switch_camera(engine);
        }
    
        public override RCRTCCamera WhichCamera()
        {
            return (RCRTCCamera)NativeIOS.rtc_which_camera(engine);
        }
    
        public override bool IsCameraFocusSupported()
        {
            return NativeIOS.rtc_is_camera_focus_supported(engine);
        }
    
        public override bool IsCameraExposurePositionSupported()
        {
            return NativeIOS.rtc_is_camera_exposure_position_supported(engine);
        }
    
        public override int SetCameraFocusPositionInPreview(double x, double y)
        {
            return NativeIOS.rtc_set_camera_focus_position_in_preview(engine, x, y);
        }
    
        public override int SetCameraExposurePositionInPreview(double x, double y)
        {
            return NativeIOS.rtc_set_camera_exposure_position_in_preview(engine, x, y);
        }
    
        public override int SetCameraCaptureOrientation(RCRTCCameraCaptureOrientation orientation)
        {
            return NativeIOS.rtc_set_camera_capture_orientation(engine, (int)orientation);
        }
    
        public override int MuteLocalStream(RCRTCMediaType type, bool mute)
        {
            return NativeIOS.rtc_mute_local_stream(engine, (int)type, mute);
        }
    
        public override int MuteRemoteStream(String userId, RCRTCMediaType type, bool mute)
        {
            return NativeIOS.rtc_mute_remote_stream(engine, userId, (int)type, mute);
        }
    
        public override int MuteAllRemoteAudioStreams(bool mute)
        {
            return NativeIOS.rtc_mute_all_remote_audio_streams(engine, mute);
        }
    
        private IntPtr toNativeView(RCRTCView view)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = typeof(RCRTCView);
            MethodInfo method = type.GetMethod("InitNativeView", flag);
            object[] arguments = { engine };
            method.Invoke(view, arguments);
            FieldInfo field = type.GetField("_view", flag);
            return (IntPtr)field.GetValue(view);
        }
    
        public override int SetLocalView(RCRTCView view)
        {
            return NativeIOS.rtc_set_local_view(engine, toNativeView(view));
        }
    
        public override int RemoveLocalView()
        {
            return NativeIOS.rtc_remove_local_view(engine);
        }
    
        public override int SetRemoteView(String userId, RCRTCView view)
        {
            return NativeIOS.rtc_set_remote_view(engine, userId, toNativeView(view));
        }
    
        public override int RemoveRemoteView(String userId)
        {
            return NativeIOS.rtc_remove_remote_view(engine, userId);
        }
    
        public override int SetLiveMixView(RCRTCView view)
        {
            return NativeIOS.rtc_set_live_mix_view(engine, toNativeView(view));
        }
    
        public override int RemoveLiveMixView()
        {
            return NativeIOS.rtc_remove_live_mix_view(engine);
        }
    
        public override int SetLiveMixInnerCdnStreamView(RCRTCView view)
        {
            return NativeIOS.rtc_set_live_mix_inner_cdn_stream_view(engine, toNativeView(view));
        }
    
        public override int SetLocalCustomStreamView(string tag, RCRTCView view)
        {
            return NativeIOS.rtc_set_local_custom_stream_view(tag, toNativeView(view));
        }
    
        public override int SetRemoteCustomStreamView(string userId, string tag, RCRTCView view)
        {
            return NativeIOS.rtc_set_remote_custom_stream_view(userId, tag, toNativeView(view));
        }
    
        public override int AddLiveCdn(String url)
        {
            return NativeIOS.rtc_add_live_cdn(engine, url);
        }
    
        public override int RemoveLiveCdn(String url)
        {
            return NativeIOS.rtc_remove_live_cdn(engine, url);
        }
    
        public override int SetLiveMixLayoutMode(RCRTCLiveMixLayoutMode mode)
        {
            return NativeIOS.rtc_set_live_mix_layout_mode(engine, (int)mode);
        }
    
        public override int SetLiveMixRenderMode(RCRTCLiveMixRenderMode mode)
        {
            return NativeIOS.rtc_set_live_mix_render_mode(engine, (int)mode);
        }
    
        public override int SetLiveMixCustomLayouts(IList<RCRTCCustomLayout> layouts)
        {
            if (layouts == null || layouts.Count == 0)
                return -1;
            rtc_custom_layout[] cobjects = new rtc_custom_layout[layouts.Count];
            for (int i = 0; i < layouts.Count; i++)
            {
                cobjects[i] = toCustomLayout(layouts[i]);
            }
    
            return NativeIOS.rtc_set_live_mix_custom_layouts(engine, ref cobjects, layouts.Count);
        }
    
        public override int SetLiveMixCustomAudios(IList<String> userIds)
        {
            if (userIds == null || userIds.Count == 0)
                return -1;
            List<String> list = new List<string>(userIds);
            return NativeIOS.rtc_set_live_mix_custom_audios(engine, list.ToArray(), list.Count);
        }
    
        public override int SetLiveMixVideoBitrate(int bitrate, bool tiny)
        {
            return NativeIOS.rtc_set_live_mix_video_bitrate(engine, bitrate, tiny);
        }
    
        public override int SetLiveMixVideoResolution(int width, int height, bool tiny)
        {
            return NativeIOS.rtc_set_live_mix_video_resolution(engine, width, height, tiny);
        }
    
        public override int SetLiveMixVideoFps(RCRTCVideoFps fps, bool tiny)
        {
            return NativeIOS.rtc_set_live_mix_video_fps(engine, (int)fps, tiny);
        }
    
        public override int SetLiveMixAudioBitrate(int bitrate)
        {
            return NativeIOS.rtc_set_live_mix_audio_bitrate(engine, bitrate);
        }
    
        public override int SetStatsListener(RCRTCStatsListener listener)
        {
            rtc_stats_listener_proxy proxy = toStatsListenerProxy(listener);
            return NativeIOS.rtc_set_stats_listener(engine, ref proxy);
        }
    
        public override int CreateAudioEffect(String path, int effectId)
        {
            return NativeIOS.rtc_create_audio_effect(engine, path, effectId);
        }
    
        public override int ReleaseAudioEffect(int effectId)
        {
            return NativeIOS.rtc_release_audio_effect(engine, effectId);
        }
    
        public override int PlayAudioEffect(int effectId, int volume, int loop)
        {
            return NativeIOS.rtc_play_audio_effect(engine, effectId, volume, loop);
        }
    
        public override int PauseAudioEffect(int effectId)
        {
            return NativeIOS.rtc_pause_audio_effect(engine, effectId);
        }
    
        public override int PauseAllAudioEffects()
        {
            return NativeIOS.rtc_pause_all_audio_effects(engine);
        }
    
        public override int ResumeAudioEffect(int effectId)
        {
            return NativeIOS.rtc_resume_audio_effect(engine, effectId);
        }
    
        public override int ResumeAllAudioEffects()
        {
            return NativeIOS.rtc_resume_all_audio_effects(engine);
        }
    
        public override int StopAudioEffect(int effectId)
        {
            return NativeIOS.rtc_stop_audio_effect(engine, effectId);
        }
    
        public override int StopAllAudioEffects()
        {
            return NativeIOS.rtc_stop_all_audio_effects(engine);
        }
    
        public override int AdjustAudioEffectVolume(int effectId, int volume)
        {
            return NativeIOS.rtc_adjust_audio_effect_volume(engine, effectId, volume);
        }
    
        public override int GetAudioEffectVolume(int effectId)
        {
            return NativeIOS.rtc_get_audio_effect_volume(engine, effectId);
        }
    
        public override int AdjustAllAudioEffectsVolume(int volume)
        {
            return NativeIOS.rtc_adjust_all_audio_effects_volume(engine, volume);
        }
    
        public override int StartAudioMixing(String path, RCRTCAudioMixingMode mode, bool playback, int loop)
        {
            return NativeIOS.rtc_start_audio_mixing(engine, path, (int)mode, playback, loop);
        }
    
        public override int StopAudioMixing()
        {
            return NativeIOS.rtc_stop_audio_mixing(engine);
        }
    
        public override int PauseAudioMixing()
        {
            return NativeIOS.rtc_pause_audio_mixing(engine);
        }
    
        public override int ResumeAudioMixing()
        {
            return NativeIOS.rtc_resume_audio_mixing(engine);
        }
    
        public override int AdjustAudioMixingVolume(int volume)
        {
            return NativeIOS.rtc_adjust_audio_mixing_volume(engine, volume);
        }
    
        public override int AdjustAudioMixingPlaybackVolume(int volume)
        {
            return NativeIOS.rtc_adjust_audio_mixing_playback_volume(engine, volume);
        }
    
        public override int AdjustAudioMixingPublishVolume(int volume)
        {
            return NativeIOS.rtc_adjust_audio_mixing_publish_volume(engine, volume);
        }
    
        public override int GetAudioMixingPlaybackVolume()
        {
            return NativeIOS.rtc_get_audio_mixing_playback_volume(engine);
        }
    
        public override int GetAudioMixingPublishVolume()
        {
            return NativeIOS.rtc_get_audio_mixing_publish_volume(engine);
        }
    
        public override int SetAudioMixingPosition(double position)
        {
            return NativeIOS.rtc_set_audio_mixing_position(engine, position);
        }
    
        public override double GetAudioMixingPosition()
        {
            return NativeIOS.rtc_get_audio_mixing_position(engine);
        }
    
        public override int GetAudioMixingDuration()
        {
            return NativeIOS.rtc_get_audio_mixing_duration(engine);
        }
    
        public override int SetLocalAudioCapturedListener(RCRTCOnWritableAudioFrameListener listener)
        {
            rtc_writable_audio_frame_listener_proxy proxy = toWritableAudioFrameListener("", listener);
            return NativeIOS.rtc_set_local_audio_captured_listener(engine, ref proxy);
        }
    
        public override int SetRemoteAudioReceivedListener(String userId, RCRTCOnWritableAudioFrameListener listener)
        {
            rtc_writable_audio_frame_listener_proxy proxy = toWritableAudioFrameListener(userId, listener);
            return NativeIOS.rtc_set_remote_audio_received_listener(engine, userId, ref proxy);
        }
    
        public override String GetSessionId()
        {
            return NativeIOS.rtc_get_session_rtc_id(engine);
        }
    
        private rtc_engine_setup toEngineSetup(RCRTCEngineSetup setup)
        {
            rtc_engine_setup cobject;
            cobject.reconnectable = setup.IsReconnectable();
            cobject.statsReportInterval = setup.GetStatsReportInterval();
            cobject.enableSRTP = setup.IsEnableSRTP();
            if (setup.GetAudioSetup() != null)
            {
                rtc_audio_setup audio;
                audio.audioCodecType = (int)setup.GetAudioSetup().GetAudioCodecType();
                audio.mixOtherAppsAudio = setup.GetAudioSetup().IsMixOtherAppsAudio();
                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(audio));
                Marshal.StructureToPtr(audio, ptr, true);
                cobject.audioSetup = ptr;
            }
            else
            {
                cobject.audioSetup = IntPtr.Zero;
            }
    
            if (setup.GetVideoSetup() != null)
            {
                rtc_video_setup video;
                video.enableTinyStream = setup.GetVideoSetup().IsEnableTinyStream();
                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(video));
                Marshal.StructureToPtr(video, ptr, true);
                cobject.videoSetup = ptr;
            }
            else
            {
                cobject.videoSetup = IntPtr.Zero;
            }
            if (setup.GetMediaUrl() != null)
            {
                cobject.mediaUrl = setup.GetMediaUrl();
            }
            else
            {
                cobject.mediaUrl = "";
            }
            return cobject;
        }
    
        private rtc_room_setup toRoomSetup(RCRTCRoomSetup setup)
        {
            rtc_room_setup cobject;
            cobject.role = (int)setup.GetRole();
            cobject.mediaType = (int)setup.GetMediaType();
            cobject.joinType = (int)setup.GetJoinType();
            return cobject;
        }
    
        private rtc_audio_config toAudioConfig(RCRTCAudioConfig config)
        {
            rtc_audio_config cobject;
            cobject.quality = (int)config.GetQuality();
            cobject.scenario = (int)config.GetScenario();
            cobject.recordingVolume = config.GetRecordingVolume();
            return cobject;
        }
    
        private rtc_video_config toVideoConfig(RCRTCVideoConfig config)
        {
            rtc_video_config cobject;
            cobject.minBitrate = config.GetMinBitrate();
            cobject.maxBitrate = config.GetMaxBitrate();
            cobject.fps = (int)config.GetFps();
            cobject.resolution = (int)config.GetResolution();
            return cobject;
        }
    
        private rtc_custom_layout toCustomLayout(RCRTCCustomLayout layout)
        {
            rtc_custom_layout cobject;
            cobject.type = layout.GetTag() != null ? -1 : 0;
            cobject.id = layout.GetUserId();
            cobject.tag = layout.GetTag();
            cobject.x = layout.GetX();
            cobject.y = layout.GetY();
            cobject.width = layout.GetWidth();
            cobject.height = layout.GetHeight();
            return cobject;
        }
    
        private rtc_stats_listener_proxy toStatsListenerProxy(RCRTCStatsListener listener)
        {
            StatsListener = listener;
            rtc_stats_listener_proxy proxy;
            proxy.remove = listener == null;
            proxy.onNetworkStats = on_rtc_network_stats;
            proxy.onLocalAudioStats = on_rtc_local_audio_stats;
            proxy.onLocalVideoStats = on_rtc_local_video_stats;
            proxy.onRemoteAudioStats = on_rtc_remote_audio_stats;
            proxy.onRemoteVideoStats = on_rtc_remote_video_stats;
            proxy.onLiveMixAudioStats = on_rtc_live_mix_audio_stats;
            proxy.onLiveMixVideoStats = on_rtc_live_mix_video_stats;
            proxy.onLiveMixMemberAudioStats = on_rtc_live_mix_member_audio_stats;
            proxy.OnLiveMixMemberCustomAudioStats = on_rtc_live_mix_member_custom_audio_stats;
            return proxy;
        }
    
        private rtc_writable_audio_frame_listener_proxy toWritableAudioFrameListener(
            String userId, RCRTCOnWritableAudioFrameListener listener)
        {
            rtc_writable_audio_frame_listener_proxy proxy;
            proxy.remove = listener == null;
            if (proxy.remove)
            {
                AudioListeners.Remove(userId);
            }
            else
            {
                AudioListeners.Add(userId, listener);
            }
            proxy.onAudioFrame = on_rtc_audio_frame;
            return proxy;
        }
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern void rtc_set_engine_listeners(
            OnErrorDelegate on_rtc_error, OnKickedDelegate on_rtc_kicked, OnRoomJoinedDelegate on_rtc_room_joined,
            OnRoomLeftDelegate on_rtc_room_left, OnPublishedDelegate on_rtc_published,
            OnUnpublishedDelegate on_rtc_unpublished, OnSubscribedDelegate on_rtc_subscribed,
            OnUnsubscribedDelegate on_rtc_unsubscribed, OnLiveMixSubscribedDelegate on_rtc_live_mix_subscribed,
            OnLiveMixUnsubscribedDelegate on_rtc_live_mix_unsubscribed, OnCameraEnabledDelegate on_rtc_enable_camera,
            OnCameraSwitchedDelegate on_rtc_switch_camera, OnLiveCdnAddedDelegate on_rtc_live_cdn_added,
            OnLiveCdnRemovedDelegate on_rtc_live_cdn_removed,
            OnLiveMixLayoutModeSetDelegate on_rtc_live_mix_layout_mode_set,
            OnLiveMixRenderModeSetDelegate on_rtc_live_mix_render_mode_set,
            OnLiveMixCustomLayoutsSetDelegate on_rtc_live_mix_custom_layouts_mode_set,
            OnLiveMixCustomAudiosSetDelegate on_rtc_live_mix_custom_audios_set,
            OnLiveMixVideoBitrateSetDelegate on_rtc_live_mix_video_bitrate_set,
            OnLiveMixVideoResolutionSetDelegate on_rtc_live_mix_video_resolution_set,
            OnLiveMixVideoFpsSetDelegate on_rtc_live_mix_video_fps_set,
            OnLiveMixAudioBitrateSetDelegate on_rtc_live_mix_audio_bitrate_set,
            OnAudioEffectCreatedDelegate on_rtc_audio_effect_created,
            OnAudioEffectFinishedDelegate on_rtc_audio_effect_finished,
            OnAudioMixingStartedDelegate on_rtc_audio_mixing_started,
            OnAudioMixingPausedDelegate on_rtc_audio_mixing_paused,
            OnAudioMixingStoppedDelegate on_rtc_audio_mixing_stopped,
            OnAudioMixingFinishedDelegate on_rtc_audio_mixing_finished, OnUserJoinedDelegate on_rtc_user_joined,
            OnUserOfflineDelegate on_rtc_user_offline, OnUserLeftDelegate on_rtc_user_left,
            OnRemotePublishedDelegate on_rtc_remote_published, OnRemoteUnpublishedDelegate on_rtc_remote_unpublished,
            OnRemoteLiveMixPublishedDelegate on_rtc_remote_live_mix_published,
            OnRemoteLiveMixUnpublishedDelegate on_rtc_remote_live_mix_unpublished,
            OnRemoteStateChangedDelegate on_rtc_remote_state_changed, OnRemoteFirstFrameDelegate on_rtc_remote_first_frame,
            OnRemoteLiveMixFirstFrameDelegate on_rtc_remote_live_mix_first_frame);
    
        public override int StartNetworkProbe(RCRTCNetworkProbeListener listener)
        {
            ios_network_probe_proxy proxy = toIOSNetworkProbeProxy(listener);
            return NativeIOS.rtc_start_network_Probe(ref proxy);
        }
    
        public override int SetWatermark(string path, double x, double y, double zoom)
        {
            return NativeIOS.rtc_set_watermark(path, x, y, zoom);
        }
    
        public override int CreateCustomStreamFromFile(string path, string tag, bool replace, bool playback)
        {
            return NativeIOS.rtc_create_custom_stream_from_file(path, tag, replace, playback);
        }
    
        public override int SetCustomStreamVideoConfig(string tag, RCRTCVideoConfig config)
        {
            rtc_video_config cobject = toVideoConfig(config);
            return NativeIOS.rtc_set_custom_stream_video_config(tag, ref cobject);
        }
    
        public override int MuteLiveMixStream(RCRTCMediaType type, bool mute)
        {
            RCUnityLogger.getInstance().log("MuteLiveMixStream", $"type={type},mute={mute}");
            return NativeIOS.rtc_mute_live_mix_stream(type, mute);
        }
    
        public override int SetLiveMixBackgroundColor(int color)
        {
            RCUnityLogger.getInstance().log("SetLiveMixBackgroundColor", $"color={color}");
            return NativeIOS.rtc_set_live_mix_background_color(color);
        }
    
        public override int MuteLocalCustomStream(string tag, bool mute)
        {
            RCUnityLogger.getInstance().log("MuteLocalCustomStream", $"tag={tag},mute={mute}");
            return NativeIOS.rtc_mute_local_custom_stream(tag, mute);
        }
    
        public override int RemoveLocalCustomStreamView(string tag)
        {
            RCUnityLogger.getInstance().log("RemoveLocalCustomStreamView", $"tag={tag}");
            return NativeIOS.rtc_remove_local_custom_stream_view(tag);
        }
    
        public override int PublishCustomStream(string tag)
        {
            RCUnityLogger.getInstance().log("PublishCustomStream", $"tag={tag}");
            return NativeIOS.rtc_publish_custom_stream(tag);
        }
    
        public override int UnpublishCustomStream(string tag)
        {
            RCUnityLogger.getInstance().log("UnpublishCustomStream", $"tag={tag}");
            return NativeIOS.rtc_unpublish_custom_stream(tag);
        }
    
        public override int MuteRemoteCustomStream(string userId, string tag, RCRTCMediaType type, bool mute)
        {
            RCUnityLogger.getInstance().log("MuteRemoteCustomStream", $"userId={userId},tag={tag},type={type},mute={mute}");
            return NativeIOS.rtc_mute_remote_custom_stream(userId, tag, type, mute);
        }
    
        public override int RemoveRemoteCustomStreamView(string userId, string tag)
        {
            RCUnityLogger.getInstance().log("RemoveRemoteCustomStreamView", $"userId={userId},tag={tag}");
            return NativeIOS.rtc_remove_remote_custom_stream_view(userId, tag);
        }
    
        public override int SubscribeCustomStream(string userId, string tag, RCRTCMediaType type, bool tiny)
        {
            RCUnityLogger.getInstance().log("SubscribeCustomStream", $"userId={userId},tag={tag},type={type},tiny={tiny}");
            return NativeIOS.rtc_subscribe_custom_stream(userId, tag, type, tiny);
        }
    
        public override int UnsubscribeCustomStream(string userId, string tag, RCRTCMediaType type)
        {
            RCUnityLogger.getInstance().log("UnsubscribeCustomStream", $"userId={userId},tag={tag},type={type}");
            return NativeIOS.rtc_unsubscribe_custom_stream(userId, tag, type);
        }
    
        public override int RequestJoinSubRoom(string roomId, string userId, bool autoLayout, string extra)
        {
            RCUnityLogger.getInstance().log("RequestJoinSubRoom",
                                            $"roomId={roomId},userId={userId},autoLayout={autoLayout},extra={extra}");
            return NativeIOS.rtc_request_join_sub_room(roomId, userId, autoLayout, extra);
        }
    
        public override int CancelJoinSubRoomRequest(string roomId, string userId, string extra)
        {
            RCUnityLogger.getInstance().log("CancelJoinSubRoomRequest", $"roomId={roomId},userId={userId},extra={extra}");
            return NativeIOS.rtc_cancel_join_sub_room_request(roomId, userId, extra);
        }
    
        public override int ResponseJoinSubRoomRequest(string roomId, string userId, bool agree, bool autoLayout,
                                                       string extra)
        {
            RCUnityLogger.getInstance().log(
                "ResponseJoinSubRoomRequest",
                $"roomId={roomId},userId={userId},agree={agree},autoLayout={autoLayout},extra={extra}");
            return NativeIOS.rtc_response_join_sub_room_request(roomId, userId, agree, autoLayout, extra);
        }
    
        public override int JoinSubRoom(string roomId)
        {
            RCUnityLogger.getInstance().log("JoinSubRoom", $"roomId={roomId}");
            return NativeIOS.rtc_join_sub_room(roomId);
        }
    
        public override int LeaveSubRoom(string roomId, bool disband)
        {
            RCUnityLogger.getInstance().log("LeaveSubRoom", $"roomId={roomId},disband={disband}");
            return NativeIOS.rtc_leave_sub_room(roomId, disband);
        }
    
        public override int SwitchLiveRole(RCRTCRole role)
        {
            RCUnityLogger.getInstance().log("SwitchLiveRole", $"role={role}");
            return NativeIOS.rtc_switch_live_role(role);
        }
    
        public override int EnableLiveMixInnerCdnStream(bool enable)
        {
            RCUnityLogger.getInstance().log("EnableLiveMixInnerCdnStream", $"enable={enable}");
            return NativeIOS.rtc_enable_live_mix_inner_cdn_stream(enable);
        }
    
        public override int SubscribeLiveMixInnerCdnStream()
        {
            RCUnityLogger.getInstance().log("SubscribeLiveMixInnerCdnStream", $"");
            return NativeIOS.rtc_subscribe_live_mix_inner_cdn_stream();
        }
    
        public override int UnsubscribeLiveMixInnerCdnStream()
        {
            RCUnityLogger.getInstance().log("UnsubscribeLiveMixInnerCdnStream", $"");
            return NativeIOS.rtc_unsubscribe_live_mix_inner_cdn_stream();
        }
    
        public override int RemoveLiveMixInnerCdnStreamView()
        {
            RCUnityLogger.getInstance().log("RemoveLiveMixInnerCdnStreamView", $"");
            return NativeIOS.rtc_remove_live_mix_inner_cdn_stream_view();
        }
    
        public override int SetLocalLiveMixInnerCdnVideoResolution(int width, int height)
        {
            RCUnityLogger.getInstance().log("SetLocalLiveMixInnerCdnVideoResolution", $"width={width},height={height}");
            return NativeIOS.rtc_set_local_live_mix_inner_cdn_video_resolution(width, height);
        }
    
        public override int SetLocalLiveMixInnerCdnVideoFps(RCRTCVideoFps fps)
        {
            RCUnityLogger.getInstance().log("SetLocalLiveMixInnerCdnVideoFps", $"fps={fps}");
            return NativeIOS.rtc_set_local_live_mix_inner_cdn_video_fps(fps);
        }
    
        public override int MuteLiveMixInnerCdnStream(bool mute)
        {
            RCUnityLogger.getInstance().log("MuteLiveMixInnerCdnStream", $"mute={mute}");
            return NativeIOS.rtc_mute_live_mix_inner_cdn_stream(mute);
        }
    
        public override int RemoveWatermark()
        {
            RCUnityLogger.getInstance().log("RemoveWatermark", $"");
            return NativeIOS.rtc_remove_watermark();
        }
    
        public override int StopNetworkProbe()
        {
            RCUnityLogger.getInstance().log("StopNetworkProbe", $"");
            return NativeIOS.rtc_stop_network_probe();
        }
    
        public override int StartEchoTest(int timeInterval)
        {
            RCUnityLogger.getInstance().log("StartEchoTest", $"timeInterval={timeInterval}");
            return NativeIOS.rtc_start_echo_test(timeInterval);
        }
    
        public override int StopEchoTest()
        {
            RCUnityLogger.getInstance().log("StopEchoTest", $"");
            return NativeIOS.rtc_stop_echo_test();
        }
    
        public override int EnableSei(bool enable)
        {
            RCUnityLogger.getInstance().log("EnableSei", $"enable={enable}");
            return NativeIOS.rtc_enable_sei(enable);
        }
    
        public override int SendSei(string sei)
        {
            RCUnityLogger.getInstance().log("SendSei", $"sei={sei}");
            return NativeIOS.rtc_send_sei(sei);
        }
    
        public override int PreconnectToMediaServer()
        {
            RCUnityLogger.getInstance().log("PreconnectToMediaServer", $"");
            return NativeIOS.rtc_preconnect_to_media_server();
        }
    
        private IntPtr engine = IntPtr.Zero;
    
        private static RCRTCEngineIOS Instance = null;
    
        private static RCRTCStatsListener StatsListener = null;
    
        private static Dictionary<String, object> Listeners = new Dictionary<String, object>();
    
        private static Dictionary<String, RCRTCOnWritableAudioFrameListener> AudioListeners = null;
    }
}
    
#endif
