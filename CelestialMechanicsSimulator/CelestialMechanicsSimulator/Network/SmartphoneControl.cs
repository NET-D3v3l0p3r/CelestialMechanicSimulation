using QRCodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CelestialMechanicsSimulator.Network
{
    public class SmartphoneControl
    {
        public string APIServer { get; private set; }
        public string APIRootDirectory{get;private set;}
        public string APIFile { get; private set; }
        public string Key { get; private set; }

        public string QRCode { get; private set; }
       

        private Solarsystem privateHandler;

        public SmartphoneControl(Solarsystem handler, string server, string dir, string file, string key)
        {
            privateHandler = handler;
            APIServer = server;
            APIRootDirectory = dir;
            APIFile = file;
            Key = key;
            QRCode = "";
        }

        public void GetQR()
        {
            string url = "http://" + APIServer + "/" + APIRootDirectory + "/" + APIFile + "?mode=api_app_read_qr" + "&pc=" + Environment.UserName + "&key=" + Key;
            var ExternThread = new Thread(new ThreadStart(() =>
            {
                var content = NavigateToNewThread(url);
                if (content.Contains("Key or username is wrong"))
                {
                    Console.WriteLine("PC is not valid!");
                    privateHandler.Exit();
                }
                QRCode = content;
            }));
            ExternThread.SetApartmentState(ApartmentState.STA);
            ExternThread.Start();
        }

        public string NavigateToNewThread(string website)
        {
            WebBrowser wb = new WebBrowser();
            wb.Navigate(website);
            while (wb.ReadyState != WebBrowserReadyState.Complete) { Application.DoEvents(); }
            return wb.DocumentText;
        }
    }
}
