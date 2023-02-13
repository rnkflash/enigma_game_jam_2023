Shader "GachaSurvivals/UberFX_Transparent"
{
    Properties
    {
        [Header(Transparent Blend)]
        [Space(10)]
        _Tint ("Color", Color) = (1,1,1,1)
        _Emission ("Emission", Color) = (0,0,0,0)
        [Space(10)]

        [Toggle(RGBA)] _MainTexRGBA ("RGBA", Float) = 0
        [Space(10)]
        _MainTex ("MainTex", 2D) = "black" {}
        [Space(10)]
    
        _Intensity ("Intensity", Range(1,3)) = 0.5
        _Opacity ("Alpha Opacity", Range(0.0,1)) = 1
        _CutOut ("CutOut", Range(0.0,1.0)) = 0
        [Space(10)]

        [Enum(Off,0,Front,1,Back,2)] _Cull ("Cull", Int) = 0
        [Enum(Off,0,On,1)] _ZWrite ("ZWrite", Int) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Int) = 4
    }

    SubShader
    {

        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha

        Cull [_Cull]
        ZWrite [_ZWrite]
        ZTest[_ZTest]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_instancing
            #pragma multi_compile __ RGBA

            #ifdef SHADER_API_OPENGL  
			#pragma glsl
			#endif
            
            #include "UnityCG.cginc"
            //#include "CustomRenderCG.cginc"

            struct input {
                float4 vertex           : POSITION;
                float2 uv               : TEXCOORD0;
                fixed4 vcolor           : COLOR0;
                
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos              : SV_POSITION;
                float2 uv               : TEXCOORD0;
                fixed4 vcolor           : COLOR0;

                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            sampler2D _MainTex;
            half4 _MainTex_ST;

            fixed4 _Tint;
            fixed4 _Emission;

            half _Intensity;
            half _Opacity;
            half _CutOut;
                    
            v2f vert (input v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                //Position
				o.pos = UnityObjectToClipPos(v.vertex);

                //UV
                o.uv.xy = v.uv;

                //Vetex Color
                o.vcolor = v.vcolor;

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                //Texture
                #ifdef RGBA
                fixed4 baseColor = tex2D(_MainTex, TRANSFORM_TEX(i.uv.xy, _MainTex));
                #else
                fixed mainTex = tex2D(_MainTex, TRANSFORM_TEX(i.uv.xy, _MainTex)).r;
                #endif

                //Color
                fixed4 colorOut;
                colorOut.rgb = i.vcolor.rgb;
                colorOut.rgb *= _Tint * _Intensity;
                #ifdef RGBA
                colorOut.rgb *= baseColor;
                #endif

                //Alpha
                colorOut.a = i.vcolor.a * _Opacity;
                #ifdef RGBA
                colorOut.a *= baseColor.a;
                #else
                colorOut.a *= mainTex;
                #endif

                //Emission
                half3 emission = _Emission * 10;
                colorOut.rgb += emission;

                //CutOut
                clip(colorOut.a - _CutOut);

                //Post
                //colorOut.rgb = colorOut.rgb * _Gain;
                colorOut.rgb = colorOut.rgb;
                fixed3 luma = dot(colorOut.rgb, fixed3(0.213, 0.715, 0.072));
	            fixed3 diff = colorOut - luma;
	            //colorOut.rgb = luma + diff * _Saturation;
	            colorOut.rgb = luma + diff;
                			
                return fixed4(colorOut.rgb, colorOut.a);
            }
            ENDCG
        }
    }
}