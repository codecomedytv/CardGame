using CardGame.Server;
using Godot;

namespace CardGame.Client.Match
{
    public class Gfx: Tween
    {
        public float Delay = 0.0F;

        public override void _Ready()
        {
            // Not 100% sure this is the correct signal
            Connect("tween_all_completed", this, nameof(Reset));
        }

        public void StartAnimation()
        {
            this.Start();
        }
        

        public float AddDelay(float delay)
        {
            Delay += delay;
            return TotalDelay();
        }

        public float TotalDelay()
        {
            return Delay;
        }

        public void Reset()
        {
            Delay = 0.0F;
            RemoveAll();
        }
    }
}

