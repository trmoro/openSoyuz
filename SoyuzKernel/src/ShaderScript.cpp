#include "ShaderScript.h"

//Screen Vertex Script
const char* ShaderScript::Basic_Vertex = "#version 330 core													\n\
	layout(location = 0) in vec3 position;																	\n\
	layout(location = 1) in vec3 normal;																	\n\
	layout(location = 2) in vec2 texCoord;																	\n\
	uniform mat4 ProjViewModel;																				\n\
	uniform mat4 RotationMatrix;																			\n\
	out vec3 m_normal;																						\n\
	out vec2 m_texCoord;																					\n\
	void main() {																							\n\
		m_texCoord = texCoord;																				\n\
		m_normal = vec3(RotationMatrix * vec4(normal, 1.0)).xyz;											\n\
		gl_Position = ProjViewModel * vec4(position, 1.0);													\n\
	}																										";

//Screen Fragment Script
const char* ShaderScript::Screen_Fragment = "#version 330 core												\n\
	in vec3 m_normal;																						\n\
	in vec2 m_texCoord;																						\n\
	out vec4 color;																							\n\
	uniform sampler2D m_texture;																			\n\
	void main() {																							\n\
		color = texture2D(m_texture, vec2(m_texCoord.x, m_texCoord.y));										\n\
		if (color.a == 0)																					\n\
			discard;																						\n\
	}																										";

//Color Fragment Script
const char* ShaderScript::Color_Fragment = "#version 330 core												\n\
	in vec3 m_normal;																						\n\
	in vec2 m_texCoord;																						\n\
	uniform vec4 m_color;																					\n\
	out vec4 out_color;																						\n\
																											\n\
	uniform int m_isTextured;																				\n\
	uniform sampler2D m_texture;																			\n\
																											\n\
	void main() {																							\n\
		out_color = m_color;																				\n\
																											\n\
		if (m_isTextured == 1)																				\n\
			out_color *= texture2D(m_texture, m_texCoord);													\n\
	}																										";

//Normal Fragment Script
const char* ShaderScript::Normal_Fragment = "#version 330 core												\n\
	in vec3 m_normal;																						\n\
	in vec2 m_texCoord;																						\n\
	uniform vec4 m_color;																					\n\
	out vec4 color;																							\n\
	void main() {																							\n\
		color = vec4(m_normal,1) + vec4(0.5,0.5,0.5,1);														\n\
	}																										";

//Mix Fragment Script
const char* ShaderScript::Mix_Fragment = "#version 330 core													\n\
	in vec3 m_normal;																						\n\
	in vec2 m_texCoord;																						\n\
	out vec4 color;																							\n\
	uniform sampler2D m_t1;																					\n\
	uniform sampler2D m_t2;																					\n\
	uniform int m_op;																						\n\
	void main() {																							\n\
		vec4 a = texture2D(m_t1, vec2(m_texCoord.x, m_texCoord.y));											\n\
		vec4 b = texture2D(m_t2, vec2(m_texCoord.x, m_texCoord.y));											\n\
		if(m_op == 0)																						\n\
			color = a + b;																					\n\
		else if(m_op == 1)																					\n\
			color = a - b;																					\n\
		else if(m_op == 2)																					\n\
			color = a * b;																					\n\
		else if(m_op == 3)																					\n\
			color = a / b;																					\n\
		else if(m_op == 4 && (b.r > 0 || b.g > 0 || b.b > 0 || b.a > 0)	)									\n\
			color = a;																						\n\
		else if(m_op == 5)																					\n\
		{																									\n\
			if(b.r > 0)																						\n\
				color.r = a.r;																				\n\
			if(b.g > 0)																						\n\
				color.g = a.g;																				\n\
			if(b.b > 0)																						\n\
				color.b = a.b;																				\n\
			if(b.a > 0)																						\n\
				color.a = a.a;																				\n\
		}																									\n\
		else if(m_op == 6)																					\n\
			color = (b * b.a) + (a * (1 - b.a));															\n\																									\n\
		if (color.a <= 0)																					\n\
			discard;																						\n\
	}																										";

