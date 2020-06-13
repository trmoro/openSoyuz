using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Soyuz
{
    //Model
    public class Model
    {
        //Hidden
        public bool IsHidden { get; set; }

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

        //Uniform Dictionaries
        public Dictionary<string, int> UniformInt { get; set; }
        public Dictionary<string, float> UniformFloat { get; set; }
        public Dictionary<string, Vector2> UniformVec2 { get; set; }
        public Dictionary<string, Vector3> UniformVec3 { get; set; }
        public Dictionary<string, Vector4> UniformVec4 { get; set; }
        public Dictionary<string, Font> UniformFont { get; set; }


        /// <summary>
        /// Model Constructor
        /// </summary>
        public Model()
        {
            //Create Model
            ModelID = Engine.Core.CreateModel();
            IsHidden = false;

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

            //Uniform Dictionaries
            UniformInt      = new Dictionary<string, int>();
            UniformFloat    = new Dictionary<string, float>();
            UniformVec2     = new Dictionary<string, Vector2>();
            UniformVec3     = new Dictionary<string, Vector3>();
            UniformVec4     = new Dictionary<string, Vector4>();
            UniformFont     = new Dictionary<string, Font>();
        }

        /// <summary>
        /// Update Spatial Properties (Position, Scale and Rotation)
        /// </summary>
        public void UpdateProperties()
        {
            Engine.Core.SetModelPosition(ModelID, Position.X, Position.Y, Position.Z);
            Engine.Core.SetModelRotation(ModelID, Rotation.X, Rotation.Y, Rotation.Z);
            Engine.Core.SetModelScale(ModelID, Scale.X, Scale.Y, Scale.Z);
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

        /// <summary>
        /// Load Model
        /// </summary>
        /// <param name="Path"></param>
        public void Load(string Path)
        {
            Engine.Core.LoadModel(ModelID, Path);
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
        /// Return a list of triangles
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int,int,int>> TrianglesIndices()
        {
            List<Tuple<int, int, int>> tris = new List<Tuple<int, int, int>>();

            for(int i = 0; i < Indices.Count; i+=3)
                tris.Add(new Tuple<int,int,int>(Indices[i],Indices[(i+1) % Indices.Count],Indices[(i+2) % Indices.Count]) );

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

            for(int i = 0; i < Positions.Count; i++)
            {
                float d = Vector3.Distance(Positions[i], position);
                if(d < distance)
                {
                    distance = d;
                    index = i;
                }
            }

            //Return
            return index;
        }

        /// <summary>
        /// Rectangle
        /// </summary>
        public void Rect()
        {
            Positions.Add(new Vector3(0, 0, 0));
            Positions.Add(new Vector3(1, 0, 0));
            Positions.Add(new Vector3(1, 1, 0));
            Positions.Add(new Vector3(0, 1, 0));

            Normals.Add(new Vector3(0,0,1));
            Normals.Add(new Vector3(0,0,1));
            Normals.Add(new Vector3(0,0,1));
            Normals.Add(new Vector3(0,0,1));

            UVs.Add(new Vector2(0,0));
            UVs.Add(new Vector2(1,0));
            UVs.Add(new Vector2(1,1));
            UVs.Add(new Vector2(0,1));

            Indices.Add(0);
            Indices.Add(1);
            Indices.Add(2);

            Indices.Add(0);
            Indices.Add(2);
            Indices.Add(3);
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
            UVs.Add(new Vector2(1, 0));
            UVs.Add(new Vector2(1, 1));
            UVs.Add(new Vector2(0, 1));

            UVs.Add(new Vector2(0, 0));
            UVs.Add(new Vector2(1, 0));
            UVs.Add(new Vector2(1, 1));
            UVs.Add(new Vector2(0, 1));

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
                    UVs.Add(new Vector2( (float)m / (float) nMeridian, (float)p / (float)nParallel ) );

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

        //Sphere
        public void SphereData(int nMeridian, int nParallel,float[,] data)
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
                    float radius = 1 + (data[m,p] * 0.05f );

                    Positions.Add(GenerateSphereVertex(radius, ((float)m / (float)nMeridian) * 2 * Math.PI, ((float)p / (float)nParallel) * Math.PI - (Math.PI / 2)));
                    Normals.Add(GenerateSphereNormal(((float)m / (float)nMeridian) * 2 * Math.PI, ((float)p / (float)nParallel) * Math.PI - (Math.PI / 2)));
                    UVs.Add(new Vector2( (float)p / (float)nParallel, (float)m / (float)nMeridian) );


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
