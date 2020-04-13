using System;
using System.Collections.Generic;
using System.Text;

using Soyuz;
using System.Linq;

namespace TestProject
{
    public class RaycastOutline : Actor
    {
        //Raycast
        public Raycaster Raycaster { get; set; }

        //Render Step
        public RenderStep RenderStep { get; set; }

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
            //If left click
            if (RenderStep != null && Raycaster != null && Engine.Core.IsMouseClicked(0))
            {
                //Update
                Raycaster.Update();

                RenderStep.Models.Clear();
                Dictionary<Model, float> dictonary = Raycaster.AABB();
                if (dictonary.Count > 0)
                    RenderStep.Models.Add(dictonary.ElementAt(0).Key);
            }
        }
    }
}
