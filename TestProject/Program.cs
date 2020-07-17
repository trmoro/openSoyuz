using System;
using System.Numerics;
using Soyuz;
using Soyuz.Meshes;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {

            //Create Engine
            Engine e = new Engine();
            Engine.Core.SetWindowTitle("TestProject");

            //Create Cubemap
            Texture cubemap = new Texture();
            cubemap.LoadAsCubemap("Cubemap/right.jpg", "Cubemap/left.jpg", "Cubemap/top.jpg", "Cubemap/bottom.jpg", "Cubemap/front.jpg", "Cubemap/back.jpg");

            //Create Scene
            Scene s = new Scene();

            //Create Planet
            Model mod = new Model();
            mod.Name = "TestModel";
            mod.Position = new Vector3(0);
            mod.Scale = new Vector3(0.03f);

            //Set Material
            mod.Material = new Material() {
                Color = new Vector4(0.2f,0.7f,1,1),
                IsTextured = false,
                Texture = cubemap,
                RefractRatio = 1.0f / 1.52f
            };

            //Add 
            mod.Load("Models/medieval_house.fbx");
            //mod.Meshes.Add(Sphere.Default(512,512).Compile() );
            //mod.Meshes.Add(Sphere.TriangleVertex(512, 512).Compile());
            mod.Update();
            mod.SetDrawMode(Engine.Core.Model_DrawMode_Triangles);
            s.Models.Add(mod);

            //Add Camera : each camera generates an image
            Camera c = new Camera(false);
            c.SetSkybox(cubemap);
            s.Cameras.Add(c);

            //Add Planet Shader
            Shader shader = new Shader();
            //shader.Load("Shaders/Reflective.vs", "Shaders/Refract.fs");
            shader.LoadPrefab(Engine.Core.Prefab_Shader_Refract);
            c.AddShader(shader, m => m.Name == "TestModel");

            //Add Orbit Viewer
            OrbitViewer orbitViewer = new OrbitViewer() { Camera = c, Model = mod, Distance = 15, Speed = 0.02f };
            s.AddActor(orbitViewer);

            //Load Font
            Font font = new Font();
            font.Load("Fonts/consola.ttf", 24);

            //GUI
            GUI ui = new GUI();
            ui.Text(55, 15, new Vector4(1), "I am a text", font, 85, 1.2f);
            ui.Box(55, 10, 150, 150, new Vector4(1, 0, 0, 0.9f));
            ui.Button(10, 200, 150, 32, new Vector4(0, 0, 1, 0.9f), "Button", font);
            s.AddActor(ui);

            //Add Scene to Engine
            e.Scene = s;

            //Create Renderer
            Renderer r = new Renderer();

            //Blur Edge
            ConvolutionRender blur_edge = new ConvolutionRender(c, new Matrix4x4(1, 2, 1, 0, 2, 4, 2, 0, 1, 2, 1, 0, 0, 0, 0, 1.0f / 16.0f));
            r.RenderSteps.Add(blur_edge);

            //Mix GUI with base
            MixRender mixGUI = new MixRender(c, ui.Render, MixRender.MixOperation.B_on_A);
            r.RenderSteps.Add(mixGUI);

            //Set Frame to show
            r.Show(mixGUI);

            //Set Renderer
            e.Renderer = r;

            //Update
            e.Update();
        }
    }
}
