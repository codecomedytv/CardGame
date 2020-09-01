using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

namespace CardGame.Client.Game
{
    public delegate Tween Command(Tween gfx);

    public class CommandQueue //: //Godot.Object
    {
        private readonly Queue<object> Commands = new Queue<object>();
        public Tween Gfx; // Temp
        
        public void Add(object command)
        {
            if (command is Command || command is xCommand)
            {
                Commands.Enqueue(command);
            }
        }

        // Setting State Should be a Queued Action In Future
        public async Task Execute()
        {
            // Investigate Await ForEach?
            while(Commands.Count > 0)
            {
                var cmd = Commands.Dequeue();
                if (cmd is Command command)
                {
                    var executor = command(Gfx);
                    executor.Start();
                    await Gfx.ToSignal(executor, "tween_all_completed"); // hate it
                    executor.RemoveAll();
                }
                else if (cmd is xCommand xCommand)
                {
                    await xCommand.Execute(Gfx);
                    Gfx.RemoveAll();
                }
            }
        }
    }
}