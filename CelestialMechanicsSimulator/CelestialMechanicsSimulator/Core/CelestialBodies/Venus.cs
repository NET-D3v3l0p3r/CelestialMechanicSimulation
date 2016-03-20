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
using CelestialMechanicsSimulator.Core.InstancedCircle;
namespace CelestialMechanicsSimulator.Core.CelestialBodies
{
    
    public class Venus : CelestialBody
    {
        public Venus(Solarsystem handler, GraphicsDevice device, Camera camera)
            : base(handler, device, camera)
        {
            @Type = CelestialBodyType.Planet;
            Size = 6050 * 2; 
            NumericEccentricity = 0.0067f;
            TurnAroundTime = 19414166.4f;
            PlanetMass = 4.869E24;
            PhysicsScale = 2800000;
            DeltaTime = 60 * 60 * 24 * 7 * 0.01;
            PortionY = 0.99;
            PortionZ = 0.01;
            InstancedEllipse = new InstancedEllipse(Handler, Camera, 35, Color.White);
            InstancedEllipse.PrimitiveRenderType = PrimitiveType.LineStrip;
            LoadContent();
            Sign = -1;
        }

        public override void Update(GameTime gTime)
        {
            base.Update(gTime);
            SetWorldMatrix(
            Matrix.CreateScale(Scale) *
            Matrix.CreateFromAxisAngle(Vector3.UnitX, 7.447933f) *
            Matrix.CreateRotationY(Rotation) *
            Matrix.CreateTranslation(new Vector3((float)this.XCoord + GLOBALX, (float)this.ZCoord + GLOBALY, (float)this.YCoord + GLOBALZ)));
           
        }
    }
}
