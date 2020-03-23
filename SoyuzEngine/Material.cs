using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Soyuz
{
    public class Material
    {
        //RGBA Color
        public Vector4 Color { get; set; }

        //Diffuse RGB
        public Vector3 Diffuse { get; set; }

        //Specular RGB
        public Vector3 Specular { get; set; }

        //Shininess
        public float Shininess { get; set; }

        //Is Textured
        public bool IsTextured { get; set; }

        //Texture ID
        public Texture Texture { get; set; }

        /// <summary>
        /// Material Constructor
        /// </summary>
        public Material()
        {
            //Defaults Values
            Color = new Vector4(1, 1, 1, 1);
            Diffuse = new Vector3(0.5f, 0.5f, 0.5f);
            Specular = new Vector3(0.1f, 0.1f, 0.1f);
            Shininess = 64.0f;
            IsTextured = false;
            Texture = null;
        }
    }
}
