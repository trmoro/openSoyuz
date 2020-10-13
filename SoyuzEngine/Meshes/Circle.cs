using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Soyuz.Meshes
{
    public class Circle
    {
        /// <summary>
        /// Create Circle Mesh
        /// </summary>
        /// <param name="PointNumber"></param>
        /// <returns></returns>
        public static Mesh Mesh(int PointNumber)
        {
            Mesh msh = new Mesh();

            //Origin
            msh.Positions.Add(new Vector3(0, 0, 0));
            msh.Normals.Add(new Vector3(0, 0, 0));
            msh.UVs.Add(new Vector2(0, 0));

            for (int i = 0; i < PointNumber; i++)
            {
                double x = 2 * Math.PI * ((i + 1) / (double)PointNumber);

                msh.Positions.Add(new Vector3((float)Math.Cos(x), (float)Math.Sin(x), 0));
                msh.Normals.Add(new Vector3(0, 0, 1));
                msh.UVs.Add(new Vector2((float)Math.Cos(x), (float)Math.Sin(x)));

                msh.Indices.Add(0);
                msh.Indices.Add(i + 1);
                if (i < PointNumber - 1)
                    msh.Indices.Add(i + 2);
                else
                    msh.Indices.Add(1);
            }

            return msh;
        }
    }
}
