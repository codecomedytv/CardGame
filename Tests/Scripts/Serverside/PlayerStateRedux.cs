using System;
using System.Collections.Generic;
using CardGame.Server.Game;
using CardGame.Server.Game.Cards;

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
		    deckList.Add(SetCodes.Debug500500);
		    deckList.Add(SetCodes.Debug500500);
		    deckList.Add(SetCodes.Debug500500);
		    deckList.Add(SetCodes.DebugDestroyAttackingUnit);
		    deckList.Add(SetCodes.DebugDestroyAttackingUnit);
		    deckList.Add(SetCodes.DebugDestroyAttackingUnit);
		    deckList.Add(SetCodes.Debug15001000);
		    deckList.Add(SetCodes.Debug15001000);
		    deckList.Add(SetCodes.Debug15001000);
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
			var attacker = Player.Hand[6];
			var chainMines = (Support)Opponent.Hand[3];
			

			
			// Whitebox Checking
			Assert.IsTrue(Player.State == States.Idle, "Player 1 is IDLE");
			Assert.IsTrue(Opponent.State == States.Passive, "Player 0 is PASSIVE");
			
			Play.Deploy(Player.Id, attacker.Id);
			
			// Whitebox Checking
			// Player 1 took an action from IDLE that doesn't start a chain so they should
			// be active (rather than acting?) Maybe acting != activating?
			
			Assert.Has(attacker, Player.Field, "Attacker Has been Deployed");
			
			Assert.IsTrue(Player.State == States.Acting, "Player 1 is ACTING");
			Assert.IsTrue(Opponent.State == States.Active, "Player 0 is ACTIVE");
			
			Play.PassPlay(Opponent.Id);
			
			// Whitebox Check
			Assert.IsTrue(Opponent.State == States.Passing, "Player 0 is PASSING");
			Assert.IsTrue(Player.State == States.Active, "Player 1 is ACTIVE");
			
			Play.PassPlay(Player.Id);
			
			Assert.IsTrue(Player.State == States.Idle, "Player 1 is IDLE");
			Assert.IsTrue(Opponent.State == States.Passive, "Player 0 is PASSIVE");
			
			Play.EndTurn(Player.Id);
			
			Assert.IsTrue(Player.State == States.Passive, "Player 1 is PASSIVE");
			Assert.IsTrue(Opponent.State == States.Idle, "Player 0 is IDLE");
			
			Play.SetFaceDown(Opponent.Id, chainMines.Id);
			Assert.Has(chainMines, Opponent.Support, "Chain Mines Has been set");
			Play.EndTurn(Opponent.Id);
			
			Assert.IsTrue(Player.State == States.Idle, "Player 1 is IDLE");
			Assert.IsTrue(Opponent.State == States.Passive, "Player 0 is PASSIVE");
			Play.DirectAttack(Player.Id, attacker.Id);
			
			Console.WriteLine(Player.State + "|" + Player.State.ToString());
			Console.WriteLine(Opponent.State + "|" + Opponent.State.ToString());
			Assert.IsTrue(Player.State == States.Acting, "Player 1 is ACTING");
			Assert.IsTrue(Opponent.State == States.Active, "Player 0 is ACTIVE");
			
			Assert.IsTrue(chainMines.IsReady, "ChainMines Is Ready");
			Assert.IsTrue(chainMines.CanBeActivated, "ChainMine can be activated");
			Play.Activate(Opponent.Id, chainMines.Id);

			Assert.IsTrue(Opponent.State == States.Acting, "Player 0 is ACTING");
			Assert.IsTrue(Player.State == States.Active, "Player 1 is ACTIVE");
			
			Play.PassPlay(Player.Id);
			
			Assert.IsTrue(Player.State == States.Passing, "Player 1 is PASSING");
			Assert.IsTrue(Opponent.State == States.Active, "Player 0 is ACTIVE");
			
			Play.PassPlay(Opponent.Id);
			
			Assert.IsTrue(Player.State == States.Idle, "Player 1 is IDLE");
			Assert.IsTrue(Opponent.State == States.Passive, "Player 0 is PASSIVE");
			
			
			
			Assert.Has(attacker, Player.Graveyard, attacker + "Unit %s is in Player 1's discard");
			
			Assert.Has(chainMines, Opponent.Graveyard, chainMines + "Support %s is in Player 0's Discard");

	    }
    }
}
