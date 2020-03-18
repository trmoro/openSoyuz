using System;
using System.Collections.Generic;
using System.Text;

using SKW;

namespace Soyuz
{
    public class Engine
    {
        //Core
        public static Core Core;

        //Current Instance
        public static Engine Instance;

        //Scene
        public Scene Scene { get; set; }

        //Renderer
        public Renderer Renderer { get; set; }


        /// <summary>
        /// Engine Constructor
        /// </summary>
        public Engine()
        {
            //Scene and Renderer
            Scene = null;
            Renderer = null;

            //Set Singletone
            Instance = this;

            //Init SKW Core
            Core = new Core();
            Core.Init();
        }

        /// <summary>
        /// Update the selected scene, when the core is ended, cleans up
        /// </summary>
        public void Update()
        {
            //Update Core and Engine
            while(!Core.Update() )
            {
                //Update Selected Scene
                if (Scene != null && Renderer != null)
                {
                    Scene.Update();
                    Renderer.Render();
                }
            }

            //Clean Up
            CleanUp();
        }

        /// <summary>
        /// Clean Up
        /// </summary>
        private void CleanUp()
        {
        }

        //
    }
}
