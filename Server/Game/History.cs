using System.Collections.Generic;
using CardGame.Server.Game.Commands;

namespace CardGame.Server.Game
{
    public class History: Godot.Object
    {
        private int Cursor = 0;
        private int TurnCount = 0;
        private readonly List<Command> Events = new List<Command>();

        public void OnPlayExecuted(Player player, Command executedCommand)
        {
            // Should be private but will need to reroute connection
            Events.Add(executedCommand);
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