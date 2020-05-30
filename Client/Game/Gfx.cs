using CardGame.Server;
using Godot;

namespace CardGameSharp.Client.Game
{
    public class Gfx: Tween
    {
        public enum Who
        {
            Global,
            Player,
            Opponent
        };

        public float OpponentDelay = 0.0F;
        public float PlayerDelay = 0.0F;

        public override void _Ready()
        {
            // Not 100% sure this is the correct signal
            Connect("tween_all_completed", this, nameof(Reset));
        }

        public float AddDelay(float delay, int who)
        {
            switch (who)
            {
                case (int) Who.Player:
                    PlayerDelay += delay;
                    break;
                case (int) Who.Opponent:
                    OpponentDelay += delay;
                    break;
            }

            return TotalDelay(who);
        }

        public float TotalDelay(int who)
        {
            return who == (int)Who.Player ? PlayerDelay : OpponentDelay;
        }

        public void Reset()
        {
            OpponentDelay = 0.0F;
            PlayerDelay = 0.0F;
            RemoveAll();
        }
    }
}

