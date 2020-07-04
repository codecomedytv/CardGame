using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CardGame.Server.Game
{
    public class Players: IEnumerable<Player>
    {
        private Dictionary<int, Player> PlayersById = new Dictionary<int, Player>();
        public Player TurnPlayer { get; private set; }

        public Players(Player p1, Player p2)
        {
            PlayersById[p1.Id] = p1;
            PlayersById[p2.Id] = p2;
            p1.Opponent = p2;
            p2.Opponent = p1;
            p1.Seat = 1;
            p2.Seat = 2;
            TurnPlayer = PlayersById.Values.ToList()[1];
            p1.IsTurnPlayer = () => p1 == TurnPlayer;
            p2.IsTurnPlayer = () => p2 == TurnPlayer;
        }

        public void ChangeTurnPlayer() => TurnPlayer = TurnPlayer.Opponent;
        
        public IEnumerator<Player> GetEnumerator() => PlayersById.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public Player this[int id] => PlayersById[id];



    }
}