using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    internal class Lobby
    {
        public Lobby(int id, Player player)
        {
            this.Id = id;
            Players = new List<Player>() { player };
        }

        public int Id { get; set; }
        public List<Player> Players { get; set; }
    }
}
