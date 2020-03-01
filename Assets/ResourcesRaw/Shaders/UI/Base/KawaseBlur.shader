Shader "Custom/KawaseBlur"
{
	Properties
    {
		[HideInInspector]
        _MainTex("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent" }
        ZWrite Off
		Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				half2 texcoord[4]  : TEXCOORD0;
				float4 vertex   : SV_POSITION;
			};
            
			sampler2D _MainTex;
			fixed2 _MainTex_TexelSize;
			fixed _Offset;
            fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord[0] = IN.texcoord + _MainTex_TexelSize * half2( _Offset + 0.5,  _Offset + 0.5) ;//top right
				OUT.texcoord[1] = IN.texcoord + _MainTex_TexelSize * half2(-_Offset - 0.5, -_Offset - 0.5) ;//bottom left
				OUT.texcoord[2] = IN.texcoord + _MainTex_TexelSize * half2(-_Offset - 0.5,  _Offset + 0.5) ;//top left
				OUT.texcoord[3] = IN.texcoord + _MainTex_TexelSize * half2( _Offset + 0.5, -_Offset - 0.5) ;//bottom right
				return OUT;
			}		
            
			fixed4 frag(v2f i) : COLOR
			{
				fixed4 sum = 0;  
				sum += tex2D(_MainTex, i.texcoord[0]);
				sum += tex2D(_MainTex, i.texcoord[1]);
				sum += tex2D(_MainTex, i.texcoord[2]);
				sum += tex2D(_MainTex, i.texcoord[3]);
				
				return  sum * 0.25;
			}          
			ENDCG
		}
    }
    Fallback "Sprites/Default"
}
