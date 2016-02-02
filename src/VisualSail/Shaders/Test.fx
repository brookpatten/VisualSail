float4x4 ModelViewProj : WORLDVIEWPROJ; //our world view projection matrix
float4 time;

//application to vertex structure
struct a2v
{
     float4 position : POSITION0;
};

//vertex to pixel shader structure
struct v2p
{
     float4 position : POSITION0;
};

//pixel shader to screen
struct p2f
{
     float4 color : COLOR0;
};

//VERTEX SHADER
void vs( in a2v IN, out v2p OUT )
{
    //getting to position to object space
    float angle=(time%360)*2;
    //OUT.position.z = sin( IN.position.x+angle);
	
	OUT.position = mul(OUT.position, ModelViewProj);
}

//PIXEL SHADER
void ps( in v2p IN, out p2f OUT )
{
    OUT.color.r=0.0;
    OUT.color.g=0.0;
    OUT.color.b=0.5;
    OUT.color.a=0.5;
}

technique water
{
    pass p0
    {
        vertexshader = compile vs_1_1 vs();
        pixelshader = compile ps_2_0 ps();
    }
}