using System;
using System.Collections.Generic;
using System.Text;

namespace Soyuz
{
    public class Font
    {
        //ID
        public int ID { get; set; }

        /// <summary>
        /// Create Font
        /// </summary>
        public Font()
        {
            ID = Engine.Core.CreateFont();
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~Font()
        {
            Delete();
        }

        /// <summary>
        /// Load Font
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Size"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        public void Load(String Path, uint Size, uint Start = 0, uint End = 255)
        {
            Engine.Core.LoadFont(ID, Path, Size, Start, End);
        }

        /// <summary>
        /// Delete
        /// </summary>
        public void Delete()
        {
            Engine.Core.DeleteFont(ID);
        }

        //
    }
}
