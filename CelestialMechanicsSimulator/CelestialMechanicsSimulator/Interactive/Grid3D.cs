using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace CelestialMechanicsSimulator.Interactive
{
    public class Grid3D
    {
        public GraphicsDevice Device { get; set; }

        public int MaxColumn { get; set; }
        public int MaxRows { get; set; }

        public int StepWidth { get; set; }
        public int StepHeight { get; set; }

        public Color Color { get; set; }

        private List<VertexPositionColor> Vertices = new List<VertexPositionColor>();
        private VertexPositionColor[] GridVertices;
        private BasicEffect Effect;

        public Grid3D(GraphicsDevice device)
        {
            Device = device;
            Effect = new BasicEffect(Device);
        }

        public void CreateGrid()
        {
            for (int x = 0; x < MaxColumn; x++)
            {
                Vertices.Add(new VertexPositionColor(new Vector3(x * StepWidth, 0, 0), Color));
                Vertices.Add(new VertexPositionColor(new Vector3(x * StepWidth, 0, StepWidth * MaxColumn), Color));
            }
            for (int y = 0; y < MaxRows; y++)
            {
                Vertices.Add(new VertexPositionColor(new Vector3(0, 0, y * StepHeight), Color));
                Vertices.Add(new VertexPositionColor(new Vector3(StepHeight * MaxRows, 0, y * StepHeight), Color));
            }

            GridVertices = new VertexPositionColor[Vertices.Count];
            GridVertices = Vertices.ToArray();
        }

        public void Render(Matrix view, Matrix projection, Matrix world)
        {
            Effect.World = world;
            Effect.View = view;
            Effect.Projection = projection;
            Effect.VertexColorEnabled = true;
            Effect.CurrentTechnique.Passes[0].Apply();
            Device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, GridVertices, 0, GridVertices.Length / 2);
        }
    }
}
