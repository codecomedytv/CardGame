using System.Linq;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game
{
    public class CommandFactory: Godot.Object
    {
        public Command Draw(Card card)
		{
			Tween Command(Tween gfx)
			{
				card.Visible = false;
				card.Controller.Deck.Add(card);
				var globalPosition = card.Translation;
				card.Controller.Deck.Remove(card);
				card.Controller.Hand.Add(card);
				var globalDestination = card.Translation;
				var rotation = new Vector3(-25, 180, 0);
				
				// Wrap In gfx Class
				gfx.InterpolateProperty(card, nameof(card.Visible), false, true, 0.1F);
				gfx.InterpolateProperty(card, nameof(card.Translation), globalPosition, globalDestination, 0.1F);
				gfx.InterpolateProperty(card, nameof(card.RotationDegrees), card.Rotation, rotation, 0.1F);
				return gfx;
			}
			return Command;
		}
        
        public Command Draw(IPlayer opponent)
        {
	        Tween Command(Tween gfx)
	        {
		        var card = opponent.Deck.ElementAt(opponent.Deck.Count - 1);
		        opponent.Deck.Add(card);
		        var globalPosition = card.Translation;
		        opponent.Deck.Remove(card);
		        opponent.Hand.Add(card);
		        var globalDestination = card.Translation;
		        var rotation = new Vector3(60, 0, 0);
				
		        // Wrap In gfx Class
		        gfx.InterpolateProperty(card, nameof(card.Visible), false, true, 0.1F);
		        gfx.InterpolateProperty(card, nameof(card.Translation), globalPosition, globalDestination, 0.1F);
		        gfx.InterpolateProperty(card, nameof(card.RotationDegrees), card.Rotation, rotation, 0.1F);
		        return gfx;
	        };

	        return Command;
        }
        
        public Command SetFaceDown(Card card)
        {
	        Tween Command(Tween gfx)
	        {
		        var origin = card.Translation;
		        var destination = card.Controller.Support.NextSlot() + new Vector3(0, 0, 0.05F);

		        card.Controller.Hand.Remove(card);
		        card.Controller.Support.Add(card);

		        gfx.InterpolateProperty(card, nameof(card.Translation), origin, destination, 0.3F);
		        gfx.InterpolateProperty(card, nameof(card.RotationDegrees), new Vector3(-25, 180, 0), new Vector3(0, 0, 0), 0.1F);
		        gfx.InterpolateCallback(card.Controller.Hand, 0.2F, nameof(Hand.Sort));
		        return gfx;
	        }
	        return Command;
        }
        
        public Command SetFaceDown(Opponent opponent)
        {
	        Tween Command(Tween gfx)
	        {
		        var card = opponent.Hand[0];
		        var origin = card.Translation;
		        var destination = opponent.Support.NextSlot() + new Vector3(0, 0, 0.05F);

		        opponent.Hand.Remove(card);
		        opponent.Support.Add(card);

		        gfx.InterpolateProperty(card, nameof(card.Translation), origin, destination, 0.3F);
		        gfx.InterpolateProperty(card, nameof(card.RotationDegrees), new Vector3(-25, 0, 0), new Vector3(0, 0, 0),
			        0.1F);
		        gfx.InterpolateCallback(opponent.Hand, 0.2F, nameof(Hand.Sort));
		        return gfx;
	        }

	        return Command;
        }
    }
}
