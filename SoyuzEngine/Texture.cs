using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Soyuz
{
    public class Texture
    {
        //ID
        public int ID { get; set; }

        //Width
        public int Width { get; set; }

        //Height
        public int Height { get; set; }

        //Number of Channel
        public int NumberOfChannel { get; set; }

        /// <summary>
        /// Texture Constructor
        /// </summary>
        public Texture()
        {
            //Create
            ID = Engine.Core.CreateTexture();
            Width = 0;
            Height = 0;
            NumberOfChannel = 0;
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~Texture()
        {
            Delete();
        }

        /// <summary>
        /// Set With Data Array, all Texture variables must be set before
        /// </summary>
        /// <param name="Data">Float Data Array</param>
        public void SetWithDataArray(float[] Data)
        {
            Engine.Core.SetTextureWithDataArray(ID, Width, Height, NumberOfChannel, Data);
        }

        /// <summary>
        /// Load image as texture.
        /// </summary>
        /// <param name="Path">Source Path</param>
        /// <returns>Itself</returns>
        public Texture Load(String Path, int NumberOfChannel = 3)
        {
            if (File.Exists(Path))
            {
                Engine.Core.SetTextureWithSourcePath(ID, Path, (uint)NumberOfChannel);

                this.NumberOfChannel = NumberOfChannel;
                Height = Engine.Core.GetTextureHeight(ID);
                Width = Engine.Core.GetTextureWidth(ID);

                return this;
            }
            else
                Console.WriteLine("SoyuzEngine Error : Texture file doesn't exists");

            return null;
        }

        /// <summary>
        /// Load as Cubemap
        /// </summary>
        /// <param name="Right"></param>
        /// <param name="Left"></param>
        /// <param name="Top"></param>
        /// <param name="Bottom"></param>
        /// <param name="Front"></param>
        /// <param name="Back"></param>
        /// <returns>Itself</returns>
        public Texture LoadAsCubemap(String Right, String Left, String Top, String Bottom, String Front, String Back)
        {
            //Check if paths exist
            if (File.Exists(Right) && File.Exists(Left) && File.Exists(Top) && File.Exists(Bottom) && File.Exists(Front) && File.Exists(Back))
            {
                Engine.Core.SetTextureAsCubemap(ID, Right, Left, Top, Bottom, Front, Back);
                Height = Engine.Core.GetTextureHeight(ID);
                Width = Engine.Core.GetTextureWidth(ID);
                NumberOfChannel = Engine.Core.GetTextureNumberOfChannel(ID);

                return this;
            }
            else
                Console.WriteLine("SoyuzEngine Error : One or several files don't exist, unable to load Cubemap.");

            return null;
        }

        /// <summary>
        /// Get Data Array by getting values from Kernel
        /// </summary>
        public float[] GetDataArray()
        {
            return Engine.Core.GetTextureData(ID);
        }

        /// <summary>
        /// Save To PNG
        /// </summary>
        /// <param name="FilePath"></param>
        public void SavePNG(string FilePath)
        {
            Engine.Core.SaveTexturePNG(ID, FilePath);
        }

        /// <summary>
        /// Update Texture in Core
        /// </summary>
        public void Update()
        {
            Engine.Core.UpdateTexture(ID);
        }

        /// <summary>
        /// Get Pixel Value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public float GetValue(int x, int y, uint channel)
        {
            return Engine.Core.GetTexturePixel(ID, x, y, channel);
        }

        /// <summary>
        /// Set Pixel Value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="channel"></param>
        public void SetValue(int x, int y, uint channel, float value)
        {
            Engine.Core.SetTexturePixel(ID, x, y, channel, value);
        }

        /// <summary>
        /// Convolution
        /// </summary>
        /// <param name="Size"></param>
        /// <param name="FlattenMatrix"></param>
        /// <param name="Coef"></param>
        public void Convolution(uint Size, float[] FlattenMatrix, float Coef)
        {
            Engine.Core.TextureConv(ID, Size, FlattenMatrix, Coef);
        }

        /// <summary>
        /// SubConvolution
        /// </summary>
        /// <param name="Size"></param>
        /// <param name="FlattenMatrix"></param>
        /// <param name="Coef"></param>
        /// <param name="StartX"></param>
        /// <param name="EndX"></param>
        /// <param name="StartY"></param>
        /// <param name="EndY"></param>
        public void SubConvolution(uint Size, float[] FlattenMatrix, float Coef, uint StartX, uint EndX, uint StartY, uint EndY)
        {
            Engine.Core.TextureSubConv(ID, Size, FlattenMatrix, Coef, StartX, EndX, StartY, EndY);
        }

        /// <summary>
        /// Return a Data Matrix (using only R on RGB and RGBA textures)
        /// </summary>
        /// <returns></returns>
        public float[,] GetDataMatrix()
        {
            float[,] matrix = new float[Width, Height];
            float[] tmpData = GetDataArray();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                    matrix[i, j] = tmpData[(j + (i * Height)) * NumberOfChannel];
            }
            return matrix;
        }

        /// <summary>
        /// Get Data Cube 
        /// </summary>
        /// <returns></returns>
        public float[,,] GetDataCube()
        {
            float[,,] cube = new float[Width, Height, NumberOfChannel];
            float[] tmpData = GetDataArray();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    for(int k = 0; k < NumberOfChannel; k++)
                        cube[i, j, k] = tmpData[((j + (i * Height)) * NumberOfChannel) + k];
                }
            }
            return cube;
        }

        /// <summary>
        /// Fill the texture with given value
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="NumberOfChannel"></param>
        /// <param name="value"></param>
        public void Fill(int Width, int Height, int NumberOfChannel, float Value)
        {
            //Set Value
            this.Width = Width;
            this.Height = Height;
            this.NumberOfChannel = NumberOfChannel;

            //Fill
            Engine.Core.SetTextureFilled(ID, Width, Height, NumberOfChannel, Value);
        }

        /// <summary>
        /// Apply Given Transformation
        /// </summary>
        /// <param name="TransformID"></param>
        /// <param name="Arguments"></param>
        public void Transform(int TransformID,float[] Arguments)
        {
            Engine.Core.TextureTransform(ID, TransformID,Arguments);
        }

        /// <summary>
        /// Apply Perlin Noise Transformation
        /// </summary>
        /// <param name="Seed">Seed of the Perlin Noise</param>
        /// <param name="X">X Start</param>
        /// <param name="Y">Y Start</param>
        /// <param name="Step">Step</param>
        /// <param name="Ratio">Ratio of the transformation on the origin texture</param>
        /// <param name="Mult">Multiplier of the transformation</param>
        public void Perlin(uint Seed = 0, int X = 0, int Y = 0, float Step = 1, float Ratio = 1, float Mult = 1)
        {
            Transform(Engine.Core.TexTransform_Perlin, new float[6] { Seed, X, Y, Step, Ratio, Mult });
        }

        /// <summary>
        /// Paint Border Pixel
        /// </summary>
        /// <param name="Size"></param>
        /// <param name="Value"></param>
        public void Border(uint Size = 1, float Value = 0.0f)
        {
            Transform(Engine.Core.TexTransform_Border, new float[2] { Size, Value });
        }

        /// <summary>
        /// Apply an "Alpha" Curve, function to explain
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <param name="Alpha"></param>
        public void Curve(float Start, float End, float Alpha)
        {
            Transform(Engine.Core.TexTransform_Curve, new float[3] { Start, End, Alpha });
        }

        /// <summary>
        /// Rescale
        /// </summary>
        /// <param name="MinValue"></param>
        /// <param name="MaxValue"></param>
        public void Rescale(float MinValue, float MaxValue)
        {
            Transform(Engine.Core.TexTransform_Rescale, new float[2] { MinValue, MaxValue });
        }

        /// <summary>
        /// Zone Rescale
        /// </summary>
        /// <param name="Min"></param>
        /// <param name="Max"></param>
        /// <param name="NewMin"></param>
        /// <param name="NewMax"></param>
        public void ZoneRescale(float Min, float Max, float NewMin, float NewMax)
        {
            Transform(Engine.Core.TexTransform_ZoneRescale, new float[4] { Min, Max, NewMin, NewMax });
        }


        /// <summary>
        /// Delete
        /// </summary>
        public void Delete()
        {
            Engine.Core.DeleteTexture(ID);
        }

        //
    }
}
