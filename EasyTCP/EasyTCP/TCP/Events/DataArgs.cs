using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace EasyTCP.TCP.Events
{
    public class DataArgs : EventArgs
    {
        public DataArgs(TCPClient client, byte[] buffer, int offset, int count)
        {
            Client = client;
            Buffer = buffer;
            Offset = offset;
            Count = count;
        }
        public TCPClient Client;
        public byte[] Buffer;
        public int Offset;
        public int Count;
    }
}
