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
            MeshID = -1;

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

        //
    }
}
