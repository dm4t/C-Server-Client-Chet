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
using System.Threading;
using System.Net.Sockets;



//Martin Vasilov 101981 
//Nenad Hristov 102046
namespace ClientT
{
    public partial class Client : Form
    {
        static IPAddress ipaddrs = IPAddress.Parse("127.0.0.1");
       // static IPAddress ipaddrs = IPAddress.Parse("192.168.1.133");
        static int port = 10001;
        static IPEndPoint endPoint = new IPEndPoint(ipaddrs, port);
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        bool up = true;
        string message;
        byte[] byteMessage = new byte[1024];
        byte[] msg;
        int recMessage;
        bool connect = true;
        string username;
        

        public Client()
        {
            InitializeComponent();

            
            try
            {
                Thread Chet = new Thread(Server);
                Chet.Start();
            }
            catch { }
            
        }

        public void Server()
        {

            if (connect == true)
            {
                s.Connect(endPoint);
                connect = false;
                MessageBox.Show("Connected");

            }


            try
            {
                while (up)
                {
                    recMessage = s.Receive(byteMessage);
                    message = Encoding.UTF8.GetString(byteMessage, 0, recMessage);
                    if (message != "")
                    {
                        textBox2.Invoke(new Action(() => textBox2.Text += message + "\r\n"));

                       // textBox2.Text += message + "\r\n";
                    }
                   // MessageBox.Show("Client");
                }
            }
            catch
            {
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {        
                message = username + " :" + textBox1.Text;
                msg = Encoding.UTF8.GetBytes(message);
                int mes = s.Send(msg);
                textBox2.Text += message + "\r\n";
                //  MessageBox.Show("Client send"); 
                textBox1.Text = "";
        }

        private void Client_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            username = textBox3.Text;
            
        }

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            s.Close();
            Environment.Exit(Environment.ExitCode);
            
        }
    }
}
