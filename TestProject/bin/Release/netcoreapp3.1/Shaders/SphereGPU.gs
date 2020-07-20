#version 330 core
#define Pi 3.1415926535897932384626433832795

layout(points) in;
layout(triangle_strip, max_vertices = 100) out;

uniform mat4 Projection;
uniform mat4 View;
uniform mat4 Model;
uniform mat4 Rotation;

//To Fragment
out vec3 m_pos;
out vec3 m_normal;
out vec2 m_texCoord;

// 2D UV to XYZ Vec3
vec3 uvToVec3(vec2 uv)
{
	return vec3(cos(uv.x) * sin(uv.y), sin(uv.y), sin(uv.x) * cos(uv.y) );
}

void main() 
{	
	//Foreach Meridian
	for(int m = 0; m < 5; m++)
	{
		//Foreach Parallel
		for(int p = 0; p < 5; p++)
		{
			//UV of Quad
			vec2 uvs[] = vec2[](vec2(m / 5, p / 5), vec2( (m+1) / 5, p / 5), vec2(m / 5, (p+1) / 5), vec2( (m+1) / 5, (p+1) / 5) );
			
			//Quad Vertex
			for(int i = 0; i < 4; i++)
			{
				vec3 vrt = uvToVec3(vec2(uvs[i].x * 2 * Pi, (uvs[i].y*Pi)-(Pi/2.0) ) );
				
				m_pos = vec3(Model * vec4(vrt,1.0) );
				m_texCoord = uvs[i];
				m_normal = vec3(Rotation * vec4(vrt,1.0) );
				gl_Position = Projection * View * Model * vec4(vrt,1.0);
				
				EmitVertex();
			}
					
			//Quad Primitive
			EndPrimitive();
		}
	}
}										