Shader "GachaSurvivals/Grass"
{
    Properties
    {
        [MainColor] _BaseColor("BaseColor", Color) = (1,1,1,1)
        [MainTexture] _BaseMap("BaseMap", 2D) = "white" {}
        _ShadowPower ("Shadow Power", Float) = 0.35
        _Darker ("Darker Color", Color) = (0.5, 0.5, 0.5, 1)
        _DarkerShadows ("Darker Color in shadows", Color) = (0.5, 0.5, 0.5, 1)

        
        _WindDistortionMap ("Wind Distortion Map", 2D) = "white" {}
        _WindFrequency ("Wind Frequency", Vector) = (0.05, 0.05, 0, 0)
        _WindStrength ("Wind Strength", Float) = 1
        _WindStrength2 ("Wind Strength 2", Float) = 1
        
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
        float4 _WindFrequency; 
        float _WindStrength;
        float _WindStrength2;
        float4 _Darker;
        float4 _DarkerShadows;
        float _ShadowPower;


        //CBUFFER_END
        ENDHLSL

        Pass
        {
            Tags { "LightMode"="UniversalForward" }

            Cull Back

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
                float4 color        : COLOR;
            };

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                //float3 positionWS   : TEXCOORD1;
                float4 positionHCS  : SV_POSITION;
                float4 screenPos 		: TEXCOORD1;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            sampler2D _WindDistortionMap;

            //Vertex
            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                half posOffset = IN.positionOS.z + IN.positionOS.y * HALF_PI; // z x 

                half turbulenceSpeed = _Time.x * _WindFrequency.w + posOffset * _WindFrequency.z; // x.w.z

                float2 uv = (IN.color.xx * _Time.xy * _WindFrequency.xy); //x
                
                float2 windSample = tex2Dlod(_WindDistortionMap, float4(turbulenceSpeed,turbulenceSpeed * 1.2, 0, 0)).rg;

                windSample = 2 * windSample - 1;

                IN.positionOS.z += (windSample.y + 1) * IN.uv.y * _WindStrength; //y.y
                IN.positionOS.x += windSample.x * IN.uv.y * _WindStrength2; //z.y
                //IN.positionOS.z += sin(_Time.y) * _WindStrength;

                // y в высоту, x в стороны, z вперед и назад 

                // как было 

                // x в высоту, z вперед и назад, y в стороны

                // GetVertexPositionInputs computes position in different spaces (ViewSpace, WorldSpace, Homogeneous Clip Space)
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionHCS = positionInputs.positionCS;
                OUT.screenPos = ComputeScreenPos(OUT.positionHCS);
                //OUT.positionWS = positionInputs.positionWS;
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);


                return OUT;
            }

            //Fragment
            half4 frag(Varyings IN) : SV_Target
            {
                // shadowCoord is position in shadow light space
                //float4 shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
                //Light mainLight = GetMainLight(shadowCoord);
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                color = lerp(color, _Darker, 1 - IN.uv.y);

                half4 finalColor = color;
                float vingetteScreenPos = distance(IN.screenPos.xy / IN.screenPos.w, float2(0.5, 0.5));
				//float vingette = float4(vingetteScreenPos, vingetteScreenPos, vingetteScreenPos, 1.0);
                half vingetteMask = vingetteScreenPos * _Vignette;
                
                finalColor.rgb = postProcessing(finalColor.rgb, vingetteMask);
                //finalColor.rgb = postProcessing(finalColor.rgb, 0);
                return finalColor * _GIIntensity;
                //color = lerp(_DarkerShadows,color);
                return color;
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