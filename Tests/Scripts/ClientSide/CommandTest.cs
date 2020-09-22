using System.Threading.Tasks;
using CardGame.Client.Game;
using CardGame.Client.Game.Players;
using Godot;
using Godot.Collections;

namespace CardGame.Tests.Scripts.ClientSide
{
	/*
	 *  These tests do not care about rules, only about actions..
	 * ..essentially we are only focused on seeing if our command objects work correctly..
	 * ..there will be a separate test suite (integration-ish) that will focus on cross-server/client rules
	 */
	public class CommandTest: WAT.Test
	{
		private Match Match;
		private Player Player;
		private Messenger Messenger;

		private async Task<object[]> Execution(double timeLimit = 1)
		{
			return await ToSignal(UntilSignal(Match, nameof(Match.OnExecutionComplete), timeLimit), YIELD);
		}
		
		public override async void Start()
		{
			Match = new Match();
			AddChild(Match);
			Player = Match.Player;
			Messenger = Match.Messenger;
			var deckList = new Dictionary<int, SetCodes>
			{
				[0] = SetCodes.AlphaDungeonGuide, [1] = SetCodes.AlphaQuestReward
			};
			Messenger.QueueEvent(CommandId.LoadDeck, new object[]{deckList});
			Messenger.ExecuteEvents();
			await Execution();
		}

		[Test]
		public async void When_They_Deploy_A_Unit()
		{
			const int toDeployId = 0;
			Messenger.QueueEvent(CommandId.Draw, new object[] {toDeployId});
			Messenger.QueueEvent(CommandId.Deploy, new object[] {toDeployId});
			Messenger.ExecuteEvents();
			await Execution();

			var toDeploy = Match.Player.Units[0];
			Assert.IsEqual(toDeploy.Zone, Match.Player.Units, "Unit was deployed");
		}
		
		[Test]
		public async void When_They_Set_A_Support()
		{
			const int toSetId = 1;
			Messenger.QueueEvent(CommandId.Draw, new object[] {toSetId});
			Messenger.QueueEvent(CommandId.SetFaceDown, new object[] {toSetId});
			Messenger.ExecuteEvents();
			await Execution();

			var toSet = Match.Player.Support[0];
			Assert.IsEqual(toSet.Zone, Match.Player.Support, "Support was Set");
		}

	}
}
