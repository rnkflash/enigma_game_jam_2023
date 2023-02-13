Shader "GachaSurvivals/UberFX"
{
    Properties
    {
        [Header(Main Settings)]
        [Space]
        _BaseMap ("Texture", 2D) = "white" {}
        _BaseColor("Color", Color) = (1, 1, 1, 1)

        [Header(Base Color Settings)]
        [Space]
        [Toggle] _UseLUT("Use LUT", Float) = 0.0
        [NoScaleOffset]_LUTMap ("LUT", 2D) = "white" {}

        _GradientSmoothness("Gradient Smooothness", Range(0,1)) = 0
        //[KeywordEnum(Cubic, Five, Sin)] _SmoothFunction ("Smooth Mode", Float) = 0
        _BaseColor1("Color 1", Color) = (0,0,0,1)
        _BaseColor1Ratio("Color 1 Ratio", Range(0,1)) = 0
        _BaseColor2("Color 2", Color) = (0.333,0.333,0.333,1)
        _BaseColor2Ratio("Color 2 Ratio", Range(0,1)) = 0.333
        _BaseColor3("Color 3", Color) = (0.667,0.667,0.667,1)
        _BaseColor3Ratio("Color 3 Ratio", Range(0,1)) = 0.667
        _BaseColor4("Color 4", Color) = (1,1,1,1)
        _BaseColor4Ratio("Color 4 Ratio", Range(0,1)) = 1

        [Header(Emission Settings)]
        [Space]
        _EmissionColor1("Emission Color 1", Color) = (0,0,0,1)
        _EmissionColor1Ratio("Color 1 Ratio", Range(0,1)) = 0
        _EmissionColor2("Emission Color 2", Color) = (1,1,1,1)
        _EmissionColor2Ratio("Color 2 Ratio", Range(0,1)) = 1

        [Header(Fade Settings)]
        [Space]
        [Toggle] _Emission("Use Emission", Float) = 0.0
        [Toggle] _FadeAxis("Fade By Axis", Float) = 0
        _FadeVector("_FadeVector", Vector) = (0,0,0,0)

        [Header(Alpha Settings)]
        [Space]
        [Toggle] _BlueChannelAlpha("Use Blue Channel for Alpha", Float) = 0.0
        [Toggle] _UseSecondUVForAlpha("Use Second UV for Alpha", Float) = 0.0
        [Toggle] _AlphaTest("Alpha Clip", Float) = 0.0
        _Cutoff("AlphaCutout", Range(0.0, 1.0)) = 0.5
        [Toggle] _AlphaPremultiply("Alpha Premultiply", Float) = 1

        
        [Header(Rendering Settings)]
        [Space]
        [KeywordEnum(Normal, CheckColor, CheckAlpha)] _RenderMode ("Render Mode", Float) = 0
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
            #pragma shader_feature _FADEAXIS_ON
            #pragma shader_feature _USELUT_ON
            #pragma shader_feature _BLUECHANNELALPHA_ON
            #pragma shader_feature _USESECONDUVFORALPHA_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _EMISSION_ON
            #pragma shader_feature _ _RENDERMODE_CHECKCOLOR _RENDERMODE_CHECKALPHA
            //#pragma shader_feature _SMOOTHFUNCTION_CUBIC _SMOOTHFUNCTION_FIVE _SMOOTHFUNCTION_SIN

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #include "Assets/Content/Rendering/Shaders/Test/UberFXInput.hlsl"

            struct Attributes
            {
                float4 positionOS       : POSITION;
                float2 uv               : TEXCOORD0;
                float2 uv1              : TEXCOORD1;
                float4 color            : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 uv        : TEXCOORD0;
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
                output.uv.xy = TRANSFORM_TEX(input.uv, _BaseMap);
                output.uv.zw = input.uv1;
                output.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
                output.color = input.color * _BaseColor;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv = input.uv.xy;
                half3 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
                half colorMask = texColor.r;
                float alphaOverride = GetAlpha(texColor);
#if defined(_USESECONDUVFORALPHA_ON)
                float2 uv2 = input.uv.zw;
                half3 texColorUV2 = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv2);
                alphaOverride = GetAlpha(texColorUV2);
#endif

#if defined(_USELUT_ON)
                half3 color = GetColorFromLUT(texColor.r);
                color = GetColorFromGradient(texColor.r);
#else
                half3 color = texColor; 
#endif
#if defined(_EMISSION_ON)
                half3 emissionColor = GetColorBetweenTwoColors(_EmissionColor1, _EmissionColor2, _EmissionColor1Ratio, _EmissionColor2Ratio, texColor.g);
                color += emissionColor;
#endif                
                color *= input.color.rgb;
#if defined(_FADEAXIS_ON)
                float fadeXAxis = RemapTo01(_FadeVector.x, _FadeVector.x + _FadeVector.y, uv.x) - 1;
                float fadeYAxis = RemapTo01(_FadeVector.z, _FadeVector.z + _FadeVector.w, uv.y) - 1;
                alphaOverride += fadeXAxis + fadeYAxis;
                alphaOverride *= input.color.a;
#else
                alphaOverride = RemapTo01(_FadeVector.x, _FadeVector.y, alphaOverride);
                alphaOverride *= input.color.a;
#endif
                float alpha = texColor.r * alphaOverride;
                alpha = saturate(alpha);
                AlphaDiscard(alpha, _Cutoff);
#ifdef _ALPHAPREMULTIPLY_ON
                color *= alpha;
#endif

                color = MixFog(color, input.fogCoord);
                alpha = OutputAlpha(alpha);
#if defined(_RENDERMODE_CHECKCOLOR)
                return half4(color, 1);
#elif defined(_RENDERMODE_CHECKALPHA)
                return half4(alpha, alpha, alpha, 1);
#else
                return half4(color, alpha);
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

            #include "Assets/Content/Rendering/Shaders/Test/UberFXInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }
    }
}
