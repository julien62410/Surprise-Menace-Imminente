Shader "Unlit/S_Battery"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_PulseFrequency("Pulse Frequency", float) = 1
		_PulseSpeed("Pulse Speed", float) = 1

		_PulseWidth("Pulse Width", float) = 0.2
		_PulseSmooth("Pulse Smooth", Range(0,1)) = 0.2
		_PulseRemap("Pulse Remap", float) = 4

		_StepSparkle("Step Sparkle", float) = 0.2

		_Color("Color", Color) = (1,1,1,1)
		_SparkleColor("Sparkle Color", Color) = (1,1,1,1)
		_FresnelColor("Fresnel Color", Color) = (1,1,1,1)

		_ShadowColor("Shadow Color", Color) = (0,0,0,0)
		_ShadowSmoothness("Shadow Smoothness", Range(0,1)) = 0.5
		_LightDir("LightDir", Vector) =(0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
		Blend One One
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
				float3 viewDir : TEXCOORD1;
            };

            struct v2f
            {
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 position : POSITION1;
				float3 viewDir : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _PulseFrequency;
			float _PulseWidth;
			float _PulseSmooth;
			float _PulseSpeed;
			float _PulseRemap;
			float _StepSparkle;

			float4 _LightDir;
			float _ShadowSmoothness;

			fixed4 _Color;
			fixed4 _SparkleColor;
			fixed4 _FresnelColor;
			fixed4 _ShadowColor;

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
				return  pow(1 - nDotL, power);
			}

            v2f vert (appdata v)
            {
                v2f o;
				o.position = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal =  v.normal;
				o.worldNormal = mul(unity_ObjectToWorld, v.normal);
				o.viewDir = ObjSpaceViewDir(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv + _Time.y * 0.12);
				float freq = 1 / _PulseFrequency;
				float time = saturate(frac(_Time.y * _PulseSpeed / freq) * freq);
				float remapTime = time * 2 - 1;

				float zPos = i.position.z / _PulseRemap + 0.5;
				float topMask = smoothstep(remapTime - _PulseWidth, remapTime - _PulseWidth + _PulseSmooth, zPos);
				float bottomMask = 1 - smoothstep(remapTime + _PulseWidth, remapTime + _PulseWidth + _PulseSmooth, zPos);
				
				float mask = bottomMask * topMask;
				float sparkle = (1 -mask) * col.r;

				float maskSparkle = step(sparkle, _StepSparkle) * (1 -step(mask, 0)) * mask;
				fixed4 fresn = fresnel(normalize(i.normal), normalize(i.viewDir), lerp(1, 2, (sin(_Time.y * 7) + 1) / 2)) * _FresnelColor;

				fixed4 applySparkle = lerp(_Color, _SparkleColor, maskSparkle);

				float shadow = toon(i.worldNormal);

				col = lerp(applySparkle * _ShadowColor, applySparkle, shadow);
				col += fresn;
				return col;
            }
            ENDCG
        }
    }
}
