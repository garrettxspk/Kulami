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
        public static List<NetIncomingMessage> connections; //Figure out what they want to do with the connection list
        private static Queue<string> moveQueue;

        public KulamiPeer()
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
            config.Port = 3070;
            config.AcceptIncomingConnections = true;

            peer = new NetPeer(config);
            peer.Start();
            localIdentifier = peer.UniqueIdentifier;
            listener = new peerListener(peer, localIdentifier);
            connections = new List<NetIncomingMessage>();
            moveQueue = new Queue<string>();
            NetThread = new Thread(new ThreadStart(listener.processNetwork));
            NetThread.Start();
        }

        public void killPeer()
        {
            listener.shouldQuit();
            NetThread.Abort();
            peer.Shutdown("Thread and Peer Terminated. No longer doing Network games\n");
            Console.WriteLine("Bye Bye!");
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
            string result = "";
            while (listener.errorMessage == "")
            {
                if (moveQueue.Count != 0)
                {
                    result = moveQueue.Dequeue();
                    break;
                }
            }

            if (result == "")
            {
                result = listener.errorMessage;
                Console.WriteLine(result);
            }

            return result;
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

        public void quit()
        {
            listener.shouldQuit();
        }
    }
}
