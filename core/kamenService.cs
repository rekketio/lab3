using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace core
{
    public class kamenService : IkamenService
    {
        List<Lobby> lobbies = new List<Lobby>();

        int next_id_player = 1;
        int next_id_lobby = 1;

        public Tuple<int, int, string> Connect(string nickname)
        {
            int lobbyID;
            string nickname_opponent = "Нет соперника";

            Player player = new Player()
            {
                Id = next_id_player,
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
                Lobby lobby = new Lobby(next_id_lobby, player);
                lobbyID = lobby.Id;
                lobbies.Add(lobby);
            }
            else
            {
                foreach (var opponent in freeLobby.Players)
                {
                    opponent.OperationContext.GetCallbackChannel<IServiceCallback>().StartGame(player.Name);
                    nickname_opponent = opponent.Name;
                }
                freeLobby.Players.Add(player);
                lobbyID = freeLobby.Id;
            }
            next_id_player++;
            next_id_lobby++;

            return new Tuple<int, int, string>(lobbyID, player.Id, nickname_opponent);
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
