using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Soyuz.Meshes
{
    public class Line
    {
        /// <summary>
        /// Circle
        /// </summary>
        /// <param name="PointNumber"></param>
        /// <returns></returns>
        public static Mesh Circle(int PointNumber)
        {
            Mesh msh = new Mesh();

            for(int i = 0; i < PointNumber; i++)
            {
                double x = 2 * Math.PI * ((i + 1) / (double) PointNumber);
                msh.Positions.Add(new Vector3( (float) Math.Cos(x), 0, (float) Math.Sin(x)));
                msh.Normals.Add(new Vector3(0, 1, 0));
                msh.UVs.Add(new Vector2( (float)Math.Cos(x), (float)Math.Sin(x) ));
                msh.Indices.Add(i);
            }
            msh.Indices.Add(0);

            return msh;
        }
    }
}
