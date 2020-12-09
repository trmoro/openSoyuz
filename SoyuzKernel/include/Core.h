#pragma once

//GLEW
#define GLEW_STATIC
#include <GL/glew.h>

//Vector
#include <vector>

//GLFW
#include "GLFW/glfw3.h"

//GLM
#include "glm/glm.hpp"
#include "glm/mat4x4.hpp"
#include "glm/gtc/matrix_transform.hpp"

//Log
#include "Log.h"

//Model
#include "Model.h"

//Mesh
#include "Mesh.h"

//Font
#include "Font.h"

//Input
#include "Input.h"

//FrameBuffer
#include "FrameBuffer.h"

//Shader
#include "Shader.h"

//Model
#include "Model.h"

//Texture
#include "Texture.h"

//Texture Transform
#define TEXTF_PERLIN		0
#define TEXTF_BORDER		1
#define TEXTF_CURVE			2
#define TEXTF_RESCALE		3
#define TEXTF_ZONERESCALE	4

//MSAA
#define MSAA 4

//Shader Scripts
#include "ShaderScript.h"
#define PREFAB_SHADER_COLOR		0
#define PREFAB_SHADER_NORMAL	1
#define PREFAB_SHADER_MIX		2
#define PREFAB_SHADER_CONV		3
#define PREFAB_SHADER_LIGHTING	4
#define PREFAB_SHADER_FONT		5
#define PREFAB_SHADER_GUI		6
#define PREFAB_SHADER_REVERSE	7
#define PREFAB_SHADER_SKYBOX	8
#define PREFAB_SHADER_REFLECT	9
#define PREFAB_SHADER_REFRACT	10

//Model Drawing Mode
#define MODEL_DRAW_TRIANGLES	0
#define MODEL_DRAW_QUADS		1
#define MODEL_DRAW_LINES		2
#define MODEL_DRAW_LINE_STRIP	3
#define MODEL_DRAW_POINTS		4

namespace SK
{

	class Core
	{
	public:

		//Constructor / Destructor
		Core();
		~Core();

		//Init
		void init(const char* window_title);

		//Update
		bool update();

		//Create Framebuffer
		int createFrameBuffer();

		//Create Shader
		int createShader();

		//Create Model
		int createModel();

		//Create Texture
		int createTexture();

		//Create Font
		int createFont();

		//Create Mesh
		int createMesh();

		//Add Mesh
		int addMesh(Mesh* mesh);

		//Add Mesh to Model
		void addMeshToModel(int modelID, int meshID);

		//Prepare Memory
		void meshPrepareMemory(int meshID, unsigned int nVertex, unsigned int nIndex);

		//Add Vertex
		void meshAddVertex(int meshID, float x, float y, float z, float nX, float nY, float nZ, float uvX, float uvY);

		//Add Index
		void meshAddIndex(int meshID, int i);

		//Compile
		void meshCompile(int meshID);

		//Set Model Position
		void setModelPosition(int modelID, float x, float y, float z);

		//Set Model Rotation
		void setModelRotation(int modelID, float x, float y, float z);

		//Set Model Scale
		void setModelScale(int modelID, float x, float y, float z);

		//Load Model
		void loadModel(int modelID, const char* path);

		//Set Mesh Draw Mode
		void setMeshDrawMode(int meshID, unsigned int drawmode);

		//Set Perspective Camera
		void setPerspectiveCamera(float x, float y, float z, float targetX, float targetY, float targetZ, float radius, float near, float far);

		//Set Orthographic Camera
		void setOrthographicCamera(float near, float far);

		//Set Orthographic Box Camera
		void setOrthographicBoxCamera(float x, float y, float z, float targetX, float targetY, float targetZ, float xMin, float xMax, float yMin, float yMax, float zMin, float zMax);

		//Set Shader
		void setShader(int shaderID, const char* vertex, const char* fragment);

		//Set Shader with geometry
		void setShader(int shaderID, const char* vertex, const char* geometry, const char* fragment);

		//Set Shader with Prefab Shader Script
		void setPrefabShader(int shaderID, int prefabID);

		//Set Uniform FrameBuffer
		void setUniformFrameBuffer(int shaderID, const char* name, int frameBufferID, bool depth, int textureId);

		//Set Uniform Int
		void setUniformi(int shaderID, const char* name, int value);

		//Set Uniform Float
		void setUniformf(int shaderID, const char* name, float value);

		//Set Uniform Vec2
		void setUniformVec2(int shaderID, const char* name, float x, float y);

		//Set Uniform Vec3
		void setUniformVec3(int shaderID, const char* name, float x, float y, float z);

		//Set Uniform Vec4
		void setUniformVec4(int shaderID, const char* name, float x, float y, float z, float w);

		//Set Uniform Mat3x3
		void setUniformMat3(int shaderID, const char* name, float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33);

		//Set Uniform Texture
		void setUniformTexture(int shaderID, const char* name, int textureID, int textureIndex);

		//Set Uniform Font
		void setUniformFont(int shaderID, const char* name, int fontID, int textureIndex);

		//Set Texture with data array
		void setTextureWithDataArray(int textureID, unsigned int width, unsigned int height, unsigned int nChannel, float* data);

