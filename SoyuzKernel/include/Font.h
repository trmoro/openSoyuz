#pragma once

#include <iostream>
#include <map>

#include <GL/glew.h>

#define generic GenericFromFreeTypeLibrary
#include <ft2build.h>
#include FT_FREETYPE_H
#undef generic

#include "Log.h"

#include "Model.h"

namespace SK
{
	struct glyph
	{
		int uvY;
		int uvW;
		int uvH;
		unsigned int width;
		unsigned int height;
		int xBearing;
		int xAdvance;
		int yOffset;
	};

	class Font
	{
	public:

		//Constructor / Destructor
		Font(Log* log);
		~Font();

		//Load
		void load(std::string path, unsigned int size, unsigned int start, unsigned int end);

		//Add Text as Mesh to the given Model
		void addTextAsMesh(Model* model, std::string text, float x, float y, float max_width, float lineSpacing);

		//Get Texture ID
		GLuint getTexture() const;
		
		//Get Glyph
		glyph* getGlyph(unsigned int id);

		//Getter
		unsigned int getWidth() const;
		unsigned int getHeight() const;
		unsigned int getTallest() const;

		//Init and Cleaning
		static void initFreeType();
		static void clearFreeType();

	private:

		//Log
		Log* m_log;

		//Texture ID
		GLuint m_texture;

		//Sizes
		unsigned int m_width;
		unsigned int m_height;
		unsigned int m_tallest;

		//Glyphs
		std::map<unsigned int, glyph*> m_glyphs;
		
		//FreeType Instance
		static FT_Library fLib;
	};
}
