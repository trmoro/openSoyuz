#pragma once

#include <map>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>

#include <assimp/Importer.hpp>
#include <assimp/scene.h>
#include <assimp/postprocess.h>

#include "Mesh.h"

#include "Log.h"

namespace SK
{
	class Model
	{
	public:
		
		//Constructor
		Model(Log* log);

		//Destructor
		~Model();

		//Add Mesh
		void addMesh(int meshID, Mesh* mesh);

		//Add "Hidden" Mesh
		//Hidden Mesh have negative id in the map
		//They are created by loading Assimp model
		void addHiddenMesh(Mesh* mesh);

		///Source https://learnopengl.com/Model-Loading/Model

		//Load with Path
		void load(const char* path);

		//Process Assimp Node
		void processNode(aiNode* node, const aiScene* scene);

		//Process Assimp Mesh
		void processMesh(aiMesh* mesh);

		//Render
		void render();

		//Setters
		void setPosition(glm::vec3 pos);
		void setRotation(glm::vec3 rot);
		void setScale(glm::vec3 scale);

		//Compute Matrices
		glm::mat4 getModelRotationMatrix() const;
		glm::mat4 getRotationMatrix() const;
	
		//Delete Mesh
		void deleteMesh(Mesh* mesh);

		//Delete Hiddden Meshes
		void deleteHiddenMeshes();

		//Dicard Meshes
		void discardMeshes();

		//Delete Meshes
		void deleteMeshes();

	private:

		//Meshes
		std::map<int,Mesh*> m_meshes;
		
		//Vector3D
		glm::vec3 m_position;
		glm::vec3 m_rotation;
		glm::vec3 m_scale;

		//Log
		Log* m_log;
	};
}