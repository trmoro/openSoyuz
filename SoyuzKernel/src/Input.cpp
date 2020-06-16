#include "Input.h"

#include <iostream>

namespace SK
{
	//Window
	int Input::Window_Height	= 0;
	int Input::Window_Width		= 0;

	//Key
	int Input::Key_Char		= 0;
	int Input::Key_Id		= 0;
	int Input::Key_Scancode = 0;
	int Input::Key_Action	= 0;
	int Input::Key_Mods		= 0;

	//Mouse Position
	double Input::Mouse_X = 0;
	double Input::Mouse_Y = 0;

	//Mouse Click
	int Input::Mouse_Button = -1;
	int Input::Mouse_Action = 0;

	//GLFW Key Callback
	void Input::keyCallback(GLFWwindow* window, int key, int scancode, int action, int mods)
	{
		Input::Key_Id = key;
		Input::Key_Scancode = scancode;
		Input::Key_Action = action;
		Input::Key_Mods = mods;
	}

	//GLFW Character Key Callback
	void Input::characterCallback(GLFWwindow* window, unsigned int codepoint)
	{
		Input::Key_Char = codepoint;
	}

	//GLFW Window Size Callback
	void Input::windowSizeCallback(GLFWwindow* window, int width, int height)
	{
		Input::Window_Height = height;
		Input::Window_Width = width;
	}

	//GLFW Cursor Position Callback
	void Input::cursorPositionCallback(GLFWwindow* window, double xpos, double ypos)
	{
		Input::Mouse_X = xpos;
		Input::Mouse_Y = ypos;
	}

	//GLFW Mouse Button Callback
	void Input::mouseButtonCallback(GLFWwindow* window, int button, int action, int mods)
	{
		Input::Mouse_Button = button;
		Input::Mouse_Action = action;
	}

	//GLFW Scroll Callback
	void Input::scrollCallback(GLFWwindow* window, double xoffset, double yoffset)
	{
	}

	//GLFW Drop Callback
	void Input::dropCallback(GLFWwindow* window, int count, const char** paths)
	{
	}
}