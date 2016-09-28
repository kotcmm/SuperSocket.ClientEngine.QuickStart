using SuperSocket.SocketBase.Protocol;

namespace ProtobufServer
{
    public class ProtobufRequestInfo : IRequestInfo
    {
        public ProtobufRequestInfo(DefeatMessage.Types.Type type, DefeatMessage body)
        {
            Type = type;
            Key = type.ToString();
            Body = body;
        }

        public DefeatMessage.Types.Type Type { get; private set; }

        public DefeatMessage Body { get; private set; }

        public string Key { get; set; }
    }
}