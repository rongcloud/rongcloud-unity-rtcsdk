
#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;

namespace cn_rongcloud_rtc_unity
{
    internal class NativeWin
    {
        const string RTC_Lib = "RTCWinWapper";

        #region RTC Engine
        [DllImport(RTC_Lib, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void rcrtc_init_wrapper_logger(int level);

        [DllImport(RTC_Lib, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr rcrtc_create_engine(IntPtr client);

        [DllImport(RTC_Lib, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr rcrtc_create_engine_with_setup(IntPtr client, ref rtc_engine_setup setup);

        [DllImport(RTC_Lib, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void rcrtc_engine_destory(IntPtr engine);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void rcrtc_set_engine_listeners(
                OnErrorDelegate on_rtc_error,
                OnKickedDelegate on_rtc_kicked,
                OnRoomJoinedDelegate on_rtc_room_joined,
                OnRoomLeftDelegate on_rtc_room_left,
                OnPublishedDelegate on_rtc_published,
                OnUnpublishedDelegate on_rtc_unpublished,
                OnSubscribedDelegate on_rtc_subscribed,
                OnUnsubscribedDelegate on_rtc_unsubscribed,
                OnLiveMixSubscribedDelegate on_rtc_live_mix_subscribed,
                OnLiveMixUnsubscribedDelegate on_rtc_live_mix_unsubscribed,
                OnCameraEnabledDelegate on_rtc_enable_camera,
                OnCameraSwitched on_rtc_switch_camera,
                OnLiveCdnAddedDelegate on_rtc_live_cdn_added,
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
                OnAudioMixingFinishedDelegate on_rtc_audio_mixing_finished,
                OnUserJoinedDelegate on_rtc_user_joined,
                OnUserOfflineDelegate on_rtc_user_offline,
                OnUserLeftDelegate on_rtc_user_left,
                OnRemotePublishedDelegate on_rtc_remote_published,
                OnRemoteUnpublishedDelegate on_rtc_remote_unpublished,
                OnRemoteLiveMixPublishedDelegate on_rtc_remote_live_mix_published,
                OnRemoteLiveMixUnpublishedDelegate on_rtc_remote_live_mix_unpublished,
                OnRemoteStateChangedDelegate on_rtc_remote_state_changed,
                OnRemoteFirstFrameDelegate on_rtc_remote_first_frame,
                OnRemoteLiveMixFirstFrameDelegate on_rtc_remote_live_mix_first_frame
                );

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_stats_listener(IntPtr engine, ref rtc_stats_listener_proxy proxy);
        #endregion

        #region Room
        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_join_room(IntPtr engine, string id);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_join_room_with_setup(IntPtr engine, string id, ref rtc_room_setup setup);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_leave_room(IntPtr engine);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern string rcrtc_get_session_id(IntPtr engine);
        #endregion

        #region Publish
        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_publish(IntPtr engine, int type);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_unpublish(IntPtr engine, int type);
        #endregion

        #region Subscribe
        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_subscribe(IntPtr engine, string id, int type, bool tiny);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_subscribe_with_user_ids(IntPtr engine, string[] ids, int count, int type, bool tiny);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_unsubscribe(IntPtr engine, string id, int type);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_unsubscribe_with_user_ids(IntPtr engine, string[] ids, int count, int type);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_subscribe_live_mix(IntPtr engine, int type, bool tiny);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_unsubscribe_live_mix(IntPtr engine, int type);
        #endregion

        #region Config
        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_audio_config(IntPtr engine, ref rtc_audio_config config);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_video_config(IntPtr engine, ref rtc_video_config config, bool tiny);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_adjust_local_volume(IntPtr engine, int volume);
        #endregion

        #region Divice

        /// Camera

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern IntPtr rcrtc_get_camera_list(IntPtr engine, ref int count);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_enable_camera(IntPtr engine, ref rtc_device camera, bool asDefault);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_disable_camera(IntPtr engine, ref rtc_device camera);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_switch_to_camera(IntPtr engin, ref rtc_device camera);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_which_camera(IntPtr engine, ref rtc_device camera);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void rcrtc_set_camera_change_callback(OnCameraListChangeDelegate cb);

        /// Microphone
        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern IntPtr rcrtc_get_microphone_list(IntPtr engine, ref int count);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_enable_microphone(IntPtr engine, ref rtc_device mic, bool asDefault);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_disable_microphone(IntPtr engine, ref rtc_device mic);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void rcrtc_set_microphone_change_callback(OnMicrophoneListChangeDelegate cb);

        /// Speaker
        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern IntPtr rcrtc_get_speaker_list(IntPtr engine, ref int count);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_enable_speaker(IntPtr engine, ref rtc_device speaker);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_disable_speaker(IntPtr engine, ref rtc_device speaker);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void rcrtc_set_speaker_change_callback(OnSpeakerListChangeDelegate cb);

        #endregion

        #region Mute
        [DllImport(RTC_Lib, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_mute_local_stream(IntPtr engine, int type, bool mute);

        [DllImport(RTC_Lib, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_mute_remote_stream(IntPtr engine, string id, int type, bool mute);

        [DllImport(RTC_Lib, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_mute_all_remote_audio_streams(IntPtr engine, bool mute);
        #endregion

        #region Audio & Video Data
        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_local_video_listener(IntPtr engine, ref rtc_video_listener_proxy proxy);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_remote_video_listener(IntPtr engine, string userId, ref rtc_video_listener_proxy proxy);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_live_mix_video_listener(IntPtr engine, ref rtc_video_listener_proxy proxy);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_local_audio_captured_listener(IntPtr engine, ref rtc_writable_audio_frame_listener_proxy proxy);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_remote_audio_received_listener(IntPtr engine, string userId, ref rtc_writable_audio_frame_listener_proxy proxy);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_memory_copy(IntPtr s, IntPtr d, int length);
        #endregion

        #region Live
        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_add_live_cdn(IntPtr engine, string url);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_remove_live_cdn(IntPtr engine, string url);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_live_mix_layout_mode(IntPtr engine, int mode);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_live_mix_render_mode(IntPtr engine, int mode);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_live_mix_custom_layouts(IntPtr engine, ref rtc_custom_layout[] layouts, int count);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_live_mix_custom_audios(IntPtr engine, string[] ids, int count);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_live_mix_video_bitrate(IntPtr engine, int bitrate, bool tiny);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_live_mix_video_resolution(IntPtr engine, int width, int height, bool tiny);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_live_mix_video_fps(IntPtr engine, int fps, bool tiny);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_live_mix_audio_bitrate(IntPtr engine, int bitrate);
        #endregion

        #region Audio Effect
        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_create_audio_effect(IntPtr engine, string path, int id);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_release_audio_effect(IntPtr engine, int id);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_play_audio_effect(IntPtr engine, int id, int volume, int loop);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_pause_audio_effect(IntPtr engine, int id);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_pause_all_audio_effects(IntPtr engine);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_resume_audio_effect(IntPtr engine, int id);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_resume_all_audio_effects(IntPtr engine);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_stop_audio_effect(IntPtr engine, int id);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_stop_all_audio_effects(IntPtr engine);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_adjust_audio_effect_volume(IntPtr engine, int id, int volume);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_get_audio_effect_volume(IntPtr engine, int id);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_adjust_all_audio_effects_volume(IntPtr engine, int volume);
        #endregion

        #region Audio Mix
        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_start_audio_mixing(IntPtr engine, string path, int mode, bool playback, int loop);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_stop_audio_mixing(IntPtr engine);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_pause_audio_mixing(IntPtr engine);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_resume_audio_mixing(IntPtr engine);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_adjust_audio_mixing_volume(IntPtr engine, int volume);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_adjust_audio_mixing_playback_volume(IntPtr engine, int volume);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_adjust_audio_mixing_publish_volume(IntPtr engine, int volume);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_get_audio_mixing_playback_volume(IntPtr engine);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_get_audio_mixing_publish_volume(IntPtr engine);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_set_audio_mixing_position(IntPtr engine, double position);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern double rcrtc_get_audio_mixing_position(IntPtr engine);

        [DllImport(RTC_Lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rcrtc_get_audio_mixing_duration(IntPtr engine);
        #endregion

    }
}
#endif