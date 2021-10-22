using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Network_Programming_Examples
{
    public class Server
    {
        
        public static void StartServer(int portNumber)
        {
            IPAddress IP = Dns.GetHostEntry("localhost").AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(IP, portNumber);

            try
            {
                Socket listener = new Socket(IP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                listener.Bind(localEndPoint);

                listener.Listen(10);

                Console.WriteLine("Waiting for a connection...");

                
                Socket handler = listener.Accept();

                string data = null;
                byte[] bytes = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);   

                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }

                }
                
                Console.WriteLine("Text received. : {0}", data);
               
                for (int i = 0; i < bytes.Length; i++)
                {
                    Console.WriteLine(bytes[i]);
                }
                

                byte[] msg = Encoding.ASCII.GetBytes(data);
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
             
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            

            Console.WriteLine("\n Press any key to continue...");
            Console.ReadKey();

        }

        public static void SendMessage(int portNumber)
        {
            byte[] bytes = new byte[1024];

            try
            {
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, portNumber);

                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(localEndPoint);

                    Console.WriteLine("Socket connected to {0} using {1}.", sender.RemoteEndPoint.ToString(), sender.ProtocolType.ToString());

                    Console.WriteLine("Enter message to send to the server.");
                    string message = Console.ReadLine();


                    byte[] msg = Encoding.ASCII.GetBytes(message + "<EOF>");

                    int bytesSent = sender.Send(msg);

                    int bytesReceived = sender.Receive(bytes);

                    Console.WriteLine("Echo = {0}", Encoding.ASCII.GetString(bytes, 0, bytesReceived));

                    sender.Shutdown(SocketShutdown.Both);

                    sender.Close();

                }

                catch(ArgumentNullException ex)
                {
                    Console.WriteLine("ArgumentNullException" + ex.Message);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        

    }
}
