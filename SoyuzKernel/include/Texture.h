#pragma once

#include "GL/glew.h"

#include "Log.h"

namespace SK
{
	class Texture
	{
	public:

		//Constructor
		Texture(Log* log);

		//Generate from data array
		void genFromDataArray(unsigned int width, unsigned int height, unsigned int nChannel, float* data);

		//Generate from source path
		void genFromPath(const char* path, unsigned int nChannel);

		//Get ID
		GLuint getID() const;

		//Get Width
		unsigned int getWidth() const;
		
		//Get Height
		unsigned int getHeight() const;

		//Get Number Of Channel
		unsigned int getNumberOfChannel() const;

	private:

		//ID
		GLuint m_id;

		//Width and Height
		unsigned int m_w;
		unsigned int m_h;

		//Number of Color Channels
		unsigned int m_nChannel;

		//Log
		Log* m_log;

	};
}