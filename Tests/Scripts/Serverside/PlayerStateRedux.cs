using System.Collections.Generic;
using CardGame.Server;
using Godot.Collections;

namespace CardGame.Tests.Scripts.Serverside
{
    public class PlayerStateRedux: GameFixture
    {
	    public override string Title()
	    {
		    return "Triggered Event Cards Redux";
	    }

	    [Test]
	    public void OnAttack()
	    {
		    var deckList = new List<SetCodes>();
		    deckList.Add(SetCodes.Debug500_500);
		    deckList.Add(SetCodes.Debug500_500);
		    deckList.Add(SetCodes.Debug500_500);
		    deckList.Add(SetCodes.DebugDestroyAttackingUnit);
		    deckList.Add(SetCodes.DebugDestroyAttackingUnit);
		    deckList.Add(SetCodes.DebugDestroyAttackingUnit);
		    deckList.Add(SetCodes.Debug1500_1000);
		    deckList.Add(SetCodes.Debug1500_1000);
		    deckList.Add(SetCodes.Debug1500_1000);
		    StartGame(deckList);
		    
		    // LINK SUMMARY
			// ACTION TAKEN (NON CHAIN)
			// PLAYER WHO TOOK ACTION MAY CHAIN
			// OPPONENT MAY CHAIN
			// RESOLVE
			
			// UPDATE
			// Player A May Take Action From IDLE
			// Player A May Respond To Own Action (Repeat This Until Pass) From ACTIVE
			// Player B May Respond (From ACTIVE)
			// Player A May Respond (From ACTIVE)
			
			const int directAttack = -1;
			var attacker = Players[1].Hand[6];
			var chainMines = (Support)Players[0].Hand[3];
			

			
			// Whitebox Checking
			Assert.IsEqual(Players[1].State, Player.States.Idle, "Player 1 is IDLE");
			Assert.IsEqual(Players[0].State, Player.States.Passive, "Player 0 is PASSIVE");
			
			Play.Deploy(Players[1].Id, attacker.Id);
			
			// Whitebox Checking
			// Player 1 took an action from IDLE that doesn't start a chain so they should
			// be active (rather than acting?) Maybe acting != activating?
			
			Assert.Has(attacker, Players[1].Field, "Attacker Has been Deployed");
			
			Assert.IsEqual(Players[1].State, Player.States.Acting, "Player 1 is ACTING");
			Assert.IsEqual(Players[0].State, Player.States.Active, "Player 0 is ACTIVE");
			
			Play.PassPlay(Players[0].Id);
			
			// Whitebox Check
			Assert.IsEqual(Players[0].State, Player.States.Passing, "Player 0 is PASSING");
			Assert.IsEqual(Players[1].State, Player.States.Active, "Player 1 is ACTIVE");
			
			Play.PassPlay(Players[1].Id);
			
			Assert.IsEqual(Players[1].State, Player.States.Idle, "Player 1 is IDLE");
			Assert.IsEqual(Players[0].State, Player.States.Passive, "Player 0 is PASSIVE");
			
			Play.EndTurn(Players[1].Id);
			
			Assert.IsEqual(Players[1].State, Player.States.Passive, "Player 1 is PASSIVE");
			Assert.IsEqual(Players[0].State, Player.States.Idle, "Player 0 is IDLE");
			
			Play.SetFaceDown(Players[0].Id, chainMines.Id);
			Assert.Has(chainMines, Players[0].Support, "Chain Mines Has been set");
			Play.EndTurn(Players[0].Id);
			
			Assert.IsEqual(Players[1].State, Player.States.Idle, "Player 1 is IDLE");
			Assert.IsEqual(Players[0].State, Player.States.Passive, "Player 0 is PASSIVE");
			
			Play.Attack(Players[1].Id, attacker.Id, directAttack);
			
			Assert.IsEqual(Players[1].State, Player.States.Acting, "Player 1 is ACTING");
			Assert.IsEqual(Players[0].State, Player.States.Active, "Player 0 is ACTIVE");
			
			Assert.IsTrue(chainMines.Ready, "ChainMines Is Ready");
			Assert.IsTrue(chainMines.CanBeActivated, "ChainMine can be activated");
			Play.Activate(Players[0].Id, chainMines.Id, 0, new Array<int>());

			Assert.IsEqual(Players[0].State, Player.States.Acting, "Player 0 is ACTING");
			Assert.IsEqual(Players[1].State, Player.States.Active, "Player 1 is ACTIVE");
			
			Play.PassPlay(Players[1].Id);
			
			Assert.IsEqual(Players[1].State, Player.States.Passing, "Player 1 is PASSING");
			Assert.IsEqual(Players[0].State, Player.States.Active, "Player 0 is ACTIVE");
			
			Play.PassPlay(Players[0].Id);
			
			Assert.IsEqual(Players[1].State, Player.States.Idle, "Player 1 is IDLE");
			Assert.IsEqual(Players[0].State, Player.States.Passive, "Player 0 is PASSIVE");
			
			
			
			Assert.Has(attacker, Players[1].Graveyard, attacker + "Unit %s is in Player 1's discard");
			
			Assert.Has(chainMines, Players[0].Graveyard, chainMines + "Support %s is in Player 0's Discard");

	    }
    }
}
