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
using CelestialMechanicsSimulator.Core.InstancedCircle;
namespace CelestialMechanicsSimulator.Core.CelestialBodies
{
    public class Saturn : CelestialBody
    {
        public Saturn(Solarsystem handler, GraphicsDevice device, Camera camera)
            : base(handler, device, camera)
        {
            @Type = CelestialBodyType.Planet;
            Size = 130500;
            NumericEccentricity = 0.05648f;
            TurnAroundTime = 928735200.0f;
            PlanetMass = 5.683E26;
            PhysicsScale = 2800000;
            DeltaTime = 60 * 60 * 24 * 7;
            PortionY = 0.9;
            PortionZ = 0.03;
            InstancedEllipse = new InstancedEllipse(Handler, Camera, 1500, Color.White);
            InstancedEllipse.PrimitiveRenderType = PrimitiveType.TriangleList;
            LoadContent();
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
