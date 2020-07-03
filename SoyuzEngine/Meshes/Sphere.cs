using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Soyuz.Meshes
{
    public class Sphere
    {
        /// <summary>
        /// Convert U,V coordinate to a Vector3 Sphere Coordinate
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Vector3 UVToVector(double u, double v)
        {
            return new Vector3((float)(Math.Cos(u) * Math.Cos(v) ), (float)(Math.Sin(v) ), (float)(Math.Sin(u) * Math.Cos(v) ));
        }

        /// <summary>
        /// Quad Faces Sphere
        /// </summary>
        /// <param name="nMeridian"></param>
        /// <param name="nParallel"></param>
        /// <returns></returns>
        public static Mesh QuadFaces(int nMeridian, int nParallel)
        {
            //Create Mesh
            Mesh msh = new Mesh();

            //Foreach Meridian
            for (int i = 0; i < nMeridian; i++)
            {
                //Foreach Parallel
                for (int j = 0; j < nParallel; j++)
                {
                    //Create Vertex
                    double u = (double)i / (double)nMeridian;
                    double v = (double)j / (double)(nParallel - 1);
                    Vector3 vrt = UVToVector(u * 2 * Math.PI, (v * Math.PI) - (Math.PI / 2));
                    msh.Positions.Add(vrt);
                    msh.Normals.Add(vrt);
                    msh.UVs.Add(new Vector2((float)u, (float)v));

                    //Create Triangles
                    if (j < nParallel - 1)
                    {
                        //Current Position
                        int pos = (i * nParallel) + j;

                        // Two triangles
                        if (i < nMeridian - 1)
                        {
                            msh.Indices.Add(pos);
                            msh.Indices.Add(pos + 1);
                            msh.Indices.Add(pos + 1 + nParallel);
                            msh.Indices.Add(pos + nParallel);
                        }
                        else
                        {
                            msh.Indices.Add(pos);
                            msh.Indices.Add(pos + 1);
                            msh.Indices.Add(j + 1);
                            msh.Indices.Add(j);
                        }
                    }
                }
            }

            //Return
            return msh;
        }

        /// <summary>
        /// Triangle Faces Spehere
        /// </summary>
        /// <param name="nMeridian"></param>
        /// <param name="nParallel"></param>
        /// <returns></returns>
        public static Mesh TriangleFaces(int nMeridian, int nParallel)
        {
            //Create Mesh
            Mesh msh = new Mesh();

            //Foreach Meridian
            for (int i = 0; i < nMeridian; i++)
            {
                //Foreach Parallel
                for (int j = 0; j < nParallel; j++)
                {
                    //Create Vertex
                    double u = (double)i / (double)nMeridian;
                    double v = (double)j / (double) (nParallel-1);
                    Vector3 vrt = UVToVector(u * 2 * Math.PI, (v * Math.PI) - (Math.PI / 2));
                    msh.Positions.Add(vrt);
                    msh.Normals.Add(vrt);
                    msh.UVs.Add(new Vector2((float)u, (float)v));

                    //Create Triangles
                    if (j < nParallel - 1)
                    {
                        //Current Position
                        int pos = (i * nParallel) + j;

                        // Two triangles
                        if (i < nMeridian - 1)
                        {
                            msh.Indices.Add(pos);
                            msh.Indices.Add(pos + 1);
                            msh.Indices.Add(pos + 1 + nParallel);

                            msh.Indices.Add(pos);
                            msh.Indices.Add(pos + 1 + nParallel);
                            msh.Indices.Add(pos + nParallel);
                        }
                        else
                        {
                            msh.Indices.Add(pos);
                            msh.Indices.Add(pos + 1);
                            msh.Indices.Add(j + 1);

                            msh.Indices.Add(pos);
                            msh.Indices.Add(j + 1);
                            msh.Indices.Add(j);
                        }
                    }
                }
            }

            //Return
            return msh;
        }

        /// <summary>
        /// Triangle Faces Sphere with Radius Data
        /// </summary>
        /// <param name="nMeridian"></param>
        /// <param name="nParallel"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Mesh RadiusData(int nMeridian, int nParallel, float[,] data)
        {
            //Create Mesh
            Mesh msh = new Mesh();

            //Foreach Meridian
            for (int i = 0; i < nMeridian; i++)
            {
                //Foreach Parallel
                for (int j = 0; j < nParallel; j++)
                {
                    //Get Radius
                    float radius = data[i, j];

                    //Create Vertex
                    double u = (double)i / (double)nMeridian;
                    double v = (double)j / (double)(nParallel - 1);
                    Vector3 vrt = UVToVector(u * 2 * Math.PI, (v * Math.PI) - (Math.PI / 2));
                    msh.Positions.Add(vrt * radius);
                    msh.Normals.Add(vrt);
                    msh.UVs.Add(new Vector2((float)u, (float)v));

                    //Create Triangles
                    if (j < nParallel - 1)
                    {
                        //Current Position
                        int pos = (i * nParallel) + j;

                        // Two triangles
                        if (i < nMeridian - 1)
                        {
                            msh.Indices.Add(pos);
                            msh.Indices.Add(pos + 1);
                            msh.Indices.Add(pos + 1 + nParallel);

                            msh.Indices.Add(pos);
                            msh.Indices.Add(pos + 1 + nParallel);
                            msh.Indices.Add(pos + nParallel);
                        }
                        else
                        {
                            msh.Indices.Add(pos);
                            msh.Indices.Add(pos + 1);
                            msh.Indices.Add(j + 1);

                            msh.Indices.Add(pos);
                            msh.Indices.Add(j + 1);
                            msh.Indices.Add(j);
                        }
                    }
                }
            }

            //Return
            return msh;
        }
    }
}
