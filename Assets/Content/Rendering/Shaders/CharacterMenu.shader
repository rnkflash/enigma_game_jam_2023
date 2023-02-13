Shader "GachaSurvivals/СharacterMenu"
{
    Properties
    {
        [MainColor] _BaseColor("BaseColor", Color) = (1,1,1,1)
        [MainTexture] _BaseMap("BaseMap", 2D) = "white" {}
        _Darker ("Darker Color", Color) = (0.7, 0.7, 0.7, 1)
        //_MainLightDir ("LightDir", Vector) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline"}

        // Include material cbuffer for all passes. 
        // The cbuffer has to be the same for all passes to make this shader SRP batcher compatible.
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
        float4 _BaseMap_ST;
        half4 _BaseColor;
        float4 _Darker;
        //float4 _MainLightDir;
        CBUFFER_END
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
            //#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            //#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            //#pragma multi_compile _ _SHADOWS_SOFT

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
                float3 positionWS   : TEXCOORD1;
                float4 positionHCS  : SV_POSITION;
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
                OUT.positionWS = positionInputs.positionWS;
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.worldNormal = normalize(mul(IN.normal, (float3x3)UNITY_MATRIX_I_M));
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                fixed3 worldViewDir = normalize(IN.worldNormal);
                
                // shadowCoord is position in shadow light space
                float3 normal = normalize(IN.worldNormal);
				float NdotL = (dot(normalize(_LightDir.xyz), normal));

                float rimDot = 1 - dot(worldViewDir, normal);
				half rimIntensity = rimDot * pow(NdotL, 0.5);
				rimIntensity = smoothstep(_Rim - 0.01, _Rim + 0.01, rimIntensity);
				half3 rim = rimIntensity * _RimColor;
                
                //NdotL = NdotL * 0.5 + 0.5; 
            	float lightIntensity = NdotL > 0 ? 1 : 0;
                
                //float4 shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
                //Light mainLight = GetMainLight(shadowCoord);
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);// * _BaseColor;
                //color *= (mainLight.shadowAttenuation);
                color *= (_BaseColor + lightIntensity);
                
                

                
                //color *= (1 + lightIntensity);
                color = lerp(color, color * _Darker, saturate(1 - IN.positionHCS.y)+0.5);
                half4 finalColor = color;
                finalColor.rgb = postProcessing(finalColor.rgb, 0);
                finalColor.rgb = finalColor.rgb * _SkyReflection + rim;
                return finalColor * _GIIntensity;
                
                //return color;
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