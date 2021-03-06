﻿using System;
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

        /// <summary>
        /// Add a Blur FX on Edges
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="ListOfRenderStep"></param>
        public static void EdgeBlurFX(RenderStep Start, List<RenderStep> ListOfRenderStep)
        {
            //Edge Detector
            ConvolutionRender edge_detect = new ConvolutionRender(Start, new float[3, 3] { { -1, -1, -1 }, { -1, 8, -1 }, { -1, -1, -1 } }, 1);
            ListOfRenderStep.Add(edge_detect);

            //Blur Edge
            ConvolutionRender blur_edge = new ConvolutionRender(edge_detect, new float[3, 3] { { 1, 2, 1 }, { 2, 4, 2 }, { 1, 2, 1 } }, 1.0f / 16.0f);
            ListOfRenderStep.Add(blur_edge);
        }

        //
    }
}
