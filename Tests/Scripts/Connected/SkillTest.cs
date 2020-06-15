using CardGame.Client;
using Godot.Collections;

namespace CardGame.Tests.Scripts.Connected
{
    public class SkillTest: ConnectedFixture
    {
	    public override string Title() => "When using a Skill";

	    public override void Pre()
	    {
		    for (var i = 0; i < 38; i++)
		    {
			    DeckList.Add(SetCodes.Alpha_DungeonGuide);
		    }
		    
		    DeckList.Add(SetCodes.DebugBounceFromField);
		    AddGame();
	    }
	    public override void Post() => RemoveGame();

	    [Test]
	    public async void That_Returns_A_Unit_To_Opponents_Hand()
	    {
		    /*var input = Clients[1].GetNode<Game>("1").GameInput;
		    var player = Clients[1].GetNode<Game>("1").Player;
		    var input2 = Clients[0].GetNode<Game>("1").GameInput;
		    var player2 = Clients[0].GetNode<Game>("1").Player;
		    var Bouncer = player.Hand[0];
		    input.OnSetFaceDown(Bouncer);
		    await ToSignal(UntilTimeout(1.0F), YIELD);
		    input.EndTurn();
		    await ToSignal(UntilTimeout(1.0F), YIELD);
		    var Unit = player2.Hand[2];
		    input2.OnDeploy(Unit);
		    await ToSignal(UntilTimeout(1.0F), YIELD);
			Bouncer.ActivateCard(new Array<int>{Unit.Id});
			await ToSignal(UntilTimeout(1.0F), YIELD);
			input2.OnPassPriority();
			await ToSignal(UntilTimeout(1.0F), YIELD);
			input.OnPassPriority();
			await ToSignal(UntilTimeout(1.0F), YIELD);

		    Assert.Has(Unit, player2.Hand, "Unit was bounced");
		    Assert.Has(Bouncer, player.Graveyard, "Support was activated");*/

		    // When left in this crashes and we haven't implemented the proper system for it yet
		    Assert.Fail("Commented Out Test");
	    }
    }
}
