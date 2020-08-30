using System.Linq;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game
{
    public class CommandFactory
    {
        public Command Draw(IPlayer player, Card card)
		{
			Tween Command(Tween gfx)
			{
				card.Visible = false;
				player.Deck.Add(card);
				var globalPosition = card.Translation;
				player.Deck.Remove(card);
				player.Hand.Add(card);
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
		
		
	}
}