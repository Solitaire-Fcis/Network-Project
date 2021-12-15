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
        Socket serverSocket;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            //TODO: initialize this.serverSocket
            this.LoadRedirectionRules(redirectionMatrixPath);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint hostEndPoint = new IPEndPoint(IPAddress.Any, portNumber);
            serverSocket.Bind(hostEndPoint);
            byte[] data;
        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(1000);
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                Socket clientSocket = this.serverSocket.Accept();
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
            string WelcomeMessage = "Welcome To My Server";
            byte[] data = Encoding.ASCII.GetBytes(WelcomeMessage);
            RevievedClientSocket.Send(data);
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
                    Request req = new Request(Encoding.ASCII.GetString(data));
                    // TODO: Call HandleRequest Method that returns the response
                    
                    // TODO: Send Response back to client

                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                }
            }

            // TODO: close client socket
            RevievedClientSocket.Close();
        }

        Response HandleRequest(Request request)
        {
            throw new NotImplementedException();
            string content;
            try
            {
                //TODO: check for bad request 

                //TODO: map the relativeURI in request to get the physical path of the resource.

                //TODO: check for redirect

                //TODO: check file exists

                //TODO: read the physical file

                // Create OK response
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                // TODO: in case of exception, return Internal Server Error. 
            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            
            // else read file and return its content
            return string.Empty;
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                // then fill Configuration.RedirectionRules dictionary 
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Environment.Exit(1);
            }
        }
    }
}
