using System.Linq;
using CardGame.Client.Assets.Audio;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game
{
    public class SendCardToGraveyard: Command
    {
        private readonly Card Card;

        public SendCardToGraveyard(Card card)
        {
            Card = card;
        }
        protected override void SetUp(Effects gfx)
        {
            // Use Zone Properties On Cards In Future
            if (Card.Controller.Units.Contains(Card))
            {
                Card.Controller.Units.Remove(Card);
            }
            else if(Card.Controller.Support.Contains(Card))
            {
                Card.Controller.Support.Remove(Card);
            }

            var origin = Card.Translation;
            Card.OwningPlayer.Graveyard.Add(Card);
            // Should probably defined this base on count of cards in graveyard
            var destination = Card.OwningPlayer.Graveyard.View.GlobalTransform.origin + new Vector3(0, 0, 0.1F);
            Card.Translation = origin;
				
            gfx.Play(Audio.Destroyed, 0.4F);
            gfx.InterpolateProperty(Card, nameof(Card.Translation), origin, destination, 0.3F);
        }
    }
}