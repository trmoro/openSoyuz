#pragma once

#include "GL/glew.h"

#include "Log.h"

#include "Texture.h"

namespace SK
{
	class FrameBuffer
	{
	public:
		
		//Constructor	
		FrameBuffer(Log* log);

		//Destructor
		~FrameBuffer();
	
		//Update
		void update(int width, int height);

		//Clear
		void clear();

		//Set Skybox
		void setSkybox(Texture* t);

		//Disable Skybox
		void disableSkybox();

		//Get ID
		GLuint getID() const;

		//Get Texture ID
		GLuint getTextureID() const;

		//Get Depth ID
		GLuint getDepthID() const;

		//Get Skybox Texture
		Texture* getSkyboxTexture() const;

		//Has Skybox
		bool hasSkybox() const;

	private:

		//IDS
		GLuint m_framebufferID;
		GLuint m_textureID;
		GLuint m_depthID;

		//Width and Height
		int m_width;
		int m_height;

		//Log
		Log* m_log;

		//Skybox
		bool m_hasSkybox;
		Texture* m_skyboxTexture;

	};
}
