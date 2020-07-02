#include "Texture.h"

#define STB_IMAGE_IMPLEMENTATION
#include "stb/stb_image.h"

#define STB_IMAGE_WRITE_IMPLEMENTATION
#include "stb/stb_image_write.h"

#include "Core.h"

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
		glDeleteTextures(1, &m_id);
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

		//Parameters
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

		//Mipmap
		glGenerateMipmap(GL_TEXTURE_2D);

		//Unbind
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


	//Fill
	void Texture::genFilled(unsigned int width, unsigned int height, unsigned int nChannel, float value)
	{
		//Generate Array
		float* dataArray = new float[width * height * nChannel];
		for (int i = 0; i < width * height * nChannel; i++)
			dataArray[i] = value;
		
		//Use Data Array
		genFromDataArray(width, height, nChannel, dataArray);
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

	//Update
	void Texture::update()
	{
		glBindTexture(GL_TEXTURE_2D, m_id);

		if (m_nChannel == 4)
			glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, m_w, m_h, GL_RGBA, GL_FLOAT, m_data);
		else if (m_nChannel == 3)
			glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, m_w, m_h, GL_RGB, GL_FLOAT, m_data);
		else if (m_nChannel == 1)
			glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, m_w, m_h, GL_RED, GL_FLOAT, m_data);

		glBindTexture(GL_TEXTURE_2D, 0);
	}

	//Convolution
	void Texture::conv(unsigned int size, float* matrix, float coef)
	{
		//Modified data array
		float* modData = new float[m_w * m_h * m_nChannel];

		//Size divided by 2
		int s2 = (int)(size / 2);

		//Y
		for (unsigned int y = 0; y < m_h; y++)
		{
			//X
			for (unsigned int x = 0; x < m_w; x++)
			{
				//Color Channel
				for (unsigned int c = 0; c < m_nChannel; c++)
				{
					//Not in border pixel
					if (x > 0 && y > 0 && x < m_w - 1 && y < m_h - 1)
					{
						//Value
						float val = 0;

						//Convolution Sum (V = y kernel, U = x kernel)
						for (unsigned int v = 0; v < size; v++)
						{
							for (unsigned int u = 0; u < size; u++)
								val += matrix[(u * size) + v] * getPixel(x - s2 + u,y - s2 + v,c);
						}

						//Multiply by coefficient to Set
						modData[getArrayPosition(x, y, c) ] = val * coef;
					}
					//In border = same value
					else
						modData[getArrayPosition(x, y, c)] = getPixel(x, y, c);
				}

				//
			}
		}

		//Set
		delete m_data;
		m_data = modData;
	}

	//Get Pixel
	float Texture::getPixel(float x, float y, unsigned int channel) const
	{
		return m_data[getArrayPosition(x, y, channel)];
	}

	//Set Pixel
	void Texture::setPixel(float x, float y, unsigned int channel, float value)
	{
		//Check minimum and maximum
		if (value < 0)
			value = 0;
		else if (value > 1)
			value = 1;

		m_data[getArrayPosition(x,y,channel)] = value;
	}

	//Apply a Transform
	void Texture::applyTransform(unsigned int id, float* args)
	{
		switch (id)
		{
		case TEXTF_PERLIN:
			applyPerlinNoise((unsigned int) args[0], args[1], args[2], args[3] );
			break;
		case TEXTF_BORDER:
			border((unsigned int)args[0], args[1]);
			break;
		default:
			break;
		}
	}

	//Apply Perlin Noise
	void Texture::applyPerlinNoise(unsigned int seed, float step, float ratio, float mult)
	{
		//Create Perlin Noise
		PerlinNoise* pn = new PerlinNoise(seed);

		//Foreach pixel
		for (unsigned int x = 0; x < m_w; x++)
		{
			for (unsigned int y = 0; y < m_h; y++)
			{
				//Compute value
				float val = 1 * pn->noise( (double) x * step / (double) m_w,  (double) y * step / (double) m_h, 0);
				val = mult * ( (1.0f - ratio) + (val * ratio) );

				//Multiply
				for (unsigned int c = 0; c < m_nChannel; c++)
					setPixel(x, y, c, getPixel(x, y, c) * val);
			}

		}

		//Clear
		delete pn;
	}

	//Paint border
	void Texture::border(unsigned int size, float value)
	{
		//Top Lines
		for (unsigned int y = 0; y < size; y++)
		{
			for (unsigned int x = 0; x < m_w; x++)
			{
				for (unsigned int c = 0; c < m_nChannel; c++)
					setPixel(x, y, c, value);
			}
		}

		//Bottom Lines
		for (unsigned int y = m_h - size; y < m_h; y++)
		{
			for (unsigned int x = 0; x < m_w; x++)
			{
				for (unsigned int c = 0; c < m_nChannel; c++)
					setPixel(x, y, c, value);
			}
		}

		//Left Lines
		for (unsigned int x = 0; x < size; x++)
		{
			for (unsigned int y = 0; y < m_h; y++)
			{
				for (unsigned int c = 0; c < m_nChannel; c++)
					setPixel(x, y, c, value);
			}
		}

		//Right Lines
		for (unsigned int x = m_w - size; x < m_w; x++)
		{
			for (unsigned int y = 0; y < m_h; y++)
			{
				for (unsigned int c = 0; c < m_nChannel; c++)
					setPixel(x, y, c, value);
			}
		}
	}

	//Get Data Array Position
	unsigned int Texture::getArrayPosition(float x, float y, unsigned int channel) const
	{
		//If negative X / Y
		if (x < 0)
			x += m_w;
		if (y < 0)
			y += m_h;

		//Modulos
		x = (int) x % m_w;
		y = (int) y % m_h;
		

		//Return
		return (unsigned int) ((((y * m_w) + x) * m_nChannel) + channel);
	}

	//
}