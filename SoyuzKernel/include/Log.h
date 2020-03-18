#pragma once

#define LOG_OK		0
#define LOG_ERROR	1
#define LOG_MAT4	2

#include <iostream>
#include <string>
#include <vector>

#include "glm/glm.hpp"

namespace SK
{

	class Log
	{
	public:

		//Constructor / Destructor
		Log();
		~Log();

		//Add
		void add(std::string msg, unsigned int id);

		//Add Mat4
		void add(glm::mat4 mat);
	
	private:
		std::vector<std::string> m_logs;
		std::vector<unsigned int> m_logsID;
	};
}