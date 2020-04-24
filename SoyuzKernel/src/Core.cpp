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
		m_textures = std::vector<Texture*>();

		m_projViewMatrix = glm::mat4(0);

		//Core Inited
		m_log->add("Core Initialized", LOG_OK);
	}

	//Destructor
	Core::~Core()
	{
		//Clean Up GLFW
		glfwDestroyWindow(m_window);
		glfwTerminate();

		//Log
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
		glfwWindowHint(GLFW_SAMPLES, MSAA);

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
		int meshID = createMesh(modelID);

		//Prepare Memory
		meshPrepareMemory(modelID, meshID, 4, 6);

		//Add Vertices
		meshAddVertex(modelID, meshID, 0, 0, 0, 0, 0, 0, 1, 1);
		meshAddVertex(modelID, meshID, 1, 0, 0, 0, 0, 0, 0, 1);
		meshAddVertex(modelID, meshID, 1, 1, 0, 0, 0, 0, 0, 0);
		meshAddVertex(modelID, meshID, 0, 1, 0, 0, 0, 0, 1, 0);

		//Add Indices : Two Triangles
		meshAddIndex(modelID, meshID, 0);
		meshAddIndex(modelID, meshID, 1);
		meshAddIndex(modelID, meshID, 2);
		meshAddIndex(modelID, meshID, 0);
		meshAddIndex(modelID, meshID, 2);
		meshAddIndex(modelID, meshID, 3);

		//Compile
		meshCompile(modelID, meshID);

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
		int meshID = createMesh(modelID);

		//Prepare Memory
		meshPrepareMemory(modelID, meshID, 4, 6);

		//Add Vertices
		meshAddVertex(modelID, meshID, 0, 0, 0, 0, 0, 0, 0, 1);
		meshAddVertex(modelID, meshID, 1, 0, 0, 0, 0, 0, 1, 1);
		meshAddVertex(modelID, meshID, 1, 1, 0, 0, 0, 0, 1, 0);
		meshAddVertex(modelID, meshID, 0, 1, 0, 0, 0, 0, 0, 0);

		//Add Indices : Two Triangles
		meshAddIndex(modelID, meshID, 0);
		meshAddIndex(modelID, meshID, 1);
		meshAddIndex(modelID, meshID, 2);
		meshAddIndex(modelID, meshID, 0);
		meshAddIndex(modelID, meshID, 2);
		meshAddIndex(modelID, meshID, 3);

		//Compile
		meshCompile(modelID, meshID);

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
		Model* md = new Model();
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

	//Create Mesh
	int Core::createMesh(int modelID) const
	{
		return m_models[modelID]->createMesh();
	}

	//Mesh Prepare Memory
	void Core::meshPrepareMemory(int modelID, int meshID, unsigned int nVertex, unsigned int nIndex)
	{
		m_models[modelID]->meshPrepareMemory(meshID, nVertex, nIndex);
	}

	//Mesh Add Vertex
	void Core::meshAddVertex(int modelID, int meshID, float x, float y, float z, float nX, float nY, float nZ, float uvX, float uvY)
	{
		m_models[modelID]->meshAddVertex(meshID, x, y, z, nX, nY, nZ, uvX, uvY);
	}

	//Mesh Add Index
	void Core::meshAddIndex(int modelID, int meshID, int i)
	{
		m_models[modelID]->meshAddIndex(meshID, i);
	}

	//Mesh Compile
	void Core::meshCompile(int modelID, int meshID)
	{
		m_models[modelID]->meshCompile(meshID);
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

	//Set Perspective Camera
	void Core::setPerspectiveCamera(float x, float y, float z, float targetX, float targetY, float targetZ, float radius, float near, float far)
	{
		//Projection
		glm::mat4 projection = glm::perspective(glm::radians(radius), (float) Input::Window_Width / (float) Input::Window_Height, near, far);

		//View
		glm::mat4 view = glm::lookAt(glm::vec3(x,y,z), glm::vec3(targetX,targetY,targetZ), glm::vec3(0, 1, 0));
		//view = glm::rotate(view, 3.14159265359f, glm::vec3(0.0f, 0.0f, 1.0f));

		//Set
		m_projViewMatrix = projection * view;
		m_projectionMatrix = projection;
		m_viewMatrix = view;
	}

	//Set Orthographic Camera
	void Core::setOrthographicCamera(float near, float far)
	{
		m_projViewMatrix = glm::ortho(0.0f, (float) Input::Window_Width, (float) Input::Window_Height, 0.0f, near, far);
	}

	//Set Orthographic Box Camera
	void Core::setOrthographicBoxCamera(float x, float y, float z, float targetX, float targetY, float targetZ, float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
	{
		glm::mat4 projection = glm::ortho(xMin, xMax, yMin, yMax, zMin, zMax);
		glm::mat4 view = glm::lookAt(glm::vec3(x, y, z), glm::vec3(targetX, targetY, targetZ), glm::vec3(0, 1, 0));

		//Set
		m_projViewMatrix = projection * view;
		m_projectionMatrix = projection;
		m_viewMatrix = view;
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
		case PREFAB_SHADER_LIGHTING:
			m_log->add("Prefab : Lighting Shader", LOG_OK);
			m_shaders[shaderID]->set(ShaderScript::Lighting_Vertex, ShaderScript::Lighting_Fragment);
			break;
		}
	}

	//Set Uniform FrameBuffer
	void Core::setUniformFrameBuffer(int shaderID, const char* name, int frameBufferID, bool depth, int textureId)
	{
		//Active Texture
		glActiveTexture(GL_TEXTURE0 + textureId);
		glEnable(GL_TEXTURE_2D);

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
		glEnable(GL_TEXTURE_2D);

		//Bind
		glBindTexture(GL_TEXTURE_2D, m_textures[textureID]->getID());

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
		m_screenShader->setUniformMatrix4((GLchar*)"ProjViewModel", getProjView() * m_frameBufferRenderModel->getModelRotationMatrix());
		m_screenShader->setUniformMatrix4((GLchar*)"RotationMatrix", m_frameBufferRenderModel->getRotationMatrix());
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
		m_screenShader->setUniformMatrix4((GLchar*)"ProjViewModel", getProjView() * m_screenModel->getModelRotationMatrix());
		m_screenShader->setUniformMatrix4((GLchar*)"RotationMatrix", m_screenModel->getRotationMatrix());
		m_screenModel->render();

		//Unbind
		glBindTexture(GL_TEXTURE_2D, 0);
		glUseProgram(0);
	}

	//Render
	void Core::renderInit(int frameBufferID, int shaderID)
	{
		//Get Objects
		FrameBuffer* f = m_framebuffers[frameBufferID];
		Shader* s = m_shaders[shaderID];

		//Width and Height
		int width = Input::Window_Width;
		int height = Input::Window_Height;

		//Update FrameBuffer
		f->update(width, height);

		//Binding
		glBindFramebuffer(GL_FRAMEBUFFER, f->getID());
		glViewport(0, 0, width, height);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		glUseProgram(s->getID());
	}

	//Render Model
	void Core::renderModel(int shaderID, int modelID)
	{
		//Get Objects
		Shader* s = m_shaders[shaderID];
		Model* m = m_models[modelID];
		
		//Render
		s->setUniformMatrix4((GLchar*)"ProjViewModel", m_projViewMatrix * m->getModelRotationMatrix());
		s->setUniformMatrix4((GLchar*)"ModelRotationMatrix", m->getModelRotationMatrix());
		s->setUniformMatrix4((GLchar*)"RotationMatrix", m->getRotationMatrix());
		m->render();
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

	//Get Projection View Matrix
	glm::mat4 Core::getProjView() const
	{
		return m_projViewMatrix;
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

	//
}