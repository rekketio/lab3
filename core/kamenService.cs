using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace core
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class kamenService : IkamenService
    {
        List<Lobby> lobbies = new List<Lobby>();

        int PlayerID = 1;
        int LobbyID = 1;

        public Tuple<int, int, string> Connect(string nickname)
        {
            int lobbyID;
            string opNick = "Нет соперника";

            Player player = new Player()
            {
                Id = PlayerID,
                Name = nickname,
                OperationContext = OperationContext.Current
            };

            Lobby freeLobby = null;
            foreach (var Lobby in lobbies)
            {
                if (Lobby.Players.Count == 1)
                {
                    freeLobby = Lobby;
                    break;
                }
            }

            if (freeLobby == null)
            {
                Lobby lobby = new Lobby(LobbyID, player);
                lobbyID = lobby.Id;
                lobbies.Add(lobby);
            }
            else
            {
                foreach (var opponent in freeLobby.Players)
                {
                    opponent.OperationContext.GetCallbackChannel<IServiceCallback>().StartGame(player.Name);
                    opNick = opponent.Name;
                }
                freeLobby.Players.Add(player);
                lobbyID = freeLobby.Id;
            }
            PlayerID++;
            LobbyID++;

            return new Tuple<int, int, string>(lobbyID, player.Id, opNick);
        }

        public void Disconnect(int lobbyId)
        {
            var lobby = lobbies.FirstOrDefault(lobby_ => lobby_.Id == lobbyId);
            if (lobby != null)
                lobbies.Remove(lobby);
        }

        public void GameMoveSend(int GameSelection, int lobbyId, int senderID)
        {
            var Lobby = lobbies.FirstOrDefault(lobby_ => lobby_.Id == lobbyId);
            var opponent = Lobby.Players.FirstOrDefault(player_ => player_.Id != senderID);
            if (opponent != null)
                opponent.OperationContext.GetCallbackChannel<IServiceCallback>().GameMoveReceive(GameSelection);

        }
    }
}
