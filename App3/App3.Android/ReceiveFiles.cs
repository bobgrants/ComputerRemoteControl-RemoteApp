using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App3.Droid
{
    class ReceiveFiles
    {
        //public ArrayList nSockets = new ArrayList();

        //MainPage mp;
        public TcpListener tcpListener;
        NetworkStream networkStream;
        

        

        public void listenerThread()
        {
            Console.WriteLine("THREAD START");

            tcpListener = new TcpListener(IPAddress.Any, 8000);
            tcpListener.Start();
            Console.WriteLine("LISTENER START");

            //tcpListener.AcceptSocket();

            networkStream = new NetworkStream(tcpListener.AcceptSocket());


            handlerThread();
            
        }

        public void handlerThread()
        {
            var documentsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString();
            var filePath = System.IO.Path.Combine(documentsPath, "testfile1");
            
            //Socket handlerSocket = (Socket)nSockets[nSockets.Count - 1];

            //NetworkStream networkStream = new NetworkStream(tcpListener.AcceptSocket());
            int thisRead = 0;
            int blockSize = 1024;
            Byte[] dataByte = new Byte[blockSize];

            lock (this)
            {
                // Only one process can access
                // the same file at any given time
                //Stream fileStream = File.OpenWrite("d:\\SubmittedFile.txt");
                //Stream fileStream = File.OpenWrite(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString());

                Stream fileStream = (System.IO.File.Create(filePath));
                //Console.WriteLine("FILENAME ? : " + networkStream.Read(dataByte, 0, blockSize));

                Console.WriteLine("dataByte : "  + dataByte);
                Console.WriteLine("blockSize : " + blockSize);
                Console.WriteLine("networkStream : " + networkStream);

                while (true)
                {
                    thisRead = networkStream.Read(dataByte, 0, blockSize);
                    fileStream.Write(dataByte, 0, thisRead);
                    if (thisRead == 0) break;
                }
                fileStream.Close();
                networkStream.Close();
                //handlerSocket.Close();
            }
            //handlerSocket = null;
            tcpListener.Stop();
        }


    }
}