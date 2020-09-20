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
            if (Card.Controller.UnitsModel.Contains(Card))
            {
                Card.Controller.UnitsModel.Remove(Card);
            }
            else if(Card.Controller.SupportModel.Contains(Card))
            {
                Card.Controller.SupportModel.Remove(Card);
            }
				
            Card.OwningPlayer.Graveyard.Add(Card);
				
            gfx.Play(Audio.Destroyed, 0.4F);
            var origin = Card.Translation;
            var destination = Card.OwningPlayer.Graveyard.GlobalTransform.origin + new Vector3(0, 0, 0.1F);
            gfx.InterpolateProperty(Card, nameof(Card.Translation), origin, destination, 0.3F);
        }
    }
}