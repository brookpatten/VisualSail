#include "../Framework2/Direct3D/Direct3DApp.h"

D3DVERTEXELEMENT9 vertexFormat[] = {
	{ 0, 0,            D3DDECLTYPE_FLOAT3, D3DDECLMETHOD_DEFAULT, D3DDECLUSAGE_POSITION, 0 },
	{ 0, sizeof(vec3), D3DDECLTYPE_FLOAT2, D3DDECLMETHOD_DEFAULT, D3DDECLUSAGE_TEXCOORD, 0 },
	D3DDECL_END()
};

#define RT_SIZE 128

class MainApp : public Direct3DApp {
public:
	void initMenu();
	void resetCamera();
	void setViewport(unsigned int w, unsigned int h);

	bool init();
	bool load();
	bool unload();

	void drawSkyBox();
	void drawDrop(float x, float y, float s);
	void drawWater();
	bool drawFrame();
protected:
	LPDIRECT3DVERTEXDECLARATION9 vDecl;

	TextureID rt0, rt1;
	TextureID environment;

	ShaderID drop, water, waterPhysics;

	bool autoDrip;
	bool clearRTs;
};

void MainApp::initMenu(){
	Menu *menu = menuSystem->getMainMenu();
	menu->addMenuItem("Auto drip: ", &autoDrip, INPUT_BOOL);

	App::initMenu();
}

void MainApp::resetCamera(){
	position = vec3(256, 50, -256);
	wx = 0.08f;
	wy = PI / 4;
	wz = 0;
}

void MainApp::setViewport(unsigned int w, unsigned int h){
	Direct3DApp::setViewport(w, h);
	clearRTs = true;
}

bool MainApp::init(){
	autoDrip = true;
	return true;
}

bool MainApp::load(){
	if (caps.PixelShaderVersion < D3DPS_VERSION(2,0) || caps.VertexShaderVersion < D3DVS_VERSION(1,1)){
		String error("This demo requires pixel shader 2.0 and vertex shader 1.1.\n"
					 "Your card/driver only supports pixel shader ");
		error.sprintf("%d.%d", (caps.PixelShaderVersion  >> 8) & 255, caps.PixelShaderVersion  & 255);
		error += " and vertex shader ";
		error.sprintf("%d.%d", (caps.VertexShaderVersion >> 8) & 255, caps.VertexShaderVersion & 255);

		addToLog(error);
		return false;
	}

	setDefaultFont("../Textures/Fonts/Future.font", "../Textures/Fonts/Future.dds");

	if ((drop  = renderer->addShader("drop.shd" )) == SHADER_NONE) return false;
	if ((water = renderer->addShader("water.shd")) == SHADER_NONE) return false;
	if ((waterPhysics = renderer->addShader("waterPhysics.shd")) == SHADER_NONE) return false;

	if ((rt0 = renderer->addRenderTexture(RT_SIZE, RT_SIZE, FORMAT_RGBA16, false)) == TEXTURE_NONE) return false;
	if ((rt1 = renderer->addRenderTexture(RT_SIZE, RT_SIZE, FORMAT_RGBA16, false)) == TEXTURE_NONE) return false;


	if ((environment = renderer->addCubemap(
		"../Textures/CubeMaps/Mountain2/negx.jpg", 
		"../Textures/CubeMaps/Mountain2/posx.jpg", 
		"../Textures/CubeMaps/Mountain2/negy.jpg", 
		"../Textures/CubeMaps/Mountain2/posy.jpg", 
		"../Textures/CubeMaps/Mountain2/negz.jpg", 
		"../Textures/CubeMaps/Mountain2/posz.jpg")) == TEXTURE_NONE) return false;

	dev->CreateVertexDeclaration(vertexFormat, &vDecl);

	clearRTs = true;

	return true;
}

bool MainApp::unload(){
	vDecl->Release();
	return true;
}

void MainApp::drawSkyBox(){
	renderer->setDepthFunc(DEPTH_NONE);
	renderer->setMask(COLOR);
	renderer->setTextures(environment);
	renderer->apply();

	struct Vertex {
		vec3 position;
		vec3 texCoord;
	};

	Vertex vertices[8];

	vertices[0].position = vec3(-5,  5,  5);
	vertices[1].position = vec3( 5,  5,  5);
	vertices[2].position = vec3(-5, -5,  5);
	vertices[3].position = vec3( 5, -5,  5);
	vertices[4].position = vec3(-5,  5, -5);
	vertices[5].position = vec3( 5,  5, -5);
	vertices[6].position = vec3(-5, -5, -5);
	vertices[7].position = vec3( 5, -5, -5);

	for (unsigned int i = 0; i < 8; i++){
		vertices[i].texCoord = vertices[i].position;
	}

	static short indices[] = {0,1,2,  1,3,2,  1,5,3,  5,7,3,  5,4,7,  4,6,7,  4,0,6,  0,2,6,  4,5,0,  5,1,0,  2,3,6,  3,7,6};

	dev->SetFVF(D3DFVF_XYZ | D3DFVF_TEXCOORDSIZE3(0) | (1 << D3DFVF_TEXCOUNT_SHIFT));
	dev->DrawIndexedPrimitiveUP(D3DPT_TRIANGLELIST, 0, sizeof(indices) / sizeof(short), 12, indices, D3DFMT_INDEX16, vertices, sizeof(Vertex));
}

