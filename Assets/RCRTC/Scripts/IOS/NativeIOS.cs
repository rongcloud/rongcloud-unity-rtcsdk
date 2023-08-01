#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

namespace cn_rongcloud_rtc_unity
{
    public class NativeIOS
    {
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern IntPtr rtc_create_engine();
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern IntPtr rtc_create_engine_with_setup(ref rtc_engine_setup setup);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern void rtc_release_engine(IntPtr engine);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_join_room(IntPtr engine, string id, ref rtc_room_setup config);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_leave_room(IntPtr engine);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_publish(IntPtr engine, int type);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_unpublish(IntPtr engine, int type);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_subscribe(IntPtr engine, string id, int type, bool tiny);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_subscribe_with_user_ids(IntPtr engine, string[] ids, int count, int type, bool tiny);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_subscribe_live_mix(IntPtr engine, int type, bool tiny);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_unsubscribe(IntPtr engine, string id, int type);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_unsubscribe_with_user_ids(IntPtr engine, string[] ids, int count, int type);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_unsubscribe_live_mix(IntPtr engine, int type);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_audio_config(IntPtr engine, ref rtc_audio_config config);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_video_config(IntPtr engine, ref rtc_video_config config, bool tiny);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_enable_microphone(IntPtr engine, bool enable);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_enable_speaker(IntPtr engine, bool enable);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_adjust_local_volume(IntPtr engine, int volume);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_enable_camera(IntPtr engine, bool enable, int camera);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_switch_camera(IntPtr engine);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_which_camera(IntPtr engine);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern bool rtc_is_camera_focus_supported(IntPtr engine);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern bool rtc_is_camera_exposure_position_supported(IntPtr engine);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_camera_focus_position_in_preview(IntPtr engine, double x, double y);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_camera_exposure_position_in_preview(IntPtr engine, double x, double y);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_camera_capture_orientation(IntPtr engine, int orientation);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_mute_local_stream(IntPtr engine, int type, bool mute);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_mute_remote_stream(IntPtr engine, string id, int type, bool mute);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_mute_all_remote_audio_streams(IntPtr engine, bool mute);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_local_view(IntPtr engine, IntPtr view);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_remove_local_view(IntPtr engine);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_remote_view(IntPtr engine, string id, IntPtr view);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_remove_remote_view(IntPtr engine, string id);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_live_mix_view(IntPtr engine, IntPtr view);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_remove_live_mix_view(IntPtr engine);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_live_mix_inner_cdn_stream_view(IntPtr engine, IntPtr view);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_add_live_cdn(IntPtr engine, string url);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_remove_live_cdn(IntPtr engine, string url);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_live_mix_layout_mode(IntPtr engine, int mode);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_live_mix_render_mode(IntPtr engine, int mode);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_live_mix_custom_layouts(IntPtr engine, ref rtc_custom_layout[] layouts,
                                                                   int count);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_live_mix_custom_audios(IntPtr engine, string[] ids, int count);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_live_mix_video_bitrate(IntPtr engine, int bitrate, bool tiny);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_live_mix_video_resolution(IntPtr engine, int width, int height, bool tiny);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_live_mix_video_fps(IntPtr engine, int fps, bool tiny);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_live_mix_audio_bitrate(IntPtr engine, int bitrate);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_stats_listener(IntPtr engine, ref rtc_stats_listener_proxy proxy);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_create_audio_effect(IntPtr engine, string path, int id);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_release_audio_effect(IntPtr engine, int id);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_play_audio_effect(IntPtr engine, int id, int volume, int loop);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_pause_audio_effect(IntPtr engine, int id);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_pause_all_audio_effects(IntPtr engine);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_resume_audio_effect(IntPtr engine, int id);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_resume_all_audio_effects(IntPtr engine);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_stop_audio_effect(IntPtr engine, int id);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_stop_all_audio_effects(IntPtr engine);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_adjust_audio_effect_volume(IntPtr engine, int id, int volume);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_get_audio_effect_volume(IntPtr engine, int id);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_adjust_all_audio_effects_volume(IntPtr engine, int volume);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_start_audio_mixing(IntPtr engine, string path, int mode, bool playback, int loop);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_stop_audio_mixing(IntPtr engine);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_pause_audio_mixing(IntPtr engine);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_resume_audio_mixing(IntPtr engine);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_adjust_audio_mixing_volume(IntPtr engine, int volume);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_adjust_audio_mixing_playback_volume(IntPtr engine, int volume);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_adjust_audio_mixing_publish_volume(IntPtr engine, int volume);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_get_audio_mixing_playback_volume(IntPtr engine);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_get_audio_mixing_publish_volume(IntPtr engine);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_audio_mixing_position(IntPtr engine, double position);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern double rtc_get_audio_mixing_position(IntPtr engine);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_get_audio_mixing_duration(IntPtr engine);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_local_audio_captured_listener(IntPtr engine,
                                                                         ref rtc_writable_audio_frame_listener_proxy proxy);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_remote_audio_received_listener(
            IntPtr engine, string id, ref rtc_writable_audio_frame_listener_proxy proxy);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern string rtc_get_session_rtc_id(IntPtr engine);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_start_network_Probe(ref ios_network_probe_proxy proxy);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_watermark(string cpath, double x, double y, double zoom);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_create_custom_stream_from_file(string path, string tag, bool replace, bool playback);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_custom_stream_video_config(string tag, ref rtc_video_config config);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_local_custom_stream_view(string tag, IntPtr view);
    
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_remote_custom_stream_view(string userId, string tag, IntPtr view);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_mute_live_mix_stream(RCRTCMediaType type, bool mute);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_live_mix_background_color(int color);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_mute_local_custom_stream(string tag, bool mute);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_remove_local_custom_stream_view(string tag);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_publish_custom_stream(string tag);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_unpublish_custom_stream(string tag);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_mute_remote_custom_stream(string userId, string tag, RCRTCMediaType type, bool mute);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_remove_remote_custom_stream_view(string userId, string tag);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_subscribe_custom_stream(string userId, string tag, RCRTCMediaType type, bool tiny);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_unsubscribe_custom_stream(string userId, string tag, RCRTCMediaType type);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_request_join_sub_room(string roomId, string userId, bool autoLayout, string extra);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_cancel_join_sub_room_request(string roomId, string userId, string extra);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_response_join_sub_room_request(string roomId, string userId, bool agree,
                                                                      bool autoLayout, string extra);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_join_sub_room(string roomId);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_leave_sub_room(string roomId, bool disband);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_switch_live_role(RCRTCRole role);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_enable_live_mix_inner_cdn_stream(bool enable);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_subscribe_live_mix_inner_cdn_stream();
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_unsubscribe_live_mix_inner_cdn_stream();
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_remove_live_mix_inner_cdn_stream_view();
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_local_live_mix_inner_cdn_video_resolution(int width, int height);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_set_local_live_mix_inner_cdn_video_fps(RCRTCVideoFps fps);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_mute_live_mix_inner_cdn_stream(bool mute);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_remove_watermark();
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_stop_network_probe();
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_start_echo_test(int timeInterval);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_stop_echo_test();
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_enable_sei(bool enable);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_send_sei(string sei);
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int rtc_preconnect_to_media_server();
    
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void rtc_set_IRCRTCIWListener(
            OnLiveMixBackgroundColorSet onLiveMixBackgroundColorSet,
            OnAudioMixingProgressReported onAudioMixingProgressReported, OnCustomStreamPublished onCustomStreamPublished,
            OnCustomStreamPublishFinished onCustomStreamPublishFinished,
            OnCustomStreamUnpublished onCustomStreamUnpublished,
            OnRemoteCustomStreamPublished onRemoteCustomStreamPublished,
            OnRemoteCustomStreamUnpublished onRemoteCustomStreamUnpublished,
            OnRemoteCustomStreamStateChanged onRemoteCustomStreamStateChanged,
            OnRemoteCustomStreamFirstFrame onRemoteCustomStreamFirstFrame,
            OnCustomStreamSubscribed onCustomStreamSubscribed, OnCustomStreamUnsubscribed onCustomStreamUnsubscribed,
            OnJoinSubRoomRequested onJoinSubRoomRequested, OnJoinSubRoomRequestCanceled onJoinSubRoomRequestCanceled,
            OnJoinSubRoomRequestResponded onJoinSubRoomRequestResponded,
            OnJoinSubRoomRequestReceived onJoinSubRoomRequestReceived,
            OnCancelJoinSubRoomRequestReceived onCancelJoinSubRoomRequestReceived,
            OnJoinSubRoomRequestResponseReceived onJoinSubRoomRequestResponseReceived, OnSubRoomJoined onSubRoomJoined,
            OnSubRoomLeft onSubRoomLeft, OnSubRoomBanded onSubRoomBanded, OnSubRoomDisband onSubRoomDisband,
            OnLiveRoleSwitched onLiveRoleSwitched, OnRemoteLiveRoleSwitched onRemoteLiveRoleSwitched,
            OnLiveMixInnerCdnStreamEnabled onLiveMixInnerCdnStreamEnabled,
            OnRemoteLiveMixInnerCdnStreamPublished onRemoteLiveMixInnerCdnStreamPublished,
            OnRemoteLiveMixInnerCdnStreamUnpublished onRemoteLiveMixInnerCdnStreamUnpublished,
            OnLiveMixInnerCdnStreamSubscribed onLiveMixInnerCdnStreamSubscribed,
            OnLiveMixInnerCdnStreamUnsubscribed onLiveMixInnerCdnStreamUnsubscribed,
            OnLocalLiveMixInnerCdnVideoResolutionSet onLocalLiveMixInnerCdnVideoResolutionSet,
            OnLocalLiveMixInnerCdnVideoFpsSet onLocalLiveMixInnerCdnVideoFpsSet, OnWatermarkSet onWatermarkSet,
            OnWatermarkRemoved onWatermarkRemoved, OnNetworkProbeStarted onNetworkProbeStarted,
            OnNetworkProbeStopped onNetworkProbeStopped, OnSeiEnabled onSeiEnabled, OnSeiReceived onSeiReceived,
            OnLiveMixSeiReceived onLiveMixSeiReceived);
    }
}
#endif
