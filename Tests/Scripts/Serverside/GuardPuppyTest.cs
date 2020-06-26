using System.Collections.Generic;
using CardGame.Server;
using CardGame.Server.Game;
using CardGame.Server.Game.Tags;
using Godot.Collections;

namespace CardGame.Tests.Scripts.Serverside
{
    public class GuardPuppyTest : GameFixture
    {
        private List<SetCodes> DeckList = new List<SetCodes>();

        public override string Title()
        {
            return "Given Guard Puppy";
        }

        public override void Pre()
        {
            DeckList.Clear();
            for (var i = 0; i < 11; i++)
            {
                DeckList.Add(SetCodes.Alpha_DungeonGuide);
            }
        }

        [Test]
        public void guard_puppy()
        {
            // Cannot Be Destroyed By Battle
            //No Other Units can be attacked (Other than puppy)

            DeckList.Add(SetCodes.Alpha_GuardPuppy);
            StartGame(DeckList);
            var guardPuppy = Player.Hand[0];
            var dungeonGuide = Player.Hand[1];
            var dungeonGuide2 = Player.Hand[2];
            Play.Deploy(Player.Id, dungeonGuide.Id);
            Play.PassPlay(Opponent.Id);
            Play.PassPlay(Player.Id);
            Play.Deploy(Player.Id, guardPuppy.Id);
            Play.PassPlay(Opponent.Id);
            Play.PassPlay(Player.Id);
            Play.Deploy(Player.Id, dungeonGuide2.Id);
            Play.PassPlay(Opponent.Id);
            Play.PassPlay(Player.Id);
            Play.EndTurn(Player.Id);
            // Need to add tests for cannot be attacked
            Assert.IsTrue(dungeonGuide.HasTag(TagIds.CannotBeAttacked),
                "Then a Unit that was played before it has the tag 'Cannot Be Attacked'");
            Assert.IsTrue(dungeonGuide2.HasTag(TagIds.CannotBeAttacked),
                "Then a Unit that was played after it has the tag 'Cannot Be Attacked'");
        }
    }
}