//Mix Fragment Script
const char* ShaderScript::Conv_Fragment = "#version 330 core												\n\
	in vec3 m_normal;																						\n\
	in vec2 m_texCoord;																						\n\
	out vec4 color;																							\n\
	uniform sampler2D m_texture;																			\n\
	uniform mat3 m_convMat;																					\n\
	uniform float m_convCoef;																				\n\
	void main() {																							\n\
		vec2 tex_offset = 1.0 / textureSize(m_texture, 0);													\n\
		vec4 result = texture2D(m_texture, m_texCoord) * m_convMat[1][1];									\n\
																											\n\
		result += texture2D(m_texture, m_texCoord + vec2(-tex_offset.x, 0.0)) * m_convMat[0][1];			\n\
		result += texture2D(m_texture, m_texCoord + vec2(tex_offset.x, 0.0)) * m_convMat[2][1];				\n\
																											\n\
		result += texture2D(m_texture, m_texCoord + vec2(-tex_offset.x, -tex_offset.y)) * m_convMat[0][0];	\n\
		result += texture2D(m_texture, m_texCoord + vec2(0, -tex_offset.y)) * m_convMat[1][0];				\n\
		result += texture2D(m_texture, m_texCoord + vec2(tex_offset.x, -tex_offset.y)) * m_convMat[2][0];	\n\
																											\n\
		result += texture2D(m_texture, m_texCoord + vec2(-tex_offset.x, tex_offset.y)) * m_convMat[0][2];	\n\
		result += texture2D(m_texture, m_texCoord + vec2(0, tex_offset.y)) * m_convMat[1][2];				\n\
		result += texture2D(m_texture, m_texCoord + vec2(tex_offset.x, tex_offset.y)) * m_convMat[2][2];	\n\
																											\n\
		result.r *= m_convCoef;								 												\n\
		result.g *= m_convCoef;								 												\n\
		result.b *= m_convCoef;								 												\n\
		color = result;																						\n\
	}																										";

//Lighting Vertex Script
const char* ShaderScript::Lighting_Vertex = "#version 330 core												\n\
	layout(location = 0) in vec3 position;																	\n\
	layout(location = 1) in vec3 normal;																	\n\
	layout(location = 2) in vec2 texCoord;																	\n\
	uniform mat4 ProjViewModel;																				\n\
	uniform mat4 ModelRotationMatrix;																		\n\
	uniform mat4 RotationMatrix;																			\n\
	out vec3 m_normal;																						\n\
	out vec2 m_texCoord;																					\n\
	out vec3 m_pos;																							\n\
	void main() {																							\n\
		m_texCoord = texCoord;																				\n\
		m_normal = vec3(RotationMatrix * vec4(normal, 1.0)).xyz;											\n\
		m_pos = vec3(ModelRotationMatrix * vec4(position, 1.0)).xyz;										\n\
		gl_Position = ProjViewModel * vec4(position, 1.0);													\n\
	}																										";

//Lighting Fragment Script
const char* ShaderScript::Lighting_Fragment = "#version 330 core											\n\
	in vec3 m_pos;																							\n\
	in vec3 m_normal;																						\n\
	in vec2 m_texCoord;																						\n\
																											\n\
	uniform vec4 m_color;																					\n\
	uniform vec3 m_diffuse;																					\n\
	uniform vec3 m_specular;																				\n\
	uniform float m_shininess;																				\n\
																											\n\
	uniform int m_isTextured;																				\n\
	uniform sampler2D m_texture;																			\n\
																											\n\
	uniform vec3 m_camPos;																					\n\
																											\n\
	struct DirLight {																						\n\
		vec3 dir;																							\n\
		vec3 color;																							\n\
	};																										\n\
																											\n\
	struct PointLight {																						\n\
		vec3 pos;																							\n\
		vec3 color;																							\n\
		float constant;																						\n\
		float linear;																						\n\
		float quadratic;																					\n\
	};																										\n\
																											\n\
	struct SpotLight {																						\n\
		vec3 pos;																							\n\
		vec3 dir;																							\n\
		vec3 color;																							\n\
		float in_cutoff;																					\n\
		float out_cutoff;																					\n\
	};																										\n\
																											\n\
	#define N_MAX_LIGHT 8																					\n\
																											\n\
	uniform int m_nDirLight;																				\n\
	uniform DirLight m_dirLights[N_MAX_LIGHT];																\n\
																											\n\
	uniform int m_nPointLight;																				\n\
	uniform PointLight m_pointLights[N_MAX_LIGHT];															\n\
																											\n\
	uniform int m_nSpotLight;																				\n\
	uniform SpotLight m_spotLights[N_MAX_LIGHT];															\n\
																											\n\
	out vec4 out_color;																						\n\
																											\n\
	vec3 calcPhong(vec3 pos, vec3 normal, vec3 camPos, vec3 lightColor, vec3 lightDir, float intensity)		\n\
	{																										\n\
		// Ambient																							\n\
		vec3 ambient = (lightColor * m_color.rgb) * intensity;												\n\
																											\n\
		// Diffuse 																							\n\
		float diff = max(dot(normal, lightDir), 0.0);														\n\
		vec3 diffuse = (lightColor * (diff * m_diffuse)) * intensity;										\n\
																											\n\
		// Specular																							\n\
		vec3 viewDir = normalize(camPos - pos);																\n\
		vec3 reflectDir = reflect(-lightDir, normal);  														\n\
		float spec = pow(max(dot(viewDir, reflectDir), 0.0), m_shininess);									\n\
		vec3 specular = (lightColor * (spec * m_specular)) * intensity;										\n\
																											\n\
		return ambient + diffuse + specular;																\n\
	}																										\n\
																											\n\
	vec3 calcDirLight(DirLight l)																			\n\
	{																										\n\
		return calcPhong(m_pos,normalize(m_normal),m_camPos,l.color,normalize(l.dir),1.0);					\n\
	}																										\n\
																											\n\
	vec3 calcPointLight(PointLight l)																		\n\
	{																										\n\
		float distance = length(l.pos - m_pos);																\n\
		float attenuation = 1.0 / (l.constant + l.linear * distance + l.quadratic * (distance * distance)); \n\
		return calcPhong(m_pos,normalize(m_normal),m_camPos,l.color,normalize(l.pos - m_pos),attenuation);	\n\
	}																										\n\
																											\n\
	vec3 calcSpotLight(SpotLight l)																			\n\
	{																										\n\
		vec3 fragLightDir = normalize(l.pos - m_pos);														\n\
		float theta = dot(fragLightDir, normalize(-l.dir));													\n\
		if(theta > l.in_cutoff)																				\n\
		{																									\n\
			float epsilon = l.in_cutoff - l.out_cutoff;														\n\
			float intensity = clamp((theta - l.out_cutoff) / epsilon, 0.0, 1.0);							\n\
			return calcPhong(m_pos, normalize(m_normal), m_camPos, l.color, normalize(l.dir), intensity);	\n\
		}																									\n\
		else																								\n\
			return vec3(0, 0, 0);																			\n\
	}																										\n\
																											\n\
	void main() {																							\n\
		vec3 result = vec3(0, 0, 0);																		\n\
																											\n\
		for (int i = 0; i < m_nDirLight; i++)																\n\
			result += calcDirLight(m_dirLights[i]);															\n\
																											\n\
		for (int i = 0; i < m_nPointLight; i++)																\n\
			result += calcPointLight(m_pointLights[i]);														\n\
																											\n\
		for (int i = 0; i < m_nSpotLight; i++)																\n\
			result += calcSpotLight(m_spotLights[i]);														\n\
																											\n\
		out_color = vec4(result, m_color.a);																\n\
																											\n\
		if (m_isTextured == 1)																				\n\
			out_color *= texture2D(m_texture, m_texCoord);													\n\
																											\n\
		if (out_color.a <= 0)																				\n\
			discard;																						\n\
	}																										";

