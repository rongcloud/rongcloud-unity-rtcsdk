//
//  Copyright © 2021 RongCloud. All rights reserved.
//

#import "RongUnityRTC.h"

#import <Foundation/Foundation.h>

#import <RongRTCLibWrapper/RongRTCLibWrapper.h>

#import <Metal/Metal.h>
NS_ASSUME_NONNULL_BEGIN

@interface RCRTCIWUnityVideoFrame : NSObject
@property(nonatomic, readonly) int width;
@property(nonatomic, readonly) int height;
@property(nonatomic, readonly) int rotation;
@property(nonatomic, readonly) const uint8_t *dataY;
@property(nonatomic, readonly) const uint8_t *dataU;
@property(nonatomic, readonly) const uint8_t *dataV;
@property(nonatomic, readonly) int strideY;
@property(nonatomic, readonly) int strideU;
@property(nonatomic, readonly) int strideV;
@end


//Metal渲染的上下文，unity渲染时不需要
@interface MetalContext : NSObject
@property (nonatomic, strong) id<MTLTexture> yTexture;
@property (nonatomic, strong) id<MTLTexture> uTexture;
@property (nonatomic, strong) id<MTLTexture> vTexture;
@property (nonatomic, assign) MTLRegion region;//Y贴图填充区域，Metal使用
@property (nonatomic, assign) MTLRegion smallRegion;//U、V贴图填充区域，Metal使用
@property (nonatomic, assign) int textureWidth;
@property (nonatomic, assign) int textureHeight;
@end


@interface RCRTCIWUnityView
@property (nonatomic, strong) MetalContext *metalContext;
+ (RCRTCIWUnityView *)create;
- (void)destroy;
- (RCRTCIWUnityVideoFrame *)getCurrentFrame;
- (NSInteger)getBufferLength;
@end

NS_ASSUME_NONNULL_END
