#include "Log.h"

namespace SK
{
	//Constructor
	Log::Log()
	{
		m_logs = std::vector<std::string>();
		m_logsID = std::vector<unsigned int>();
	}

	//Destructor
	Log::~Log()
	{
		m_logs.clear();
		m_logsID.clear();
	}

	//Add to Log
	void Log::add(std::string msg, unsigned int id)
	{
		//Add
		m_logs.push_back(msg);
		m_logsID.push_back(id);

		//Display to Console
		std::cout << id << " : " << msg << std::endl;
	}

	//Add Mat4x4
	void Log::add(glm::mat4 mat)
	{
		std::string str = "";
		for (unsigned int x = 0; x < mat.length(); x++)
		{
			for (unsigned int y = 0; y < mat[x].length(); y++)
				str += std::to_string(mat[x][y]) + " ";
			str += "\n";
		}
		m_logs.push_back(str);
		m_logsID.push_back(LOG_MAT4);
		
		std::cout << str << std::endl;

	}

	//
}