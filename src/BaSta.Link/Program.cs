using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BaSta.Link
{
    /// <summary>
    /// Program class for main entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main entry point.
        /// </summary>
        public static void Main()
        {
            // TcpListener server = null;
            // try
            // {
            //     // Set the TcpListener on port 13000.
            //     int port = 4000;
            //     IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            //
            //     // TcpListener server = new TcpListener(port);
            //     server = new TcpListener(localAddr, port);
            //
            //     // Start listening for client requests.
            //     server.Start();
            //
            //     // Buffer for reading data
            //     byte[] bytes = new Byte[256];
            //     string data;
            //
            //     // Enter the listening loop.
            //     while (true)
            //     {
            //         Console.Write("Waiting for a connection... ");
            //
            //         // Perform a blocking call to accept requests.
            //         // You could also user server.AcceptSocket() here.
            //         TcpClient client = server.AcceptTcpClient();
            //         Console.WriteLine("Connected!");
            //
            //         // Get a stream object for reading and writing
            //         NetworkStream stream = client.GetStream();
            //
            //         int i = 59;
            //
            //         const string template = "<Template Name=\"SEMAPHORE\"><PARAM Name=\"MINUTES\" Value=\"9\"/><PARAM Name=\"SECONDS\" Value=\"{0}\"/><PARAM Name=\"PERIOD\" Value=\"3\"/></Template>";
            //
            //         while (i-- > 0)
            //         {
            //             Thread.Sleep(1000);
            //
            //             string output = string.Format(template, i);
            //
            //             Console.WriteLine($"Writing: {output}");
            //
            //             stream.Write(Encoding.ASCII.GetBytes(output));
            //         }
            //
            //
            //         //// Loop to receive all the data sent by the client.
            //         //while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            //         //{
            //         //    // Translate data bytes to a ASCII string.
            //         //    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            //         //    Console.WriteLine("Received: {0}", data);
            //
            //         //    // Process the data sent by the client.
            //         //    data = data.ToUpper();
            //
            //         //    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
            //
            //         //    // Send back a response.
            //         //    stream.Write(msg, 0, msg.Length);
            //         //    Console.WriteLine("Sent: {0}", data);
            //         //}
            //
            //         // Shutdown and end connection
            //         client.Close();
            //     }
            // }
            // catch (SocketException e)
            // {
            //     Console.WriteLine("SocketException: {0}", e);
            // }
            // finally
            // {
            //     // Stop listening for new clients.
            //     server.Stop();
            // }
            //
            // Console.WriteLine("\nHit enter to continue...");
            // Console.Read();
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                int port = 3001;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                byte[] bytes = new byte[256];
                string data;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}