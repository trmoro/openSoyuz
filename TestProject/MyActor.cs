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
            mod.Rotation += new Vector3(0,0.01f,0);

            char a = 'a';
            if (Engine.Core.IsKeyPressed((sbyte) 'd') )
                cam.Move(new Vector3(0.05f, 0, 0) );
            if (Engine.Core.IsKeyPressed((sbyte)'q'))
                cam.Move(new Vector3(-0.05f, 0, 0));
            if (Engine.Core.IsKeyPressed((sbyte)'z'))
                cam.Move(new Vector3(0, 0, 0.05f));
            if (Engine.Core.IsKeyPressed((sbyte)'s'))
                cam.Move(new Vector3(0, 0, -0.05f));

        }
    }
}
