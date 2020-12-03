using System;
using System.Collections.Generic;
using System.Text;

using System.Numerics;
using Soyuz.Meshes;

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

        //Is Triggered
        public bool IsTriggered { get; set; }

        //Mouse Move
        public Vector2 TriggerMouse { get; set; }
        public bool MouseMoveX { get; set; }
        public bool MouseMoveY { get; set; }

        //Reverse
        public bool Reverse { get; set; }

        //Children
        public List<GUIElement> Children { get; set; }

        //Callback
        public Action<GUIElement> OnHover;
        public Action<GUIElement> OnClick;
        public Action<GUIElement, float> OnMouseMoveX;
        public Action<GUIElement, float> OnMouseMoveY;

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
            Children = new List<GUIElement>();
            IsHidden = false;

            IsTriggered = false;
            IsClicked = false;
            IsHovered = false;
            MouseMoveX = false;
            MouseMoveY = false;
            Reverse = false;
            UniformInt.Add("Reverse", 0);
        }

        //Update
        public virtual void Update(Vector2 Mouse)
        {
            //TODO : Add scrolling

            //Show ?
            if (!IsHidden)
            {
                //Auto Update Position and Color
                Position = new Vector3(X, Y, Depth);
                Material.Color = Color;

                //Reverse
                if (Reverse == true)
                    UniformInt["Reverse"] = 1;
                else
                    UniformInt["Reverse"] = 0;

                //Check if mouse is in
                if (Mouse.X >= X && Mouse.X <= X + Width && Mouse.Y >= Y && Mouse.Y <= Y + Height)
                {
                    //Hover
                    IsHovered = true;
                    if(OnHover != null)
                        OnHover(this);

                    //Left Click to Trigger
                    if (Engine.Core.IsMouseClicked(0) && !IsTriggered)
                    {
                        IsTriggered = true;
                        TriggerMouse = Mouse;
                    }

                    //Left click released in the box
                    if (Engine.Core.IsMouseReleased(0) && IsTriggered)
                    {
                        if(OnClick != null)
                            OnClick(this);
                        IsClicked = true;
                        IsTriggered = false;
                    }
                    else
                        IsClicked = false;
                }
                //Outside the box
                else
                {
                    //Not hovered
                    IsHovered = false;

                    //Left click outside the box
                    if (Engine.Core.IsMouseReleased(0) && IsTriggered)
                        IsTriggered = false;
                }

                //Left Click during trigger
                if(Engine.Core.IsMouseClicked(0) && IsTriggered && (MouseMoveX || MouseMoveY) )
                {
                    if (MouseMoveX)
                    {
                        X += Mouse.X - TriggerMouse.X;
                        if (this.OnMouseMoveX != null)
                            OnMouseMoveX(this, Mouse.X - TriggerMouse.X);
                    }
                    if (MouseMoveY)
                    {
                        Y += Mouse.Y - TriggerMouse.Y;
                        if(this.OnMouseMoveY != null)
                            OnMouseMoveY(this, Mouse.Y - TriggerMouse.Y);
                    }
                    foreach (GUIElement e in Children)
                    {
                        if (MouseMoveX)
                        {
                            e.X += Mouse.X - TriggerMouse.X;
                            if (e.OnMouseMoveX != null)
                                e.OnMouseMoveX(this, Mouse.X - TriggerMouse.X);
                        }
                        if (MouseMoveY)
                        {
                            e.Y += Mouse.Y - TriggerMouse.Y;
                            if (e.OnMouseMoveY != null)
                                e.OnMouseMoveY(this, Mouse.Y - TriggerMouse.Y);
                        }
                    }
                    TriggerMouse = Mouse;
                }

                //Update Children
                foreach (GUIElement g in Children)
                    g.Update(Mouse);
            }
        }

        //
    }

    /// <summary>
    /// Text Class
    /// </summary>
    public class Text : GUIElement
    {
        //Text
        public string Value { get; set; }
        private string LastValue { get; set; }

        //Max Width
        public float MaxWidth { get; set; }
        private float LastMaxWidth { get; set; }

        //Line Spacing
        public float LineSpacing { get; set; }
        public float LastLineSpacing { get; set; }

        //X Offset
        public float X_Offset { get; set; }
        public float LastX_Offset { get; set; }

        //Y Offset
        public float Y_Offset { get; set; }
        public float LastY_Offset { get; set; }

        //Font
        public Font Font;
        private Font LastFont;

        //Text Constructor
        public Text(float X, float Y, Vector4 Color, string Text, Font Font, float MaxWidth = float.MaxValue, float LineSpacing = 1, float xOffset = 0, float yOffset = 0) : base()
        {
            //Set Values
            this.X = X;
            this.Y = Y;
            this.Color = Color;

            Value = Text;
            LastValue = Text;

            this.MaxWidth = MaxWidth;
            LastMaxWidth = MaxWidth;

            this.LineSpacing = LineSpacing;
            LastLineSpacing = LineSpacing;

            X_Offset = xOffset;
            LastX_Offset = xOffset;

            Y_Offset = yOffset;
            LastY_Offset = yOffset;

            this.Font = Font;
            LastFont = Font;

            //Set Text
            Engine.Core.AddTextAsMesh(Font.ID, ModelID, Text, xOffset, yOffset, MaxWidth, LineSpacing);
            UniformFont.Add("m_font", Font);
        }



        //Update
        public override void Update(Vector2 Mouse)
        {
            //Call Base Update
            base.Update(Mouse);

            //Compare Changes
            if(!Value.Equals(LastValue) || MaxWidth != LastMaxWidth || LineSpacing != LastLineSpacing || X_Offset != LastX_Offset || Y_Offset != LastY_Offset || LastFont.ID != Font.ID)
            {
                LastValue = Value;
                LastMaxWidth = MaxWidth;
                LastLineSpacing = LineSpacing;
                LastX_Offset = X_Offset;
                LastY_Offset = Y_Offset;
                LastFont = Font;
                DeleteHiddenMeshes();
                Engine.Core.AddTextAsMesh(Font.ID, ModelID, Value, X_Offset, Y_Offset, MaxWidth, LineSpacing);
            }
        }

        //
    }

    /// <summary>
    /// Box Class
    /// </summary>
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

            Meshes.Add(Quad.Rect().Compile() );
            Update();
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="Mouse"></param>
        public override void Update(Vector2 Mouse)
        {
            //Call Base Update
            base.Update(Mouse);

            //Auto-resize Box
            Scale = new Vector3(Width, Height, 1);
        }

        //

    }

    /// <summary>
    /// Button
    /// </summary>
    public class Button : GUIElement
    {
        //Text
        public Text Text { get; set; }

        /// <summary>
        /// Button Constructor
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Color"></param>
        /// <param name="Text"></param>
        /// <param name="Font"></param>
        /// <param name="LineSpacing"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        public Button(float X, float Y, float Width, float Height, Vector4 Color, string Text, Font Font, float LineSpacing = 1, float xOffset = 0, float yOffset = 0)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Color = Color;

            Meshes.Add(Quad.Rect().Compile());
            Update();

            //Text
            this.Text = new Text(X, Y, new Vector4(1), Text, Font, Width, LineSpacing, xOffset, yOffset);
            Children.Add(this.Text);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="Mouse"></param>
        public override void Update(Vector2 Mouse)
        {
            //Call Base Update
            base.Update(Mouse);

            //Auto-resize Box
            Scale = new Vector3(Width, Height, 1);

        }

    }

    /// <summary>
    /// Slider Class
    /// </summary>
    public class Slider : GUIElement
    {
        //Min Max
        public float Min { get; set; }
        public float Max { get; set; }

        //Value
        public float Value { get; set; }

        //Cursor
        public Box Cursor { get; set; }

        /// <summary>
        /// Slider
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Min"></param>
        /// <param name="Max"></param>
        /// <param name="Value"></param>
        /// <param name="Color"></param>
        public Slider(float X, float Y, float Width, float Height, float Min, float Max, float Value, Vector4 Color) : base()
        {
            this.X = X;
            this.Width = Width;
            this.Height = Height - 8;
            this.Y = Y + 4;
            if (this.Height < 4)
                this.Height = Height;

            this.Color = new Vector4(0.1f,0.1f,0.1f,0.9f);

            this.Min = Min;
            this.Max = Max;
            this.Value = Value;

            Cursor = new Box(X, Y, 8, Height, Color);
            Cursor.MouseMoveX = true;
            Children.Add(Cursor);

            Meshes.Add(Quad.Rect().Compile());
            Update();
        }

        //Update
        public override void Update(Vector2 Mouse)
        {
            //Call Base Update
            base.Update(Mouse);

            //Auto-resize Box
            Scale = new Vector3(Width, Height, 1);

            //Set Cursor Position
            if (Cursor.X < X)
                Cursor.X = X;
            else if (Cursor.X > X + Width - Cursor.Width)
                Cursor.X = X + Width - Cursor.Width;

            //Set Value by Cursor Position
            Value = (((Cursor.X - X) / (Width - Cursor.Width)) * (Max - Min)) + Min;

        }

        //

    }

    //
}
