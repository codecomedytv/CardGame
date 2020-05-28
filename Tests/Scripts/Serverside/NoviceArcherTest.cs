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
                deckList.Add(SetCodes.Alpha_DungeonGuide);
            }
            
            deckList.Add(SetCodes.Debug500_500);
            deckList.Add(SetCodes.Alpha_NoviceArcher);

            StartGame(deckList);

            var weakling = Players[1].Hand[1];
            Play.Deploy(Players[1].Id, weakling.Id);
            Play.PassPlay(Players[0].Id);
            Play.PassPlay(Players[1].Id);
            Play.EndTurn(Players[1].Id);
            var noviceArcher = Players[0].Hand[0];
            Play.Deploy(Players[0].Id, noviceArcher.Id);
            // Game gets paused here
            GD.Print("Targeting!");
            Play.Target(Players[0].Id, weakling.Id);
            
            Assert.Has(weakling, Players[1].Graveyard, "Then it destroys a 500/500 Unit");
        }

    }
}
