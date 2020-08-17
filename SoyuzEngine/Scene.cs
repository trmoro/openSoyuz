using System;
using System.Collections.Generic;
using System.Text;

namespace Soyuz
{
    public class Scene
    {
        //Actors, read only
        public List<Actor> Actors { get; }

        //Actor to add
        private List<Actor> ActorsToAdd;

        //Models
        public List<Model> Models { get; set; }

        //Lights
        public List<Light> Lights { get; set; }

        //Cameras
        public List<Camera> Cameras { get; set; }
        
        /// <summary>
        /// Scene Constructor
        /// </summary>
        public Scene()
        {
            //Actor
            Actors = new List<Actor>();
            ActorsToAdd = new List<Actor>();

            //Objects
            Models = new List<Model>();
            Lights = new List<Light>();
            Cameras = new List<Camera>();
        }

        /// <summary>
        /// Update Scene
        /// </summary>
        public void Update()
        {
            //Add SceneObjects
            foreach(Actor a in ActorsToAdd)
            {
                a.Start();
                Actors.Add(a);
            }
            ActorsToAdd.Clear();

            //Update all SceneObjects
            foreach (Actor a in Actors)
                a.Update();

            //Render
            Render();
        }

        /// <summary>
        /// Add a SceneObject
        /// </summary>
        /// <param name="o">SceneObject to add</param>
        public void AddActor(Actor a)
        {
            ActorsToAdd.Add(a);
        }

        //Render
        private void Render()
        {
            //Update Model Space Coordinate
            foreach (Model m in Models)
            {
                if(m != null && !m.IsDeleted)
                    m.UpdateProperties();
            }

            //Render All Cameras
            foreach (Camera c in Cameras)
                c.Render();
        }

        //
    }
}
