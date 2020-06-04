using System;
using CardGame;
using CardGame.Client;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace CardGameSharp.Tests.Scripts.Connected
{
    public class ActionResponseTest: ConnectedFixture
    {
	    public override string Title()
	    {
		    return "Given A Client Action";
	    }

	    public override void Pre()
	    {
		    for (var i = 0; i < 38; i++)
		    {
			    DeckList.Add(SetCodes.Alpha_DungeonGuide);
		    }

		    DeckList.Add(SetCodes.Alpha_QuestReward);
		    DeckList.Add(SetCodes.Alpha_CounterAttack);
		    AddGame();
	    }

	    public override void Post()
	    {
		    DeckList.Clear();
		    RemoveGame();
		    // await for a second?????
	    }

	    [Test]
	    public async void WhenTurnPlayerDeclaresAttack()
	    {
		    var input = Clients[1].GetNode<Game>("1").GameInput;
		    var player = Clients[1].GetNode<Game>("1").Player;
		    var input2 = Clients[0].GetNode<Game>("1").GameInput;
		    var player2 = Clients[0].GetNode<Game>("1").Player;
		    var oldHealth = player.Health;

			// Player 1 sets counter_attack
		    var CounterAttack = player.Hand[0];
		    GD.Print(CounterAttack.Title);
		    GD.Print("state:", player.State);
		    GD.Print("state:", player2.State);
		    input.OnSetFaceDown(CounterAttack);
		    await ToSignal(UntilTimeout(.5F), YIELD);
		    input.EndTurn();
		    await ToSignal(UntilTimeout(.5F), YIELD);
		    var DungeonGuide = player2.Hand[2];
		    input2.OnDeploy(DungeonGuide); // Future responses tests might break this
		    await ToSignal(UntilTimeout(.5F), YIELD);
		    input.OnPassPriority();
		    await ToSignal(UntilTimeout(.5F), YIELD);
		    input2.OnPassPriority();
		    await ToSignal(UntilTimeout(.5F), YIELD);
		    input2.EndTurn();
		    await ToSignal(UntilTimeout(.5F), YIELD);
		    input.EndTurn();
		    await ToSignal(UntilTimeout(.5F), YIELD);
		    input2.OnAttack(DungeonGuide, -1);
		    await ToSignal(UntilTimeout(.5F), YIELD);
		    CounterAttack.ActivateCard(0, new Array());
		    await ToSignal(UntilTimeout(.5F), YIELD);
		    input2.OnPassPriority();
		    await ToSignal(UntilTimeout(.5F), YIELD);
		    input.OnPassPriority();
		    await ToSignal(UntilTimeout(.5F), YIELD);

		    Assert.Has(DungeonGuide, player2.Graveyard,
			    "Then their opponent can respond with a support card (that destroys the attacker)");
		    Assert.IsEqual(oldHealth, player.Health, "(And the battle was cancelled)");
	    }
    }
}

/*	

	# Player 1 sets counter_attack
	var CounterAttack = player.Hand[0];
	input.set_facedown(CounterAttack);
	await ToSignal(UntilTimeout(.5F), YIELD);
	input.EndTurn();
	await ToSignal(UntilTimeout(.5F), YIELD);
	var DungeonGuide = player2.Hand[2];
	input2.deploy(DungeonGuide) # Future responses tests might break this
	await ToSignal(UntilTimeout(.5F), YIELD);
	input.PassPriority();
	await ToSignal(UntilTimeout(.5F), YIELD);
	input2.PassPriority();
	await ToSignal(UntilTimeout(.5F), YIELD);
	input2.EndTurn();
	await ToSignal(UntilTimeout(.5F), YIELD);
	input.EndTurn();
	await ToSignal(UntilTimeout(.5F), YIELD);
	input2.DirectAttack(DungeonGuide);
	await ToSignal(UntilTimeout(.5F), YIELD);
	CounterAttack.Activate();
	await ToSignal(UntilTimeout(.5F), YIELD);
	input2.PassPriority();
	await ToSignal(UntilTimeout(.5F), YIELD);
	input.PassPriority();
	await ToSignal(UntilTimeout(.5F), YIELD);
	
	Assert.Has(DungeonGuide, player2.Graveyard, \
				"Then their opponent can respond with a support card (that destroys the attacker)")
	Assert.IsEqual(oldHealth, player.Health, "(And the battle was cancelled)");
*/
