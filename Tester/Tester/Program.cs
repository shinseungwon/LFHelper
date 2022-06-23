﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HelperDotNet;
using Microsoft.Office.Interop.Excel;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(File.ReadAllText(@"\\172.22.13.33\DTS\ExceedToDTS\hnm05-kr\WMSOSTS\Archive\WMSSHP_I216_LF03_00547746_000_20220524001332263.txt"));

            //If Left(Value, 1) = "1" or Left(Value, 1) = "2" or Left(Value, 1) = "3" 
            //    or Left(Value, 1) = "4" Then Value = "★" Else Value = "♥" End If

            //string name = "이 민";
            ////string name = "신 승원";

            //string Value = "";

            //if(Value.Length == 2)
            //{
            //    Value = name.Substring(0, Value.Length - 1) + "*";
            //}
            //else if (Value.Length >= 3)
            //{
            //    Value = name.Substring(0, Value.Length - 1) + "*";
            //}
            //else
            //{
            //    Value = name;
            //}


            //If Len(Value) = 2 Then Value = Left(Value, Len(Value) - 1) + "*"
            //Else If Len(Value) >= 3 Then Left(Value, Len(Value - 2) + "*" + Right(Value, 1)
            //End If


            //AsynchronousSocketListener.StartListening();            

            ////Connect DB
            //string connectionString = "Data Source=" + "172.22.8.143" + ",1433; Initial Catalog=" + "KRWMS"
            //    + "; User id=" + "superuser" + "; Password=" + "superuser" + ";";

            //DbHelper dh = new DbHelper(connectionString);
            //Logger l = new Logger(Directory.GetCurrentDirectory() + @"\Logger");
            //dh.SetLogger(l);

            //Connect DB
            string connectionString = "Data Source=" + "172.22.17.155" + ",1433; Initial Catalog="
                + "KRWMS" + "; Integrated Security=SSPI;";

            DbHelper dh = new DbHelper(connectionString);
            dh.throwError = true;
            Logger l = new Logger(Directory.GetCurrentDirectory() + @"\Logger");
            dh.SetLogger(l);

            string query = "select top 100 * from krwms..orders";
            DataSet ds = new DataSet();
            dh.CallQuery(query, ref ds);

            foreach (DataRow r in ds.Tables[0].Rows)
            {
                foreach (DataColumn c in ds.Tables[0].Columns)
                {
                    Console.Write(r[c] + " ");
                }
                Console.WriteLine();
            }

            //string query = "select wsdata from krarchive..wsoutbound_log(nolock) where seqno in ( '257018156', '257025373')";
            //DataSet ds = new DataSet();
            //dh.CallQuery(query, ref ds);

            //오더정보
            //주소 가져오기
            //주소 CJ직접 정제
            //되면 TLOG 아카이빙 확인
            //면 복구
            //0으로 리트리거
            //awaiting
            //9되면 찍어주고
            //돌면서 오더에 m_zip 확인

            //테스팅


        }

        // State object for reading client data asynchronously  
        public class StateObject
        {
            // Size of receive buffer.  
            public const int BufferSize = 1024;

            // Receive buffer.  
            public byte[] buffer = new byte[BufferSize];

            // Received data string.
            public StringBuilder sb = new StringBuilder();

            // Client socket.
            public Socket workSocket = null;
        }

        public class AsynchronousSocketListener
        {
            // Thread signal.  
            public static ManualResetEvent allDone = new ManualResetEvent(false);

            public AsynchronousSocketListener()
            {
            }

            public static void StartListening()
            {
                // Establish the local endpoint for the socket.  
                // The DNS name of the computer  
                // running the listener is "host.contoso.com".  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP socket.  
                Socket listener = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and listen for incoming connections.  
                try
                {
                    listener.Bind(localEndPoint);
                    listener.Listen(100);

                    while (true)
                    {
                        // Set the event to nonsignaled state.  
                        allDone.Reset();

                        // Start an asynchronous socket to listen for connections.  
                        Console.WriteLine("Waiting for a connection...");
                        listener.BeginAccept(
                            new AsyncCallback(AcceptCallback),
                            listener);

                        // Wait until a connection is made before continuing.  
                        allDone.WaitOne();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Console.WriteLine("\nPress ENTER to continue...");
                Console.Read();

            }

            public static void AcceptCallback(IAsyncResult ar)
            {
                // Signal the main thread to continue.  
                allDone.Set();

                // Get the socket that handles the client request.  
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
            }

            public static void ReadCallback(IAsyncResult ar)
            {
                String content = String.Empty;

                // Retrieve the state object and the handler socket  
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                // Read data from the client socket.
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read
                    // more data.  
                    content = state.sb.ToString();
                    if (content.IndexOf("<EOF>") > -1)
                    {
                        // All the data has been read from the
                        // client. Display it on the console.  
                        Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                            content.Length, content);
                        // Echo the data back to the client.  
                        Thread.Sleep(100000);
                        Send(handler, content);
                    }
                    else
                    {
                        // Not all data received. Get more.  
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                    }
                }
            }

            private static void Send(Socket handler, String data)
            {
                // Convert the string data to byte data using ASCII encoding.  
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Begin sending the data to the remote device.  
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);
            }

            private static void SendCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the socket from the state object.  
                    Socket handler = (Socket)ar.AsyncState;

                    // Complete sending the data to the remote device.  
                    int bytesSent = handler.EndSend(ar);
                    Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}