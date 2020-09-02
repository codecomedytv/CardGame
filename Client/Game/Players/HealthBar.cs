using Godot;

namespace CardGame.Client.Game.Players
{
    public class HealthBar : Sprite
    {
        private TextureProgress Bar;
        private Label Count;
        
        // Change Requires Some Work
        // We can't use the interpolated value in onhealthchanged because it'll be incorrect
        private Label Change;
        public int Value = 8000;
        
        public override void _Ready()
        {
            Bar = GetNode<TextureProgress>("Bar");
            Count = GetNode<Label>("Count");
            Change = GetNode<Label>("Change");
        }

        public void OnHealthChanged(int lifeLost)
        {
            // We're assuming lost for now but in future gain may also be a thing
            Value -= lifeLost;
            Count.Text = Value < 0 ? 0.ToString() : Value.ToString();
            Bar.Value = Value < 0 ? 0 : Value;
        }
    }
}
