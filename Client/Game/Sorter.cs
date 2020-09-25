using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones
{
    public class Sorter : Godot.Object
    {
        private readonly IEnumerable<Card> Cards;
            
        public Sorter(IEnumerable<Card> cards)
        {
            Cards = cards;
        }

        public void Sort()
        {
            // This was mainly designed for our hand
            const float modifier = 1.2F;
            var lastCardOnTheLeft = Cards.ToList().Count / 2;
            var initial = -lastCardOnTheLeft * modifier + 0.6F;
            foreach (var card in Cards)
            {
                card.Translation = new Vector3(initial + 2.5F, card.Translation.y, card.Translation.z);
                initial += modifier;
            }
            
            CallDeferred("free");
        }
            
            
    }
}