//Font Fragment Script
const char* ShaderScript::Gui_Fragment = "#version 330 core													\n\
	in vec3 m_normal;																						\n\
	in vec2 m_texCoord;																						\n\
	uniform vec4 m_color;																					\n\
	out vec4 out_color;																						\n\
																											\n\
	uniform int m_isTextured;																				\n\
	uniform sampler2D m_texture;																			\n\
																											\n\
	void main() {																							\n\
		out_color = m_color;																				\n\
		if (m_isTextured == 1)																				\n\
			out_color *= texture2D(m_texture, m_texCoord);													\n\
	}																										";

//GUI Fragment Script
const char* ShaderScript::Font_Fragment = "#version 330 core												\n\
	in vec3 m_normal;																						\n\
	in vec2 m_texCoord;																						\n\
	uniform vec4 m_color;																					\n\
	out vec4 out_color;																						\n\
																											\n\
	uniform sampler2D m_font;																				\n\
																											\n\
	void main() {																							\n\
		out_color = texelFetch(m_font, ivec2(m_texCoord.xy),0);												\n\
		out_color.g = out_color.r;																			\n\
		out_color.b = out_color.r;																			\n\
		out_color.a = 1;																					\n\
		out_color *= m_color;																				 \n\
		if(out_color.r == 0)																				\n\
			discard;																						\n\
	}																										";

//Reverse Fragment Script
const char* ShaderScript::Reverse_Fragment = "#version 330 core												\n\
	in vec3 m_normal;																						\n\
	in vec2 m_texCoord;																						\n\
	out vec4 out_color;																						\n\
	uniform sampler2D m_source;																				\n\
	uniform int m_op;																						\n\
	void main() {																							\n\
		vec2 uv = m_texCoord;																				\n\
		if(m_op == 0)																						\n\
			uv.x = 1 - uv.x;																				\n\
		else if(m_op == 1)																					\n\
			uv.y = 1 - uv.y;																				\n\
		else if(m_op == 2)																					\n\
		{																									\n\
			uv.x = 1 - uv.x;																				\n\
			uv.y = 1 - uv.y;																				\n\
		}																									\n\
		out_color = texture2D(m_source, uv);																\n\
		if (out_color.a <= 0)																				\n\
			discard;																						\n\
	}																										";