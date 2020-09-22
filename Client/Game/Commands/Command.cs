using System.Threading.Tasks;
using Godot;

namespace CardGame.Client.Game
{
    public abstract class Command
    {
        public Command() { }
        
        public async Task<Task> Execute(Effects gfx)
        {
            gfx.RemoveAll();
            SetUp(gfx);
            await gfx.Start();
            return Task.CompletedTask;
        }

        protected abstract void SetUp(Effects gfx);

        protected void AddProxyDelay(Effects gfx)
        {
            // Used for Commands that are casted immediately
        }
    }
}