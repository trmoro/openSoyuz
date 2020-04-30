using System;
using System.Collections.Generic;
using System.Text;

using Soyuz;
using System.Linq;
using System.Numerics;

namespace TestProject
{
    public class RaycastOutline : Actor
    {
        //Raycast
        public Raycaster Raycaster { get; set; }

        //Render Step
        public RenderStep RenderStep { get; set; }

        //Pointer Light
        public PointLight Pointer { get; set; }

        //Constant Update
        public bool ConstantUpdate { get; set; }

        //Models
        public List<Model> Models { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public RaycastOutline()
        {
            Raycaster = null;
            RenderStep = null;
            Pointer = null;
            ConstantUpdate = false;
            Models = new List<Model>();
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
            //If left click
            if (RenderStep != null && Raycaster != null && (Engine.Core.IsMouseClicked(0) || ConstantUpdate) )
            {
                //Update
                Raycaster.Update();

                RenderStep.Models.Clear();
                Dictionary<Model, float> dictonary = Raycaster.BoundingSphere(Models);
                if (dictonary.Count > 0)
                {
                    RenderStep.Models.Add(dictonary.ElementAt(0).Key);

                    //Move Pointer
                    if (Pointer != null)
                        Pointer.Position = Raycaster.Camera.Position + (Raycaster.RayDirection * dictonary.ElementAt(0).Value);

                    int index = Models[0].Meshes[0].GetNearestVertexIndex(Pointer.Position);
                    Texture tex = Models[0].Material.Texture;

                    Vector2 uvpos = Models[0].Meshes[0].UVs[index] * tex.Width;

                    if (Engine.Core.IsMouseClicked(0))
                        Area(tex, uvpos, 3, -0.02f);
                    else if (Engine.Core.IsMouseClicked(1))
                        Area(tex, uvpos, 3, 0.02f);

                    //
                }
            }
            
            //
        }

        /// <summary>
        /// Apply an Circle Area Pencil
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="size"></param>
        /// <param name="dif"></param>
        private void Area(Texture tex, Vector2 origin, int size, float dif, float min = 0.25f)
        {
            //Size divided by two
            int s2 = size / 2;

            //Max distance to center
            float maxDistance = Vector2.Distance(new Vector2(s2, s2), new Vector2(0, 0));

            //Modify
            for(int u = 0; u < size; u++)
            {
                for(int v = 0; v < size; v++)
                {
                    Vector2 uv = new Vector2(origin.X - s2 + u, origin.Y - s2 + v);
                    float distance = Vector2.Distance(new Vector2(u,v), new Vector2(s2,s2));
                    float delta = dif * ( ( (maxDistance - distance) * (1.0f - min) ) + min);
                    tex.SetValue((int)uv.X, (int)uv.Y, 0, tex.GetValue((int)uv.X, (int)uv.Y, 0) + delta);

                }
            }

            //Update
            tex.Update();
        }

        //
    }
}
