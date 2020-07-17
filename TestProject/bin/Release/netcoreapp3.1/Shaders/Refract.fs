#version 330 core

in vec3 m_pos;
in vec3 m_normal;

uniform samplerCube m_texture;
uniform vec3 m_camPos;

out vec4 out_color;

void main() {
	
	float ratio = 1.00 / 1.309;
	
	vec3 I = normalize(m_pos - m_camPos);
    vec3 R = refract(I, normalize(m_normal), ratio );
	
    vec3 color = texture(m_texture, R).rgb;
	out_color = vec4(color,1.0);
}