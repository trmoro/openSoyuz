using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using Soyuz;

namespace TestProject
{
    public class OrbitViewer : Actor
    {
        //Camera
        public Camera Camera { get; set; }

        //Model
        public Model Model { get; set; }

        //Center
        public Vector3 Center { get; set; }

        //Distance from center
        public float Distance { get; set; }

        //"U" axis value
        public float U { get; set; }

        //"V" axis value
        public float V { get; set; }

        //Speed
        public float Speed { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrbitViewer()
        {
            Model = null;
            Camera = null;
            Center = new Vector3(0);
            Distance = 10;
            U = 0;
            V = 0;
            Speed = 0.02f;
        }

        /// <summary>
        /// Starting
        /// </summary>
        public override void Start()
        {
            
        }

        /// <summary>
        /// Updating
        /// </summary>
        public override void Update()
        {
            //If variables set
            if(Model != null && Camera != null)
            {
                // U / V Positions
                if (Engine.Core.IsKeyPressed((sbyte)'d'))
                    U += Speed;
                if (Engine.Core.IsKeyPressed((sbyte)'q'))
                    U -= Speed;
                if (Engine.Core.IsKeyPressed((sbyte)'z'))
                    V += Speed;
                if (Engine.Core.IsKeyPressed((sbyte)'s'))
                    V -= Speed;
                
                //Distance
                if (Engine.Core.IsKeyPressed((sbyte)'a'))
                    Distance += Speed;
                if (Engine.Core.IsKeyPressed((sbyte)'e'))
                    Distance -= Speed;

                //Update Position
                Camera.Target = Center;
                Camera.Position = Center + new Vector3((float)(Math.Cos(U) * Math.Cos(V) * Distance), (float)(Math.Sin(V) * Distance), (float)(Math.Sin(U) * Math.Cos(V) * Distance));
            }

            //
        }

        //
    }
}
