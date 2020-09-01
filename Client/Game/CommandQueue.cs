using System.Collections.Generic;
using Godot;

namespace CardGame.Client.Game
{
    public class CommandQueue
    {
        private readonly Queue<xCommand> Commands = new Queue<xCommand>();
        public Tween Gfx; // Temp
        
        public void Add(xCommand command)
        {
            Commands.Enqueue(command);
        }

        public async void Execute()
        {
            // Investigate Await ForEach?
            while(Commands.Count > 0)
            {
                var cmd = Commands.Dequeue();
                await cmd.Execute(Gfx);
                Gfx.RemoveAll();
            }
        }
    }
}