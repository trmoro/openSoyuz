using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Soyuz
{
    /// <summary>
    /// Model Class
    /// </summary>
    public class Model
    {
        //Parent Actor
        public Actor ParentActor { get; set; }

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

        //Multi Shader Variable
        public string MultiShader_StrVal { get; set; }
        public bool MultiShader_Pass { get; set; }

        //Deleted
        public bool IsDeleted { get; private set; }


        /// <summary>
        /// Model Constructor
        /// </summary>
        public Model()
        {
            //Create Model
            ModelID = Engine.Core.CreateModel();
            IsHidden = false;
            ParentActor = null;

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

            //Multishader Variable
            MultiShader_StrVal = "";
            MultiShader_Pass = true;

            //Uniform Dictionaries
            UniformInt      = new Dictionary<string, int>();
            UniformFloat    = new Dictionary<string, float>();
            UniformVec2     = new Dictionary<string, Vector2>();
            UniformVec3     = new Dictionary<string, Vector3>();
            UniformVec4     = new Dictionary<string, Vector4>();
            UniformFont     = new Dictionary<string, Font>();

            //Deleted
            IsDeleted = false;
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~Model()
        {
            Delete();
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
        /// Update Meshes List in Kernel, Barycenter and Vertex-Length
        /// </summary>
        public void Update()
        {
            //Discard Meshes (not deleted from memory but removed from rendering if not added)
            Engine.Core.DiscardModelMeshes(ModelID);

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
                //Add Meshes to Model in Kernel
                Engine.Core.AddMeshToModel(ModelID, m.MeshID);

                for (int i = 0; i < m.Positions.Count; i++)
                {
                    //Modify Barycenter variables
                    Vector3 pos = m.Positions[i];
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
            }

            //Compute Barycenter
            if(nVertex > 0)
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

        /// <summary>
        /// Set Model's meshes draw mode
        /// </summary>
        /// <param name="DrawMode"></param>
        public void SetDrawMode(int DrawMode)
        {
            foreach (Mesh m in Meshes)
                m.SetDrawMode(DrawMode);
        }

        /// <summary>
        /// Delete
        /// </summary>
        public void Delete()
        {
            IsDeleted = true;
            Engine.Core.DeleteModel(ModelID);
        }

        /// <summary>
        /// Delete Hidden Meshes
        /// </summary>
        public void DeleteHiddenMeshes()
        {
            Engine.Core.DeleteHiddenMeshes(ModelID);
        }

        //End of Model Class

    }

}
