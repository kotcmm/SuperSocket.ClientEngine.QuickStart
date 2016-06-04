using SuperSocket.ProtoBase;

namespace ProtobufClient
{
    public class ProtobufPackageInfo : IPackageInfo
    {
        public ProtobufPackageInfo(DefeatMessage.Types.Type type, DefeatMessage body)
        {
            Type = type;
            Key = type.ToString();
            Body = body;
        }

        public string Key { get; private set; }

        public DefeatMessage Body { get; private set; }
        public DefeatMessage.Types.Type Type { get; private set; }
    }
}