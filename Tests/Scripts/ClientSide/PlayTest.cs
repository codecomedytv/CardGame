using CardGame.Client.Game;
using Godot.Collections;

namespace CardGame.Tests.Scripts.ClientSide
{
    public class PlayTest: WAT.Test
    {
        [Test]
        public async void When_They_Deploy_A_Unit()
        {
            // What's our goal with these tests?
            // Should be treated separately than a test that tests inter-communication?
            // (I'm going to go with yes, these tests don't care about rules, just about actions working correctly)
            // These tests will overlap with both serverside and the interconnected tests for great coverage (i hope)
            
            var match = new Match();
            AddChild(match);
            var messenger = match.Messenger;
            var deckList = new Dictionary<int, SetCodes>();
            
            for (var i = 0; i < 40; i++) { deckList[i] = SetCodes.AlphaDungeonGuide; }
            
            messenger.QueueEvent(CommandId.LoadDeck, new object[] {deckList});
            messenger.ExecuteEvents();
            await ToSignal(UntilSignal(match, nameof(Match.OnExecutionComplete), 10), YIELD);

            var toDeploy = match.Player.Deck[0];
            messenger.QueueEvent(CommandId.Draw, new object[] {toDeploy.Id});
            messenger.QueueEvent(CommandId.Deploy, new object[] {toDeploy.Id});
            messenger.ExecuteEvents();
            await ToSignal(UntilSignal(match, nameof(Match.OnExecutionComplete), 10), YIELD);
            
            Assert.IsEqual(toDeploy.Zone, match.Player.Units, "Unit was deployed");
        }
        
        [Test]
        public async void When_They_Set_A_Support()
        {
            var match = new Match();
            AddChild(match);
            var messenger = match.Messenger;
            var deckList = new Dictionary<int, SetCodes>();
            
            for (var i = 0; i < 40; i++) { deckList[i] = SetCodes.AlphaQuestReward; }
            
            messenger.QueueEvent(CommandId.LoadDeck, new object[] {deckList});
            messenger.ExecuteEvents();
            await ToSignal(UntilSignal(match, nameof(Match.OnExecutionComplete), 10), YIELD);

            var toSet = match.Player.Deck[0];
            messenger.QueueEvent(CommandId.Draw, new object[] {toSet.Id});
            messenger.QueueEvent(CommandId.SetFaceDown, new object[] {toSet.Id});
            messenger.ExecuteEvents();
            await ToSignal(UntilSignal(match, nameof(Match.OnExecutionComplete), 10), YIELD);
            
            Assert.IsEqual(toSet.Zone, match.Player.Support, "Unit was deployed");
        }

    }
}