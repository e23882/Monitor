using System;
using System.Threading;
using WebSocketSharp.Server;
using WebSocketSharp;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace Server
{
    public class Server
    {
        #region Property
        public WebSocketServer SocketServer { get; set; }
        #endregion

        #region Memberfunction
        public Server(int port)
        {
            SocketServer = new WebSocketServer(port);
            SocketServer.AddWebSocketService<Connect>("/Connect");
            SocketServer.Start();

            while (true)
            {
                try 
                {
                    var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                               Screen.PrimaryScreen.Bounds.Height,
                                               PixelFormat.Format32bppArgb);

                    var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

                    gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                                Screen.PrimaryScreen.Bounds.Y,
                                                0,
                                                0,
                                                Screen.PrimaryScreen.Bounds.Size,
                                                CopyPixelOperation.SourceCopy);

                    System.IO.MemoryStream ms = new MemoryStream();
                    bmpScreenshot.Save(ms, ImageFormat.Jpeg);
                    byte[] byteImage = ms.ToArray();
                    var SigBase64 = Convert.ToBase64String(byteImage);

                    SocketServer.WebSocketServices["/Connect"].Sessions.Broadcast(SigBase64);
                }
                catch 
                {

                }
                
                Thread.Sleep(100);
            }
        }

        public static byte[] ImageToByteArray(System.Drawing.Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }
        #endregion
    }
}
