Shader "GachaSurvivals/UniversalFX With StandartTexture"
{
    Properties
    {
        _BaseMap ("Texture", 2D) = "white" {}
        _BaseColor("Color", Color) = (1, 1, 1, 1)

        [Toggle] _AlphaTest("Alpha Clip", Float) = 0.0
        _Cutoff("AlphaCutout", Range(0.0, 1.0)) = 0.5
        [Toggle] _AlphaPremultiply("Alpha Premultiply", Float) = 1
        _AlphaBeforeAfterPremultiply("BlackWhite for Premultiply XY, for Alpha ZW", Vector) = (0,1,0,1)

        [KeywordEnum(Normal, CheckColor, CheckAlpha)] _RenderMode ("Render Mode", Float) = 0
        [Header(Rendering Settings)]
        [Space]
        
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Blend Source", Float) = 1.0
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Blend Destination", Float) = 10.0
        [Toggle] _ZWrite("ZWrite", Float) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Int) = 4
        _Offset("ZOffset", float) = 0
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 2.0
        [Enum(None,0,Alpha,1,Red,8,Green,4,Blue,2,RGB,14,RGBA,15)] _ColorMask("Color Mask", Int) = 15


        [Header(Stencil Settings)]
        [Space]
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Comp", Float) = 8
        [Enum(UnityEngine.Rendering.StencilOp)]_StencilPass ("Pass", Float) = 0
        [Enum(UnityEngine.Rendering.StencilOp)]_StencilFail ("Fail", Float) = 0
        [Enum(UnityEngine.Rendering.StencilOp)]_StencilZFail ("ZFail", Float) = 0
        [IntRange]_Stencil ("Ref", Range(0,255)) = 0
        [IntRange]_StencilReadMask ("ReadMask", Range(0,255)) = 255
        [IntRange]_StencilWriteMask ("WriteMask", Range(0,255)) = 255
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalPipeline" "Queue"="Transparent"}
        LOD 100

        Pass
        {
            Name "Unlit"

            Stencil
            {
                Ref [_Stencil]
                Comp [_StencilComp]
                Pass [_StencilPass]
                Fail [_StencilFail]
                ZFail [_StencilZFail]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
            }

            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            ZTest[_ZTest]
            Cull[_Cull]
            Offset[_Offset],[_Offset]
            ColorMask[_ColorMask]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _ _RENDERMODE_CHECKCOLOR _RENDERMODE_CHECKALPHA

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #include "Assets/Content/Rendering/Shaders/Test/UnlitUniversalFXInput.hlsl"

            struct Attributes
            {
                float4 positionOS       : POSITION;
                float2 uv               : TEXCOORD0;
                float4 color            : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv        : TEXCOORD0;
                float fogCoord  : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float4 color            : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.vertex = vertexInput.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
                output.color = input.color * _BaseColor;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                half2 uv = input.uv;
                half4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv) * input.color;
                half3 color = texColor.rgb;
                half alpha = texColor.a;
                AlphaDiscard(alpha, _Cutoff);

#ifdef _ALPHAPREMULTIPLY_ON
                half modAlpha = (alpha - _AlphaBeforeAfterPremultiply.x) / (_AlphaBeforeAfterPremultiply.y - _AlphaBeforeAfterPremultiply.x);
                modAlpha = saturate(modAlpha);
                color *= modAlpha;
#endif

                color = MixFog(color, input.fogCoord);
                alpha = OutputAlpha(alpha);
                half afterAlpha = (alpha - _AlphaBeforeAfterPremultiply.z) / (_AlphaBeforeAfterPremultiply.w - _AlphaBeforeAfterPremultiply.z);
                afterAlpha = saturate(afterAlpha);
#if defined(_RENDERMODE_CHECKCOLOR)
                return half4(color, 1);
#elif defined(_RENDERMODE_CHECKALPHA)
                return half4(afterAlpha, afterAlpha, afterAlpha, 1);
#else
                return half4(color, afterAlpha);
#endif
            }
            ENDHLSL
        }
        Pass
        {
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #include "Assets/Content/Rendering/Shaders/Test/UnlitUniversalFXInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }
    }
}
