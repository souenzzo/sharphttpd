using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace sharphttpd
{
    class Program
    {
        public static string data = null;
        static void Main(string[] args)
        {
            var port = int.Parse(args[0]);
            byte[] bytes = new Byte[1024];

            Socket listener = new Socket(SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            listener.Bind(new IPEndPoint(0, port));
            listener.Listen(10);

            Console.WriteLine("Waiting for a connection on {0}...", port); 
            Console.WriteLine("Run something like `echo 'oi <EOF>' | nc 127.0.0.1 8080`");
            Socket handler = listener.Accept();
            data = null;

            // An incoming connection needs to be processed.  
            while (true)
            {
                int bytesRec = handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (data.IndexOf("<EOF>") > -1)
                {
                    break;
                }
            }

            // Show the data on the console.  
            Console.WriteLine("Text received : {0}", data);

            // Echo the data back to the client.  
            byte[] msg = Encoding.ASCII.GetBytes(data);

            handler.Send(msg);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
    }
}
