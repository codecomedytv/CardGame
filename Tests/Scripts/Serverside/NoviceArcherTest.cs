using System;
using System.Collections.Generic;
using CardGame.Server;
using Godot;

namespace CardGame.Tests.Scripts.Serverside
{
    public class NoviceArcherTest: GameFixture
    {
        public override string Title()
        {
            return "Given Novice Archer";
        }

        [Test]
        public void When_Played()
        {
            var deckList = new List<SetCodes>();
            for (var i = 0; i < 11; i++)
            {
                deckList.Add(SetCodes.AlphaDungeonGuide);
            }
            
            deckList.Add(SetCodes.Debug500500);
            deckList.Add(SetCodes.AlphaNoviceArcher);

            StartGame(deckList);

            var weakling = Player.Hand[1];
            Play.Deploy(Player.Id, weakling.Id);
            Play.PassPlay(Opponent.Id);
            Play.PassPlay(Player.Id);
            Play.EndTurn(Player.Id);
            var noviceArcher = Opponent.Hand[0];
            Play.Deploy(Opponent.Id, noviceArcher.Id);
            Play.PassPlay(Player.Id);
            Play.PassPlay(Opponent.Id);
            Play.Target(Opponent.Id, weakling.Id);
            
            Assert.Has(weakling, Player.Graveyard, "Then it destroys a 500/500 Unit");
        }

    }
}
