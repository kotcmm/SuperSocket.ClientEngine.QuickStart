using System;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace ProtobufServer
{
	public class ProtobufAppServer : AppServer<ProtobufAppSession,ProtobufRequestInfo>
	{
		public ProtobufAppServer ()
            :base(new DefaultReceiveFilterFactory< ProtobufReceiveFilter, ProtobufRequestInfo >())
		{
		}
	}
}

