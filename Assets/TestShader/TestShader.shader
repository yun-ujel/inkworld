Shader "Experimental/TestShader"
{
    // Properties are options set per material, exposed by the material inspector
    Properties
    {
        [Header(Surface Options)] // Creates a text header
        
        // [MainTexture] and [MainColor] allow Material.mainTexture and Material.color to use the correct properties
        [MainTexture] _ColorMap("Color", 2D) = "white" {}
        [MainColor] _ColorTint("Tint", Color) = (1, 1, 1, 1)
    }


	// Subshaders allow for different behaviour and options for different pipelines and platforms
    SubShader
    {
        // These tags are shared by all passes in this sub shader
        Tags{"RenderPipeline" = "UniversalPipeline"}

        // Shaders can have several passes which are used to render different data about the material
        // Each pass has it's own vertex and fragment function and shader variant keywords
        Pass 
        {
            Name "ForwardLit" // For debugging
            Tags{"LightMode" = "UniversalForward"} // Pass specific tags. 
            // "UniversalForward" tells Unity this is the main lighting pass of this shader
            
            HLSLPROGRAM // Begin HLSL code
            
            // Register our programmable stage functions
            #pragma vertex Vertex
            #pragma fragment Fragment

            // Include our code file
            #include "TestShaderForwardLitPass.hlsl"
            
            ENDHLSL

        }
    }
}