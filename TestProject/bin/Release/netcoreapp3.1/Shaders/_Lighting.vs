#version 330 core

layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec2 texCoord;

uniform mat4 Projection;
uniform mat4 View;
uniform mat4 Model;
uniform mat4 ModelRotation;

out vec3 m_normal;
out vec2 m_texCoord;
out vec3 m_pos;

void main() {
	m_texCoord = texCoord;
	m_normal = vec3(ModelRotation * vec4(normal, 1.0)).xyz;
	m_pos = vec3(Model * vec4(position, 1.0)).xyz;
	gl_Position = Projection * View * Model * vec4(position, 1.0);
}										