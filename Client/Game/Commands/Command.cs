using System.Threading.Tasks;
using Godot;

namespace CardGame.Client.Game
{
    public abstract class Command
    {
        public Command() { }
        
        public async Task<Task> Execute(Tween gfx)
        {
            SetUp(gfx);
            gfx.Start();
            await gfx.ToSignal(gfx, "tween_all_completed");
            return Task.CompletedTask;
        }

        protected abstract void SetUp(Tween gfx);
    }
}