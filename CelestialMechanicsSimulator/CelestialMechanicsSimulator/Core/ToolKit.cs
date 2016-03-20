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
using System.IO;
namespace CelestialMechanicsSimulator.Core
{
    public static class ToolKit
    {
        public static Texture2D ToTexture2D(Color tColor, GraphicsDevice gDevice)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(1, 1);
            bmp.SetPixel(0, 0, System.Drawing.Color.FromArgb(tColor.A, tColor.R, tColor.G, tColor.B));
            var mStream = new MemoryStream();
            bmp.Save(mStream, System.Drawing.Imaging.ImageFormat.Png);
            return Texture2D.FromStream(gDevice, mStream);
        }

        public static Vector2 Project(GraphicsDevice device, Matrix projection, Matrix view, Matrix world, Vector3 current_pos)
        {
            var coord = device.Viewport.Project(current_pos, projection, view, world);
            return new Vector2(coord.X, coord.Y);
        }

        public static Color GetPixel(Texture2D tex2d, int x, int y)
        {
            Color[] data = new Color[tex2d.Width * tex2d.Height];
            tex2d.GetData<Color>(data);
            return data[x + y * tex2d.Width]; 
        }

        public static Ray CalculateRay(Vector2 mouseLocation, Matrix view, Matrix projection, Viewport viewport)
        {
            Vector3 nearPoint = viewport.Unproject(new Vector3(mouseLocation.X,
                    mouseLocation.Y, 0.0f),
                    projection,
                    view,
                    Matrix.Identity);

            Vector3 farPoint = viewport.Unproject(new Vector3(mouseLocation.X,
                    mouseLocation.Y, 1.0f),
                    projection,
                    view,
                    Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }
    }
}
