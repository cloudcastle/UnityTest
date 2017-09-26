// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "SpriteRenderer/Recolor" {
Properties {
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Red ("Red", Color) = (1,0.5,0.5,0.5)
	_Green ("Green", Color) = (0.5,1,0.5,0.5)
	_Blue ("Blue", Color) = (0.5,0.5,1,0.5)
	_Alpha ("Alpha", Color) = (0.5,0.5,0.5,1)
	_Const ("Const", Color) = (0.5,0.5,0.5,0.5)
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 100

    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
				float4 color    : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
				fixed4 color    : COLOR;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;


			fixed4 _Red;
			fixed4 _Green;
			fixed4 _Blue;
			fixed4 _Alpha;
			fixed4 _Const;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 d = tex2D(_MainTex, i.texcoord);
				fixed4 c = (_Const-0.5) + d.r * (_Red-0.5) + d.g * (_Green-0.5) + d.b * (_Blue-0.5) + d.a * (_Alpha-0.5);
		        c = c * 2;
		        c.a *= d.a;
		        c.rgb *= c.a;
				return c * i.color;
            }
        ENDCG
    }
}

}
