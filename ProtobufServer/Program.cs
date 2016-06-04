using System;
using System.IO;
using Google.ProtocolBuffers;

namespace ProtobufServer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            Console.WriteLine("Press any key to start the server!");

            Console.ReadKey();
            Console.WriteLine();

            var appServer = new ProtobufAppServer();

            //Setup the appServer
            if (!appServer.Setup(2012)) //Setup with listening port
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();

            appServer.NewRequestReceived += AppServerOnNewRequestReceived;

            //Try to start the appServer
            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("The server started successfully, press key 'q' to stop it!");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            //Stop the appServer
            appServer.Stop();

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }

	    private static void AppServerOnNewRequestReceived(ProtobufAppSession session, ProtobufRequestInfo requestInfo)
	    {
	        switch (requestInfo.Type)
	        {
                case DefeatMessage.Types.Type.BackMessage:
                    Console.WriteLine("BackMessage:{0}", requestInfo.Body.BackMessage.Content);
                    break;
                case DefeatMessage.Types.Type.CallMessage:
	                Console.WriteLine("CallMessage:{0}", requestInfo.Body.CallMessage.Content);

                    var backMessage = BackMessage.CreateBuilder()
					.SetContent("Hello I am form C# server by SuperSocket").Build();
                    var message = DefeatMessage.CreateBuilder()
                        .SetType(DefeatMessage.Types.Type.BackMessage)
                        .SetBackMessage(backMessage).Build();

                    using (var stream = new MemoryStream())
                    {

                        CodedOutputStream os = CodedOutputStream.CreateInstance(stream);

                        os.WriteMessageNoTag(message);

                        os.Flush();

                        byte[] data = stream.ToArray();
                        session.Send(new ArraySegment<byte>(data));

                    }


                    break;

            }
	    }
	}
}
