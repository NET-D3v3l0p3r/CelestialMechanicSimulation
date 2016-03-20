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
using CelestialMechanicsSimulator.Core.CelestialBodies;
namespace CelestialMechanicsSimulator.Core.Maths
{
    public class FlatRec3D
    {
        public Solarsystem Handler { get; private set; }
        public GraphicsDevice Device { get; private set; }

        public Camera Camera { get; set; }
        public Matrix World { get; set; }

        private VertexPositionColor[] Vertices;
        private BasicEffect Effect;
        private Vector3 Position;

        public FlatRec3D(Solarsystem g, Camera camera, Vector3 position, Color color)
        {
            Handler = g;
            Device = g.GraphicsDevice;
            Camera = camera;

            Effect = new BasicEffect(Device);
            Effect.VertexColorEnabled = true;

            Position = position;

            Vertices = new VertexPositionColor[6];

            Vertices[0] = new VertexPositionColor(new Vector3(0, 0, 0), color);
            Vertices[1] = new VertexPositionColor(new Vector3(1, 0, 0), color);
            Vertices[2] = new VertexPositionColor(new Vector3(0, 0, 1), color);

            Vertices[3] = new VertexPositionColor(new Vector3(0, 0, 1), color);
            Vertices[4] = new VertexPositionColor(new Vector3(1, 0, 1), color);
            Vertices[5] = new VertexPositionColor(new Vector3(1, 0, 0), color);
        }

        public void Render()
        {
            Effect.World = Matrix.CreateScale(500) * Matrix.CreateTranslation(new Vector3(CelestialBody.GLOBALX + Position.X, CelestialBody.GLOBALY + Position.Y, CelestialBody.GLOBALZ + Position.Z));
            Effect.View = Camera.ViewMatrix;
            Effect.Projection = Camera.ProjectionsMatrix;
            Effect.CurrentTechnique.Passes[0].Apply();
            Device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, Vertices, 0, Vertices.Length / 3);
        }
    }
}
