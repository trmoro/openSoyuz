#include "Mesh.h"

#include "Core.h"

#include <iostream>

namespace SK
{
	//Mesh Constructor
	Mesh::Mesh()
	{
		m_vertices = new GLfloat[0];
		m_indices  = new GLuint[0];

		m_nVertex = 0;
		m_nIndex  = 0;

		m_isCompiled = false;

		m_vao = 0;
		m_vbo = 0;
		m_ebo = 0;

		m_drawmode = MODEL_DRAW_TRIANGLES;

		m_isSkybox = false;
	}

	//Delete Mesh
	Mesh::~Mesh()
	{
		glDeleteVertexArrays(1, &m_vao);
		glDeleteBuffers(1, &m_vbo);
		glDeleteBuffers(1, &m_ebo);
		delete m_vertices;
		delete m_indices;
	}

	//Prepare Memory
	void Mesh::prepareMemory(unsigned int nVertex, unsigned int nIndex)
	{
		//Free
		delete m_vertices;
		delete m_indices;
		m_nVertex = 0;
		m_nIndex = 0;

		//Allow
		m_vertices = new GLfloat[nVertex * 8];
		m_indices = new GLuint[nIndex];
	}

	//Add Vertex
	void Mesh::addVertex(float x, float y, float z, float nX, float nY, float nZ, float uvX, float uvY)
	{
		m_vertices[(m_nVertex * 8) + 0] = x;
		m_vertices[(m_nVertex * 8) + 1] = y;
		m_vertices[(m_nVertex * 8) + 2] = z;

		m_vertices[(m_nVertex * 8) + 3] = nX;
		m_vertices[(m_nVertex * 8) + 4] = nY;
		m_vertices[(m_nVertex * 8) + 5] = nZ;

		m_vertices[(m_nVertex * 8) + 6] = uvX;
		m_vertices[(m_nVertex * 8) + 7] = uvY;
		
		m_nVertex++;
	}

	//Add Index
	void Mesh::addIndex(int i)
	{
		m_indices[m_nIndex] = (unsigned int) i;

		m_nIndex++;
	}

	//Compile
	void Mesh::compile()
	{
		//Clear if already compile
		if (m_isCompiled)
		{
			glDeleteVertexArrays(1, &m_vao);
			glDeleteBuffers(1, &m_vbo);
			glDeleteBuffers(1, &m_ebo);
		}

		//Generate Buffer
		glGenVertexArrays(1, &m_vao);

		//Bind Vertex Array
		glBindVertexArray(m_vao);

		//Vertex Buffer
		glGenBuffers(1, &m_vbo);
		glBindBuffer(GL_ARRAY_BUFFER, m_vbo);
		glBufferData(GL_ARRAY_BUFFER, m_nVertex * 8 * sizeof(GLfloat), m_vertices, GL_STATIC_DRAW);

		//Position
		glEnableVertexAttribArray(0);
		glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (GLvoid*)0);

		//Normal
		glEnableVertexAttribArray(1);
		glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (GLvoid*)(3 * sizeof(GLfloat)));

		//Texture Coordinate
		glEnableVertexAttribArray(2);
		glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (GLvoid*)(6 * sizeof(GLfloat)));

		//Unbind Vertex Buffer
		glBindBuffer(GL_ARRAY_BUFFER, 0);

		//Indice Buffer
		glGenBuffers(1, &m_ebo);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, m_ebo);
		glBufferData(GL_ELEMENT_ARRAY_BUFFER, m_nIndex * sizeof(GLuint), m_indices, GL_STATIC_DRAW);

		//Unbind
		glBindVertexArray(0);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);

		//Set Compiled
		m_isCompiled = true;
	}

	//Render
	void Mesh::render()
	{
		if (m_isCompiled)
		{
			//Default Mesh Render
			if (!m_isSkybox)
			{
				GLenum drawmode = GL_TRIANGLES;
				switch (m_drawmode)
				{
				case MODEL_DRAW_QUADS:
					drawmode = GL_QUADS;
					break;
				case MODEL_DRAW_LINES:
					drawmode = GL_LINES;
					break;
				case MODEL_DRAW_LINE_STRIP:
					drawmode = GL_LINE_STRIP;
					break;
				case MODEL_DRAW_POINTS:
					drawmode = GL_POINTS;
					break;
				}

				//Bind VAO
				glBindVertexArray(m_vao);
				glDrawElements(drawmode, m_nIndex, GL_UNSIGNED_INT, nullptr);
				glBindVertexArray(0);
			}
			//Skybox Render
			else
			{
				glBindVertexArray(m_vao);
				glDrawArrays(GL_TRIANGLES, 0, 36);
				glBindVertexArray(0);
			}

			//
		}
	}

	//Set Drawmode
	void Mesh::setDrawMode(unsigned int drawmode)
	{
		m_drawmode = drawmode;
	}

	//Transform as a Skybox Mesh
	//Source : https://learnopengl.com/code_viewer_gh.php?code=src/4.advanced_opengl/6.1.cubemaps_skybox/cubemaps_skybox.cpp
	void Mesh::transformAsSkybox()
	{
		float skyboxVertices[] = {
			// positions          
			-1.0f,  1.0f, -1.0f,
			-1.0f, -1.0f, -1.0f,
			 1.0f, -1.0f, -1.0f,
			 1.0f, -1.0f, -1.0f,
			 1.0f,  1.0f, -1.0f,
			-1.0f,  1.0f, -1.0f,

			-1.0f, -1.0f,  1.0f,
			-1.0f, -1.0f, -1.0f,
			-1.0f,  1.0f, -1.0f,
			-1.0f,  1.0f, -1.0f,
			-1.0f,  1.0f,  1.0f,
			-1.0f, -1.0f,  1.0f,

			 1.0f, -1.0f, -1.0f,
			 1.0f, -1.0f,  1.0f,
			 1.0f,  1.0f,  1.0f,
			 1.0f,  1.0f,  1.0f,
			 1.0f,  1.0f, -1.0f,
			 1.0f, -1.0f, -1.0f,

			-1.0f, -1.0f,  1.0f,
			-1.0f,  1.0f,  1.0f,
			 1.0f,  1.0f,  1.0f,
			 1.0f,  1.0f,  1.0f,
			 1.0f, -1.0f,  1.0f,
			-1.0f, -1.0f,  1.0f,

			-1.0f,  1.0f, -1.0f,
			 1.0f,  1.0f, -1.0f,
			 1.0f,  1.0f,  1.0f,
			 1.0f,  1.0f,  1.0f,
			-1.0f,  1.0f,  1.0f,
			-1.0f,  1.0f, -1.0f,

			-1.0f, -1.0f, -1.0f,
			-1.0f, -1.0f,  1.0f,
			 1.0f, -1.0f, -1.0f,
			 1.0f, -1.0f, -1.0f,
			-1.0f, -1.0f,  1.0f,
			 1.0f, -1.0f,  1.0f
		};

		// skybox VAO
		unsigned int skyboxVAO, skyboxVBO;
		glGenVertexArrays(1, &skyboxVAO);
		glGenBuffers(1, &skyboxVBO);
		glBindVertexArray(skyboxVAO);
		glBindBuffer(GL_ARRAY_BUFFER, skyboxVBO);
		glBufferData(GL_ARRAY_BUFFER, sizeof(skyboxVertices), &skyboxVertices, GL_STATIC_DRAW);
		glEnableVertexAttribArray(0);
		glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);

		m_vao = skyboxVAO;
		m_vbo = skyboxVBO;
		m_isCompiled = true;
		m_isSkybox = true;

	}

	
}