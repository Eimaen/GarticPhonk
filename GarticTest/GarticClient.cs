using GarticTest.Model;
using GarticTest.Model.Enums;
using GarticTest.Model.Messages.Client;
using GarticTest.Model.Messages.Server;
using GarticTest.Model.Types;
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

        public delegate void GameTurnEventHandler(GarticClient client, TurnType turnType, int turnNumber, int previousUserId, object previousTurnData);
        public event GameTurnEventHandler OnGameTurn;

        public delegate void PlayerJoinEventHandler(GarticClient client, int id, string nickname, string avatar);
        public event PlayerJoinEventHandler OnPlayerJoin;

        public delegate void PlayerLeftEventHandler(GarticClient client, int id, int newOwnerId);
        public event PlayerLeftEventHandler OnPlayerLeft;

        public delegate void DisconnectEventHandler(GarticClient client, int reason);
        public event DisconnectEventHandler OnDisconnect;

        public Dictionary<int, User> Players { get; private set; } = new Dictionary<int, User>();

        public GarticSocket Socket { get; private set; }
        public GarticDrawController Draw { get; private set; }

        public GarticClient() { }

        public GarticClient(string nickname, string avatar, string roomId) : base()
        {
            Login(nickname, avatar, roomId);
        }

        public async void Login(string nickname, string avatar, string roomId)
        {
            Socket = new GarticSocket();
            foreach (var user in (await Socket.Connect("en", nickname, avatar, roomId)).Users)
                Players.Add(user.Id, user);
            Socket.OnMessage += OnSocketMessage;
            Draw = new GarticDrawController(Socket);
        }

        public void SendSentence(string sentence)
        {
            Socket.SendString(new GarticTextInputMessage(sentence, TextInputType.Sentence).Serialize());
        }

        public void SendGuess(string guess)
        {
            Socket.SendString(new GarticTextInputMessage(guess, TextInputType.Guess).Serialize());
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
                Draw.TurnNumber = castMessage.Number;

                OnGameTurn?.Invoke(this, castMessage.TurnType, castMessage.Number, castMessage.PreviousUserId, castMessage.PreviousTurnData);
            }

            if (message.Type == GarticMessageType.PlayerJoin)
            {
                var castMessage = message as GarticPlayerJoinMessage;
                Players.Add(castMessage.Id, new User
                {
                    Id = castMessage.Id,
                    Avatar = castMessage.Avatar,
                    Change = castMessage.Change,
                    Nick = castMessage.Nickname,
                    Owner = castMessage.Owner,
                    Points = castMessage.Points,
                    Viewer = castMessage.Spectator
                });

                OnPlayerJoin?.Invoke(this, castMessage.Id, castMessage.Nickname, castMessage.Avatar);
            }

            if (message.Type == GarticMessageType.PlayerLeft)
            {
                var castMessage = message as GarticPlayerLeftMessage;
                Players.Remove(castMessage.Id);
                Players[castMessage.NewOwnerId].Owner = true;

                OnPlayerLeft?.Invoke(this, castMessage.Id, castMessage.NewOwnerId);
            }

            if (message.Type == GarticMessageType.Disconnect)
            {
                var castMessage = message as GarticDisconnectMessage;
                OnDisconnect?.Invoke(this, castMessage.Reason);
            }
        }
    }
}
