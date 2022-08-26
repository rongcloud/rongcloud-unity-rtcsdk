using System;

namespace cn_rongcloud_rtc_unity
{
    /// <summary>
    /// 自定义布局
    /// </summary>
    public class RCRTCCustomLayout
    {
        /// <summary>
        /// 创建自定义布局
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="x">混流图层坐标的 x 值</param>
        /// <param name="y">混流图层坐标的 y 值</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        public RCRTCCustomLayout(String userId, int x, int y, int width, int height)
        {
            this.userId = userId;
            this.tag = null;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// 创建自定义布局
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="tag">流全局唯一标签</param>
        /// <param name="x">混流图层坐标的 x 值</param>
        /// <param name="y">混流图层坐标的 y 值</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        public RCRTCCustomLayout(String userId, String tag, int x, int y, int width, int height)
        {
            this.userId = userId;
            this.tag = tag;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// 用户id
        /// </summary>
        /// <returns></returns>
        public String GetUserId()
        {
            return userId;
        }

        /// <summary>
        /// 流全局唯一标签
        /// </summary>
        /// <returns></returns>
        public String GetTag()
        {
            return tag;
        }

        /// <summary>
        /// 混流图层坐标的 x 值
        /// </summary>
        /// <returns></returns>
        public int GetX()
        {
            return x;
        }

        /// <summary>
        /// 混流图层坐标的 y 值
        /// </summary>
        /// <returns></returns>
        public int GetY()
        {
            return y;
        }

        /// <summary>
        /// 宽
        /// </summary>
        /// <returns></returns>
        public int GetWidth()
        {
            return width;
        }

        /// <summary>
        /// 高
        /// </summary>
        /// <returns></returns>
        public int GetHeight()
        {
            return height;
        }

        private String userId;
        private String tag;
        private int x;
        private int y;
        private int width;
        private int height;

    }

}