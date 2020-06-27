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
        private readonly List<Event> Events = new List<Event>();

        public void Add(Event Event)
        {
            // if (Events.Contains(command))
            // {
            //     // Figure out a way to handle compound effects (effects that call other ones)
            //     // possibly give them an identity, cache them, and then check against the references
            //     
            //     // Some Events Trigger Other Events (for example ending a turn triggers readying cards)
            //     // What about events that call commands? Invert them?
            //     return;
            // }
            Event.Execute();
            Events.Add(Event);
            if (Event is EndTurn)
            {
                TurnCount += 1;
            }
            EmitSignal(nameof(EventRecorded), Event);
            
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

        public Event this[int index] => Events[index];
    }
}