#include "Shader.h"

namespace SK
{
	//Constructor
	Shader::Shader(Log* log)
	{
		m_log = log;
		m_id = 0;
	}

	//Destructor
	Shader::~Shader()
	{
		glDeleteShader(m_id);
	}

	//Set with Geometry
	void Shader::set(const GLchar* srcVertex, bool geometry, const GLchar* srcGeometry, const GLchar* srcFragment)
	{
		//Vertex shader
		GLuint vertexShader = glCreateShader(GL_VERTEX_SHADER);
		glShaderSource(vertexShader, 1, &srcVertex, NULL);
		glCompileShader(vertexShader);

		//Check for compile time errors
		GLint success;
		GLchar infoLog[512];
		glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success);
		if (!success)
		{
			glGetShaderInfoLog(vertexShader, 512, NULL, infoLog);
			m_log->add("Vertex Shader Compilation FAILED\n" + std::string(infoLog), LOG_ERROR );
		}

		//Geometry shader
		GLuint geometryShader = 0;
		if (geometry)
		{
			geometryShader = glCreateShader(GL_GEOMETRY_SHADER);
			glShaderSource(geometryShader, 1, &srcGeometry, NULL);
			glCompileShader(geometryShader);

			//Check for compile time errors
			glGetShaderiv(geometryShader, GL_COMPILE_STATUS, &success);
			if (!success)
			{
				glGetShaderInfoLog(geometryShader, 512, NULL, infoLog);
				m_log->add("Geometry Shader Compilation FAILED\n" + std::string(infoLog), LOG_ERROR);
			}
		}

		//Fragment shader
		GLuint fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
		glShaderSource(fragmentShader, 1, &srcFragment, NULL);
		glCompileShader(fragmentShader);

		//Check for compile time errors
		glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &success);
		if (!success)
		{
			glGetShaderInfoLog(fragmentShader, 512, NULL, infoLog);
			m_log->add("Fragment Shader Compilation FAILED\n" + std::string(infoLog), LOG_ERROR);
		}

		//Link shaders
		GLuint shaderProgram = glCreateProgram();
		glAttachShader(shaderProgram, vertexShader);
		if(geometry)
			glAttachShader(shaderProgram, geometryShader);
		glAttachShader(shaderProgram, fragmentShader);
		glLinkProgram(shaderProgram);

		//Check for linking errors
		glGetProgramiv(shaderProgram, GL_LINK_STATUS, &success);
		if (!success)
		{
			glGetProgramInfoLog(shaderProgram, 512, NULL, infoLog);
			m_log->add("Linking FAILED\n" + std::string(infoLog), LOG_ERROR);
		}

		//Delete Shader
		glDeleteShader(vertexShader);
		glDeleteShader(geometryShader);
		glDeleteShader(fragmentShader);

		m_log->add("Shader Compilation OK", LOG_OK);

		//Return Program
		m_id = shaderProgram;
	}

	//Set shader with only Vertex and Fragment
	void Shader::set(const GLchar* srcVertex, const GLchar* srcFragment)
	{
		set(srcVertex, false, "", srcFragment);
	}

	//Get ID
	GLuint Shader::getID() const
	{
		return m_id;
	}

	//Set Uniform for Int
	void Shader::setUniformi(GLchar* uniform, int value) const
	{
		GLuint loc = glGetUniformLocation(m_id, uniform);
		glUniform1i(loc, value);
	}

	//Set Uniform for Float
	void Shader::setUniformf(GLchar* uniform, float value) const
	{
		GLuint loc = glGetUniformLocation(m_id, uniform);
		glUniform1f(loc, value);
	}

	//Vector 2
	void Shader::setUniformVec2(GLchar* uniform, glm::vec2 value) const
	{
		GLuint loc = glGetUniformLocation(m_id, uniform);
		glUniform2f(loc, value.x, value.y);
	}

	//Vector 3
	void Shader::setUniformVec3(GLchar* uniform, glm::vec3 value) const
	{
		GLuint loc = glGetUniformLocation(m_id, uniform);
		glUniform3f(loc, value.x, value.y, value.z);
	}

	//Set Uniform for vec4
	void Shader::setUniformVec4(GLchar* uniform, glm::vec4 value) const
	{
		GLuint loc = glGetUniformLocation(m_id, uniform);
		glUniform4f(loc, value.x, value.y, value.z, value.w);
	}

	//Set Uniform for Matrix3x3
	void Shader::setUniformMatrix3(GLchar* uniform, glm::mat3 value) const
	{
		GLuint matrixID = glGetUniformLocation(m_id, uniform);
		glUniformMatrix3fv(matrixID, 1, GL_FALSE, &value[0][0]);
	}

	//Set Uniform for Matrix4x4
	void Shader::setUniformMatrix4(GLchar* uniform, glm::mat4 value) const
	{
		GLuint matrixID = glGetUniformLocation(m_id, uniform);
		glUniformMatrix4fv(matrixID, 1, GL_FALSE, &value[0][0]);
	}
}