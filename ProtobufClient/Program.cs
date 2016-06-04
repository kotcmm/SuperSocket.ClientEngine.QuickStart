using System;
using System.IO;
using System.Net;
using Google.ProtocolBuffers;
using SuperSocket.ClientEngine;

namespace ProtobufClient
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			EasyClient client = new EasyClient();
            client.Initialize(new ProtobufReceiveFilter(), (info =>
            {
                switch (info.Type)
                {
                    case DefeatMessage.Types.Type.BackMessage:
                        Console.WriteLine("BackMessage:{0}", info.Body.BackMessage.Content);
                        break;
                    case DefeatMessage.Types.Type.CallMessage:
                        Console.WriteLine("CallMessage:{0}", info.Body.CallMessage.Content);
                        break;

                }
            }));
		    var flag = client.ConnectAsync(new DnsEndPoint("127.0.0.1", 2012));
		    if (flag.Result)
		    {
		        var callMessage = CallMessage.CreateBuilder()
                    .SetContent("Hello I am form C# client by SuperSocket ClientEngine").Build();
		        var message = DefeatMessage.CreateBuilder()
		            .SetType(DefeatMessage.Types.Type.CallMessage)
		            .SetCallMessage(callMessage).Build();

                using (var stream = new MemoryStream())
                {

                    CodedOutputStream os = CodedOutputStream.CreateInstance(stream);

                    os.WriteMessageNoTag(message);

                    os.Flush();

                    byte[] data = stream.ToArray();
					client.Send(new ArraySegment<byte>(data));
                   
                }
                
            }
            Console.ReadKey();
        }
	}
}
