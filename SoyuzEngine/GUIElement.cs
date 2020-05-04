using System;
using System.Collections.Generic;
using System.Text;

using System.Numerics;

namespace Soyuz
{
    //GUIElement Base Class
    public class GUIElement : Model
    {
        //X
        public float X { get; set; }

        //Y
        public float Y { get; set; }

        //Width
        public float Width { get; set; }

        //Height
        public float Height { get; set; }

        //Color
        public Vector4 Color { get; set; }

        //Depth
        public float Depth { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GUIElement() : base()
        {
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
            Depth = 0;
            Color = new Vector4(1);
        }

        //Update
        public virtual void Update(Vector2 Mouse)
        {
            //Auto Update Position and Color
            Position = new Vector3(X, Y, Depth);
            Material.Color = Color;
        }

        //
    }

    //Text
    public class Text : GUIElement
    {
        //Text Constructor
        public Text(float X, float Y, Vector4 Color, string Text, Font Font, float MaxWidth = float.MaxValue, float LineSpacing = 1, float xOffset = 0, float yOffset = 0) : base()
        {
            //Set Value
            this.X = X;
            this.Y = Y;
            this.Color = Color;

            //Set Text
            Engine.Core.AddTextAsMesh(Font.ID, ModelID, Text, xOffset, yOffset, MaxWidth, LineSpacing);
            UniformFont.Add("m_font", Font);
            Compile();
        }

        //Update
        public override void Update(Vector2 Mouse)
        {
            //Call Base Update
            base.Update(Mouse);

            //Compare Changes to update (TO DO)
        }

        //
    }

    //Box
    public class Box : GUIElement
    {

        /// <summary>
        /// Box Constructor
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Color"></param>
        public Box(float X, float Y, float Width, float Height, Vector4 Color) : base()
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Color = Color;

            Mesh rect = new Mesh();
            rect.Rect();
            Meshes.Add(rect);
            Compile();
        }

        //Update
        public override void Update(Vector2 Mouse)
        {
            //Call Base Update
            base.Update(Mouse);

            //Auto-resize Box
            Scale = new Vector3(Width, Height, 1);
        }

        //

    }

    //
}
