#version 330 core

in vec3 m_pos;
in vec3 m_normal;

uniform samplerCube m_skybox;

uniform vec3 m_camPos;

out vec4 out_color;

void main() {

	vec3 I = normalize(m_pos - m_camPos);
    vec3 R = reflect(I, normalize(m_normal));
    out_color = vec4(texture(m_skybox, R).rgb, 1.0);
}