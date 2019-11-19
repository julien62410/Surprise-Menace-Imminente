Shader "Unlit/S_Eye"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_EyeColor ("Eye Color", Color) = (1,1,1,1)
		_IrisColor ("Iris Color", Color) = (1,1,1,1)

		_EyeSize("Eye Size", float) = 1
		_EyeSmooth("Eye Smooth", Range(0,1)) = 0.2
		_EyeCenter("Eye Center", Vector) = (0,0,0,0)
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
				float3 pos : POSITION1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _EyeCenter;
			fixed4 _EyeColor;
			fixed4 _IrisColor;
			float _EyeSize;
			float _EyeSmooth;

            v2f vert (appdata v)
            {
                v2f o;
				o.pos = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				float dis = distance(_EyeCenter, i.pos);
				float smoothDist = smoothstep(_EyeSize, _EyeSize +_EyeSmooth, dis);
				fixed4 col = lerp(_IrisColor, _EyeColor, smoothDist);
                return col;
            }
            ENDCG
        }
    }
}
