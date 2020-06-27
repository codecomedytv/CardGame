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
                DeckList.Add(SetCodes.AlphaDungeonGuide);
            }
        }

        [Test]
        public void guard_puppy()
        {
            // Cannot Be Destroyed By Battle
            //No Other Units can be attacked (Other than puppy)

            DeckList.Add(SetCodes.AlphaGuardPuppy);
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
        
        [Test]
        public void test_guard_puppy_removed_from_the_field()
        {

	        DeckList.Add(SetCodes.DebugDestroyOpponentUnit);
	        DeckList.Add(SetCodes.AlphaGuardPuppy);

	        StartGame(DeckList);
	        var DestroyUnit = Player.Hand[1];
	        Play.SetFaceDown(Player.Id, DestroyUnit.Id);
	        Play.EndTurn(Player.Id);
	        var GuardPuppy = Opponent.Hand[0];
	        var DungeonGuide = Opponent.Hand[2];
	        Play.Deploy(Opponent.Id, DungeonGuide.Id);
	        Play.PassPlay(Player.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.Deploy(Opponent.Id, GuardPuppy.Id);
	        Play.PassPlay(Player.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.EndTurn(Opponent.Id);
	        Play.Activate(Player.Id, DestroyUnit.Id, GuardPuppy.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.PassPlay(Player.Id);
	
	        Assert.Has(GuardPuppy, Opponent.Graveyard, "GuardPuppy is in owner's discard");
	        Assert.IsFalse(DungeonGuide.HasTag(TagIds.CannotBeAttacked), "Then a Unit it tagged no longer has the tag");
        }
        
        [Test]
        public void test_tag_unit_removed_from_field()
        {	
	        DeckList.Add(SetCodes.DebugDestroyOpponentUnit);
	        DeckList.Add(SetCodes.AlphaGuardPuppy);
	
	        StartGame(DeckList);
	        var destroyUnit = Player.Hand[1];
	        Play.SetFaceDown(Player.Id, destroyUnit.Id);
	        Play.EndTurn(Player.Id);
	        var guardPuppy = Opponent.Hand[0];
	        var dungeonGuide = Opponent.Hand[2];
	        Play.Deploy(Opponent.Id, dungeonGuide.Id);
	        Play.PassPlay(Player.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.Deploy(Opponent.Id, guardPuppy.Id);
	        Play.PassPlay(Player.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.EndTurn(Opponent.Id);
	        Play.Activate(Player.Id, destroyUnit.Id, dungeonGuide.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.PassPlay(Player.Id);
	
	        Assert.Has(dungeonGuide, Opponent.Graveyard, "That Unit is in owner's discard");
	        Assert.IsFalse(dungeonGuide.HasTag(TagIds.CannotBeAttacked), "Then that Unit no longer has the tag");
        }
    }
}