#version 330 core

layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;

uniform mat4 Projection;
uniform mat4 View;
uniform mat4 Model;
uniform mat4 Rotation;

out vec3 m_pos;
out vec3 m_normal;

void main() {
	
	m_normal = vec3(Model * vec4(normal, 1.0));
	m_pos = vec3(Model * vec4(position, 1.0));
    gl_Position = Projection * View * Model * vec4(position, 1.0);
}										