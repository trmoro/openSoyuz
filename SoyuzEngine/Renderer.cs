using System;
using System.Collections.Generic;
using System.Text;

namespace Soyuz
{
    public class Renderer
    {
        //Render Step to show
        private RenderStep m_show;

        //Render Steps
        public List<RenderStep> RenderSteps { get; set; }
        
        /// <summary>
        /// Renderer Constructor
        /// </summary>
        public Renderer()
        {
            RenderSteps = new List<RenderStep>();
        }

        /// <summary>
        /// Render the Scene
        /// </summary>
        public void Render()
        {
            //Render all Scene
            foreach (RenderStep st in RenderSteps)
                st.Render();

            //Show the wanted RenderStep Result
            Engine.Core.ShowFrameBuffer(m_show.FrameBufferID);
        }

        /// <summary>
        /// Select the Render Step result to show
        /// </summary>
        /// <param name="st">RenderStep</param>
        public void Show(RenderStep st)
        {
            m_show = st;
        }
    }
}
