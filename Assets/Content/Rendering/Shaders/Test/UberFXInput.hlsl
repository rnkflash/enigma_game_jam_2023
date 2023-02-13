#ifndef UNIVERSAL_UNLIT_INPUT_INCLUDED
#define UNIVERSAL_UNLIT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
float4 _BaseMap_ST;
float4 _FadeVector;
half4 _BaseColor;
half3 _BaseColor1;
half3 _BaseColor2;
half3 _BaseColor3;
half3 _BaseColor4;
float _BaseColor1Ratio;
float _BaseColor2Ratio;
float _BaseColor3Ratio;
float _BaseColor4Ratio;
half3 _EmissionColor1;
half3 _EmissionColor2;
float _EmissionColor1Ratio;
float _EmissionColor2Ratio;
float _GradientSmoothness;
half _Cutoff;
half _Glossiness;
half _Metallic;
CBUFFER_END

TEXTURE2D(_LUTMap);       SAMPLER(sampler_LUTMap);

#endif

float SmoothFunc(float x){//x должен иметь значения от 0 до 1
#if defined(_SMOOTHFUNCTION_CUBIC)
    return x*x*(3.0-2.0*x);
#elif defined(_SMOOTHFUNCTION_FIVE)
    return x*x*x*(x*(x*6-15)+10);
#elif defined(_SMOOTHFUNCTION_SIN)
    return sin((x * 2 - 1) * HALF_PI) * 0.5 + 0.5;
#else
    return x*x*(3.0-2.0*x);
#endif
}

float RemapTo01(float a, float b, float x){
    return saturate((x - a)/(b - a));
}

half3 GetColorBetweenTwoColors(half3 color1, half3 color2, float colorRatio1, float colorRatio2, float x){
    float remapedLinearX = RemapTo01(colorRatio1, colorRatio2, x);
    float smoothedX = SmoothFunc(remapedLinearX);
    float mixedX = lerp(remapedLinearX, smoothedX, _GradientSmoothness);
    return lerp(color1, color2, mixedX);
}

half3 GetColorFromGradient(float x){
    half3 colLerp1 = GetColorBetweenTwoColors(_BaseColor1, _BaseColor2, _BaseColor1Ratio, _BaseColor2Ratio, x);
    half3 colLerp2 = GetColorBetweenTwoColors(_BaseColor2, _BaseColor3, _BaseColor2Ratio, _BaseColor3Ratio, x);
    half3 colLerp3 = GetColorBetweenTwoColors(_BaseColor3, _BaseColor4, _BaseColor3Ratio, _BaseColor4Ratio, x);

    half3 col = _BaseColor1;
    if(x < _BaseColor1Ratio)
        col = _BaseColor1;
    else if(x < _BaseColor2Ratio)
        col = colLerp1;
    else if (x < _BaseColor3Ratio)
        col = colLerp2;
    else if (x < _BaseColor4Ratio)
        col = colLerp3;
    else
        col = _BaseColor4;
    return col;
}

half3 GetColorFromLUT(float x){
    return SAMPLE_TEXTURE2D(_LUTMap, sampler_LUTMap, float2(x, 0.5)).rgb;
}

float GetAlpha(float3 textureSample){
#if defined(_BLUECHANNELALPHA_ON)
	return textureSample.b;
#else
	return textureSample.r;
#endif
}