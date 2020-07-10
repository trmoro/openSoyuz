#include "FrameBuffer.h"

namespace SK
{
	//Constructor
	FrameBuffer::FrameBuffer(Log* log)
	{
		m_framebufferID = 0;
		m_textureID = 0;
		m_depthID = 0;

		m_width = 0;
		m_height = 0;

		m_log = log;

		//Skybox
		m_hasSkybox = false;
		m_skyboxTexture = nullptr;
	}

	//Destructor
	FrameBuffer::~FrameBuffer()
	{
		clear();
	}

	//Update
	void FrameBuffer::update(int width, int height)
	{
		//Check if we need to create a new framebuffer
		if (m_width != width || m_height != height)
		{
			//Clear
			clear();

			//Update Variable
			m_width = width;
			m_height = height;

			//Create one FrameBuffer
			glGenFramebuffers(1, &m_framebufferID);
			glBindFramebuffer(GL_FRAMEBUFFER, m_framebufferID);

			//Create Texture
			glGenTextures(1, &m_textureID);

			//Texture Settings
			glBindTexture(GL_TEXTURE_2D, m_textureID);
			glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, m_width, m_height, 0, GL_RGBA, GL_UNSIGNED_BYTE, 0);

			//Poor filtering
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
			glBindTexture(GL_TEXTURE_2D, 0);

			/*Anti-aliasing
			glBindTexture(GL_TEXTURE_2D_MULTISAMPLE, m_textureID);
			glTexImage2DMultisample(GL_TEXTURE_2D_MULTISAMPLE, MSAA, GL_RGB, m_width, m_height, GL_TRUE);
			glBindTexture(GL_TEXTURE_2D, 0);
			*/

			//Set color attachement
			glFramebufferTexture(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, m_textureID, 0);
			GLenum* drawBuffers = new GLenum[1]{ GL_COLOR_ATTACHMENT0 };

			//Depth Texture
			glGenTextures(1, &m_depthID);
			glBindTexture(GL_TEXTURE_2D, m_depthID);
			glTexImage2D(GL_TEXTURE_2D, 0, GL_DEPTH_COMPONENT24, m_width, m_height, 0, GL_DEPTH_COMPONENT, GL_FLOAT, 0);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
			glBindTexture(GL_TEXTURE_2D, 0);

			// Set draw buffers
			glDrawBuffers(1, drawBuffers);

			// Depth texture
			glFramebufferTexture(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, m_depthID, 0);

			//Unbind 
			glBindFramebuffer(GL_FRAMEBUFFER, 0);
			glBindTexture(GL_TEXTURE_2D, 0);

			//Check error
			if (glCheckFramebufferStatus(GL_FRAMEBUFFER) != GL_FRAMEBUFFER_COMPLETE)
				m_log->add("FrameBuffer is UNCOMPLETE", LOG_ERROR);
			else
				m_log->add("FrameBuffer is COMPLETE", LOG_OK);
		}
	}

	//Clear
	void FrameBuffer::clear()
	{
		glDeleteTextures(1, &m_textureID);
		glDeleteTextures(1, &m_depthID);
		glDeleteFramebuffers(1, &m_framebufferID);
	}

	//Set Skybox
	void FrameBuffer::setSkybox(Texture* t)
	{
		m_hasSkybox = true;
		m_skyboxTexture = t;
	}

	//Disable Skybox
	void FrameBuffer::disableSkybox()
	{
		m_hasSkybox = false;
	}

	//Get id
	GLuint FrameBuffer::getID() const
	{
		return m_framebufferID;
	}

	//Get Texture ID
	GLuint FrameBuffer::getTextureID() const
	{
		return m_textureID;
	}

	//Get Depth ID
	GLuint FrameBuffer::getDepthID() const
	{
		return m_depthID;
	}

	//Get Skybox Texture
	Texture* FrameBuffer::getSkyboxTexture() const
	{
		return m_skyboxTexture;
	}
	
	//Has Skybox
	bool FrameBuffer::hasSkybox() const
	{
		return m_hasSkybox;
	}
}