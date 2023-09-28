// This file contains the vertex and fragment functions for the forward lit pass
// This is the shader pass that computes visible colors for a material
// by reading material, light, shadow, etc. data

// Pull in URP library functions and our own common functions
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

// PROPERTIES

float4 _ColorTint;
float _Smoothness;

// Textures
TEXTURE2D(_ColorMap); SAMPLER(sampler_ColorMap); // RGB = albedo, A = alpha

float4 _ColorMap_ST; // This is automatically set by Unity. Used in TRANSFORM_TEX to apply UV tiling


// This attributes struct receives data about the mesh we're currently rendering
// Data is automatically placed in fields according to their semantic
struct Attributes {
	float3 positionOS : POSITION; // Position in object space
	float2 uv : TEXCOORD0; // Material texture UVs

	float3 normalOS : NORMAL;
};

// This struct is output by the vertex function and input to the fragment function.
// Note that fields will be transformed by the intermediary rasterization stage
struct Interpolators {
	// This value should contain the position in clip space (which is similar to a position on screen)
	// when output from the vertex function. It will be transformed into pixel position of the current
	// fragment on the screen when read from the fragment function
	float4 positionCS : SV_POSITION;

	// The following variables will retain their values from the vertex stage, except the
	// rasterizer will interpolate them between vertices
	float2 uv : TEXCOORD0; // Material texture UVs

	float3 normalWS : TEXCOORD1;
	float3 positionWS : TEXCOORD2;
};

// The vertex function. This runs for each vertex on the mesh.
// It must output the position on the screen each vertex should appear at,
// as well as any data the fragment function will need
Interpolators Vertex(Attributes input) {
	Interpolators output;

	// These helper functions, found in URP/ShaderLib/ShaderVariablesFunctions.hlsl
	// transform object space values into world and clip space
	VertexPositionInputs posnInputs = GetVertexPositionInputs(input.positionOS);
	VertexNormalInputs normInputs = GetVertexNormalInputs(input.normalOS);

	// Pass position and orientation data to the fragment function
	output.positionCS = posnInputs.positionCS;
	output.uv = TRANSFORM_TEX(input.uv, _ColorMap);

	output.normalWS = normInputs.normalWS;
	output.positionWS = posnInputs.positionWS;

	return output;
}

// The fragment function. This runs once per fragment, which you can think of as a pixel on the screen
// It must output the final color of this pixel
float4 Fragment(Interpolators input) : SV_TARGET {
	// Sample the color map
	float4 colorSample = SAMPLE_TEXTURE2D(_ColorMap, sampler_ColorMap, input.uv);

	InputData lightingInput = (InputData)0;
	
	lightingInput.positionWS = input.positionWS;
	lightingInput.viewDirectionWS = GetWorldSpaceNormalizeViewDir(input.positionWS);

	lightingInput.normalWS = normalize(input.normalWS); // World Space vectors are normalized for smoother lighting.
	// This will take significantly more processing power, so if we're running slow, use the expression below instead:
	//lightingInput.normalWS = input.normalWS;

	lightingInput.shadowCoord = TransformWorldToShadowCoord(input.positionWS);

	SurfaceData surfaceInput = (SurfaceData)0;

	surfaceInput.albedo = colorSample.rgb * _ColorTint.rgb;
	surfaceInput.alpha = colorSample.a * _ColorTint.a;
	surfaceInput.specular = 1;
	surfaceInput.smoothness = _Smoothness;

	return UniversalFragmentBlinnPhong(lightingInput, surfaceInput);
}