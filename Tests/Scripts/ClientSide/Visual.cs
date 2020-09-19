using System.Threading.Tasks;
using CardGame.Client.Game;
using CardGame.Server.Game.Zones;
using Godot.Collections;

namespace CardGame.Tests.Scripts.ClientSide
{
    public class Visual: WAT.Test
    {
        public override async void Pre()
        {
            await ToSignal(UntilTimeout(2F), YIELD);
        }

        [Test]
        public async void LoadDeck()
        {
            var match = new TestMatch();
            AddChild(match);
            var messenger = match.Messenger;
            messenger.QueueEvent(CommandId.LoadDeck, new object[] {new Dictionary<int, SetCodes>{ {1, SetCodes.AlphaDungeonGuide} }});
            messenger.ExecuteEvents();
            await ToSignal(UntilSignal(match, nameof(Match.OnExecutionComplete), 10), YIELD);
            Assert.IsEqual(match.Player.Deck.Count, 1);
        }

        [Test]
        public async void ActivateCard()
        {
            var match = new TestMatch();
            AddChild(match);
            var messenger = match.Messenger;
            messenger.QueueEvent(CommandId.Draw, new object[]{});
            messenger.QueueEvent(CommandId.SetFaceDown, new object[]{});
            messenger.QueueEvent(CommandId.RevealCard, new object[] {1, SetCodes.AlphaQuestReward, ZoneIds.Support});
            messenger.QueueEvent(CommandId.Activate, new object[] {1, 0});
            messenger.ExecuteEvents();
            await ToSignal(UntilSignal(match, nameof(Match.OnExecutionComplete), 10), YIELD);
            await ToSignal(UntilTimeout(5F), YIELD);

        }
        
    }
}
