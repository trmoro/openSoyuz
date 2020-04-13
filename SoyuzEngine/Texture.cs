using System;
using System.Collections.Generic;
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
        /// Set With Data Array, all Texture variables must be set before
        /// </summary>
        /// <param name="Data">Float Data Array</param>
        public void SetWithDataArray(float[] Data)
        {
            Engine.Core.SetTextureWithDataArray(ID, Width, Height, NumberOfChannel, Data);
        }

        /// <summary>
        /// Set With Path
        /// </summary>
        /// <param name="Path">Source Path</param>
        public void SetWithPath(String Path, int NumberOfChannel=3)
        {
            Engine.Core.SetTextureWithSourcePath(ID, Path, (uint) NumberOfChannel);

            this.NumberOfChannel = NumberOfChannel;
            Height = Engine.Core.GetTextureHeight(ID);
            Width = Engine.Core.GetTextureWidth(ID);
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

        //
    }
}
