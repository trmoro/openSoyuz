#version 330 core

in vec3 m_pos;
in vec3 m_normal;
in vec2 m_texCoord;

uniform vec4 m_color;
uniform vec3 m_diffuse;
uniform vec3 m_specular;
uniform float m_shininess;

uniform int m_isTextured;
uniform sampler2D m_texture;

uniform vec3 m_camPos;

struct DirLight {
	vec3 dir;
	vec3 color;
};

struct PointLight {
	vec3 pos;
	vec3 color;
	float constant;
	float linear;
	float quadratic;
};

struct SpotLight {
	vec3 pos;
	vec3 dir;
	vec3 color;
	float in_cutoff;
	float out_cutoff;
};

#define N_MAX_LIGHT 8

uniform int m_nDirLight;
uniform DirLight m_dirLights[N_MAX_LIGHT];

uniform int m_nPointLight;
uniform PointLight m_pointLights[N_MAX_LIGHT];

uniform int m_nSpotLight;
uniform SpotLight m_spotLights[N_MAX_LIGHT];

//Height of the current vertex
in float Height;

out vec4 out_color;

vec3 calcPhong(vec3 pos, vec3 normal, vec3 camPos, vec3 lightColor, vec3 lightDir, float intensity)
{
	// Ambient
	vec3 ambient = (lightColor * m_color.rgb) * intensity;

	// Diffuse 
	float diff = max(dot(normal, lightDir), 0.0);
	vec3 diffuse = (lightColor * (diff * m_diffuse)) * intensity;

	// Specular
	vec3 viewDir = normalize(camPos - pos);
	vec3 reflectDir = reflect(-lightDir, normal);  
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), m_shininess);
	vec3 specular = (lightColor * (spec * m_specular)) * intensity;

	return ambient + diffuse + specular;
}

vec3 calcDirLight(DirLight l)
{
	return calcPhong(m_pos,normalize(m_normal),m_camPos,l.color,normalize(l.dir),1.0);
}

vec3 calcPointLight(PointLight l)
{
	float distance = length(l.pos - m_pos);
	float attenuation = 1.0 / (l.constant + l.linear * distance + l.quadratic * (distance * distance));
	return calcPhong(m_pos,normalize(m_normal),m_camPos,l.color,normalize(l.pos - m_pos),attenuation);
}

vec3 calcSpotLight(SpotLight l)
{
	vec3 fragLightDir = normalize(l.pos - m_pos);
	float theta = dot(fragLightDir, normalize(-l.dir));
	if(theta > l.in_cutoff)
	{
		float epsilon = l.in_cutoff - l.out_cutoff;
		float intensity = clamp((theta - l.out_cutoff) / epsilon, 0.0, 1.0);
		return calcPhong(m_pos, normalize(m_normal), m_camPos, l.color, normalize(l.dir), intensity);
	}
	else
		return vec3(0, 0, 0);
}

void main() {
	vec3 result = vec3(0, 0, 0);

	for (int i = 0; i < m_nDirLight; i++)
		result += calcDirLight(m_dirLights[i]);

	for (int i = 0; i < m_nPointLight; i++)
		result += calcPointLight(m_pointLights[i]);

	for (int i = 0; i < m_nSpotLight; i++)
		result += calcSpotLight(m_spotLights[i]);

	out_color = vec4(result, 1) * m_color;
	
	out_color = vec4(1,1,1,1);
}