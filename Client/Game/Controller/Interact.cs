using CardGame.Client.Library.Card;
using Godot;

namespace CardGame.Client.Match
{
    public class Interact: AnimatedSprite
    {
        public Card Card;

        public void Add(Card card)
        {
            Input.SetMouseMode(Input.MouseMode.Hidden);
            Card = card;
            Visible = true;
        }

        public new void Stop()
        {
            Visible = false;
            Input.SetMouseMode(Input.MouseMode.Visible);
            Card.LegalPlay.Stop();
            Card.LegalPlay.Hide();
            Card = null;
        }

        public override void _Process(float delta)
        {
            Position = GetGlobalMousePosition();
            if (Card == null || Card.LegalPlay.Visible) return;
            Card.LegalPlay.Play();
            Card.LegalPlay.Show();
        }
    }
}
