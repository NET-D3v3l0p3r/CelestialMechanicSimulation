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
using System.Net;
using System.IO;
namespace CelestialMechanicsSimulator.Core.CelestialBodies
{

    public class NetworkCB : CelestialBody
    {
        public NetworkCB(Solarsystem handler, GraphicsDevice device, Camera camera, string name, float numeric_e, float sideral_period, float weight, string texture)
            : base(handler, device, camera)
        {
            @Type = CelestialBodyType.Planet;
            Name = name;
            Size = weight;
            NumericEccentricity = numeric_e;
            TurnAroundTime = sideral_period;

            Random rand = new Random();
            Texture = ToolKit.ToTexture2D(new Color(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)), Handler.GraphicsDevice);

            var CLoader = Handler.Content;
            Model = CLoader.Load<Model>(ModelPath);

            Calculate();

        }

        public override void Update(GameTime gTime)
        {
            base.Update(gTime);
            SetWorldMatrix(
            Matrix.CreateScale(Scale) *
            Matrix.CreateFromAxisAngle(Vector3.UnitX, 7.447933f) *
            Matrix.CreateRotationY(Rotation) *
            Matrix.CreateTranslation(new Vector3((float)this.XCoord + GLOBALX, 0 + GLOBALY, (float)this.YCoord + GLOBALZ)));
        }
    }
}
