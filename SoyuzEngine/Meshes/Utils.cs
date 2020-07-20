using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Soyuz.Meshes
{
    public class Utils
    {
        /// <summary>
        /// Origin Point
        /// </summary>
        /// <returns></returns>
        public static Mesh Point()
        {
            Mesh msh = new Mesh();

            msh.Positions.Add(new Vector3(0));
            msh.Normals.Add(new Vector3(0));
            msh.UVs.Add(new Vector2(0));
            msh.Indices.Add(0);

            return msh;
        }
    }
}
