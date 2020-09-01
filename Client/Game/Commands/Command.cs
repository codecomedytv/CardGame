using System.Threading.Tasks;
using Godot;

namespace CardGame.Client.Game
{
    public abstract class xCommand
    {
        public xCommand()
        {
            
        }
        
        public async Task<Task> Execute(Tween gfx)
        {
            SetUp(gfx);
            await gfx.ToSignal(gfx, "tween_all_completed");
            return Task.CompletedTask;
        }

        protected virtual void SetUp(Tween gfx)
        {
            // 
        }
    }
}