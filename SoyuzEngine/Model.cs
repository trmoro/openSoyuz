﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Soyuz
{
    //Model
    public class Model
    {
        //All Meshes
        public List<Mesh> Meshes { get; set; }

        //Material
        public Material Material { get; set; }

        //Model ID
        public int ModelID { get; set; }

        //Position
        public Vector3 Position { get; set; }

        //Rotation
        public Vector3 Rotation { get; set; }

        //Scale
        public Vector3 Scale { get; set; }

        //Name
        public String Name { get; set; }

        //Barycenter
        public Vector3 Barycenter { get; set; }

        //XYZ Vertex Lenght
        public Vector3 VertexLength { get; set; }

        /// <summary>
        /// Model Constructor
        /// </summary>
        public Model()
        {
            //Create Model
            ModelID = Engine.Core.CreateModel();

            //Default Material
            Material = new Material();

            //Meshes
            Meshes = new List<Mesh>();

            //Space Variables
            Position = new Vector3(0);
            Rotation = new Vector3(0);
            Scale = new Vector3(1);

            //Name
            Name = "noname";
        }

        /// <summary>
        /// Compile Model and its Meshes
        /// </summary>
        public void Compile()
        {
            //Barycenter Variables
            Vector3 bary = new Vector3(0);
            int nVertex = 0;

            //Vertex Length Variable
            float minX = 0;
            float maxX = 0;
            float minY = 0;
            float maxY = 0;
            float minZ = 0;
            float maxZ = 0;

            //For all Meshes
            foreach(Mesh m in Meshes)
            {
                //Init
                if (m.MeshID == -1)
                    m.MeshID = Engine.Core.CreateMesh(ModelID);

                //Prepare Mesh Memory
                Engine.Core.MeshPrepareMemory(ModelID, m.MeshID, (uint) m.Positions.Count, (uint) m.Indices.Count );

                //Add Vertices
                for (int i = 0; i < m.Positions.Count; i++)
                {
                    //Add Vertex
                    Vector3 pos = m.Positions[i];
                    Vector3 nor = m.Normals[i];
                    Vector2 uv = m.UVs[i];
                    Engine.Core.MeshAddVertex(ModelID, m.MeshID,pos.X,pos.Y,pos.Z,nor.X,nor.Y,nor.Z,uv.X,uv.Y);

                    //Modify Barycenter variables
                    bary += pos;
                    nVertex++;

                    //First Vertex
                    if(nVertex == 0)
                    {
                        minX = pos.X;
                        maxX = pos.X;
                        minY = pos.Y;
                        maxY = pos.Y;
                        minZ = pos.Z;
                        maxZ = pos.Z;
                    }

                    //Test Max and min
                    else
                    {
                        //X
                        if (pos.X > maxX)
                            maxX = pos.X;
                        if (pos.X < minX)
                            minX = pos.X;

                        //Y
                        if (pos.Y > maxY)
                            maxY = pos.Y;
                        if (pos.Y < minY)
                            minY = pos.Y;

                        //Z
                        if (pos.Z > maxZ)
                            maxZ = pos.Z;
                        if (pos.Z < minZ)
                            minZ = pos.Z;
                    }

                    //
                }

                //Add Indices
                for (int i = 0; i < m.Indices.Count; i++)
                    Engine.Core.MeshAddIndex(ModelID, m.MeshID, m.Indices[i]);

                //Compile Mesh
                Engine.Core.MeshCompile(ModelID, m.MeshID);
            }

            //Compute Barycenter
            Barycenter = bary / nVertex;

            //Compute Vertex Length
            VertexLength = new Vector3(Math.Abs(maxX - minX), Math.Abs(maxY - minY), Math.Abs(maxZ - minZ));
        }

        //End of Model Class

    }

    //Mesh
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
        /// Create a Cube Mesh
        /// </summary>
        public void Cube()
        {
            Positions.Add(new Vector3(-0.5f, -0.5f, -0.5f));
            Positions.Add(new Vector3(0.5f, -0.5f, -0.5f));
            Positions.Add(new Vector3(0.5f, -0.5f, 0.5f));
            Positions.Add(new Vector3(-0.5f, -0.5f, 0.5f));

            Positions.Add(new Vector3(-0.5f, 0.5f, -0.5f));
            Positions.Add(new Vector3(0.5f, 0.5f, -0.5f));
            Positions.Add(new Vector3(0.5f, 0.5f, 0.5f));
            Positions.Add(new Vector3(-0.5f, 0.5f, 0.5f));

            Normals.Add(new Vector3(-0.5f, -0.5f, -0.5f));
            Normals.Add(new Vector3(0.5f, -0.5f, -0.5f));
            Normals.Add(new Vector3(0.5f, -0.5f, 0.5f));
            Normals.Add(new Vector3(-0.5f, -0.5f, 0.5f));

            Normals.Add(new Vector3(-0.5f, 0.5f, -0.5f));
            Normals.Add(new Vector3(0.5f, 0.5f, -0.5f));
            Normals.Add(new Vector3(0.5f, 0.5f, 0.5f));
            Normals.Add(new Vector3(-0.5f, 0.5f, 0.5f));

            UVs.Add(new Vector2(0, 0));
            UVs.Add(new Vector2(0, 0));
            UVs.Add(new Vector2(0, 0));
            UVs.Add(new Vector2(0, 0));

            UVs.Add(new Vector2(0, 0));
            UVs.Add(new Vector2(0, 0));
            UVs.Add(new Vector2(0, 0));
            UVs.Add(new Vector2(0, 0));

            //Down 2 triangles
            Indices.Add(0);
            Indices.Add(1);
            Indices.Add(2);
            Indices.Add(0);
            Indices.Add(2);
            Indices.Add(3);

            //Up 2 triangles
            Indices.Add(4);
            Indices.Add(5);
            Indices.Add(6);
            Indices.Add(4);
            Indices.Add(6);
            Indices.Add(7);

            //Front 2 Triangles
            Indices.Add(0);
            Indices.Add(4);
            Indices.Add(3);
            Indices.Add(7);
            Indices.Add(4);
            Indices.Add(3);

            //Back 2 Triangles
            Indices.Add(1);
            Indices.Add(5);
            Indices.Add(2);
            Indices.Add(6);
            Indices.Add(5);
            Indices.Add(2);

            //"Left" 2 Triangles
            Indices.Add(2);
            Indices.Add(6);
            Indices.Add(3);
            Indices.Add(7);
            Indices.Add(6);
            Indices.Add(3);

            //"Right" 2 Triangles
            Indices.Add(0);
            Indices.Add(4);
            Indices.Add(1);
            Indices.Add(5);
            Indices.Add(4);
            Indices.Add(1);

        }

        //Generate Sphere Vertex
        Vector3 GenerateSphereVertex(float radius, double u, double v)
        {
            return new Vector3( (float) (Math.Cos(u) * Math.Cos(v) * radius), (float) (Math.Sin(v) * radius), (float) (Math.Sin(u) * Math.Cos(v) * radius) );
        }

        //Generate Sphere Normal
        Vector3 GenerateSphereNormal(double u, double v)
        {
            return new Vector3( (float) (Math.Cos(u) * Math.Cos(v)), (float) Math.Sin(v), (float) (Math.Sin(u) * Math.Cos(v)) );
        }

        //Generate Sphere UV
        Vector2 GenerateSphereUV(float m, float p, uint quadPod)
        {
            switch (quadPod)
            {
                case 0: return new Vector2();
                case 1: return new Vector2();
                case 2: return new Vector2();
                case 3: return new Vector2();
                default: return new Vector2();
            }
        }

        //Sphere
        public void Sphere(int nMeridian, int nParallel)
        {
            //Number of vertices and triangles
            int nVertex = nMeridian * nParallel * 4;
            int nFace = nMeridian * nParallel * 2;

            //Data
            int[] tris = new int[nFace * 3];

            //Vertices dones
            int vertexDone = 0;

            //Foreach Meridian
            for (uint m = 0; m < nMeridian; m++)
            {
                //Foreach Parallel
                for (uint p = 0; p < nParallel; p++)
                {
                    //float radius = ((heightMap.GetPixel((int)m, (int)p).grayscale) * (globalRadius / 10)) + (globalRadius);
                    float radius = 1;

                    Positions.Add(GenerateSphereVertex(radius, ((float)m / (float)nMeridian) * 2 * Math.PI, ((float)p / (float)nParallel) * Math.PI - (Math.PI / 2)) );
                    Normals.Add(GenerateSphereNormal(((float)m / (float)nMeridian) * 2 * Math.PI, ((float)p / (float)nParallel) * Math.PI - (Math.PI / 2)) );
                    UVs.Add(GenerateSphereUV(m, p, 0) );

                    //Self
                    tris[vertexDone * 6] = vertexDone;

                    //Right
                    if (p < nParallel - 1)
                        tris[1 + (vertexDone * 6)] = (int)1 + vertexDone;
                    else
                        tris[1 + (vertexDone * 6)] = (int)m * nParallel;

                    //Up / Down
                    if (m < nMeridian - 1)
                        tris[2 + (vertexDone * 6)] = (int)nParallel + vertexDone;
                    else
                        tris[2 + (vertexDone * 6)] = (int)p;

                    //Up / Down
                    if (m < nMeridian - 1)
                        tris[3 + (vertexDone * 6)] = (int)nParallel + vertexDone;
                    else
                        tris[3 + (vertexDone * 6)] = (int)p;

                    //Right
                    if (p < nParallel - 1)
                        tris[4 + (vertexDone * 6)] = (int)1 + vertexDone;
                    else
                        tris[4 + (vertexDone * 6)] = (int)m * nParallel;

                    //Up Right
                    if (m < nMeridian - 1 && p < nParallel - 1)
                        tris[5 + (vertexDone * 6)] = (int)nParallel + vertexDone + 1;
                    else if (m < nMeridian - 1)
                        tris[5 + (vertexDone * 6)] = (int)(nParallel * (m + 1));
                    else if (p < nParallel - 1)
                        tris[5 + (vertexDone * 6)] = (int)p + 1;
                    else
                        tris[5 + (vertexDone * 6)] = (int)0;


                    //Vertex is done
                    vertexDone += 1;
                }
            }

            //Position
            Indices.AddRange(tris);
        }

        //

    }
}
