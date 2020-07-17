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
        /// Quad Faces Sphere by Index
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

                        // Two Quad
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
        /// Triangle Faces by Vertex
        /// Interest : Better normal mapping
        /// </summary>
        /// <param name="nMeridian"></param>
        /// <param name="nParallel"></param>
        /// <returns></returns>
        public static Mesh TriangleVertex(int nMeridian, int nParallel)
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
                    double up1 = (double) (i+1) / (double)nMeridian;

                    double v = (double) j / (double) (nParallel-1);
                    double vp1 = (double) (j+1) / (double)(nParallel - 1);

                    //Positions
                    Vector3 v0 = UVToVector(u * 2 * Math.PI, (v * Math.PI) - (Math.PI / 2));
                    Vector3 v1 = UVToVector(up1 * 2 * Math.PI, (v * Math.PI) - (Math.PI / 2));
                    Vector3 v2 = UVToVector(up1 * 2 * Math.PI, ( vp1 * Math.PI) - (Math.PI / 2));
                    Vector3 v3 = UVToVector(u * 2 * Math.PI, ( vp1 * Math.PI) - (Math.PI / 2));

                    //Normal
                    Vector3 n0 = Mesh.ComputeNormal(v0, v1, v2);
                    Vector3 n1 = Mesh.ComputeNormal(v0, v2, v3);
                                        
                    //UV
                    Vector2 uv0 = new Vector2((float)u, (float)v);
                    Vector2 uv1 = new Vector2((float)up1, (float)v);
                    Vector2 uv2 = new Vector2((float)up1, (float)vp1);
                    Vector2 uv3 = new Vector2((float)u, (float)vp1);

                    //Add
                    msh.Positions.Add(v0);
                    msh.Positions.Add(v1);
                    msh.Positions.Add(v2);
                    msh.UVs.Add(uv0);
                    msh.UVs.Add(uv1);
                    msh.UVs.Add(uv2);
                    msh.Normals.Add(n0);
                    msh.Normals.Add(n0);
                    msh.Normals.Add(n0);

                    msh.Positions.Add(v0);
                    msh.Positions.Add(v2);
                    msh.Positions.Add(v3);
                    msh.UVs.Add(uv0);
                    msh.UVs.Add(uv2);
                    msh.UVs.Add(uv3);
                    msh.Normals.Add(n1);
                    msh.Normals.Add(n1);
                    msh.Normals.Add(n1);
                }
            }

            for (int i = 0; i < msh.Positions.Count; i++)
                msh.Indices.Add(i);

            //Return
            return msh;
        }

        /// <summary>
        /// Default Sphere, Normal mapping to do
        /// </summary>
        /// <param name="nMeridian"></param>
        /// <param name="nParallel"></param>
        /// <returns></returns>
        public static Mesh Default(int nMeridian, int nParallel)
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
