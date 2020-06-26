using System.Collections.Generic;
using System.Net.Sockets;
using CardGame.Server.Game.Cards;
using Godot;

namespace CardGame.Server.Game.Tags
{
    public enum TagIds
    {
        CannotBeAttacked,
        CannotBeDestroyedByBattle,
        CannotBeDestroyedByEffect,
        CannotTakeBattleDamage,
        CannotTakeEffectDamage,
    }

    public class Tag
    {
        public readonly TagIds TagId;
        private readonly List<Card> Tagged = new List<Card>();
        private readonly List<Player> TaggedPlayers = new List<Player>();

        public Tag(TagIds tagId)
        {
            TagId = tagId;
        }

        public void Add(Card card)
        {
            if (card.Tags.Contains(this))
            {
                return;
            }
            Tagged.Add(card);
            card.Tags.Add(this);
        }
        
        public void Add(Player player)
        {
            if (player.Tags.Contains(this))
            {
                return;
            }
            TaggedPlayers.Add(player);
            player.Tags.Add(this);
        }

        public void Remove(Card card)
        {
            Tagged.Remove(card);
            card.Tags.Remove(this);
        }

        public void Remove(Player player)
        {
            TaggedPlayers.Remove(player);
            player.Tags.Remove(this);
        }

        public void UnTagAll()
        {
            foreach (var card in Tagged)
            {
                card.Tags.Remove(this);
            }

            foreach (var player in TaggedPlayers)
            {
                player.Tags.Remove(this);
            }
            Tagged.Clear();
            TaggedPlayers.Clear();
        }
        
    }

}