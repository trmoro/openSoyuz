#include "Model.h"

#include "Core.h"

namespace SK
{
	//Constructor
	Model::Model(Log* log)
	{
		m_log = log;

		m_meshes = std::map<int,Mesh*>();

		m_position = glm::vec3(0, 0, 0);
		m_rotation = glm::vec3(0, 0, 0);
		m_scale = glm::vec3(1, 1, 1);
	}

	//Delete Model
	Model::~Model()
	{
		
	}

	//Add Mesh
	void Model::addMesh(int meshID, Mesh* mesh)
	{
		m_meshes[meshID] = mesh;
	}

	//Add Hidden Mesh
	void Model::addHiddenMesh(Mesh* mesh)
	{
		m_meshes[m_meshes.size() * -1] = mesh;
	}

	//Load with path
	void Model::load(const char* path)
	{
		Assimp::Importer import;
		const aiScene* scene = import.ReadFile(path, aiProcess_Triangulate | aiProcess_FlipUVs);

		if (!scene || scene->mFlags & AI_SCENE_FLAGS_INCOMPLETE || !scene->mRootNode)
		{
			m_log->add(import.GetErrorString(),LOG_ERROR);
			return;
		}

		//Process "Root node"
		processNode(scene->mRootNode, scene);
	}

	//Process Assimp Node
	void Model::processNode(aiNode* node, const aiScene* scene)
	{
		// process all the node's meshes (if any)
		for (unsigned int i = 0; i < node->mNumMeshes; i++)
			processMesh(scene->mMeshes[node->mMeshes[i]]);
		// then do the same for each of its children
		for (unsigned int i = 0; i < node->mNumChildren; i++)
			processNode(node->mChildren[i], scene);
	}

	//Process Assimp Mesh
	void Model::processMesh(aiMesh* mesh)
	{
		//Create Mesh, add it to model ( BUT NOT TO Core )
		Mesh* msh = new Mesh();
		addHiddenMesh(msh);

		//Prepare Mesh Memory (triangle faces needed)
		msh->prepareMemory(mesh->mNumVertices, mesh->mNumFaces * 3);

		//Add Vertices
		for (unsigned int i = 0; i < mesh->mNumVertices; i++)
		{
			glm::vec3 pos = glm::vec3(mesh->mVertices[i].x, mesh->mVertices[i].y, mesh->mVertices[i].z);
			glm::vec3 nor = glm::vec3(mesh->mNormals[i].x, mesh->mNormals[i].y, mesh->mNormals[i].z);

			glm::vec2 tex = glm::vec2(0, 0);
			if (mesh->mTextureCoords[0])
				tex = glm::vec2(mesh->mTextureCoords[0][i].x, mesh->mTextureCoords[0][i].y);

			msh->addVertex(pos.x, pos.y, pos.z, nor.x, nor.y, nor.z, tex.x, tex.y);
		}

		//Indices
		for (unsigned int i = 0; i < mesh->mNumFaces; i++)
		{
			aiFace face = mesh->mFaces[i];
			for (unsigned int j = 0; j < face.mNumIndices; j++)
				msh->addIndex(face.mIndices[j]);
		}

		//Compile
		msh->compile();
	}

	//Render
	void Model::render()
	{
		std::map<int, Mesh*>::iterator it;
		for (it = m_meshes.begin(); it != m_meshes.end(); it++)
		{
			if(it->second != nullptr)
				it->second->render();
		}
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

	//Delete Mesh
	void Model::deleteMesh(Mesh* mesh)
	{
		std::map<int, Mesh*>::iterator it;
		for (it = m_meshes.begin(); it != m_meshes.end(); it++)
		{
			if (it->second != nullptr && it->second == mesh)
				it->second = nullptr;
		}

	}

	//
}