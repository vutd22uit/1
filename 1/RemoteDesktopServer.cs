using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class RemoteDesktopServer
{
    private TcpListener listener;
    private Thread listenerThread;

    public RemoteDesktopServer(string ipAddress, int port)
    {
        listener = new TcpListener(IPAddress.Parse(ipAddress), port);
        listenerThread = new Thread(new ThreadStart(ListenForClients));
        listenerThread.Start();
        Console.WriteLine("Server started.");
    }

    private void ListenForClients()
    {
        listener.Start();

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected.");
            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
            clientThread.Start(client);
        }
    }

    private void HandleClientComm(object client_obj)
    {
        TcpClient client = (TcpClient)client_obj;
        NetworkStream clientStream = client.GetStream();

        while (true)
        {
            try
            {
                Bitmap screenshot = CaptureScreen();
                MemoryStream ms = new MemoryStream();
                screenshot.Save(ms, ImageFormat.Png);
                byte[] buffer = ms.ToArray();

                byte[] dataLength = BitConverter.GetBytes(buffer.Length);
                clientStream.Write(dataLength, 0, dataLength.Length);

                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();
                Console.WriteLine("Screenshot sent to client.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client disconnected. " + ex.Message);
                client.Close();
                break;
            }
        }
    }

    private Bitmap CaptureScreen()
    {
        Bitmap screenshot = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,
                                       System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
        Graphics gfx = Graphics.FromImage(screenshot);
        gfx.CopyFromScreen(0, 0, 0, 0, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
        return screenshot;
    }
}
