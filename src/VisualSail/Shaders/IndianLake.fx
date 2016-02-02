float4x4 ModelWorld : WORLD; //our world matrix
float4 time; //the time that we will pass to our shader for calculation of the normals

//application to vertex structure
struct a2v
{
     float4 position : POSITION0;
     float3 normal : NORMAL;
     float2 tex0 : TEXCOORD0;
     float3 tangent : TANGENT;
     float3 binormal : BINORMAL;
};

//vertex to pixel shader structure
struct v2p
{
     float4 position : POSITION0;
     float2 tex0 : TEXCOORD0;
     float2 tex1 : TEXCOORD1;
     float3 lightVec : TEXCOORD2;
     float3 eyeVec : TEXCOORD3;
     float att : TEXCOORD4;
};

//pixel shader to screen
struct p2f
{
     float4 color : COLOR0;
};

void vs( in a2v IN, out v2p OUT)
{
	OUT.position = IN.position;
}
void ps( in v2p IN, out p2f OUT)
{
	OUT.color = IN.att;
}
technique IndianLake
{
    pass p0
    {
        vertexshader = compile vs_1_1 vs();
        pixelshader = compile ps_2_0 ps();
    }
}