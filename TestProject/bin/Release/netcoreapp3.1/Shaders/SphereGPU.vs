#version 330 core

layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec2 tex;

uniform mat4 Projection;
uniform mat4 View;
uniform mat4 Model;
uniform mat4 Rotation;

void main() {
	gl_Position = vec4(position, 1.0);
}										