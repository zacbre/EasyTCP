using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace EasyTCP.TCP.Events
{
    public class ClientArgs : EventArgs
    {
        public ClientArgs(TCPClient p)
        {
            Client = p;
        }
        public TCPClient Client;
    }
}
