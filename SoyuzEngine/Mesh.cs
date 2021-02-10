using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
            Positions.Clear();
            Normals.Clear();
            UVs.Clear();
            Indices.Clear();
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
        /// Translate all mesh vertex
        /// </summary>
        /// <param name="vector"></param>
        public Mesh Translate(Vector3 vector)
        {
            for (int i = 0; i < Positions.Count; i++)
                Positions[i] = Positions[i] + vector;
            return this;
        }

        /// <summary>
        /// Scale all mesh vertex
        /// </summary>
        /// <param name="vector"></param>
        public Mesh Scale(Vector3 vector)
        {
            for (int i = 0; i < Positions.Count; i++)
                Positions[i] = Positions[i] * vector;
            return this;
        }

        /// <summary>
        /// Rotate all mesh vertex
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public Mesh Rotate(Vector3 vector)
        {
            for (int i = 0; i < Positions.Count; i++)
            {
                //Transform
                Matrix4x4 rx = Matrix4x4.CreateRotationX(vector.X);
                Matrix4x4 ry = Matrix4x4.CreateRotationY(vector.Y);
                Matrix4x4 rz = Matrix4x4.CreateRotationZ(vector.Z);
                Vector3 pos = Vector3.Transform(Positions[i], rx);
                pos = Vector3.Transform(pos, ry);
                pos = Vector3.Transform(pos, rz);

                //Set
                Positions[i] = pos;
            }

            return this;
        }

        /// <summary>
        /// Add OBJ model
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public Mesh AddOBJ(string Path)
        {
            FileStream fileStream = new FileStream(Path, FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line;
                Mesh tmp = new Mesh();

                while ((line = reader.ReadLine()) != null)
                {

                    string[] d = line.Split(" ");

                    //Vertex
                    if (d.Length == 4 && d[0].Equals("v"))
                        tmp.Positions.Add(new Vector3(float.Parse(d[1], NumberStyles.Any, CultureInfo.InvariantCulture), float.Parse(d[2], NumberStyles.Any, CultureInfo.InvariantCulture), float.Parse(d[3], NumberStyles.Any, CultureInfo.InvariantCulture)));
                    //UV
                    if (d.Length == 3 && d[0].Equals("vt"))
                        tmp.UVs.Add(new Vector2(float.Parse(d[1], NumberStyles.Any, CultureInfo.InvariantCulture), float.Parse(d[2], NumberStyles.Any, CultureInfo.InvariantCulture)));
                    //Normal
                    if (d.Length == 4 && d[0].Equals("vn"))
                        tmp.Normals.Add(new Vector3(float.Parse(d[1], NumberStyles.Any, CultureInfo.InvariantCulture), float.Parse(d[2], NumberStyles.Any, CultureInfo.InvariantCulture), float.Parse(d[3], NumberStyles.Any, CultureInfo.InvariantCulture)));
                    //Face
                    if (d.Length == 4 && d[0].Equals("f"))
                    {
                        string[] tri1 = d[1].Split("/");
                        string[] tri2 = d[2].Split("/");
                        string[] tri3 = d[3].Split("/");

                        Positions.Add(tmp.Positions[int.Parse(tri1[0]) - 1]);
                        Positions.Add(tmp.Positions[int.Parse(tri2[0]) - 1]);
                        Positions.Add(tmp.Positions[int.Parse(tri3[0]) - 1]);

                        UVs.Add(tmp.UVs[int.Parse(tri1[1]) - 1]);
                        UVs.Add(tmp.UVs[int.Parse(tri2[1]) - 1]);
                        UVs.Add(tmp.UVs[int.Parse(tri3[1]) - 1]);

                        Normals.Add(tmp.Normals[int.Parse(tri1[2]) - 1]);
                        Normals.Add(tmp.Normals[int.Parse(tri2[2]) - 1]);
                        Normals.Add(tmp.Normals[int.Parse(tri3[2]) - 1]);

                        for (int i = 0; i < 3; i++)
                            Indices.Add(Indices.Count);
                    }
                }
            }

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
