#pragma once

#include "GL/glew.h"

namespace SK
{

	class Mesh
	{
	public:
		//Constructor
		Mesh();

		//Destructor
		~Mesh();

		//Prepare Memory
		void prepareMemory(unsigned int nVertex, unsigned int nIndex);

		//Add Vertex
		void addVertex(float x, float y, float z, float nX, float nY, float nZ, float uvX, float uvY);

		//Add Index
		void addIndex(int i);

		//Compile
		void compile();

		//Render
		void render();

		//Set Draw Mode
		void setDrawMode(unsigned int drawmode);

		//Transform as Skybox Mesh
		void transformAsSkybox();

	private:

		//Data
		GLfloat* m_vertices;
		GLuint*  m_indices;

		//VAO, VBO and EBO
		GLuint m_vao, m_vbo, m_ebo;

		//Is Compiled ?
		bool m_isCompiled;

		//Number of Element Added
		int m_nVertex;
		int m_nIndex;

		//Drawing Mode
		unsigned int m_drawmode;

		//Is Skybox
		bool m_isSkybox;

	};

}