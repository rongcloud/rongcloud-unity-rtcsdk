//
//  Copyright © 2021 RongCloud. All rights reserved.
//

#ifndef RC_RongUnityRTC
#define RC_RongUnityRTC

#import <Foundation/Foundation.h>

extern "C" {

typedef struct rtc_audio_setup
{
    int audioCodecType;
} rtc_audio_setup;

typedef struct rtc_video_setup
{
    bool enableTinyStream;
} rtc_video_setup;

typedef struct rtc_engine_setup
{
    bool reconnectable;
    int statsReportInterval;
    bool enableSRTP;
    rtc_audio_setup *audioSetup;
    rtc_video_setup *videoSetup;
} rtc_engine_setup;

typedef struct rtc_room_setup
{
    int role;
    int type;
} rtc_room_setup;

typedef struct rtc_audio_config
{
    int quality;
    int scenario;
} rtc_audio_config;

typedef struct rtc_video_config
{
    int minBitrate;
    int maxBitrate;
    int fps;
    int resolution;
} rtc_video_config;

typedef struct rtc_custom_layout
{
    int type;
    const char *id;
    const char *tag;
    int x;
    int y;
    int width;
    int height;
} rtc_custom_layout;

typedef struct rtc_network_stats
{
    int type;
    const char *ip;
    int sendBitrate;
    int receiveBitrate;
    int rtt;
} rtc_network_stats;

typedef struct rtc_local_audio_stats
{
    int codec;
    int bitrate;
    int volume;
    double packageLostRate;
    int rtt;
} rtc_local_audio_stats;

typedef struct rtc_local_video_stats
{
    bool tiny;
    int codec;
    int bitrate;
    int fps;
    int width;
    int height;
    double packageLostRate;
    int rtt;
} rtc_local_video_stats;

typedef struct rtc_remote_audio_stats
{
    int codec;
    int bitrate;
    int volume;
    double packageLostRate;
    int rtt;
} rtc_remote_audio_stats;

typedef struct rtc_remote_video_stats
{
    int codec;
    int bitrate;
    int fps;
    int width;
    int height;
    double packageLostRate;
    int rtt;
} rtc_remote_video_stats;

typedef void (*on_rtc_network_stats)(rtc_network_stats *stats);

typedef void (*on_rtc_local_audio_stats)(rtc_local_audio_stats *stats);

typedef void (*on_rtc_local_video_stats)(rtc_local_video_stats *stats);

typedef void (*on_rtc_remote_audio_stats)(const char *userId, rtc_remote_audio_stats *stats);

typedef void (*on_rtc_remote_video_stats)(const char *userId, rtc_remote_video_stats *stats);

typedef void (*on_rtc_live_mix_audio_stats)(rtc_remote_audio_stats *stats);

typedef void (*on_rtc_live_mix_video_stats)(rtc_remote_video_stats *stats);

typedef struct rtc_stats_listener_proxy
{
    bool remove;
    on_rtc_network_stats onNetworkStats;
    on_rtc_local_audio_stats onLocalAudioStats;
    on_rtc_local_video_stats onLocalVideoStats;
    on_rtc_remote_audio_stats onRemoteAudioStats;
    on_rtc_remote_video_stats onRemoteVideoStats;
    on_rtc_live_mix_audio_stats onLiveMixAudioStats;
    on_rtc_live_mix_video_stats onLiveMixVideoStats;
} rtc_stats_listener_proxy;

typedef struct rtc_audio_frame
{
//    void *data;
    CFTypeRef data;
    int length;
    int channels;
    int sampleRate;
    int bytesPerSample;
    int samples;
} audio_frame;

typedef void (*on_rtc_audio_frame)(const char* , rtc_audio_frame *frame);

typedef struct rtc_audio_listener_proxy
{
    bool remove;
    on_rtc_audio_frame onAudioFrame;
} rtc_audio_listener_proxy;

typedef void (*on_rtc_error)(int code, const char *message);

typedef void (*on_rtc_kicked)(const char *id, const char *message);

typedef void (*on_rtc_room_joined)(int code, const char *message);

typedef void (*on_rtc_room_left)(int code, const char *message);

typedef void (*on_rtc_published)(int type, int code, const char *message);

typedef void (*on_rtc_unpublished)(int type, int code, const char *message);

typedef void (*on_rtc_subscribed)(const char *id, int type, int code, const char *message);

typedef void (*on_rtc_unsubscribed)(const char *id, int type, int code, const char *message);

typedef void (*on_rtc_live_mix_subscribed)(int type, int code, const char *message);

typedef void (*on_rtc_live_mix_unsubscribed)(int type, int code, const char *message);

typedef void (*on_rtc_enable_camera)(bool enable, int code, const char *message);

typedef void (*on_rtc_switch_camera)(int camera, int code, const char *message);

typedef void (*on_rtc_live_cdn_added)(const char *url, int code, const char *message);

typedef void (*on_rtc_live_cdn_removed)(const char *url, int code, const char *message);

typedef void (*on_rtc_live_mix_layout_mode_set)(int code, const char *message);

typedef void (*on_rtc_live_mix_render_mode_set)(int code, const char *message);

typedef void (*on_rtc_live_mix_custom_layouts_set)(int code, const char *message);

typedef void (*on_rtc_live_mix_custom_audios_set)(int code, const char *message);

typedef void (*on_rtc_live_mix_video_bitrate_set)(bool tiny, int code, const char *message);

typedef void (*on_rtc_live_mix_video_resolution_set)(bool tiny, int code, const char *message);

typedef void (*on_rtc_live_mix_video_fps_set)(bool tiny, int code, const char *message);

typedef void (*on_rtc_live_mix_audio_bitrate_set)(int code, const char *message);

typedef void (*on_rtc_audio_effect_created)(int id, int code, const char *message);
typedef void (*on_rtc_audio_effect_finished)(int id);

typedef void (*on_rtc_audio_mixing_started)();
typedef void (*on_rtc_audio_mixing_paused)();
typedef void (*on_rtc_audio_mixing_stopped)();
typedef void (*on_rtc_audio_mixing_finished)();

typedef void (*on_rtc_user_joined)(const char *id);

typedef void (*on_rtc_user_offline)(const char *id);

typedef void (*on_rtc_user_left)(const char *id);

typedef void (*on_rtc_remote_published)(const char *id, int type);

typedef void (*on_rtc_remote_unpublished)(const char *id, int type);

typedef void (*on_rtc_remote_live_mix_published)(int type);

typedef void (*on_rtc_remote_live_mix_unpublished)(int type);

typedef void (*on_rtc_remote_state_changed)(const char *id, int type, bool muted);

typedef void (*on_rtc_remote_first_frame)(const char *id, int type);

typedef void (*on_rtc_remote_live_mix_first_frame)(int type);

}

#endif /* RongUnityRTC_h */
