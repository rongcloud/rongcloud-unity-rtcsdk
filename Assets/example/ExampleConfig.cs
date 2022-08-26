using System;

namespace cn_rongcloud_rtc_unity_example
{
    public class ExampleConfig
    {
        public static void Reset()
        {
            AppKey = "";
        }
        
        public static String AppKey { get; set; } = "";

        public static String AppVersion = "";

        public static String Prefix = "";

        public static String NavServer { get; set; } = "";

        public static String FileServer { get; set; }= "";

        public static String MediaServer { get; set; }= "";

        public static readonly String Host = "";
    }
}