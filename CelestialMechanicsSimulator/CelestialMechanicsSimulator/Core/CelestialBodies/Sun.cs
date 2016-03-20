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
namespace CelestialMechanicsSimulator.Core.CelestialBodies
{
    public class Sun : CelestialBody
    {
        private float SunRotation;
        public Sun(Solarsystem handler, GraphicsDevice device, Camera camera)
            : base(handler, device, camera)
        {
            @Type = CelestialBodyType.Focus;
            RenderWithBloom = true ;
            Size = 200000;
            NumericEccentricity = 0;
            PlanetMass = CelestialBody.SUN_G / CelestialBody.GRAVITATIONCONSTANT;
            TurnAroundTime = 0;
            IsAllowedToRender = true;
            LoadContent();
        }

        public override void Update(GameTime gTime)
        {
            base.Update(gTime);
            SunRotation = (float)gTime.TotalGameTime.TotalSeconds;
            SetWorldMatrix(
            Matrix.CreateScale(Scale + (float)Math.Sin((double)SunRotation) * 50) *
            Matrix.CreateRotationY(SunRotation) *
            Matrix.CreateRotationZ(SunRotation) *
            Matrix.CreateRotationX(SunRotation) *
            Matrix.CreateFromAxisAngle(Vector3.UnitX, 7.447933f) *
            Matrix.CreateTranslation(new Vector3(CelestialBody.GLOBALX + (float)XCoord, CelestialBody.GLOBALY + (float)ZCoord, CelestialBody.GLOBALZ + (float)YCoord)));
        }
    }
}
