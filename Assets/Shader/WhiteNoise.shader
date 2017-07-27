Shader "Custom/White Noise" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
	}

	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100


	    ZWrite On
	    Blend SrcAlpha OneMinusSrcAlpha
	
		Pass {  
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
			
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
					float4 scrPos:TEXCOORD1;
				};		
			
				struct Input {
					float2 uv_MainTex;
					float4 screenPos;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _Color;
			
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					o.scrPos = ComputeScreenPos(o.vertex);
					return o;
				}

				float rand(float2 co)
				{
				    return frac(sin( dot(float3(frac(sin(co.x*12774)), frac(sin(co.y*67774)), _Time.x) ,float3(12.9898,78.233, 38.221) )) * 43758.5453);
				}
			
				fixed4 frag (v2f i) : SV_Target
				{
					float r = rand(i.scrPos.xy);
					half4 col = half4(r, r, r, r);
					return col;
				}
			ENDCG
		}
	}
}
