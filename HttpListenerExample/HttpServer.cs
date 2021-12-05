using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace HttpListenerExample
{
    internal class HttpServer
    {
        public HttpServer()
        {
            listener = new HttpListener();
            listener.Prefixes.Add(url);
        }

        public async Task HandleIncomingRequests()
        {
            bool runServer = true;
            listener.Start();
            while(runServer)
            {
                Console.WriteLine(runServer);
                HttpListenerContext context = await listener.GetContextAsync();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(request.Url?.AbsolutePath);
                Console.WriteLine(request.HttpMethod);
                Console.WriteLine(request.UserHostName);
                Console.WriteLine(request.UserAgent);
                Console.WriteLine();
                if (request.HttpMethod == "POST" && request.Url?.AbsolutePath == "/shutdown")
                    runServer = false;
                if (request.Url?.AbsolutePath != "/favicon.ico")
                    ++pageViews;

                string submitDisabled = !runServer ? "disabled" : "";
                byte[] data = Encoding.UTF8.GetBytes(string.Format(pageData, pageViews, submitDisabled));

                response.ContentType = "text/html";
                response.ContentLength64 = data.LongLength;
                response.ContentEncoding = Encoding.UTF8;
                await response.OutputStream.WriteAsync(data);
                response.Close();
            }
            listener.Close();
        }

        private HttpListener listener;
        private const string url = "http://localhost:8000/";
        private static int pageViews = 0;
        private static int requestCount = 0;
        private static string pageData =
            "<!DOCTYPE>" +
            "<html>" +
            "  <head>" +
            "    <title>HttpListener Example</title>" +
            "  </head>" +
            "  <body>" +
            "    <p>Page Views: {0}</p>" +
            "    <form method=\"post\" action=\"shutdown\">" +
            "      <input type=\"submit\" value=\"Shutdown\" {1}>" +
            "    </form>" +
            "  </body>" +
            "</html>";
    }
}
