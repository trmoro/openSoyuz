﻿using System;
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

        //Is Hovered
        public bool IsHovered { get; set; }
            
        //Is Clicked
        public bool IsClicked { get; set; }

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
            ResetMouseEvent();
        }

        //Update
        public virtual void Update(Vector2 Mouse)
        {
            //Reset all Mouse Event
            ResetMouseEvent();

            //Auto Update Position and Color
            Position = new Vector3(X, Y, Depth);
            Material.Color = Color;

            //Check if mouse is in
            if(Mouse.X >= X && Mouse.X <= X + Width && Mouse.Y >= Y && Mouse.Y <= Y + Height)
            {
                IsHovered = true;

                //Left Click
                if (Engine.Core.IsMouseClicked(0))
                    IsClicked = true;
            }
            
        }

        /// <summary>
        /// Copy Mouse Event
        /// </summary>
        /// <param name="g"></param>
        public void CopyMouseEvent(GUIElement g)
        {
            IsHovered = g.IsHovered;
            IsClicked = g.IsClicked;
        }

        //Reset Mouse Event
        private void ResetMouseEvent()
        {
            IsHovered = false;
            IsClicked = false;
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
