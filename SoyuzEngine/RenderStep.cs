using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

namespace Soyuz
{
    /// <summary>
    /// Render Step Class
    /// </summary>
    public class RenderStep
    {
        //Shader
        public Shader Shader;

        //FrameBuffer ID
        public int FrameBufferID = -1;

        //List of Model to Render
        public List<Model> Models { get; set; }

        //List of Model to NOT Render
        public List<Model> NoRenderModels { get; set; }

        //Bool to set the behaviour of the render step when the Model list is empty
        public bool IfEmptyRenderAll { get; set; }
            
        /// <summary>
        /// Render Step Constructor
        /// </summary>
        public RenderStep()
        {
            //Create FrameBuffer
            FrameBufferID = Engine.Core.CreateFrameBuffer();

            //Create Shader
            Shader = new Shader();

            //List of Model to Render and not
            Models = new List<Model>();
            NoRenderModels = new List<Model>();
            IfEmptyRenderAll = true;
        }

        /// <summary>
        /// Render the Scene
        /// </summary>
        public virtual void Render()
        {
            //Render
            Engine.Core.UseFramebuffer(FrameBufferID);
            Engine.Core.UseShader(Shader.ID);

            //If Empty
            if (IfEmptyRenderAll)
                RenderModels(Engine.Instance.Scene.Models, Shader.ID);
            //If not empty
            else if (Models.Count > 0)
                RenderModels(Models, Shader.ID);

            //Render Skybox (Kernel will detected if there is or not a skybox)
            Engine.Core.RenderSkybox(FrameBufferID);
        }

        /// <summary>
        /// Set Skybox with a Cubemap Texture
        /// </summary>
        /// <param name="Cubemap"></param>
        public void SetSkybox(Texture Cubemap)
        {
            Engine.Core.SetSkybox(FrameBufferID, Cubemap.ID);
        }

        /// <summary>
        /// Disable Skybox
        /// </summary>
        public void DisableSkybox()
        {
            Engine.Core.DisableSkybox(FrameBufferID);
        }

        //Render Models
        protected void RenderModels(List<Model> models, int ShaderID)
        {
            //Foreach Model
            foreach (Model m in models)
            {
                //Check if we have to render model
                if (!NoRenderModels.Contains(m) && !m.IsHidden)
                {
                    //Material
                    Material mat = m.Material;

                    //Set Material Uniform
                    Engine.Core.SetUniformVec4(ShaderID, "m_color", mat.Color.X, mat.Color.Y, mat.Color.Z, mat.Color.W);
                    Engine.Core.SetUniformVec3(ShaderID, "m_diffuse", mat.Diffuse.X, mat.Diffuse.Y, mat.Diffuse.Z);
                    Engine.Core.SetUniformVec3(ShaderID, "m_specular", mat.Specular.X, mat.Specular.Y, mat.Specular.Z);
                    Engine.Core.SetUniformF(ShaderID, "m_shininess", mat.Shininess);

                    //Set Material Texture Uniform
                    if (mat.IsTextured)
                        Engine.Core.SetUniformI(ShaderID, "m_isTextured", 1);
                    else
                        Engine.Core.SetUniformI(ShaderID, "m_isTextured", 0);
                    if(mat.Texture != null)
                        Engine.Core.SetUniformTexture(ShaderID, "m_texture", mat.Texture.ID, 0);

                    //Light List
                    List<Light> ls;

                    //Directional Light
                    ls = Engine.Instance.Scene.Lights.FindAll(x => x.Type == Light.LightType.Directional);
                    Engine.Core.SetUniformI(ShaderID, "m_nDirLight", ls.Count);
                    for (int i = 0; i < ls.Count; i++)
                    {
                        DirectionalLight dl = (DirectionalLight)ls[i];
                        Engine.Core.SetUniformVec3(ShaderID, "m_dirLights[" + i + "].color", dl.Color.X, dl.Color.Y, dl.Color.Z);
                        Engine.Core.SetUniformVec3(ShaderID, "m_dirLights[" + i + "].dir", dl.Direction.X, dl.Direction.Y, dl.Direction.Z);
                    }

                    //Point Light
                    ls = Engine.Instance.Scene.Lights.FindAll(x => x.Type == Light.LightType.Point);
                    Engine.Core.SetUniformI(ShaderID, "m_nPointLight", ls.Count);
                    for (int i = 0; i < ls.Count; i++)
                    {
                        PointLight pl = (PointLight)ls[i];
                        Engine.Core.SetUniformVec3(ShaderID, "m_pointLights[" + i + "].color", pl.Color.X, pl.Color.Y, pl.Color.Z);
                        Engine.Core.SetUniformVec3(ShaderID, "m_pointLights[" + i + "].pos", pl.Position.X, pl.Position.Y, pl.Position.Z);
                        Engine.Core.SetUniformF(ShaderID, "m_pointLights[" + i + "].constant", pl.Constant);
                        Engine.Core.SetUniformF(ShaderID, "m_pointLights[" + i + "].linear", pl.Linear);
                        Engine.Core.SetUniformF(ShaderID, "m_pointLights[" + i + "].quadratic", pl.Quadratic);
                    }

                    //Spot Light
                    ls = Engine.Instance.Scene.Lights.FindAll(x => x.Type == Light.LightType.Spot);
                    Engine.Core.SetUniformI(ShaderID, "m_nSpotLight", ls.Count);
                    for (int i = 0; i < ls.Count; i++)
                    {
                        SpotLight sl = (SpotLight)ls[i];
                        Engine.Core.SetUniformVec3(ShaderID, "m_spotLights[" + i + "].color", sl.Color.X, sl.Color.Y, sl.Color.Z);
                        Engine.Core.SetUniformVec3(ShaderID, "m_spotLights[" + i + "].pos", sl.Position.X, sl.Position.Y, sl.Position.Z);
                        Engine.Core.SetUniformVec3(ShaderID, "m_spotLights[" + i + "].dir", sl.Direction.X, sl.Direction.Y, sl.Direction.Z);
                        Engine.Core.SetUniformF(ShaderID, "m_spotLights[" + i + "].in_cutoff", sl.In_Cutoff);
                        Engine.Core.SetUniformF(ShaderID, "m_spotLights[" + i + "].out_cutoff", sl.Out_Cutoff);
                    }

                    //Custom Model Uniform

                    //Int
                    foreach (string u in m.UniformInt.Keys)
                        Engine.Core.SetUniformI(ShaderID, u, m.UniformInt[u]);
                    //Float
                    foreach (string u in m.UniformFloat.Keys)
                        Engine.Core.SetUniformF(ShaderID, u, m.UniformFloat[u]);
                    //Vec2
                    foreach (string u in m.UniformVec2.Keys)
                        Engine.Core.SetUniformVec2(ShaderID, u, m.UniformVec2[u].X, m.UniformVec2[u].Y);
                    //Vec3
                    foreach (string u in m.UniformVec3.Keys)
                        Engine.Core.SetUniformVec3(ShaderID, u, m.UniformVec3[u].X, m.UniformVec3[u].Y, m.UniformVec3[u].Z);
                    //Vec4
                    foreach (string u in m.UniformVec4.Keys)
                        Engine.Core.SetUniformVec4(ShaderID, u, m.UniformVec4[u].X, m.UniformVec4[u].Y, m.UniformVec4[u].Z, m.UniformVec4[u].W);
                    //Font
                    foreach (string u in m.UniformFont.Keys)
                        Engine.Core.SetUniformFont(ShaderID, u, m.UniformFont[u].ID, 0);

                    //Render
                    Engine.Core.RenderModel(ShaderID, m.ModelID);
                }

                //
            }
        }

    }

