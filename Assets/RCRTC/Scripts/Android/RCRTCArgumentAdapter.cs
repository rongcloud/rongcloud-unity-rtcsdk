using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cn_rongcloud_rtc_unity
{
    public class ArgumentAdapter
    {
        private static AndroidJavaClass EngineSetupBuilderClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.setup.RCRTCIWEngineSetup$Builder");
        private static AndroidJavaClass AudioSetupBuilderClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.setup.RCRTCIWAudioSetup$Builder");
        private static AndroidJavaClass VideoSetupBuilderClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.setup.RCRTCIWVideoSetup$Builder");
        private static AndroidJavaClass RoomSetupBuilderClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.setup.RCRTCIWRoomSetup$Builder");
        private const string AudioConfigClass = "cn.rongcloud.rtc.wrapper.config.RCRTCIWAudioConfig";
        private const string VideoConfigClass = "cn.rongcloud.rtc.wrapper.config.RCRTCIWVideoConfig";
        private static AndroidJavaClass AudioCodecTypeClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWAudioCodecType");
        private static AndroidJavaClass AudioQualityClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWAudioQuality");
        private static AndroidJavaClass AudioScenarioClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWAudioScenario");
        private static AndroidJavaClass CameraClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWCamera");
        private static AndroidJavaClass MediaTypeClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWMediaType");
        private static AndroidJavaClass JoinTypeClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWJoinType");
        private static AndroidJavaClass RoleClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWRole");
        private static AndroidJavaClass AudioMixModeClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWAudioMixingMode");
        private static AndroidJavaClass VideoFpsClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWVideoFps");
        private static AndroidJavaClass VideoResolutionClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWVideoResolution");
        private static AndroidJavaClass CameraCaptureOrientationClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWCameraCaptureOrientation");
        private static AndroidJavaClass LiveMixLayoutModeClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWLiveMixLayoutMode");
        private static AndroidJavaClass LiveMixRenderModeClass = new AndroidJavaClass("cn.rongcloud.rtc.wrapper.constants.RCRTCIWLiveMixRenderMode");

        private const string CustomLayoutClassFullName = "cn.rongcloud.rtc.wrapper.module.RCRTCIWCustomLayout";

        public static AndroidJavaObject toEngineSetup(RCRTCEngineSetup setup)
        {
            if (setup == null) return null;
            AndroidJavaObject jobject = EngineSetupBuilderClass.CallStatic<AndroidJavaObject>("create")
                                        .Call<AndroidJavaObject>("withReconnectable", setup.IsReconnectable())
                                        .Call<AndroidJavaObject>("withStatsReportInterval", setup.GetStatsReportInterval())
                                        .Call<AndroidJavaObject>("withEnableSRTP", setup.IsEnableSRTP())
                                        .Call<AndroidJavaObject>("withAudioSetup", toAudioSetup(setup.GetAudioSetup()))
                                        .Call<AndroidJavaObject>("withVideoSetup", toVideoSetup(setup.GetVideoSetup()))
                                        .Call<AndroidJavaObject>("withEnableVersionMatch", false)
                                        .Call<AndroidJavaObject>("withMediaUrl", setup.GetMediaUrl())
                                        .Call<AndroidJavaObject>("build");
            return jobject;
        }

        public static AndroidJavaObject toAudioSetup(RCRTCAudioSetup setup)
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

        public static AndroidJavaObject toVideoSetup(RCRTCVideoSetup setup)
        {
            if (setup == null) return null;
            AndroidJavaObject jobject = VideoSetupBuilderClass.CallStatic<AndroidJavaObject>("create")
                                        .Call<AndroidJavaObject>("withEnableTinyStream", setup.IsEnableTinyStream())
                                        .Call<AndroidJavaObject>("withEnableEncoderTexture", setup.IsEnableTexture())
                                        .Call<AndroidJavaObject>("withHardwareEncoderFrameRate", setup.GetHardwareEncoderFrameRate())
                                        .Call<AndroidJavaObject>("withEnableHardwareEncoderHighProfile", setup.IsEnableHardwareEncoderHighProfile())
                                        .Call<AndroidJavaObject>("withEnableHardwareEncoder", setup.IsEnableHardwareEncoder())
                                        .Call<AndroidJavaObject>("withEnableHardwareDecoder", setup.IsEnableHardwareDecoder())
                                        .Call<AndroidJavaObject>("build");
            return jobject;
        }

        public static AndroidJavaObject toRoomSetup(RCRTCRoomSetup setup)
        {
            AndroidJavaObject jobject = RoomSetupBuilderClass.CallStatic<AndroidJavaObject>("create")
                                        .Call<AndroidJavaObject>("withRole", toRole(setup.GetRole()))
                                        .Call<AndroidJavaObject>("withMediaType", toMediaType(setup.GetMediaType()))
                                        .Call<AndroidJavaObject>("withJoinType", toJoinType(setup.GetJoinType()))
                                        .Call<AndroidJavaObject>("build");
            return jobject;
        }

        public static AndroidJavaObject toAudioConfig(RCRTCAudioConfig config)
        {
            AndroidJavaObject jobject = new AndroidJavaObject(AudioConfigClass);
            jobject.Call<AndroidJavaObject>("setQuality", toAudioQuality(config.GetQuality()));
            jobject.Call<AndroidJavaObject>("setScenario", toAudioScenario(config.GetScenario()));
            jobject.Call<AndroidJavaObject>("setRecordingVolume", config.GetRecordingVolume());
            return jobject;
        }

        public static AndroidJavaObject toVideoConfig(RCRTCVideoConfig config)
        {
            AndroidJavaObject jobject = new AndroidJavaObject(VideoConfigClass);
            jobject.Call<AndroidJavaObject>("setMinBitrate", config.GetMinBitrate());
            jobject.Call<AndroidJavaObject>("setMaxBitrate", config.GetMaxBitrate());
            jobject.Call<AndroidJavaObject>("setFps", toVideoFps(config.GetFps()));
            jobject.Call<AndroidJavaObject>("setResolution", toVideoResolution(config.GetResolution()));
            return jobject;
        }

        public static AndroidJavaObject toAudioCodecType(RCRTCAudioCodecType type)
        {
            return AudioCodecTypeClass.GetStatic<AndroidJavaObject>(type.ToString());
        }

        public static AndroidJavaObject toAudioQuality(RCRTCAudioQuality quality)
        {
            return AudioQualityClass.GetStatic<AndroidJavaObject>(quality.ToString());
        }

        public static AndroidJavaObject toAudioScenario(RCRTCAudioScenario scenario)
        {
            return AudioScenarioClass.GetStatic<AndroidJavaObject>(scenario.ToString());
        }

        public static AndroidJavaObject toCamera(RCRTCCamera camera)
        {
            return CameraClass.GetStatic<AndroidJavaObject>(camera.ToString());
        }

        public static AndroidJavaObject toMediaType(RCRTCMediaType type)
        {
            return MediaTypeClass.GetStatic<AndroidJavaObject>(type.ToString());
        }

        private static AndroidJavaObject toJoinType(RCRTCJoinType type)
        {
            return JoinTypeClass.GetStatic<AndroidJavaObject>(type.ToString());
        }

        public static AndroidJavaObject toRole(RCRTCRole role)
        {
            return RoleClass.GetStatic<AndroidJavaObject>(role.ToString());
        }

        public static AndroidJavaObject toAudioMixMode(RCRTCAudioMixingMode mode)
        {
            return AudioMixModeClass.GetStatic<AndroidJavaObject>(mode.ToString());
        }

        public static AndroidJavaObject toVideoFps(RCRTCVideoFps fps)
        {
            return VideoFpsClass.GetStatic<AndroidJavaObject>(fps.ToString());
        }

        public static AndroidJavaObject toVideoResolution(RCRTCVideoResolution resolution)
        {
            return VideoResolutionClass.GetStatic<AndroidJavaObject>(resolution.ToString());
        }

        public static AndroidJavaObject toCameraCaptureOrientation(RCRTCCameraCaptureOrientation orientation)
        {
            return CameraCaptureOrientationClass.GetStatic<AndroidJavaObject>(orientation.ToString());
        }

        public static AndroidJavaObject toLiveMixLayoutMode(RCRTCLiveMixLayoutMode mode)
        {
            return LiveMixLayoutModeClass.GetStatic<AndroidJavaObject>(mode.ToString());
        }

        public static AndroidJavaObject toLiveMixRenderMode(RCRTCLiveMixRenderMode mode)
        {
            return LiveMixRenderModeClass.GetStatic<AndroidJavaObject>(mode.ToString());
        }

        public static AndroidJavaObject toCustomLayout(RCRTCCustomLayout layout)
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

        public static AndroidJavaObject toCustomLayouts(IList<RCRTCCustomLayout> layouts)
        {
            var arrayList = new AndroidJavaObject("java.util.ArrayList");
            for (int i = 0; i < layouts.Count; ++i)
            {
                arrayList.Call<bool>("add", toCustomLayout(layouts[i]));
            }

            return arrayList;
        }

        public static AndroidJavaObject toStringList(IList<string> stringList)
        {
            var arrayList = new AndroidJavaObject("java.util.ArrayList");
            for (int i = 0; i < stringList.Count; ++i)
            {
                arrayList.Call<bool>("add", stringList[i]);
            }

            return arrayList;
        }

        public static AndroidJavaObject toStringArray(IList<string> strings)
        {
            AndroidJavaClass jclass = new AndroidJavaClass("java.lang.reflect.Array");
            AndroidJavaObject jobject = jclass.CallStatic<AndroidJavaObject>("newInstance", new AndroidJavaClass("java.lang.String"), strings.Count);
            for (int i = 0; i < strings.Count; ++i)
            {
                jclass.CallStatic("set", jobject, i, new AndroidJavaObject("java.lang.String", strings[i]));
            }
            return jobject;
        }

        public static RCRTCMediaType toMediaType(AndroidJavaObject jtype)
        {
            int type = jtype.Call<int>("getType");
            return (RCRTCMediaType)type;
        }

        public static RCRTCCamera toCamera(AndroidJavaObject jcamera)
        {
            int camera = jcamera.Call<int>("getCamera");
            return (RCRTCCamera)camera;
        }

        public static RCRTCNetworkType toNetworkType(AndroidJavaObject jtype)
        {
            int type = jtype.Call<int>("getType");
            return (RCRTCNetworkType)type;
        }

        public static RCRTCAudioCodecType toAudioCodecType(AndroidJavaObject jtype)
        {
            int type = jtype.Call<int>("getType");
            return (RCRTCAudioCodecType)type;
        }

        public static RCRTCVideoCodecType toVideoCodecType(AndroidJavaObject jtype)
        {
            int type = jtype.Call<int>("getType");
            return (RCRTCVideoCodecType)type;
        }

        public static RCRTCLocalAudioStats toLocalAudioStats(AndroidJavaObject jstats)
        {
            AndroidJavaObject jcodec = jstats.Call<AndroidJavaObject>("getCodec");
            int bitrate = jstats.Call<int>("getBitrate");
            int volume = jstats.Call<int>("getVolume");
            double packageLostRate = jstats.Call<double>("getPackageLostRate");
            int rtt = jstats.Call<int>("getRtt");
            return new RCRTCLocalAudioStats(toAudioCodecType(jcodec), bitrate, volume, packageLostRate, rtt);
        }

        public static RCRTCLocalVideoStats toLocalVideoStats(AndroidJavaObject jstats)
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

        public static RCRTCRemoteAudioStats toRemoteAudioStats(AndroidJavaObject jstats)
        {
            AndroidJavaObject jcodec = jstats.Call<AndroidJavaObject>("getCodec");
            int bitrate = jstats.Call<int>("getBitrate");
            int volume = jstats.Call<int>("getVolume");
            double packageLostRate = jstats.Call<double>("getPackageLostRate");
            int rtt = jstats.Call<int>("getRtt");
            return new RCRTCRemoteAudioStats(toAudioCodecType(jcodec), bitrate, volume, packageLostRate, rtt);
        }

        public static RCRTCRemoteVideoStats toRemoteVideoStats(AndroidJavaObject jstats)
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
    }
}