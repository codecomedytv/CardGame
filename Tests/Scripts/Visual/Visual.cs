using System.Collections.Generic;
using CardGame.Client.Game;
using Godot;

namespace CardGame.Tests.Visual
{
    public class Deploy: Node
    {
        // Add Game
        // Use Messenger For Setup

        public override void _Ready()
        {
            // We don't need to talk to the server for these
            // we do need to await though (add an event to the match?
            var match = new Match();
            AddChild(match);
            var messenger = match.GetNode<Messenger>("Messenger");
            messenger.QueueEvent(CommandId.LoadDeck, new object[] {new Dictionary<int, SetCodes>{ {1, SetCodes.AlphaDungeonGuide} }});
            messenger.ExecuteEvents();
        }
    }
}