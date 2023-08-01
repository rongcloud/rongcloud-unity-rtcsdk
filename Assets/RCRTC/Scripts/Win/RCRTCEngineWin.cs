
#if UNITY_STANDALONE_WIN
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using cn_rongcloud_im_unity;

namespace cn_rongcloud_rtc_unity
{
    internal partial class RCRTCEngineWin : RCRTCEngine
    {
        private static RCRTCEngineWin Instance = null;
        private IntPtr rtc_engine = IntPtr.Zero;

        internal RCRTCEngineWin(RCRTCEngineSetup setup)
        {
            IntPtr im_client = IntPtr.Zero;
            if (RCIMEngine.instance is RCIMWinEngine)
            {
                im_client = ((RCIMWinEngine)RCIMEngine.instance).im_client;
            }
            if (im_client == IntPtr.Zero)
            {
                throw new Exception("请先初始化im引擎!");
            }
            NativeWin.rcrtc_set_engine_listeners(
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
            NativeWin.rcrtc_set_new_listener(
                on_custom_stream_published,
                on_custom_stream_publish_finished,
                on_custom_stream_unpublished,
                on_remote_custom_stream_published,
                on_remote_custom_stream_unpublished,
                on_remote_custom_stream_state_changed,
                on_remote_custom_stream_first_frame,
                on_custom_stream_subscribed,
                on_custom_stream_unsubscribed,
                on_live_role_switched,
                on_remote_live_role_switched,
                on_live_mix_background_color_set
                );
            NativeWin.rcrtc_set_camera_change_callback(on_rtc_camera_list_changed);
            NativeWin.rcrtc_set_microphone_change_callback(on_rtc_microphone_list_changed);
            NativeWin.rcrtc_set_speaker_change_callback(on_rtc_speaker_list_changed);
            if (rtc_engine == IntPtr.Zero)
            {
                if (setup == null)
                {
                    rtc_engine = NativeWin.rcrtc_create_engine(im_client);
                }
                else
                {
                    rtc_engine_setup cobject = toEngineSetup(setup);
                    rtc_engine = NativeWin.rcrtc_create_engine_with_setup(im_client, ref cobject);
                    if (cobject.audioSetup != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(cobject.audioSetup);
                    }
                    if (cobject.videoSetup != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(cobject.videoSetup);
                    }
                }
            }

            Instance = this;
        }

        public override void Destroy()
        {
            if (rtc_engine != IntPtr.Zero)
            {
                NativeWin.rcrtc_engine_destory(rtc_engine);
                rtc_engine = IntPtr.Zero;
            }
            Instance = null;
            video_render_listeners.Clear();
            video_frame_listeners.Clear();
            audio_frame_listeners.Clear();
            StatsListener = null;
            base.Destroy();
        }

        public override int JoinRoom(String roomId, RCRTCRoomSetup setup)
        {
            if (setup == null)
            {
                int ret = NativeWin.rcrtc_join_room(rtc_engine, roomId);
                return ret;
            }
            else
            {
                rtc_room_setup cobject = toRoomSetup(setup);
                int ret = NativeWin.rcrtc_join_room_with_setup(rtc_engine, roomId, ref cobject);
                return ret;
            }
        }

        public override int LeaveRoom()
        {
            return NativeWin.rcrtc_leave_room(rtc_engine);
        }

        public override int Publish(RCRTCMediaType type)
        {
            return NativeWin.rcrtc_publish(rtc_engine, (int)type);
        }

        public override int Unpublish(RCRTCMediaType type)
        {
            return NativeWin.rcrtc_unpublish(rtc_engine, (int)type);
        }

        public override int Subscribe(String userId, RCRTCMediaType type, bool tiny = false)
        {
            return NativeWin.rcrtc_subscribe(rtc_engine, userId, (int)type, tiny);
        }

        public override int Subscribe(IList<String> userIds, RCRTCMediaType type, bool tiny = false)
        {
            List<String> list = new List<string>(userIds);
            return NativeWin.rcrtc_subscribe_with_user_ids(rtc_engine, list.ToArray(), list.Count, (int)type, tiny);
        }

        public override int SubscribeLiveMix(RCRTCMediaType type, bool tiny)
        {
            return NativeWin.rcrtc_subscribe_live_mix(rtc_engine, (int)type, tiny);
        }

        public override int Unsubscribe(String userId, RCRTCMediaType type)
        {
            return NativeWin.rcrtc_unsubscribe(rtc_engine, userId, (int)type);
        }

        public override int Unsubscribe(IList<String> userIds, RCRTCMediaType type)
        {
            List<String> list = new List<string>(userIds);
            return NativeWin.rcrtc_unsubscribe_with_user_ids(rtc_engine, list.ToArray(), list.Count, (int)type);
        }

        public override int UnsubscribeLiveMix(RCRTCMediaType type)
        {
            return NativeWin.rcrtc_unsubscribe_live_mix(rtc_engine, (int)type);
        }

        public override int SetAudioConfig(RCRTCAudioConfig config)
        {
            rtc_audio_config cobject = toAudioConfig(config);
            return NativeWin.rcrtc_set_audio_config(rtc_engine, ref cobject);
        }

        public override int SetVideoConfig(RCRTCVideoConfig config, bool tiny)
        {
            rtc_video_config cobject = toVideoConfig(config);
            return NativeWin.rcrtc_set_video_config(rtc_engine, ref cobject, tiny);
        }

        public override int AdjustLocalVolume(int volume)
        {
            return NativeWin.rcrtc_adjust_local_volume(rtc_engine, volume);
        }

        public override int MuteLocalStream(RCRTCMediaType type, bool mute)
        {
            return NativeWin.rcrtc_mute_local_stream(rtc_engine, (int)type, mute);
        }

        public override int MuteRemoteStream(String userId, RCRTCMediaType type, bool mute)
        {
            return NativeWin.rcrtc_mute_remote_stream(rtc_engine, userId, (int)type, mute);
        }

        public override int MuteAllRemoteAudioStreams(bool mute)
        {
            return NativeWin.rcrtc_mute_all_remote_audio_streams(rtc_engine, mute);
        }

        #region Device
        public override RCRTCDevice[] GetCameraList()
        {
            Int32 count = 0;
            IntPtr ptr = NativeWin.rcrtc_get_camera_list(rtc_engine, ref count);
            RCRTCDevice[] list = new RCRTCDevice[count];
            if (count > 0 && ptr != IntPtr.Zero)
            {
                rtc_device[] c_list = new rtc_device[count];
                NativeUtils.GetStructListByPtr<rtc_device>(ref c_list, ptr, count);
                for (int i = 0; i < c_list.Length; i++)
                {
                    RCRTCDevice camera = new RCRTCDevice();
                    camera.name = c_list[i].name;
                    camera.id = c_list[i].id;
                    camera.index = c_list[i].index;
                    list[i] = camera;
                }
            }
            return list;
        }

        public override int EnableCamera(RCRTCDevice camera, bool enable, bool asDefault)
        {
            if (enable)
            {
                rtc_device cobject = toDevice(camera);
                return NativeWin.rcrtc_enable_camera(rtc_engine, ref cobject, asDefault);
            }
            else
            {
                rtc_device cobject = toDevice(camera);
                return NativeWin.rcrtc_disable_camera(rtc_engine, ref cobject);
            }
        }

        public override int SwitchCamera(RCRTCDevice camera)
        {
            rtc_device cobject = toDevice(camera);
            return NativeWin.rcrtc_switch_to_camera(rtc_engine, ref cobject);
        }

        public override RCRTCDevice WhichCamera()
        {
            rtc_device cobject = toDevice(new RCRTCDevice());
            NativeWin.rcrtc_which_camera(rtc_engine, ref cobject);
            RCRTCDevice camera = new RCRTCDevice
            {
                name = cobject.name,
                id = cobject.id,
                index = cobject.index
            };
            return camera;
        }

        public override RCRTCDevice[] GetMicrophoneList()
        {
            Int32 count = 0;
            IntPtr ptr = NativeWin.rcrtc_get_microphone_list(rtc_engine, ref count);
            RCRTCDevice[] list = new RCRTCDevice[count];
            if (count > 0 && ptr != IntPtr.Zero)
            {
                rtc_device[] c_list = new rtc_device[count];
                NativeUtils.GetStructListByPtr<rtc_device>(ref c_list, ptr, count);
                for (int i = 0; i < c_list.Length; i++)
                {
                    RCRTCDevice mic = new RCRTCDevice();
                    mic.name = c_list[i].name;
                    mic.id = c_list[i].id;
                    mic.index = c_list[i].index;
                    list[i] = mic;
                }
            }
            return list;
        }

        public override int EnableMicrophone(RCRTCDevice microphone, bool enable, bool asDefault)
        {
            if (enable)
            {
                rtc_device cobject = toDevice(microphone);
                return NativeWin.rcrtc_enable_microphone(rtc_engine, ref cobject, asDefault);
            }
            else
            {
                rtc_device cobject = toDevice(microphone);
                return NativeWin.rcrtc_disable_microphone(rtc_engine, ref cobject);
            }
        }

        public override RCRTCDevice[] GetSpeakerList()
        {
            Int32 count = 0;
            IntPtr ptr = NativeWin.rcrtc_get_speaker_list(rtc_engine, ref count);
            RCRTCDevice[] list = new RCRTCDevice[count];
            if (count > 0 && ptr != IntPtr.Zero)
            {
                rtc_device[] c_list = new rtc_device[count];
                NativeUtils.GetStructListByPtr<rtc_device>(ref c_list, ptr, count);
                for (int i = 0; i < c_list.Length; i++)
                {
                    RCRTCDevice speaker = new RCRTCDevice();
                    speaker.name = c_list[i].name;
                    speaker.id = c_list[i].id;
                    speaker.index = c_list[i].index;
                    list[i] = speaker;
                }
            }
            return list;
        }

        public override int EnableSpeaker(RCRTCDevice speaker, bool enable)
        {
            if (enable)
            {
                rtc_device cobject = toDevice(speaker);
                return NativeWin.rcrtc_enable_speaker(rtc_engine, ref cobject);
            }
            else
            {
                rtc_device cobject = toDevice(speaker);
                return NativeWin.rcrtc_disable_speaker(rtc_engine, ref cobject);
            }
        }
        #endregion

        #region Video Data
        public override int SetLocalView(RCRTCView view)
        {
            rtc_video_listener_proxy proxy = toVideoListener(view, "");
            return NativeWin.rcrtc_set_local_video_listener(rtc_engine, ref proxy);
        }

        public override int RemoveLocalView()
        {
            rtc_video_listener_proxy proxy = toVideoListener(null, "");
            return NativeWin.rcrtc_set_local_video_listener(rtc_engine, ref proxy);
        }

        public override int SetRemoteView(String userId, RCRTCView view)
        {
            rtc_video_listener_proxy proxy = toVideoListener(view, userId);
            return NativeWin.rcrtc_set_remote_video_listener(rtc_engine, userId, ref proxy);
        }

        public override int RemoveRemoteView(String userId)
        {
            rtc_video_listener_proxy proxy = toVideoListener(null, userId);
            return NativeWin.rcrtc_set_remote_video_listener(rtc_engine, userId, ref proxy);
        }

        public override int SetLiveMixView(RCRTCView view)
        {
            rtc_video_listener_proxy proxy = toVideoListener(view, "RCRongLiveMix");
            return NativeWin.rcrtc_set_live_mix_video_listener(rtc_engine, ref proxy);
        }

        public override int RemoveLiveMixView()
        {
            rtc_video_listener_proxy proxy = toVideoListener(null, "RCRongLiveMix");
            return NativeWin.rcrtc_set_live_mix_video_listener(rtc_engine, ref proxy);
        }
        #endregion

        #region Audio Data
        public override int SetLocalAudioCapturedListener(RCRTCOnWritableAudioFrameListener listener)
        {
            rtc_writable_audio_frame_listener_proxy proxy = toWritableAudioFrameListener("", listener);
            return NativeWin.rcrtc_set_local_audio_captured_listener(rtc_engine, ref proxy);
        }

        public override int SetRemoteAudioReceivedListener(String userId, RCRTCOnWritableAudioFrameListener listener)
        {
            rtc_writable_audio_frame_listener_proxy proxy = toWritableAudioFrameListener(userId, listener);
            return NativeWin.rcrtc_set_remote_audio_received_listener(rtc_engine, userId, ref proxy);
        }
        /*
        public override int SetLocalVideoCapturedListener(RCRTCOnWritableVideoFrameListener listener)
        {
            video_frame_listeners[""] = listener;
            return 0;
        }

        public override int SetRemoteVideoReceivedListener(String userId, RCRTCOnWritableVideoFrameListener listener)
        {
            video_frame_listeners[userId] = listener;
            return 0;
        }*/
        #endregion

        #region LiveMix
        public override int AddLiveCdn(String url)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_add_live_cdn(rtc_engine, url);
        }

