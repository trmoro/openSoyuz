using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Soyuz
{
    /// <summary>
    /// Mesh Class
    /// </summary>
    public class Mesh
    {
        //Mesh ID
        public int MeshID { get; set; }

        //Positions, Normals and UVs
        public List<Vector3> Positions { get; set; }
        public List<Vector3> Normals { get; set; }
        public List<Vector2> UVs { get; set; }

        //Indices
        public List<int> Indices { get; set; }

        /// <summary>
        /// Mesh Constructor
        /// </summary>
        public Mesh()
        {
            MeshID = Engine.Core.CreateMesh();

            Positions = new List<Vector3>();
            Normals = new List<Vector3>();
            UVs = new List<Vector2>();

            Indices = new List<int>();
        }

        /// <summary>
        /// Clear Mesh
        /// </summary>
        public void Clear()
        {
            Positions.Clear();
            Normals.Clear();
            UVs.Clear();
            Indices.Clear();
        }

        /// <summary>
        /// Return a list of triangles
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int, int, int>> TrianglesIndices()
        {
            List<Tuple<int, int, int>> tris = new List<Tuple<int, int, int>>();

            for (int i = 0; i < Indices.Count; i += 3)
                tris.Add(new Tuple<int, int, int>(Indices[i], Indices[(i + 1) % Indices.Count], Indices[(i + 2) % Indices.Count]));

            return tris;
        }

        /// <summary>
        /// Get Nearseat Vertex Index
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns></returns>
        public int GetNearestVertexIndex(Vector3 position)
        {
            //Return value
            int index = -1;
            float distance = float.MaxValue;

            for (int i = 0; i < Positions.Count; i++)
            {
                float d = Vector3.Distance(Positions[i], position);
                if (d < distance)
                {
                    distance = d;
                    index = i;
                }
            }

            //Return
            return index;
        }

        /// <summary>
        /// Set Draw Mode
        /// </summary>
        /// <param name="DrawMode"></param>
        public void SetDrawMode(int DrawMode)
        {
            Engine.Core.SetMeshDrawMode(MeshID, DrawMode);
        }

        /// <summary>
        /// Delete Mesh
        /// </summary>
        public void Delete()
        {
            Engine.Core.DeleteMesh(MeshID);
        }       

        /// <summary>
        /// Compile Mesh
        /// </summary>
        /// <returns>itselft</returns>
        public Mesh Compile()
        {
            //Prepare Mesh Memory
            Engine.Core.MeshPrepareMemory(MeshID, (uint) Positions.Count, (uint) Indices.Count);

            //Add Vertices
            for (int i = 0; i < Positions.Count; i++)
            {
                //Add Vertex
                Vector3 pos = Positions[i];
                Vector3 nor = Normals[i];
                Vector2 uv = UVs[i];
                Engine.Core.MeshAddVertex(MeshID, pos.X, pos.Y, pos.Z, nor.X, nor.Y, nor.Z, uv.X, uv.Y);
            }

            //Add Indices
            for (int i = 0; i < Indices.Count; i++)
                Engine.Core.MeshAddIndex(MeshID, Indices[i]);

            //Compile Mesh
            Engine.Core.MeshCompile(MeshID);

            return this;
        }

        /// <summary>
        /// Compute Normal with 3 Position
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Vector3 ComputeNormal(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 u = b - a;
            Vector3 v = c - a;
            return new Vector3((u.Y * v.Z) - (u.Z * v.Y), (u.Z * v.X) - (u.X * v.Z), (u.X * v.Y) - (u.Y * v.X));
        }

        //
    }
}
