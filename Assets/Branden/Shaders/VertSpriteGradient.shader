Shader "Custom/VertSpriteGradient" {
	Properties{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Top Color", Color) = (1,1,1,1)
		_Color2("Bottom Color", Color) = (1,1,1,1)
		_Scale("Scale", Float) = 1

		// these six unused properties are required when a shader
		// is used in the UI system, or you get a warning.
		// look to UI-Default.shader to see these.
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
	}

	SubShader{
		Tags{
			"Queue" = "Background" 
			"IgnoreProjector" = "True"
			"RenderType"="Transparent"
		}

		LOD 100
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{
			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag
			#include "UnityCG.cginc"

			fixed4 _Color;
			fixed4 _Color2;
			fixed  _Scale;

			struct v2f {
				float4 pos : SV_POSITION;
				fixed4 col : COLOR;
			};

			v2f vert(appdata_full v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

				fixed4 rgbLerped = lerp(_Color2, _Color, v.texcoord.y);
				rgbLerped.a = v.color.a;

				o.col = rgbLerped;
				return o;
			}

			float4 frag(v2f i) : COLOR{
				float4 c = i.col;
				return c;
			}
			ENDCG
		}
	}
}