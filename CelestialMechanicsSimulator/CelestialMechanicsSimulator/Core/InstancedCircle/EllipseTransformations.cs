using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CelestialMechanicsSimulator.Core.InstancedCircle;
using CelestialMechanicsSimulator.Core.CelestialBodies;
namespace CelestialMechanicsSimulator.Core.InstancedCircle
{
    public class EllipseTransformations : Transformation
    {
        private Vector3 Coordinate;

        public EllipseTransformations(Vector3 coord)
        {
            Coordinate = coord;
        }

        public override void Update(GameTime gTime)
        {
            TransformMatrix = Matrix.CreateTranslation(Coordinate + new Vector3(CelestialBody.GLOBALX, CelestialBody.GLOBALY, CelestialBody.GLOBALZ));
            base.Update(gTime);
        }
    }
}
