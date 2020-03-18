#version 330 core

in vec3 m_normal;
in vec2 m_texCoord;

layout (location = 0) out vec4 color;

void main()
{
	color = vec4(m_normal,1);
}