    /// <summary>
    /// Multi Shader Render Step
    /// </summary>
    public class MultiShaderRender : RenderStep
    {
        //Shader Map
        public Dictionary<Shader, Func<Model,bool> > ShaderMap { get; set; }

        /// <summary>
        /// Multi Shader Render
        /// </summary>
        public MultiShaderRender()
        {
            //Create FrameBuffer
            FrameBufferID = Engine.Core.CreateFrameBuffer();

            //ShaderMap
            ShaderMap = new Dictionary<Shader, Func<Model, bool>>();

            //List of Model to Render and not
            Models = new List<Model>();
            NoRenderModels = new List<Model>();
            IfEmptyRenderAll = true;
        }

        /// <summary>
        /// Add Shader with condition
        /// </summary>
        /// <param name="Shader"></param>
        /// <param name="Condition"></param>
        public void AddShader(Shader Shader, Func<Model,bool> Condition)
        {
            ShaderMap[Shader] = Condition;
        }

        /// <summary>
        /// Render the Scene
        /// </summary>
        public override void Render()
        {
            //Set Framebuffer to use
            Engine.Core.UseFramebuffer(FrameBufferID);

            //Render
            foreach (Shader s in ShaderMap.Keys)
            {
                //Set Shader to use
                Engine.Core.UseShader(s.ID);

                //If Empty
                if (IfEmptyRenderAll)
                    RenderModels(Engine.Instance.Scene.Models.Where(ShaderMap[s]).ToList(), s.ID);
                //If not empty
                else if (Models.Count > 0)
                    RenderModels(Models.Where(ShaderMap[s]).ToList(), s.ID);
            }

            //Render Skybox (Kernel will detected if there is or not a skybox)
            Engine.Core.RenderSkybox(FrameBufferID);

        }
    }


    /// <summary>
    /// Mix two Render Step
    /// </summary>
    public class MixRender : RenderStep
    {
        
        //First RenderStep
        public RenderStep First { get; set; }
        
        //Second RenderStep
        public RenderStep Second { get; set; }

        //Mix Operation
        public enum MixOperation { Add = 0, Min = 1, Mul = 2, Div = 3, Mask = 4, MaskRGBA = 5, B_on_A = 6}

        //Operation
        public MixOperation Operation;

