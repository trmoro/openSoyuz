using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Soyuz
{
    /// <summary>
    /// Raycaster
    /// </summary>
    public class Raycaster : Actor
    {
        //Ray
        public Ray Ray { get; set; }

        //Render Ray
        public bool RenderRay { get; set; }

        //Ray Direction
        public Vector3 RayDirection { get; set; }

        //Camera
        public Camera Camera { get; set; }

        //Length
        public float Length { get; set; }

        //Scene
        public Scene Scene;

        /// <summary>
        /// Constructor
        /// </summary>
        public Raycaster(Camera c, Scene s) : base()
        {
            //Create Ray and add it to Scene
            Ray = new Ray();
            RenderRay = false;

            //Set Camera
            Camera = c;

            //Set Scene
            Scene = s;

            //Set Default Length
            Length = 100;
        }

        /// <summary>
        /// Start
        /// </summary>
        public override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            //Update Mouse To World Position
            Camera.UpdateKernelCamera();
            Engine.Core.UpdateMouseToWorld();
            RayDirection = new Vector3(Engine.Core.GetMouseToWorldX(), Engine.Core.GetMouseToWorldY(), Engine.Core.GetMouseToWorldZ());

            //Render Ray
            if (RenderRay)
            {
                //Add to Scene
                if (!Scene.Models.Contains(Ray))
                    Scene.Models.Add(Ray);

                //Update Ray
                Ray.Update(RayDirection, Length);

                //Move Ray
                Ray.Position = Camera.Position;
            }
            //Remove Ray if it was in Scene
            else if(Scene.Models.Contains(Ray))
                Scene.Models.Remove(Ray);    
        }

        //Get Interval Commons Part
        public Tuple<float,float> IntervalCommons(Tuple<float,float> a, Tuple<float,float> b)
        {
            return new Tuple<float, float>(Math.Max(a.Item1, b.Item1), Math.Min(a.Item2, b.Item2));
        }

        //Check Interval Collision and return where the commons values are in percentages
        public Tuple<float,float> IntervalCollision(float aMin, float aMax, float bMin, float bMax)
        {
            //Scaling
            float scaled_bMin = (bMin - aMin) / (aMax - aMin);
            float scaled_bMax = (bMax - aMin) / (aMax - aMin);

            //Return Min-Max Interval
            if(scaled_bMax >= scaled_bMin)
                return new Tuple<float, float>(scaled_bMin, scaled_bMax);
            else
                return new Tuple<float, float>(scaled_bMax, scaled_bMin);
        }

        //Axis-aligned Bounding Box
        public Dictionary<Model, float> AABB(List<Model> models)
        {
            //Collision Map
            Dictionary<Model, float> CollisionMap = new Dictionary<Model, float>();

            //Ray-End
            Vector3 rayEnd = Camera.Position + (RayDirection * Length);

            //For each Models
            foreach (Model m in models)
            {
                //Space Barycenter
                Vector3 spaceBarycenter = m.Position + m.Barycenter;

                //Centered Scaled-Vertex Length
                Vector3 scaledLength = Vector3.Multiply(m.VertexLength, m.Scale) / 2.0f;

                //XYZ Bounding Box Interval Collision checking
                Tuple<float, float> interX = IntervalCollision(Camera.Position.X, rayEnd.X, 
                    spaceBarycenter.X - scaledLength.X,spaceBarycenter.X + scaledLength.X);

                Tuple<float, float> interY = IntervalCollision(Camera.Position.Y, rayEnd.Y,
                    spaceBarycenter.Y - scaledLength.Y, spaceBarycenter.Y + scaledLength.Y);

                Tuple<float, float> interZ = IntervalCollision(Camera.Position.Z, rayEnd.Z,
                    spaceBarycenter.Z - scaledLength.Z, spaceBarycenter.Z + scaledLength.Z);

                Tuple<float, float> interXY = IntervalCommons(interX, interY);
                Tuple<float, float> interXYZ = IntervalCommons(interXY, interZ);

                //Collision check
                if (interXYZ.Item1 <= interXYZ.Item2 && interXYZ.Item1 >= 0 && interXYZ.Item2 <= 1)
                    CollisionMap[m] = interXYZ.Item1;

            }

            //Order By Lowest to Greatest distance and return
            CollisionMap.OrderBy(i => i.Value);
            return CollisionMap;

        }

        //Check collision with all scene models and Axis-aligned Bounding Box
        public Dictionary<Model, float> AABB()
        {
            return AABB(Scene.Models);
        }

        /// <summary>
        /// AABB on all meshes of all models (Don(t work)
        /// </summary>
        /// <param name="models">Models tested</param>
        /// <returns></returns>
        public Dictionary<Model, float> FaceAABB(List<Model> models)
        {
            //Collision Map
            Dictionary<Model, float> CollisionMap = new Dictionary<Model, float>();

            //Ray-End
            Vector3 rayEnd = Camera.Position + (RayDirection * Length);

            //For each Models
            foreach (Model m in models)
            {
                //For each meshes
                foreach (Mesh ms in m.Meshes)
                {
                    //For each triangles faces
                    foreach (Tuple<int, int, int> tris in ms.TrianglesIndices())
                    {
                        //Position
                        Vector3 t1 = ms.Positions[tris.Item1];
                        Vector3 t2 = ms.Positions[tris.Item2];
                        Vector3 t3 = ms.Positions[tris.Item3];

                        //Create AABB
                        float maxX = Math.Max(Math.Max(t1.X, t2.X), t3.X);
                        float maxY = Math.Max(Math.Max(t1.Y, t2.Y), t3.Y);
                        float maxZ = Math.Max(Math.Max(t1.Z, t2.Z), t3.Z);

                        float minX = Math.Min(Math.Min(t1.X, t2.X), t3.X);
                        float minY = Math.Min(Math.Min(t1.Y, t2.Y), t3.Y);
                        float minZ = Math.Min(Math.Min(t1.Z, t2.Z), t3.Z);

                        //XYZ Bounding Box Interval Collision checking
                        Tuple<float, float> interX = IntervalCollision(Camera.Position.X, rayEnd.X,minX,maxX);
                        Tuple<float, float> interY = IntervalCollision(Camera.Position.Y, rayEnd.Y,minY,maxY);
                        Tuple<float, float> interZ = IntervalCollision(Camera.Position.Z, rayEnd.Z,minZ,maxZ);

                        Tuple<float, float> interXY = IntervalCommons(interX, interY);
                        Tuple<float, float> interXYZ = IntervalCommons(interXY, interZ);

                        //Collision check
                        if (interXYZ.Item1 <= interXYZ.Item2 && interXYZ.Item1 >= 0 && interXYZ.Item2 <= 1)
                        {
                            //Get smallest value
                            if (CollisionMap.ContainsKey(m) && CollisionMap[m] > interXYZ.Item1)
                                CollisionMap[m] = interXYZ.Item1;
                            else if (!CollisionMap.ContainsKey(m))
                                CollisionMap[m] = interXYZ.Item1;
                        }
                    }
                }

                //

            }

            //Order By Lowest to Greatest distance and return
            CollisionMap.OrderBy(i => i.Value);
            return CollisionMap;

        }

        /// <summary>
        /// Face AABB on all models of the Scene
        /// </summary>
        /// <returns></returns>
        public Dictionary<Model, float> FaceAABB()
        {
            return FaceAABB(Scene.Models);
        }

        /// <summary>
        /// Bounding Sphere
        /// https://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-sphere-intersection
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public Dictionary<Model, float> BoundingSphere(List<Model> models)
        {
            //Collision Map
            Dictionary<Model, float> CollisionMap = new Dictionary<Model, float>();

            //For each Models
            foreach (Model m in models)
            {
                //Space Barycenter
                Vector3 spaceBarycenter = m.Position + m.Barycenter;

                //Centered Scaled-Vertex Lengt and Radius
                Vector3 scaledLength = Vector3.Multiply(m.VertexLength, m.Scale) / 2.0f;
                float radius = scaledLength.X;

                //L
                Vector3 L = spaceBarycenter - Camera.Position;

                //tCA
                float tca = Vector3.Dot(RayDirection, L);

                //Only continue if tca > 0 (no negative and at least one intersection)
                if(tca > 0)
                {
                    //d
                    double d = Math.Sqrt(L.LengthSquared() - Math.Pow(tca,2) );

                    //Check if d is smaller or equal than Sphere Radius
                    if (d <= radius)
                    {
                        //tHC
                        double thc = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(d, 2));

                        //Add t0 to Collision Map
                        CollisionMap[m] = (float) (tca - thc);
                    }

                    //
                }

            }

            //Order By Lowest to Greatest distance and return
            CollisionMap.OrderBy(i => i.Value);
            return CollisionMap;

        }

        /// <summary>
        /// Bounding Sphere with all scene's models
        /// </summary>
        /// <returns></returns>
        public Dictionary<Model, float> BoundingSphere()
        {
            return BoundingSphere(Scene.Models);
        }

        //End of Class
    }

    /// <summary>
    /// Ray Class
    /// </summary>
    public class Ray : Model
    {
        //Line Mesh
        private Mesh line;

        /// <summary>
        /// Ray Constructor
        /// </summary>
        public Ray() : base()
        {
            Material = new Material()
            {
                Color = new Vector4(1,1,1,1),
                Diffuse = new Vector3(1,1,1),
                Specular = new Vector3(1,1,1),
                Shininess = 64.0f
            };

            //Create Line
            line = new Mesh();

            line.Positions.Add(new Vector3(0));
            line.Positions.Add(new Vector3(0));

            line.Normals.Add(new Vector3(0));
            line.Normals.Add(new Vector3(0));

            line.UVs.Add(new Vector2(0));
            line.UVs.Add(new Vector2(0));

            line.Indices.Add(0);
            line.Indices.Add(1);

            //Add Line
            Meshes.Add(line);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="RayDirection"></param>
        /// <param name="length"></param>
        public void Update(Vector3 RayDirection, float length)
        {
            //Change the position of the line end-vertex
            line.Positions[1] = RayDirection;

            //Change the position of the line start-vertex
            line.Positions[0] = new Vector3(RayDirection.X, RayDirection.Y + length, RayDirection.Z);

            //Re-Compile
            line.Compile();
            Update();
        }

    }
}
