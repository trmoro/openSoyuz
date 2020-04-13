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
            t.SetWithPath("pm.png",1);
            float[,] tData = new float[t.Width, t.Height];
            float[] tmpD = t.GetDataArray();
            for (int i = 0; i < t.Width; i++)
            {
                for (int j = 0; j < t.Height; j++)
                    tData[i, j] = tmpD[j + (i * t.Height)];
            }

            //5x5 conv
            float[,] convData = (float[,]) tData.Clone();
            for(int i = 1; i < t.Width - 1; i++)
            {
                for(int j = 1; j < t.Height - 1; j++)
                {
                    convData[i, j] = 0;
                    for(int u = -1; u < 2; u++)
                    {
                        for (int v = -1; v < 2; v++)
                            convData[i, j] += convData[i + u, j + v];
                    }
                    convData[i,j] /= 12.0f;
                }
            }
            
            float[] convDataFlat = new float[t.Width * t.Height];
            for(int i = 0; i < t.Width; i++)
            {
                for (int j = 0; j < t.Height; j++)
                    convDataFlat[j + (i * t.Height)] = convData[i, j];
            }


            Texture t2 = new Texture();
            t2.Width = t.Width;
            t2.Height = t.Height;
            t2.NumberOfChannel = 1;
            t2.SetWithDataArray(convDataFlat);
            t2.SavePNG("convpm.png");

            //Create Scene
            Scene s = new Scene();

            //Create Actor (Update and Start Method)
            MyActor a = new MyActor();
            s.AddActor(a);

            //Create Model
            Model planet = new Model();
            planet.Name = "Planet";
            planet.Position = new Vector3(0);
            planet.Scale = new Vector3(1);

            //Set Material
            planet.Material = new Material() {
                Color = new Vector4(1f, 1, 1f, 1f),
                Diffuse = new Vector3(0.5f, 0.5f, 0.61f),
                Specular = new Vector3(0.5f, 0.5f, 0.5f),
                Shininess = 16.0f
            };

            //Create Mesh
            Mesh msh = new Mesh();
            msh.SphereData(t.Width,t.Height, convData);
            //msh.Cube();

            //Add To Model
            planet.Meshes.Add(msh);

            //Compile Model
            planet.Compile();

            //Add Model
            s.Models.Add(planet);

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
            a.mod = planet;
            
            //Lights
            s.Lights.Add(new DirectionalLight()
            {
                Color = new Vector3(0.76f, 0.7f, 0.75f),
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
            c.NoRenderModels.Add(planet);

            //Create a Duplicate
            Camera d = new Camera();
            c.Duplicates.Add(d);
            d.IfEmptyRenderAll = false;

            //Create another Duplicate
            Camera pc = new Camera("Shaders/Planet.vs", "Shaders/Planet.fs");
            c.Duplicates.Add(pc);
            pc.IfEmptyRenderAll = false;
            pc.Models.Add(planet);

            //Raycaster
            RaycastOutline raycastOutline = new RaycastOutline() { Raycaster = new Raycaster(c, s), RenderStep = d } ;
            s.AddActor(raycastOutline);
            
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

            //Set Frame to show
            r.Show(mr3);

            //Set Renderer
            e.Renderer = r;

            //Update
            e.Update();
        }
    }
}
