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
using System.Diagnostics;
namespace CelestialMechanicsSimulator.Core.Observer.miscellaneous.Weapon
{
    /*
     *
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
                hasRightClicked = true;
            else if (Mouse.GetState().RightButton == ButtonState.Released)
                hasRightClicked = false;

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !hasShot && hasRightClicked)
            {
                t.Tick += new EventHandler((object sender, EventArgs e) =>
                {
                    t.Stop();
                    sound.Play();
                    hasShot = false;
                    hasRightClicked = false;
                });
                hasShot = true;
                hasRightClicked = true;
                t.Interval = 150;
                t.Start();
            }
     * 
     *    Matrix[] boneTransforms1 = new Matrix[Model1.Bones.Count];
            Model1.CopyAbsoluteBoneTransformsTo(boneTransforms1);

            foreach (ModelMesh mesh in Model1.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    Matrix world = Matrix.Identity;
                    if(hasRightClicked)
                    world = boneTransforms1[mesh.ParentBone.Index] * Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(3.26798f) * Matrix.CreateTranslation(new Vector3(-0.272f, -0.185f, 0.5f)) * Matrix.Invert(gPlayer.Camera.ViewMatrix);
                    else world = boneTransforms1[mesh.ParentBone.Index] * Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(3.26798f) * Matrix.CreateTranslation(new Vector3(-0.572f, -0.17f, 0.2f)) * Matrix.Invert(gPlayer.Camera.ViewMatrix); ;
                    effect.World = world;
                    Debug.WriteLine(r);
                    effect.View = gPlayer.Camera.ViewMatrix;
                    effect.Projection = gPlayer.Camera.ProjectionsMatrix;
                    
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.TextureEnabled = true;
                    effect.CurrentTechnique.Passes[0].Apply();
                }

                mesh.Draw();
            }
     */
    public class M4A1
    {
        private bool isAiming, hasShot;
        private Camera Camera;
        private int CurrentAmmo = -1;

        private float LerpX = -.968f;
        private float LerpZ = .5f;


        public GraphicsDevice Device { get; private set; }
        public Solarsystem Handler { get; private set; }

        public Observer Player { get; private set; }

        public Model M4A1Model { get; private set; }
        public SoundEffect M4A1Shooting { get; private set; }

        public List<Ammunition.StandardAmmo> Magazine { get; private set; }
        
        public M4A1(Solarsystem handler, GraphicsDevice device, Observer player)
        {
            isAiming = false;
            hasShot = false;

            Handler = handler;
            Device = device;
            Player = player;

            Camera = Player.Camera;
            Magazine = new List<Ammunition.StandardAmmo>();
        }

        public void Load()
        {
            M4A1Model = Handler.Content.Load<Model>(@"Eastereggs\M4A1\m4a1_s");
            M4A1Shooting = Handler.Content.Load<SoundEffect>(@"Eastereggs\M4A1\m4a1_shooting");
  
        }
        public void Aim()
        {
            //-.668f
            //-.968f
            //1.5f
            //.5f


            if (LerpZ < 1.35)
            {
                LerpZ += .1f;
                LerpX = -.668f;
            }

            isAiming = true;
        }
        public void Unaim()
        {
            if (LerpZ >= .8f)
            {
                LerpZ -= .5f;
                LerpX = -.968f;
            }
            isAiming = false;
        }

        public void Update(GameTime gTime)
        {
            foreach (var ammo in Magazine)
                ammo.Update(gTime);
               
            if (Magazine.Count > 35)
            {
                Magazine.Clear();
                CurrentAmmo = -1;
            }
        }


        public void Shoot(ShootingPrinciple principle)
        {
            switch (principle)
            {
                case ShootingPrinciple.Automatic:
                    System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
                    if (!hasShot && isAiming)
                    {
                        t.Tick += new EventHandler((object sender, EventArgs e) =>
                        {
                            t.Stop();
                            M4A1Shooting.Play();
                            //AMMO SHOOT HERE

                            var Ammo = new Ammunition.StandardAmmo(Handler, Device, Camera);
                            Magazine.Add(Ammo);
                            CurrentAmmo++;
                            Magazine[CurrentAmmo].Shoot(Camera.ViewMatrix);

                            hasShot = false;
                            isAiming = false;
                        });
                        hasShot = true;
                        isAiming = true;
                        t.Interval = 150;
                        t.Start();
                    }
                    break;

                case ShootingPrinciple.SemiAutomatic:
                    if (!hasShot && isAiming)
                    {
                        hasShot = true;
                        isAiming = true;
                        M4A1Shooting.Play();
                        var Ammo = new Ammunition.StandardAmmo(Handler, Device, Camera);
                        Magazine.Add(Ammo);
                        CurrentAmmo++;
                        Magazine[CurrentAmmo].Shoot(Camera.ViewMatrix);
                        //AMMO SHOOT HERE
                    }
                    break;
            }
        }

        public void Untrigger()
        {
            hasShot = false;
        }

        public void Render()
        {
            foreach (var ammo in Magazine)
                ammo.Render();

            Matrix[] boneTransforms = new Matrix[M4A1Model.Bones.Count];
            M4A1Model.CopyAbsoluteBoneTransformsTo(boneTransforms);


            //-.668f
            //-.968f
            //1.5f
            //.5f

            foreach (ModelMesh mesh in M4A1Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    Matrix world = Matrix.Identity;
                    if (isAiming)
                        world = boneTransforms[mesh.ParentBone.Index] * Matrix.CreateScale(0.2f) * Matrix.CreateRotationY(MathHelper.ToRadians(180)) * Matrix.CreateTranslation(new Vector3(LerpX, -0.34f, LerpZ)) * Matrix.Invert(Camera.ViewMatrix);
                    else
                        world = boneTransforms[mesh.ParentBone.Index] * Matrix.CreateScale(0.2f) * Matrix.CreateRotationY(MathHelper.ToRadians(180)) * Matrix.CreateTranslation(new Vector3(LerpX, -0.38f, LerpZ)) * Matrix.Invert(Camera.ViewMatrix);
                    
                    effect.World = world;
                    effect.View = Camera.ViewMatrix;
                    effect.Projection = Camera.ProjectionsMatrix;

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.TextureEnabled = true;
                    effect.CurrentTechnique.Passes[0].Apply();
                }

                mesh.Draw();
            }


        }

        public enum ShootingPrinciple
        {
            SemiAutomatic,
            Automatic
        }
       
    }
}
