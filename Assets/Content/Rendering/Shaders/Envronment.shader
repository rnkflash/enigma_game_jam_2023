Shader "GachaSurvivals/Environment"
{
    Properties
    {
        [MainColor] _BaseColor("BaseColor", Color) = (1,1,1,1)
        [MainTexture] _BaseMap("BaseMap", 2D) = "white" {}
        _ColorShadow ("ColorShadow", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline"}
        

        // Include material cbuffer for all passes. 
        // The cbuffer has to be the same for all passes to make this shader SRP batcher compatible.
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        //CBUFFER_START(UnityPerMaterial)
        float4 _BaseMap_ST;
        half4 _BaseColor;
        half4 _ColorShadow;
        //CBUFFER_END
        ENDHLSL

        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            
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
            
            // -------------------------------------
            // Universal Render Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Assets/Content/Rendering/Lib/PostProcess.hlsl"
                      
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                //float3 positionWS   : TEXCOORD1;
                float4 positionHCS  : SV_POSITION;
                float4 screenPos 		: TEXCOORD1;
                float3 worldNormal : NORMAL;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                // GetVertexPositionInputs computes position in different spaces (ViewSpace, WorldSpace, Homogeneous Clip Space)
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionHCS = positionInputs.positionCS;
                //OUT.positionWS = positionInputs.positionWS;
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.screenPos = ComputeScreenPos(OUT.positionHCS);
                OUT.worldNormal = normalize(mul(IN.normal, (float3x3)UNITY_MATRIX_I_M));
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // shadowCoord is position in shadow light space
                float3 normal = normalize(IN.worldNormal);
				float NdotL = saturate(dot(normalize(_LightDir.xyz), normal));
            	float lightIntensity = NdotL > 0 ? 1 : 0;
                
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;

                color *= (_ColorShadow + lightIntensity);

                half4 finalColor = color;
                
                float vingetteScreenPos = distance(IN.screenPos.xy / IN.screenPos.w, float2(0.5, 0.5));
				//float vingette = float4(vingetteScreenPos, vingetteScreenPos, vingetteScreenPos, 1.0);
                half vingetteMask = vingetteScreenPos * _Vignette;
                
                finalColor.rgb = postProcessing(finalColor.rgb, vingetteMask);
                //finalColor.rgb = postProcessing(finalColor.rgb, 0);
                return finalColor * _GIIntensity;
                return finalColor * _GIIntensity;
                return finalColor;
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