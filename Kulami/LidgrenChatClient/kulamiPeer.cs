using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Threading;
using System.Net;


namespace LidgrenKulamiPeer
{
    public partial class KulamiPeer
    {
        public static NetPeer peer;
        private string name;
        private static long localIdentifier;
        public const string signature = "team2";
        public NetPeerConfiguration config;
        public peerListener listener; //Event Listener (GUI) implement later
        public Thread NetThread;
        private static Queue<string> moveQueue;

        public KulamiPeer(int port)
        {
            config = new NetPeerConfiguration(signature);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.UnconnectedData);
            config.EnableMessageType(NetIncomingMessageType.Data);
            config.EnableMessageType(NetIncomingMessageType.StatusChanged);
            config.MaximumConnections = 1;
            config.AutoFlushSendQueue = true;
            config.Port = port;
            config.AcceptIncomingConnections = true;          
        }

        public void Start()
        {
            peer = new NetPeer(config);
            peer.Start();
            localIdentifier = peer.UniqueIdentifier;

            listener = new peerListener(peer, localIdentifier);
            moveQueue = new Queue<string>();

            NetThread = new Thread(new ThreadStart(listener.processNetwork));
            NetThread.Start();
        }

        public void killPeer()
        {
            listener.shouldQuit();
            NetThread.Abort();
            peer.Shutdown("Thread and Peer Terminated. No longer doing Network games\n");
            Console.WriteLine("Network thread terminated and peer destroyed/set to null");
        }

        public void sendMove(string move)
        {
            NetOutgoingMessage message = KulamiPeer.peer.CreateMessage();
            message.Write(signature);
            message.Write(move);
            KulamiPeer.peer.SendMessage(message, listener.connection, NetDeliveryMethod.ReliableOrdered);
        }

        public string getMove()
        {
            // Checks the queue five times, waits at most 25 seconds before returning an error.
            string result = null;
            if (moveQueue.Count != 0 && listener.errorMessage == "")
            {
                result = moveQueue.Dequeue();
            }

            //if (listener.errorMessage != "")
            //{
            //    result = listener.errorMessage;
            //    listener.errorMessage = "";
            //    Console.WriteLine(result);
            //}

            return result;
        }
    }
}
