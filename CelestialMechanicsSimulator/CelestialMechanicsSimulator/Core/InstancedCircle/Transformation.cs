using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CelestialMechanicsSimulator.Core.InstancedCircle
{
    public class Transformation
    {
        public Matrix TransformMatrix { get; set; }

        public Transformation() { TransformMatrix = Matrix.Identity; }
        public virtual void Update(GameTime gTime) { }
    }
}
