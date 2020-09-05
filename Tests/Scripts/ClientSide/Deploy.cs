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
            var match = new Match();
            AddChild(match);
            var messenger = match.GetNode<Messenger>("Messenger");
            messenger.QueueEvent(CommandId.LoadDeck, new object[] {new Dictionary<int, SetCodes>{ {1, SetCodes.AlphaDungeonGuide} }});
            messenger.ExecuteEvents();
            await ToSignal(UntilSignal(match, nameof(Match.OnExecutionComplete), 5F), YIELD);
            Assert.IsEqual(match.GetNode<Player>("Spatial/Table3D/PlayMat/Player").Deck.Count, 1);
        }
        
    }
}
