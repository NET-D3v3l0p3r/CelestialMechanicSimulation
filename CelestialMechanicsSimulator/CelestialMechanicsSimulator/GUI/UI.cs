using CelestialMechanicsSimulator.Core.CelestialBodies;
using CelestialMechanicsSimulator.Network;
using QRCodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CelestialMechanicsSimulator.GUI
{
    public partial class UI : Form
    {
        Solarsystem Handler;
        Point Position;
        CelestialBody Planet;
        SmartphoneControl qr_control;
        string qr = "";

        List<PointF> Path = new List<PointF>();
        float xPos, yPos;
        float distance;
        PointF centerP;

        public UI(Solarsystem handler, Point position, CelestialBody planet)
        {
            InitializeComponent();
            Handler = handler;
            Position = position;
            Planet = planet;
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            MethodInvoker Invoker = delegate
            {
                LoadData();
                Bitmap bmp = new Bitmap(planet_path.Width, planet_path.Height);
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                        bmp.SetPixel(i, j, Color.Black);
                Graphics g = Graphics.FromImage(bmp);
                for (float i = 0; i < 2 * Math.PI; i += 0.1f)
                {
                    var x = ((planet_path.Width / 2) + (planet_path.Width / 2) * (float)Math.Cos(i));
                    var y = ((planet_path.Height / 2) + (planet_path.Height / 2) * (float)Math.Sin(i));

                    Path.Add(new PointF(x, y));
                }

                g.DrawLine(Pens.White, new Point(planet_path.Width / 2, 0), new Point(planet_path.Width / 2, planet_path.Height));
                g.DrawLine(Pens.White, new Point(0, planet_path.Height / 2), new Point(planet_path.Width, planet_path.Height / 2));

                for (int i = 0; i < Path.Count; i++)
                {
                    Color color = Color.FromArgb(255, Planet.TypicalColor.R, Planet.TypicalColor.G, Planet.TypicalColor.B);
                    Pen Pen = new Pen(color);
                    if (i != Path.Count - 1)
                        g.DrawLine(Pen, Path[i].X, Path[i].Y, Path[i + 1].X, Path[i + 1].Y);
                    else g.DrawLine(Pen, Path[i].X, Path[i].Y, Path[0].X, Path[0].Y);
                }
                planet_path.Image = bmp;

                xPos = -float.NaN;
                yPos = float.NaN;
                centerP = new PointF(-float.NaN, float.NaN);
            };

            Invoke(Invoker);
            qr_control = new SmartphoneControl(Handler, "gfdimage.esy.es", "CMS", "Input.php", "215215215");

            ShowInTaskbar = false;


        }
        private void LoadData()
        {
            this.Location = Position;
            this.Size = new Size(365, Handler.GraphicsDeviceManager.PreferredBackBufferHeight);
            this.BackColor = Color.Black;
            this.Text = "Planet: " + Planet.Name;
            TopMost = true;

            PName.Text = "Planet: " + Planet.Name;
            PSize.Text = "Size: " + Planet.Size + "";
            PA.Text = "MajorAxis A: " + Planet.MajorAxisA + "";
            PB.Text = "MinorAxis B: " + Planet.MinorAxisB + "";
            numeric.Text = "ε: " + Planet.NumericEccentricity + "";
            linear.Text = "e: " + Planet.LinearEccentricity + "";
            a_p.Text = "Aphel - Periphel: " + Planet.Aphel + " - " + Planet.Periphel;


            tb_mass.Value = (int)(Planet.PlanetMass / Planet.PlanetMassConst) * 2;
            mass.Text = (tb_mass.Value * 0.5 * Planet.PlanetMassConst) + "kg = (" + Planet.PlanetMassConst + "*" + tb_mass.Value + "* 0.5) kg";

            tb_vx.Value = tb_vy.Value = tb_vz.Value = 10;

            lb_vx.Text = (Planet.VelocityX) + "m/s";
            lb_vy.Text = (Planet.VelocityY) + "m/s";
            lb_vz.Text = (Planet.VelocityZ) + "m/s";

        }

        private void GUI_Paint(object sender, PaintEventArgs e)
        {
            this.Location = Position;
        }

        public void SetPlanet(CelestialBody planet)
        {
            MethodInvoker Invoker = delegate
            {
                Planet = planet;
                LoadData();
                centerP = new PointF((planet_path.Width / 2) + (Planet.LinearEccentricity / Planet.MajorAxisA) * 200 - 7, planet_path.Height / 2 - 8);
                planet_path.Invalidate();
            };
            Invoke(Invoker);
        }

        private void planet_path_Paint(object sender, PaintEventArgs e)
        {
            MethodInvoker Invoker = delegate
            {
                e.Graphics.FillEllipse(Brushes.Orange, centerP.X, centerP.Y, 15, 15);
            };
            Invoke(Invoker);
        }

        public void UpdateQR(Microsoft.Xna.Framework.GameTime gTime)
        {
            MethodInvoker Invoker = delegate
            {
                if (gTime.TotalGameTime.TotalSeconds % 5 < 0.2)
                {
                    //qr_control.GetQR();
                    //if (qr != qr_control.QRCode)
                    //{
                    //    Console.Beep();
                    //    qr = qr_control.QRCode;
                    //}
                    //qr_image.Image = QRCode.Generate("http://gfdimage.esy.es/CMS/Input.php?mode=gui&qr=" + qr, 191, 1, ErrorCorrectionLevel.High);
                }
                
            };
            Invoke(Invoker);
        }


        private void tb_vx_ValueChanged(object sender, EventArgs e)
        {
            lb_vx.Text = (tb_vx.Value * 0.1 * Planet.VelocityX) + "m/s";
            Planet.VelocityX = tb_vx.Value * 0.1 * Planet.VelocityX;
        }

        private void tb_vy_ValueChanged(object sender, EventArgs e)
        {
            lb_vy.Text = (tb_vy.Value * 0.1 * Planet.VelocityY) + "m/s";
            Planet.VelocityY = tb_vy.Value * 0.1 * Planet.VelocityY;
        }

        private void tb_vz_ValueChanged(object sender, EventArgs e)
        {
            lb_vz.Text = (tb_vz.Value * 0.1 * Planet.VelocityZ) + "m/s";
            Planet.VelocityZ = tb_vz.Value * 0.1 * Planet.VelocityZ;
        }

        private void tb_mass_ValueChanged_1(object sender, EventArgs e)
        {
            var Value = tb_mass.Value * 0.5;
            mass.Text = (Value * Planet.PlanetMassConst) + "kg = (" + Planet.PlanetMassConst + "*" + tb_mass.Value + "* 0.5) kg";
            Planet.PlanetMass = Value * Planet.PlanetMassConst;
        }

    
    }
}   
