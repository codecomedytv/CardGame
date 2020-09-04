using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game.Commands
{
    public class Activate: Command
    {
        // Our Cards Are Activated ClientSide so we always know this is going to be Opponent
        private readonly Opponent Opponent;
        private readonly Card Card;
        private readonly TextureRect ActivationView;

        public Activate(Opponent opponent, Card card, TextureRect activationView)
        {
            Opponent = opponent;
            Card = card;
            ActivationView = activationView;
        }
        
        protected override void SetUp(Tween gfx)
        {
            var fakeCard = Opponent.Support[0];
            Opponent.Support.Remove(fakeCard);
            Opponent.Support.Add(Card);
            Card.Translation = fakeCard.Translation;
            fakeCard.Free();

            ActivationView.Texture = Card.Art;
            gfx.InterpolateProperty(Card, nameof(Card.RotationDegrees), new Vector3(0, 0, 0), new Vector3(0, 180, 0),
                0.1F);
            gfx.InterpolateCallback(ActivationView, 0.2F, "set_visible", true);
            gfx.InterpolateProperty(ActivationView, "modulate", Colors.White, ActivationView.Modulate, 0.3F,
                Tween.TransitionType.Linear, Tween.EaseType.In, 0.1F);
            gfx.InterpolateCallback(ActivationView, 1, "set_visible", false);
        }
    }
}