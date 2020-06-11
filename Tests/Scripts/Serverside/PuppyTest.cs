using System.Collections.Generic;
using CardGame.Server;
using CardGame.Server.Game;
using Godot.Collections;

namespace CardGame.Tests.Scripts.Serverside
{
    public class PuppyTest: GameFixture
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
	        var guardPuppy = Players[1].Hand[0];
	        var dungeonGuide = Players[1].Hand[1];
	        var dungeonGuide2 = Players[1].Hand[2];
	        Play.Deploy(Players[1].Id, dungeonGuide.Id);
	        Play.PassPlay(Players[0].Id);
	        Play.PassPlay(Players[1].Id);
	        Play.Deploy(Players[1].Id, guardPuppy.Id);
	        Play.PassPlay(Players[0].Id);
	        Play.PassPlay(Players[1].Id);
	        Play.Deploy(Players[1].Id, dungeonGuide2.Id);
	        Play.PassPlay(Players[0].Id);
	        Play.PassPlay(Players[1].Id);
	        Play.EndTurn(Players[1].Id);
			// Need to add tests for cannot be attacked
			Assert.IsTrue(dungeonGuide.HasTag(Tag.CannotBeAttacked),
				"Then a Unit that was played before it has the tag 'Cannot Be Attacked'");
	        Assert.IsTrue(dungeonGuide2.HasTag(Tag.CannotBeAttacked), 
	        "Then a Unit that was played after it has the tag 'Cannot Be Attacked'");
        }
        
        [Test]
        public void test_guard_puppy_removed_from_the_field()
        {

	        DeckList.Add(SetCodes.DebugDestroyOpponentUnit);
	        DeckList.Add(SetCodes.Alpha_GuardPuppy);

	        StartGame(DeckList);
	        var DestroyUnit = Players[1].Hand[1];
	        Play.SetFaceDown(Players[1].Id, DestroyUnit.Id);
	        Play.EndTurn(Players[1].Id);
	        var GuardPuppy = Players[0].Hand[0];
	        var DungeonGuide = Players[0].Hand[2];
	        Play.Deploy(Players[0].Id, DungeonGuide.Id);
	        Play.PassPlay(Players[1].Id);
	        Play.PassPlay(Players[0].Id);
	        Play.Deploy(Players[0].Id, GuardPuppy.Id);
	        Play.PassPlay(Players[1].Id);
	        Play.PassPlay(Players[0].Id);
	        Play.EndTurn(Players[0].Id);
	        Play.Activate(Players[1].Id, DestroyUnit.Id, new Array<int> {GuardPuppy.Id});
	        Play.PassPlay(Players[0].Id);
	        Play.PassPlay(Players[1].Id);
	
	        Assert.Has(GuardPuppy, Players[0].Graveyard, "GuardPuppy is in owner's discard");
	        Assert.IsFalse(DungeonGuide.HasTag(Tag.CannotBeAttacked), "Then a Unit it tagged no longer has the tag");
        }
        
        [Test]
        public void test_tag_unit_removed_from_field()
        {	
	        DeckList.Add(SetCodes.DebugDestroyOpponentUnit);
	        DeckList.Add(SetCodes.Alpha_GuardPuppy);
	
	        StartGame(DeckList);
	        var destroyUnit = Players[1].Hand[1];
	        Play.SetFaceDown(Players[1].Id, destroyUnit.Id);
	        Play.EndTurn(Players[1].Id);
	        var guardPuppy = Players[0].Hand[0];
	        var dungeonGuide = Players[0].Hand[2];
	        Play.Deploy(Players[0].Id, dungeonGuide.Id);
	        Play.PassPlay(Players[1].Id);
	        Play.PassPlay(Players[0].Id);
	        Play.Deploy(Players[0].Id, guardPuppy.Id);
	        Play.PassPlay(Players[1].Id);
	        Play.PassPlay(Players[0].Id);
	        Play.EndTurn(Players[0].Id);
	        Play.Activate(Players[1].Id, destroyUnit.Id, new Array<int> { dungeonGuide.Id });
	        Play.PassPlay(Players[0].Id);
	        Play.PassPlay(Players[1].Id);
	
	        Assert.Has(dungeonGuide, Players[0].Graveyard, "That Unit is in owner's discard");
	        Assert.IsFalse(dungeonGuide.HasTag(Tag.CannotBeAttacked), "Then that Unit no longer has the tag");
        }
    }
    
}

/*






*/