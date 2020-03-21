Shader "Custom/WaveProgress"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}

		_Color("颜色", Color) = (1,0,0,1)
		_WaveSpeed("波动速度",Range(0,10)) = 2
		_Progress("进度",Range(0,1)) = 0

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			
			#pragma multi_compile __ UNITY_UI_ALPHACLIP

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			fixed4 _Color;
			fixed _WaveSpeed;
			half _Progress;

			sampler2D _MainTex;
            float4 _MainTex_ST;

			sampler2D _LightTex;
            float4 _LightTex_ST;
			fixed _ShineSpeed;

			v2f vert(appdata v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color * _Color;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv1 = v.uv1;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				half2 uv = i.uv;
				fixed4 color = i.color * tex2D(_MainTex, uv);
				half2 uv1 = i.uv1;
				half val = uv1.y + sin(uv1.x * 6.0 + _Time.y * _WaveSpeed)* 0.4;  
				val *= 0.1;
				color.rgb = lerp(lerp(color.rgb + 0.1 , color.rgb, _Progress), 1.0, 0.6 - uv1.y);
				color.a = uv1.y + val - 0.2 > _Progress ? 0 : color.a;
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif
				return color;
			}
		ENDCG
		}
	}
}