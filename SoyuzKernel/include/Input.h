#pragma once

#include "GLFW/glfw3.h"

namespace SK
{
	class Input
	{
	public:

		//GLFW Callbacks
		static void keyCallback(GLFWwindow* window, int key, int scancode, int action, int mods);
		static void characterCallback(GLFWwindow* window, unsigned int codepoint);
		static void windowSizeCallback(GLFWwindow* window, int width, int height);
		static void cursorPositionCallback(GLFWwindow* window, double xpos, double ypos);
		static void mouseButtonCallback(GLFWwindow* window, int button, int action, int mods);
		static void scrollCallback(GLFWwindow* window, double xoffset, double yoffset);
		static void dropCallback(GLFWwindow* window, int count, const char** paths);

		//Window Size
		static int Window_Height;
		static int Window_Width;

		//Key
		static int Key_Char;
		static int Key_Id;
		static int Key_Scancode;
		static int Key_Action;
		static int Key_Mods;

		//Mouse Position
		static double Mouse_X;
		static double Mouse_Y;

		//Mouse Click
		static int Mouse_Button;
		static int Mouse_Action;
	};
}
