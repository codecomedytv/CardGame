using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class Activate: xCommand
    {
        // Our Cards Are Activated ClientSide so we always know this is going to be Opponent
        private readonly Opponent Opponent;
        private readonly Card Card;

        public Activate(Opponent opponent, Card card)
        {
            Opponent = opponent;
            Card = card;
        }
        protected override void SetUp(Tween gfx)
        {
            var fakeCard = Opponent.Support[0];
            Opponent.Support.Remove(fakeCard);
            Opponent.Support.Add(Card);
            Card.Translation = fakeCard.Translation;
            fakeCard.Free();

            gfx.InterpolateProperty(Card, nameof(Card.RotationDegrees), new Vector3(0, 0, 0), new Vector3(0, 180, 0),
                0.1F);
        }
    }
}