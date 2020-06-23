using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CardGame.Server.Game
{
    public class Players: IEnumerable<Player>
    {
        private Dictionary<int, Player> PlayersById = new Dictionary<int, Player>();

        public Players(IReadOnlyList<Player> players)
        {
            foreach (var player in players) {PlayersById[player.Id] = player;}
            players[0].Opponent = players[1];
            players[1].Opponent = players[0];
        }
        public IEnumerator<Player> GetEnumerator() => PlayersById.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public Player this[int id] => PlayersById[id];

        // Todo: Temporary Method. Remove This
        public Player TurnPlayer() => PlayersById.Values.ToList()[1];

    }
}