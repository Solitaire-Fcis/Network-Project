using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        string TypeOfContent = "text/html";
        Socket serverSocket;
        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            //TODO: initialize this.serverSocket
            LoadRedirectionRules(redirectionMatrixPath);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint hostEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), portNumber);
            serverSocket.Bind(hostEndPoint);
        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(1000);
            Console.WriteLine("\t\t\t\t\tWelCome To Fcis Network Server !\t\t\t\t\t");
            Console.WriteLine("Waiting For Connection....[IP:127.0.0.1][Port:1000]");
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
              
                Socket clientSocket = serverSocket.Accept();
                Console.WriteLine("New Client Just Landed On Our Server:{0}", clientSocket.RemoteEndPoint);
                Thread OpenNewThread = new Thread(new ParameterizedThreadStart(HandleConnection));
                OpenNewThread.Start(clientSocket);
                //TODO: accept connections and start thread for each accepted connection.

            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            Socket RevievedClientSocket = (Socket)obj;
            byte[] data;
            int recievedLength;
            RevievedClientSocket.ReceiveTimeout = 0;
            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    // TODO: Receive request
                    data = new byte[1024];
                    recievedLength = RevievedClientSocket.Receive(data);
                    // TODO: break the while loop if receivedLen==0
                    
                    if (recievedLength==0)
                    {
                        Console.WriteLine("Client with IP:{0} ended the connection with us :(", RevievedClientSocket.RemoteEndPoint);
                        break;
                    }
                    // TODO: Create a Request object using received request string
                    Request req = new Request(Encoding.ASCII.GetString(data,0,recievedLength));
                    // TODO: Call HandleRequest Method that returns the response
                    Response res = HandleRequest(req);
                    // TODO: Send Response back to client
                    data = Encoding.ASCII.GetBytes(res.ResponseString);
                    RevievedClientSocket.Send(data);
                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }

            // TODO: close client socket
            RevievedClientSocket.Close();
        }

        Response HandleRequest(Request request)
        {
            //throw new NotImplementedException();
            string content;
            try
            {
                //TODO: check for bad request 
                if (request.ParseRequest()==false)
                {
                    content = LoadDefaultPage(Configuration.BadRequestDefaultPageName);
                    return new Response(StatusCode.BadRequest, TypeOfContent, content, null);
                }
                //TODO: map the relativeURI in request to get the physical path of the resource.
                //TODO: check for redirect
                if (Configuration.RedirectionRules.ContainsKey(request.relativeURI))
                {
                    string Redirected_page = Configuration.RedirectionRules[request.relativeURI];
                    content = LoadDefaultPage(GetRedirectionPagePathIFExist(Redirected_page));
                    return new Response(StatusCode.Redirect, TypeOfContent, content, Redirected_page);
                }
                if (request.relativeURI == string.Empty)
                {
                    if (File.Exists(Path.Combine(Configuration.RootPath, Configuration.MainPageName)))
                    {
                        content = LoadDefaultPage(Configuration.MainPageName);
                        return new Response(StatusCode.OK, TypeOfContent, content, null);
                    }
                }
                //TODO: check file exists

                //TODO: read the physical file
                // Create OK response
                content = LoadDefaultPage(request.relativeURI);
                if (content != "")
                    return new Response(StatusCode.OK, TypeOfContent, content, null);
                
                //404 Not Found
                content = LoadDefaultPage(Configuration.NotFoundDefaultPageName);
                return new Response(StatusCode.NotFound, TypeOfContent, content, null);
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error. 
                content = LoadDefaultPage(Configuration.InternalErrorDefaultPageName);
                return new Response(StatusCode.InternalServerError, TypeOfContent, content, null);
            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            if (File.Exists(Configuration.RootPath+"\\"+relativePath))
            {
                return relativePath;
            }
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            if (!File.Exists(filePath))
            {
                Logger.LogException(new Exception(defaultPageName + " doesn't exist"));
                return string.Empty;
            }
            return File.ReadAllText(filePath);
            // else read file and return its content
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                int i = 0;
                // TODO: using the filepath paramter read the redirection rules from file 
                string[] space =new string[] { "\r\n" };
                string[] redirectionalRules = File.ReadAllText(filePath).Split(space,StringSplitOptions.RemoveEmptyEntries);
                // then fill Configuration.RedirectionRules dictionary 
                Configuration.RedirectionRules = new Dictionary<string, string>();
                while (i<redirectionalRules.Length)
                {
                    string[] Page = redirectionalRules[i].Split(',');
                    Configuration.RedirectionRules.Add(Page[0], Page[1]);
                    i++;
                }
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                Environment.Exit(1);
            }
        }
    }
}
