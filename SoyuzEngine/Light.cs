using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Soyuz
{
    //Base Light
    public class Light
    {
        //Color
        public Vector3 Color { get; set; }

        //Light Type Enumeration
        public enum LightType { None, Directional, Spot, Point };

        //Type
        public LightType Type { get; set; }

        /// <summary>
        /// Light Constructor with White Color as default
        /// </summary>
        public Light()
        {
            Color = new Vector3(1);
            Type = LightType.None;
        }

        //
    }

    //Directional Light
    public class DirectionalLight : Light
    {
        //Direction
        public Vector3 Direction { get; set; }

        /// <summary>
        /// Directional Light Constructor
        /// </summary>
        public DirectionalLight() : base()
        {
            Type = LightType.Directional;
            Direction = new Vector3(1);
        }
    }

    //Point Light
    public class PointLight : Light
    {
        //Position
        public Vector3 Position { get; set; }

        //Constant
        public float Constant { get; set; }

        //Linear
        public float Linear { get; set; }

        //Quadratic
        public float Quadratic { get; set; }

        /// <summary>
        /// Point Light Constructor with default values
        /// </summary>
        public PointLight() : base()
        {
            Type = LightType.Point;
            Position = new Vector3(0);
            Constant = 1.0f;
            Linear = 0.045f;
            Quadratic = 0.0075f;
        }
    }

    //Spot Light
    public class SpotLight : Light
    {
        //Position
        public Vector3 Position { get; set; }

        //Direction
        public Vector3 Direction { get; set; }

        //In Cut-Off
        public float In_Cutoff { get; set; }

        //Out Cut-Off
        public float Out_Cutoff { get; set; }

        /// <summary>
        /// Spot Light Constructor with default values
        /// </summary>
        public SpotLight() : base()
        {
            Type = LightType.Spot;
            Position = new Vector3(0);
            Direction = new Vector3(1);
            In_Cutoff = 0.91f;
            Out_Cutoff = 0.82f;
        }


    }

    //

}
