Shader "Inkworld/Global/ShaderSkeleton" {
	Properties {
		[MainTexture] _ColorMap("Color", 2D) = "white" {}
		[MainColor] _ColorTint("Tint", Color) = (1, 1, 1, 1)
	}

	SubShader {
		Tags {
			"RenderPipeline" = "UniversalPipeline"
		}

		Pass {
		Name "ForwardLit"
		Tags {"LightMode" = "UniversalForward"}

		HLSLPROGRAM

		#pragma vertex Vertex
		#pragma fragment Fragment

		#include "ShaderSkeletonForwardLitPass.hlsl"

		ENDHLSL

		}
	}
}