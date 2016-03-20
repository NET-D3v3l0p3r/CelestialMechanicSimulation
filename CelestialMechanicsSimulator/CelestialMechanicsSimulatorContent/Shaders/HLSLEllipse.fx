texture colortexture;

float screenWidth;
float screenHeight;
float radius;
float2 pos;

sampler ColorMap = sampler_state
{
 Texture = <colortexture>;
};


float4 CirclePS(float2 texCoords : TEXCOORD0) : COLOR
{
	float4 texColour = tex2D(ColorMap, texCoords);

	float3 pixelPosition = float3(screenWidth * texCoords.x,
	screenHeight * texCoords.y,0);

	float2 difference = pos - pixelPosition;
	float final = length(difference);

	float3 finalColor = texColour.rgb;

	if(final < radius)
	{
		if(final + 2 > radius)
		{
			finalColor = float3(0.2f, 0.7f, 0.2f);
		}
		else
		{
			//finalColor = float3(1.0f, 1.0f, 1.0f) * texColour.rgb;
			finalColor = 0.9f * texColour.rgb;
		}
	}

	return float4(finalColor, 1.0f);
}

technique Circle
{
 pass Pass1
 {
 PixelShader = compile ps_2_0 CirclePS();
 }
}