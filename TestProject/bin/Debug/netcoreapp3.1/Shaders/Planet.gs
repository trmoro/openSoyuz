#version 330 core

layout(triangles) in;
layout(triangle_strip, max_vertices = 3) out;

uniform sampler2D m_texture;

uniform mat4 ProjViewModel;
uniform mat4 ModelRotationMatrix;

//From Vertex
in vec2 gs_texCoord[];
in vec3 gs_pos[];

//To Fragment
out vec2 m_texCoord;
out vec3 m_pos;
out float Height;

void main() 
{	
	//Heights
	float h0 = (texture2D(m_texture, gs_texCoord[0]).r * 0.05) + 1;
	float h1 = (texture2D(m_texture, gs_texCoord[1]).r * 0.05) + 1;
	float h2 = (texture2D(m_texture, gs_texCoord[2]).r * 0.05) + 1;

	//Vertex 1
	m_texCoord = gs_texCoord[0];
	m_pos = gs_pos[0];
	Height = h0;
	gl_Position = ProjViewModel * (gl_in[0].gl_Position * vec4(h0,h0,h0,1) );
	EmitVertex();
	
	//Vertex 2
	m_texCoord = gs_texCoord[1];
	m_pos = gs_pos[1];
	Height = h1;
	gl_Position = ProjViewModel * (gl_in[1].gl_Position * vec4(h1,h1,h1,1) );
	EmitVertex();
	
	//Vertex 3
	m_texCoord = gs_texCoord[2];
	m_pos = gs_pos[2];
	Height = h2;
	gl_Position = ProjViewModel * (gl_in[2].gl_Position * vec4(h2,h2,h2,1)  );
	EmitVertex();
	
    EndPrimitive();
}										