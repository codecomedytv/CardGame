using System.Linq;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game
{
    public class CommandFactory: Godot.Object
    {
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
