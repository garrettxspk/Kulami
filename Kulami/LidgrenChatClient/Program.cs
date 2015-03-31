using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LidgrenKulamiPeer
{
    class Program
    {

        //private static Form1 s_form;
        public static KulamiPeer kulamiPeer;

        
        static void Main()
        {
            // This does nothing! just the way we like it!!!
            //check kulamiPeer.listener.connection
            kulamiPeer = new KulamiPeer(3070);
            for (int i = 0; i < 3; i++)
            {
                kulamiPeer.sendMove(Console.ReadLine());
                Console.WriteLine("enter a key to continue...");
                Console.ReadKey();
                Console.WriteLine(kulamiPeer.getMove());
            }
            kulamiPeer.killPeer();
        }
    }
}
