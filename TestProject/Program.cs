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
            t.Load("Images/earth.jpg", 1);

            /*
            t.Fill(400, 400, 1, 1);
            t.Perlin(15752, 20, 1, 1);
            t.Perlin(79024, 10, 1, 1);
            t.Border(15, 0);

            float[] conv = new float[81];
            for (int i = 0; i < 81; i++)
                conv[i] = 1;
            t.Convolution(9, conv, 1.0f / 81.0f);
            t.Convolution(9, conv, 1.0f / 81.0f);
            t.Convolution(9, conv, 1.0f / 81.0f);
            */

            t.Update();
            //t.SavePNG("Images/perlin.png");

            //t.SetWithPath("test.png",1);
            //t.SavePNG("test.png");

            //Create Scene
            Scene s = new Scene();

            //Create Planet
            Model planet = new Model();
            planet.Name = "Planet";
            planet.Position = new Vector3(0);
            planet.Scale = new Vector3(10);

            //Set Material
            planet.Material = new Material() {
                Color = new Vector4(1f, 1, 1f, 1f),
                Diffuse = new Vector3(0.5f, 0.5f, 0.61f),
                Specular = new Vector3(0.5f, 0.5f, 0.5f),
                Shininess = 16.0f,
                IsTextured = false,
                Texture = t
            };

            //Create Mesh
            Mesh msh = new Mesh();
            msh.Sphere(t.Width,t.Height );

            //Add 
            planet.Meshes.Add(msh);
            planet.Compile();
            s.Models.Add(planet);

            //Load Model
            Model medHouse = new Model();
            medHouse.Scale = new Vector3(0.0025f);
            medHouse.Position = new Vector3(0, 10.5f, 0);
            medHouse.Load("Models/medieval_house.fbx");
            s.Models.Add(medHouse);

            //Light
            s.Lights.Add(new DirectionalLight()
            {
                Color = new Vector3(0.3f, 0.3f, 0.3f),
                Direction = new Vector3(-1,-1,1)
            });

            //Add Camera : each camera generates an image
            Camera c = new Camera();
            s.Cameras.Add(c);
            c.NoRenderModels.Add(planet);

            //Add Orbit Viewer
            OrbitViewer orbitViewer = new OrbitViewer() { Camera = c, Model = planet, Distance = 15, Speed = 0.02f };
            s.AddActor(orbitViewer);

            //Create a Duplicate
            Camera d = new Camera();
            c.Duplicates.Add(d);
            d.IfEmptyRenderAll = false;

            //Create another Duplicate
            Camera pc = new Camera("Shaders/Planet.vs", "Shaders/Planet.gs", "Shaders/Planet.fs");
            c.Duplicates.Add(pc);
            pc.IfEmptyRenderAll = false;
            pc.Models.Add(planet);

            //Pointer Model
            Model pointer = new Model() { Name = "pointerModel", Scale = new Vector3(0.1f) };
            Mesh pointerMesh = new Mesh();
            pointerMesh.Sphere(16, 16);
            pointer.Meshes.Add(pointerMesh);
            pointer.Compile();
            s.Models.Add(pointer);

            //Raycaster
            PointLight pl = new PointLight() {Constant = 2.0f, Linear = 1f, Quadratic = 3.6f };
            RaycastOutline raycastOutline = new RaycastOutline() { Raycaster = new Raycaster(c, s), RenderStep = d, Pointer = pl, ConstantUpdate = true } ;
            raycastOutline.Models.Add(planet);
            s.AddActor(raycastOutline);
            s.Lights.Add(pl);

            //Load Font
            Font font = new Font();
            font.Load("Fonts/consola.ttf", 14);

            //GUI
            GUI ui = new GUI();
            ui.Text(55, 15, new Vector4(1), "I am a text", font, 85, 1.2f);
            ui.Box(55, 10, 150, 150, new Vector4(1, 0, 0, 0.995f));
            s.AddActor(ui);

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

            //Replace
            MixRender mr3 = new MixRender(mr2, pc, MixRender.MixOperation.B_on_A);
            r.RenderSteps.Add(mr3);

            //Blur GUI
            //ConvolutionRender blurUI = new ConvolutionRender(ui.Render, new Matrix4x4(1, 2, 1, 0, 2, 4, 2, 0, 1, 2, 1, 0, 0, 0, 0, 1.0f / 16.0f));
            //r.RenderSteps.Add(blurUI);

            //Mix GUI with base
            MixRender mr4 = new MixRender(mr3, ui.Render, MixRender.MixOperation.B_on_A);
            r.RenderSteps.Add(mr4);

            //Set Frame to show
            r.Show(mr4);

            //Set Renderer
            e.Renderer = r;

            //Update
            e.Update();
        }
    }
}
