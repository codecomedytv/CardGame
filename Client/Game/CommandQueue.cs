using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

namespace CardGame.Client.Game
{
    public delegate Tween Command(Tween gfx);

    public class CommandQueue //: //Godot.Object
    {
        private readonly Queue<Command> Commands = new Queue<Command>();
        public Tween Gfx; // Temp
        
        public void Add(Command command)
        {
            Commands.Enqueue(command);
        }

        // Setting State Should be a Queued Action In Future
        public async Task Execute()
        {
            // Investigate Await ForEach?
            while(Commands.Count > 0)
            {
                var command = Commands.Dequeue();
                var executor = command(Gfx);
                executor.Start();
                await Gfx.ToSignal(executor, "tween_all_completed"); // hate it
                executor.RemoveAll();
            }
        }
    }
}