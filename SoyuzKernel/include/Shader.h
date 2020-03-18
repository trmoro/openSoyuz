#pragma once

#define UNIFORM_INT		0
#define UNIFORM_FLOAT	1
#define UNIFORM_VEC4	2
#define UNIFORM_MAT4	3

#include <string>

#include "GL/glew.h"
#include "glm/glm.hpp"

#include "Log.h"

namespace SK
{
	struct Uniform
	{
		std::string name;
		unsigned int type;
		
		int int_val;
		float float_val;
	};

	class Shader
	{
	public:

		//Constructor
		Shader(Log* log);

		//Set Shader with Geometry
		void set(const GLchar* srcVertex, bool geometry, const GLchar* srcGeometry, const GLchar* srcFragment);

		//Set Shader with only Vertex and Fragment
		void set(const GLchar* srcVertex, const GLchar* srcFragment);

		//ID
		GLuint getID() const;

		//Set Uniform Int
		void setUniformi(GLchar* uniform, int value) const;

		//Set Uniform Float
		void setUniformf(GLchar* uniform, float value) const;

		//Set Uniform Vec2
		void setUniformVec2(GLchar* uniform, glm::vec2 value) const;

		//Set Uniform Vec3
		void setUniformVec3(GLchar* uniform, glm::vec3 value) const;

		//Set Uniform Vec4
		void setUniformVec4(GLchar* uniform, glm::vec4 value) const;

		//Set Uniform Mat3x3
		void setUniformMatrix3(GLchar* uniform, glm::mat3 value) const;

		//Set Uniform Mat4x4
		void setUniformMatrix4(GLchar* uniform, glm::mat4 value) const;

	private:

		//Log
		Log* m_log;

		//ID
		GLuint m_id;
	
	};
}
