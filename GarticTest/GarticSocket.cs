using GarticTest.Model;
using GarticTest.Model.Messages.Client;
using GarticTest.Model.Messages.Server;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarticTest
{
    internal class GarticSocket
    {
        public delegate void MessageEventHandler(GarticMessage message);
        public event MessageEventHandler OnMessage;

        ClientWebSocket ws;
        HttpClient http;
        Guid guid;

        Task pingLoop, receiveLoop, processQueue;
        LinkedList<string> wsQueue;

        public GarticSocket()
        {
            wsQueue = new LinkedList<string>();
            ws = new ClientWebSocket();
            http = new HttpClient(new HttpClientHandler() { UseCookies = true });
            http.DefaultRequestHeaders.Add("Origin", "https://garticphone.com");
            http.DefaultRequestHeaders.Add("Referer", "https://garticphone.com/");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:97.0) Gecko/20100101 Firefox/97.0");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            http.Timeout = TimeSpan.FromSeconds(10);
            guid = Guid.NewGuid();
        }

        public async Task Connect(string language, string username, string avatar, string roomId, string server = "")
        {
            if (server == string.Empty)
                server = await (await http.GetAsync($"https://garticphone.com/api/server?code={roomId}")).Content.ReadAsStringAsync();
            string jsonContent = await (await http.GetAsync($"{server}/socket.io/?EIO=3&transport=polling&t")).Content.ReadAsStringAsync();
            Console.WriteLine(jsonContent);
            string sid = new Regex("sid\":\"(.*?)\"").Match(jsonContent).Groups[1].Value;
            Console.WriteLine(sid);
            string reqString = $"42[1,\"{guid.ToString().ToLower()}\",\"{username}\",\"{avatar}\",\"{language}\",true,\"{roomId}\",null,null]";
            Console.WriteLine(await (await http.PostAsync($"{server}/socket.io/?EIO=3&transport=polling&sid={sid}", new StringContent(reqString.Length + ":" + reqString))).Content.ReadAsStringAsync());
            Console.WriteLine(await (await http.GetAsync($"{server}/socket.io/?EIO=3&transport=polling&sid={sid}")).Content.ReadAsStringAsync());
            await ws.ConnectAsync(new Uri($"{server.Replace("https", "wss")}/socket.io/?EIO=3&transport=websocket&sid={sid}"), CancellationToken.None);

            // Handshake
            SendString("2probe");
            SendString("5");
            await Ping();

            StartRoutine();
        }

        public async Task Disconnect()
        {
            SendString("42[2,28]");
            if (receiveLoop != null)
                receiveLoop.Dispose();
            if (pingLoop != null)
                pingLoop.Dispose();
            if (processQueue != null)
                processQueue.Dispose();
            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        }

        public async void ProcessQueue()
        {
            while (true)
            {
                while (wsQueue.Count > 0)
                {
                    ArraySegment<byte> buffer = new ArraySegment<byte>();
                    try
                    {
                        byte[] encoded;
                        lock (wsQueue)
                        {
                            encoded = Encoding.UTF8.GetBytes(wsQueue.First.Value);
                            wsQueue.RemoveFirst();
                        }
                        buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);
                    }
                    catch { }
                    if (buffer != null && buffer.Array != null && buffer.Any())
                        await ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        private void StartRoutine()
        {
            processQueue = Task.Run(ProcessQueue);
            pingLoop = Task.Run(PingLoop);
            receiveLoop = Task.Run(ReceiveLoop);
        }

        private void PingLoop()
        {
            while (true)
            {
                Thread.Sleep(10000);
                Ping().Wait();
            }
        }

        private async void ReceiveLoop()
        {
            while (ws.State == WebSocketState.Open)
            {
                string message = await ReadString();
                GarticMessage parsed = GarticMessage.Parse(message);

                if (parsed.Type == GarticMessageType.Unknown)
                    Console.WriteLine(message);
                else
                    Console.WriteLine(parsed.Type);

                OnMessage.Invoke(parsed);
            }
        }

        private async Task<bool> Ping()
        {
            SendString("2");
            return true; // Nah...
        }

        public void SendString(string data, bool important = false)
        {
            if (important)
                lock (wsQueue)
                    wsQueue.AddFirst(data);
            else lock (wsQueue)
                    wsQueue.AddLast(data);
        }

        public async Task<string> ReadString()
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);

            WebSocketReceiveResult result;

            using (var ms = new MemoryStream())
            {
                do
                {
                    result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(ms, Encoding.UTF8))
                    return reader.ReadToEnd();
            }
        }
    }
}
