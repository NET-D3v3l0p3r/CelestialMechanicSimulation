using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CelestialMechanicsSimulator.Core.InstancedCircle;
namespace CelestialMechanicsSimulator.Core.CelestialBodies
{
    public class Halleyscher_Komet : CelestialBody
    {
        public Halleyscher_Komet(Solarsystem handler, GraphicsDevice device, Camera camera)
            : base(handler, device, camera)
        {
            @Type = CelestialBodyType.Planet;
            Size = 130000f;
            NumericEccentricity = 0.967f;
            TurnAroundTime = 2375291520.0f;
            PlanetMass = 2E14;
            PhysicsScale = 2800000;
            DeltaTime = 60 * 60 * 24 * 7;
            RenderWithBloom = true;
            PortionY = 0.8;
            PortionZ = 0.5;
            InstancedEllipse = new InstancedEllipse(Handler, Camera, 35, Color.White);
            InstancedEllipse.PrimitiveRenderType = PrimitiveType.LineStrip;
            ModelPath = @"Comet\LargeAsteroid";

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
