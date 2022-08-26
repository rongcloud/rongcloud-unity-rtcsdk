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
    internal class RCRTCEngineIOS : RCRTCEngine
    {
        internal RCRTCEngineIOS(RCRTCEngineSetup setup)
        {
            rtc_set_engine_listeners(
                on_rtc_error,
                on_rtc_kicked,
                on_rtc_room_joined,
                on_rtc_room_left,
                on_rtc_published,
                on_rtc_unpublished,
                on_rtc_subscribed,
                on_rtc_unsubscribed,
                on_rtc_live_mix_subscribed,
                on_rtc_live_mix_unsubscribed,
                on_rtc_enable_camera,
                on_rtc_switch_camera,
                on_rtc_live_cdn_added,
                on_rtc_live_cdn_removed,
                on_rtc_live_mix_layout_mode_set,
                on_rtc_live_mix_render_mode_set,
                on_rtc_live_mix_custom_layouts_set,
                on_rtc_live_mix_custom_audios_set,
                on_rtc_live_mix_video_bitrate_set,
                on_rtc_live_mix_video_resolution_set,
                on_rtc_live_mix_video_fps_set,
                on_rtc_live_mix_audio_bitrate_set,
                on_rtc_audio_effect_created,
                on_rtc_audio_effect_finished,
                on_rtc_audio_mixing_started,
                on_rtc_audio_mixing_paused,
                on_rtc_audio_mixing_stopped,
                on_rtc_audio_mixing_finished,
                on_rtc_user_joined,
                on_rtc_user_offline,
                on_rtc_user_left,
                on_rtc_remote_published,
                on_rtc_remote_unpublished,
                on_rtc_remote_live_mix_published,
                on_rtc_remote_live_mix_unpublished,
                on_rtc_remote_state_changed,
                on_rtc_remote_first_frame,
                on_rtc_remote_live_mix_first_frame
                );
            if (setup == null)
            {
                engine = rtc_create_engine();
            }
            else
            {
                rtc_engine_setup cobject = toEngineSetup(setup);
                engine = rtc_create_engine_with_setup(ref cobject);
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
                rtc_release_engine(engine);
                engine = IntPtr.Zero;
            }

            Instance = null;
            StatsListener = null;
            AudioListeners.Clear();
            AudioListeners = null;
            base.Destroy();
        }

        public override int JoinRoom(String roomId, RCRTCRoomSetup setup)
        {
            rtc_room_setup cobject = toRoomSetup(setup);
            int ret = rtc_join_room(engine, roomId, ref cobject);
            return ret;
        }

        public override int LeaveRoom()
        {
            return rtc_leave_room(engine);
        }

        public override int Publish(RCRTCMediaType type)
        {
            return rtc_publish(engine, (int)type);
        }

        public override int Unpublish(RCRTCMediaType type)
        {
            return rtc_unpublish(engine, (int)type);
        }

        public override int Subscribe(String userId, RCRTCMediaType type, bool tiny)
        {
            return rtc_subscribe(engine, userId, (int)type, tiny);
        }

        public override int Subscribe(IList<String> userIds, RCRTCMediaType type, bool tiny)
        {
            List<String> list = new List<string>(userIds);
            return rtc_subscribe_with_user_ids(engine, list.ToArray(), list.Count, (int)type, tiny);
        }

        public override int SubscribeLiveMix(RCRTCMediaType type, bool tiny)
        {
            return rtc_subscribe_live_mix(engine, (int)type, tiny);
        }

        public override int Unsubscribe(String userId, RCRTCMediaType type)
        {
            return rtc_unsubscribe(engine, userId, (int)type);
        }

        public override int Unsubscribe(IList<String> userIds, RCRTCMediaType type)
        {
            List<String> list = new List<string>(userIds);
            return rtc_unsubscribe_with_user_ids(engine, list.ToArray(), list.Count, (int)type);
        }

        public override int UnsubscribeLiveMix(RCRTCMediaType type)
        {
            return rtc_unsubscribe_live_mix(engine, (int)type);
        }

        public override int SetAudioConfig(RCRTCAudioConfig config)
        {
            rtc_audio_config cobject = toAudioConfig(config);
            return rtc_set_audio_config(engine, ref cobject);
        }

        public override int SetVideoConfig(RCRTCVideoConfig config, bool tiny)
        {
            rtc_video_config cobject = toVideoConfig(config);
            return rtc_set_video_config(engine, ref cobject, tiny);
        }

        public override int EnableMicrophone(bool enable)
        {
            return rtc_enable_microphone(engine, enable);
        }

        public override int EnableSpeaker(bool enable)
        {
            return rtc_enable_speaker(engine, enable);
        }

        public override int AdjustLocalVolume(int volume)
        {
            return rtc_adjust_local_volume(engine, volume);
        }

        public override int EnableCamera(bool enable, RCRTCCamera camera)
        {
            return rtc_enable_camera(engine, enable, (int)camera);
        }

        public override int SwitchCamera()
        {
            return rtc_switch_camera(engine);
        }

        public override RCRTCCamera WhichCamera()
        {
            return (RCRTCCamera)rtc_which_camera(engine);
        }

        public override bool IsCameraFocusSupported()
        {
            return rtc_is_camera_focus_supported(engine);
        }

        public override bool IsCameraExposurePositionSupported()
        {
            return rtc_is_camera_exposure_position_supported(engine);
        }

        public override int SetCameraFocusPositionInPreview(double x, double y)
        {
            return rtc_set_camera_focus_position_in_preview(engine, x, y);
        }

        public override int SetCameraExposurePositionInPreview(double x, double y)
        {
            return rtc_set_camera_exposure_position_in_preview(engine, x, y);
        }

        public override int SetCameraCaptureOrientation(RCRTCCameraCaptureOrientation orientation)
        {
            return rtc_set_camera_capture_orientation(engine, (int)orientation);
        }

        public override int MuteLocalStream(RCRTCMediaType type, bool mute)
        {
            return rtc_mute_local_stream(engine, (int)type, mute);
        }

        public override int MuteRemoteStream(String userId, RCRTCMediaType type, bool mute)
        {
            return rtc_mute_remote_stream(engine, userId, (int)type, mute);
        }

        public override int MuteAllRemoteAudioStreams(bool mute)
        {
            return rtc_mute_all_remote_audio_streams(engine, mute);
        }

        private IntPtr toNativeView(RCRTCView view)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = typeof(RCRTCView);
            MethodInfo method = type.GetMethod("InitNativeView", flag);
            object[] arguments = { engine };
            method.Invoke(view, arguments);
            FieldInfo field = type.GetField("_view", flag);
            return (IntPtr) field.GetValue(view);
        }

        public override int SetLocalView(RCRTCView view)
        {
            return rtc_set_local_view(engine, toNativeView(view));
        }

        public override int RemoveLocalView()
        {
            return rtc_remove_local_view(engine);
        }

        public override int SetRemoteView(String userId, RCRTCView view)
        {
            return rtc_set_remote_view(engine, userId, toNativeView(view));
        }

        public override int RemoveRemoteView(String userId)
        {
            return rtc_remove_remote_view(engine, userId);
        }

        public override int SetLiveMixView(RCRTCView view)
        {
            return rtc_set_live_mix_view(engine, toNativeView(view));
        }

        public override int RemoveLiveMixView()
        {
            return rtc_remove_live_mix_view(engine);
        }

        public override int AddLiveCdn(String url)
        {
            return rtc_add_live_cdn(engine, url);
        }

        public override int RemoveLiveCdn(String url)
        {
            return rtc_remove_live_cdn(engine, url);
        }

        public override int SetLiveMixLayoutMode(RCRTCLiveMixLayoutMode mode)
        {
            return rtc_set_live_mix_layout_mode(engine, (int)mode);
        }

        public override int SetLiveMixRenderMode(RCRTCLiveMixRenderMode mode)
        {
            return rtc_set_live_mix_render_mode(engine, (int)mode);
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

            return rtc_set_live_mix_custom_layouts(engine, ref cobjects, layouts.Count);
        }

        public override int SetLiveMixCustomAudios(IList<String> userIds)
        {
            if (userIds == null || userIds.Count == 0)
                return -1;
            List<String> list = new List<string>(userIds);
            return rtc_set_live_mix_custom_audios(engine, list.ToArray(), list.Count);
        }

        public override int SetLiveMixVideoBitrate(int bitrate, bool tiny)
        {
            return rtc_set_live_mix_video_bitrate(engine, bitrate, tiny);
        }

        public override int SetLiveMixVideoResolution(int width, int height, bool tiny)
        {
            return rtc_set_live_mix_video_resolution(engine, width, height, tiny);
        }

        public override int SetLiveMixVideoFps(RCRTCVideoFps fps, bool tiny)
        {
            return rtc_set_live_mix_video_fps(engine, (int)fps, tiny);
        }

        public override int SetLiveMixAudioBitrate(int bitrate)
        {
            return rtc_set_live_mix_audio_bitrate(engine, bitrate);
        }

        public override int SetStatsListener(RCRTCStatsListener listener)
        {
            rtc_stats_listener_proxy proxy = toStatsListenerProxy(listener);
            return rtc_set_stats_listener(engine, ref proxy);
        }

        public override int CreateAudioEffect(String path, int effectId)
        {
            return rtc_create_audio_effect(engine, path, effectId);
        }

        public override int ReleaseAudioEffect(int effectId)
        {
            return rtc_release_audio_effect(engine, effectId);
        }

        public override int PlayAudioEffect(int effectId, int volume, int loop)
        {
            return rtc_play_audio_effect(engine, effectId, volume, loop);
        }

        public override int PauseAudioEffect(int effectId)
        {
            return rtc_pause_audio_effect(engine, effectId);
        }

        public override int PauseAllAudioEffects()
        {
            return rtc_pause_all_audio_effects(engine);
        }

        public override int ResumeAudioEffect(int effectId)
        {
            return rtc_resume_audio_effect(engine, effectId);
        }

        public override int ResumeAllAudioEffects()
        {
            return rtc_resume_all_audio_effects(engine);
        }

        public override int StopAudioEffect(int effectId)
        {
            return rtc_stop_audio_effect(engine, effectId);
        }

        public override int StopAllAudioEffects()
        {
            return rtc_stop_all_audio_effects(engine);
        }

        public override int AdjustAudioEffectVolume(int effectId, int volume)
        {
            return rtc_adjust_audio_effect_volume(engine, effectId, volume);
        }

        public override int GetAudioEffectVolume(int effectId)
        {
            return rtc_get_audio_effect_volume(engine, effectId);
        }

        public override int AdjustAllAudioEffectsVolume(int volume)
        {
            return rtc_adjust_all_audio_effects_volume(engine, volume);
        }

        public override int StartAudioMixing(String path, RCRTCAudioMixingMode mode, bool playback, int loop)
        {
            return rtc_start_audio_mixing(engine, path, (int)mode, playback, loop);
        }

        public override int StopAudioMixing()
        {
            return rtc_stop_audio_mixing(engine);
        }

        public override int PauseAudioMixing()
        {
            return rtc_pause_audio_mixing(engine);
        }

        public override int ResumeAudioMixing()
        {
            return rtc_resume_audio_mixing(engine);
        }

        public override int AdjustAudioMixingVolume(int volume)
        {
            return rtc_adjust_audio_mixing_volume(engine, volume);
        }

        public override int AdjustAudioMixingPlaybackVolume(int volume)
        {
            return rtc_adjust_audio_mixing_playback_volume(engine, volume);
        }

        public override int AdjustAudioMixingPublishVolume(int volume)
        {
            return rtc_adjust_audio_mixing_publish_volume(engine, volume);
        }

        public override int GetAudioMixingPlaybackVolume()
        {
            return rtc_get_audio_mixing_playback_volume(engine);
        }

        public override int GetAudioMixingPublishVolume()
        {
            return rtc_get_audio_mixing_publish_volume(engine);
        }

        public override int SetAudioMixingPosition(double position)
        {
            return rtc_set_audio_mixing_position(engine, position);
        }

        public override double GetAudioMixingPosition()
        {
            return rtc_get_audio_mixing_position(engine);
        }

        public override int GetAudioMixingDuration()
        {
            return rtc_get_audio_mixing_duration(engine);
        }

        public override int SetLocalAudioCapturedListener(RCRTCOnWritableAudioFrameListener listener)
        {
            rtc_writable_audio_frame_listener_proxy proxy = toWritableAudioFrameListener("", listener);
            return rtc_set_local_audio_captured_listener(engine, ref proxy);
        }

        public override int SetRemoteAudioReceivedListener(String userId, RCRTCOnWritableAudioFrameListener listener)
        {
            rtc_writable_audio_frame_listener_proxy proxy = toWritableAudioFrameListener(userId, listener);
            return rtc_set_remote_audio_received_listener(engine, userId, ref proxy);
        }

        public override String GetSessionId()
        {
            return rtc_get_session_rtc_id(engine);
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

            return cobject;
        }

        private rtc_room_setup toRoomSetup(RCRTCRoomSetup setup)
        {
            rtc_room_setup cobject;
            cobject.role = (int)setup.GetRole();
            cobject.type = (int)setup.GetMediaType();
            return cobject;
        }

        private rtc_audio_config toAudioConfig(RCRTCAudioConfig config)
        {
            rtc_audio_config cobject;
            cobject.quality = (int)config.GetQuality();
            cobject.scenario = (int)config.GetScenario();
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
            return proxy;
        }

        private rtc_writable_audio_frame_listener_proxy toWritableAudioFrameListener(String userId, RCRTCOnWritableAudioFrameListener listener)
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

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct rtc_audio_setup
        {
            public int audioCodecType;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct rtc_video_setup
        {
            [MarshalAs(UnmanagedType.U1)] public bool enableTinyStream;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct rtc_engine_setup
        {
            [MarshalAs(UnmanagedType.U1)] public bool reconnectable;

            public int statsReportInterval;

            [MarshalAs(UnmanagedType.U1)] public bool enableSRTP;
            public IntPtr audioSetup;
            public IntPtr videoSetup;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct rtc_room_setup
        {
            public int role;
            public int type;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct rtc_audio_config
        {
            public int quality;
            public int scenario;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct rtc_video_config
        {
            public int minBitrate;
            public int maxBitrate;
            public int fps;
            public int resolution;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct rtc_custom_layout 
        {
            public int type;
            public string id;
            public string tag;
            public int x;
            public int y;
            public int width;
            public int height;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct rtc_stats_listener_proxy
        {
            [MarshalAs(UnmanagedType.U1)] public bool remove;
            public OnNetworkStats onNetworkStats;
            public OnLocalAudioStats onLocalAudioStats;
            public OnLocalVideoStats onLocalVideoStats;
            public OnRemoteAudioStats onRemoteAudioStats;
            public OnRemoteVideoStats onRemoteVideoStats;
            public OnLiveMixAudioStats onLiveMixAudioStats;
            public OnLiveMixVideoStats onLiveMixVideoStats;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct rtc_network_stats
        {
            public int type;
            public string ip;
            public int sendBitrate;
            public int receiveBitrate;
            public int rtt;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct rtc_local_audio_stats
        {
            public int codec;
            public int bitrate;
            public int volume;
            public double packageLostRate;
            public int rtt;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct rtc_local_video_stats
        {
            [MarshalAs(UnmanagedType.U1)] public bool tiny;
            public int codec;
            public int bitrate;
            public int fps;
            public int width;
            public int height;
            public double packageLostRate;
            public int rtt;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct rtc_remote_audio_stats
        {
            public int codec;
            public int bitrate;
            public int volume;
            public double packageLostRate;
            public int rtt;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct rtc_remote_video_stats
        {
            public int codec;
            public int bitrate;
            public int fps;
            public int width;
            public int height;
            public double packageLostRate;
            public int rtt;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct rtc_writable_audio_frame_listener_proxy
        {
            [MarshalAs(UnmanagedType.U1)] public bool remove;
            public OnAudioFrame onAudioFrame;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct rtc_audio_frame
        {
            public IntPtr data;
            public int length;
            public int channels;
            public int sampleRate;
            public int bytesPerSample;
            public int samples;
        }
        
        delegate void OnNetworkStats(ref rtc_network_stats cstats);

        delegate void OnLocalAudioStats(ref rtc_local_audio_stats cstats);

        delegate void OnLocalVideoStats(ref rtc_local_video_stats cstats);

        delegate void OnRemoteAudioStats(string userId, ref rtc_remote_audio_stats cstats);

        delegate void OnRemoteVideoStats(string userId, ref rtc_remote_video_stats cstats);

        delegate void OnLiveMixAudioStats(ref rtc_remote_audio_stats cstats);

        delegate void OnLiveMixVideoStats(ref rtc_remote_video_stats cstats);

        delegate void OnAudioFrame(String userId, ref rtc_audio_frame cframe);

        [MonoPInvokeCallback(typeof(OnNetworkStats))]
        static void on_rtc_network_stats(ref rtc_network_stats cstats)
        {
            RCRTCNetworkStats stats = new RCRTCNetworkStats((RCRTCNetworkType)cstats.type, cstats.ip,
                cstats.sendBitrate, cstats.receiveBitrate, cstats.rtt);
            StatsListener?.OnNetworkStats(stats);
        }

        [MonoPInvokeCallback(typeof(OnLocalAudioStats))]
        static void on_rtc_local_audio_stats(ref rtc_local_audio_stats cstats)
        {
            RCRTCLocalAudioStats stats = new RCRTCLocalAudioStats((RCRTCAudioCodecType)cstats.codec, cstats.bitrate,
                cstats.volume, cstats.packageLostRate, cstats.rtt);
            StatsListener?.OnLocalAudioStats(stats);
        }

        [MonoPInvokeCallback(typeof(OnLocalVideoStats))]
        static void on_rtc_local_video_stats(ref rtc_local_video_stats cstats)
        {
            RCRTCLocalVideoStats stats = new RCRTCLocalVideoStats(cstats.tiny, (RCRTCVideoCodecType)cstats.codec, cstats.bitrate,
                cstats.fps, cstats.width, cstats.height, cstats.packageLostRate, cstats.rtt);
            StatsListener?.OnLocalVideoStats(stats);
        }

        [MonoPInvokeCallback(typeof(OnRemoteAudioStats))]
        static void on_rtc_remote_audio_stats(string userId, ref rtc_remote_audio_stats cstats)
        {
            RCRTCRemoteAudioStats stats = new RCRTCRemoteAudioStats((RCRTCAudioCodecType)cstats.codec, cstats.bitrate,
                cstats.volume, cstats.packageLostRate, cstats.rtt);
            StatsListener?.OnRemoteAudioStats(userId, stats);
        }

        [MonoPInvokeCallback(typeof(OnRemoteVideoStats))]
        static void on_rtc_remote_video_stats(string userId, ref rtc_remote_video_stats cstats)
        {
            RCRTCRemoteVideoStats stats = new RCRTCRemoteVideoStats((RCRTCVideoCodecType)cstats.codec, cstats.bitrate,
                cstats.fps, cstats.width, cstats.height, cstats.packageLostRate, cstats.rtt);
            StatsListener?.OnRemoteVideoStats(userId, stats);
        }

        [MonoPInvokeCallback(typeof(OnLiveMixAudioStats))]
        static void on_rtc_live_mix_audio_stats(ref rtc_remote_audio_stats cstats)
        {
            RCRTCRemoteAudioStats stats = new RCRTCRemoteAudioStats((RCRTCAudioCodecType)cstats.codec, cstats.bitrate,
                cstats.volume, cstats.packageLostRate, cstats.rtt);
            StatsListener?.OnLiveMixAudioStats(stats);
        }

        [MonoPInvokeCallback(typeof(OnLiveMixVideoStats))]
        static void on_rtc_live_mix_video_stats(ref rtc_remote_video_stats cstats)
        {
            RCRTCRemoteVideoStats stats = new RCRTCRemoteVideoStats((RCRTCVideoCodecType)cstats.codec, cstats.bitrate,
                cstats.fps, cstats.width, cstats.height, cstats.packageLostRate, cstats.rtt);
            StatsListener?.OnLiveMixVideoStats(stats);
        }

        [MonoPInvokeCallback(typeof(OnAudioFrame))]
        static void on_rtc_audio_frame(string id, ref rtc_audio_frame cframe)
        {
            byte[] data = new byte[cframe.length];
            Marshal.Copy(cframe.data, data, 0, cframe.length);
            RCRTCAudioFrame frame = new RCRTCAudioFrame(data, cframe.length, cframe.channels, cframe.sampleRate,
                cframe.bytesPerSample, cframe.samples);
            data = AudioListeners[id]?.OnAudioFrame(ref frame);
            Marshal.Copy(data, 0, cframe.data, data.Length);
        }

        [MonoPInvokeCallback(typeof(OnErrorDelegate))]
        static void on_rtc_error(int code, string message)
        {
            Instance?.OnError?.Invoke(code, message);
        }

        [MonoPInvokeCallback(typeof(OnKickedDelegate))]
        static void on_rtc_kicked(string roomId, string message)
        {
            Instance?.OnKicked?.Invoke(roomId, message);
        }

        [MonoPInvokeCallback(typeof(OnRoomJoinedDelegate))]
        static void on_rtc_room_joined(int code, string message)
        {
            Debug.Log("on_rtc_room_joined code = " + code + ", message = " + message);
            Instance?.OnRoomJoined?.Invoke(code, message);
        }

        [MonoPInvokeCallback(typeof(OnRoomLeftDelegate))]
        static void on_rtc_room_left(int code, string message)
        {
            Instance?.OnRoomLeft?.Invoke(code, message);
        }

        [MonoPInvokeCallback(typeof(OnPublishedDelegate))]
        static void on_rtc_published(RCRTCMediaType type, int code, string message)
        {
            Instance?.OnPublished?.Invoke(type, code, message);
        }

        [MonoPInvokeCallback(typeof(OnUnpublishedDelegate))]
        static void on_rtc_unpublished(RCRTCMediaType type, int code, string message)
        {
            Instance?.OnUnpublished?.Invoke(type, code, message);
        }

        [MonoPInvokeCallback(typeof(OnSubscribedDelegate))]
        static void on_rtc_subscribed(string id, RCRTCMediaType type, int code, string message)
        {
            Instance?.OnSubscribed?.Invoke(id, type, code, message);
        }

        [MonoPInvokeCallback(typeof(OnUnsubscribedDelegate))]
        static void on_rtc_unsubscribed(string id, RCRTCMediaType type, int code, string message)
        {
            Instance?.OnUnsubscribed?.Invoke(id, type, code, message);
        }

        [MonoPInvokeCallback(typeof(OnLiveMixSubscribedDelegate))]
        static void on_rtc_live_mix_subscribed(RCRTCMediaType type, int code, string message)
        {
            Instance?.OnLiveMixSubscribed?.Invoke(type, code, message);
        }

        [MonoPInvokeCallback(typeof(OnLiveMixUnsubscribedDelegate))]
        static void on_rtc_live_mix_unsubscribed(RCRTCMediaType type, int code, string message)
        {
            Instance?.OnLiveMixUnsubscribed?.Invoke(type, code, message);
        }

        [MonoPInvokeCallback(typeof(OnCameraEnabledDelegate))]
        static void on_rtc_enable_camera(bool enable, int code, string message)
        {
            Instance?.OnCameraEnabled?.Invoke(enable, code, message);
        }

        [MonoPInvokeCallback(typeof(OnCameraSwitchedDelegate))]
        static void on_rtc_switch_camera(RCRTCCamera camera, int code, string message)
        {
            Instance?.OnCameraSwitched?.Invoke(camera, code, message);
        }

        [MonoPInvokeCallback(typeof(OnLiveCdnAddedDelegate))]
        static void on_rtc_live_cdn_added(string url, int code, string message)
        {
            Instance?.OnLiveCdnAdded?.Invoke(url, code, message);
        }

        [MonoPInvokeCallback(typeof(OnLiveCdnRemovedDelegate))]
        static void on_rtc_live_cdn_removed(string url, int code, string message)
        {
            Instance?.OnLiveCdnRemoved?.Invoke(url, code, message);
        }

        [MonoPInvokeCallback(typeof(OnLiveMixLayoutModeSetDelegate))]
        static void on_rtc_live_mix_layout_mode_set(int code, string message)
        {
            Instance?.OnLiveMixLayoutModeSet?.Invoke(code, message);
        }

        [MonoPInvokeCallback(typeof(OnLiveMixRenderModeSetDelegate))]
        static void on_rtc_live_mix_render_mode_set(int code, string message)
        {
            Instance?.OnLiveMixRenderModeSet?.Invoke(code, message);
        }

        [MonoPInvokeCallback(typeof(OnLiveMixCustomLayoutsSetDelegate))]
        static void on_rtc_live_mix_custom_layouts_set(int code, string message)
        {
            Instance?.OnLiveMixCustomLayoutsSet?.Invoke(code, message);
        }

        [MonoPInvokeCallback(typeof(OnLiveMixCustomAudiosSetDelegate))]
        static void on_rtc_live_mix_custom_audios_set(int code, string message)
        {
            Instance?.OnLiveMixCustomAudiosSet?.Invoke(code, message);
        }

        [MonoPInvokeCallback(typeof(OnLiveMixVideoBitrateSetDelegate))]
        static void on_rtc_live_mix_video_bitrate_set(bool tiny, int code, string message)
        {
            Instance?.OnLiveMixVideoBitrateSet?.Invoke(tiny, code, message);
        }

        [MonoPInvokeCallback(typeof(OnLiveMixVideoResolutionSetDelegate))]
        static void on_rtc_live_mix_video_resolution_set(bool tiny, int code, string message)
        {
            Instance?.OnLiveMixVideoResolutionSet?.Invoke(tiny, code, message);
        }

        [MonoPInvokeCallback(typeof(OnLiveMixVideoFpsSetDelegate))]
        static void on_rtc_live_mix_video_fps_set(bool tiny, int code, string message)
        {
            Instance?.OnLiveMixVideoFpsSet?.Invoke(tiny, code, message);
        }

        [MonoPInvokeCallback(typeof(OnLiveMixAudioBitrateSetDelegate))]
        static void on_rtc_live_mix_audio_bitrate_set(int code, string message)
        {
            Instance?.OnLiveMixAudioBitrateSet?.Invoke(code, message);
        }

        [MonoPInvokeCallback(typeof(OnAudioEffectCreatedDelegate))]
        static void on_rtc_audio_effect_created(int id, int code, string message)
        {
            Instance?.OnAudioEffectCreated?.Invoke(id, code, message);
        }

        [MonoPInvokeCallback(typeof(OnAudioEffectFinishedDelegate))]
        static void on_rtc_audio_effect_finished(int id)
        {
            Instance?.OnAudioEffectFinished?.Invoke(id);
        }

        [MonoPInvokeCallback(typeof(OnAudioMixingStartedDelegate))]
        static void on_rtc_audio_mixing_started()
        {
            Instance?.OnAudioMixingStarted?.Invoke();
        }

        [MonoPInvokeCallback(typeof(OnAudioMixingPausedDelegate))]
        static void on_rtc_audio_mixing_paused()
        {
            Instance?.OnAudioMixingPaused?.Invoke();
        }

        [MonoPInvokeCallback(typeof(OnAudioMixingStoppedDelegate))]
        static void on_rtc_audio_mixing_stopped()
        {
            Instance?.OnAudioMixingStopped?.Invoke();
        }

        [MonoPInvokeCallback(typeof(OnAudioMixingFinishedDelegate))]
        static void on_rtc_audio_mixing_finished()
        {
            Instance?.OnAudioMixingFinished?.Invoke();
        }

        [MonoPInvokeCallback(typeof(OnUserJoinedDelegate))]
        static void on_rtc_user_joined(string id)
        {
            Instance?.OnUserJoined?.Invoke(id);
        }

        [MonoPInvokeCallback(typeof(OnUserOfflineDelegate))]
        static void on_rtc_user_offline(string id)
        {
            Instance?.OnUserOffline?.Invoke(id);
        }

        [MonoPInvokeCallback(typeof(OnUserLeftDelegate))]
        static void on_rtc_user_left(string id)
        {
            Instance?.OnUserLeft?.Invoke(id);
        }

        [MonoPInvokeCallback(typeof(OnRemotePublishedDelegate))]
        static void on_rtc_remote_published(string id, RCRTCMediaType type)
        {
            Instance?.OnRemotePublished?.Invoke(id, type);
        }

        [MonoPInvokeCallback(typeof(OnRemoteUnpublishedDelegate))]
        static void on_rtc_remote_unpublished(string id, RCRTCMediaType type)
        {
            Instance?.OnRemoteUnpublished?.Invoke(id, type);
        }

        [MonoPInvokeCallback(typeof(OnRemoteLiveMixPublishedDelegate))]
        static void on_rtc_remote_live_mix_published(RCRTCMediaType type)
        {
            Instance?.OnRemoteLiveMixPublished?.Invoke(type);
        }

        [MonoPInvokeCallback(typeof(OnRemoteLiveMixUnpublishedDelegate))]
        static void on_rtc_remote_live_mix_unpublished(RCRTCMediaType type)
        {
            Instance?.OnRemoteLiveMixUnpublished?.Invoke(type);
        }

        [MonoPInvokeCallback(typeof(OnRemoteStateChangedDelegate))]
        static void on_rtc_remote_state_changed(string id, RCRTCMediaType type, bool disabled)
        {
            Instance?.OnRemoteStateChanged?.Invoke(id, type, disabled);
        }

        [MonoPInvokeCallback(typeof(OnRemoteFirstFrameDelegate))]
        static void on_rtc_remote_first_frame(string id, RCRTCMediaType type)
        {
            Instance?.OnRemoteFirstFrame?.Invoke(id, type);
        }

        [MonoPInvokeCallback(typeof(OnRemoteLiveMixFirstFrameDelegate))]
        static void on_rtc_remote_live_mix_first_frame(RCRTCMediaType type)
        {
            Instance?.OnRemoteLiveMixFirstFrame?.Invoke(type);
        }

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern void rtc_set_engine_listeners(
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
            OnCameraSwitchedDelegate on_rtc_switch_camera,
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

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern IntPtr rtc_create_engine();

        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr rtc_create_engine_with_setup(ref rtc_engine_setup setup);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern void rtc_release_engine(IntPtr engine);

        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int rtc_join_room(IntPtr engine, string id, ref rtc_room_setup config);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_leave_room(IntPtr engine);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_publish(IntPtr engine, int type);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_unpublish(IntPtr engine, int type);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_subscribe(IntPtr engine, string id, int type, bool tiny);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_subscribe_with_user_ids(IntPtr engine, string[] ids, int count, int type, bool tiny);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_subscribe_live_mix(IntPtr engine, int type, bool tiny);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_unsubscribe(IntPtr engine, string id, int type);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_unsubscribe_with_user_ids(IntPtr engine, string[] ids, int count, int type);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_unsubscribe_live_mix(IntPtr engine, int type);

        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int rtc_set_audio_config(IntPtr engine, ref rtc_audio_config config);

        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int rtc_set_video_config(IntPtr engine, ref rtc_video_config config, bool tiny);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_enable_microphone(IntPtr engine, bool enable);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_enable_speaker(IntPtr engine, bool enable);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_adjust_local_volume(IntPtr engine, int volume);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_enable_camera(IntPtr engine, bool enable, int camera);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_switch_camera(IntPtr engine);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_which_camera(IntPtr engine);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern bool rtc_is_camera_focus_supported(IntPtr engine);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern bool rtc_is_camera_exposure_position_supported(IntPtr engine);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_set_camera_focus_position_in_preview(IntPtr engine, double x, double y);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_set_camera_exposure_position_in_preview(IntPtr engine, double x, double y);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_set_camera_capture_orientation(IntPtr engine, int orientation);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_mute_local_stream(IntPtr engine, int type, bool mute);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_mute_remote_stream(IntPtr engine, string id, int type, bool mute);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_mute_all_remote_audio_streams(IntPtr engine, bool mute);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_set_local_view(IntPtr engine, IntPtr view);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_remove_local_view(IntPtr engine);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_set_remote_view(IntPtr engine, string id, IntPtr view);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_remove_remote_view(IntPtr engine, string id);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_set_live_mix_view(IntPtr engine, IntPtr view);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_remove_live_mix_view(IntPtr engine);
        
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_add_live_cdn(IntPtr engine, string url);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_remove_live_cdn(IntPtr engine, string url);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_set_live_mix_layout_mode(IntPtr engine, int mode);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_set_live_mix_render_mode(IntPtr engine, int mode);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_set_live_mix_custom_layouts(IntPtr engine, ref rtc_custom_layout[] layouts, int count);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_set_live_mix_custom_audios(IntPtr engine, string[] ids, int count);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_set_live_mix_video_bitrate(IntPtr engine, int bitrate, bool tiny);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_set_live_mix_video_resolution(IntPtr engine, int width, int height, bool tiny);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_set_live_mix_video_fps(IntPtr engine, int fps, bool tiny);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_set_live_mix_audio_bitrate(IntPtr engine, int bitrate);

        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int rtc_set_stats_listener(IntPtr engine, ref rtc_stats_listener_proxy proxy);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_create_audio_effect(IntPtr engine, string path, int id);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_release_audio_effect(IntPtr engine, int id);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_play_audio_effect(IntPtr engine, int id, int volume, int loop);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_pause_audio_effect(IntPtr engine, int id);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_pause_all_audio_effects(IntPtr engine);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_resume_audio_effect(IntPtr engine, int id);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_resume_all_audio_effects(IntPtr engine);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_stop_audio_effect(IntPtr engine, int id);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_stop_all_audio_effects(IntPtr engine);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_adjust_audio_effect_volume(IntPtr engine, int id, int volume);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_get_audio_effect_volume(IntPtr engine, int id);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_adjust_all_audio_effects_volume(IntPtr engine, int volume);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_start_audio_mixing(IntPtr engine, string path, int mode, bool playback, int loop);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_stop_audio_mixing(IntPtr engine);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_pause_audio_mixing(IntPtr engine);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_resume_audio_mixing(IntPtr engine);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_adjust_audio_mixing_volume(IntPtr engine, int volume);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_adjust_audio_mixing_playback_volume(IntPtr engine, int volume);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_adjust_audio_mixing_publish_volume(IntPtr engine, int volume);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_get_audio_mixing_playback_volume(IntPtr engine);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_get_audio_mixing_publish_volume(IntPtr engine);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_set_audio_mixing_position(IntPtr engine, double position);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern double rtc_get_audio_mixing_position(IntPtr engine);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_get_audio_mixing_duration(IntPtr engine);

        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int rtc_set_local_audio_captured_listener(IntPtr engine, ref rtc_writable_audio_frame_listener_proxy proxy);

        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int rtc_set_remote_audio_received_listener(IntPtr engine, string id, ref rtc_writable_audio_frame_listener_proxy proxy);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern string rtc_get_session_rtc_id(IntPtr engine);

        private IntPtr engine = IntPtr.Zero;

        private static RCRTCEngineIOS Instance = null;

        private static RCRTCStatsListener StatsListener = null;

        private static Dictionary<String, RCRTCOnWritableAudioFrameListener> AudioListeners = null;
    }
}

#endif
