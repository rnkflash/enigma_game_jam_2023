/*
Created by jiadong chen
https://jiadong-chen.medium.com/
*/

Shader "GachaSurvivals/AnimMapShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_AnimMap ("AnimMap", 2D) ="white" {}
		_Noise ("Noise", 2D) ="white" {}
		_AnimLen("Anim Length", Float) = 0
	    _Color ("Color", Color) = (1,1,1,1)
	    _ColorShadow ("ColorShadow", Color) = (1,1,1,1)
	}
	
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline"}
        Cull off

        Pass
        {
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float2 uv : TEXCOORD0;
                float4 pos : POSITION;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };


            CBUFFER_START(UnityPerMaterial)
                float _AnimLen;
                sampler2D _MainTex;
                float4 _MainTex_ST;
                sampler2D _AnimMap;
                sampler2D _Noise;
				float4 _Color;
				float4 _ColorShadow;
                float4 _AnimMap_TexelSize;//x == 1/width
            CBUFFER_END 
            
            float4 ObjectToClipPos (float3 pos)
            {
                return mul (UNITY_MATRIX_VP, mul (UNITY_MATRIX_M, float4 (pos,1)));
            }
            
            v2f vert (appdata v, uint vid : SV_VertexID)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                
                float3 worldPos = mul(unity_ObjectToWorld, v.pos);
            	
            	worldPos = worldPos / 50;//80norm
				float4 noise = tex2Dlod(_Noise, float4( worldPos.x, worldPos.z, 0, 0));

            	
            	float f = _Time.y / _AnimLen + (noise.x );

                fmod(f, 1.0);

                float animMap_x = (vid + 0.5) * _AnimMap_TexelSize.x;
                float animMap_y = f;

                float4 pos = tex2Dlod(_AnimMap, float4(animMap_x, animMap_y, 0, 0));

                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            	o.worldNormal = normalize(mul(v.normal, (float3x3)UNITY_MATRIX_I_M));
                o.vertex = ObjectToClipPos(pos);
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
				float NdotL = dot(_MainLightPosition.xyz, normal);
            	float lightIntensity = NdotL > 0 ? 1 : 0;
                float4 col = tex2D(_MainTex, i.uv) * _Color;
            	col *= (_ColorShadow + lightIntensity);
                return col;
            }
            ENDHLSL
        }
    	//UsePass "Universal Render Pipeline/Lit/ShadowCaster"
	}
}
