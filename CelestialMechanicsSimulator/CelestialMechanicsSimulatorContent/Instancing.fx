//-----------------------------------------------------------------------------
// InstancedModel.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

float4x4 World;
float4x4 View;
float4x4 Projection;

float3 InstanceColor;

struct VertexShaderInput
{
    float4 Position : POSITION0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
};

VertexShaderOutput VertexShaderCommon(VertexShaderInput input, float4x4 instanceTransform)
{
    VertexShaderOutput output;
    float4 worldPosition = mul(input.Position, instanceTransform);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    return output;
}

VertexShaderOutput HardwareInstancingVertexShader(VertexShaderInput input,
	float4x4 instanceTransform : BLENDWEIGHT)
{
    return VertexShaderCommon(input, mul(World, transpose(instanceTransform)));
}
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	return float4(InstanceColor.rgb, 1);
}

technique HardwareInstancing
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 HardwareInstancingVertexShader();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
