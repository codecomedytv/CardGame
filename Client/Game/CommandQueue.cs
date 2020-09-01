using System.Collections.Generic;
using Godot;

namespace CardGame.Client.Game
{
    public class CommandQueue
    {
        private readonly Queue<Command> Commands = new Queue<Command>();
        public Tween Gfx; // Temp
        
        public void Add(Command command) { Commands.Enqueue(command); }

        public async void Execute()
        {
            while(Commands.Count > 0)
            {
                var cmd = Commands.Dequeue();
                await cmd.Execute(Gfx);
            }
        }
    }
}