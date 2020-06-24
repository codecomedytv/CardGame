using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
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
            if (Events.Contains(command))
            {
                // Figure out a way to handle compound effects (effects that call other ones)
                // possibly give them an identity, cache them, and then check against the references
                
                // Some Events Trigger Other Events (for example ending a turn triggers readying cards)
                // What about events that call commands? Invert them?
                return;
            }
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