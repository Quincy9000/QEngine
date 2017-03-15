using System;
using Lidgren.Network;
using QEngine.Interfaces;
using QEngine.System;

namespace QEngine.Prefabs
{
	public enum ConnectionType
	{
		Server,
		Client,
		Wait
	}

	public class QNetwork : QBehavior, IQUpdate
	{
		public NetPeer Peer { get; private set; }

		public ConnectionType Connection { get; private set; }

		public QNetwork(ConnectionType type, string ip = "") : base("QNetwork")
		{
			Connection = type;
			switch(Connection)
			{
				case ConnectionType.Server:
				{
					var config = new NetPeerConfiguration(Window.Title);
					config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
					config.EnableMessageType(NetIncomingMessageType.DebugMessage);
					config.EnableMessageType(NetIncomingMessageType.Error);
					config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
					config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
					config.EnableMessageType(NetIncomingMessageType.WarningMessage);
					config.Port = 5001;
					Peer = new NetServer(config);
					Peer.Start();
				}
					break;
				case ConnectionType.Client:
				{
					var config = new NetPeerConfiguration(Window.Title);
					config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
					config.EnableMessageType(NetIncomingMessageType.DebugMessage);
					config.EnableMessageType(NetIncomingMessageType.Error);
					config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
					config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
					config.EnableMessageType(NetIncomingMessageType.WarningMessage);
					Peer = new NetClient(config);
					Peer.Start();
					Peer.Connect(ip, 5001);
				}
					break;
				case ConnectionType.Wait:
				{
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void StartConnection(ConnectionType type, string ip = "")
		{
			Connection = type;
			switch(Connection)
			{
				case ConnectionType.Server:
				{
					var config = new NetPeerConfiguration(Window.Title);
					config.Port = 5001;
					Peer = new NetServer(config);
					((NetServer)Peer).Start();
				}
					break;
				case ConnectionType.Client:
				{
					var config = new NetPeerConfiguration(Window.Title);
					Peer = new NetClient(config);
					((NetClient)Peer).Start();
					((NetClient)Peer).Connect(ip, 5001);
				}
					break;
				case ConnectionType.Wait:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void Disconnect()
		{
			Peer?.Shutdown("");
		}

		public void SendMessage(IQMessage m)
		{
			NetOutgoingMessage om = Peer.CreateMessage();
			om.Write((byte)m.MessageType);
			m.Encode(om);

			if(Peer is NetServer server)
				server.SendToAll(om, NetDeliveryMethod.UnreliableSequenced);
			if(Peer is NetClient client)
				client.SendMessage(om, NetDeliveryMethod.UnreliableSequenced);
		}

//		public void SendUpdate(string updateParameter)
//		{
//			switch(Connection)
//			{
//				case ConnectionType.Client:
//				{
//					var client = (NetClient)Peer;
//					var m = client.CreateMessage(updateParameter);
//					client?.SendMessage(m, NetDeliveryMethod.ReliableUnordered);
//					break;
//				}
//				case ConnectionType.Server:
//				{
//					var server = (NetServer)Peer;
//					var m = server.CreateMessage(updateParameter);
//					server?.SendToAll(m, NetDeliveryMethod.ReliableUnordered);
//					break;
//				}
//			}
//		}
//
//		//TODO: Figure out how to properly api sending messages to clients
//		public void SendMessage(string msg, int id)
//		{
//			switch(Connection)
//			{
//				case ConnectionType.Client:
//				{
//					var client = (NetClient)Peer;
//					var m = client.CreateMessage(msg);
//					client?.SendMessage(m, client.Connections[id], NetDeliveryMethod.ReliableUnordered);
//					break;
//				}
//				case ConnectionType.Server:
//				{
//					var server = (NetServer)Peer;
//					var m = server.CreateMessage(msg);
//					server?.SendMessage(m, server.Connections[id], NetDeliveryMethod.ReliableUnordered);
//					break;
//				}
//			}
//		}
//		
//		public void SendMessage(string msg, NetConnection peer)
//		{
//			switch(Connection)
//			{
//				case ConnectionType.Client:
//				{
//					var client = (NetClient)Peer;
//					var m = client.CreateMessage(msg);
//					client?.SendMessage(m, peer, NetDeliveryMethod.ReliableUnordered);
//					break;
//				}
//				case ConnectionType.Server:
//				{
//					var server = (NetServer)Peer;
//					var m = server.CreateMessage(msg);
//					server?.SendMessage(m, peer, NetDeliveryMethod.ReliableUnordered);
//					break;
//				}
//			}
//		}

		public delegate void IncommingMessage(NetIncomingMessage message);

		public event IncommingMessage OnIncommingMessage;

		public void Update(QTime time)
		{
			switch(Connection)
			{
				case ConnectionType.Server:
				{
					if(Peer is NetServer server)
					{
						NetIncomingMessage messageIn;
						while((messageIn = server.ReadMessage()) != null)
						{
							OnIncommingMessage?.Invoke(messageIn);
							server.Recycle(messageIn);
						}
					}
					break;
				}
				case ConnectionType.Client:
				{
					if(Peer is NetClient client)
					{
						NetIncomingMessage messageIn;
						while((messageIn = client.ReadMessage()) != null)
						{
							OnIncommingMessage?.Invoke(messageIn);
							client.Recycle(messageIn);
						}
					}
					break;
				}
				case ConnectionType.Wait:
					break;
			}
		}
	}
}