//http://blogs.msdn.com/b/shawnhar/archive/2010/06/17/drawinstancedprimitives-in-xna-game-studio-4-0.aspx
/*
 * Microsoft Permissive License (Ms-PL)

This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.

1. Definitions
The terms “reproduce,” “reproduction,” “derivative works,” and “distribution” have the same meaning here as under U.S. copyright law.
A “contribution” is the original software, or any additions or changes to the software.
A “contributor” is any person that distributes its contribution under this license.
 “Licensed patents” are a contributor’s patent claims that read directly on its contribution.

2. Grant of Rights
(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.

3. Conditions and Limitations
 (A) No Trademark License- This license does not grant you rights to use any contributors’ name, logo, or trademarks.
(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
(E) The software is licensed “as-is.” You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CelestialMechanicsSimulator.Core;
using CelestialMechanicsSimulator.Core.CelestialBodies;
namespace CelestialMechanicsSimulator.Core.InstancedCircle
{
    public class InstancedEllipse
    {
        public Game Handler { get; private set; }
        public Camera Camera { get; private set; }
        public Color EllipsoidColor { get; set; }
        public float Scale { get; set; }

        public PrimitiveType PrimitiveRenderType { get; set; }

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
        new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0),
        new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 1),
        new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 2),
        new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 3));

        public List<Transformation> Transformations;
        public Matrix[] Transforms;

        private Model EllipseFillModel;
        private Matrix[] ModelBones;
        
        private DynamicVertexBuffer DynamicVertexBuffer;
        private Effect Effect;

        public bool IsNull { get { return Transforms == null; } }

        public InstancedEllipse(Game handler, Camera camera, float scale, Color color)
        {
            Handler = handler;
            Camera = camera;

            EllipseFillModel = handler.Content.Load<Model>(@"EllipseFillModel");
            Effect = handler.Content.Load<Effect>(@"Instancing");

            Transformations = new List<Transformation>();

            ModelBones = new Matrix[EllipseFillModel.Bones.Count];
            EllipseFillModel.CopyAbsoluteBoneTransformsTo(ModelBones);

            Scale = scale;
            EllipsoidColor = color;
        }

        public void CalculateEllipsoid(float cx, float cy, float major_a, float minor_b, float scale, float err)
        {
            for (float i = 0; i < Math.PI * 2; i += err)
            {
                var x = (float)(cx + (major_a * scale) * (float)(Math.Cos(i)));
                var y = (float)(cy + (minor_b * scale) * (float)(Math.Sin(i)));
                Transformations.Add(new EllipseTransformations(new Vector3(x, 0, y)));
            }
            var x1 = (float)(cx + (major_a * scale) * (float)(Math.Cos(0)));
            var y1 = (float)(cy + (minor_b * scale) * (float)(Math.Sin(0)));
            Transformations.Add(new EllipseTransformations(new Vector3(x1, 0, y1)));

            Transforms = new Matrix[Transformations.Count];
        }

        public void CalculateEllipsoid(string name, double x, double y, double z, double vx, double vy, double vz, double dt, double period, float scale, double exponent)
        {
            double r = x;
            double ax = 0;
            double ay = 0;
            double az = 0;

            for (double i = 0; i < period; i += dt)
            {
                for (int j = 0; j < CelestialBody.InfluencingBodies.Length; j++)
                {
                    if (name != CelestialBody.InfluencingBodies[j].Name)
                    {
                        r = Math.Sqrt(Math.Pow(x - CelestialBody.InfluencingBodies[j].XCoord, 2) + Math.Pow(y - CelestialBody.InfluencingBodies[j].YCoord, 2) + Math.Pow(z - CelestialBody.InfluencingBodies[j].ZCoord, 2));
                        ax += (-CelestialBody.InfluencingBodies[j].PlanetMass * CelestialBody.GRAVITATIONCONSTANT) * (x / (Math.Pow(r, exponent)));
                        ay += (-CelestialBody.InfluencingBodies[j].PlanetMass * CelestialBody.GRAVITATIONCONSTANT) * (y / (Math.Pow(r, exponent)));
                        az += (-CelestialBody.InfluencingBodies[j].PlanetMass * CelestialBody.GRAVITATIONCONSTANT) * (z / (Math.Pow(r, exponent)));
                    }
                }

                vx += ax * dt;
                vy += ay * dt;
                vz += az * dt;

                x += vx * dt;
                y += vy * dt;
                z += vz * dt;

                ax = ay = az = 0;
                
                Transformations.Add(new EllipseTransformations(new Vector3((float)x / scale, (float)z / scale, (float)y / scale)));

            }

            Transforms = new Matrix[Transformations.Count];

        }

        public void SetTransformations(List<Transformation> transformations)
        {
            Transformations = transformations;
            Transforms = new Matrix[Transformations.Count];
        }

        public void Update(GameTime gTime)
        {
            for (int i = 0; i < Transformations.Count; i++)
            {
                Transformations[i].Update(gTime);
                Transforms[i] = Transformations[i].TransformMatrix;
            }
        }

        public void AddTransformation(Transformation ellipse_transformed)
        {
            Transformations.Add(ellipse_transformed);
            Transforms = new Matrix[Transformations.Count];

        }

        public void Flush()
        {
            Transformations.Clear();
            Transforms = new Matrix[0];
        }

        public void Render()
        {
            DrawModelHardwareInstancing(EllipseFillModel, ModelBones, Transforms, Camera.ViewMatrix, Camera.ProjectionsMatrix);
        }
        /// <summary>
        /// Efficiently draws several copies of a piece of geometry using hardware instancing.
        /// </summary>
        void DrawModelHardwareInstancing(Model model, Matrix[] modelBones,
                                         Matrix[] instances, Matrix view, Matrix projection)
        {
            if (instances.Length == 0)
                return;

            // If we have more instances than room in our vertex buffer, grow it to the neccessary size.
            if ((DynamicVertexBuffer == null) ||
                (instances.Length > DynamicVertexBuffer.VertexCount))
            {
                if (DynamicVertexBuffer != null)
                    DynamicVertexBuffer.Dispose();

                DynamicVertexBuffer = new DynamicVertexBuffer(Handler.GraphicsDevice, VertexDeclaration,
                                                               instances.Length, BufferUsage.WriteOnly);
            }

            // Transfer the latest instance transform matrices into the instanceVertexBuffer.
            DynamicVertexBuffer.SetData(instances, 0, instances.Length, SetDataOptions.Discard);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Tell the GPU to read from both the model vertex buffer plus our instanceVertexBuffer.
                    Handler.GraphicsDevice.SetVertexBuffers(
                        new VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset, 0),
                        new VertexBufferBinding(DynamicVertexBuffer, 0, 1)
                    );

                    Handler.GraphicsDevice.Indices = meshPart.IndexBuffer;

                    Effect.CurrentTechnique = Effect.Techniques["HardwareInstancing"];

                    Effect.Parameters["World"].SetValue(Matrix.CreateScale(Scale));
                    Effect.Parameters["View"].SetValue(view);
                    Effect.Parameters["Projection"].SetValue(projection);
                    Effect.Parameters["InstanceColor"].SetValue(EllipsoidColor.ToVector3());
                    

                    // Draw all the instance copies in a single call.
                    foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        RasterizerState rs = new RasterizerState();
                        rs.CullMode = CullMode.None;
                        rs.FillMode = FillMode.Solid;
                        Handler.GraphicsDevice.RasterizerState = rs;
                        Handler.GraphicsDevice.DrawInstancedPrimitives(PrimitiveRenderType, 0, 0,
                                                               meshPart.NumVertices, meshPart.StartIndex,
                                                               meshPart.PrimitiveCount, instances.Length);
                    }
                }
            }
        }


    }
}
