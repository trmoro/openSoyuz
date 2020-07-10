#include "pch.h"
#include "Core.h"

namespace SKW
{
	//Constructor
	Core::Core() : SKWObject(new SK::Core() )
	{

	}

	//Destructor
	Core::~Core()
	{
		if (m_Instance != nullptr)
			delete m_Instance;
	}

	//Finalizer
	Core::!Core()
	{
		if (m_Instance != nullptr)
			delete m_Instance;
	}

	//Init
	void Core::Init()
	{
		m_Instance->init();
	}

	//Create FrameBuffer
	int Core::CreateFrameBuffer()
	{
		return m_Instance->createFrameBuffer();
	}

	//Create Shader
	int Core::CreateShader()
	{
		return m_Instance->createShader();
	}

	//Create Model
	int Core::CreateModel()
	{
		return m_Instance->createModel();
	}

	//Create Texture
	int Core::CreateTexture()
	{
		return m_Instance->createTexture();
	}

	//Create Font
	int Core::CreateFont()
	{
		return m_Instance->createFont();
	}

	//Add Mesh To Model
	void Core::AddMeshToModel(int ModelID, int MeshID)
	{
		m_Instance->addMeshToModel(ModelID, MeshID);
	}

	//Create Mesh
	int Core::CreateMesh()
	{
		return m_Instance->createMesh();
	}

	//Prepare Memory of Mesh
	void Core::MeshPrepareMemory(int MeshID, unsigned int nVertex, unsigned int nIndex)
	{
		m_Instance->meshPrepareMemory(MeshID, nVertex, nIndex);
	}

	//Add Vertex to a Mesh
	void Core::MeshAddVertex(int MeshID, float x, float y, float z, float nX, float nY, float nZ, float uvX, float uvY)
	{
		m_Instance->meshAddVertex(MeshID,x,y,z,nX,nY,nZ,uvX,uvY);
	}

	//Add Index to a Mesh
	void Core::MeshAddIndex(int MeshID, int i)
	{
		m_Instance->meshAddIndex(MeshID,i);
	}

	//Compile a Mesh
	void Core::MeshCompile(int MeshID)
	{
		m_Instance->meshCompile(MeshID);
	}

	//Set Model Position
	void Core::SetModelPosition(int ModelID, float x, float y, float z)
	{
		m_Instance->setModelPosition(ModelID,x,y,z);
	}

	//Set Model Rotation
	void Core::SetModelRotation(int ModelID, float x, float y, float z)
	{
		m_Instance->setModelRotation(ModelID, x, y, z);
	}

	//Set Model Scale
	void Core::SetModelScale(int ModelID, float x, float y, float z)
	{
		m_Instance->setModelScale(ModelID, x, y, z);
	}

	//Set Mesh Draw Mode
	void Core::SetMeshDrawMode(int MeshID, int DrawMode)
	{
		m_Instance->setMeshDrawMode(MeshID, DrawMode);
	}

	//Load Model
	void Core::LoadModel(int ModelID, String^ Path)
	{
		m_Instance->loadModel(ModelID, stringToCharArray(Path));
	}

	//Set Perspective Camera
	void Core::SetPerspectiveCamera(float x, float y, float z, float targetX, float targetY, float targetZ, float radius, float near, float far)
	{
		m_Instance->setPerspectiveCamera(x,y,z,targetX,targetY,targetZ,radius,near,far);
	}

	//Set Orthographic Camera
	void Core::SetOrthographicCamera(float near, float far)
	{
		m_Instance->setOrthographicCamera(near,far);
	}

