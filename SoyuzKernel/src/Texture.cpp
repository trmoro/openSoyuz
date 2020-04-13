#include "Texture.h"

#define STB_IMAGE_IMPLEMENTATION
#include "stb/stb_image.h"

#define STB_IMAGE_WRITE_IMPLEMENTATION
#include "stb/stb_image_write.h"

namespace SK
{
	//Constructor
	Texture::Texture(Log* log)
	{
		m_log = log;
		m_id = 0;
		m_w = 1;
		m_h = 1;

		m_nChannel = 4;

		m_data = new float[1];
	}

	//Destructor
	Texture::~Texture()
	{
		delete m_data;
	}

	//Generate Texture from data array
	void Texture::genFromDataArray(unsigned int width, unsigned int height, unsigned int nChannel, float* data)
	{
		glGenTextures(1, &m_id);
		glBindTexture(GL_TEXTURE_2D, m_id);

		if(nChannel == 4)
			glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_FLOAT, data);
		else if (nChannel == 3)
			glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_FLOAT, data);
		else if (nChannel == 1)
			glTexImage2D(GL_TEXTURE_2D, 0, GL_RED, width, height, 0, GL_RED, GL_FLOAT, data);


		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
		glBindTexture(GL_TEXTURE_2D, 0);
		
		//Set variables
		m_w = width;
		m_h = height;
		m_nChannel = nChannel;
		m_data = data;
	}

	//Generate Texture from source path
	void Texture::genFromPath(const char* path, unsigned int nChannel)
	{
		int width, height, channels;
		stbi_set_flip_vertically_on_load(true);
		unsigned char* image = stbi_load(path,
			&width,
			&height,
			&channels,
			nChannel);

		float* data = new float[width * height * nChannel];
		for (int i = 0; i < width * height * nChannel; i++)
			data[i] = ((float)image[i]) / 255.0f;
		genFromDataArray(width, height, nChannel, data);

		stbi_image_free(image);
	}

	//Get ID
	GLuint Texture::getID() const
	{
		return m_id;
	}

	//Get Width
	unsigned int Texture::getWidth() const
	{
		return m_w;
	}

	//Get Height
	unsigned int Texture::getHeight() const
	{
		return m_h;
	}

	//Get Number Of Channel
	unsigned int Texture::getNumberOfChannel() const
	{
		return m_nChannel;
	}

	//Get Data
	float* Texture::getData() const
	{
		return m_data;
	}

	//Save to PNG
	bool Texture::savePNG(std::string filePath) const
	{
		stbi_flip_vertically_on_write(1);

		unsigned char* ucData = new unsigned char[m_w * m_h * m_nChannel];
		for (unsigned int i = 0; i < m_w * m_h * m_nChannel; i++)
			ucData[i] = (unsigned char)(m_data[i] * 255.0f);
		return stbi_write_png(filePath.c_str(), m_w, m_h, m_nChannel, ucData, m_nChannel * m_w);
	}

	//
}