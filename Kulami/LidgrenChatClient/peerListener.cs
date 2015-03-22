﻿using System;
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
            //public static Form1 look = new Form1();

            public peerListener(NetPeer newPeer, long id)
            {
                //Copies of KulamiPeer's peer and machineIdentifier
                peer = newPeer;
                localId = id;
            }

            public void processNetwork()
            {
                KulamiPeer.peer.DiscoverLocalPeers(3070);
                Thread.Sleep(1000);// Is this needed?
                connection = null;
                Console.WriteLine("About to read messages... ");
                string signature;
                while (!quit)
                {

                    Thread.Sleep(1);
                    if (KulamiPeer.peer == null)
                        continue;
                    //spin loop for the thread responsible for passing and recieving messages.
                    NetIncomingMessage msg;
                    while ((msg = KulamiPeer.peer.ReadMessage()) != null)
                    {
                        //Console.WriteLine(msg.ReadString() + "\n");
                        switch (msg.MessageType)
                        {
                            case NetIncomingMessageType.DiscoveryRequest:
                                Console.WriteLine("Recieved Discovery Request");
                                NetOutgoingMessage response = peer.CreateMessage();
                                response.Write(SIGNATURE);
                                response.Write(localId.ToString());
                                KulamiPeer.peer.SendDiscoveryResponse(response, msg.SenderEndPoint);
                                KulamiPeer.connections.Add(msg);
                                break;

                            case NetIncomingMessageType.DiscoveryResponse:
                                Console.WriteLine("Found peer at " + msg.SenderEndPoint);
                                if (msg != null) //Make sure we aren't connecting to ourself?
                                {
                                    signature = msg.ReadString();
                                    if (signature == SIGNATURE)
                                    {
                                        string peerIdAsString = msg.ReadString();
                                        peerId = Convert.ToInt64(peerIdAsString);
                                        if (peerId != localId)
                                        {
                                            KulamiPeer.peer.Connect(msg.SenderEndPoint);
                                            connection = msg.SenderConnection;
                                        }
                                    }

                                }
                                break;

                            case NetIncomingMessageType.ConnectionApproval:
                                if (msg != null && (msg.SenderEndPoint.ToString() != (GetLocalIP() + ":3070")))
                                {
                                    msg.SenderConnection.Approve();
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

                            case NetIncomingMessageType.UnconnectedData:
                                string orphanData = msg.ReadString();
                                Console.WriteLine("UnconnectedData: " + orphanData);
                                break;
                        }

                        if (msg.SenderConnection != null && connection == null)
                        {
                            connection = msg.SenderConnection;
                        }
                        //if (connection != null)
                        //{
                        //    Console.WriteLine("Enter the string you would like to send.\n");
                        //    input = Console.ReadLine();
                        //    myMessage.Write(input);
                        //   
                        //    chat = KulamiPeer.peer.ReadMessage();
                        //    Console.WriteLine(chat.ToString());
                        //}
                        //KulamiPeer.peer.SendMessage(myMessage, )
                    }
                }
            }


            public string GetLocalIP()
            {
                IPHostEntry host;
                host = Dns.GetHostEntry(Dns.GetHostName());

                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "127.0.0.1";
            }

            public void shouldQuit()
            {
                quit = true;
            }
        }
    }
}
