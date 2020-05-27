using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace Soyuz
{
    public class GUI : Actor
    {
        //Main Render Camera
        public Camera Camera { get; set; }

        //Font Camera Duplicate
        public Camera TextRender { get; set; }

        //Text Model List
        public List<Text> Texts { get; set; }

        //GUIElement Render Step Effect
        public List<RenderStep> ElementsFX { get; set; }

        //Text Render Step Effects
        public List<RenderStep> TextsFX { get; set; }

        //GUI Element Camera Duplicate
        public Camera ElementRender { get; set; }

        //GUIElement List
        public List<GUIElement> Elements { get; set; } 

        //Mix Render
        public MixRender MixRender { get; set; }

        //Reverse Render (Final)
        public ReverseRender Render { get; set; }

        /// <summary>
        /// Graphic User Interface Actor
        /// </summary>
        public GUI()
        {
            //Init Cameras
            Camera = new Camera() { Type = Camera.CameraType.Orthographic, Near = -127.0f, Far = 127.0f, IfEmptyRenderAll = false };
            TextRender = new Camera() { IfEmptyRenderAll = false};
            ElementRender = new Camera() { IfEmptyRenderAll = false};

            //Duplicate Camera
            Camera.Duplicates.Add(ElementRender);
            Camera.Duplicates.Add(TextRender);

            //Lists
            Elements = new List<GUIElement>();
            Texts = new List<Text>();

            //Mix Render
            MixRender = new MixRender(ElementRender, TextRender, MixRender.MixOperation.B_on_A);

            //Reverse Render (Final Render Step)
            Render = new ReverseRender(MixRender, ReverseRender.ReverseOperation.X);

            //Renderstep Effect List
            ElementsFX = new List<RenderStep>();
            TextsFX = new List<RenderStep>();

            //Init Shader
            Engine.Core.SetPrefabShader(TextRender.ShaderID, Engine.Core.Prefab_Shader_Font);
            Engine.Core.SetPrefabShader(ElementRender.ShaderID, Engine.Core.Prefab_Shader_Gui);
        }

        //Box
        public Box Box(float X, float Y, float Width, float Height, Vector4 Color)
        {
            Box b = new Box(X, Y, Width, Height, Color);
            Elements.Add(b);
            b.Depth = 10;
            return b;
        }

        //Text
        public Text Text(float X, float Y, Vector4 Color, string Text, Font Font, float MaxWidth = float.MaxValue, float LineSpacing = 1, float xOffset = 0, float yOffset = 0)
        {
            Text t = new Text(X, Y, Color, Text, Font, MaxWidth, LineSpacing, xOffset, yOffset);
            Texts.Add(t);
            t.Depth = 9;
            return t;
        }

        /// <summary>
        /// Start
        /// </summary>
        public override void Start()
        {
            
        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            //Mouse Position
            Vector2 Mouse = new Vector2((float) Engine.Core.GetMouseX(), (float) Engine.Core.GetMouseY());

            //Foreach Element
            foreach (GUIElement g in Elements)
            {
                //Update Input
                g.Update(Mouse);

                //Update Position / Scale / Rotation
                g.UpdateProperties();
            }

            //Foreach Text
            foreach (Text t in Texts)
            {
                //Update Input
                t.Update(Mouse);

                //Update Position / Scale / Rotation
                t.UpdateProperties();
            }

            //Render
            DoRender();
        }

        //Render
        private void DoRender()
        {
            //Set Lists
            ElementRender.Models = Elements.Cast<Model>().ToList();
            TextRender.Models = Texts.Cast<Model>().ToList();

            //Camera (Render GUI Element and Font)
            Camera.Render();

            //GUI Element RenderStep Effect
            foreach(RenderStep rs in ElementsFX)
                rs.Render();

            //Text RenderStep Effect
            foreach (RenderStep rs in TextsFX)
                rs.Render();

            //Mix Text and GUIElement
            if (ElementsFX.Count > 0)
                MixRender.First = ElementsFX.Last();
            if (TextsFX.Count > 0)
                MixRender.Second = TextsFX.Last();
            MixRender.Render();

            //Final Render (Reverse X)
            Render.Render();
        }

        //
    }
}
