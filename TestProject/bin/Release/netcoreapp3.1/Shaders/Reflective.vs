#version 330 core

layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec2 texCoord;

uniform mat4 ProjViewModel;
uniform mat4 ModelRotationMatrix;
uniform mat4 RotationMatrix;

out vec3 m_pos;
out vec3 m_normal;

void main() {

	m_normal = mat3(transpose(inverse(ModelRotationMatrix))) * normal;
    m_pos = vec3(ModelRotationMatrix * vec4(position, 1.0));
		
	gl_Position = ProjViewModel * vec4(position, 1.0);
}										