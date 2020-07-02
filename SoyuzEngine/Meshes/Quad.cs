using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Soyuz.Meshes
{
    public class Quad
    {
        /// <summary>
        /// Rectangle
        /// </summary>
        public static Mesh Rect()
        {
            //Create Mesh
            Mesh msh = new Mesh();

            msh.Positions.Add(new Vector3(0, 0, 0));
            msh.Positions.Add(new Vector3(1, 0, 0));
            msh.Positions.Add(new Vector3(1, 1, 0));
            msh.Positions.Add(new Vector3(0, 1, 0));

            msh.Normals.Add(new Vector3(0, 0, 1));
            msh.Normals.Add(new Vector3(0, 0, 1));
            msh.Normals.Add(new Vector3(0, 0, 1));
            msh.Normals.Add(new Vector3(0, 0, 1));

            msh.UVs.Add(new Vector2(0, 0));
            msh.UVs.Add(new Vector2(1, 0));
            msh.UVs.Add(new Vector2(1, 1));
            msh.UVs.Add(new Vector2(0, 1));

            msh.Indices.Add(0);
            msh.Indices.Add(1);
            msh.Indices.Add(2);

            msh.Indices.Add(0);
            msh.Indices.Add(2);
            msh.Indices.Add(3);

            //Return
            return msh;
        }

        /// <summary>
        /// Create a Cube Mesh
        /// </summary>
        public static Mesh Cube()
        {
            //Create Mesh
            Mesh msh = new Mesh();

            msh.Positions.Add(new Vector3(-0.5f, -0.5f, -0.5f));
            msh.Positions.Add(new Vector3(0.5f, -0.5f, -0.5f));
            msh.Positions.Add(new Vector3(0.5f, -0.5f, 0.5f));
            msh.Positions.Add(new Vector3(-0.5f, -0.5f, 0.5f));

            msh.Positions.Add(new Vector3(-0.5f, 0.5f, -0.5f));
            msh.Positions.Add(new Vector3(0.5f, 0.5f, -0.5f));
            msh.Positions.Add(new Vector3(0.5f, 0.5f, 0.5f));
            msh.Positions.Add(new Vector3(-0.5f, 0.5f, 0.5f));

            msh.Normals.Add(new Vector3(-0.5f, -0.5f, -0.5f));
            msh.Normals.Add(new Vector3(0.5f, -0.5f, -0.5f));
            msh.Normals.Add(new Vector3(0.5f, -0.5f, 0.5f));
            msh.Normals.Add(new Vector3(-0.5f, -0.5f, 0.5f));

            msh.Normals.Add(new Vector3(-0.5f, 0.5f, -0.5f));
            msh.Normals.Add(new Vector3(0.5f, 0.5f, -0.5f));
            msh.Normals.Add(new Vector3(0.5f, 0.5f, 0.5f));
            msh.Normals.Add(new Vector3(-0.5f, 0.5f, 0.5f));

            msh.UVs.Add(new Vector2(0, 0));
            msh.UVs.Add(new Vector2(1, 0));
            msh.UVs.Add(new Vector2(1, 1));
            msh.UVs.Add(new Vector2(0, 1));

            msh.UVs.Add(new Vector2(0, 0));
            msh.UVs.Add(new Vector2(1, 0));
            msh.UVs.Add(new Vector2(1, 1));
            msh.UVs.Add(new Vector2(0, 1));

            //Down 2 triangles
            msh.Indices.Add(0);
            msh.Indices.Add(1);
            msh.Indices.Add(2);
            msh.Indices.Add(0);
            msh.Indices.Add(2);
            msh.Indices.Add(3);

            //Up 2 triangles
            msh.Indices.Add(4);
            msh.Indices.Add(5);
            msh.Indices.Add(6);
            msh.Indices.Add(4);
            msh.Indices.Add(6);
            msh.Indices.Add(7);

            //Front 2 Triangles
            msh.Indices.Add(0);
            msh.Indices.Add(4);
            msh.Indices.Add(3);
            msh.Indices.Add(7);
            msh.Indices.Add(4);
            msh.Indices.Add(3);

            //Back 2 Triangles
            msh.Indices.Add(1);
            msh.Indices.Add(5);
            msh.Indices.Add(2);
            msh.Indices.Add(6);
            msh.Indices.Add(5);
            msh.Indices.Add(2);

            //"Left" 2 Triangles
            msh.Indices.Add(2);
            msh.Indices.Add(6);
            msh.Indices.Add(3);
            msh.Indices.Add(7);
            msh.Indices.Add(6);
            msh.Indices.Add(3);

            //"Right" 2 Triangles
            msh.Indices.Add(0);
            msh.Indices.Add(4);
            msh.Indices.Add(1);
            msh.Indices.Add(5);
            msh.Indices.Add(4);
            msh.Indices.Add(1);

            //Return Mesh
            return msh;
        }
    }
}
