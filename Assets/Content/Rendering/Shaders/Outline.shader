Shader "Custom/Outline"
{
    Properties
    {
        _Color ("Glow Color", Color ) = ( 1, 1, 1, 1)
        _Intensity ("Intensity", Float) = 2
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 0
    }
    
    SubShader
    {       
        HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        
        struct Attributes
        {
            float4 positionOS   : POSITION;
            float2 uv           : TEXCOORD0;
        };
        
        struct Varyings
        {
            half4 positionCS    : SV_POSITION;
            half2 uv            : TEXCOORD0;
        };

        TEXTURE2D_X(_OutlineRenderTexture);
        SAMPLER(sampler_OutlineRenderTexture);
        

        half4 _Color;

        Varyings Vertex(Attributes input)
        {
            Varyings output;
            output.positionCS = float4(input.positionOS.xy, 0.0, 1.0); 
            output.uv = input.uv;
            if (_ProjectionParams.x < 0.0) 
                output.uv.y = 1.0 - output.uv.y;	
            return output;      
        }

        half4 Fragment(Varyings input) : SV_Target
        {
            float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
            half4 prepassColor = SAMPLE_TEXTURE2D_X(_OutlineRenderTexture, sampler_OutlineRenderTexture, uv);
            half4 color = prepassColor * _Color;
            color.a = 1;    
            return color;
        }
        
        ENDHLSL        
     
        Pass
        {
            Blend [_SrcBlend] [_DstBlend]
            
            Stencil
             {
                Ref 1
                Comp NotEqual
                Pass Replace
                Fail Keep
                ZFail Keep
                ReadMask 1
                WriteMask 1
            }  
            
            ZTest Always    // всегда рисуем, независимо от текущей глубины в буфере
            ZWrite Off      // и ничего в него не пишем
            Cull Off        // рисуем все стороны меша

            HLSLPROGRAM
           
            #pragma vertex Vertex
            #pragma fragment Fragment        

            ENDHLSL         
        }
    }
}
