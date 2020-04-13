#version 330 core

layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec2 texCoord;

uniform mat4 ProjViewModel;
uniform mat4 ModelRotationMatrix;
uniform mat4 RotationMatrix;

out vec3 m_normal;
out vec2 m_texCoord;
out vec3 m_pos;

//Height of the current vertex
out float Height;

void main() {
	m_texCoord = texCoord;
	m_normal = vec3(RotationMatrix * vec4(normal, 1.0)).xyz;
	m_pos = vec3(ModelRotationMatrix * vec4(position, 1.0)).xyz;
	gl_Position = ProjViewModel * vec4(position, 1.0);
	
	Height = distance(position,vec3(0,0,0) );
}										