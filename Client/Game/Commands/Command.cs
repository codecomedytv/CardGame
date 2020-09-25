using System;
using System.Threading.Tasks;
using Godot;

namespace CardGame.Client.Game
{
    public abstract class Command
    {
        protected Command() { }
        
        public SignalAwaiter Execute(Effects gfx)
        {
            gfx.RemoveAll();
            SetUp(gfx);
            var awaiter = gfx.Start();
            return awaiter;
        }

        protected abstract void SetUp(Effects gfx);
    }
}