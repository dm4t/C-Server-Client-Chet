using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;


//Martin Vasilov 101981 
//Nenad Hristov 102046
namespace ServerT
{
    public partial class Form1 : Form
    {
        static IPAddress ipaddrs = IPAddress.Parse("127.0.0.1");
        //static IPAddress ipaddrs = IPAddress.Parse("192.168.1.133");
        static int port = 10001;
        static IPEndPoint endPoint = new IPEndPoint(ipaddrs, port);
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Socket temp;
        bool up = true;
        bool iff = true;
        string message;
        byte[] byteMessage = new byte[1024];
        int recMessage;
        string username;

        public Form1()
        {
            InitializeComponent();
            
            try
            {

                Thread Up = new Thread(new ParameterizedThreadStart(Server));
                Up.Start();
               
            }
            catch {
                MessageBox.Show("Error");
            }
        }


        public void Server(object socket)
        {

            if (iff == true)
            {
                s.Bind(endPoint);
                s.Listen(1);
                temp = s.Accept();
                iff = false;
                MessageBox.Show("Connected with client");


            }


           try
            {
               // MessageBox.Show("Good");
                while (up)
                {

                    recMessage = temp.Receive(byteMessage);
                    message = Encoding.UTF8.GetString(byteMessage, 0, recMessage);

                     textBox1.Invoke(new Action(() => textBox1.Text += message + "\r\n"));

                   // textBox1.Text += message + "\r\n";
                    
                    //MessageBox.Show("Server");
                }
           }
           catch
            {
                MessageBox.Show("Client is disconnected");
           }
        }


            private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            username = textBox3.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sendMessage = username + ": " + textBox2.Text;
            byte[] sendMsg = Encoding.UTF8.GetBytes(sendMessage);
            temp.Send(sendMsg);
            textBox1.Text += sendMessage + "\r\n";
            //  MessageBox.Show("Server send");
            textBox2.Text = "";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
           Environment.Exit(Environment.ExitCode);
        }
    }
}
