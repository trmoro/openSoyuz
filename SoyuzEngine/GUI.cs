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

        //AutoZ Step
        public const float AutoZStep = -0.001f;

        //Z Index Start
        public const float ZIndexStart = 127.0f;

        //Z Index
        public float ZIndex { get; set; }

        /// <summary>
        /// Graphic User Interface Actor
        /// </summary>
        public GUI()
        {
            //Init Cameras and Z Index
            ZIndex = ZIndexStart;
            Camera = new Camera() { Type = Camera.CameraType.Orthographic, Near = -ZIndex, Far = ZIndex, IfEmptyRenderAll = false };
            TextRender = new Camera(false) { IfEmptyRenderAll = false};
            ElementRender = new Camera(false) { IfEmptyRenderAll = false};

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
            TextRender.AddPrefabShader(Engine.Core.Prefab_Shader_Font, m => !m.IsHidden);
            ElementRender.AddPrefabShader(Engine.Core.Prefab_Shader_Gui, m => !m.IsHidden);
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="e"></param>
        public void Remove(GUIElement e)
        {
            foreach (GUIElement c in e.Children)
                Elements.Remove(c);
            Elements.Remove(e);
        }

        /// <summary>
        /// Remove Text
        /// </summary>
        /// <param name="t"></param>
        public void Remove(Text t)
        {
            foreach (Text c in t.Children)
                Texts.Remove(c);
            Texts.Remove(t);
        }

        /// <summary>
        /// Show
        /// </summary>
        /// <param name="e"></param>
        public void Show(GUIElement e)
        {
            foreach (GUIElement c in e.Children)
                c.IsHidden = false;
            e.IsHidden = false;
        }

        /// <summary>
        /// Show text
        /// </summary>
        /// <param name="t"></param>
        public void Show(Text t)
        {
            foreach (Text c in t.Children)
                c.IsHidden = false;
            t.IsHidden = false;
        }

        /// <summary>
        /// Hide
        /// </summary>
        /// <param name="e"></param>
        public void Hide(GUIElement e)
        {
            foreach (GUIElement c in e.Children)
                c.IsHidden = true;
            e.IsHidden = true;
        }

        /// <summary>
        /// Hide text
        /// </summary>
        /// <param name="t"></param>
        public void Hide(Text t)
        {
            foreach (Text c in t.Children)
                c.IsHidden = true;
            t.IsHidden = true;
        }

        /// <summary>
        /// Box
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Color"></param>
        /// <returns></returns>
        public Box Box(float X, float Y, float Width, float Height, Vector4 Color)
        {
            Box b = new Box(X, Y, Width, Height, Color);
            Elements.Add(b);

            b.Depth = ZIndex;
            ZIndex += AutoZStep;

            return b;
        }

        /// <summary>
        /// Text
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Color"></param>
        /// <param name="Text"></param>
        /// <param name="Font"></param>
        /// <param name="MaxWidth"></param>
        /// <param name="LineSpacing"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <returns></returns>
        public Text Text(float X, float Y, Vector4 Color, string Text, Font Font, float MaxWidth = float.MaxValue, float LineSpacing = 1, float xOffset = 0, float yOffset = 0)
        {
            Text t = new Text(X, Y, Color, Text, Font, MaxWidth, LineSpacing, xOffset, yOffset);
            Texts.Add(t);

            t.Depth = ZIndex;
            ZIndex += AutoZStep;

            return t;
        }

        public GUIElement Button(float X, float Y, float Width, float Height, Vector4 Color, string Text, Font Font, float LineSpacing = 1, float xOffset = 0, float yOffset = 0)
        {
            //Box
            Box b = new Box(X, Y, Width, Height, Color);
            Elements.Add(b);
            b.Depth = ZIndex;
            ZIndex += AutoZStep;

            //Text
            Text t = new Text(X, Y, new Vector4(1), Text, Font, Width, LineSpacing, xOffset, yOffset);
            Texts.Add(t);
            t.Depth = ZIndex;
            ZIndex += AutoZStep;

            //Add Text to Box
            b.Children.Add(t);

            //Return Box
            return b;
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
            ElementRender.Models = Elements.Where(e => e.IsHidden == false).Cast<Model>().ToList();
            TextRender.Models = Texts.Where(e => e.IsHidden == false).Cast<Model>().ToList();

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
