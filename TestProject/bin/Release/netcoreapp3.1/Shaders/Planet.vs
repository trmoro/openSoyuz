#version 330 core

layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec2 texCoord;

uniform mat4 Projection;
uniform mat4 View;
uniform mat4 Model;
uniform mat4 ModelRotation;

out vec3 m_normal;

//To Geometry
out vec2 gs_texCoord;
out vec3 gs_pos;

void main() {

	gs_texCoord = texCoord;
	gs_pos = vec3(Model * vec4(position, 1.0)).xyz;

	m_normal = vec3(ModelRotation * vec4(normal, 1.0)).xyz;
		
	gl_Position = vec4(position, 1.0);
}										