        public override int RemoveLiveCdn(String url)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_remove_live_cdn(rtc_engine, url);
        }

        public override int SetLiveMixLayoutMode(RCRTCLiveMixLayoutMode mode)
        {
            return NativeWin.rcrtc_set_live_mix_layout_mode(rtc_engine, (int)mode-1);
        }

        public override int SetLiveMixRenderMode(RCRTCLiveMixRenderMode mode)
        {
            return NativeWin.rcrtc_set_live_mix_render_mode(rtc_engine, (int)mode-1);
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

            return NativeWin.rcrtc_set_live_mix_custom_layouts(rtc_engine, ref cobjects, layouts.Count);
        }

        public override int SetLiveMixCustomAudios(IList<String> userIds)
        {
            if (userIds == null || userIds.Count == 0)
                return -1;
            List<String> list = new List<string>(userIds);
            return NativeWin.rcrtc_set_live_mix_custom_audios(rtc_engine, list.ToArray(), list.Count);
        }

        public override int SetLiveMixVideoBitrate(int bitrate, bool tiny)
        {
            return NativeWin.rcrtc_set_live_mix_video_bitrate(rtc_engine, bitrate, tiny);
        }
        public override int SetLiveMixVideoResolution(int width, int height, bool tiny)
        {
            return NativeWin.rcrtc_set_live_mix_video_resolution(rtc_engine, width, height, tiny);
        }