void MainApp::drawDrop(float x, float y, float s){
	renderer->setShader(drop);
	renderer->apply();
	
	renderer->changeShaderConstant1f("strength", s);

	#define N 8
	vec3 verts[N];

	for (int i = 0; i < N; i++){
		float f = i * 2.0f * PI / N;
		verts[i] = vec3(x + 0.025f * cosf(f), y + 0.025f * sinf(f), 0);
	}

	dev->SetFVF(D3DFVF_XYZ);
	dev->DrawPrimitiveUP(D3DPT_TRIANGLEFAN, N-2, verts, sizeof(vec3));
}

void MainApp::drawWater(){
	struct Vertex {
		vec3 pos;
		vec2 texCoord;
	};
	Vertex verts[4];

	float off = 0.5f / RT_SIZE;

	static int currRT = 0;

	static float lastTime = 0;
	if (time - lastTime > 0.01f){
		lastTime = time - fmodf(time - lastTime, 0.01f);

		if (clearRTs){
			renderer->changeRenderTarget(rt0, RT_NONE);
			dev->Clear(0, NULL, D3DCLEAR_TARGET, D3DCOLOR_RGBA(127, 127, 127, 127), 1.0f, 0);

			renderer->changeRenderTarget(rt1, RT_NONE);
			dev->Clear(0, NULL, D3DCLEAR_TARGET, D3DCOLOR_RGBA(127, 127, 127, 127), 1.0f, 0);

			clearRTs = false;
			currRT = 1;
		} else {
			currRT = 1 - currRT;
			renderer->changeRenderTarget((currRT == 0)? rt0 : rt1, RT_NONE);
		}

		renderer->setShader(waterPhysics);
		renderer->setTextures((currRT == 0)? rt1 : rt0);
		renderer->apply();
		
		verts[0].pos = vec3(-1,  1, 0);
		verts[1].pos = vec3( 1,  1, 0);
		verts[2].pos = vec3( 1, -1, 0);
		verts[3].pos = vec3(-1, -1, 0);

		verts[0].texCoord = vec2(    off,     off);
		verts[1].texCoord = vec2(1 + off,     off);
		verts[2].texCoord = vec2(1 + off, 1 + off);
		verts[3].texCoord = vec2(    off, 1 + off);

		renderer->changeShaderConstant1f("off", off);
		//renderer->changeShaderConstant1f("off", 2 * off);

		dev->SetVertexDeclaration(vDecl);
		dev->DrawPrimitiveUP(D3DPT_TRIANGLEFAN, 2, verts, sizeof(Vertex));


		static float lastDropTime = 0;

		if (!captureMouse && rButton){
			float winXn = 2 * float(mouseX) / float(width)  - 1;
			float winYn = 2 * float(mouseY) / float(height) - 1;

			vec4 vScr(winXn, -winYn, 0, 1);

			mat4 invMvp = !(projection * modelView);
			vec4 unProj = invMvp * vScr;

			vec3 dir = normalize((unProj.xyz() / unProj.w) - position);
			vec3 p = position - (position.y / dir.y) * dir;

			drawDrop(p.x / 256.0f, -p.z / 256.0f, 0.3f);
		}
		
		if (autoDrip && (time - lastDropTime > 0.3f)){
			lastDropTime = time - fmodf(time - lastDropTime, 0.3f);

			float x = float(rand()) / RAND_MAX * 2 - 1;
			float y = float(rand()) / RAND_MAX * 2 - 1;
			float s = float(rand()) / RAND_MAX;

			drawDrop(x, y, s);
		}

		renderer->changeToDefaultRenderTarget();
	}

	renderer->setShader(water);
	renderer->setTextures((currRT == 0)? rt0 : rt1, environment);
	renderer->apply();

	renderer->changeShaderConstant4x4f("view_proj_matrix", projection * modelView);
	//renderer->changeShaderConstant1f("off", 2 * off);
	renderer->changeShaderConstant3f("camPos", position);

	verts[0].pos = vec3(-256, 0,  256);
	verts[1].pos = vec3( 256, 0,  256);
	verts[2].pos = vec3( 256, 0, -256);
	verts[3].pos = vec3(-256, 0, -256);

	verts[0].texCoord = vec2(0, 1);
	verts[1].texCoord = vec2(1, 1);
	verts[2].texCoord = vec2(1, 0);
	verts[3].texCoord = vec2(0, 0);

	dev->SetVertexDeclaration(vDecl);
	dev->DrawPrimitiveUP(D3DPT_TRIANGLEFAN, 2, verts, sizeof(Vertex));
}

bool MainApp::drawFrame(){
    dev->Clear(0, NULL, /*D3DCLEAR_TARGET | */D3DCLEAR_ZBUFFER, D3DCOLOR_RGBA(0, 0, 0, 0), 1.0f, 0);

	projection = projectionMatrixX(1.5f, float(height) / float(width), 1, 3000);
	dev->SetTransform(D3DTS_PROJECTION, (D3DMATRIX *) (const float *) transpose(projection));

	modelView = rotateZXY(-wx, -wy, -wz);
	dev->SetTransform(D3DTS_VIEW, (D3DMATRIX *) (const float *) transpose(modelView));
	drawSkyBox();

	modelView.translate(-position);
	dev->SetTransform(D3DTS_VIEW, (D3DMATRIX *) (const float *) transpose(modelView));

	drawWater();

	return true;
}

App *app = new MainApp();
