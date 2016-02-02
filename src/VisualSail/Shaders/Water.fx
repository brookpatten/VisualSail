float4x4 ModelViewProj : WORLDVIEWPROJ; //our world view projection matrix
float4x4 ModelViewIT : WORLDVIEWIT; //our inverse transpose matrix
float4x4 ModelWorld : WORLD; //our world matrix
float4 lightPos; //our light position in object space
float4 time; //the time that we will pass to our shader for calculation of the normals

texture texture0; //our texture
texture texture1; //our normal map

sampler2D texSampler0 : TEXUNIT0 = sampler_state
{
     Texture = (texture0);
    MIPFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MINFILTER = LINEAR;
};
sampler2D texSampler1 : TEXUNIT1 = sampler_state
{
    Texture = (texture1);
   MIPFILTER = LINEAR;
   MAGFILTER = LINEAR;
   MINFILTER = LINEAR;
};

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

//VERTEX SHADER
void vs( in a2v IN, out v2p OUT )
{
     //getting to position to object space
    OUT.position = mul(IN.position, ModelViewProj);

    //getting the position of the vertex in the world
    float4 posWorld = mul(IN.position, ModelWorld);
    
    float3 eye = normalize(IN.normal);

    //getting vertex -> light vector
    float3 light = normalize(lightPos - posWorld);

    //calculating the binormal and setting the Tangent Binormal and Normal matrix
    float3x3 TBNMatrix = float3x3(IN.tangent, IN.binormal , IN.normal);

    //Passing the eye vector to the pixel shader for use with reflection
    OUT.eyeVec = normalize(mul(TBNMatrix, eye));
    //setting the lightVector
    OUT.lightVec = mul(TBNMatrix, light);

    //calculate the attenuation
    OUT.att = 1/( 1 + ( 0.005 * distance(lightPos.xyz, posWorld) ) );
 
    OUT.tex0 = IN.tex0;
    OUT.tex1 = IN.tex0;
}

//PIXEL SHADER
void ps( in v2p IN, out p2f OUT )
{
    //Calculating 3 sets of normals that will scroll over the surface.
    float3 normal = 2.0f * tex2D(texSampler0, (0.5 * IN.tex0) + (time * 0.5)).rgb - 1.0f; //the normal map
    float3 normal1 = 2.0f * tex2D(texSampler0, (1 * IN.tex0) + (time * 1)).rgb - 1.0f; //the normal2nd map
    float3 normal2 = 2.0f * tex2D(texSampler0, (2 * IN.tex0) + (time * 2)).rgb - 1.0f; //the normal3rd map

    //our eye vector normalized
    float3 eye = normalize(IN.eyeVec);

    //calculating the final normal from all the normals in the above equations.
    //This will be used to batch the diffuse calculation and get us a light intensity.
    float3 finalNormal = normalize(normal + normal1 * 2 + normal2 * 4);

    //getting the reflection vector and a surface to eye vector
    float3 reflectVector = reflect(eye, finalNormal);

     //calculate the color from the cubemap
    float4 color = texCUBE(texSampler1, reflectVector);

    //normalize the light
    float3 light = normalize(IN.lightVec);

    //set the output color
    float diffuse = saturate(dot(finalNormal, light));

    //multiply the attenuation with the color
    OUT.color = IN.att * color * diffuse;
}

technique water
{
    pass p0
    {
        vertexshader = compile vs_1_1 vs();
        pixelshader = compile ps_2_0 ps();
    }
}