        public override int SetLiveMixVideoFps(RCRTCVideoFps fps, bool tiny)
        {
            return NativeWin.rcrtc_set_live_mix_video_fps(rtc_engine, (int)fps, tiny);
        }

        public override int SetLiveMixAudioBitrate(int bitrate)
        {
            return NativeWin.rcrtc_set_live_mix_audio_bitrate(rtc_engine, bitrate);
        }

        public override int MuteLiveMixStream(RCRTCMediaType type, bool mute)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int SetLiveMixBackgroundColor(int color)
        {
            return NativeWin.rcrtc_set_live_mix_background_color(rtc_engine, color);
        }

        public override int EnableLiveMixInnerCdnStream(bool enable)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int SubscribeLiveMixInnerCdnStream()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int UnsubscribeLiveMixInnerCdnStream()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int SetLiveMixInnerCdnStreamView(RCRTCView view)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int RemoveLiveMixInnerCdnStreamView()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int SetLocalLiveMixInnerCdnVideoResolution(int width, int height)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int SetLocalLiveMixInnerCdnVideoFps(RCRTCVideoFps fps)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int MuteLiveMixInnerCdnStream(bool mute)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }
        #endregion

        #region Audio Effect
        public override int CreateAudioEffect(String path, int effectId)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_create_audio_effect(rtc_engine, path, effectId);
        }

        public override int ReleaseAudioEffect(int effectId)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_release_audio_effect(rtc_engine, effectId);
        }

        public override int PlayAudioEffect(int effectId, int volume, int loop)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_play_audio_effect(rtc_engine, effectId, volume, loop);
        }

        public override int PauseAudioEffect(int effectId)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_pause_audio_effect(rtc_engine, effectId);
        }

        public override int PauseAllAudioEffects()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_pause_all_audio_effects(rtc_engine);
        }

        public override int ResumeAudioEffect(int effectId)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_resume_audio_effect(rtc_engine, effectId);
        }

        public override int ResumeAllAudioEffects()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_resume_all_audio_effects(rtc_engine);
        }

        public override int StopAudioEffect(int effectId)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_stop_audio_effect(rtc_engine, effectId);
        }

        public override int StopAllAudioEffects()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_stop_all_audio_effects(rtc_engine);
        }

        public override int AdjustAudioEffectVolume(int effectId, int volume)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_adjust_audio_effect_volume(rtc_engine, effectId, volume);
        }

        public override int GetAudioEffectVolume(int effectId)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_get_audio_effect_volume(rtc_engine, effectId);
        }

        public override int AdjustAllAudioEffectsVolume(int volume)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_adjust_all_audio_effects_volume(rtc_engine, volume);
        }
        #endregion

        #region Audio Mix
        public override int StartAudioMixing(String path, RCRTCAudioMixingMode mode, bool playback, int loop)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_start_audio_mixing(rtc_engine, path, (int)mode, playback, loop);
        }

        public override int StopAudioMixing()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_stop_audio_mixing(rtc_engine);
        }

        public override int PauseAudioMixing()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_pause_audio_mixing(rtc_engine);
        }

        public override int ResumeAudioMixing()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_resume_audio_mixing(rtc_engine);
        }

        public override int AdjustAudioMixingVolume(int volume)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_adjust_audio_mixing_volume(rtc_engine, volume);
        }

        public override int AdjustAudioMixingPlaybackVolume(int volume)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_adjust_audio_mixing_playback_volume(rtc_engine, volume);
        }

        public override int AdjustAudioMixingPublishVolume(int volume)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_adjust_audio_mixing_publish_volume(rtc_engine, volume);
        }

        public override int GetAudioMixingPlaybackVolume()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_get_audio_mixing_playback_volume(rtc_engine);
        }

        public override int GetAudioMixingPublishVolume()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_get_audio_mixing_publish_volume(rtc_engine);
        }

        public override int SetAudioMixingPosition(double position)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_set_audio_mixing_position(rtc_engine, position);
        }

        public override double GetAudioMixingPosition()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_get_audio_mixing_position(rtc_engine);
        }

        public override int GetAudioMixingDuration()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
            //return NativeWin.rcrtc_get_audio_mixing_duration(rtc_engine);
        }

        public override String GetSessionId()
        {
            return NativeWin.rcrtc_get_session_id(rtc_engine);
        }
        #endregion

        #region Utils
        public override int SwitchLiveRole(RCRTCRole role)
        {
            return NativeWin.rcrtc_switch_live_role(rtc_engine, (int)role);
        }

        public override int StartNetworkProbe(RCRTCNetworkProbeListener listener)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int StopNetworkProbe()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int SetWatermark(string path, double x, double y, double zoom)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int RemoveWatermark()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int StartEchoTest(int timeInterval)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int StopEchoTest()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int EnableSei(bool enable)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int SendSei(string sei)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int PreconnectToMediaServer()
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }
        #endregion

        #region CustomStream
        public override int CreateCustomStreamFromFile(string path, string tag, bool replace, bool playback)
        {
            return NativeWin.rcrtc_create_custom_stream_from_file(rtc_engine, path, tag, replace, playback);
        }

        public override int SetCustomStreamVideoConfig(string tag, RCRTCVideoConfig config)
        {
            rtc_video_config cconfig = toVideoConfig(config);
            return NativeWin.rcrtc_set_custom_stream_video_config(rtc_engine, tag, ref cconfig);
        }

        public override int MuteLocalCustomStream(string tag, bool mute)
        {
            return NativeWin.rcrtc_mute_local_custom_stream(rtc_engine, tag, mute);
        }

        public override int SetLocalCustomStreamView(string tag, RCRTCView view)
        {
            rtc_video_listener_proxy proxy = toVideoListener(view, "RCRongLocalCustom"+tag);
            return NativeWin.rcrtc_set_local_custom_stream_video_listener(rtc_engine, tag, ref proxy);
        }

        public override int RemoveLocalCustomStreamView(string tag)
        {
            rtc_video_listener_proxy proxy = toVideoListener(null, "RCRongLocalCustom" + tag);
            return NativeWin.rcrtc_set_local_custom_stream_video_listener(rtc_engine, tag, ref proxy);
        }

        public override int PublishCustomStream(string tag)
        {
            return NativeWin.rcrtc_publich_custom_stream(rtc_engine, tag);
        }

        public override int UnpublishCustomStream(string tag)
        {
            return NativeWin.rcrtc_unpublich_custom_stream(rtc_engine, tag);
        }

        public override int MuteRemoteCustomStream(string userId, string tag, RCRTCMediaType type, bool mute)
        {
            return NativeWin.rcrtc_mute_remote_custom_stream(rtc_engine, userId, tag, (int)type, mute);
        }

        public override int SetRemoteCustomStreamView(string uerId, string tag, RCRTCView view)
        {
            rtc_video_listener_proxy proxy = toVideoListener(view, "RCRongRemoteCustom" + uerId + tag);
            return NativeWin.rcrtc_set_remote_custom_stream_video_listener(rtc_engine, uerId, tag, ref proxy);
        }

        public override int RemoveRemoteCustomStreamView(string userId, string tag)
        {
            rtc_video_listener_proxy proxy = toVideoListener(null, "RCRongRemoteCustom" + userId + tag);
            return NativeWin.rcrtc_set_remote_custom_stream_video_listener(rtc_engine, userId, tag, ref proxy);
        }

        public override int SubscribeCustomStream(string userId, string tag, RCRTCMediaType type, bool tiny)
        {
            return NativeWin.rcrtc_subscribe_custom_stream(rtc_engine, userId, tag, (int)type);
        }

        public override int UnsubscribeCustomStream(string userId, string tag, RCRTCMediaType type)
        {
            return NativeWin.rcrtc_unsubscribe_custom_stream(rtc_engine, userId, tag, (int)type);
        }
        #endregion

        #region SubRoom
        public override int RequestJoinSubRoom(string roomId, string userId, bool autoLayout, string extra)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int CancelJoinSubRoomRequest(string roomId, string userId, string extra)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int ResponseJoinSubRoomRequest(string roomId, string userId, bool agree, bool autoLayout, string extra)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int JoinSubRoom(string roomId)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }

        public override int LeaveSubRoom(string roomId, bool disband)
        {
            Debug.Log("此接口暂时不支持");
            return -1;
        }
        #endregion

        #region Stats Listener
        public override int SetStatsListener(RCRTCStatsListener listener)
        {
            rtc_stats_listener_proxy proxy = toStatsListenerProxy(listener);
            return NativeWin.rcrtc_set_stats_listener(rtc_engine, ref proxy);
        }        
        #endregion

        private static RCRTCStatsListener StatsListener = null;
        private static Dictionary<string, RCRTCOnVideoFrameListener> video_render_listeners = new Dictionary<string, RCRTCOnVideoFrameListener>();
        private static Dictionary<string, RCRTCOnWritableAudioFrameListener> audio_frame_listeners = new Dictionary<string, RCRTCOnWritableAudioFrameListener>();
        private static Dictionary<string, RCRTCOnWritableVideoFrameListener> video_frame_listeners = new Dictionary<string, RCRTCOnWritableVideoFrameListener>();

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

            if (setup.GetMediaUrl() != null)
            {
                cobject.mediaUrl = setup.GetMediaUrl();
            }
            else
            {
                cobject.mediaUrl = "";
            }

            if (setup.GetLogPath() != null)
            {
                cobject.logPath = setup.GetLogPath();
            }
            else
            {
                cobject.logPath = "";
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

        private rtc_video_listener_proxy toVideoListener(RCRTCOnVideoFrameListener listener, string uid)
        {
            video_render_listeners[uid] = listener;
            rtc_video_listener_proxy proxy;
            proxy.remove = listener == null;
            proxy.onVideoFrame = on_rtc_video_frame;
            return proxy;
        }

        private rtc_writable_audio_frame_listener_proxy toWritableAudioFrameListener(String uid, RCRTCOnWritableAudioFrameListener listener)
        {
            audio_frame_listeners[uid] = listener;
            rtc_writable_audio_frame_listener_proxy proxy;
            proxy.remove = listener == null;
            proxy.onAudioFrame = on_rtc_audio_frame;
            return proxy;
        }

        private rtc_device toDevice(RCRTCDevice device)
        {
            rtc_device cobject;
            cobject.name = device.name;
            cobject.id = device.id;
            cobject.index = device.index;
            return cobject;
        }

        private rtc_video_config toVideoConfig(RCRTCVideoConfig config)
        {
            rtc_video_config cobject;
            cobject.minBitrate = config.GetMinBitrate();
            cobject.maxBitrate = config.GetMaxBitrate();
            switch (config.GetFps())
            {
                case RCRTCVideoFps.FPS_10:
                    cobject.fps = 10;
                    break;
                case RCRTCVideoFps.FPS_15:
                    cobject.fps = 15;
                    break;
                case RCRTCVideoFps.FPS_24:
                    cobject.fps = 24;
                    break;
                case RCRTCVideoFps.FPS_30:
                    cobject.fps = 30;
                    break;
                default:
                    cobject.fps = 15;
                    break;
            }
            cobject.resolution = (int)config.GetResolution()+1;
            return cobject;
        }

        private rtc_audio_config toAudioConfig(RCRTCAudioConfig config)
        {
            rtc_audio_config cobject;
            cobject.quality = (int)config.GetQuality();
            cobject.scenario = (int)config.GetScenario();
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

    }
}

#endif

