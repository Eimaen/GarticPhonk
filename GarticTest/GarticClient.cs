using GarticTest.Model;
using GarticTest.Model.Enums;
using GarticTest.Model.Messages.Client;
using GarticTest.Model.Messages.Server;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest
{
    internal class GarticClient
    {
        public delegate void MessageEventHandler(GarticClient client, GarticMessage message);
        public event MessageEventHandler OnMessage;

        public delegate void GamemodeEventHandler(GarticClient client, GarticGamemode gamemode);
        public event GamemodeEventHandler OnGamemodeSwitch;

        public delegate void GameTurnEventHandler(GarticClient client, TurnType turnType, int previousUserId, string previousTurnData);
        public event GameTurnEventHandler OnGameTurn;

        public delegate void PlayerJoinEventHandler(GarticClient client, int id, string nickname, string avatar);
        public event PlayerJoinEventHandler OnPlayerJoin;

        public delegate void DisconnectEventHandler(GarticClient client, int reason);
        public event DisconnectEventHandler OnDisconnect;

        public GarticSocket Socket { get; private set; }
        public GarticDrawController Draw { get; private set; }

        public GarticClient() { }

        public GarticClient(string nickname, string avatar, string roomId)
        {
            Login(nickname, avatar, roomId);
        }

        public async void Login(string nickname, string avatar, string roomId)
        {
            Socket = new GarticSocket();
            await Socket.Connect("en", nickname, avatar, roomId);
            Socket.OnMessage += OnSocketMessage;
            Draw = new GarticDrawController(Socket);
        }

        public void SendTextInput(string sentence)
        {
            Socket.SendString(new GarticTextInputMessage(sentence).Serialize());
        }

        public void SendSubmit()
        {
            Socket.SendString(new GarticClientSubmitMessage().Serialize());
        }

        private void OnSocketMessage(GarticMessage message)
        {
            OnMessage?.Invoke(this, message);

            if (message.Type == GarticMessageType.GamemodeSwitch)
            {
                var castMessage = message as GarticGamemodeSwitchMessage;
                OnGamemodeSwitch?.Invoke(this, castMessage.Gamemode);
            }

            if (message.Type == GarticMessageType.GameTurn)
            {
                var castMessage = message as GarticGameTurnMessage;
                OnGameTurn?.Invoke(this, castMessage.TurnType, castMessage.PreviousUserId, castMessage.PreviousTurnData);
            }

            if (message.Type == GarticMessageType.PlayerJoin)
            {
                var castMessage = message as GarticPlayerJoinMessage;
                OnPlayerJoin?.Invoke(this, castMessage.Id, castMessage.Nickname, castMessage.Avatar);
            }

            if (message.Type == GarticMessageType.Disconnect)
            {
                var castMessage = message as GarticDisconnectMessage;
                OnDisconnect?.Invoke(this, castMessage.Reason);
            }
        }
    }
}
