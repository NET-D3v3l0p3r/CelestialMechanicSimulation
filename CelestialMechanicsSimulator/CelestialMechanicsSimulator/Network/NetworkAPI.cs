using CelestialMechanicsSimulator.Core.CelestialBodies;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace CelestialMechanicsSimulator.Network
{
    public class NetworkAPI
    {
        private Thread ExternThread = null;

        public Solarsystem Handler { get; private set; }
        public string Webserver { get; set; }
        public string RootDirectory { get; set; }
        public string Class { get; set; }

        public NetworkAPI(Solarsystem handler, string server, string directory, string @class)
        {
            Handler = handler;
            Webserver = server;
            RootDirectory = directory;
            Class = @class;
        }

        public void Start()
        {
            System.Timers.Timer Timer = new System.Timers.Timer(2500.0);
            Timer.Elapsed += new System.Timers.ElapsedEventHandler((object sender, System.Timers.ElapsedEventArgs e) =>
            {
                string url = "http://" + Webserver + "/" + RootDirectory + "/" + Class + "?mode=api";
                var ExtenThread = new Thread(new ThreadStart(() =>
                {
                    WebBrowser wb = new WebBrowser();
                    wb.Navigate(url);
                    while (wb.ReadyState != WebBrowserReadyState.Complete) { Application.DoEvents(); }
                    string[] datas = wb.DocumentText.Split(new string[] { "<br>" }, StringSplitOptions.None);
                    for (int i = 0; i < datas.Length; i++)
                    {
                        var row = datas[i].Split('|');
                        if(datas[i].Contains("|"))
                        {
                            string Name = row[0];
                            float NumericE = float.Parse(row[1], CultureInfo.InvariantCulture);
                            float Weight = float.Parse(row[3], CultureInfo.InvariantCulture);
                            float SideralPeriod = float.Parse(row[2], CultureInfo.InvariantCulture);
                            string Texture = row[4];


                            if(!Handler.CelestialBodies.Exists(p=>p.Name == Name))
                            {
                                var Body = new NetworkCB(Handler, Handler.GraphicsDevice, Handler.Observer.Camera, Name, NumericE, SideralPeriod, Weight, Texture);
                                Handler.CelestialBodies.Add(Body);
                                //Handler.SpeechRecognizer.AddCommand("Markiere " + Body.Name, new Action(() =>
                                //{
                                //    Body.SetFocus();
                                //    Handler.UI.SetPlanet(Body);
                                //}));
                                //Handler.SpeechRecognizer.AddCommand("Folge " + Body.Name, new Action(() =>
                                //{
                                //    Body.Follow();
                                //}));
                                //Handler.SpeechRecognizer.AddCommand("Gehe zu " + Body.Name, new Action(() =>
                                //{
                                //    Handler.Observer.Camera.SetPosition(new Vector3(Body.XCoord, Handler.Observer.Camera.CameraPosition.Y, Body.YCoord));
                                //    Handler.Observer.Camera.CameraPosition = new Vector3(Handler.Observer.Camera.CameraPosition.X, MathHelper.Lerp(Handler.Observer.Camera.CameraPosition.Y, 0, 0.8f), Handler.Observer.Camera.CameraPosition.Z);
                                //}));
                            }

                        }
                    }
                }));
                ExtenThread.SetApartmentState(ApartmentState.STA);
                ExtenThread.Start();
           

            });
            Timer.Start();
         
        }

        public void Terminate()
        {
            ExternThread.Abort();
        }

    }
}
