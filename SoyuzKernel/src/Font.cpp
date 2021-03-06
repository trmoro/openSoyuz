#include "Font.h"

namespace SK
{
	//FreeType Library
	FT_Library Font::fLib(0);

	//Create
	Font::Font(Log* log)
	{
		//Log
		m_log = log;

		//Value
		m_width = 0;
		m_height = 0;
		m_tallest = 0;
		m_texture = 0;
	}

	//Delete
	Font::~Font()
	{
		glDeleteTextures(1, &m_texture);
		std::map<unsigned int, glyph*>::iterator it;
		for (it = m_glyphs.begin(); it != m_glyphs.end(); it++)
			delete it->second;
		m_glyphs.clear();
	}

	//Load
	void Font::load(std::string path, unsigned int size, unsigned int start, unsigned int end)
	{

		//Initialize Face
		FT_Face face;
		if (FT_New_Face(fLib, path.c_str(), 0, &face))
			m_log->add("FreeType : Failed to load font", LOG_ERROR);
		FT_Set_Pixel_Sizes(face, 0, size);

		//Init Variable
		m_width = 0;
		m_height = 0;
		m_tallest = 0;

		m_glyphs = std::map<unsigned int, glyph*>();
		std::vector<unsigned char> glyphsData = std::vector<unsigned char>();

		//First Pass : Determine Width and Height
		for (unsigned int i = start; i <= end; i++)
		{
			//Load
			if (FT_Load_Glyph(face, FT_Get_Char_Index(face, i), FT_LOAD_RENDER))
				m_log->add("FreeType : Failed to load Glyph", LOG_ERROR);
			if (FT_Render_Glyph(face->glyph, FT_RENDER_MODE_NORMAL))
				m_log->add("FreeType : Failed to render Glyph", LOG_ERROR);

			//Set Width, Height and Tallest
			if (m_width < face->glyph->bitmap.width)
				m_width = face->glyph->bitmap.width;
			if (m_tallest < face->glyph->bitmap.rows)
				m_tallest = face->glyph->bitmap.rows;
			m_height += face->glyph->bitmap.rows;

			glyph* g = new glyph{ 0, (int) face->glyph->bitmap.width, (int) face->glyph->bitmap.rows, face->glyph->bitmap.width, face->glyph->bitmap.rows, face->glyph->bitmap_left, face->glyph->advance.x / 64, face->glyph->bitmap_top };
			m_glyphs[i] = g;

			//std::cout << "Index " << g.id << " : ";
			for (unsigned int a = 0; a < (g->width * g->height); a++)
				glyphsData.push_back(*(face->glyph->bitmap.buffer + a));
		}

		//Second Pass : Generate Data
		unsigned char* fontData = new unsigned char[m_width * m_height];
		for (unsigned int i = 0; i < (m_width * m_height); i++)
			fontData[i] = 0;

		unsigned int z = 0;
		unsigned int iData = 0;
		for (unsigned int i = 0; i < m_glyphs.size(); i++)
		{
			//Draw on texture
			glyph* g = m_glyphs[i];
			for (unsigned int y = 0; y < g->height; y++)
			{
				for (unsigned int x = 0; x < g->width; x++)
				{
					//Insert
					fontData[x + ((y + z) * m_width)] = glyphsData.at(iData);
					iData++;
				}
			}
			//Update Glyph Data
			g->uvY = z;
			g->yOffset = m_tallest - g->yOffset;

			z += g->height;
		}

		//We're done with freetype
		FT_Done_Face(face);

		//Storing Mode
		glPixelStorei(GL_UNPACK_ALIGNMENT, 1);

		//Generate Texture
		glGenTextures(1, &m_texture);
		glBindTexture(GL_TEXTURE_2D, m_texture);
		glTexImage2D(GL_TEXTURE_2D, 0, GL_RED, m_width, m_height, 0, GL_RED, GL_UNSIGNED_BYTE, fontData);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
		glBindTexture(GL_TEXTURE_2D, 0);

		//Initial Storing Mode
		glPixelStorei(GL_UNPACK_ALIGNMENT, 4);

		//Clear Data
		delete[] fontData;
	}

	//Add Text as a Mesh to the given Model
	void Font::addTextAsMesh(Model* model, std::string text, float x, float y, float max_width, float lineSpacing)
	{
		//Create Mesh
		Mesh* msh = new Mesh();
		model->addHiddenMesh(msh);

		//Text Size
		unsigned int textSize = text.size();

		//Prepare Memory
		msh->prepareMemory(4 * textSize, 6 * textSize);

		//Deltas
		float dX = 0;
		float dY = 0;

		//For all char
		for (unsigned int i = 0; i < textSize; i++)
		{
			//Get Glyph
			glyph* g = m_glyphs[text[i]];

			//New line needed
			if (dX + g->xAdvance > max_width)
			{
				dX = 0;
				dY += getTallest() * lineSpacing;
			}

			//Add Vertices
			msh->addVertex(x + dX + g->xBearing, y + dY + g->yOffset, 0, 0, 0, 1, 0.0f, g->uvY);
			msh->addVertex(x + dX + g->width + g->xBearing, y + dY + g->yOffset, 0, 0, 0, 1, g->uvW, g->uvY);
			msh->addVertex(x + dX + g->width + g->xBearing, y + dY + g->height + g->yOffset, 0, 0, 0, 1, g->uvW, g->uvY + g->uvH);
			msh->addVertex(x + dX + g->xBearing, y + dY + g->height + g->yOffset, 0, 0, 0, 1, 0.0f, g->uvY + g->uvH);

			//Add Indices
			msh->addIndex(0 + (4 * i) );
			msh->addIndex(1 + (4 * i) );
			msh->addIndex(2 + (4 * i) );

			msh->addIndex(0 + (4 * i) );
			msh->addIndex(2 + (4 * i) );
			msh->addIndex(3 + (4 * i) );

			//New Letter
			dX += g->xAdvance;
		}

		//Compile
		msh->compile();
	}

	//Get Texture
	GLuint Font::getTexture() const
	{
		return m_texture;
	}

	//Get Glyph
	glyph* Font::getGlyph(unsigned int id)
	{
		return m_glyphs[id];
	}

	//Get Widht
	unsigned int Font::getWidth() const
	{
		return m_width;
	}

	//Get Height
	unsigned int Font::getHeight() const
	{
		return m_height;
	}

	//Get Tallest value
	unsigned int Font::getTallest() const
	{
		return m_tallest;
	}

	//Init FreeType
	void Font::initFreeType()
	{
		//Initialize FreeType
		if (FT_Init_FreeType(&fLib))
			std::cout << "ERROR : Could not init FreeType Library" << std::endl;
	}

	//Clear FreeType
	void Font::clearFreeType()
	{
		FT_Done_FreeType(Font::fLib);
	}
}