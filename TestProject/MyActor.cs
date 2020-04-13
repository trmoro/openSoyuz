using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using Soyuz;

namespace TestProject
{
    public class MyActor : Actor
    {
        public Model mod { get; set; }

        public Camera cam { get; set; }

        public override void Start()
        {
            //Console.WriteLine("Start MyActor");
        }

        public override void Update()
        {

            if (Engine.Core.IsKeyPressed((sbyte) 'd') )
                mod.Rotation += new Vector3(0.02f, 0.0f, 0);
            if (Engine.Core.IsKeyPressed((sbyte)'q'))
                mod.Rotation += new Vector3(-0.02f, 0.0f, 0);
            if (Engine.Core.IsKeyPressed((sbyte)'z'))
                mod.Rotation += new Vector3(0, 0.0f, 0.02f);
            if (Engine.Core.IsKeyPressed((sbyte)'s'))
                mod.Rotation += new Vector3(0, 0.0f, -0.02f);

        }
    }
}
