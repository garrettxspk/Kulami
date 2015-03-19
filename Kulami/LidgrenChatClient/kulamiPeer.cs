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
        private peerListener listener; //Event Listener (GUI) implement later
        public Thread NetThread;
        public static List<NetIncomingMessage> connections; //Figure out what they want to do with the connection list
        private List<NetIncomingMessage> peersList;
        private static Queue<string> moveQueue;

        public KulamiPeer()
        {

            config = new NetPeerConfiguration(signature);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.UnconnectedData);
            config.EnableMessageType(NetIncomingMessageType.Data);
            config.AutoFlushSendQueue = true;
            config.Port = 3070;
            config.AcceptIncomingConnections = true;
            moveQueue = new Queue<string>();
            peer = new NetPeer(config);
            peer.Start();
            listener = new peerListener(peer, localIdentifier);
            localIdentifier = peer.UniqueIdentifier;
            connections = new List<NetIncomingMessage>();
            NetThread = new Thread(new ThreadStart(listener.processNetwork));
            NetThread.Start();
        }

        public void killPeer() // Do deconstructors get implicitly called in C# or do they need to be called?
        {
            NetThread.Abort();
            peer.Shutdown("Thread and Peer Terminated. No longer doing Network games\n");
            Console.WriteLine("Bye Bye!");
        }

        public bool sendMove(string move)
        {
            NetOutgoingMessage message = KulamiPeer.peer.CreateMessage();
            if (listener.connection != null)
            {
                message.Write(move);
                KulamiPeer.peer.SendMessage(message, listener.connection, NetDeliveryMethod.ReliableOrdered);
                return true;
            }
            else
                return false;          
        }

        public string getMove()
        {
            if (moveQueue.Count != 0)
                return moveQueue.Dequeue();
            else
                return "";
        }

        public void sendChatMessage(string text)
        {
            NetOutgoingMessage message = KulamiPeer.peer.CreateMessage();
            message.Write(signature);
            message.Write((int)Protocol.messageType.chat);
            message.Write(text);
            KulamiPeer.peer.SendMessage(message, listener.connection, NetDeliveryMethod.ReliableOrdered);
        }

        public void broadcastMessage(string text)
        {
            NetOutgoingMessage om = peer.CreateMessage(text);
            IPEndPoint receiver = new IPEndPoint(NetUtility.Resolve("localhost"), peer.Port);
            peer.SendUnconnectedMessage(om, receiver);
        }


    }
}
