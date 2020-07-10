#pragma once

#include "GL/glew.h"

#include "Log.h"

#include "PerlinNoise.h"

namespace SK
{
	class Texture
	{
	public:

		//Constructor
		Texture(Log* log);

		//Destructor
		~Texture();

		//Generate from data array
		void genFromDataArray(unsigned int width, unsigned int height, unsigned int nChannel, float* data);

		//Generate from source path
		void genFromPath(const char* path, unsigned int nChannel);

		//Filled Texture
		void genFilled(unsigned int width, unsigned int height, unsigned int nChannel, float value);

		//Generate Cube Map
		void genCubemap(const char* right, const char* left, const char* top, const char* bottom, const char* front, const char* back);

		//Get ID
		GLuint getID() const;

		//Get Width
		unsigned int getWidth() const;
		
		//Get Height
		unsigned int getHeight() const;

		//Get Number Of Channel
		unsigned int getNumberOfChannel() const;

		//Get Data
		float* getData() const;

		//Save PNG
		bool savePNG(std::string filePath) const;

		//Update
		void update();

		//Convolution 3x3
		void conv(unsigned int size, float* matrix, float coef);

		//Get Pixel
		float getPixel(float x, float y, unsigned int channel) const;

		//Set Pixel
		void setPixel(float x, float y, unsigned int channel, float value);

		//Apply Transform
		void applyTransform(unsigned int id, float* args);

		//Is Cubemap
		bool isCubemap() const;

	private:

		//Get Data Array Position
		unsigned int getArrayPosition(float x, float y, unsigned int channel) const;

		//ID
		GLuint m_id;

		//Width and Height
		unsigned int m_w;
		unsigned int m_h;

		//Number of Color Channels
		unsigned int m_nChannel;

		//Data
		float* m_data;

		//Is Cube Map
		bool m_isCubeMap;

		//Log
		Log* m_log;

		///TEXTURE TRANSFORMATIONS

		//Apply Perlin Noise
		void applyPerlinNoise(unsigned int seed, float step, float ratio, float mult);

		//Border
		void border(unsigned int size, float value);

	};
}