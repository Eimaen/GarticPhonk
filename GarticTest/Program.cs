using GarticTest.Model;
using GarticTest.Model.Messages.Client;
using GarticTest.Model.Messages.Server;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarticTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Console.Write("Nickname: ");
            string username = Console.ReadLine();
            Console.Write("Invite link: ");
            string code = Console.ReadLine().Replace("https://garticphone.com/ru/?c=", string.Empty);
            GarticClient client = new GarticClient(username, "5", code);
            client.OnGameTurn += GameTurn;
            while (true) Thread.Sleep(1000);
        }

        private static void GameTurn(GarticClient client, Model.Enums.TurnType turnType, int turnNumber, int previousUserId, object previousTurnData)
        {
            if (client.Players.ContainsKey(previousUserId))
                Console.WriteLine($"Previous turn was made by: {client.Players[previousUserId].Nick}");

            if (turnType == Model.Enums.TurnType.Sentence)
            {
                Console.Write("Come up with a sentence for others to draw: ");
                client.SendSentence(Console.ReadLine());
                client.SendSubmit();
            }

            if (turnType == Model.Enums.TurnType.Draw)
            {
                Console.WriteLine($"Draw: {previousTurnData}");
                Thread thread = new Thread(() =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Images|*.jpg;*.jpeg;*.png";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                        client.Draw.DrawImage(openFileDialog.FileName, 1);
                    client.SendSubmit();
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }

            if (turnType == Model.Enums.TurnType.Guess)
            {
                Console.Write("Guess what's drawn: ");
                client.SendGuess(Console.ReadLine());
                client.SendSubmit();
            }
        }
    }
}
