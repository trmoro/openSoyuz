#include "Core.h"

namespace SK
{
	//Screen Camera Settings
	const float Core::Screen_Near	= -255.0f;
	const float Core::Screen_Far	= 255.0f;

	//Constructor
	Core::Core()
	{
		//Variable
		m_log = new Log();
		m_window = nullptr;

		m_framebuffers = std::vector<FrameBuffer*>();
		m_shaders = std::vector<Shader*>();
		m_models = std::vector<Model*>();
		m_meshes = std::vector<Mesh*>();
		m_textures = std::vector<Texture*>();
		m_fonts = std::vector<Font*>();

		m_projectionMatrix = glm::mat4(0);
		m_viewMatrix = glm::mat4(0);

		//openSoyuz
		std::cout << ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" << std::endl;
		std::cout << ">>>>>>>>   openSoyuz   >>>>>>>>" << std::endl;
		std::cout << ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" << std::endl << std::endl;

		//Core Inited
		m_log->add("Core Started", LOG_OK);
	}

	//Destructor
	Core::~Core()
	{
		//Clean Up GLFW
		glfwDestroyWindow(m_window);
		glfwTerminate();

		//Clean up FreeType
		Font::clearFreeType();

		//Delete Log
		delete m_log;
	}

	//Init Core
	void Core::init()
	{
		//Init GLFW and GLEW
		initGLFW();
		initGLEW();

		//Init OpenGL Settings
		initGLSettings();

		//Init Screen Variable (Model and Shader)
		initScreenVar();

		//Init FrameBuffer Render Model
		initFrameBufferRenderModel();

		//Init FreeType
		Font::initFreeType();

		//Skybox Mesh and Shader
		m_skyboxMesh = new Mesh();
		m_skyboxMesh->transformAsSkybox();
		m_skyboxShaderID = createShader();
		setPrefabShader(m_skyboxShaderID, PREFAB_SHADER_SKYBOX);

		//Core Inited
		m_log->add("Core Initialized", LOG_OK);
	}

	//Update
	//Returns true to end core
	bool Core::update()
	{
		//Swap Buffer and Update Event
		glfwSwapBuffers(m_window);
		glfwPollEvents();

		return glfwWindowShouldClose(m_window);
	}

	//Init GLFW
	void Core::initGLFW()
	{
		//Initialization gone wrong ?
		if (!glfwInit())
			m_log->add("GLFW initialization went wrong", LOG_ERROR);

		//Set OpenGL context to 3.3, Anti-aliazing : 4
		glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
		glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
		glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
		glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);
		//glfwWindowHint(GLFW_SAMPLES, MSAA);

		//Create Window
		Input::Window_Width = 640;
		Input::Window_Height = 480;
		m_window = glfwCreateWindow(Input::Window_Width, Input::Window_Height, "Hello World", nullptr, nullptr);
		glfwMakeContextCurrent(m_window);

		//Set Callbacks
		glfwSetKeyCallback(m_window, Input::keyCallback);
		glfwSetCharCallback(m_window, Input::characterCallback);
		glfwSetWindowSizeCallback(m_window, Input::windowSizeCallback);
		glfwSetCursorPosCallback(m_window, Input::cursorPositionCallback);
		glfwSetMouseButtonCallback(m_window, Input::mouseButtonCallback);
		glfwSetScrollCallback(m_window, Input::scrollCallback);
		glfwSetDropCallback(m_window, Input::dropCallback);

		//Set Swap Interval
		glfwSwapInterval(1);

