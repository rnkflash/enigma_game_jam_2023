Shader "GachaSurvivals/Ground"
{
    Properties
    {
        _Color ("Glow Color", Color ) = ( 1, 1, 1, 1)
        [Header(Textures)]
        [Space(10)] 
        _Texture1("Diffuse_1", 2D) = "white" {}
        
        [NoScaleOffset]_Texture2("Diffuse_2", 2D) = "white" {}
        
        [NoScaleOffset]_Texture3("Diffuse_3", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline"}

        // Include material cbuffer for all passes. 
        // The cbuffer has to be the same for all passes to make this shader SRP batcher compatible.
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"


        CBUFFER_START(UnityPerMaterial)
        float4 _Texture1_ST;
        float4 _BaseColor;
        half4 _Color;
        CBUFFER_END
        ENDHLSL

        
        //First Pass
        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            
            Offset 1 , 1
            
            Stencil 
            {
                Ref 6
                Comp always
                Pass replace
                Fail keep
            }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            
            // -------------------------------------
            // Universal Render Pipeline keywords
            /*
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            */
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Assets/Content/Rendering/Lib/PostProcess.hlsl"
                      
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                float4 color  : COLOR;
            };

            struct Varyings
            {
                half2 uv           : TEXCOORD0;
                half4 positionHCS  : SV_POSITION;
                half3 VertexColor   : TEXCOORD2;
                //float3 positionWS   : TEXCOORD3;
                half4 screenPos 		: TEXCOORD4;
            };

            TEXTURE2D(_Texture1);
            TEXTURE2D(_Texture2);
            TEXTURE2D(_Texture3);
            SAMPLER(sampler_Texture1);
            SAMPLER(sampler_Texture2);
            SAMPLER(sampler_Texture3);

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionHCS = positionInputs.positionCS;
                OUT.screenPos = ComputeScreenPos(OUT.positionHCS);
                
                OUT.uv = TRANSFORM_TEX(IN.uv, _Texture1);
                OUT.VertexColor = IN.color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {

                half4 tex1 = SAMPLE_TEXTURE2D(_Texture1, sampler_Texture1, IN.uv);
                half4 tex2 = SAMPLE_TEXTURE2D(_Texture2, sampler_Texture2, IN.uv);
                half4 tex3 = SAMPLE_TEXTURE2D(_Texture3, sampler_Texture3, IN.uv);

                tex1 = lerp(tex1, tex2, IN.VertexColor.x);
                tex1 = lerp(tex1, tex3, IN.VertexColor.y);

                half4 finalColor = tex1 * _Color;
                half vingetteScreenPos = distance(IN.screenPos.xy / IN.screenPos.w, float2(0.5, 0.5));
                half vingetteMask = vingetteScreenPos * _Vignette;
                
                finalColor.rgb = postProcessing(finalColor.rgb, vingetteMask);
                return finalColor * _GIIntensity;
            }
            ENDHLSL
        }

        // Used for rendering shadowmaps
        // TODO: there's one issue with adding this UsePass here, it won't make this shader compatible with SRP Batcher
        // as the ShadowCaster pass from Lit shader is using a different UnityPerMaterial CBUFFER. 
        // Maybe we should add a DECLARE_PASS macro that allows to user to inform the UnityPerMaterial CBUFFER to use?
        
        
        //UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}