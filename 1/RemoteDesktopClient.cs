using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace RemoteDesktopApp
{
    public class RemoteDesktopClient
    {
        private TcpClient client;
        private NetworkStream serverStream;
        private PictureBox pictureBox;

        public RemoteDesktopClient(string ipAddress, int port, PictureBox pictureBox)
        {
            client = new TcpClient(ipAddress, port);
            serverStream = client.GetStream();
            this.pictureBox = pictureBox;
            Thread clientThread = new Thread(new ThreadStart(ReceiveData));
            clientThread.Start();
            Console.WriteLine("Client started.");
        }

        private void ReceiveData()
        {
            while (true)
            {
                try
                {
                    byte[] dataLength = new byte[4];
                    serverStream.Read(dataLength, 0, 4);
                    int length = BitConverter.ToInt32(dataLength, 0);
                    Console.WriteLine("Data length received: " + length);

                    byte[] buffer = new byte[length];
                    int bytesRead = 0;
                    while (bytesRead < length)
                    {
                        bytesRead += serverStream.Read(buffer, bytesRead, length - bytesRead);
                    }
                    Console.WriteLine("Screenshot data received.");

                    MemoryStream ms = new MemoryStream(buffer);
                    Image screenshot = Image.FromStream(ms);
                    pictureBox.Invoke(new Action(() => pictureBox.Image = screenshot));
                    Console.WriteLine("Screenshot displayed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Server disconnected. " + ex.Message);
                    client.Close();
                    break;
                }
            }
        }
    }
}
