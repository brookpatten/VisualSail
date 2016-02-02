float4x4 ViewMatrix;
float4x4 ProjectionMatrix;
float4x4 WorldMatrix;
float Time;
float3 sky_color;
float3 water_color;
float3 LightDirection;

//===========================================================================

texture CubeMapTexture;
samplerCUBE CubeMapSampler = sampler_state
{  
    Texture = (CubeMapTexture); 
    MipFilter = LINEAR; 
    MinFilter = LINEAR; 
    MagFilter = LINEAR; 
};

Texture BumpTexture;
sampler2D BumpMapSampler : TEXUNIT1 = sampler_state
{ Texture   = (BumpTexture); magfilter = LINEAR; minfilter = LINEAR; 
                             mipfilter = LINEAR; AddressU  = Wrap;
                             AddressV  = Wrap; AddressW  = Wrap;};
                             
//===========================================================================                             
                             

struct VS_INPUT
 {
     float4 Position            : POSITION0;
     float2 TextureCoords       : TEXCOORD0;     
     float3 Normal              : NORMAL0;    
     float3 Binormal            : BINORMAL0;
     float3 Tangent             : TANGENT0;
 };

struct VS_OUTPUT
{
    float4 Position             : POSITION0;
    float2 TextureCoords        : TEXCOORD0;
    float3 LightDirection       : TEXCOORD1;
    float3 ViewDirection        : TEXCOORD2;
    float3 Normal			    : TEXCOORD3;
    float3 Binormal			    : TEXCOORD4;
    float3 Tangent			    : TEXCOORD5;
};

VS_OUTPUT VertexShader( VS_INPUT input )
{
	VS_OUTPUT Output;
  
    float4 p = mul(input.Position, WorldMatrix);
    
    Output.LightDirection = -LightDirection;
    
    Output.Normal = normalize(mul(input.Normal , WorldMatrix));
    Output.Tangent = normalize(mul(input.Tangent, WorldMatrix));
    Output.Binormal = normalize(mul(input.Binormal, WorldMatrix));
    
    float4 worldSpacePos = mul(input.Position, WorldMatrix);
    float3 eyePosition = mul(-ViewMatrix._m30_m31_m32, transpose(ViewMatrix));
    Output.ViewDirection = worldSpacePos - eyePosition; 
    
    Output.Position = mul(p, mul(ViewMatrix, ProjectionMatrix));    // transform Position    
    Output.TextureCoords = input.TextureCoords.xy;
    Output.Position = p;
    Output.Position.w = length(mul(p, ViewMatrix));

    return Output;
} 

float4 PixelShader( VS_OUTPUT Input ) : COLOR
{        
	float3 TSBumpNormal = 2 * (tex2D(BumpMapSampler, Input.TextureCoords - 0.05 * Time) - 0.5); // fetch bump map
	float3 TSBumpNormal2 = 2 * (tex2D(BumpMapSampler, 0.01 * (Input.TextureCoords + 0.1 * Time)) - 0.5); // fetch bump map
	TSBumpNormal= normalize(TSBumpNormal + 2 * TSBumpNormal2);    

	//float3 E = normalize(CameraDirection.xyz - In.Position.xyz);    
	Input.ViewDirection = normalize(Input.ViewDirection);
    
	float3 WSBumpNormal = mul(TSBumpNormal, float3x3(Input.Tangent.xyz, Input.Binormal.xyz, Input.Normal.xyz));
 
	//Fresnel term for light reflection/refraction ratio
	float3 R = reflect(-Input.ViewDirection, WSBumpNormal);
	float4 Cube = texCUBE(CubeMapSampler, R);

	float fresnel =clamp(pow(pow(dot(R,WSBumpNormal),0.15),2.5),0,1);
              
	float4 color;
	color.rgb =  lerp(0.45 * Cube + Cube.a * 3 + sky_color * 0.6, water_color, fresnel); 
	color.a = 1.0 - fresnel;
	
	return color;
} 

technique Water
{
    pass P0
    {
        VertexShader = compile vs_1_1 VertexShader();
        PixelShader = compile ps_2_0 PixelShader();
    }
}