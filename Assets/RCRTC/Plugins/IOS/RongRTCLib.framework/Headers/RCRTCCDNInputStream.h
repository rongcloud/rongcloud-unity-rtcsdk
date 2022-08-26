//
//  RCRTCCDNStream.h
//  RongRTCLib
//
//  Created by RongCloud on 2021/5/18.
//  Copyright © 2021 RongCloud. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "RCRTCInputStream.h"
#import "RCRTCLibDefine.h"
#import "RCRTCDrawer.h"
NS_ASSUME_NONNULL_BEGIN

typedef void (^RCRTCConfigCallback)(BOOL isSuccess, RCRTCCode code);

@interface RCRTCCDNInputStream : RCRTCInputStream

/*!
 获取订阅 CDN 流前设置的分辨率
 */
- (RCRTCVideoSizePreset)getVideoResolution;

/*!
 获取订阅 CDN 流前设置的帧率
 */
- (RCRTCVideoFPS)getVideoFps;

/*!
 当前订阅成功的 CDN 流最高支持的分辨率
 */
- (RCRTCVideoSizePreset)getHighestResolution;

/*!
 当前订阅成功的 CDN 流最高支持的帧率
 */
- (RCRTCVideoFPS)getHighestFPS;

/*!
  设置分辨率和帧率
  @param videoSizePreset 分辨率
  @param fps 帧率
 */
- (void)setVideoConfig:(RCRTCVideoSizePreset)videoSizePreset
              fpsValue:(RCRTCVideoFPS)fps
            completion:(RCRTCConfigCallback)completion;

/*!
 设置视频流的渲染视图
 
 @param render 渲染视图
 @discussion
 接受到远端用户的视频流，然后设置视频流的渲染视图，就可以渲染出远端视频
 
 @remarks 视频配置
 */
- (void)setVideoView:(id<RCRTCDrawer>)render;

@end

NS_ASSUME_NONNULL_END