	//Set Orthographic Box Camera
	void Core::SetOrthographicBoxCamera(float x, float y, float z, float targetX, float targetY, float targetZ, float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
	{
		m_Instance->setOrthographicBoxCamera(x,y,z,targetX,targetY,targetZ,xMin,xMax,yMin,yMax,zMin,zMax);
	}

	//Set Shader
	void Core::SetShader(int ShaderID, String^ vertex, String^ fragment)
	{
		m_Instance->setShader(ShaderID,stringToCharArray(vertex), stringToCharArray(fragment) );
	}

	//Set Shader with Geometry
	void Core::SetShader(int ShaderID, String^ vertex, String^ geometry, String^ fragment)
	{
		m_Instance->setShader(ShaderID, stringToCharArray(vertex), stringToCharArray(geometry), stringToCharArray(fragment));
	}

	//Set Prefab Shader
	void Core::SetPrefabShader(int ShaderID, int PrefabID)
	{
		m_Instance->setPrefabShader(ShaderID,PrefabID);
	}

	//Set Uniform I
	void Core::SetUniformI(int ShaderID, String^ Name, int Value)
	{
		m_Instance->setUniformi(ShaderID,stringToCharArray(Name), Value);
	}

	//Set Uniform Float
	void Core::SetUniformF(int ShaderID, String^ Name, float Value)
	{
		m_Instance->setUniformf(ShaderID, stringToCharArray(Name), Value);
	}

	//Set Uniform Vec2
	void Core::SetUniformVec2(int ShaderID, String^ Name, float X, float Y)
	{
		m_Instance->setUniformVec2(ShaderID, stringToCharArray(Name), X, Y);
	}

	//Set Uniform Vec3
	void Core::SetUniformVec3(int ShaderID, String^ Name, float X, float Y, float Z)
	{
		m_Instance->setUniformVec3(ShaderID, stringToCharArray(Name), X, Y, Z);
	}

	//Set Uniform Vec4
	void Core::SetUniformVec4(int ShaderID, String^ Name, float X, float Y, float Z, float W)
	{
		m_Instance->setUniformVec4(ShaderID, stringToCharArray(Name), X, Y, Z, W);
	}

	//Set Uniform Matrix3x3
	void Core::SetUniformMat3(int ShaderID, String^ Name, float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33)
	{
		m_Instance->setUniformMat3(ShaderID, stringToCharArray(Name), m11, m12, m13, m21, m22, m23, m31, m32, m33);
	}

	//Set Uniform FrameBuffer
	void Core::SetUniformFrameBuffer(int ShaderID, String^ Name, int FrameBufferID, int TextureID)
	{
		m_Instance->setUniformFrameBuffer(ShaderID, stringToCharArray(Name), FrameBufferID, false, TextureID);
	}

	//Set Uniform FrameBuffer
	void Core::SetUniformFrameBuffer(int ShaderID, String^ Name, int FrameBufferID, int TextureID, bool UseDepth)
	{
		m_Instance->setUniformFrameBuffer(ShaderID, stringToCharArray(Name), FrameBufferID, UseDepth, TextureID);
	}

	//Set Uniform Texture
	void Core::SetUniformTexture(int ShaderID, String^ Name, int TextureID, int TextureIndex)
	{
		m_Instance->setUniformTexture(ShaderID, stringToCharArray(Name), TextureID, TextureIndex);
	}

	//Set Uniform Font
	void Core::SetUniformFont(int ShaderID, String^ Name, int FontID, int TextureIndex)
	{
		m_Instance->setUniformFont(ShaderID, stringToCharArray(Name), FontID, TextureIndex);
	}

	//Set Texture with Data Array
	void Core::SetTextureWithDataArray(int TextureID, int Width, int Height, int NumberOfChannel, array<float>^ Data)
	{
		pin_ptr<float> arrayPin = &Data[0];
		m_Instance->setTextureWithDataArray(TextureID, Width, Height, NumberOfChannel, arrayPin);
	}

	//Set Texture With Source Path
	void Core::SetTextureWithSourcePath(int TextureID, String^ Path, unsigned int NumberOfChannel)
	{
		m_Instance->setTextureWithSourcePath(TextureID, stringToCharArray(Path), NumberOfChannel);
	}

	//Set Texture filled with a value
	void Core::SetTextureFilled(int TextureID, int Width, int Height, int NumberOfChannel, float Value)
	{
		m_Instance->setTextureFilled(TextureID,Width,Height,NumberOfChannel,Value);
	}

	//Get Texture Width
	int Core::GetTextureWidth(int TextureID)
	{
		return m_Instance->getTextureWidth(TextureID);
	}

	//Get Texture Height
	int Core::GetTextureHeight(int TextureID)
	{
		return m_Instance->getTextureHeight(TextureID);
	}

	//Get Texture Number Of Channel
	int Core::GetTextureNumberOfChannel(int TextureID)
	{
		return m_Instance->getTextureNumberOfChannel(TextureID);
	}


	//Get Texture Data
	array<float>^ Core::GetTextureData(int TextureID)
	{
		float* dataArray = m_Instance->getTextureData(TextureID);
		
		unsigned int w = m_Instance->getTextureWidth(TextureID);
		unsigned int h = m_Instance->getTextureHeight(TextureID);
		unsigned int c = m_Instance->getTextureNumberOfChannel(TextureID);
		unsigned int size = w * h * c;
		array<float>^ copy = gcnew array<float>(size);

		Marshal::Copy(IntPtr(dataArray), copy, 0, size);

		return copy;
	}

	//Save Texture to PNG
	bool Core::SaveTexturePNG(int TextureID, String^ FilePath)
	{
		return m_Instance->saveTexturePNG(TextureID, stringToCharArray(FilePath) );
	}

	//Texture Convolution
	void Core::TextureConv(int textureID, unsigned int size, array<float>^ matrix, float coef)
	{
		pin_ptr<float> ptr_matrix = &matrix[0];
		m_Instance->textureConv(textureID, size, ptr_matrix, coef);
	}

	//Set Texture Pixel
	void Core::SetTexturePixel(int textureID, float x, float y, unsigned int channel, float value)
	{
		m_Instance->textureSetPixel(textureID,x,y,channel,value);
	}

	//Get Texture Pixel
	float Core::GetTexturePixel(int textureID, float x, float y, unsigned int channel)
	{
		return m_Instance->textureGetPixel(textureID,x,y,channel);
	}

	//Update Texture
	void Core::UpdateTexture(int TextureID)
	{
		m_Instance->updateTexture(TextureID);
	}

	//Apply Texture Transform
	void Core::TextureTransform(int TextureID, int TransformID, array<float>^ Arguments)
	{
		pin_ptr<float> args = &Arguments[0];
		m_Instance->textureTransform(TextureID, TransformID, args);
	}

	//Set Texture as Cubemap
	void Core::SetTextureAsCubemap(int TextureID, String^ Right, String^ Left, String^ Top, String^ Bottom, String^ Front, String^ Back)
	{
		m_Instance->setTextureAsCubemap(TextureID, stringToCharArray(Right), stringToCharArray(Left), stringToCharArray(Top), stringToCharArray(Bottom), stringToCharArray(Front), stringToCharArray(Back) );
	}

	//Load Font
	void Core::LoadFont(int FontID, String^ Path, unsigned int Size, unsigned int Start, unsigned int End)
	{
		m_Instance->loadFont(FontID, stringToCharArray(Path), Size, Start, End);
	}

	//Add Text as Mesh to the given Model
	void Core::AddTextAsMesh(int FontID, int ModelID, String^ Text, float X, float Y, float MaxWidth, float LineSpacing)
	{
		m_Instance->addTextAsMesh(FontID, ModelID, stringToCharArray(Text), X, Y, MaxWidth, LineSpacing);
	}

	//Use Framebuffer
	void Core::UseFramebuffer(int FrameBufferID)
	{
		m_Instance->useFrameBuffer(FrameBufferID);
	}

	//Use Shader
	void Core::UseShader(int ShaderID)
	{
		m_Instance->useShader(ShaderID);
	}

	//Render Model
	void Core::RenderModel(int ShaderID, int ModelID)
	{
		m_Instance->renderModel(ShaderID, ModelID);
	}

	//Render FrameBuffer Init
	void Core::RenderFrameBufferInit(int FrameBufferID, int ShaderID)
	{
		m_Instance->renderFrameBufferInit(FrameBufferID, ShaderID);
	}

	//Render FrameBuffer
	void Core::RenderFrameBuffer(int ShaderID)
	{
		m_Instance->renderFrameBuffer(ShaderID);
	}

	//Show FrameBuffer
	void Core::ShowFrameBuffer(int FrameBufferID)
	{
		m_Instance->showFrameBuffer(FrameBufferID);
	}

	//Render Skybox
	void Core::RenderSkybox(int FrameBufferID)
	{
		m_Instance->renderSkybox(FrameBufferID);
	}

	//Set Skybox
	void Core::SetSkybox(int FrameBufferID, int CubemapTextureID)
	{
		m_Instance->setSkybox(FrameBufferID,CubemapTextureID);
	}

	//Disable Skybox
	void Core::DisableSkybox(int FrameBufferID)
	{
		m_Instance->disableSkybox(FrameBufferID);
	}

	//Set Clear Color
	void Core::SetClearColor(float r, float g, float b, float a)
	{
		m_Instance->setClearColor(r,g,b,a);
	}

	//Get Mouse X
	double Core::GetMouseX()
	{
		return m_Instance->getMouseX();
	}

	//Get Mouse Y
	double Core::GetMouseY()
	{
		return m_Instance->getMouseY();
	}
	
	//Is Key Pressed
	bool Core::IsKeyPressed(const char key)
	{
		return m_Instance->isKeyPressed(key);
	}

	//Is Mouse Clicked
	bool Core::IsMouseClicked(int button)
	{
		return m_Instance->isMouseClicked(button);
	}

	//Is Mouse Released
	bool Core::IsMouseReleased(int button)
	{
		return m_Instance->isMouseReleased(button);
	}

	//Update Mouse To World Coordinate
	void Core::UpdateMouseToWorld()
	{
		m_Instance->mouseToWorld();
	}

	//Get X Mouse To World
	float Core::GetMouseToWorldX()
	{
		return m_Instance->getMouseToWorldX();
	}

	//Get Y Mouse To World
	float Core::GetMouseToWorldY()
	{
		return m_Instance->getMouseToWorldY();
	}

	//Get Z Mouse To World
	float Core::GetMouseToWorldZ()
	{
		return m_Instance->getMouseToWorldZ();
	}

	//Set Window Title
	void Core::SetWindowTitle(String^ Title)
	{
		m_Instance->setWindowTitle(stringToCharArray(Title));
	}

	//Delete FrameBuffer
	void Core::DeleteFrameBuffer(int FramebufferID)
	{
		m_Instance->deleteFrameBuffer(FramebufferID);
	}

	//Delete Shader
	void Core::DeleteShader(int ShaderID)
	{
		m_Instance->deleteShader(ShaderID);
	}

	//Delete Model
	void Core::DeleteModel(int ModelID)
	{
		m_Instance->deleteModel(ModelID);
	}

	//Delete Mesh
	void Core::DeleteMesh(int MeshID)
	{
		m_Instance->deleteMesh(MeshID);
	}

	//Delete Texture
	void Core::DeleteTexture(int TextureID)
	{
		m_Instance->deleteTexture(TextureID);
	}

	//Delete Font
	void Core::DeleteFont(int FontID)
	{
		m_Instance->deleteFont(FontID);
	}

	//Update
	//If true : it's time to end core
	bool Core::Update()
	{
		return m_Instance->update();
	}
}