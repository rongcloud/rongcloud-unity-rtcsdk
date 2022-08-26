Shader "Rong/I420"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _UTex ("U", 2D) = "white" {}
        _VTex ("V", 2D) = "white" {}

		_Width ("Width", Float) = 0
		_Height ("Height", Float) = 0
		_Rotation ("Rotation", Int) =  0
		_Fit ("Fit", Int) = 0
		_Mirror ("Mirror", Float) = -1

		//MASK SUPPORT ADD
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_Colormask("Color Mask", Float) = 15 //不能写_ColorMask
		//MASK SUPPORT END
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		//MASK SUPPORT ADD
		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}
		ColorMask[_Colormask] // 将定义的_ColorMask参数导入
		//MASK SUPPORT END

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

            sampler2D _MainTex;
            sampler2D _UTex;
            sampler2D _VTex;
			float _Width;
			float _Height;
			uint _Rotation;
			uint _Fit;
			float _Mirror;

			float center = float2(0.5, 0.5);
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 temp = float2(i.uv.x, i.uv.y);
				temp -= center;
				float angle = _Rotation * (3.14 * 2 / 360);
				temp = float2(temp.x * cos(angle) - temp.y * sin(angle), temp.x * sin(angle) + temp.y * cos(angle));
				temp += center;

				if (_Fit != 0) {
					temp *= float2(_Width, _Height);

					float scale = 1.0;
					if (_Width < _Height) {
						if (_Fit == 1) {
							scale = _Height;
						} else {
							scale = _Width;
						}
					} else if (_Width > _Height) {
						if (_Fit == 1) {
							scale = _Width;
						} else {
							scale = _Height;
						}
					}
					temp /= scale;

					float2 offset = float2((1 - _Width / scale) / 2, (1 - _Height / scale) / 2);
					if (_Rotation == 90) {
						if (_Fit != 1) {
							temp += offset;
						} else {
							temp -= offset;
						}

						if (temp.y < 0 || temp.y > 1.0) {
							return fixed4(0, 0, 0, 1);
						}
						if (temp.x < -1.0 || temp.x > 0) {
							return fixed4(0, 0, 0, 1);
						}
					} else if (_Rotation == 180) {
						temp -= offset;
						
						if (temp.x < -1.0 || temp.x > 0) {
							return fixed4(0, 0, 0, 1);
						}
						if (temp.y < -1.0 || temp.y > 0) {
							return fixed4(0, 0, 0, 1);
						}
					} else if (_Rotation == 270) {
						if (_Fit != 1) {
							temp -= offset;
						} else {
							temp += offset;
						}

						if (temp.y < -1.0 || temp.y > 0) {
							return fixed4(0, 0, 0, 1);
						}
						if (temp.x < 0 || temp.x > 1.0) {
							return fixed4(0, 0, 0, 1);
						}
					} else {
						temp += offset;
						
						if (temp.x < 0 || temp.x > 1.0) {
							return fixed4(0, 0, 0, 1);
						}
						if (temp.y < 0 || temp.y > 1.0) {
							return fixed4(0, 0, 0, 1);
						}
					}
				}

				float2 uv;
				if (_Rotation == 90) {
					uv = float2(temp.x, temp.y * _Mirror * -1);
				} else if (_Rotation == 180) {
					uv = float2(temp.x * _Mirror, temp.y * -1);
				} else if (_Rotation == 270) {
					uv = float2(temp.x, temp.y * _Mirror * -1);
				} else {
					uv = float2(temp.x * _Mirror, temp.y * -1);
				}

                fixed4 ycol = tex2D(_MainTex, uv);
                fixed4 ucol = tex2D(_UTex, uv);
				fixed4 vcol = tex2D(_VTex, uv);
                
                float r = ycol.a + 1.4022 * vcol.a - 0.7011;
                float g = ycol.a - 0.3456 * ucol.a - 0.7145 * vcol.a + 0.53005;
                float b = ycol.a + 1.771 * ucol.a - 0.8855;
                
				return fixed4(r, g, b, 1);
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}


