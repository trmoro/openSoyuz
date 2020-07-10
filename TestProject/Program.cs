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
            Model planet = new Model();
            planet.Name = "Planet";
            planet.Position = new Vector3(0);
            planet.Scale = new Vector3(10);

            //Set Material
            planet.Material = new Material() {
                Color = new Vector4(1),
                IsTextured = false,
                Texture = cubemap
            };

            //Add 
            planet.Meshes.Add(Quad.Cube().Compile() );
            planet.Update();
            planet.SetDrawMode(Engine.Core.Model_DrawMode_Triangles);
            s.Models.Add(planet);

            //Create Skybox
            Model skybox = new Model();
            skybox.Name = "Skybox";
            skybox.Scale = new Vector3(100);

            //Add Camera : each camera generates an image
            Camera c = new Camera(false);
            c.SetSkybox(cubemap);
            s.Cameras.Add(c);

            //Add Planet Shader
            Shader planetShader = new Shader();
            planetShader.Load("Shaders/Reflective.vs", "Shaders/Reflective.fs");
            //planetShader.LoadPrefab(Engine.Core.Prefab_Shader_Color);
            c.AddShader(planetShader, m => m.Name == "Planet");

            //Add Orbit Viewer
            OrbitViewer orbitViewer = new OrbitViewer() { Camera = c, Model = planet, Distance = 15, Speed = 0.02f };
            s.AddActor(orbitViewer);

            //Load Font
            Font font = new Font();
            font.Load("Fonts/consola.ttf", 24);

            //GUI
            GUI ui = new GUI();
            ui.Text(55, 15, new Vector4(1), "I am a text", font, 85, 1.2f);
            ui.Box(55, 10, 150, 150, new Vector4(1, 0, 0, 0.995f));
            ui.Button(10, 200, 150, 32, new Vector4(0, 0, 1, 0.995f), "Button", font);
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
