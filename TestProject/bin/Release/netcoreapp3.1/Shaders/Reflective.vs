#version 330 core

layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec2 texCoord;

uniform mat4 Projection;
uniform mat4 ViewNoTranslation;
uniform mat4 Model;
uniform mat4 ModelRotation;

out vec3 m_pos;
out vec3 m_normal;

void main() {

	m_normal = mat3(transpose(ModelRotation)) * normal;
    m_pos = vec3(Model * vec4(position, 1.0));
		
	gl_Position = Projection * ViewNoTranslation * Model * vec4(position, 1.0);
}										