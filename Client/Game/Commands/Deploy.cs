using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game
{
    public class Deploy: Command
    {
        private readonly Card Card;
        
        public Deploy(Card card)
        {
            Card = card;
        }
        protected override void SetUp(Tween gfx)
        {
            if (Card.Controller is Opponent)
            {
                var fakeCard = Card.Controller.Hand[0];
                Card.Controller.Hand.Remove(fakeCard);
                Card.Controller.Hand.Add(Card);
                fakeCard.Free();
            }

            var origin = Card.Translation;
            var destination = Card.Controller.Units.NextSlot() + new Vector3(0, 0, 0.05F);

            Card.Controller.Hand.Remove(Card);
            Card.Controller.Units.Add(Card);

            gfx.InterpolateProperty(Card, nameof(Card.Translation), origin, destination, 0.3F);
            gfx.InterpolateProperty(Card, nameof(Card.RotationDegrees), new Vector3(-25, 180, 0), new Vector3(0, 180, 0), 0.1F);
            gfx.InterpolateCallback(Card.Controller.Hand, 0.2F, nameof(Hand.Sort));
        }
    }
}
