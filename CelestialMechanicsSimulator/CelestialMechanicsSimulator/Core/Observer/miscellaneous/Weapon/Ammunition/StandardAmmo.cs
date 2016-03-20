using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace CelestialMechanicsSimulator.Core.Observer.miscellaneous.Weapon.Ammunition
{
    public class StandardAmmo
    {
        private Matrix WorldMatrix;
        private float Z;

        public GraphicsDevice Device { get; private set; }
        public Solarsystem Handler { get; private set; }

        public Camera Camera { get; private set; }
        
        public Model AmmoModel { get; private set; }
        public BoundingBox BoundingBox { get; private set; }

        public StandardAmmo(Solarsystem handler, GraphicsDevice device, Camera camera)
        {
            Device = device;
            Handler = handler;
            Camera = camera;
            AmmoModel = Handler.Content.Load<Model>(@"Eastereggs\M4A1\Bullet");
        }

        public void Shoot(Matrix view)
        {
            WorldMatrix = view;
        }

        public void Update(GameTime gTime)
        {
            Z -= 1f * (float)gTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void Render()
        {
            Matrix[] boneTransforms = new Matrix[AmmoModel.Bones.Count];
            AmmoModel.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in AmmoModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    Matrix world = Matrix.Identity;
                    world = boneTransforms[mesh.ParentBone.Index] * Matrix.CreateScale(0.01f) * Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateFromAxisAngle(Vector3.Zero, -17.447933f) * Matrix.CreateTranslation(new Vector3(0, -0.45f,  Z)) * Matrix.Invert(WorldMatrix);
                    BoundingBox = BoundingBoxRenderer.UpdateBoundingBox(AmmoModel, world);
                    effect.World = world;       
                    effect.View = Camera.ViewMatrix;
                    effect.Projection = Camera.ProjectionsMatrix;
                    effect.AmbientLightColor = new Vector3(1.0f, 1.0f, 1.0f);
                    effect.TextureEnabled = true;
                    effect.CurrentTechnique.Passes[0].Apply();
                }

                mesh.Draw();
            }

            BoundingBoxRenderer.Render(BoundingBox, Device, Camera.ViewMatrix, Camera.ProjectionsMatrix, Color.Red);
        }
    }
}
