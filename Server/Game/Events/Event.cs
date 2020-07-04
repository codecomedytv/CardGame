using System;
using CardGame.Server.Game.Network;
using Godot;

namespace CardGame.Server.Game.Events
{
    public abstract class Event: Godot.Object
    {
        public GameEvents Identity { get; protected set; }

        protected Event()
        {
            
        }

        public virtual void SendMessage(Message message)
        {
        }

    }
    
}