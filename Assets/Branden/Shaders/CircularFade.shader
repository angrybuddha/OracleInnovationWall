Shader "Custom/CircularFade" {
	//Properties{
	//	_Color("Color", Color) = (1,1,1,1)
	//	_MainTex("Main Texture", 2D) = "white" {}

	//	_CutoffRange("Cutoff Range", Range(0,1)) = 0.2

	//	// these six unused properties are required when a shader
	//	// is used in the UI system, or you get a warning.
	//	// look to UI-Default.shader to see these.
	//	_StencilComp("Stencil Comparison", Float) = 8
	//	_Stencil("Stencil ID", Float) = 0
	//	_StencilOp("Stencil Operation", Float) = 0
	//	_StencilWriteMask("Stencil Write Mask", Float) = 255
	//	_StencilReadMask("Stencil Read Mask", Float) = 255
	//	_ColorMask("Color Mask", Float) = 15
	//}

	//SubShader{
	//	Tags{ "Queue" = "Transparent"
	//		"RenderType" = "Transparent"
	//		"IgnoreProjector" = "True" 
	//	}

	//	LOD 100

	//	CGPROGRAM
	//	#pragma surface surf BlinnPhong noforwardadd alpha:fade
	//	#pragma target 3.0

	//	#define EPSILON 0.0000001

	//	sampler2D _MainTex;
	//	uniform fixed4 _Color;

	//	uniform fixed _CutoffRange;

	//	fixed when_eq(fixed x, fixed y) {
	//		return 1 - abs(sign(x - y));
	//	}

	//	fixed when_gt(fixed x, fixed y) {
	//		return max(sign(x - y), 0);
	//	}

	//	fixed when_le(fixed x, fixed y) {
	//		return 1 - when_gt(x, y);
	//	}

	//	struct Input
	//	{
	//		fixed2 uv_MainTex;
	//		float3 worldPos;
	//	};

	//	void surf(Input IN, inout SurfaceOutput o)
	//	{
	//		float3 toWorldPoint = IN.worldPos.xyz - _CutoffPoint.xyz;

	//		float dist = dot(toWorldPoint, _CutoffDir.xyz);
	//		fixed forward = when_gt(dist, 0);
	//		fixed backward = when_le(dist, 0);

	//		fixed4 mainCol = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	//		fixed4 secCol = tex2D(_SecTex, IN.uv_SecTex) * _SecColor;

	//		float blendAmount = max(EPSILON, _BlendAmount);
	//		float blendDist = when_eq(_BlendOutIn, 1) * _BlendDist;

	//		dist += (when_eq(_BlendOutIn, 0) * when_eq(_CenterBlend, 1) * (blendAmount / 2));

	//		fixed scale = (when_eq(_BlendOutIn, 1) * max(0, abs(dist) - blendDist) / blendAmount) +
	//			(when_eq(_BlendOutIn, 0) * max(0, dist / blendAmount));

	//		fixed4 lerpedColor = lerp(mainCol, secCol, clamp(scale, 0, 1));

	//		o.Alpha = lerpedColor.a;
	//		o.Emission = lerpedColor.rgb;
	//	}

	//	ENDCG
	//}

	FallBack "Diffuse"
}
