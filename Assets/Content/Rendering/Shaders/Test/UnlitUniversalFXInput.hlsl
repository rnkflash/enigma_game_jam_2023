#ifndef UNIVERSAL_UNLIT_INPUT_INCLUDED
#define UNIVERSAL_UNLIT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
float4 _BaseMap_ST;
float4 _AlphaBeforeAfterPremultiply;
half4 _BaseColor;
half _Cutoff;
half _Glossiness;
half _Metallic;
CBUFFER_END

#endif
