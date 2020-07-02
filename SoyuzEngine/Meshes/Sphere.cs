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
        public static Vector3 UVToVector(double u, double v, float radius = 1)
        {
            return new Vector3((float)(Math.Cos(u) * Math.Cos(v) * radius), (float)(Math.Sin(v) * radius), (float)(Math.Sin(u) * Math.Cos(v) * radius));
        }

        /// <summary>
        /// Add a Sphere Mesh with Quad Faces
        /// </summary>
        /// <param name="nMeridian"></param>
        /// <param name="nParallel"></param>
        /// <returns></returns>
        public static Mesh QuadFaces(int nMeridian, int nParallel)
        {
            //Create Mesh
            Mesh msh = new Mesh();

            //Return
            return msh;
        }

        //Sphere (TO UPDATE)
        public static Mesh TriangleFaces(int nMeridian, int nParallel)
        {
            //Create Mesh
            Mesh msh = new Mesh();

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
                    msh.Positions.Add(UVToVector(((float)m / (float)nMeridian) * 2 * Math.PI, (float) ((p / (float)nParallel) * Math.PI - (Math.PI / 2)) ) );
                    msh.Normals.Add(UVToVector(((float)m / (float)nMeridian) * 2 * Math.PI, ((float)p / (float)nParallel) * Math.PI - (Math.PI / 2)));
                    msh.UVs.Add(new Vector2((float)m / (float)nMeridian, (float)p / (float)nParallel));

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
                        tris[5 + (vertexDone * 6)] = 0;


                    //Vertex is done
                    vertexDone += 1;
                }
            }

            //Position
            msh.Indices.AddRange(tris);

            //Return
            return msh;
        }

        //Sphere (TO UPDATE)
        public static Mesh RadiusData(int nMeridian, int nParallel, float[,] data)
        {
            //Create Mesh
            Mesh msh = new Mesh();

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
                    float radius = 1 + (data[m, p] * 0.05f);

                    msh.Positions.Add(UVToVector(((float)m / (float)nMeridian) * 2 * Math.PI, (float) (p / (float)nParallel * Math.PI - (Math.PI / 2) ), radius  ) );
                    msh.Normals.Add(UVToVector(((float)m / (float)nMeridian) * 2 * Math.PI, ((float)p / (float)nParallel) * Math.PI - (Math.PI / 2)));
                    msh.UVs.Add(new Vector2((float)p / (float)nParallel, (float)m / (float)nMeridian));


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
            msh.Indices.AddRange(tris);

            //Return
            return msh;
        }
    }
}
