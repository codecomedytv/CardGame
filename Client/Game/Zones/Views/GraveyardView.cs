using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones
{
    public class GraveyardView: Sprite3D, IZoneView
    {
        
        public int Count => 0;
        public void Add(Card card) { card.Translation = GlobalTransform.origin; }

        public void Remove(Card card) { }
    }
}