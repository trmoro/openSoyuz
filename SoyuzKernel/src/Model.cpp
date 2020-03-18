#include "Model.h"

namespace SK
{
	//Constructor
	Model::Model()
	{
		m_meshes = std::vector<Mesh*>();

		m_position = glm::vec3(0, 0, 0);
		m_rotation = glm::vec3(0, 0, 0);
		m_scale = glm::vec3(1, 1, 1);

	}

	//Create Mesh
	int Model::createMesh()
	{
		Mesh* ms = new Mesh();
		int i = (int)m_meshes.size();
		m_meshes.push_back(ms);
		return i;
	}

	//Prepare Memory
	void Model::meshPrepareMemory(int meshID, unsigned int nVertex, unsigned int nIndex)
	{
		m_meshes[meshID]->prepareMemory(nVertex, nIndex);
	}

	//Add Vertex
	void Model::meshAddVertex(int meshID, float x, float y, float z, float nX, float nY, float nZ, float uvX, float uvY)
	{
		m_meshes[meshID]->addVertex(x, y, z, nX, nY, nZ, uvX, uvY);
	}

	//Add Index
	void Model::meshAddIndex(int meshID, int i)
	{
		m_meshes[meshID]->addIndex(i);
	}

	//Compile Mesh
	void Model::meshCompile(int meshID)
	{
		m_meshes[meshID]->compile();
	}


	//Render
	void Model::render()
	{
		for (Mesh* m : m_meshes)
			m->render();
	}

	//Set Position
	void Model::setPosition(glm::vec3 pos)
	{
		m_position = pos;
	}

	//Set Rotation
	void Model::setRotation(glm::vec3 rot)
	{
		m_rotation = rot;
	}

	//Set Scale
	void Model::setScale(glm::vec3 scale)
	{
		m_scale = scale;
	}

	//Get Model Rotation Matrix
	glm::mat4 Model::getModelRotationMatrix() const
	{
		glm::mat4 modelMatrix = glm::translate(glm::mat4(1.0f), m_position);
		modelMatrix = glm::scale(modelMatrix, m_scale);
		modelMatrix = glm::rotate(modelMatrix, m_rotation.x, glm::vec3(1.0f, 0.0f, 0.0f));
		modelMatrix = glm::rotate(modelMatrix, m_rotation.y, glm::vec3(0.0f, 1.0f, 0.0f));
		modelMatrix = glm::rotate(modelMatrix, m_rotation.z, glm::vec3(0.0f, 0.0f, 1.0f));

		return modelMatrix;
	}

	//Get Rotation Matrix
	glm::mat4 Model::getRotationMatrix() const
	{
		glm::mat4 rotationMatrix = glm::rotate(glm::mat4(1.0f), m_rotation.x, glm::vec3(1.0f, 0.0f, 0.0f));
		rotationMatrix = glm::rotate(rotationMatrix, m_rotation.y, glm::vec3(0.0f, 1.0f, 0.0f));
		rotationMatrix = glm::rotate(rotationMatrix, m_rotation.z, glm::vec3(0.0f, 0.0f, 1.0f));

		return rotationMatrix;
	}

	//
}