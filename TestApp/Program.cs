﻿using MessagePack;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;

namespace TestApp
{
    internal class DebugServer
    {
        private TcpListener mlistener = null;

        private List<ClientWrapper> mClientList = new List<ClientWrapper>();

        private IPEndPoint mLocalEndPoint;

        private RingBuffer<string> mPool;

        public DebugServer()
        {
            mPool = new RingBuffer<string>(20);
        }

        public void Start(string strIpAddr, int port)
        {
            IPAddress ipAdder = IPAddress.Parse(strIpAddr);

            mLocalEndPoint = new IPEndPoint(ipAdder, port);

            Thread t = new Thread(Listening);

            t.Start();

            Console.WriteLine("Start end");
        }

        public void Stop()
        {
            mlistener.Stop();
        }

        public void Write(string s)
        {
            mPool.Add(s);

            lock (mClientList)
            {
                foreach (ClientWrapper client in mClientList)
                {
                    if (client.Connected)
                    {
                        client.Write(s);
                    }
                }

                RemoveDisconnectedClient();
            }
        }

        public void WriteLn(string s)
        {
            mPool.Add(s + "\n");

            lock (mClientList)
            {
                foreach (ClientWrapper client in mClientList)
                {
                    if (client.Connected)
                    {
                        client.WriteLn(s);
                    }
                }

                RemoveDisconnectedClient();
            }
        }

        private void RemoveDisconnectedClient()
        {
            lock (mClientList)
            {
                mClientList.RemoveAll((item)=>
                {
                    if (!item.Connected)
                    {
                        item.Dispose();
                        return true;
                    }

                    return false;
                });
            }
        }

        public void Listening()
        {
            mlistener = new TcpListener(mLocalEndPoint);

            Console.WriteLine("StartListening tid:" + Thread.CurrentThread.ManagedThreadId);

            try
            {
                mlistener.Start();

                while (true)
                {
                    var tcpClient = mlistener.AcceptTcpClient();

                    Console.WriteLine("Accept client");

                    ClientWrapper client = new ClientWrapper(tcpClient);

                    SendPoolToClient(client);

                    lock (mClientList)
                    {
                        mClientList.Add(client);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException " + e);
            }
            catch
            {

            }
        }

        private void SendPoolToClient(ClientWrapper client)
        {
            for (int i = 0; i < mPool.Count; i++)
            {
                client.Write(mPool[i]);
            }
        }

        private class ClientWrapper
        {
            TcpClient mClient;
            NetworkStream mStream;
            StreamWriter mWriter;

            public ClientWrapper(TcpClient client)
            {
                mClient = client;
                mStream = client.GetStream();
                mWriter = new StreamWriter(mStream, Encoding.UTF8);
            }

            public bool Connected
            {
                get => mClient.Connected;
            }

            public void WriteLn(string s)
            {
                if (!mClient.Connected)
                {
                    return;
                }
                
                try
                {
                    mWriter.WriteLine(s);
                    mWriter.Flush();
                }
                catch
                {
                    Close();
                }
            }


            public void Write(string s)
            {
                if (!mClient.Connected)
                {
                    return;
                }

                try
                {
                    mWriter.Write(s);
                    mWriter.Flush();
                }
                catch
                {
                    Close();
                }
            }

            public void Close()
            {
                mClient.Close();
                mStream.Close();
                mWriter.Close();
            }

            public void Dispose()
            {
                mClient.Dispose();
                mStream.Dispose();
                mWriter.Dispose();
            }
        }

        public class RingBuffer<T>
        {
            private T[] Data;

            private int Top = 0;

            private int Bottom = 0;

            private int Mask;

            public T this[int i] => Data[(i + Top) & Mask];

            public int Count
            {
                get;
                private set;
            }

            public int BufferSize
            {
                get;
                private set;
            }

            public RingBuffer(int size)
            {
                CreateBuffer(size);
            }

            public void CreateBuffer(int size)
            {
                BufferSize = Pow2((uint)size);
                Data = new T[BufferSize];
                Mask = BufferSize - 1;
            }

            public void Clear()
            {
                Top = 0;
                Bottom = 0;
                Count = 0;
            }

            static int Pow2(uint n)
            {
                --n;
                int p = 0;
                for (; n != 0; n >>= 1)
                {
                    p = (p << 1) + 1;
                }

                return p + 1;
            }

            public void Add(T elem)
            {
                Data[Bottom] = elem;
                Bottom = (Bottom + 1) & Mask;

                if (Count < BufferSize)
                {
                    Count++;
                }
                else
                {
                    Top = (Top + 1) & Mask;
                }
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            DebugServer server = new DebugServer();

            server.Start("127.0.0.1", 2300);

            Console.ReadLine();

            int cnt = 0;

            server.Write("123");
            server.Write("456");
            server.Write("789\n");

            while (true)
            {
                string s = "Test " + cnt++;

                Console.WriteLine("write " + s);

                server.WriteLn(s);
                Thread.Sleep(1000);
            }
        }
    }
}
