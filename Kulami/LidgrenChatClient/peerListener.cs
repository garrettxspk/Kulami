using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Lidgren.Network;
using System.Net;
using System.Net.Sockets;

namespace LidgrenKulamiPeer
{
    public partial class KulamiPeer
    {
        public class peerListener
        {
            private bool quit = false;

            public NetConnection connection;
            public static NetPeer peer = null;
            private static long localId;
            private static long peerId;
            private string SIGNATURE = "team2";
            private static int numberOfConnections;
            public string errorMessage = "";

            private bool isDisconnected;

            public bool IsDisconnected
            {
                get { return isDisconnected; }
            }

            //public static Form1 look = new Form1();


            public peerListener(NetPeer newPeer, long id)
            {
                peer = newPeer;
                localId = id;
            }

            public void processNetwork()
            {
                KulamiPeer.peer.DiscoverLocalPeers(peer.Configuration.Port);
                Thread.Sleep(1000);
                connection = null;
                numberOfConnections = 0;
                Console.WriteLine("About to read messages... ");
                string signature;
                while (!quit)
                {
                    Thread.Sleep(1);
                    if (KulamiPeer.peer == null)
                        continue;
                    NetIncomingMessage msg;
                    while ((msg = KulamiPeer.peer.ReadMessage()) != null)
                    {
                        switch (msg.MessageType)
                        {
                            case NetIncomingMessageType.DiscoveryRequest:
                                Console.WriteLine("Recieved Discovery Request");
                                NetOutgoingMessage response = peer.CreateMessage();
                                response.Write(SIGNATURE);
                                response.Write(localId.ToString());
                                response.Write(numberOfConnections);
                                KulamiPeer.peer.SendDiscoveryResponse(response, msg.SenderEndPoint);
                                break;

                            case NetIncomingMessageType.DiscoveryResponse:
                                Console.WriteLine("Found peer at " + msg.SenderEndPoint);
                                if (msg != null)
                                {
                                    signature = msg.ReadString();
                                    if (signature == SIGNATURE)
                                    {
                                        string peerIdAsString = msg.ReadString();
                                        peerId = Convert.ToInt64(peerIdAsString);
                                        if (peerId != localId)
                                        {
                                            int numberOfPeerConnections = msg.ReadInt32();
                                            if (msg.SenderConnection == null && numberOfPeerConnections == 0 && numberOfConnections == 0)//don't connect to a peer already connected to someone else
                                            {
                                                Thread.BeginCriticalRegion();
                                                numberOfConnections++;
                                                NetOutgoingMessage hail = peer.CreateMessage();
                                                hail.Write(SIGNATURE);
                                                hail.Write(localId.ToString());
                                                hail.Write(numberOfConnections);                                       
                                                KulamiPeer.peer.Connect(msg.SenderEndPoint, hail);
                                                connection = msg.SenderConnection;
                                                Thread.EndCriticalRegion();
                                            }
                                            
                                        }
                                    }
                                }
                                break;

                            case NetIncomingMessageType.ConnectionApproval:
                                if (msg != null)
                                {
                                    signature = msg.ReadString();
                                    if (signature == SIGNATURE)
                                    {
                                        string peerIdAsString = msg.ReadString();
                                        peerId = Convert.ToInt64(peerIdAsString);
                                        if (peerId != localId)
                                        {
                                            int numberOfPeerConnections = msg.ReadInt32();
                                            if (numberOfPeerConnections == 1 && numberOfConnections == 0)
                                            {
                                                //connection = msg.SenderConnection;
                                                msg.SenderConnection.Approve();
                                                numberOfConnections++;
                                                isDisconnected = false;
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    msg.SenderConnection.Deny();
                                }
                                break;

                            case NetIncomingMessageType.Data: //Moves being sent
                                signature = msg.ReadString();
                                if (signature == SIGNATURE)
                                {
                                    string move = msg.ReadString();
                                    KulamiPeer.moveQueue.Enqueue(move);
                                    Console.WriteLine("Move Recieved: " + move);
                                }
                                break;

                            case NetIncomingMessageType.StatusChanged:
                                byte recievedByte = msg.ReadByte();
                                Console.WriteLine((NetConnectionStatus)recievedByte);
                                if ((NetConnectionStatus)recievedByte == NetConnectionStatus.Disconnected)
                                {
                                    errorMessage = Convert.ToString((NetConnectionStatus)recievedByte);
                                    isDisconnected = true;
                                }
                                break;

                            case NetIncomingMessageType.UnconnectedData:
                                string orphanData = msg.ReadString();
                                Console.WriteLine("UnconnectedData: " + orphanData);
                                break;
                        }

                        if (msg.SenderConnection != null && connection == null)
                        {
                            connection = msg.SenderConnection;
                            isDisconnected = false;
                        }
                    }
                }
            }

            public void shouldQuit()
            {
                quit = true;
            }
        }
    }
}
