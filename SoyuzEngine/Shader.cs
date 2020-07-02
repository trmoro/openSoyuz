using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Soyuz
{
    public class Shader
    {
        //ID
        public int ID { get; set; }
        //Vertex
        public string Vertex { get; set; }
        //Fragment
        public string Fragment { get; set; }
        //Geometry
        public string Geometry { get; set; }

        /// <summary>
        /// Shader Constructor
        /// </summary>
        public Shader()
        {
            ID = Engine.Core.CreateShader();
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~Shader()
        {
            Delete();
        }

        /// <summary>
        /// Load Prefab
        /// </summary>
        /// <param name="PrefabID"></param>
        public void LoadPrefab(int PrefabID)
        {
            Engine.Core.SetPrefabShader(ID, PrefabID);
        }

        /// <summary>
        /// Load Shader
        /// </summary>
        /// <param name="VertexPath"></param>
        /// <param name="FragmentPath"></param>
        public void Load(string VertexPath, string FragmentPath)
        {
            Vertex = File.ReadAllText(VertexPath);
            Fragment = File.ReadAllText(FragmentPath);

            Set(Vertex, Fragment);
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="VertexPath"></param>
        /// <param name="GeometryPath"></param>
        /// <param name="FragmentPath"></param>
        public void Load(string VertexPath, string GeometryPath, string FragmentPath)
        {
            Vertex = File.ReadAllText(VertexPath);
            Fragment = File.ReadAllText(FragmentPath);
            Geometry = File.ReadAllText(GeometryPath);

            Set(Vertex, Geometry, Fragment);
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <param name="Vertex"></param>
        /// <param name="Fragment"></param>
        public void Set(string Vertex, string Fragment)
        {
            Engine.Core.SetShader(ID, Vertex, Fragment);
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <param name="Vertex"></param>
        /// <param name="Geometry"></param>
        /// <param name="Fragment"></param>
        public void Set(string Vertex, string Geometry, string Fragment)
        {
            Engine.Core.SetShader(ID, Vertex, Geometry, Fragment);
        }

        /// <summary>
        /// Delete
        /// </summary>
        public void Delete()
        {
            Engine.Core.DeleteShader(ID);
        }

        //
    }
}
