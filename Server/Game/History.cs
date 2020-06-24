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

        public void Add(Command command)
        {
            command.Execute();
            Events.Add(command);
            if (command is EndTurn) {TurnCount += 1;}
            EmitSignal(nameof(EventRecorded), command);
            
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