        /// <summary>
        /// Mix Render Constructor
        /// </summary>
        /// <param name="rs1">First Render Step</param>
        /// <param name="rs2">Second Render Step</param>
        public MixRender(RenderStep rsA, RenderStep rsB, MixOperation op) : base()
        {
            //Set RenderSteps
            First = rsA;
            Second = rsB;
            Operation = op;

            //Set Prefab Shader
            Engine.Core.SetPrefabShader(Shader.ID, Engine.Core.Prefab_Shader_Mix);
        }

        /// <summary>
        /// Render
        /// </summary>
        public override void Render()
        {
            //Render Init
            Engine.Core.RenderFrameBufferInit(FrameBufferID, Shader.ID);

            //Set Uniform
            Engine.Core.SetUniformFrameBuffer(Shader.ID, "m_t1", First.FrameBufferID, 0);
            Engine.Core.SetUniformFrameBuffer(Shader.ID, "m_t2", Second.FrameBufferID, 1);
            Engine.Core.SetUniformI(Shader.ID, "m_op", (int) Operation);

            //Render
            Engine.Core.RenderFrameBuffer(Shader.ID);
        }
    }

    /// <summary>
    /// Convolution Rendering with 3x3 Matrix and a Coef
    /// </summary>
    public class ConvolutionRender : RenderStep
    {
        //3x3 Convolution Matrix
        public float[,] ConvMatrix { get; set; }

        //Convolution Coef
        public float ConvCoef { get; set; }

        //Source RenderStep
        public RenderStep SourceRender { get; set; }

        /// <summary>
        /// Convolution Render
        /// </summary>
        /// <param name="source">Source RenderStep</param>
        /// <param name="mat">4x4 Matrix, 3x3 Convolution Matrix and the coef at the bottom left</param>
        public ConvolutionRender(RenderStep source, Matrix4x4 mat) : base()
        {
            //Set Prefab Shader
            Engine.Core.SetPrefabShader(Shader.ID, Engine.Core.Prefab_Shader_Conv);

            SourceRender = source;
            ConvMatrix = new float[3,3]{ { mat.M11, mat.M12, mat.M13},{ mat.M21, mat.M22, mat.M23 },{ mat.M31, mat.M32, mat.M33 } };
            ConvCoef = mat.M44;
        }

        /// <summary>
        /// Convolution Render Constructor
        /// </summary>
        /// <param name="source">Source RenderStep</param>
        /// <param name="mat3x3">3x3 Convolution Matrix</param>
        /// <param name="coef">Convolution Coef</param>
        public ConvolutionRender(RenderStep source, float[,] mat3x3, float coef)
        {
            //Set Prefab Shader
            Engine.Core.SetPrefabShader(Shader.ID, Engine.Core.Prefab_Shader_Conv);

            SourceRender = source;
            ConvMatrix = mat3x3;
            ConvCoef = coef;
        }

        /// <summary>
        /// Render
        /// </summary>
        public override void Render()
        {
            //Render Init
            Engine.Core.RenderFrameBufferInit(FrameBufferID, Shader.ID);

            //Set Uniform
            Engine.Core.SetUniformFrameBuffer(Shader.ID, "m_texture", SourceRender.FrameBufferID, 0);
            Engine.Core.SetUniformMat3(Shader.ID, "m_convMat", ConvMatrix[0,0], ConvMatrix[0,1], ConvMatrix[0,2]
                , ConvMatrix[1,0], ConvMatrix[1,1], ConvMatrix[1,2]
                , ConvMatrix[2,0], ConvMatrix[2,1], ConvMatrix[2,2]);
            Engine.Core.SetUniformF(Shader.ID, "m_convCoef", ConvCoef);

            //Render
            Engine.Core.RenderFrameBuffer(Shader.ID);
        }
    }

    /// <summary>
    /// Reverse Render
    /// </summary>
    public class ReverseRender : RenderStep
    {

        //RenderStep to Reverse
        public RenderStep SourceRender { get; set; }

        //Reverse Operation
        public enum ReverseOperation { X = 0, Y = 1, XY = 2 }

        //Operation
        public ReverseOperation Operation;

        /// <summary>
        /// Reverse Renderer
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="op"></param>
        public ReverseRender(RenderStep Source, ReverseOperation op) : base()
        {
            //Set RenderSteps
            SourceRender = Source;
            Operation = op;

            //Set Prefab Shader
            Engine.Core.SetPrefabShader(Shader.ID, Engine.Core.Prefab_Shader_Reverse);
        }

        /// <summary>
        /// Render
        /// </summary>
        public override void Render()
        {
            //Render Init
            Engine.Core.RenderFrameBufferInit(FrameBufferID, Shader.ID);

            //Set Uniform
            Engine.Core.SetUniformFrameBuffer(Shader.ID, "m_source", SourceRender.FrameBufferID, 0);
            Engine.Core.SetUniformI(Shader.ID, "m_op", (int)Operation);

            //Render
            Engine.Core.RenderFrameBuffer(Shader.ID);
        }
    }

    //
}
