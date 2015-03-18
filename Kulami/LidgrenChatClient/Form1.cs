using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//namespace LidgrenKulamiPeer
//{
//    public partial class Form1 : Form
//    {
//        private static KulamiPeer network;
//        public Form1()
//        {
//            InitializeComponent();
//            network = new KulamiPeer();
//        }

//        private void button1_Click(object sender, EventArgs e)
//        {
//            if (KulamiPeer.peer.Connections.Count() > 0)
//            {
//                string input = textBox_message.Text.ToString();
//                Program.kulamiPeer.listener.sendMessage(input);
//                MessageBox.Show(KulamiPeer.peer.Connections.Count().ToString());
//            }
//            else
//                network.broadcastMessage(textBox_message.Text.ToString());
//        }

//        private void button2_Click(object sender, EventArgs e)
//        {

//        }
//    }
//}
