float4x4 World;
float4x4 View;
float4x4 Projection;

texture SurfaceTexture;
sampler2D surfaceTexSampler = sampler_state {
    Texture = (SurfaceTexture);
    Filter =   POINT;
    AddressU = Clamp;
    AddressV = Clamp;
};

texture HeatLevelTexture;
sampler2D heatLevelTexSampler = sampler_state {
    Texture = (HeatLevelTexture);
    Filter =   POINT;
    AddressU = Clamp;
    AddressV = Clamp;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TextureCoordinate : TEXCOORD0;
};
struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TextureCoordinate : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.TextureCoordinate = input.TextureCoordinate;

    return output;
}

bool ApplyHeatLevel;
float RandomFloat;
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 surfaceTexture = tex2D(surfaceTexSampler, input.TextureCoordinate);
	float4 heatLevelTexture = tex2D(heatLevelTexSampler, input.TextureCoordinate);
	float brightnessRGB = 0.2126 * surfaceTexture.r + 0.7152 * surfaceTexture.g + 0.0722 * surfaceTexture.b;

    return surfaceTexture;
}

technique Main
{
    pass P0
    {
		SrcBlend = SrcAlpha;
		DestBlend = One;
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}