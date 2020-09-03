#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;

#include "../SoyuzKernel/include/Core.h"

#include "SKWObject.h"

namespace SKW
{
	public ref class Core : public SKWObject<SK::Core>
	{
	public:

		//Constructor
		Core();

		//Destructor and Finalizer
		~Core();
		!Core();

		//Init
		void Init();

		//Create FrameBuffer
		int CreateFrameBuffer();

		//Create Shader
		int CreateShader();

		//Create Model
		int CreateModel();

		//Create Texture
		int CreateTexture();

		//Create Font
		int CreateFont();

		//Create Mesh
		int CreateMesh();

		//Add Mesh to Model
		void AddMeshToModel(int ModelID, int MeshID);

		//Prepare Memory of Mesh
		void MeshPrepareMemory(int MeshID, unsigned int nVertex, unsigned int nIndex);

		//Add Vertex to a Mesh
		void MeshAddVertex(int MeshID, float x, float y, float z, float nX, float nY, float nZ, float uvX, float uvY);

		//Add Index to a Mesh
		void MeshAddIndex(int MeshID, int i);

		//Compile a Mesh
		void MeshCompile(int MeshID);

		//Set Model Position
		void SetModelPosition(int ModelID, float x, float y, float z);

		//Set Model Rotation
		void SetModelRotation(int ModelID, float x, float y, float z);

		//Set Model Scale
		void SetModelScale(int ModelID, float x, float y, float z);

		//Set Mesh Draw Mode
		void SetMeshDrawMode(int MeshID, int DrawMode);

		//Load Model
		void LoadModel(int ModelID, String^ Path);

		//Set Perspective Camera
		void SetPerspectiveCamera(float x, float y, float z, float targetX, float targetY, float targetZ, float radius, float near, float far);

		//Set Orthographic Camera
		void SetOrthographicCamera(float near, float far);

		//Set Orthographic Box Camera
		void SetOrthographicBoxCamera(float x, float y, float z, float targetX, float targetY, float targetZ, float xMin, float xMax, float yMin, float yMax, float zMin, float zMax);

		//Set Shader
		void SetShader(int ShaderID, String^ vertex, String^ fragment);
		void SetShader(int ShaderID, String^ vertex, String^ geometry, String^ fragment);

		//Set Prefab Shader
		void SetPrefabShader(int ShaderID, int PrefabID);

		//Set Uniform Int
		void SetUniformI(int ShaderID, String^ Name, int Value);

		//Set Uniform Float
		void SetUniformF(int ShaderID, String^ Name, float Value);

		//Set Uniform Vec2
		void SetUniformVec2(int ShaderID, String^ Name, float X, float Y);

		//Set Uniform Vec3
		void SetUniformVec3(int ShaderID, String^ Name, float X, float Y, float Z);

		//Set Uniform Vec4
		void SetUniformVec4(int ShaderID, String^ Name, float X, float Y, float Z, float W);

		//Set Uniform Mat3x3
		void SetUniformMat3(int ShaderID, String^ Name, float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33);

		//Set Uniform FrameBuffer
		void SetUniformFrameBuffer(int ShaderID, String^ Name, int FrameBufferID, int TextureID);

		//Set Uniform FrameBuffer with Depth ?
		void SetUniformFrameBuffer(int ShaderID, String^ Name, int FrameBufferID, int TextureID, bool UseDepth);

		//Set Uniform Texture
		void SetUniformTexture(int ShaderID, String^ Name, int TextureID, int TextureIndex);

		//Set Uniform Font
		void SetUniformFont(int ShaderID, String^ Name, int FontID, int TextureIndex);

		//Set Texture with Data Array
		void SetTextureWithDataArray(int TextureID, int Width, int Height, int NumberOfChannel, array<float>^ Data);

		//Set Texture with Source Path
		void SetTextureWithSourcePath(int TextureID, String^ Path, unsigned int NumberOfChannel);

		//Set Texture with a filling value
		void SetTextureFilled(int TextureID, int Width, int Height, int NumberOfChannel, float Value);

		//Get Texture Width
		int GetTextureWidth(int TextureID);

		//Get Texture Height
		int GetTextureHeight(int TextureID);

		//Get Texture Number Of Channel
		int GetTextureNumberOfChannel(int TextureID);

		//Get Texture Data
		array<float>^ GetTextureData(int TextureID);

		//Save Texture to PNG
		bool SaveTexturePNG(int TextureID, String^ FilePath);

		//Convolution on texture
		void TextureConv(int textureID, unsigned int size, array<float>^ matrix, float coef);

		//Set Texture Pixel Value
		void SetTexturePixel(int textureID, float x, float y, unsigned int channel, float value);

		//Get Texture Pixel Value
		float GetTexturePixel(int textureID, float x, float y, unsigned int channel);

		//Update Texture
		void UpdateTexture(int TextureID);

		//Apply Texture Transform
		void TextureTransform(int TextureID, int TransformID, array<float>^ Arguments);

		//Set Texture As Cubemap
		void SetTextureAsCubemap(int TextureID, String^ Right, String^ Left, String^ Top, String^ Bottom, String^ Front, String^ Back);

		//Load Font
		void LoadFont(int FontID, String^ Path, unsigned int Size, unsigned int Start, unsigned int End);
		
		//Add Text as Mesh to the given Model
		void AddTextAsMesh(int FontID, int ModelID, String^ Text, float X, float Y, float MaxWidth, float LineSpacing);

		//Use Framebuffer
		void UseFramebuffer(int FrameBufferID);

		//Use Shader
		void UseShader(int ShaderID);

		//Render Model
		void RenderModel(int ShaderID, int ModelID);

		//Render FrameBuffer Init
		void RenderFrameBufferInit(int FrameBufferID, int ShaderID);

		//Render FrameBuffer
		void RenderFrameBuffer(int ShaderID);

		//Show FrameBuffer
		void ShowFrameBuffer(int FrameBufferID);

		//Render Skybox
		void RenderSkybox(int FrameBufferID);

		//Set Skybox
		void SetSkybox(int FrameBufferID, int CubemapTextureID);

		//Disable Skybox
		void DisableSkybox(int FrameBufferID);

		//Set Clear Color
		void SetClearColor(float r, float g, float b, float a);

		//Get Mouse X
		double GetMouseX();

		//Get Mouse Y
		double GetMouseY();

		//Is Key Pressed
		bool IsKeyPressed(const char key);

		//Is Mouse Clicked
		bool IsMouseClicked(int button);

		//Is Mouse Released
		bool IsMouseReleased(int button);

		//Mouse To World Coordinate
		void UpdateMouseToWorld();
		float GetMouseToWorldX();
		float GetMouseToWorldY();
		float GetMouseToWorldZ();

		//Set Window Title
		void SetWindowTitle(String^ Title);

		//Delete Framebuffer
		void DeleteFrameBuffer(int FramebufferID);

		//Delete Shader
		void DeleteShader(int ShaderID);

		//Delete Model
		void DeleteModel(int ModelID);

		//Delete Mesh
		void DeleteMesh(int MeshID);

		//Delete Texture
		void DeleteTexture(int TextureID);

		//Delete Font
		void DeleteFont(int FontID);

		//Update
		//If true : it's time to end core
		bool Update();

		//Static Variable
		const int Prefab_Shader_Color		= PREFAB_SHADER_COLOR;
		const int Prefab_Shader_Normal		= PREFAB_SHADER_NORMAL;
		const int Prefab_Shader_Mix			= PREFAB_SHADER_MIX;
		const int Prefab_Shader_Conv		= PREFAB_SHADER_CONV;
		const int Prefab_Shader_Lighting	= PREFAB_SHADER_LIGHTING;
		const int Prefab_Shader_Font		= PREFAB_SHADER_FONT;
		const int Prefab_Shader_Gui			= PREFAB_SHADER_GUI;
		const int Prefab_Shader_Reverse		= PREFAB_SHADER_REVERSE;
		const int Prefab_Shader_Reflect		= PREFAB_SHADER_REFLECT;
		const int Prefab_Shader_Refract		 = PREFAB_SHADER_REFRACT;

		const int TexTransform_Perlin = TEXTF_PERLIN;
		const int TexTransform_Border = TEXTF_BORDER;

		const int Model_DrawMode_Triangles	= MODEL_DRAW_TRIANGLES;
		const int Model_DrawMode_Lines		= MODEL_DRAW_LINES;
		const int Model_DrawMode_LineStrip	= MODEL_DRAW_LINE_STRIP;
		const int Model_DrawMode_Points		= MODEL_DRAW_POINTS;

	};
}
