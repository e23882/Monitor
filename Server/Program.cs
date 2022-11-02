using System;
using System.Threading;
using System.Windows.Forms;

namespace Server
{
    public class Program 
    {
        #region Declarations
        public static Server service = null;
        #endregion

        #region Memberfunction
        #endregion
        public static void Main(string[] args) 
        {
            try 
            {
                Thread th = new Thread(() =>
                {
                    service = new Server(5566);
                });
                th.Start();
            }
            catch 
            {

            }
            
        }
    }
}
