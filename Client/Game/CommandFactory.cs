﻿using System.Linq;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game
{
    public class CommandFactory: Godot.Object
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
        
        public Command Deploy(IPlayer player, Card card)
        {
	        Tween Command(Tween gfx)
	        {
		        if (player is Opponent)
		        {
			        var fakeCard = player.Hand[0];
			        player.Hand.Remove(fakeCard);
			        player.Hand.Add(card);
			        fakeCard.Free();
		        }

		        var origin = card.Translation;
		        var destination = player.Units.NextSlot() + new Vector3(0, 0, 0.05F);

		        player.Hand.Remove(card);
		        player.Units.Add(card);

		        gfx.InterpolateProperty(card, nameof(card.Translation), origin, destination, 0.3F);
		        gfx.InterpolateProperty(card, nameof(card.RotationDegrees), new Vector3(-25, 180, 0), new Vector3(0, 180, 0), 0.1F);
		        gfx.InterpolateCallback(player.Hand, 0.2F, nameof(Hand.Sort));
		        return gfx;
	        }

	        return Command;
        }
        
        public Command SetFaceDown(IPlayer player, Card card)
        {
	        Tween Command(Tween gfx)
	        {
		        var origin = card.Translation;
		        var destination = player.Support.NextSlot() + new Vector3(0, 0, 0.05F);

		        player.Hand.Remove(card);
		        player.Support.Add(card);

		        gfx.InterpolateProperty(card, nameof(card.Translation), origin, destination, 0.3F);
		        gfx.InterpolateProperty(card, nameof(card.RotationDegrees), new Vector3(-25, 180, 0), new Vector3(0, 0, 0), 0.1F);
		        gfx.InterpolateCallback(player.Hand, 0.2F, nameof(Hand.Sort));
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
        
        public Command Activate(Opponent opponent, Card card)
        {
	        Tween Command(Tween gfx)
	        {
		        var fakeCard = opponent.Support[0];
		        opponent.Support.Remove(fakeCard);
		        opponent.Support.Add(card);
		        card.Translation = fakeCard.Translation;
		        fakeCard.Free();

		        gfx.InterpolateProperty(card, nameof(card.RotationDegrees), new Vector3(0, 0, 0), new Vector3(0, 180, 0),
			        0.1F);

		        return gfx;
	        }

	        return Command;
        }
        
        public Command SendCardToGraveyard(IPlayer player, Card card)
        {
	        Tween Command(Tween gfx)
	        {
		        // Use Zone Properties On Cards In Future
		        if (player.Units.Contains(card))
		        {
			        player.Units.Remove(card);
		        }
		        else if(player.Support.Contains(card))
		        {
			        player.Support.Remove(card);
		        }
				
		        player.Graveyard.Add(card);
				
		        var origin = card.Translation;
		        var destination = player.Graveyard.GlobalTransform.origin + new Vector3(0, 0, 0.1F);

		        gfx.InterpolateProperty(card, nameof(card.Translation), origin, destination, 0.3F);
		        return gfx;
	        }
			
	        return Command;
        }
        
        public Command Battle(IPlayer player, Card attacker, Card defender)
        {
	        Tween Command(Tween gfx)
	        {
		       
		        var attackerY = player is Opponent ? 1.75F : 0.5F;
		        var defenderY = player is Opponent ? 1.75F : 0.5F;
		        var attackerDestination = new Vector3(2.5F, attackerY, attacker.Translation.z);
		        var defenderDestination = new Vector3(2.5F, defenderY, defender.Translation.z);

		        gfx.InterpolateProperty(attacker, nameof(Translation), attacker.Translation, attackerDestination, 0.1F);
		        gfx.InterpolateProperty(defender, nameof(Translation), defender.Translation, defenderDestination, 0.1F);
		        // Insert Sound
		        gfx.InterpolateProperty(attacker, nameof(Translation), attackerDestination, attacker.Translation, 0.1F, 
			        Tween.TransitionType.Linear, Tween.EaseType.In, 0.3F);
		        gfx.InterpolateProperty(defender, nameof(Translation), defenderDestination, defender.Translation, 0.1F,
			        Tween.TransitionType.Linear, Tween.EaseType.In, 0.3F);

		        gfx.InterpolateCallback(this, 0.4F, nameof(ClearBattle), attacker, defender);
				
		        return gfx;
	        }

	        return Command;
        }
        
        private void ClearBattle(Card attacker, Card defender)
        {
	        attacker.AttackingIcon.Visible = false;
	        defender.DefendingIcon.Visible = false;
        }
    }
}
