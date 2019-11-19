﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/S_Enemy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_ColorA("Color A", Color) = (1,1,1,1)
		_ColorB("Color B", Color) = (1,1,1,1)
		_FresnelColor("Fresnel Color", Color) = (1,1,1,1)

		_ShadowColor("Shadow Color", Color) = (1,1,1,1)
		_ShadowSmoothness("Shadow Smoothness", Range(0,1)) = 0.5
		_LightDir("Light Dir", Vector) = (0,0,0,0)
		
		[Header(Wiggle)]
		_WiggleSpeed("WiggleSpeed", float) = 1
		_WiggleStrength("Wiggle Strength", float) = 1
		_WiggleMin("Wiggle Min", float) = 0.5
		_WiggleMax("Wiggle Max", float) = 0.6
		_WiggleFreq("Wiggle Freq", float) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
			//Blend One One

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
				float3 viewDir : TEXCOORD2;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 viewDir : POSITION1;
				float3 pos : POSITION2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _ColorA;
			fixed4 _ColorB;
			fixed4 _FresnelColor;
			fixed4 _ShadowColor;
			float _ShadowSmoothness;

			float _WiggleSpeed;
			float _WiggleStrength;
			float _WiggleFreq;
			float _WiggleMin;
			float _WiggleMax;

			fixed4 _LightDir;

			float toon(float3 normal)
			{
				float nDotL = dot(normalize(_LightDir), normalize(normal));
				float remapDot = (nDotL + 1) / 2;
				float sharpen = smoothstep(_ShadowSmoothness / 2, 1 - (_ShadowSmoothness / 2), remapDot);
				return sharpen;
			}

			float fresnel(float3 normal, float3 viewDir, float power)
			{
				float nDotL = dot(normalize(viewDir), normalize(normal));
				return 1 - pow(nDotL, power);
			}

			float wiggleAmount(float offset, float vertexZ)
			{
				float wig = sin(_Time.y * _WiggleSpeed + vertexZ * _WiggleFreq + offset)* _WiggleStrength * smoothstep(_WiggleMin, _WiggleMax, vertexZ);
				return wig;
			}

            v2f vert (appdata v)
            {
                v2f o;
				float wiggleX = wiggleAmount(0, v.vertex.z);
				v.vertex.x += wiggleX;

				float wiggleY = wiggleAmount(0.5, v.vertex.z);
				v.vertex.y += wiggleY * 1.2;

				o.pos = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = WorldSpaceViewDir(v.vertex);
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 noise = tex2D(_MainTex, i.uv);
				float posNoise = noise + i.uv.y;
				fixed4 col = lerp(_ColorA, _ColorB, i.pos.z + noise);
				float shadow = toon(i.normal);
				fixed4 applyShadow = lerp(col * _ShadowColor, col, shadow);
				fixed4 fresn = fresnel(i.normal, i.viewDir, 1) * _FresnelColor;
				col = applyShadow + fresn;
                return col;
            }
            ENDCG
        }
    }
}
