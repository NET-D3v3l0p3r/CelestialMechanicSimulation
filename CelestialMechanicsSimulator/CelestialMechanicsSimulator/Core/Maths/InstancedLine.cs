using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace CelestialMechanicsSimulator.Core.Maths
{
    public class InstancedLine
    {
        public Vector3 Position { get; private set; }
        public Matrix Transformation { get; private set; }

        public InstancedLine(Vector3 pos)
        {
            Transformation = Matrix.CreateTranslation(Position);
        }

        public void Update()
        {
            Transformation = Matrix.CreateTranslation(Position +  new Vector3(CelestialBodies.CelestialBody.GLOBALX, CelestialBodies.CelestialBody.GLOBALY, CelestialBodies.CelestialBody.GLOBALZ));
        }
    }
}
