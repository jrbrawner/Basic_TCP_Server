using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

// Client app is the one sending messages to a Server/listener.   
// Both listener and client can send messages back and forth once a   
// communication is established.  
public class SocketClient
{
    public static int Main(String[] args)
    {
        SendMessage(11000);
        return 0;
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

            catch (ArgumentNullException ex)
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
