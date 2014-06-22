using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using EasyTCP.TCP.Events;
using EasyTCP.TCP;
namespace EasyTCP.TCP
{
    public class TCPServer
    {
        public event EventHandler<ClientArgs> Connected = delegate { };
        private ManualResetEvent Task = new ManualResetEvent(false);
        private bool accept = false;
        private Socket p;
        public TCPServer(IPAddress bindto, int Port)
        {
            p = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                p.Bind(new IPEndPoint(bindto, Port));
            }
            catch (SocketException sx)
            {
                //error.
#if DEBUG
                Console.WriteLine("[Debug] Cannot bind to port {0}.", Port);
#endif
            }
        }

        public void Stop()
        {
            if (accept)
            {
                accept = false;
                p.Disconnect(true);
            }
        }

        public void Start()
        {
            if (!accept)
            {
                accept = true;
                p.Listen(10000);
                new Thread(new ThreadStart(AcceptConnections)).Start();
            }
        }

        private void AcceptConnections()
        {
            while (accept)
            {
                try
                {
                    Task.Reset();
                    p.BeginAccept(new AsyncCallback(AcceptClient), p);
                    Task.WaitOne();
                }
                catch
                { //error, most likely server shutdown.
                }
            }
        }

        private void AcceptClient(IAsyncResult res)
        {
            Socket x = ((Socket)res.AsyncState).EndAccept(res);
            //start up client.
            Task.Set();
            Connected(this, new ClientArgs(new TCPClient(x)));
        }
    }
}
