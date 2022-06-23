using GarticTest.Model;
using GarticTest.Model.Messages.Client;
using GarticTest.Model.Messages.Server;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
            GarticClient client = new GarticClient("GarticPhonk", "22", "");
            client.OnGameTurn += GameTurn;
            while (true) Thread.Sleep(1000);
        }

        private static void GameTurn(GarticClient client, Model.Enums.TurnType turnType, int previousUserId, string previousTurnData)
        {
            if (turnType == Model.Enums.TurnType.Sentence)
            {
                client.SendTextInput(Console.ReadLine());
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
                        client.Draw.DrawImage(openFileDialog.FileName, 2);
                    client.SendSubmit();
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }
        }
    }
}
