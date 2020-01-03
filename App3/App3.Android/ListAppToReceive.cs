using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App3.Droid
{
    class ListAppToReceive
    {
        public int NumberOfAppInt;
        public TcpListener listener;
        NetworkStream nwStream;


        public void GetAppNumbers()
        {
            //System.Threading.Thread.Sleep(100);
            Console.WriteLine("Listening...");

            //---listen ---

            listener = new TcpListener(IPAddress.Any, 8000);
            Console.WriteLine("GetAppNumbers Listening...");
            listener.Start();

            //---incoming client connected---
            TcpClient client = listener.AcceptTcpClient();

            //---get the incoming data through a network stream---
            nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];

            //---read incoming stream---
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

            //---convert the data received into a string---
            string NumberOfAppString = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("NumberOfAppString - Received : " + NumberOfAppString);

            Int32.TryParse(NumberOfAppString, out NumberOfAppInt);

            Console.WriteLine("NumberOfAppInt - Received : " + NumberOfAppInt);

            listener.Stop();
            client.Close();
        }
    }
}