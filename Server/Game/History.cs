using System.Collections.Generic;
using CardGame.Server.Game.Commands;
using Godot;

namespace CardGame.Server.Game
{
    public class History: Godot.Object
    {
        [Signal]
        public delegate void EventRecorded();
        private int Cursor = 0;
        private int TurnCount = 0;
        private readonly List<Command> Events = new List<Command>();

        public void OnPlayExecuted(Command executedCommand)
        {
            Events.Add(executedCommand);
            if (executedCommand is EndTurn) {TurnCount += 1;}
            EmitSignal(nameof(EventRecorded), executedCommand);
            
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

        public Command this[int index] => Events[index];
    }
}