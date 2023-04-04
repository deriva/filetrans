﻿using Bc.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bc.PublishWF.Biz
{
    /// <summary>
    /// Socket.Fileengine            
    /// Recvfile => filerecv()             
    /// Sendfile => filesend()              
    /// 测试纪念：local  3.00GB文件 1:49.46s             
    /// </summary>
    public class Fileengine
    {

        //---------------------show process freq
        int procm = 250;

        //------------------------------------------------------package size
        int packagesize = 4 * 1024;
        public Fileengine()
        {
        }
        public void filesend(string path, System.Net.Sockets.Socket socket)
        {
            long filelength = 0;
            long sendseek = 0;

            int paklen = 0;
            int readlen = 0;
            int buffersize = packagesize;
            int done = 0;
            int leng = 0;

            byte[] buffer = new byte[buffersize];
            byte[] namebuf = new byte[1024];
            byte[] recvbuf = new byte[1024];

            string namestr = String.Empty;
            string filename = String.Empty;

            FileStream fs = new FileStream(path, FileMode.Open);

            filelength = fs.Length;
            filename = fs.Name.Substring(fs.Name.LastIndexOf("\\") + 1);

            Console.WriteLine("File   => " + filename);
            Console.WriteLine("Length => " + filelength.ToString());

            Console.WriteLine("");
            Writeyel("Starting");

            while (done == 0)
            {
                namestr = filename + @"@#@" + filelength.ToString() + @"@#@";//--------------------sndfmat

                namebuf = Encoding.UTF8.GetBytes(namestr);
                socket.Send(namebuf, namebuf.Length, SocketFlags.None);

                Console.WriteLine("");
                Console.Write("等待对方确认 ");
                Thread wat = new Thread(waitingth);
                wat.Start();
                try
                {
                    leng = socket.Receive(recvbuf);
                }
                catch (Exception)
                {
                    wat.Abort();
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Writered("Connection Failed");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    while (true) { }
                }
                wat.Abort();
                Console.WriteLine("");
                Console.WriteLine("");
                //		if(leng==namebuf.Length&&recvbuf.Equals(namebuf))
                //		{
                done = 1;
                //		}
            }

            namebuf = Encoding.UTF8.GetBytes("start");
            socket.Send(namebuf, namebuf.Length, SocketFlags.None);

            Console.WriteLine("");

            int procn = 0;
            while (sendseek < filelength)
            {
                readlen = fs.Read(buffer, 0, buffersize);
                try
                {
                    socket.Send(buffer, readlen, SocketFlags.None);
                }
                catch (Exception)
                {
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Writered("Connection Failed");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    while (true) { }
                }
                sendseek += readlen;
                procn++;
                if (procn >= procm)
                {
                    procn = 0;
                    showproc("Process.send -> [", (float)sendseek / filelength);
                }
            }


            Console.WriteLine("\n debuginfo   " + sendseek.ToString() + " | " + filelength.ToString() + " | " + readlen.ToString() + " | ");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write("\a");
            Writeyel("---Send done---");





        }
        public void filerecv(string path, System.Net.Sockets.Socket socket)
        {
            long filelength = 0;
            long recvseek = 0;

            int paklen = 0;
            int buffersize = packagesize;
            int done = 0;
            int n = 0;

            byte[] buffer = new byte[buffersize];
            byte[] namebuf = new byte[1024];
            byte[] recvbuf = new byte[1024];

            string namestr = String.Empty;
            string filename = String.Empty;
            string pathstr = String.Empty;
            string[] namestack = null;

            FileStream fs = null;


            Console.WriteLine("");
            Console.Write("等待对方发送");

            Thread wat = new Thread(waitingth);
            wat.Start();

            done = 0;
            while (done == 0)
            {
                try
                {
                    n = socket.Receive(namebuf);
                }
                catch (Exception)
                {
                    wat.Abort();
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Writered("Connection Failed");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    while (true) { }
                }
                wat.Abort();
                namestr = Encoding.UTF8.GetString(namebuf, 0, n);
                namestack = namestr.Split(@"@#@".ToCharArray());
                if (namestack[0] == "start")
                {
                    done = 1;
                }
                if (done == 0)
                {
                    filelength = long.Parse(namestack[3]);
                    Console.WriteLine("");
                    Console.WriteLine("File   => " + namestack[0]);
                    Console.WriteLine("Length => " + filelength.ToString());
                    Console.WriteLine("");

                    //--------------------------------如果要选储存路径 写在这里
                    Console.WriteLine("你想把文件放在哪里?  -输入d转义为桌面");
                    Console.WriteLine("                   -直接Enter转义为程序所在文件夹");
                    Console.WriteLine("                   -示例 D:\\abc\\");
                    done = 0;

                    while (done == 0)
                    {
                        Console.Write("DIRECTPATH =>");
                        pathstr = Console.ReadLine();
                        if (pathstr == "d")
                        {
                            pathstr = @"C:\Users\Administrator\Desktop\";
                        }
                        done = 1;

                        if (pathstr != String.Empty)
                        {
                            if (pathstr.Substring(1, 2) != ":\\")
                            {
                                done = 0;
                                Writered("--路径格式错误  示例 D:\\abc\\  --\n");
                            }
                            try
                            {
                                Directory.Exists(pathstr);
                            }
                            catch (Exception)
                            {
                                done = 0;
                                Writered("--路径格式错误  示例 D:\\abc\\  --\n");
                            }
                            if (File.Exists(pathstr + namestack[0]) == true)
                            {
                                done = 0;
                                Writered("--文件已存在--\n");

                            }
                        }
                    }

                    done = 0;
                    //			if(File.Exists(@"C:\Users\Administrator\Desktop\"+namestack[0]))
                    //			{


                    //-------------------------------------------如果确认文件重复，写在这里
                    //				File.Delete(@"C:\Users\Administrator\Desktop\"+namestack[0]);
                    //		    }

                    fs = File.OpenWrite(pathstr + namestack[0]);

                    namestr = filename + @"@#@" + filelength.ToString() + @"@#@";//--------------------sndfmat

                    namebuf = Encoding.UTF8.GetBytes(namestr);
                    socket.Send(namebuf, namebuf.Length, SocketFlags.None);

                    //	socket.Send(namebuf,n,SocketFlags.None);
                }

            }
            done = 0;
            int procn = 0;
            Console.WriteLine("");
            while (recvseek < filelength)
            {
                paklen = socket.Receive(buffer, buffersize, SocketFlags.None);
                fs.Position = fs.Length;
                fs.Write(buffer, 0, paklen);
                recvseek += paklen;
                procn++;
                if (procn >= procm)
                {
                    procn = 0;
                    showproc("Process.recv -> [", (float)recvseek / filelength);
                }

            }


            //------------------------------------------------------------------------这边有个下下之策 等待修复
            fs.Close();
            FileStream addon = File.OpenWrite(pathstr + namestack[0]);
            addon.Write(buffer, 0, paklen);
            addon.Close();


            //---------------------------------------------------------------------------
            Console.WriteLine("\n debuginfo   " + recvseek.ToString() + " | " + filelength.ToString() + " | " + paklen.ToString() + " | ");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write("\a");
            Writeyel("---recv done---");
        }

        void waitingth()
        {
            while (true)
            {
                Console.Write(".");
                Thread.Sleep(200);
                Console.Write(".");
                Thread.Sleep(200);
                Console.Write(".");
                Thread.Sleep(200);
                Console.Write(".");
                Thread.Sleep(200);
                Console.Write(".");
                Thread.Sleep(200);
                Console.Write(".");
                Thread.Sleep(200);

                Console.Write('\u0008');
                Console.Write('\u0008');
                Console.Write('\u0008');
                Console.Write('\u0008');
                Console.Write('\u0008');
                Console.Write('\u0008');
                Console.Write(" ");
                Console.Write(" ");
                Console.Write(" ");
                Console.Write(" ");
                Console.Write(" ");
                Console.Write(" ");
                Console.Write('\u0008');
                Console.Write('\u0008');
                Console.Write('\u0008');
                Console.Write('\u0008');
                Console.Write('\u0008');
                Console.Write('\u0008');

            }
        }
        void Writeyel(string str)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(str);
            Console.ForegroundColor = ConsoleColor.White;
        }
        void Writered(string str)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(str);
            Console.ForegroundColor = ConsoleColor.White;
        }
        void showproc(string headstr, float proc)
        {

            Console.Write("\r");
            Console.Write(headstr);
            for (int i = 1; i <= (10 - proc.ToString().Length); i++)
            {
                Console.Write(" ");
            }
            Console.Write(proc.ToString() + "%");
            Console.Write(" ]");

            /*		for (int i=1;i<=(10-proc.ToString().Length);i++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write(proc.ToString()+"%");
                    Console.Write(" ]");

                    Console.Write('\u0008');
                    Console.Write('\u0008');
                    Console.Write('\u0008');
                    Console.Write('\u0008');
                    Console.Write('\u0008');

                    Console.Write('\u0008');
                    Console.Write('\u0008');
                    Console.Write('\u0008');
                    Console.Write('\u0008');
                    Console.Write('\u0008');

                    Console.Write('\u0008');
                    Console.Write('\u0008');
                    Console.Write('\u0008');




                    Console.Write(" ");
                    Console.Write(" ");
                    Console.Write(" ");
                    Console.Write(" ");
                    Console.Write(" ");
                    Console.Write(" ");
                    Console.Write(" ");
                    Console.Write(" ");
                    Console.Write('\u0008');
                    Console.Write('\u0008');
                    Console.Write('\u0008');
                    Console.Write('\u0008');
                    Console.Write('\u0008');
                    Console.Write('\u0008');
                    Console.Write('\u0008');
                    Console.Write('\u0008');*/

        }
    }


    public class FileDbConfig
    {
        static ListBox plstbox;
        private static Socket serverSocket;
        public void StartRecieve(string path, ListBox listbox)
        {
            plstbox = listbox;
            //服务器IP地址
            var serverip = "127.0.0.1";// ConfigUtils.GetValue<string>("serverip").Split(':');
            var myProt = 17589;// serverip[1].ToInt2();
            string ipstr = "127.0.0.1";// serverip[0];
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, myProt);
            serverSocket = new Socket(localEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(localEP);
            //绑定IP地址：端口
            serverSocket.Listen(10);    //设定最多10个排队连接请求
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
          
        }
        private static void ListenClientConnect()
        {
            while (true)
            {
                if (serverSocket != null)
                {
                    try
                    {
                        new Fileengine().filerecv("D:\\发布\\as.bak", serverSocket);
                    }
                    catch
                    {
                        break;
                    }
                }
            }
        }
     

        public void StartSend(string path,string IP, int Port, ListBox listbox)
        {
            plstbox = listbox;
            //服务器IP地址
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(IP), Port);

            //创建套接字
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //连接到发送端
            try
            {
                client.Connect(ipep);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine("连接服务器失败！");
              
            }
            //获得客户端节点对象
            IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;

            new Fileengine().filesend(path, client);
        }
    }
}
