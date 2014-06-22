using System;
using System.Collections.Generic;
using System.Text;
using EasyTCP.TCP;
using System.Net;
namespace TestClient
{
    class Program
    {
        static List<TCPClient> p = new List<TCPClient>();
        static void Main(string[] args)
        {
            TCPServer f = new TCPServer(IPAddress.Any, 3058);
            f.Connected += f_Connected;
            f.Start();
            //start up client.
            TCPClient x = new TCPClient();
            x.Connected += x_Connected;
            x.Received += x_Received;
            x.Connect(IPAddress.Loopback, 3058);
            Console.ReadLine();
        }

        static void x_Received(object sender, EasyTCP.TCP.Events.DataArgs e)
        {
            Console.WriteLine("Received From Server: \"{0}\".", Encoding.ASCII.GetString(e.Buffer, 0, e.Count));
            //send back to server.
            e.Client.Send(Encoding.ASCII.GetBytes("Data for Server."));
            e.Client.Disconnect();
        }

        static void x_Connected(object sender, EasyTCP.TCP.Events.ClientArgs e)
        {
            Console.WriteLine("Client Connected To Server.");
        }

        static void f_Connected(object sender, EasyTCP.TCP.Events.ClientArgs e)
        {
            //received connection.
            p.Add(e.Client);
            e.Client.Received += Client_Received;
            e.Client.Disconnected += Client_Disconnected;
            //send data to it.
            e.Client.Send(Encoding.ASCII.GetBytes("Data for Client."));
        }

        static void Client_Disconnected(object sender, EasyTCP.TCP.Events.ClientArgs e)
        {
            Console.WriteLine("Client Disconnected from Server.");
        }

        static void Client_Received(object sender, EasyTCP.TCP.Events.DataArgs e)
        {
            Console.WriteLine("Server Receieved: \"{0}\".", Encoding.ASCII.GetString(e.Buffer, 0, e.Count));
            //Disconnect Client.
        }
    }
}