		//Initialization went well
		m_log->add("GLFW initialization went well", LOG_OK);
	}

	//Init GLEW
	void Core::initGLEW()
	{
		//Enable using OpenGL 3 or greater, and init GLEW
		glewExperimental = GL_TRUE;
		GLenum err = glewInit();

		//Print Error
		if (GLEW_OK != err)
			std::cout << glewGetErrorString(err) << std::endl;
		//Show end of initialization
		else
			m_log->add("GLEW initialization went well", LOG_OK);
	}

	//Init GL Settings
	void Core::initGLSettings()
	{
		//Set GL View and Clear Option
		glClearColor(0.0f, 0.0f, 0.0f, 0.0f);

		//How OpenGL Blend
		glEnable(GL_BLEND);
		glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

		//Depth Settings
		glEnable(GL_DEPTH_TEST);
		glDepthFunc(GL_LESS);

		//Enable texture
		glEnable(GL_TEXTURE_2D);
		glEnable(GL_TEXTURE_CUBE_MAP);

		//Enable Multisampling (for anti-aliasing)
		//glEnable(GL_MULTISAMPLE);
	}

	//Init Screen Model and Shader
	void Core::initScreenVar()
	{
		//Shader
		int shaderID = createShader();
		setShader(shaderID, ShaderScript::Basic_Vertex, ShaderScript::Screen_Fragment);
		m_screenShader = getShader(shaderID);

		//Model
		int modelID = createModel();

		//Create Mesh
		Mesh* msh = new Mesh();
		m_models[modelID]->addHiddenMesh(msh);

		//Prepare Memory
		msh->prepareMemory(4, 6);

		//Add Vertices
		msh->addVertex(0, 0, 0, 0, 0, 0, 1, 1);
		msh->addVertex(1, 0, 0, 0, 0, 0, 0, 1);
		msh->addVertex(1, 1, 0, 0, 0, 0, 0, 0);
		msh->addVertex(0, 1, 0, 0, 0, 0, 1, 0);

		//Add Indices : Two Triangles
		msh->addIndex(0);
		msh->addIndex(1);
		msh->addIndex(2);
		msh->addIndex(0);
		msh->addIndex(2);
		msh->addIndex(3);

		//Compile
		msh->compile();

		//Set Position and Rotation
		setModelPosition(modelID, 0, 0, 0);
		setModelRotation(modelID, 0, 0, 0);
		setModelScale(modelID, Input::Window_Width, Input::Window_Height, 1);

		//Get model object
		m_screenModel = getModel(modelID);
		m_screenModelID = modelID;
	}

	//Init FrameBuffer Render Model
	void Core::initFrameBufferRenderModel()
	{
		//Model
		int modelID = createModel();

		//Create Mesh
		Mesh* msh = new Mesh();
		addMesh(msh);
		m_models[modelID]->addHiddenMesh(msh);

		//Prepare Memory
		msh->prepareMemory(4, 6);

		//Add Vertices
		msh->addVertex(0, 0, 0, 0, 0, 0, 0, 1);
		msh->addVertex(1, 0, 0, 0, 0, 0, 1, 1);
		msh->addVertex(1, 1, 0, 0, 0, 0, 1, 0);
		msh->addVertex(0, 1, 0, 0, 0, 0, 0, 0);

		//Add Indices : Two Triangles
		msh->addIndex(0);
		msh->addIndex(1);
		msh->addIndex(2);
		msh->addIndex(0);
		msh->addIndex(2);
		msh->addIndex(3);

		//Compile
		msh->compile();

		//Set Position and Rotation
		setModelPosition(modelID, 0, 0, 0);
		setModelRotation(modelID, 0, 0, 0);
		setModelScale(modelID, Input::Window_Width, Input::Window_Height, 1);

		//Get model object
		m_frameBufferRenderModel = getModel(modelID);
		m_frameBufferRenderModelID = modelID;
	}

	//Create FrameBuffer
	int Core::createFrameBuffer()
	{
		FrameBuffer* fb = new FrameBuffer(m_log);
		int i = (int) m_framebuffers.size();
		m_framebuffers.push_back(fb);
		return i;
	}

	//Create Shader
	int Core::createShader()
	{
		Shader* sh = new Shader(m_log);
		int i = (int) m_shaders.size();
		m_shaders.push_back(sh);
		return i;
	}

	//Create Model
	int Core::createModel()
	{
		Model* md = new Model(m_log);
		int i = (int)m_models.size();
		m_models.push_back(md);
		return i;
	}

	//Create Texture
	int Core::createTexture()
	{
		Texture* t = new Texture(m_log);
		int i = (int)m_textures.size();
		m_textures.push_back(t);
		return i;
	}

	//Create Font
	int Core::createFont()
	{
		Font* f = new Font(m_log);
		int i = (int)m_fonts.size();
		m_fonts.push_back(f);
		return i;
	}

	//Create Mesh
	int Core::createMesh()
	{
		Mesh* m = new Mesh();
		return addMesh(m);
	}

	//Add Mesh
	int Core::addMesh(Mesh* mesh)
	{
		int i = (int)m_meshes.size();
		m_meshes.push_back(mesh);
		return i;
	}

	//Add Mesh To Model
	void Core::addMeshToModel(int modelID, int meshID)
	{
		m_models[modelID]->addMesh(meshID,m_meshes[meshID]);
	}

	//Mesh Prepare Memory
	void Core::meshPrepareMemory(int meshID, unsigned int nVertex, unsigned int nIndex)
	{
		m_meshes[meshID]->prepareMemory(nVertex, nIndex);
	}

	//Mesh Add Vertex
	void Core::meshAddVertex(int meshID, float x, float y, float z, float nX, float nY, float nZ, float uvX, float uvY)
	{
		m_meshes[meshID]->addVertex(x, y, z, nX, nY, nZ, uvX, uvY);
	}

	//Mesh Add Index
	void Core::meshAddIndex(int meshID, int i)
	{
		m_meshes[meshID]->addIndex(i);
	}

	//Mesh Compile
	void Core::meshCompile(int meshID)
	{
		m_meshes[meshID]->compile();
	}

	//Set Model Position
	void Core::setModelPosition(int modelID, float x, float y, float z)
	{
		m_models[modelID]->setPosition(glm::vec3(x, y, z));
	}

	//Set Model Rotation
	void Core::setModelRotation(int modelID, float x, float y, float z)
	{
		m_models[modelID]->setRotation(glm::vec3(x, y, z));
	}

	//Set Model Scale
	void Core::setModelScale(int modelID, float x, float y, float z)
	{
		m_models[modelID]->setScale(glm::vec3(x, y, z));
	}

	//Load Model
	void Core::loadModel(int modelID, const char* path)
	{
		m_models[modelID]->load(path);
	}

	//Set Mesh Draw Mode
	void Core::setMeshDrawMode(int meshID, unsigned int drawmode)
	{
		m_meshes[meshID]->setDrawMode(drawmode);
	}

	//Set Perspective Camera
	void Core::setPerspectiveCamera(float x, float y, float z, float targetX, float targetY, float targetZ, float radius, float near, float far)
	{
		//Projection
		m_projectionMatrix = glm::perspective(glm::radians(radius), (float) Input::Window_Width / (float) Input::Window_Height, near, far);

		//View
		m_viewMatrix = glm::lookAt(glm::vec3(x,y,z), glm::vec3(targetX,targetY,targetZ), glm::vec3(0, 1, 0));
		//view = glm::rotate(view, 3.14159265359f, glm::vec3(0.0f, 0.0f, 1.0f));
	}

	//Set Orthographic Camera
	void Core::setOrthographicCamera(float near, float far)
	{
		//Projection Matrix
		m_projectionMatrix = glm::ortho(0.0f, (float) Input::Window_Width, (float) Input::Window_Height, 0.0f, near, far);

		//Identiy Matrix for View
		m_viewMatrix = glm::mat4(1);
	}

	//Set Orthographic Box Camera
	void Core::setOrthographicBoxCamera(float x, float y, float z, float targetX, float targetY, float targetZ, float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
	{
		//Projection
		m_projectionMatrix = glm::ortho(xMin, xMax, yMin, yMax, zMin, zMax);
		
		//View
		m_viewMatrix = glm::lookAt(glm::vec3(x, y, z), glm::vec3(targetX, targetY, targetZ), glm::vec3(0, 1, 0));
	}

	//Set Shader
	void Core::setShader(int shaderID, const char* vertex, const char* fragment)
	{
		m_shaders[shaderID]->set(vertex, fragment);
	}

	//Set Shader with Geometry
	void Core::setShader(int shaderID, const char* vertex, const char* geometry, const char* fragment)
	{
		m_shaders[shaderID]->set(vertex, true, geometry, fragment);
	}

	//Set Shader with Prefab Shader Script
	void Core::setPrefabShader(int shaderID, int prefabID)
	{
		switch (prefabID)
		{
		case PREFAB_SHADER_COLOR:
			m_log->add("Prefab : Color Shader", LOG_OK);
			m_shaders[shaderID]->set(ShaderScript::Basic_Vertex, ShaderScript::Color_Fragment);
			break;
		case PREFAB_SHADER_NORMAL:
			m_log->add("Prefab : Normal Shader", LOG_OK);
			m_shaders[shaderID]->set(ShaderScript::Basic_Vertex, ShaderScript::Normal_Fragment);
			break;
		case PREFAB_SHADER_MIX:
			m_log->add("Prefab : Mix Shader", LOG_OK);
			m_shaders[shaderID]->set(ShaderScript::Basic_Vertex, ShaderScript::Mix_Fragment);
			break;
		case PREFAB_SHADER_CONV:
			m_log->add("Prefab : Convolution Shader", LOG_OK);
			m_shaders[shaderID]->set(ShaderScript::Basic_Vertex, ShaderScript::Conv_Fragment);
			break;
		case PREFAB_SHADER_REVERSE:
			m_log->add("Prefab : Reverse Shader", LOG_OK);
			m_shaders[shaderID]->set(ShaderScript::Basic_Vertex, ShaderScript::Reverse_Fragment);
			break;
		case PREFAB_SHADER_LIGHTING:
			m_log->add("Prefab : Lighting Shader", LOG_OK);
			m_shaders[shaderID]->set(ShaderScript::Basic_Vertex, ShaderScript::Lighting_Fragment);
			break;
		case PREFAB_SHADER_FONT:
			m_log->add("Prefab : Font Shader", LOG_OK);
			m_shaders[shaderID]->set(ShaderScript::Basic_Vertex, ShaderScript::Font_Fragment);
			break;
		case PREFAB_SHADER_GUI:
			m_log->add("Prefab : GUI Element Shader", LOG_OK);
			m_shaders[shaderID]->set(ShaderScript::Basic_Vertex, ShaderScript::Gui_Fragment);
			break;
		case PREFAB_SHADER_SKYBOX:
			m_log->add("Prefab : Skybox Shader", LOG_OK);
			m_shaders[shaderID]->set(ShaderScript::Skybox_Vertex, ShaderScript::Skybox_Fragment);
			break;
		case PREFAB_SHADER_REFLECT:
			m_log->add("Prefab : Reflect Shader", LOG_OK);
			m_shaders[shaderID]->set(ShaderScript::Basic_Vertex, ShaderScript::Reflect_Fragment);
			break;
		case PREFAB_SHADER_REFRACT:
			m_log->add("Prefab : Refract Shader", LOG_OK);
			m_shaders[shaderID]->set(ShaderScript::Basic_Vertex, ShaderScript::Refract_Fragment);
			break;
		}
	}

	//Set Uniform FrameBuffer
	void Core::setUniformFrameBuffer(int shaderID, const char* name, int frameBufferID, bool depth, int textureId)
	{
		//Active Texture
		glActiveTexture(GL_TEXTURE0 + textureId);

		//Bind
		if(depth)
			glBindTexture(GL_TEXTURE_2D, m_framebuffers[frameBufferID]->getDepthID());
		else
			glBindTexture(GL_TEXTURE_2D, m_framebuffers[frameBufferID]->getTextureID());

		//Set Uniform
		m_shaders[shaderID]->setUniformi( (GLchar*) name, textureId);
	}

	//Set Uniform Int
	void Core::setUniformi(int shaderID, const char* name, int value)
	{
		m_shaders[shaderID]->setUniformi((GLchar*)name, value);
	}

	//Set Uniform Float
	void Core::setUniformf(int shaderID, const char* name, float value)
	{
		m_shaders[shaderID]->setUniformf((GLchar*)name, value);
	}

	//Set Uniform Vec2
	void Core::setUniformVec2(int shaderID, const char* name, float x, float y)
	{
		m_shaders[shaderID]->setUniformVec2((GLchar*)name, glm::vec2(x, y));
	}

	//Set Uniform Vec3
	void Core::setUniformVec3(int shaderID, const char* name, float x, float y, float z)
	{
		m_shaders[shaderID]->setUniformVec3((GLchar*)name, glm::vec3(x, y, z));
	}

	//Set Uniform Vec4
	void Core::setUniformVec4(int shaderID, const char* name, float x, float y, float z, float w)
	{
		m_shaders[shaderID]->setUniformVec4((GLchar*)name, glm::vec4(x, y, z, w));
	}

	//Set Uniform Matrix3x3
	void Core::setUniformMat3(int shaderID, const char* name, float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33)
	{
		glm::mat3 mat = glm::mat3(m11, m12, m13, m21, m22, m23, m31, m32, m33);
		m_shaders[shaderID]->setUniformMatrix3((GLchar*)name, mat);
	}

	//Set Uniform Texture
	void Core::setUniformTexture(int shaderID, const char* name, int textureID, int textureIndex)
	{
		//Active Texture
		glActiveTexture(GL_TEXTURE0 + textureIndex);

		//Bind
		if (m_textures[textureID]->isCubemap())
			glBindTexture(GL_TEXTURE_CUBE_MAP, m_textures[textureID]->getID());
		else
			glBindTexture(GL_TEXTURE_2D, m_textures[textureID]->getID());

		//Set Uniform
		m_shaders[shaderID]->setUniformi((GLchar*)name, textureIndex);
	}

	//Set Uniform Font
	void Core::setUniformFont(int shaderID, const char* name, int fontID, int textureIndex)
	{
		//Active Texture
		glActiveTexture(GL_TEXTURE0 + textureIndex);

		//Bind
		glBindTexture(GL_TEXTURE_2D, m_fonts[fontID]->getTexture());

		//Set Uniform
		m_shaders[shaderID]->setUniformi((GLchar*)name, textureIndex);
	}

	//Set Texture with Data Array
	void Core::setTextureWithDataArray(int textureID, unsigned int width, unsigned int height, unsigned int nChannel, float* data)
	{
		m_textures[textureID]->genFromDataArray(width, height, nChannel, data);
	}

	//Set Texture with Source Path
	void Core::setTextureWithSourcePath(int textureID, const char* path, unsigned int nChannel)
	{
		m_textures[textureID]->genFromPath(path,nChannel);
	}

	//Set Texture Filled
	void Core::setTextureFilled(int textureID, unsigned int width, unsigned int height, unsigned int nChannel, float value)
	{
		m_textures[textureID]->genFilled(width, height, nChannel, value);
	}

	//Set Texture as Cubemap
	void Core::setTextureAsCubemap(int textureID, const char* right, const char* left, const char* top, const char* bottom, const char* front, const char* back)
	{
		m_textures[textureID]->genCubemap(right, left, top, bottom, front, back);
	}

	//Get Texture Width
	unsigned int Core::getTextureWidth(int textureID)
	{
		return m_textures[textureID]->getWidth();
	}

	//Get Texture Height
	unsigned int Core::getTextureHeight(int textureID)
	{
		return m_textures[textureID]->getHeight();
	}

	//Get Texture Number Of Channel
	unsigned int Core::getTextureNumberOfChannel(int textureID)
	{
		return m_textures[textureID]->getNumberOfChannel();
	}

	//Get Texture Data
	float* Core::getTextureData(int textureID)
	{
		return m_textures[textureID]->getData();
	}

	//Save Texture to PNG
	bool Core::saveTexturePNG(int textureID, std::string filePath)
	{
		return m_textures[textureID]->savePNG(filePath);
	}

	//Convolution on texture
	void Core::textureConv(int textureID, unsigned int size, float* matrix, float coef)
	{
		m_textures[textureID]->conv(size, matrix, coef);
	}

	//Set Texture Pixel
	void Core::textureSetPixel(int textureID, float x, float y, unsigned int channel, float value)
	{
		m_textures[textureID]->setPixel(x, y, channel, value);
	}

	//Get Texture Value
	float Core::textureGetPixel(int textureID, float x, float y, unsigned int channel)
	{
		return m_textures[textureID]->getPixel(x,y,channel);
	}

	//Update Texture
	void Core::updateTexture(int textureID)
	{
		m_textures[textureID]->update();
	}

	//Apply Texture Transform
	void Core::textureTransform(int textureID, unsigned int transformID, float* args)
	{
		m_textures[textureID]->applyTransform(transformID,args);
	}

	//Load Font
	void Core::loadFont(int fontID, std::string path, unsigned int size, unsigned int start, unsigned int end)
	{
		m_fonts[fontID]->load(path, size, start, end);
	}

	//Add Text as Mesh to the given Model
	void Core::addTextAsMesh(int fontID, int modelID, std::string text, float x, float y, float max_width, float lineSpacing)
	{
		m_fonts[fontID]->addTextAsMesh(m_models[modelID], text, x, y, max_width, lineSpacing);
	}

	//Render FrameBuffer Init
	void Core::renderFrameBufferInit(int frameBufferID, int shaderID)
	{
		//Update FrameBuffer
		m_framebuffers[frameBufferID]->update(Input::Window_Width, Input::Window_Height);

		//Binding
		glBindFramebuffer(GL_FRAMEBUFFER, m_framebuffers[frameBufferID]->getID());
		glViewport(0, 0, Input::Window_Width, Input::Window_Height);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		//Use Shader
		glUseProgram(m_shaders[shaderID]->getID());

		//Resize Model
		setModelScale(m_frameBufferRenderModelID, Input::Window_Width, Input::Window_Height, 1);

		//Set Camera
		setOrthographicCamera(Screen_Near, Screen_Far);
	}

	//Render FrameBuffer
	void Core::renderFrameBuffer(int shaderID)
	{
		//Render Model
		m_screenShader->setUniformMatrix4((GLchar*)"Projection", m_projectionMatrix);
		m_screenShader->setUniformMatrix4((GLchar*)"View", m_viewMatrix);
		m_screenShader->setUniformMatrix4((GLchar*)"Model", m_frameBufferRenderModel->getModelRotationMatrix());
		m_screenShader->setUniformMatrix4((GLchar*)"ModelRotation", m_frameBufferRenderModel->getRotationMatrix());
		m_frameBufferRenderModel->render();

		//Unbind
		glBindTexture(GL_TEXTURE_2D, 0);
		glUseProgram(0);
	}

	//Show FrameBuffer
	void Core::showFrameBuffer(int frameBufferID)
	{
		/*
		glBindFramebuffer(GL_READ_FRAMEBUFFER, m_framebuffers[frameBufferID]->getDepthID() );
		glBindFramebuffer(GL_DRAW_FRAMEBUFFER, 0);
		glBlitFramebuffer(0, 0, Input::Window_Width, Input::Window_Height, 0, 0, Input::Window_Width, Input::Window_Height, GL_COLOR_BUFFER_BIT, GL_NEAREST);

		*/

		//OpenGL Texture ID
		GLuint glTextureID = m_framebuffers[frameBufferID]->getTextureID();

		//Binding
		glBindFramebuffer(GL_FRAMEBUFFER, 0);
		glViewport(0, 0, Input::Window_Width, Input::Window_Height);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		//Use Shader
		glUseProgram(m_screenShader->getID());

		//Resize Model
		setModelScale(m_screenModelID, Input::Window_Width, Input::Window_Height, 1);

		//Set Camera
		setOrthographicCamera(Screen_Near, Screen_Far);

		//Active Texture
		glActiveTexture(GL_TEXTURE0);
		glBindTexture(GL_TEXTURE_2D, glTextureID);

		//Render Model
		m_screenShader->setUniformi((GLchar*)"m_texture", 0);
		m_screenShader->setUniformMatrix4((GLchar*)"Projection", m_projectionMatrix);
		m_screenShader->setUniformMatrix4((GLchar*)"View", m_viewMatrix);
		m_screenShader->setUniformMatrix4((GLchar*)"Model", m_screenModel->getModelRotationMatrix());
		m_screenShader->setUniformMatrix4((GLchar*)"ModelRotation", m_screenModel->getRotationMatrix());
		m_screenModel->render();

		//Unbind
		glBindTexture(GL_TEXTURE_2D, 0);
		glUseProgram(0);
	}

	//Use Framebuffer
	void Core::useFrameBuffer(int frameBufferID)
	{
		//Get Objects
		FrameBuffer* f = m_framebuffers[frameBufferID];

		//Width and Height
		int width = Input::Window_Width;
		int height = Input::Window_Height;

		//Update FrameBuffer
		f->update(width, height);

		//Binding
		glBindFramebuffer(GL_FRAMEBUFFER, f->getID());
		glViewport(0, 0, width, height);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	}

	//Use Shader
	void Core::useShader(int shaderID)
	{
		Shader* s = m_shaders[shaderID];
		glUseProgram(s->getID());
	}

	//Render Model
	void Core::renderModel(int shaderID, int modelID)
	{
		//Get Objects
		Shader* s = m_shaders[shaderID];
		Model* m = m_models[modelID];
		
		//Render
		s->setUniformMatrix4((GLchar*)"Projection", m_projectionMatrix);
		s->setUniformMatrix4((GLchar*)"View", m_viewMatrix);
		s->setUniformMatrix4((GLchar*)"Model", m->getModelRotationMatrix());
		s->setUniformMatrix4((GLchar*)"Rotation", m->getRotationMatrix());

		m->render();
	}

	//Render Skybox (Use it after rendering models)
	void Core::renderSkybox(int frameBufferID)
	{
		//If Framebuffer has Skybox
		if (m_framebuffers[frameBufferID]->hasSkybox())
		{
			// Change depth function so depth test passes when values are equal to depth buffer's content
			glDepthFunc(GL_LEQUAL);

			// Use Skybox Shader
			useShader(m_skyboxShaderID);

			//Compute View matrix without translation
			glm::mat4 view = glm::mat4(glm::mat3(m_viewMatrix));

			//Set Uniform
			Shader* s = getShader(m_skyboxShaderID);
			s->setUniformMatrix4( (GLchar*) "Projection", m_projectionMatrix);
			s->setUniformMatrix4( (GLchar*) "View", view);

			// Skybox Texture
			glActiveTexture(GL_TEXTURE0);
			glBindTexture(GL_TEXTURE_CUBE_MAP, m_framebuffers[frameBufferID]->getSkyboxTexture()->getID());;

			//Render Mesh
			m_skyboxMesh->render();

			// Set depth function back to default
			glDepthFunc(GL_LESS);
		}
	}

	//Set Skybox
	void Core::setSkybox(int frameBufferID, int cubemapTextureID)
	{
		m_framebuffers[frameBufferID]->setSkybox(m_textures[cubemapTextureID]);
	}

	//Disable Skybox
	void Core::disableSkybox(int frameBufferID)
	{
		m_framebuffers[frameBufferID]->disableSkybox();
	}

	//Set Clear Color
	void Core::setClearColor(float r, float g, float b, float a)
	{
		glClearColor(r, g, b, a);
	}

	//Get Mouse X
	double Core::getMouseX()
	{
		return Input::Mouse_X;
	}

	//Get Mouse Y
	double Core::getMouseY()
	{
		return Input::Mouse_Y;
	}

	//Is Key Pressed
	bool Core::isKeyPressed(int key)
	{
		return (key == Input::Key_Char && Input::Key_Action != 0);
	}

	//Is Mouse Clicked
	bool Core::isMouseClicked(int button)
	{
		return (button == Input::Mouse_Button && Input::Mouse_Action == 1);
	}

	//Is Mouse Released
	bool Core::isMouseReleased(int button)
	{
		return (button == Input::Mouse_Button && Input::Mouse_Action == 0);
	}

	//Get Log
	Log* Core::getLog() const
	{
		return m_log;
	}

	//Get Shader
	Shader* Core::getShader(int shaderID) const
	{
		return m_shaders[shaderID];
	}

	//Get Model
	Model* Core::getModel(int modelID) const
	{
		return m_models[modelID];
	}

	//Mouse To World
	void Core::mouseToWorld()
	{
		//Source http://antongerdelan.net/opengl/raycasting.html

		//Normalised Device Coordinates
		float x = (2.0f * Input::Mouse_X) / Input::Window_Width - 1.0f;
		float y = 1.0f - (2.0f * Input::Mouse_Y) / Input::Window_Height;
		float z = 1.0f;
		glm::vec3 ray_nds = glm::vec3(-x, y, z);

		//Homogeneous Clip Coordinates
		glm::vec4 ray_clip = glm::vec4(ray_nds.x, ray_nds.y, -1.0, 1.0);

		//4d Eye (Camera) Coordinates
		glm::vec4 ray_eye = glm::inverse(m_projectionMatrix) * ray_clip;
		ray_eye = glm::vec4(ray_eye.x, ray_eye.y, -1.0, 0.0);

		//4d World Coordinates
		glm::vec4 ray_wor = glm::inverse(m_viewMatrix) * ray_eye;
		ray_wor = glm::normalize(ray_wor);
		m_mouseToWorld = glm::vec3(ray_wor.x, ray_wor.y, ray_wor.z);
	}

	//Mouse To World X
	float Core::getMouseToWorldX() const
	{
		return m_mouseToWorld.x;
	}

	//Mouse To World Y
	float Core::getMouseToWorldY() const
	{
		return m_mouseToWorld.y;
	}

	//Mouse To World Z
	float Core::getMouseToWorldZ() const
	{
		return m_mouseToWorld.z;
	}

	//Get Display Width
	int Core::getDisplayWidth() const
	{
		return Input::Window_Width;
	}

	//Get Display Height
	int Core::getDisplayHeight() const
	{
		return Input::Window_Height;
	}

	//Set Window Title
	void Core::setWindowTitle(const char* title)
	{
		glfwSetWindowTitle(m_window, title);
	}

	//Delete Framebuffer
	void Core::deleteFrameBuffer(int framebufferID)
	{
		delete m_framebuffers[framebufferID];
		m_framebuffers[framebufferID] = nullptr;
	}

	//Delete Shader
	void Core::deleteShader(int shaderID)
	{
		delete m_shaders[shaderID];
		m_shaders[shaderID] = nullptr;
	}

	//Delete Model
	void Core::deleteModel(int modelID)
	{
		delete m_models[modelID];
		m_models[modelID] = nullptr;
	}

	//Delete Mesh
	void Core::deleteMesh(int meshID)
	{
		//CHANGES TODO

		//for (Model* mod : m_models)
		//	mod->deleteMesh(m_meshes[meshID]);
		
		//std::cout << meshID << " " << m_meshes.size() << std::endl;
		
		//delete m_meshes[meshID];
		//m_meshes[meshID] = nullptr;
	}

	//Delete Texture
	void Core::deleteTexture(int textureID)
	{
		delete m_textures[textureID];
		m_textures[textureID] = nullptr;
	}

	//Delete Font
	void Core::deleteFont(int fontID)
	{
		delete m_fonts[fontID];
		m_fonts[fontID] = nullptr;
	}

	//
}