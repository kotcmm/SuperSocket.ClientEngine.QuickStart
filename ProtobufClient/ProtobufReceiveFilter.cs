using Google.ProtocolBuffers;
using SuperSocket.ProtoBase;

namespace ProtobufClient
{
    /// <summary>
    ///     A decoder that splits the received {@link ByteBuf}s dynamically by the
    ///     value of the Google Protocol Buffers
    ///     <a href="http://code.google.com/apis/protocolbuffers/docs/encoding.html#varints">
    ///         Base
    ///         128 Varints
    ///     </a>
    ///     integer length field in the message.For example:
    ///    
    ///         BEFORE DECODE (302 bytes)       AFTER DECODE (300 bytes)
    ///         +--------+---------------+      +---------------+
    ///         | Length | Protobuf Data |----->| Protobuf Data |
    ///         | 0xAC02 |  (300 bytes)  |      |  (300 bytes)  |
    ///         +--------+---------------+      +---------------+
    /// </summary>
    public class ProtobufReceiveFilter : IReceiveFilter<ProtobufPackageInfo>
    {
        public ProtobufPackageInfo Filter(BufferList data, out int rest)
        {
            rest = 0;
            var buffStream = new BufferStream();
            buffStream.Initialize(data);

            var stream = CodedInputStream.CreateInstance(buffStream);
            var varint32 = (int) stream.ReadRawVarint32();
            if (varint32 <= 0) return default(ProtobufPackageInfo);

            var total = data.Total;
            var packageLen = varint32 + (int) stream.Position;

            if (total >= packageLen)
            {
                rest = total - packageLen;
                var body = stream.ReadRawBytes(varint32);
                var message = DefeatMessage.ParseFrom(body);
                var requestInfo = new ProtobufPackageInfo(message.Type, message);
                return requestInfo;
            }

            return default(ProtobufPackageInfo);
        }

        public IReceiveFilter<ProtobufPackageInfo> NextReceiveFilter { get; protected set; }
        public FilterState State { get; protected set; }

        public void Reset()
        {
            NextReceiveFilter = null;
            State = FilterState.Normal;
        }
    }
}