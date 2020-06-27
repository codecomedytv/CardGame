using System.Collections.Generic;
using CardGame.Server.Game.Events;
using Godot;

namespace CardGame.Server.Game
{
    public class History: Godot.Object
    {
        [Signal]
        public delegate void EventRecorded();
        private int Cursor = 0;
        private int TurnCount = 0;
        private readonly List<Event> gameEvents = new List<Event>();

        public void Add(Event gameEvent)
        {
            gameEvents.Add(gameEvent);
            if (gameEvent is EndTurn)
            {
                TurnCount += 1;
            }
            EmitSignal(nameof(EventRecorded), gameEvent);
            
        }

        public void Redo()
        {
            Cursor += 1;
            var gameEvent = gameEvents[Cursor];
        }

        public void Undo()
        {
            Cursor -= 1;
            var gameEvent = gameEvents[Cursor];
        }

        public Event this[int index] => gameEvents[index];
    }
}