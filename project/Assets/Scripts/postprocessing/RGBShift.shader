Shader "Hidden/Custom/RGBShift"
{
	HLSLINCLUDE

#include "PostProcessing/Shaders/StdLib.hlsl"

		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float _Rshift;
	float _Bshift;
	float _Gshift;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float2 tcR = float2(i.texcoord.x+_Rshift, i.texcoord.y);
		float2 tcG = float2(i.texcoord.x+_Gshift, i.texcoord.y);
		float2 tcB = float2(i.texcoord.x+_Bshift, i.texcoord.y);
		float4 color = float4(
			SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, tcR).r,
			SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, tcG).g,
			SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, tcB).b,
			1.0);
		return color;
	}

		ENDHLSL

		SubShader
	{
		Cull Off ZWrite Off ZTest Always

			Pass
		{
			HLSLPROGRAM

#pragma vertex VertDefault
#pragma fragment Frag

			ENDHLSL
		}
	}
}