// Pixel shader extracts the brighter areas of an image.
// This is the first step in applying a bloom postprocess.

sampler TextureSampler : register(s0);

float BloomThreshold = .85f;
float blur = 0.008; 
 
struct PixelInput 
{ 
    float2 TexCoord : TEXCOORD0; 
}; 

float4 PixelShaderFunction(PixelInput input, float2 texCoord : TEXCOORD0) : COLOR0
{
    // Look up the original image color.
    float4 color = tex2D(TextureSampler,     float2(input.TexCoord.x+blur, input.TexCoord.y+blur)); 

		if((color.r == (1.0f/255)*255 && color.g > (1.0f/255)*160 && color.b == 0) || (color.r == (1.0f/255)*255 && color.g == 0 && color.b == 0)){
    // Adjust it to keep only values brighter than the specified threshold.
    color += tex2D( TextureSampler, 
        float2(input.TexCoord.x-blur, input.TexCoord.y-blur)); 
    color += tex2D( TextureSampler, 
        float2(input.TexCoord.x+blur, input.TexCoord.y-blur)); 
    color += tex2D( TextureSampler, 
        float2(input.TexCoord.x-blur, input.TexCoord.y+blur)); 

	color.rgb *= 5;
	color.rgb += .5f;
    color = color / 4; 
	color = saturate((color - BloomThreshold) / (1 - BloomThreshold));
	
		}
    return color;
}


technique BloomExtract
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}