using System.Linq;
using CardGame.Client.Game;
using CardGame.Client.Game.Players;
using Godot.Collections;

namespace CardGame.Tests.Scripts.ClientSide
{
    public class Visual: WAT.Test
    {
        [Test]
        public async void LoadDeck()
        {
            var match = new TestMatch();
            AddChild(match);
            var messenger = match.GetNode<Messenger>("Messenger");
            messenger.QueueEvent(CommandId.LoadDeck, new object[] {new Dictionary<int, SetCodes>{ {1, SetCodes.AlphaDungeonGuide} }});
            messenger.ExecuteEvents();
            await ToSignal(UntilSignal(match, nameof(Match.OnExecutionComplete), 1F), YIELD);
            Assert.IsEqual(match.Player.Deck.Count, 1);
        }
        
    }
}
