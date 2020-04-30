﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Soyuz
{
    public class Camera : RenderStep
    {
        //Position
        public Vector3 Position { get; set; }
        
        //Target
        public Vector3 Target { get; set; }

        //Enum of Camera Type
        public enum CameraType { Perspective = 0, Orthographic = 1, OrthographicBox = 2 }

        //Type
        public CameraType Type;

        //Radius
        public float Radius { get; set; }

        //Near
        public float Near { get; set; }

        //Far
        public float Far { get; set; }

        //Orthobox Variables
        public float OB_minX { get; set; }
        public float OB_minY { get; set; }
        public float OB_minZ { get; set; }
        public float OB_maxX { get; set; }
        public float OB_maxY { get; set; }
        public float OB_maxZ { get; set; }

        //Duplicates
        public List<Camera> Duplicates;

        /// <summary>
        /// Camera Constructor with default
        /// </summary>
        public Camera() : base()
        {
            Position = new Vector3(0);
            Target = new Vector3(0, 0, 1);
            Type = CameraType.Perspective;

            //Set Prefab Shader
            Engine.Core.SetPrefabShader(ShaderID, Engine.Core.Prefab_Shader_Lighting);

            Radius = 80.0f;
            Near = 0.005f;
            Far = 100.0f;

            //Duplicates
            Duplicates = new List<Camera>();
        }

        /// <summary>
        /// Camera Constructor with Prefab
        /// </summary>
        /// <param name="PrefabID"></param>
        public Camera(int PrefabID)
        {
            Position = new Vector3(0);
            Target = new Vector3(0, 0, 1);
            Type = CameraType.Perspective;

            //Set Prefab Shader
            Engine.Core.SetPrefabShader(ShaderID, PrefabID);

            Radius = 80.0f;
            Near = 0.005f;
            Far = 100.0f;

            //Duplicates
            Duplicates = new List<Camera>();
        }

        /// <summary>
        /// Camera Constructor with Vertex and Fragment Shader Path
        /// </summary>
        /// <param name="VertexPath"></param>
        /// <param name="FragmentPath"></param>
        public Camera(string VertexPath, string FragmentPath)
        {
            Position = new Vector3(0);
            Target = new Vector3(0, 0, 1);
            Type = CameraType.Perspective;

            //Set if files exist
            if (File.Exists(VertexPath) && File.Exists(FragmentPath))
                Engine.Core.SetShader(ShaderID, File.ReadAllText(VertexPath), File.ReadAllText(FragmentPath));
            //Use default
            else
                Engine.Core.SetPrefabShader(ShaderID, Engine.Core.Prefab_Shader_Lighting);

            Radius = 80.0f;
            Near = 0.005f;
            Far = 100.0f;

            //Duplicates
            Duplicates = new List<Camera>();
        }

        /// <summary>
        /// Camera Constructor with Full Shader
        /// </summary>
        /// <param name="VertexPath"></param>
        /// <param name="GeometryPath"></param>
        /// <param name="FragmentPath"></param>
        public Camera(string VertexPath, string GeometryPath, string FragmentPath)
        {
            Position = new Vector3(0);
            Target = new Vector3(0, 0, 1);
            Type = CameraType.Perspective;

            //Set if files exist
            if (File.Exists(VertexPath) && File.Exists(GeometryPath) && File.Exists(FragmentPath))
                Engine.Core.SetShader(ShaderID, File.ReadAllText(VertexPath), File.ReadAllText(GeometryPath), File.ReadAllText(FragmentPath));
            //Use default
            else
                Engine.Core.SetPrefabShader(ShaderID, Engine.Core.Prefab_Shader_Lighting);

            Radius = 80.0f;
            Near = 0.005f;
            Far = 100.0f;

            //Duplicates
            Duplicates = new List<Camera>();
        }

        //Render
        public override void Render()
        {
            //Set Camera
            if (Type == CameraType.Perspective)
                Engine.Core.SetPerspectiveCamera(Position.X, Position.Y, Position.Z, Target.X, Target.Y, Target.Z, Radius, Near, Far);
            else if (Type == CameraType.Orthographic)
                Engine.Core.SetOrthographicCamera(Near, Far);
            else
                Engine.Core.SetOrthographicBoxCamera(Position.X, Position.Y, Position.Z, Target.X, Target.Y, Target.Z, OB_minX, OB_maxX, OB_minY, OB_maxY, OB_minZ, OB_maxZ);

            //Set Camera Position
            Engine.Core.SetUniformVec3(ShaderID, "m_camPos", Position.X, Position.Y, Position.Z);

            //Base Rendering Method
            base.Render();

            //Render Duplicates
            foreach(Camera d in Duplicates)
            {
                //Set Variables
                d.Position  = Position;
                d.Target    = Target;
                d.Type      = Type;
                d.Radius    = Radius;
                d.Near      = Near;
                d.Far       = Far;
                d.OB_minX   = OB_minX;
                d.OB_maxX   = OB_maxX;
                d.OB_minY   = OB_minY;
                d.OB_maxY   = OB_maxY;
                d.OB_minZ   = OB_minZ;
                d.OB_maxZ   = OB_maxZ;

                //Render
                d.Render();

            }
        }

        /// <summary>
        /// Move
        /// </summary>
        /// <param name="delta"></param>
        public void Move(Vector3 delta)
        {
            Position += delta;
            Target += delta;
        }
    }
}
