using App3.Droid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace App3
{
    public partial class MainPage : ContentPage
    {

        int PORT_NO = 8000;
        string SERVER_IP = "192.168.0.14";

        string connectionStatus = "closed";

        int pingTimeout = 800;

        ReceiveFiles rf;
        ListAppToReceive la;

        public IPAddress localClientIP;



        public MainPage()
        {

            rf = new ReceiveFiles();
            la = new ListAppToReceive();

            InitializeComponent();

            VolumeDown_Image.IsVisible = false;

            AutoConnect();


            IpAddressLabel();


        }

        private void IpAddressLabel()
        {

            IPHostEntry host;

            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IpLabel.Text = ip.ToString();
                    localClientIP = ip;
                }
            }
        }

        void AutoConnect()
        {
            if (Application.Current.Properties.ContainsKey("SERVER_IP_SAVED"))
            {
                entryField.Text = Application.Current.Properties["SERVER_IP_SAVED"].ToString();

                PingCheckAlive(entryField.Text);
            }
            else
            {
                EnableButtons();
            }
        }

        private void ConnectBtn_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("entryField.Text : " + entryField.Text);
            SERVER_IP = entryField.Text;
            ButtonConnection(entryField.Text);
        }

        public void EnableButtons()
        {
            if (connectionStatus == "closed")
            {
                VolumeDown_Image.IsVisible = false;
                VolumeDown_Button.IsEnabled = false;
                VolumeDown_Button.IsVisible = false;

                VolumeUp_Image.IsVisible = false;
                VolumeUp_Button.IsEnabled = false;
                VolumeUp_Button.IsVisible = false;

                PlayPause_Image.IsVisible = false;
                PlayPause_Button.IsEnabled = false;
                PlayPause_Button.IsVisible = false;

                Up_Image.IsVisible = false;
                Up_Button.IsEnabled = false;
                Up_Button.IsVisible = false;

                Down_Image.IsVisible = false;
                Down_Button.IsEnabled = false;
                Down_Button.IsVisible = false;

                Left_Image.IsVisible = false;
                Left_Button.IsEnabled = false;
                Left_Button.IsVisible = false;

                Enter_Image.IsVisible = false;
                Enter_Button.IsEnabled = false;
                Enter_Button.IsVisible = false;

                Right_Image.IsVisible = false;
                Right_Button.IsEnabled = false;
                Right_Button.IsVisible = false;

                Retour_Image.IsVisible = false;
                Retour_Button.IsEnabled = false;
                Retour_Button.IsVisible = false;


                grid2.IsEnabled = false;
                grid2.IsVisible = false;

                entryField.IsEnabled = true;
                entryField.IsVisible = true;
                connectBtn.IsEnabled = true;
                connectBtn.IsVisible = true;
                row_entryField.Height = 40;
                connectBtn.Text = "CONNECT";
            }

            else if (connectionStatus == "open")

            {
                VolumeDown_Image.IsVisible = true;
                VolumeDown_Button.IsEnabled = true;
                VolumeDown_Button.IsVisible = true;

                VolumeUp_Image.IsVisible = true;
                VolumeUp_Button.IsEnabled = true;
                VolumeUp_Button.IsVisible = true;

                PlayPause_Image.IsVisible = true;
                PlayPause_Button.IsEnabled = true;
                PlayPause_Button.IsVisible = true;

                Up_Image.IsVisible = true;
                Up_Button.IsEnabled = true;
                Up_Button.IsVisible = true;

                Down_Image.IsVisible = true;
                Down_Button.IsEnabled = true;
                Down_Button.IsVisible = true;

                Left_Image.IsVisible = true;
                Left_Button.IsEnabled = true;
                Left_Button.IsVisible = true;

                Enter_Image.IsVisible = true;
                Enter_Button.IsEnabled = true;
                Enter_Button.IsVisible = true;

                Right_Image.IsVisible = true;
                Right_Button.IsEnabled = true;
                Right_Button.IsVisible = true;

                Retour_Image.IsVisible = true;
                Retour_Button.IsEnabled = true;
                Retour_Button.IsVisible = true;

                grid2.IsEnabled = true;
                grid2.IsVisible = true;


                entryField.IsEnabled = false;
                entryField.IsVisible = false;
                connectBtn.IsEnabled = false;
                connectBtn.IsVisible = false;
                row_entryField.Height = 1;
                connectBtn.Text = "";
            }
        }

        void PingCheckAlive(string IP)
        {
            var client = new TcpClient();
            IP = Application.Current.Properties["SERVER_IP_SAVED"].ToString();

            client.ConnectAsync(IP, PORT_NO);
            System.Threading.Thread.Sleep(pingTimeout);

            if (client.Connected == false)
            {
                connectionStatus = "closed";
                Console.WriteLine("Connection Failure - " + connectionStatus);
                EnableButtons();
                client.Close();
                return;
            }
            else if (client.Connected == true)
            {
                connectionStatus = "open";
                Console.WriteLine("Connection Success - " + connectionStatus);
                EnableButtons();
                Application.Current.Properties["SERVER_IP_SAVED"] = IP;
                client.Close();
                return;
            }
        }

        void ButtonConnection(string IP)
        {
            var client = new TcpClient();

            client.ConnectAsync(IP, PORT_NO);
            System.Threading.Thread.Sleep(pingTimeout);

            if (client.Connected == false)
            {
                Application.Current.SavePropertiesAsync();
                connectionStatus = "closed";
                Console.WriteLine("Connection Failure - " + connectionStatus);
                EnableButtons();
                client.Close();
                return;
            }
            else if (client.Connected == true)
            {
                Application.Current.Properties["SERVER_IP_SAVED"] = IP;
                Application.Current.SavePropertiesAsync();
                connectionStatus = "open";
                Console.WriteLine("Connection Success - " + connectionStatus);
                EnableButtons();
                client.Close();
                return;
            }
        }


        private void VolumeDown_Clicked(object sender, EventArgs e)
        {

            try
            {
                string textToSend = "volumeDown";

                //---create a TCPClient object at the IP and port no.---
                var client = new TcpClient();

                client.ConnectAsync(SERVER_IP, PORT_NO);
                System.Threading.Thread.Sleep(100);

                if (client.Connected == false)
                {
                    connectionStatus = "closed";
                    Console.WriteLine("Connection Failure - " + connectionStatus);
                    EnableButtons();
                    client.Close();
                    return;
                }
                else if (client.Connected == true)
                {
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                    //---send the text---
                    Console.WriteLine("Sending : " + textToSend);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                    //---read back the text---
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                    Console.ReadLine();
                    client.Close();
                }
                
            }

            catch (Exception e1)
            {
                Console.WriteLine("ERROR : " + e1.ToString());
            }
        }

        private void VolumeUp_Clicked(object sender, EventArgs e)
        {
            try
            {
                string textToSend = "volumeUp";

                //---create a TCPClient object at the IP and port no.---
                var client = new TcpClient();

                client.ConnectAsync(SERVER_IP, PORT_NO);
                System.Threading.Thread.Sleep(100);

                if (client.Connected == false)
                {
                    connectionStatus = "closed";
                    Console.WriteLine("Connection Failure - " + connectionStatus);
                    client.Close();
                    EnableButtons();
                    return;
                }
                else if (client.Connected == true)
                {
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                    //---send the text---
                    Console.WriteLine("Sending : " + textToSend);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                    //---read back the text---
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                    Console.ReadLine();
                    client.Close();
                }
            }

            catch (Exception e1)
            {
                Console.WriteLine("ERROR : " + e1.ToString());
            }
        }

        private void PlayPause_Clicked(object sender, EventArgs e)
        {
            try
            {
                string textToSend = "playPause";

                //---create a TCPClient object at the IP and port no.---
                var client = new TcpClient();

                client.ConnectAsync(SERVER_IP, PORT_NO);
                System.Threading.Thread.Sleep(100);

                if (client.Connected == false)
                {
                    connectionStatus = "closed";
                    Console.WriteLine("Connection Failure - " + connectionStatus);
                    client.Close();
                    EnableButtons();
                    return;
                }
                else if (client.Connected == true)
                {
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                    //---send the text---
                    Console.WriteLine("Sending : " + textToSend);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                    //---read back the text---
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                    Console.ReadLine();
                    client.Close();
                }
            }

            catch (Exception e1)
            {
                Console.WriteLine("ERROR : " + e1.ToString());
            }
        }

        private void Up_Clicked(object sender, EventArgs e)
        {
            try
            {
                string textToSend = "up";

                //---create a TCPClient object at the IP and port no.---
                var client = new TcpClient();

                client.ConnectAsync(SERVER_IP, PORT_NO);
                System.Threading.Thread.Sleep(100);

                if (client.Connected == false)
                {
                    connectionStatus = "closed";
                    Console.WriteLine("Connection Failure - " + connectionStatus);
                    client.Close();
                    EnableButtons();
                    return;
                }
                else if (client.Connected == true)
                {
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                    //---send the text---
                    Console.WriteLine("Sending : " + textToSend);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                    //---read back the text---
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                    Console.ReadLine();
                    client.Close();
                }
            }

            catch (Exception e1)
            {
                Console.WriteLine("ERROR : " + e1.ToString());
            }
        }


        private void Down_Clicked(object sender, EventArgs e)
        {
            try
            {
                string textToSend = "down";

                //---create a TCPClient object at the IP and port no.---
                var client = new TcpClient();

                client.ConnectAsync(SERVER_IP, PORT_NO);
                System.Threading.Thread.Sleep(100);

                if (client.Connected == false)
                {
                    connectionStatus = "closed";
                    Console.WriteLine("Connection Failure - " + connectionStatus);
                    client.Close();
                    EnableButtons();
                    return;
                }
                else if (client.Connected == true)
                {
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                    //---send the text---
                    Console.WriteLine("Sending : " + textToSend);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                    //---read back the text---
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                    Console.ReadLine();
                    client.Close();
                }
            }

            catch (Exception e1)
            {
                Console.WriteLine("ERROR : " + e1.ToString());
            }
        }

        private void Left_Clicked(object sender, EventArgs e)
        {
            try
            {
                string textToSend = "left";

                //---create a TCPClient object at the IP and port no.---
                var client = new TcpClient();

                client.ConnectAsync(SERVER_IP, PORT_NO);
                System.Threading.Thread.Sleep(100);

                if (client.Connected == false)
                {
                    connectionStatus = "closed";
                    Console.WriteLine("Connection Failure - " + connectionStatus);
                    client.Close();
                    EnableButtons();
                    return;
                }
                else if (client.Connected == true)
                {
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                    //---send the text---
                    Console.WriteLine("Sending : " + textToSend);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                    //---read back the text---
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                    Console.ReadLine();
                    client.Close();
                }
            }

            catch (Exception e1)
            {
                Console.WriteLine("ERROR : " + e1.ToString());
            }
        }

        private void Enter_Clicked(object sender, EventArgs e)
        {
            try
            {
                string textToSend = "enter";

                //---create a TCPClient object at the IP and port no.---
                var client = new TcpClient();

                client.ConnectAsync(SERVER_IP, PORT_NO);
                System.Threading.Thread.Sleep(100);

                if (client.Connected == false)
                {
                    connectionStatus = "closed";
                    Console.WriteLine("Connection Failure - " + connectionStatus);
                    client.Close();
                    EnableButtons();
                    return;
                }
                else if (client.Connected == true)
                {
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                    //---send the text---
                    Console.WriteLine("Sending : " + textToSend);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                    //---read back the text---
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                    Console.ReadLine();
                    client.Close();
                }
            }

            catch (Exception e1)
            {
                Console.WriteLine("ERROR : " + e1.ToString());
            }
        }

        private void Right_Clicked(object sender, EventArgs e)
        {
            try
            {
                string textToSend = "right";

                //---create a TCPClient object at the IP and port no.---
                var client = new TcpClient();

                client.ConnectAsync(SERVER_IP, PORT_NO);
                System.Threading.Thread.Sleep(100);

                if (client.Connected == false)
                {
                    connectionStatus = "closed";
                    Console.WriteLine("Connection Failure - " + connectionStatus);
                    client.Close();
                    EnableButtons();
                    return;
                }
                else if (client.Connected == true)
                {
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                    //---send the text---
                    Console.WriteLine("Sending : " + textToSend);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                    //---read back the text---
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                    Console.ReadLine();
                    client.Close();
                }
            }

            catch (Exception e1)
            {
                Console.WriteLine("ERROR : " + e1.ToString());
            }
        }

        private void Retour_Clicked(object sender, EventArgs e)
        {
            try
            {
                string textToSend = "retour";

                //---create a TCPClient object at the IP and port no.---
                var client = new TcpClient();

                client.ConnectAsync(SERVER_IP, PORT_NO);
                System.Threading.Thread.Sleep(100);

                if (client.Connected == false)
                {
                    connectionStatus = "closed";
                    Console.WriteLine("Connection Failure - " + connectionStatus);
                    client.Close();
                    EnableButtons();
                    return;
                }
                else if (client.Connected == true)
                {
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                    //---send the text---
                    Console.WriteLine("Sending : " + textToSend);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                    //---read back the text---
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                    Console.ReadLine();
                    client.Close();
                }
            }

            catch (Exception e1)
            {
                Console.WriteLine("ERROR : " + e1.ToString());
            }
        }

        private void buttonTestMethod(object sender, EventArgs e)
        {
            
                string textToSend = "updateApps";

                //---create a TCPClient object at the IP and port no.---
                var client = new TcpClient();

            client.Connect(SERVER_IP, PORT_NO);

            if (client.Connected == false)
                {
                    connectionStatus = "closed";
                    Console.WriteLine("Connection Failure - " + connectionStatus);
                    client.Close();
                    EnableButtons();
                    return;
                }
                else if (client.Connected == true)
                {
                    //la.GetAppNumbers();

                NetworkStream nwStream = client.GetStream();
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                //---send the text---
                Console.WriteLine("Sending : " + textToSend);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                //la.GetAppNumbers();

                client.Close();
                new Task(la.GetAppNumbers).Start();

                }  


            //catch (Exception e1)
            //{
            //    Console.WriteLine("ERROR : " + e1.ToString());
            //}
        }
    }
}
