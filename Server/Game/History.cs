using System.Collections.Generic;
using CardGame.Server.Game.Events;

namespace CardGame.Server.Game
{
    public class History: Godot.Object
    {
        private int Cursor = 0;
        private int TurnCount = 0;
        private readonly List<GameEvent> Events = new List<GameEvent>();

        public void OnPlayExecuted(Player player, GameEvent gameEvent)
        {
            // Should be private but will need to reroute connection
            Events.Add(gameEvent);
            //if(gameEvent.Identity == TurnEnded) {TurnCount += 1}
        }

        public void Redo()
        {
            Cursor += 1;
            var gameEvent = Events[Cursor];
        }

        public void Undo()
        {
            Cursor -= 1;
            var gameEvent = Events[Cursor];
        }
    }
}