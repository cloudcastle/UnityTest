Shader "MyUnlit" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100
	
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
			
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					o.scrPos = ComputeScreenPos(o.vertex);
					return o;
				}
			
				fixed4 frag (v2f i) : SV_Target
				{
					float2 uv2 = (i.scrPos.xy/i.scrPos.w);
					fixed4 col = tex2D(_MainTex, uv2);
					col.b = col.b * 1.2;
					return col;
				}
			ENDCG
		}
	}
}
