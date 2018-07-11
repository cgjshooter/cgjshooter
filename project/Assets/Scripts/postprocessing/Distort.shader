Shader "Hidden/Custom/Distort"
{
	HLSLINCLUDE

#include "PostProcessing/Shaders/StdLib.hlsl"

		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float _Height;
	float _Intensity;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float t = _Time * 600.;
		float2 tc = float2(i.texcoord.x, i.texcoord.y);
		tc.x *= 1.+( sin(tc.y*_Height) + sin(tc.y*_Height*0.65+0.73) )*_Intensity;
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