using System;
using System.Numerics;
using Soyuz;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {

            //Create Engine
            Engine e = new Engine();

            //Create Texture
            Texture t = new Texture();
            t.SetWithPath("mandelbrot.jpg");

            //Create Scene
            Scene s = new Scene();

            //Create Actor (Update and Start Method)
            MyActor a = new MyActor();
            s.AddActor(a);

            //Create Model
            Model m = new Model();
            m.Name = "Cubo";
            m.Position = new Vector3(0);
            m.Scale = new Vector3(1);

            //Set Material
            m.Material = new Material() {
                Color = new Vector4(1f, 0.8f, 1f, 0.95f),
                Diffuse = new Vector3(0.5f, 0.5f, 0.61f),
                Specular = new Vector3(0.5f, 0.5f, 0.5f),
                Shininess = 16.0f,
                IsTextured = true,
                Texture = t
            };

            //Create Mesh
            Mesh msh = new Mesh();
            //msh.Sphere(128,128);
            msh.Cube();

            //Add To Model
            m.Meshes.Add(msh);

            //Compile Model
            m.Compile();

            //Add Model
            s.Models.Add(m);

            //Plane
            Model plane = new Model();
            plane.Name = "planeo";
            plane.Position = new Vector3(0, -3, 0);
            plane.Scale = new Vector3(20, 1, 20);

            Mesh pln = new Mesh();
            pln.Cube();
            plane.Meshes.Add(pln);

            plane.Compile();
            s.Models.Add(plane);

            //Link model to actor
            a.mod = m;
            
            //Lights
            s.Lights.Add(new DirectionalLight()
            {
                Color = new Vector3(0.12f, 0.1f, 0.25f),
                Direction = new Vector3(-1,-1,1)
            });

            s.Lights.Add(new SpotLight() {
                Color = new Vector3(0.45f, 0.35f, 0.5f),
                Position = new Vector3(3),
                Direction = new Vector3(-1),
                In_Cutoff = 0.9978f,
                Out_Cutoff = 0.953f
            });

            s.Lights.Add(new PointLight()
            {
                Color = new Vector3(0.3f,1.0f,0.4f),
                Position = new Vector3(0,0.2f,0),
                Linear = 0.35f,
                Quadratic = 0.44f
            });

            //Add Camera : each camera generates an image
            Camera c = new Camera();
            c.Position = new Vector3(2,2,-0.01f);
            c.Target = new Vector3(0);
            s.Cameras.Add(c);
            a.cam = c;

            //Create a Duplicate
            Camera d = c.CreateDuplicate();
            d.IfEmptyRenderAll = false;

            //Raycaster
            Raycaster rc = new Raycaster(c, s);
            s.AddActor(rc);
            rc.debugRS = d;
            
            //Add Scene to Engine
            e.Scene = s;

            //Create Renderer
            Renderer r = new Renderer();

            //Edge Detector
            ConvolutionRender edge_detect = new ConvolutionRender(d, new float[3, 3] { { -1, -1, -1 }, { -1, 8, -1 }, { -1, -1, -1 } }, 1);
            r.RenderSteps.Add(edge_detect);

            //Blur Edge
            ConvolutionRender blur_edge = new ConvolutionRender(edge_detect, new Matrix4x4(1, 2, 1, 0, 2, 4, 2, 0, 1, 2, 1, 0, 0, 0, 0, 1.0f / 16.0f));
            r.RenderSteps.Add(blur_edge);

            //Mix the blured edge and the mix image
            MixRender mr2 = new MixRender(blur_edge, c, MixRender.MixOperation.Add);
            r.RenderSteps.Add(mr2);

            //Set Frame to show
            r.Show(mr2);

            //Set Renderer
            e.Renderer = r;

            //Update
            e.Update();
        }
    }
}
