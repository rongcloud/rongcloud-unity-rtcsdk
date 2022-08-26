using System;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.Rendering;

namespace cn_rongcloud_rtc_unity
{
    /// <summary>
    /// 用于渲染视频数据
    /// </summary>
#if UNITY_STANDALONE_WIN
    public class RCRTCView : MonoBehaviour, RCRTCOnVideoFrameListener
#else
    public class RCRTCView : MonoBehaviour
#endif
    {
        private void Start()
        {
            if (SurfaceType == RCRTCViewSurfaceType.Renderer)
            {
                _renderer = GetComponent<Renderer>();
                if (_renderer != null)
                {
                    Material material = new Material(Shader.Find("Rong/I420"));
                    _renderer.material = material;
                }
            }

            if (_renderer == null || SurfaceType == RCRTCViewSurfaceType.RawImage)
            {
                _rawImage = GetComponent<RawImage>();
                if (_rawImage != null)
                {
                    Material material = new Material(Shader.Find("Rong/I420"));
                    _rawImage.material = material;
                    _rectTransform = GetComponent<RectTransform>();
                    SurfaceType = RCRTCViewSurfaceType.RawImage;
                }
            }

            _initialized = _renderer != null || _rawImage != null;
            MaskCheck();
#if UNITY_IOS
            _renderType = RenderType;
            GraphicsDeviceType graphicsType = SystemInfo.graphicsDeviceType;
            //如果图形API不是Metal，则使用UnityRender方式渲染
            if (graphicsType != GraphicsDeviceType.Metal)
                _renderType = RCRTCViewRenderType.UnityRender;
            if (_renderType == RCRTCViewRenderType.UnityRender)
            {
                yTexturePtr = Marshal.AllocHGlobal(1920 * 1080);
                uTexturePtr = Marshal.AllocHGlobal(1920 * 1080 / 4);
                vTexturePtr = Marshal.AllocHGlobal(1920 * 1080 / 4);
            }
#endif
#if UNITY_STANDALONE_WIN
            yTexturePtr = Marshal.AllocHGlobal(1920 * 1080);
            uTexturePtr = Marshal.AllocHGlobal(1920 * 1080 / 4);
            vTexturePtr = Marshal.AllocHGlobal(1920 * 1080 / 4);
#endif
        }

        private void OnDisable()
        {
            if (yTexture != null)
            {
                Destroy(yTexture);
                yTexture = null;
            }
            if (uTexture != null)
            {
                Destroy(uTexture);
                uTexture = null;
            }
            if (vTexture != null)
            {
                Destroy(vTexture);
                vTexture = null;
            }
            width = 0;
            height = 0;
#if UNITY_STANDALONE_WIN
            _width = 0;
            _height = 0;
#endif
            if (_initialized)
            {
                ApplyTexture(0, 0, 0, null, null, null);
            }
        }

        private void OnDestroy()
        {
            if (HasView())
            {
                DestroySurface();
            }

            if (yTexture != null)
            {
                Destroy(yTexture);
                yTexture = null;
            }
            if (uTexture != null)
            {
                Destroy(uTexture);
                uTexture = null;
            }
            if (vTexture != null)
            {
                Destroy(vTexture);
                vTexture = null;
            }

            _renderer = null;
            _rawImage = null;

#if UNITY_IOS
            //UnityRender模式下且指针不为空才需要FreeHGlobal
            if (_renderType == RCRTCViewRenderType.UnityRender)
            {
                if (yTexturePtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(yTexturePtr);
                if (uTexturePtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(uTexturePtr);
                if (vTexturePtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(vTexturePtr);
            }
#endif
#if UNITY_STANDALONE_WIN
            if (yTexturePtr != IntPtr.Zero)
                Marshal.FreeHGlobal(yTexturePtr);
            if (uTexturePtr != IntPtr.Zero)
                Marshal.FreeHGlobal(uTexturePtr);
            if (vTexturePtr != IntPtr.Zero)
                Marshal.FreeHGlobal(vTexturePtr);
#endif
        }

        private void Update()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (!_initialized || !HasView())
            {
                return;
            }

            if (getFrameBufferCount() > 0)
            {
#if UNITY_IOS
                int width = 0;
                int height = 0;
                int rotation = 0;

                int yLength = 0;
                int uLength = 0;
                int vLength = 0;
                IntPtr yTextureId = IntPtr.Zero;
                IntPtr uTextureId = IntPtr.Zero;
                IntPtr vTextureId = IntPtr.Zero;
                if (_renderType == RCRTCViewRenderType.MetalRender)
                    rtc_get_native_texture(_view, ref yTextureId, ref uTextureId, ref vTextureId, ref width, ref height, ref rotation);
                else
                    rtc_get_current_frame(_view, yTexturePtr, uTexturePtr, vTexturePtr, ref yLength, ref uLength, ref vLength, ref width, ref height, ref rotation);
                
                if (NeedCreateTexture(yTextureId, uTextureId, vTextureId, width, height))
#elif UNITY_ANDROID
                AndroidJavaObject frame = _view.Call<AndroidJavaObject>("getCurrentFrame");
                int width = frame.Call<int>("getWidth");
                int height = frame.Call<int>("getHeight");
                int rotation =  frame.Call<int>("getRotation");
                if (NeedCreateTexture() && width > 0 && height > 0)
#endif
                {
                    yTexture = uTexture = vTexture = null;
#if UNITY_IOS
                    if (_renderType == RCRTCViewRenderType.MetalRender)
                    {
                        yTexture = Texture2D.CreateExternalTexture(width, height, TextureFormat.Alpha8, false, false, yTextureId);
                        uTexture = Texture2D.CreateExternalTexture(width >> 1, height >> 1, TextureFormat.Alpha8, false, false, uTextureId);
                        vTexture = Texture2D.CreateExternalTexture(width >> 1, height >> 1, TextureFormat.Alpha8, false, false, vTextureId);
                        this.yTexturePtr = yTextureId;
                        this.uTexturePtr = uTextureId;
                        this.vTexturePtr = vTextureId;
                    }
#endif
                    if (yTexture == null || uTexture == null || vTexture == null)
                    {
                        yTexture = new Texture2D(width, height, TextureFormat.Alpha8, false);
                        uTexture = new Texture2D(width >> 1, height >> 1, TextureFormat.Alpha8, false);
                        vTexture = new Texture2D(width >> 1, height >> 1, TextureFormat.Alpha8, false);
                    }
                    if (SurfaceType == RCRTCViewSurfaceType.Renderer)
                    {
                        _renderer.material.mainTexture = yTexture;
                    }

                    if (SurfaceType == RCRTCViewSurfaceType.RawImage)
                    {
                        _rawImage.texture = yTexture;
                    }

                    this.width = width;
                    this.height = height;
                }

                bool isMetalRender = false;
#if UNITY_IOS
                if (_renderType == RCRTCViewRenderType.MetalRender)
                    isMetalRender = true;
#endif
                if(!isMetalRender)
                {
                    if (this.width != width || this.height != height)
                    {
                        yTexture.Resize(width, height);
                        uTexture.Resize(width >> 1, height >> 1);
                        vTexture.Resize(width >> 1, height >> 1);
                        this.width = width;
                        this.height = height;
                    }
#if UNITY_IOS
                    yTexture.LoadRawTextureData(yTexturePtr, yLength);
                    uTexture.LoadRawTextureData(uTexturePtr, uLength);
                    vTexture.LoadRawTextureData(vTexturePtr, vLength);
#elif UNITY_ANDROID
                    yTexture.LoadRawTextureData((byte[])(Array)frame.Call<sbyte[]>("getY"));
                    uTexture.LoadRawTextureData((byte[])(Array)frame.Call<sbyte[]>("getU"));
                    vTexture.LoadRawTextureData((byte[])(Array)frame.Call<sbyte[]>("getV"));
#endif
                    yTexture.Apply();
                    uTexture.Apply();
                    vTexture.Apply();
                }

                float _w = 1.0f;
                float _h = 1.0f;

                if (SurfaceType == RCRTCViewSurfaceType.RawImage)
                {
                    if (rotation % 180 == 0)
                    {
                        _w = _rectTransform.rect.size.x / width;
                        _h = _rectTransform.rect.size.y / height;
                    }
                    else
                    {
                        _w = _rectTransform.rect.size.y / width;
                        _h = _rectTransform.rect.size.x / height;
                    }
                }

                ApplyTexture(_w, _h, rotation, yTexture, uTexture, vTexture);

#if UNITY_ANDROID
                frame.Call("release");
#endif
            }
#endif
#if UNITY_STANDALONE_WIN
            if (NeedCreateTexture() && _width > 0 && _height > 0)
            {
                yTexture = uTexture = vTexture = null;

                if (yTexture == null || uTexture == null || vTexture == null)
                {
                    yTexture = new Texture2D(_width, _height, TextureFormat.Alpha8, false);
                    uTexture = new Texture2D(_width >> 1, _height >> 1, TextureFormat.Alpha8, false);
                    vTexture = new Texture2D(_width >> 1, _height >> 1, TextureFormat.Alpha8, false);
                }
                if (SurfaceType == RCRTCViewSurfaceType.Renderer)
                {
                    _renderer.material.mainTexture = yTexture;
                }

                if (SurfaceType == RCRTCViewSurfaceType.RawImage)
                {
                    _rawImage.texture = yTexture;
                }

                this.width = _width;
                this.height = _height;
            }

            if (this.width != _width || this.height != _height)
            {
                yTexture.Resize(_width, _height);
                uTexture.Resize(_width >> 1, _height >> 1);
                vTexture.Resize(_width >> 1, _height >> 1);
                this.width = _width;
                this.height = _height;
            }

            if (width > 0 && height > 0)
            {
                int yLength = width * height;
                int uLength = width * height / 4;
                int vLength = width * height / 4;
                yTexture.LoadRawTextureData(yTexturePtr, yLength);
                uTexture.LoadRawTextureData(uTexturePtr, uLength);
                vTexture.LoadRawTextureData(vTexturePtr, vLength);

                yTexture.Apply();
                uTexture.Apply();
                vTexture.Apply();

                float _w = 1.0f;
                float _h = 1.0f;

                if (SurfaceType == RCRTCViewSurfaceType.RawImage)
                {
                    _w = _rectTransform.rect.size.x / width;
                    _h = _rectTransform.rect.size.y / height;
                }
                ApplyTexture(_w, _h, 0, yTexture, uTexture, vTexture);
            }

#endif
        }
        
#if UNITY_IOS
        private void InitNativeView(IntPtr engine)
        {
            if (_view != IntPtr.Zero)
            {
                return;
            }
            _view = rtc_create_view();
        }
#elif UNITY_ANDROID
        private void InitNativeView(AndroidJavaObject engine)
        {
            if (_view != null)
            {
                return;
            }
            _view = new AndroidJavaObject(ViewClass);
        }
#endif

        private bool HasView()
        {
#if UNITY_IOS
            return _view != IntPtr.Zero;
#elif UNITY_ANDROID
            return _view != null;
#else
            return false;
#endif
        }

        /// <summary>
        /// 清除资源，在removeview后调用
        /// </summary>
        public void DestroySurface()
        {
#if UNITY_IOS
            rtc_destroy_view(_view);
            _view = IntPtr.Zero;
#elif UNITY_ANDROID
            _view.Call("release");
            _view = null;
#endif
        }
#if UNITY_ANDROID || UNITY_STANDALONE_WIN
        private bool NeedCreateTexture()
        {
            return yTexture == null || uTexture == null || vTexture == null;
        }
#elif UNITY_IOS
        private bool NeedCreateTexture(IntPtr yTextureId, IntPtr uTextureId, IntPtr vTextureId, int width, int height)
        {
            if (_renderType == RCRTCViewRenderType.MetalRender)
                return
                   this.yTexturePtr != yTextureId ||
                   this.uTexturePtr != uTextureId ||
                   this.vTexturePtr != vTextureId ||
                   this.width != width ||
                   this.height != height;
            else
                return (yTexture == null || uTexture == null || vTexture == null) && width > 0 && height > 0;
        }
#endif
        private int getFrameBufferCount()
        {
#if UNITY_IOS
            return rtc_get_frame_buffer_count(_view);
#elif UNITY_ANDROID
            return _view.Call<int>("getFrameBufferCount");
#else
            return 0;
#endif
        }

        private void ApplyTexture(float width, float height, int rotation, Texture2D yTexture, Texture2D uTexture, Texture2D vTexture)
        {
            MaskCheck();
            if (SurfaceType == RCRTCViewSurfaceType.Renderer)
            {
                _renderer.sharedMaterial.SetFloat("_Width", width);
                _renderer.sharedMaterial.SetFloat("_Height", height);
                _renderer.sharedMaterial.SetInt("_Rotation", rotation);
                _renderer.sharedMaterial.SetInt("_Fit", (int)FitType);
                _renderer.sharedMaterial.SetInt("_Mirror", Mirror ? -1 : 1);

                _renderer.sharedMaterial.SetTexture("_MainTex", yTexture);
                _renderer.sharedMaterial.SetTexture("_UTex", uTexture);
                _renderer.sharedMaterial.SetTexture("_VTex", vTexture);
            }
            else if (SurfaceType == RCRTCViewSurfaceType.RawImage)
            {
                _rawImage.material.SetFloat("_Width", width);
                _rawImage.material.SetFloat("_Height", height);
                _rawImage.material.SetInt("_Rotation", rotation);
                _rawImage.material.SetInt("_Fit", (int)FitType);
                _rawImage.material.SetInt("_Mirror", Mirror ? -1 : 1);

                _rawImage.material.SetTexture("_MainTex", yTexture);
                _rawImage.material.SetTexture("_UTex", uTexture);
                _rawImage.material.SetTexture("_VTex", vTexture);
            }
        }

        private void MaskCheck()
        {
            if (GetComponentInParent<Mask>() != null)
            {
                if (SurfaceType == RCRTCViewSurfaceType.Renderer)
                {
                    _renderer.sharedMaterial.SetInt("_StencilComp", 3);
                    _renderer.sharedMaterial.SetInt("_Stencil", 1);
                }
                else if (SurfaceType == RCRTCViewSurfaceType.RawImage)
                {
                    _rawImage.material.SetInt("_StencilComp", 3);
                    _rawImage.material.SetInt("_Stencil", 1);
                }
            }
        }

#if UNITY_IOS || UNITY_STANDALONE_WIN
#if UNITY_IOS
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern IntPtr rtc_create_view();

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern void rtc_destroy_view(IntPtr view);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int rtc_get_frame_buffer_count(IntPtr view);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern void rtc_get_current_frame(IntPtr view, IntPtr dataY, IntPtr dataU, IntPtr dataV, ref int lengthY, ref int lengthU, ref int lengthV, ref int width, ref int height, ref int rotation);

        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern void rtc_get_native_texture(IntPtr view, ref IntPtr textureY, ref IntPtr textureU, ref IntPtr textureV, ref int textureWidth, ref int textureHeight, ref int rotation);

        private IntPtr _view = IntPtr.Zero;

        private RCRTCViewRenderType _renderType = RCRTCViewRenderType.MetalRender;

        [SerializeField]
        public RCRTCViewRenderType RenderType = RCRTCViewRenderType.MetalRender;
#else
        private int _width = 0;
        private int _height = 0;
        
        public void OnVideoFrame(ref RCRTCVideoFrame frame)
        {
            int width = frame.Width;
            int height = frame.Height;

            int yLength = width * height;
            int uLength = width * height / 4;
            int vLength = width * height / 4;

            NativeWin.rcrtc_memory_copy(frame.DataY, yTexturePtr, yLength);
            NativeWin.rcrtc_memory_copy(frame.DataU, uTexturePtr, uLength);
            NativeWin.rcrtc_memory_copy(frame.DataV, vTexturePtr, vLength);

            _width = width;
            _height = height;
        }
#endif

        private IntPtr yTexturePtr = IntPtr.Zero;//贴图数据指针或外部贴图指针
        private IntPtr uTexturePtr = IntPtr.Zero;
        private IntPtr vTexturePtr = IntPtr.Zero;
        
#elif UNITY_ANDROID
        private AndroidJavaObject _view = null;

        private const String ViewClass = "cn.rongcloud.rtc.wrapper.platform.unity.RCRTCIWUnityView";
#endif
        /// <summary>
        /// 渲染模式
        /// </summary>
        [SerializeField]
        RCRTCViewSurfaceType SurfaceType = RCRTCViewSurfaceType.Renderer;

        /// <summary>
        /// 填充模式
        /// </summary>
        [SerializeField]
        public RCRTCViewFitType FitType = RCRTCViewFitType.COVER;

        /// <summary>
        /// 是否镜像显示
        /// </summary>
        [SerializeField]
        public Boolean Mirror = false;

        private int width = 0;
        private int height = 0;
        private Texture2D yTexture = null;
        private Texture2D uTexture = null;
        private Texture2D vTexture = null;

        private bool _initialized = false;
        private Renderer _renderer = null;
        private RawImage _rawImage = null;
        private RectTransform _rectTransform = null;
    }

}