		//Set Texture with source path
		void setTextureWithSourcePath(int textureID, const char* path, unsigned int nChannel);

		//Set Texture with filling
		void setTextureFilled(int textureID, unsigned int width, unsigned int height, unsigned int nChannel, float value);

		//Set Texture as Cubemap
		void setTextureAsCubemap(int textureID, const char* right, const char* left, const char* top, const char* bottom, const char* front, const char* back);

		//Get Texture Width
		unsigned int getTextureWidth(int textureID);

		//Get Texture Height
		unsigned int getTextureHeight(int textureID);

		//Get Texture Number Of Channel
		unsigned int getTextureNumberOfChannel(int textureID);

		//Get Texture Data
		float* getTextureData(int textureID);

		//Save Texture to PNG
		bool saveTexturePNG(int textureID, std::string filePath);

		//Convolution on texture
		void textureConv(int textureID, unsigned int size, float* matrix, float coef);

		//Sub-Convolution on texture
		void textureSubConv(int textureID, unsigned int size, float* matrix, float coef, unsigned int startX, unsigned int endX, unsigned int startY, unsigned int endY);

		//Set Texture Pixel Value
		void textureSetPixel(int textureID, float x, float y, unsigned int channel, float value);

		//Get Texture Pixel Value
		float textureGetPixel(int textureID, float x, float y, unsigned int channel);

		//Update Texture
		void updateTexture(int textureID);

		//Texture Transform
		void textureTransform(int textureID, unsigned int transformID, float* args);

		//Load Font
		void loadFont(int fontID, std::string path, unsigned int size, unsigned int start, unsigned int end);

		//Add Text as Mesh to the given Model
		void addTextAsMesh(int fontID, int modelID, std::string text, float x, float y, float max_width, float lineSpacing);

		//Render FrameBuffer Init
		void renderFrameBufferInit(int frameBufferID, int shaderID);

		//Render FrameBuffer
		void renderFrameBuffer(int shaderID);

		//Show FrameBuffer
		void showFrameBuffer(int frameBufferID);

		//Use Framebuffer
		void useFrameBuffer(int frameBufferID);

		//Use Shader
		void useShader(int shaderID);

		//Render Model
		void renderModel(int shaderID, int modelID);

		//Render Skybox
		void renderSkybox(int frameBufferID);

		//Set Skybox
		void setSkybox(int frameBufferID, int cubemapTextureID);

		//Disable Skybox
		void disableSkybox(int frameBufferID);

		//Set Clear Color
		void setClearColor(float r, float g, float b, float a);

		//Get Mouse X / Y
		double getMouseX();
		double getMouseY();

		//Is Key Pressed
		bool isKeyPressed(int key);

		//Is Mouse Button Clicked or Released
		bool isMouseClicked(int button);
		bool isMouseReleased(int button);

		//Get Log
		Log* getLog() const;

		//Get Shader
		Shader* getShader(int shaderID) const;

		//Get Model
		Model* getModel(int modelID) const;

		//Mouse To World
		void mouseToWorld();

		//Get Mouse To World
		float getMouseToWorldX() const;
		float getMouseToWorldY() const;
		float getMouseToWorldZ() const;

		//Get Display Width and Height
		int getDisplayWidth() const;
		int getDisplayHeight() const;

		//Set Window Title
		void setWindowTitle(const char* title);

		//Delete Framebuffer
		void deleteFrameBuffer(int framebufferID);

		//Delete Shader
		void deleteShader(int shaderID);

		//Delete Model
		void deleteModel(int modelID);

		//Delete Hidden Meshes
		void deleteHiddenMeshes(int modelID);

		//Delete Mesh
		void deleteMesh(int meshID);

		//Delete Texture
		void deleteTexture(int textureID);

		//Delete Font
		void deleteFont(int fontID);

	private:

		//GLFW init
		void initGLFW(const char* window_title);

		//GLEW init
		void initGLEW();

		//Init GL Settings
		void initGLSettings();

		//Init Screen Shader and Screen
		void initScreenVar();

		//Init FrameBuffer Render Model
		void initFrameBufferRenderModel();

		//Log
		Log* m_log;

		//GLFW Window
		GLFWwindow* m_window;

		//Resources
		std::vector<FrameBuffer*>	m_framebuffers;
		std::vector<Shader*>		m_shaders;
		std::vector<Model*>			m_models;
		std::vector<Mesh*>			m_meshes;
		std::vector<Texture*>		m_textures;
		std::vector<Font*>			m_fonts;

		//Projection/View Matrix
		glm::mat4 m_projectionMatrix;
		glm::mat4 m_viewMatrix;

		//Screen Camera Settings
		static const float Screen_Near;
		static const float Screen_Far;

		//Screen Shader and Model
		Shader* m_screenShader;
		Model*	m_screenModel;
		int		m_screenModelID;

		//FrameBuffer Render Model
		Model*	m_frameBufferRenderModel;
		int		m_frameBufferRenderModelID;

		//Mouse To World
		glm::vec3 m_mouseToWorld;

		//Skybox Mesh
		Mesh* m_skyboxMesh;
		int	m_skyboxShaderID;

	};
}