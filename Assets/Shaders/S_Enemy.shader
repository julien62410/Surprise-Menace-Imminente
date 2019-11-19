// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/S_Enemy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_ColorA("Color A", Color) = (1,1,1,1)
		_ColorB("Color B", Color) = (1,1,1,1)

		_ShadowColor("Shadow Color", Color) = (1,1,1,1)
		_ShadowSmoothness("Shadow Smoothness", Range(0,1)) = 0.5
		_LightDir("Light Dir", Vector) = (0,0,0,0)
		
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 localPos : TEXCOORD1;
				float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _ColorA;
			fixed4 _ColorB;
			fixed4 _ShadowColor;
			float _ShadowSmoothness;

			fixed4 _LightDir;

			float toon(float3 normal)
			{
				float nDotL = dot(normalize(_LightDir), normalize(normal));
				float remapDot = (nDotL + 1) / 2;
				float sharpen = smoothstep(_ShadowSmoothness / 2, 1 - (_ShadowSmoothness / 2), remapDot);
				return sharpen;
			}

            v2f vert (appdata v)
            {
                v2f o;
				o.localPos = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = v.normal;
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = lerp(_ColorA, _ColorB, i.uv.y);
				float shadow = toon(i.normal);
				fixed4 applyShadow = lerp(col * _ShadowColor, col, shadow);
                return applyShadow;
            }
            ENDCG
        }
    }
}
