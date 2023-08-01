﻿using System;

namespace cn_rongcloud_rtc_unity_example
{
    public class ExampleConfig
    {
        
        public static String AppKey { get; set; } = "";

        public static String AppVersion = "1.0";

        public static String Prefix = "utd_";

        public static String NavServer { get; set; } = "";

        public static String FileServer { get; set; }= "";

        public static String MediaServer { get; set; }= "";

        public static readonly String Host = "";
    }

    public class Config
    {
        public string AppKey;
        public string NavServer;
        public string FileServer;
        public string MediaServer;
        public string Token;
        public string Id;

        public static Config DefaultConfig()
        {
            return new Config
            {
                AppKey = "", // 必填
                NavServer = "nav.cn.ronghub.com",
                FileServer = "up.qbox.me",
                MediaServer = "",
                Token = "", // 必填
                Id = "utd_0000"
            };
        }

        public string Title()
        {
            if (Id == "utd_0000")
            {
                return AppKey + "-" + Id + "(默认)";
            }
            return AppKey + "-" + Id;
        }

        public override bool Equals(object obj)
        {
            Config c = obj as Config;

            if (this.Title() == c.Title())
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Title().GetHashCode();
        }
    }
}