Shader "Unlit/S_Battery"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_PulseFrequency("Pulse Frequency", float) = 1
		_PulseSpeed("Pulse Speed", float) = 1

		_PulseWidth("Pulse Width", float) = 0.2
		_PulseSmooth("Pulse Smooth", Range(0,1)) = 0.2
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _PulseFrequency;
			float _PulseWidth;
			float _PulseSmooth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				float freq = 1 / _PulseFrequency;
				float time = frac(_Time.y / freq);

				float topMask = 0;
				
                return time;
            }
            ENDCG
        }
    }
}
