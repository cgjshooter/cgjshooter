Shader "Hidden/Custom/Pixelate"
{
	HLSLINCLUDE

#include "PostProcessing/Shaders/StdLib.hlsl"

		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	
	float2 _Pixelate;


	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float2 tc = floor(i.texcoord.xy * _Pixelate) / _Pixelate;
		
		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, tc);
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