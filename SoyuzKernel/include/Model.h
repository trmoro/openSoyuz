#pragma once

#include <vector>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>

#include "Mesh.h"

namespace SK
{
	class Model
	{
	public:
		
		//Constructor
		Model();

		//Create Mesh
		int createMesh();

		//Prepare Memory
		void meshPrepareMemory(int meshID, unsigned int nVertex, unsigned int nIndex);

		//Add Vertex
		void meshAddVertex(int meshID, float x, float y, float z, float nX, float nY, float nZ, float uvX, float uvY);

		//Add Index
		void meshAddIndex(int meshID, int i);

		//Compile
		void meshCompile(int meshID);

		//Render
		void render();

		//Setters
		void setPosition(glm::vec3 pos);
		void setRotation(glm::vec3 rot);
		void setScale(glm::vec3 scale);

		//Compute Matrices
		glm::mat4 getModelRotationMatrix() const;
		glm::mat4 getRotationMatrix() const;
	
	private:

		//Meshes
		std::vector<Mesh*> m_meshes;
		
		//Vector3D
		glm::vec3 m_position;
		glm::vec3 m_rotation;
		glm::vec3 m_scale;

	};
}