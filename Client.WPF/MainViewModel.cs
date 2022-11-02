using System.Configuration;
using System;
using System.Windows.Media.Imaging;
using WebSocketSharp;
using System.IO;
using Newtonsoft.Json.Bson;
using System.Threading;
using System.Diagnostics.Contracts;

namespace Harmos
{
    public class MainViewModel : ViewModelBase
    {
        #region Declarations
        private BitmapImage _Source = new BitmapImage();
        private string _MB;
        private string _Status;

        #endregion

        #region Property
        public BitmapImage Source
        {
            get
            {
                return _Source;
            }
            set
            {
                _Source = value;
                OnPropertyChanged();
            }
        }
        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged();
            }
        }
        public string MB
        {
            get
            {
                return _MB;
            }
            set
            {
                _MB = value;
                OnPropertyChanged();
            }
        }
        public WebSocket ws { get; set; }
        #endregion

        #region Memberfunction
        public MainViewModel()
        {
            InitialClient();
        }
        public void InitialClient() 
        {
            var server = ConfigurationSettings.AppSettings["Server"];
            ws = new WebSocket($"ws://{server}:5566/Connect");
            ws.OnMessage += Ws_OnMessage;
            ws.OnOpen += Ws_OnOpen;
            ws.OnClose += Ws_OnClose;
            ws.Connect();
            Status = "伺服器連線成功!";
        }

        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            Status = "伺服器中斷連線...嘗試重連....";
            InitialClient();
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            Console.WriteLine("Connected");
        }
        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            var ws = (sender as WebSocket);
            
            if (ws is null)
            {
                return;
            }

            string receiveData = e.Data;
            switch (receiveData) 
            {
                case "CLOSE":
                    Console.WriteLine("hit");
                    break;
                default:
                    byte[] binaryData = Convert.FromBase64String(receiveData);

                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = new MemoryStream(binaryData);
                    MB = Math.Round(((binaryData.Length / 1024f) / 1024f), 2).ToString();
                    bi.EndInit();
                    bi.Freeze();
                    Source = bi;
                    break;
            }
           
        }
        public void SaveImage(BitmapImage image, string localFilePath)
        {
            image.DownloadCompleted += (sender, args) =>
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapImage)sender));
                using (var filestream = new FileStream(localFilePath, FileMode.Create))
                {
                    encoder.Save(filestream);
                }
            };
        }
        #endregion
    }
}
