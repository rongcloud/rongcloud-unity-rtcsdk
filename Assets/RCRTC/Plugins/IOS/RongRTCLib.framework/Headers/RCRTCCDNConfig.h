//
//  RCRTCCDNConfig.h
//  RongRTCLib
//
//  Created by RongCloud on 2020/5/21.
//  Copyright © 2020 RongCloud. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface RCRTCCDNConfig : NSObject

/**
 version
 */
@property (nonatomic, assign, readonly) NSInteger version;

/**
 cdn list
 */
@property (nonatomic, strong) NSMutableSet *cdnList;

@end

NS_ASSUME_NONNULL_END
