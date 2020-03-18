using System;
using System.Collections.Generic;
using System.Numerics;

namespace Soyuz
{
    /// <summary>
    /// Render Step Class
    /// </summary>
    public class RenderStep
    {
        //Shader ID
        public int ShaderID = -1;

        //FrameBuffer ID
        public int FrameBufferID = -1;

        //List of Model to Render
        public List<Model> Models { get; set; }

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
            ShaderID = Engine.Core.CreateShader();

            //List of Model to Render
            Models = new List<Model>();
            IfEmptyRenderAll = true;
        }

        /// <summary>
        /// Render the Scene
        /// </summary>
        public virtual void Render()
        {
            //Render
            Engine.Core.RenderInit(FrameBufferID, ShaderID);

            //If Emptry
            if (IfEmptyRenderAll)
                RenderModels(Engine.Instance.Scene.Models);
            //If not empty
            else if (Models.Count > 0)
                RenderModels(Models);
            //
        }

        //Render Models
        private void RenderModels(List<Model> models)
        {
            //Foreach Model
            foreach (Model m in models)
            {
                //Material
                Material mat = m.Material;

                //Set Material Uniform
                Engine.Core.SetUniformVec4(ShaderID, "m_color", mat.Color.X, mat.Color.Y, mat.Color.Z, mat.Color.W);
                Engine.Core.SetUniformVec3(ShaderID, "m_diffuse", mat.Diffuse.X, mat.Diffuse.Y, mat.Diffuse.Z);
                Engine.Core.SetUniformVec3(ShaderID, "m_specular", mat.Specular.X, mat.Specular.Y, mat.Specular.Z);
                Engine.Core.SetUniformF(ShaderID, "m_shininess", mat.Shininess);

                //Light List
                List<Light> ls;

                //Directional Light
                ls = Engine.Instance.Scene.Lights.FindAll(x => x.Type == Light.LightType.Directional);
                Engine.Core.SetUniformI(ShaderID, "m_nDirLight", ls.Count);
                for(int i = 0; i < ls.Count; i++)
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

                //Render
                Engine.Core.RenderModel(ShaderID, m.ModelID);
            }
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
        public enum MixOperation { Add = 0, Min = 1, Mul = 2, Div = 3, Mask = 4, MaskRGBA = 5}

        //Operation
        public MixOperation Operation;

        /// <summary>
        /// Mix Render Constructor
        /// </summary>
        /// <param name="rs1">First Render Step</param>
        /// <param name="rs2">Second Render Step</param>
        public MixRender(RenderStep rs1, RenderStep rs2, MixOperation op) : base()
        {
            //Set RenderSteps
            First = rs1;
            Second = rs2;
            Operation = op;

            //Set Prefab Shader
            Engine.Core.SetPrefabShader(ShaderID, Engine.Core.Prefab_Shader_Mix);
        }

        /// <summary>
        /// Render
        /// </summary>
        public override void Render()
        {
            //Render Init
            Engine.Core.RenderFrameBufferInit(FrameBufferID, ShaderID);

            //Set Uniform
            Engine.Core.SetUniformFrameBuffer(ShaderID, "m_t1", First.FrameBufferID, 0);
            Engine.Core.SetUniformFrameBuffer(ShaderID, "m_t2", Second.FrameBufferID, 1);
            Engine.Core.SetUniformI(ShaderID, "m_op", (int) Operation);

            //Render
            Engine.Core.RenderFrameBuffer(ShaderID);
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
            Engine.Core.SetPrefabShader(ShaderID, Engine.Core.Prefab_Shader_Conv);

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
            Engine.Core.SetPrefabShader(ShaderID, Engine.Core.Prefab_Shader_Conv);

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
            Engine.Core.RenderFrameBufferInit(FrameBufferID, ShaderID);

            //Set Uniform
            Engine.Core.SetUniformFrameBuffer(ShaderID, "m_texture", SourceRender.FrameBufferID, 0);
            Engine.Core.SetUniformMat3(ShaderID, "m_convMat", ConvMatrix[0,0], ConvMatrix[0,1], ConvMatrix[0,2]
                , ConvMatrix[1,0], ConvMatrix[1,1], ConvMatrix[1,2]
                , ConvMatrix[2,0], ConvMatrix[2,1], ConvMatrix[2,2]);
            Engine.Core.SetUniformF(ShaderID, "m_convCoef", ConvCoef);

            //Render
            Engine.Core.RenderFrameBuffer(ShaderID);
        }
    }

    //
}
