using System;

namespace HttpListenerExample
{
    class Program
    {
        public static void Main(string[] args)
        {
            HttpServer server = new HttpServer();
            server.HandleIncomingRequests().GetAwaiter().GetResult();
        }
    }
}
